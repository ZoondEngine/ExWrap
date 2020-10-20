using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Ex.Application.Firelands.Service
{
    public partial class UpdaterAgentService : ServiceBase
    {
        public UpdaterAgentService()
        {
            InitializeComponent();

            AutoLog = true;
            CanStop = true;
            CanPauseAndContinue = false;
        }

        protected override void OnStart( string[] args )
        {
        }

        protected override void OnStop()
        {

        }
    }
}
