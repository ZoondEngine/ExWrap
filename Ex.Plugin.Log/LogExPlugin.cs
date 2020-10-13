using Ex.Exceptions;
using Ex.Plug;
using Ex.Plugin.Log.Behaviours;

namespace Ex.Plugin.Log
{
    public class LogExPlugin : IExPlugin, ILoggableObject
    {
        private ExLogObject m_Log;

        public T As<T>() where T : IExPlugin
        {
            if ( typeof( T ) != GetType() )
                return default;

            return ( T )( object )this;
        }

        public string GetIdentifier()
            => "Ex.Plugin.Log";

        public string GetName()
            => "ExLogger";

        public string GetVersion()
            => "1.0.0.0";

        public void OnLoad()
        {
            m_Log = ExObject.Instantiate<ExLogObject>();
            m_Log.Writer().Trace( "Logging plugin successfully loaded" );
        }

        public void OnUnload()
        {
            if(m_Log != null)
            {
                m_Log.Writer().BeforeDestroy();
                m_Log.Writer().TraceImmediate("Logging plugin unloading");
                m_Log.Writer().Destroy();
            }
        }

        public LogWriteBehaviour Writer()
        {
            if(m_Log != null)
            {
                return m_Log.Writer();
            }

            throw new ExException( "Can't get writer before installing plugin!" );
        }
    }
}
