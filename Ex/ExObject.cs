using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Ex
{
    public class ExObject
    {
        private ConcurrentBag<ExBehaviour> m_Behaviours { get; set; } = new ConcurrentBag<ExBehaviour>();
        private string m_Tag { get; set; }

        public ExObject()
        {
            SetTag( GetType().FullName + "__" + Guid.NewGuid().ToString() );
        }
        public ExObject( string tag )
        {
            SetTag( tag );
        }

        public static T Instantiate<T>() where T : ExObject, new()
            => ExObjectManager.Instantiate<T>();

        public static T FindObjectOfTag<T>( string tag ) where T : ExObject
            => ExObjectManager.GetObjectByTag<T>( tag );

        public static T FindObjectOfType<T>() where T : ExObject
            => ExObjectManager.GetObjectByType<T>();

        public static T Unbox<T>( ExObject obj ) where T : ExObject
            => obj.Unbox<T>();
        public static ExObject Box( ExObject obj )
            => obj.Box();

        public T AddComponent<T>() where T : ExBehaviour, new()
        {
            var obj = new T();
            AddComponentByTypeInstance_Internal( obj );
            return obj;
        }
        public T GetComponent<T>() where T : ExBehaviour
            => ( T )m_Behaviours.FirstOrDefault( ( x ) => x.GetType() == typeof( T ) );
        public T[] GetAllComponents<T>() where T : ExBehaviour
        {
            List<T> premade = new List<T>();
            var components = GetInternalBehaviours().ToList();

            for ( var i = 0; i < components.Count; i++ )
            {
                if ( components[ i ].GetType() == typeof( T ) )
                    premade.Add( ( T )components[ i ] );
            }

            return premade.Count > 0 ? premade.ToArray() : null;
        }
        public void RemoveComponent<T>() where T : ExBehaviour
        {
            var obj = GetComponent<T>();
            if ( obj != null )
            {
                obj.BeforeDestroy();
                obj.Destroy();

                m_Behaviours = new ConcurrentBag<ExBehaviour>( m_Behaviours.Except( new[] { obj } ) );
            }
        }

        public string GetTag()
            => m_Tag;
        public T Unbox<T>() where T : ExObject
            => ( T )this;
        public ExObject Box()
            => this;

        public virtual void Destroy()
        {
            throw new NotImplementedException( $"Method 'Destroy' must be implemented in '{GetTag()}' before destroying!" );
        }

        private void AddComponentByTypeInstance_Internal( ExBehaviour instance )
        {
            instance.ParentObject = this;
            instance.Awake();

            m_Behaviours.Add( instance );
        }
        public ConcurrentBag<ExBehaviour> GetInternalBehaviours()
            => m_Behaviours;
        private void SetTag( string tag )
            => m_Tag = tag;
    }
}
