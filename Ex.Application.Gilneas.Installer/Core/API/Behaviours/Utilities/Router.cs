using System.Collections.Generic;

namespace Ex.Application.Gilneas.Installer.Core.API.Behaviours.Utilities
{
    static class Router
    {
        private static string m_TargetRoute = "https://cdn.firelands.su/installer/";
        private static Dictionary<Uri, string> m_Uri = new Dictionary<Uri, string>()
        {
            [ Uri.GetLoaderInc ]      = "include",
            [ Uri.GetLoaderManifest ] = "manifest",
            [ Uri.GetLoaderArchive ]  = "download"
        };

        public static string Url(Uri uri)
        {
            if(m_Uri.ContainsKey(uri))
                return m_TargetRoute + m_Uri[ uri ];

            return "";
        }
    }
}
