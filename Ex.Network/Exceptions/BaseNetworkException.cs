using System;
using System.Diagnostics;

namespace Ex.Network.Exceptions
{
    public class BaseNetworkException : Exception
    {
        private readonly StackTrace m_StackTrace;
        public BaseNetworkException( string message )
            : base( message )
        {
            m_StackTrace = new StackTrace( this, true );
        }

        public int ExceptionLine()
        {
            var frame = m_StackTrace.GetFrame( 0 );
            if ( frame != null )
            {
                return frame.GetFileLineNumber();
            }

            return 0;
        }
        public StackTrace GetStackTrace()
            => m_StackTrace;
        public bool IsBaseOf<T>() where T : BaseNetworkException
            => this is T;
    }
}
