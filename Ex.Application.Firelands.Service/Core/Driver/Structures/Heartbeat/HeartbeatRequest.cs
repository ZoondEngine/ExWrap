using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex.Application.Firelands.Service.Core.Driver.Structures.Heartbeat
{
    public class HeartbeatRequest : BaseRequest
    {
        public int WowProcess;
        public int ServiceProcess;

        public override bool Valid()
        {
            return base.Valid()
                && WowProcess != 0
                && ServiceProcess != 0;
        }
    }
}
