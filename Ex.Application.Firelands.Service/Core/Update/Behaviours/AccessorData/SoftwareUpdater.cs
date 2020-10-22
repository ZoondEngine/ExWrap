using Ex.Crypto;

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;

namespace Ex.Application.Firelands.Service.Core.Update.Behaviours.AccessorData
{
    public class SoftwareUpdater
    {
        private List<ManifestItem> m_Manifest;
        private string m_Path;
        private string m_Process;
        private HashObject m_HashObject;

        public SoftwareUpdater Build(string path, string process, List<ManifestItem> manifest, HashObject hash)
        {
            m_Path = path;
            m_Process = process;
            m_Manifest = manifest;
            m_HashObject = hash;

            return this;
        }

        public bool Execute()
        {
            if(DestroyProcess())
            {
                return Install();
            }

            return false;
        }

        private bool Install()
        {
            WebClient client = new WebClient();

            foreach(var item in m_Manifest)
            {
                var local = m_Path + "\\" + item.Local();
                var folder = Path.GetDirectoryName( local );

                if ( !Directory.Exists( folder ) )
                    Directory.CreateDirectory( folder );

                try
                {
                    if(File.Exists(local))
                    {
                        if ( m_HashObject.MD5( local ) != item.Hash() )
                        {
                            File.Delete( local );
                            client.DownloadFile( item.Remote(), local );

                            if ( m_HashObject.MD5( local ) != item.Hash() )
                                return false;
                        }
                    }
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        private bool DestroyProcess()
        {
            try
            {
                var processes = Process.GetProcesses().Where( ( x ) => x.ProcessName.ToLower() == m_Process.ToLower() );
                if ( processes.Count() > 0 )
                {
                    foreach ( var process in processes )
                    {
                        process.Kill();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
