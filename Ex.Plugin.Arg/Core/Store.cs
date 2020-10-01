using System.Collections.Generic;

namespace Ex.Plugin.Arg.Core
{
    internal class Store : ExObject
    {
        private List<Arg> m_Args;

        public void Initialize()
        {
            m_Args = new List<Arg>();
        }

        public void Update( List<Arg> args )
            => m_Args = args;

        public void Push( Arg arg )
            => m_Args.Add( arg );

        public void Push( string raw )
            => Push( Arg.Parse( raw, Count() ) );

        public void Remove( Arg arg )
            => m_Args.Remove( arg );

        public void Remove( int index )
            => Remove( Get( index ) );

        public Arg Get( int index )
        {
            if ( IsValidIndex( index ) )
            {
                return m_Args[ index ];
            }

            return null;
        }

        public void Clear()
            => m_Args.Clear();

        public List<Arg> All()
            => m_Args;

        public int Count()
            => m_Args.Count;

        public bool IsValidIndex( int index )
            => m_Args.Count > index;
    }
}
