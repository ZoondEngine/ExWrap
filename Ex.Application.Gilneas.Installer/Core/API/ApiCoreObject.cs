using Ex.Application.Gilneas.Installer.Core.API.Behaviours;
using Ex.Application.Gilneas.Installer.Core.API.Behaviours.Utilities;
using Ex.Attributes;
using Ex.Exceptions;

using System;

namespace Ex.Application.Gilneas.Installer.Core.API
{
    public enum Method
    {
        GET,
        POST
    }
    public enum Uri
    {
        GetLoaderInc,
        GetLoaderManifest,
        GetLoaderArchive
    }

    [RequiredBehaviour(typeof(ApiAccessorBehaviour))]
    public class ApiCoreObject : ExObject
    {
        static readonly string[] m_Suffix = 
        { 
            "bytes", "KB", "MB", "GB", "TB", 
            "PB", "EB", "ZB", "YB" 
        };

        private ManifestContainer m_Manifest;
        private int m_Files;
        private long m_Size;

        public ApiCoreObject()
            : base()
        { }

        public ApiCoreObject(string tag)
            : base(tag)
        { }

        public async void LoadManifest()
        {
            var manifestFile = await Accessor().Builder<string>().GetAsync( Router.Url( Uri.GetLoaderManifest ) );

            if ( Accessor().Empty( manifestFile ) )
            {
                throw new ExException( "Can't download required files!" );
            }

            m_Manifest = new ManifestContainer(manifestFile.Split('\n'));
        }

        public async void LoadInclude()
        {
            var includeFile = await Accessor().Builder<string>().GetAsync( Router.Url( Uri.GetLoaderInc ) );
            if ( Accessor().Empty( includeFile ) )
            {
                throw new ExException( "Can't download required files!" );
            }

            var includeSplitted = includeFile.Split( ':' );
            if ( includeSplitted.Length != 2 )
            {
                throw new ExException( "Invalid include file!" );
            }

            if ( !int.TryParse( includeSplitted[ 0 ], out var files ) || long.TryParse( includeSplitted[ 1 ], out var size ) )
            {
                throw new ExException( "Invalid include file format!" );
            }
            else
            {
                m_Files = files;
                m_Size = size;
            }
        }

        public ManifestContainer Manifest()
            => m_Manifest;
        public int Files()
            => m_Files;
        public long Size()
            => m_Size;

        public string Humanized(long data = 0, int decimalPlaces = 1)
        {
            if ( data == 0 )
                data = m_Size;

            if ( data < 0 ) { return "-" + Humanized( -data ); }

            int i = 0;
            decimal dValue = data;

            while ( Math.Round( dValue, decimalPlaces ) >= 1000 )
            {
                dValue /= 1024;
                i++;
            }

            return string.Format( "{0:n" + decimalPlaces + "} {1}", dValue, m_Suffix[ i ] );
        }

        public ApiAccessorBehaviour Accessor()
            => GetComponent<ApiAccessorBehaviour>();
    }
}
