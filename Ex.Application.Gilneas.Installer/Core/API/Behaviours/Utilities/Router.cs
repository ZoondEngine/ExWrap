using System.Collections.Generic;

namespace Ex.Application.Gilneas.Installer.Core.API.Behaviours.Utilities
{
    static class Router
    {
        private static string m_TargetRoute = "http://access.firelands.su/public/backend-query/installer-service/";
        private static Dictionary<Uri, string> m_Uri = new Dictionary<Uri, string>()
        {
            [ Uri.GetLoaderInc ]      = "get-include",
            [ Uri.GetLoaderManifest ] = "get-manifest",
            [ Uri.GetFile ]           = "get-file",
            [ Uri.GetLoaderArchive ]  = "download"
        };

        public static string Url(Uri uri, bool beta = false)
        {
            if(m_Uri.ContainsKey(uri))
                return (m_TargetRoute + m_Uri[ uri ]) + (beta == false ? "/0" : "/1");

            return "";
        }
    }
}
