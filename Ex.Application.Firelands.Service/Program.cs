using System.ServiceProcess;

namespace Ex.Application.Firelands.Service
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new UpdaterAgentService()
            };
            ServiceBase.Run( ServicesToRun );
        }
    }
}
