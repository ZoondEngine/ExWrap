using Ex.Application.Firelands.Service.Core.Driver.Behaviours;
using Ex.Application.Firelands.Service.Core.Driver.Structures;
using Ex.Attributes;
using Ex.Plugin.Log.Behaviours;

namespace Ex.Application.Firelands.Service.Core.Driver
{
    public delegate void HeartbeatFail();

    [RequiredBehaviour(typeof(LogWriteBehaviour))]
    [RequiredBehaviour(typeof(DriverHeartbeatBehaviour))]
    [RequiredBehaviour(typeof(DriverCommunicateBehaviour))]
    [RequiredBehaviour(typeof(DriverLoadingBehaviour))]
    public class ExDriverObject : ExObject
    {
        private event HeartbeatFail OnHeartbitFail;

        public ExDriverObject()
            : base()
        {
            Log().Init( "service", "c:\\fl\\logs" );
        }

        public ExDriverObject(string tag)
            : base(tag)
        {
            Log().Init( "service", "c:\\fl\\logs" );
        }

        public bool Load()
            => Loader().Load("C:\\Windows\\System32\\firelands_ac.sys", "FirelandsAC", "FirelandsAC", "\\\\.\\FirelandsAC" );
        public bool Unload()
            => Loader().Unload();

        public void Subscribe( HeartbeatFail fail )
            => OnHeartbitFail += fail;
        public void Unsubscribe( HeartbeatFail fail )
            => OnHeartbitFail -= fail;

        public void Call()
            => OnHeartbitFail?.Invoke();

        public void EnableHeartbeat()
            => GetComponent<DriverHeartbeatBehaviour>().Switch( true );
        public void DisableHeartbeat()
            => GetComponent<DriverHeartbeatBehaviour>().Switch( false );

        public bool Request<TIn, TOut>( uint code, TIn request, TOut response ) 
            where TIn : BaseRequest 
            where TOut : BaseResponse
        {
            return Communicate().Send( code, request, response );
        }

        public LogWriteBehaviour Log()
            => GetComponent<LogWriteBehaviour>();
        public DriverCommunicateBehaviour Communicate()
            => GetComponent<DriverCommunicateBehaviour>();
        public DriverLoadingBehaviour Loader()
            => GetComponent<DriverLoadingBehaviour>();
    }
}
