using Ex.Exceptions;

using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Ex
{
    internal static class ExObjectManager
    {
        const int STACK_SIZE = 64 * 1024 * 1024;

        private static ConcurrentDictionary<int, ExObject> m_ObjectsList;
        private static Thread m_ListIteratorThread;
        private static TimeSpan m_PreviousTick;
        private static int m_ComparisonIndex;
        public static bool IsWork { get; private set; }

        public static void Instance( int stackSize = STACK_SIZE )
        {
            m_ComparisonIndex = 0;
            m_ObjectsList = new ConcurrentDictionary<int, ExObject>();
            m_ListIteratorThread = new Thread( Iterate, stackSize )
            {
                IsBackground = true
            };
            m_ListIteratorThread.Start();

            IsWork = true;
        }
        public static void Destroy()
        {
            try
            {
                m_ListIteratorThread.Abort();
            }
            catch
            { } // ignore it
        }

        public static T Instantiate<T>( T obj ) where T : ExObject, new()
        {
            if ( !IsWork )
            {
                Instance();
            }

            ExRequiredAttributeController.Inject( obj );

            if ( !m_ObjectsList.TryAdd( m_ComparisonIndex, obj ) )
            {
#if DEBUG
                throw new GOMConcurrentException( $"{MethodBase.GetCurrentMethod().Name}: Can't instantiate object '{typeof( T ).FullName}'" );
#endif
            }

            m_ComparisonIndex++;
            return obj;
        }
        public static T Instantiate<T>() where T : ExObject, new()
        {
            if ( !IsWork )
            {
                Instance();
            }

            return Instantiate( new T() );
        }

        public static void Destroy<T>( string tag = null ) where T : ExObject
        {
            try
            {
                int idx = -1;
                if ( tag == null )
                {
                    idx = GetInternalIndexOfObjectByType<T>();
                }
                else
                {
                    idx = GetInternalIndexOfObjectByTag<T>( tag );
                }

                if ( m_ObjectsList.TryRemove( idx, out var obj ) )
                {
                    obj.Destroy();
                }
                else
                {
#if DEBUG
                    throw new GOMConcurrentException( $"{MethodBase.GetCurrentMethod().Name}: Can't destroy object '{typeof( T ).FullName}'." );
#endif
                }
            }
            catch ( Exception e )
            {
#if DEBUG
                throw new GOMConcurrentException( $"{MethodBase.GetCurrentMethod().Name}: Can't destroy object '{typeof( T ).FullName}'. {e.Message}" );
#endif
            }
        }
        public static T GetObjectByTag<T>( string tag ) where T : ExObject
            => ( T )m_ObjectsList.FirstOrDefault( ( x ) => x.Value.GetTag().ToLower() == tag.ToLower() & x.Value.GetType() == typeof( T ) ).Value;

        public static T GetObjectByType<T>() where T : ExObject
            => GetObjectByTypeInstance<T>( typeof( T ) );
        public static T GetObjectByTypeInstance<T>( Type type ) where T : ExObject
            => ( T )m_ObjectsList.FirstOrDefault( ( x ) => x.Value.GetType() == type ).Value;

        private static int GetInternalIndexOfObjectByType<T>()
            => m_ObjectsList.FirstOrDefault( ( x ) => x.Value.GetType() == typeof( T ) ).Key;
        private static int GetInternalIndexOfObjectByTag<T>( string tag ) where T : ExObject
            => m_ObjectsList.FirstOrDefault( ( x ) => x.Value.GetTag().ToLower() == tag.ToLower() & x.Value.GetType() == typeof( T ) ).Key;

        private static void Iterate()
        {
            lock ( m_ObjectsList )
            {
                Thread.Sleep( 100 );
                Thread.BeginCriticalRegion();

                var currentTickSpan = TimeSpan.FromTicks( DateTime.Now.Ticks );
                if ( m_PreviousTick == null )
                {
                    m_PreviousTick = currentTickSpan;
                }

                for ( var idx = 0; idx < m_ObjectsList.Count; idx++ )
                {
                    var currentObject = m_ObjectsList[ idx ];
                    var behaviours = currentObject.GetInternalBehaviours();
                    if ( behaviours != null )
                    {
                        foreach ( var item in behaviours )
                        {
                            item.CurrentTickTime = currentTickSpan;
                            item.PreviousTickTime = m_PreviousTick;
                            item.Update();
                        }
                    }
                    else
                    {
#if DEBUG
                        throw new GOMIterationException( $"{MethodBase.GetCurrentMethod().Name}: 'GetInternalBehaviours' returns null from '{currentObject.GetTag()}'" );
#endif
                    }
                }

                Thread.EndCriticalRegion();
            }

            Iterate();
        }
    }
}
