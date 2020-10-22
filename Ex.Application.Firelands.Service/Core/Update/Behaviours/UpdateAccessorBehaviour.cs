using Newtonsoft.Json;

using System;
using System.Net;

namespace Ex.Application.Firelands.Service.Core.Update.Behaviours
{
    public enum RequestAction
    {
        Version,
        Manifest,
        Include
    }

    public class ServiceResponse<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T Content { get; set; }
    }

    public class UpdateAccessorBehaviour : ExBehaviour
    {
        private WebClient m_WebClient;

        public override void Awake()
        {
            m_WebClient = new WebClient();
        }

        public Version QueryVersion( Updatable what )
            => VersionRequest( ApplyBeta( Uri( what, RequestAction.Version ) ) );
        public string QueryManifest( Updatable what )
             => ManifestRequest( ApplyBeta( Uri( what, RequestAction.Manifest ) ) );
        public bool QueryManifest( Updatable what, out string manifest )
            => ManifestRequest( ApplyBeta( Uri( what, RequestAction.Manifest ) ), out manifest );
        public string QueryInclude(Updatable what)
            => IncludeRequest( ApplyBeta( Uri( what, RequestAction.Include ) ) );
        public bool QueryInclude( Updatable what, out int files, out long size )
            => IncludeRequest( ApplyBeta( Uri( what, RequestAction.Include ) ), out files, out size );

        public Version DriverVersion()
            => QueryVersion( Updatable.Driver );
        public string DriverManifest()
            => QueryManifest( Updatable.Driver );
        public bool DriverManifest( out string manifest )
            => QueryManifest( Updatable.Driver, out manifest );
        public string DriverInclude()
            => QueryInclude( Updatable.Driver );
        public bool DriverInclude( out int files, out long size )
            => QueryInclude( Updatable.Driver, out files, out size );

        public Version LauncherVersion()
            => QueryVersion( Updatable.Launcher );
        public string LauncherManifest()
            => QueryManifest( Updatable.Launcher );
        public bool LauncherManifest( out string manifest )
            => QueryManifest( Updatable.Launcher, out manifest );
        public string LauncherInclude()
            => QueryInclude( Updatable.Launcher );
        public bool LauncherInclude( out int files, out long size )
            => QueryInclude( Updatable.Launcher, out files, out size );

        public Version ServiceVersion()
            => QueryVersion( Updatable.Service );
        public string ServiceManifest()
            => QueryManifest( Updatable.Service );
        public bool ServiceManifest( out string manifest )
            => QueryManifest( Updatable.Service, out manifest );
        public string ServiceInclude()
            => QueryInclude( Updatable.Service );
        public bool ServiceInclude( out int files, out long size )
            => QueryInclude( Updatable.Service, out files, out size );

        private Version VersionRequest(string uri)
        {
            if(Version.TryParse(Request<string>(uri).Content, out var ver))
            {
                return ver;
            }

            return Version.Parse( "0.0.0.0" );
        }
        private string ManifestRequest( string uri )
            => Request<string>( uri ).Content;
        private bool ManifestRequest( string uri, out string manifest )
        {
            var response = ManifestRequest( uri );
            if(response != null)
            {
                manifest = response;
                return true;
            }

            manifest = "";
            return false;
        }
        private string IncludeRequest( string uri )
            => Request<string>( uri ).Content;
        private bool IncludeRequest( string uri, out int count, out long size )
        {
            var response = IncludeRequest( uri );
            if(response != null)
            {
                var splitted = response.Split( ':' );
                if(splitted.Length == 2)
                {
                    if(int.TryParse(splitted[0], out int c)
                        && long.TryParse(splitted[1], out long s))
                    {
                        count = c;
                        size = s;

                        return true;
                    }
                }
            }

            count = 0;
            size = 0L;
            return false;
        }

        private ServiceResponse<T> Request<T>(string uri)
        {
            var remoteStr = m_WebClient.DownloadString( uri );
            if(remoteStr != null)
            {
                var converted = JsonConvert.DeserializeObject<ServiceResponse<T>>( remoteStr );
                if(converted != null)
                {
                    return converted;
                }
            }

            return null;
        }

        private string Uri( Updatable what, RequestAction action )
            => $"http://access.firelands.su/public/backend-query/update-service/get-" + what.ToString().ToLower() + $"-{action.ToString().ToLower()}/";
        private string ApplyBeta( string uri, bool beta = false )
            => uri + ( beta == false ? "0" : "1" );
    }
}
