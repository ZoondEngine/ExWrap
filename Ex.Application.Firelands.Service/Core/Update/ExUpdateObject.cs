using Ex.Application.Firelands.Service.Core.Driver;
using Ex.Application.Firelands.Service.Core.Update.Behaviours;
using Ex.Application.Firelands.Service.Core.Update.Behaviours.AccessorData;
using Ex.Attributes;
using Ex.Crypto;

using System.Collections.Generic;
using System.Linq;

namespace Ex.Application.Firelands.Service.Core.Update
{
    public enum Updatable
    {
        Driver,
        Launcher,
        Service
    }

    [RequiredBehaviour(typeof(UpdateLocalStoreBehaviour))]
    [RequiredBehaviour(typeof(UpdateAccessorBehaviour))]
    public class ExUpdateObject : ExObject
    {
        private string m_Error;

        public ExUpdateObject()
            : base()
        { }

        public ExUpdateObject(string tag)
            : base(tag)
        { }

        private void SetError( string what )
            => m_Error = what;
        public string Error()
            => m_Error;

        public bool AvailableDriver()
            => Available( Updatable.Driver );
        public bool AvailableLauncher()
            => Available( Updatable.Launcher );
        public bool AvailableService()
            => Available( Updatable.Service );

        private bool Available(Updatable what)
        {
            var localVersion = Store().Get( what );
            if(!localVersion.Zero())
            {
                var remoteVersion = Accessor().QueryVersion( what );
                if(!remoteVersion.Zero())
                {
                    return localVersion >= remoteVersion;
                }
                else
                {
                    SetError( $"Can't read remote version of '{what}'" );
                }
            }
            else
            {
                SetError( $"Can't read local version of '{what}'" );
            }

            return false;
        }

        public bool UpdateDriver()
        {
            var driver = FindObjectOfType<ExDriverObject>();
            if(driver != null)
            {
                if(driver.Unload())
                {
                    if(Update("C:\\Windows\\System32\\firelands_ac.sys", "FirelandsAC", Updatable.Driver))
                    {
                        if(driver.Load())
                        {
                            return true;
                        }
                        else
                        {
                            SetError( $"Can't load anti cheat driver!" );
                        }
                    }
                    else
                    {
                        SetError( $"Can't update driver from remote servers!" );
                    }
                }
                else
                {
                    SetError( $"Can't unload driver from local machine!" );
                }
            }
            else
            {
                SetError( $"ExDriverObject can be a null!" );
            }

            return false;
        }

        public bool UpdateLauncher(string path)
            => Update( path, "Firelands", Updatable.Launcher );

        public bool UpdateService()
        {
            // TODO: need to make service update
            // i think need to download some installer service(simple)
            // he must be a check existing of a current service and disable it if runned
            // then must be an install that via command line
            return true;
        }

        public bool AvailableAll()
            => AvailableDriver() && AvailableLauncher() && AvailableService();
        public bool UpdateAll()
            => UpdateDriver() && UpdateLauncher( Store().GetLauncherPath() ) && UpdateService();

        public bool Check()
        {
            if ( !AvailableAll() )
                return UpdateAll();

            return true;
        }

        private bool Update(string path, string process, Updatable what)
        {
            var manifest = Accessor().QueryManifest( what );
            if(manifest != null)
            {
                var manifestLines = manifest.Split( '\r', '\n' );
                manifestLines = manifestLines.Where( ( x ) => x != "" ).ToArray();

                List<ManifestItem> items = new List<ManifestItem>();

                foreach(var line in manifestLines)
                {
                    items.Add( ManifestItem.Parse( line ) );
                }

                return new SoftwareUpdater().Build( path, process, items, Instantiate<HashObject>() ).Execute();
            }

            return true;
        }

        public UpdateLocalStoreBehaviour Store()
            => GetComponent<UpdateLocalStoreBehaviour>();
        public UpdateAccessorBehaviour Accessor()
            => GetComponent<UpdateAccessorBehaviour>();
    }
}
