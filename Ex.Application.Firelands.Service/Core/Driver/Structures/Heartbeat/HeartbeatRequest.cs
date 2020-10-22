namespace Ex.Application.Firelands.Service.Core.Driver.Structures.Heartbeat
{
    public class HeartbeatRequest : BaseRequest
    {
        public int GameProcess;
        public int ServiceProcess;
        public bool EnableSecurity;

        public override bool Valid()
        {
            return base.Valid()
                && GameProcess != 0
                && ServiceProcess != 0;
        }
    }
}
