using Ex.Application.Firelands.Service.Core.Access;
using Ex.Application.Firelands.Service.Core.Driver;
using Ex.Application.Firelands.Service.Core.Update;

namespace Ex.Application.Firelands.Service
{
    public static class Entry
    {
        public static void Start()
        {
            var driverObject = ExObject.Instantiate<ExDriverObject>();
            var accessObject = ExObject.Instantiate<ExAccessObject>();
            var updateObject = ExObject.Instantiate<ExUpdateObject>();

            if ( !updateObject.AvailableAll() )
                updateObject.UpdateAll();

            if ( !driverObject.Load() )
            {
                driverObject.Log().ErrorImmediate( $"Service not work! Can't load AC driver!" );
                Stop();
            }

            driverObject.EnableHeartbeat();

            // TODO: start access object to received and handle external commands
            // ofc with signature checking too
        }

        public static void Stop()
        {
            var driver = ExObject.FindObjectOfType<ExDriverObject>();
            driver.DisableHeartbeat();
            if ( !driver.Unload() )
            {
                driver.Log().ErrorImmediate( $"Can't unload AC driver!" );
            }
        }
    }
}
