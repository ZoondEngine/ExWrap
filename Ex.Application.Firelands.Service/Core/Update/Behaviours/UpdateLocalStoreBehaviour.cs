using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ex.Application.Firelands.Service.Core.Update.Behaviours
{
    public class UpdateLocalStoreBehaviour : ExBehaviour
    {
        private class VersionEntry
        {
            private string m_Software;
            private Version m_Version;

            public VersionEntry(string software, Version ver)
            {
                m_Software = software;
                m_Version = ver;
            }

            public string Software()
                => m_Software;
            public Version Ver()
                => m_Version;

            public override string ToString()
            {
                return $"{m_Software}={m_Version}";
            }
        }

        private List<VersionEntry> m_Versions;
        private string m_Stamp;

        public override void Awake()
        {
            m_Versions = new List<VersionEntry>();
            m_Stamp = Environment.GetFolderPath( Environment.SpecialFolder.System ) + "firelands.services.stamp";

            Read();
        }

        public Version Get( Updatable what )
            => m_Versions.FirstOrDefault( ( x ) => x.Software().ToLower() == what.ToString().ToLower() )?.Ver() ?? Version.Parse("0.0.0.0");

        public string GetLauncherPath()
            => Encoding.UTF8.GetString( File.ReadAllBytes( Environment.GetFolderPath( Environment.SpecialFolder.System ) + "firelands.registry" ) );

        public bool Write()
        {
            try
            {
                if ( File.Exists( m_Stamp ) )
                    File.Delete( m_Stamp );

                using ( var fs = new FileStream( m_Stamp, FileMode.OpenOrCreate, FileAccess.ReadWrite ) )
                {
                    for ( var i = 0; i < m_Versions.Count; i++ )
                    {
                        var buffer = Encoding.UTF8.GetBytes( m_Versions[ i ].ToString() );
                        fs.Write( buffer, 0, buffer.Length );
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Read()
        {
            if ( File.Exists( m_Stamp ) )
            {
                var lines = File.ReadAllLines( m_Stamp );
                if(lines.Length == 3)
                {

                    for ( var i = 0; i < lines.Length; i++ )
                    {
                        var spliced = lines[ i ].Split( '=' );
                        if ( spliced.Length != 2 )
                            break;

                        if ( !Version.TryParse( spliced[ 1 ], out var ver ) )
                        {
                            break;
                        }
                        else
                        {
                            m_Versions.Add( new VersionEntry( spliced[ 0 ], ver ) );
                        }
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
