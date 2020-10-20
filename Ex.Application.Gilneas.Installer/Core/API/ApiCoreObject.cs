using Ex.Application.Gilneas.Installer.Core.API.Behaviours;
using Ex.Application.Gilneas.Installer.Core.API.Behaviours.Utilities;
using Ex.Attributes;
using Ex.Exceptions;

using System;
using System.Threading.Tasks;

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
        GetLoaderArchive,
        GetFile
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

        public async void ManifestAsync()
            => await Task.Run( () => Manifest() );
        public async void IncludeAsync()
            => await Task.Run( () => Include() );

        public void Manifest()
        {
            var manifestFile = Accessor().Builder<string>().Request( Router.Url( Uri.GetLoaderManifest, Entry.Settings().Read<bool>("Testing", "Beta") ) ).First();

            if ( Accessor().Empty( manifestFile.Content ) )
            {
                throw new ExException( "Can't download required files!" );
            }

            m_Manifest = new ManifestContainer(manifestFile.Content.Split('\r', '\n'));
        }

        public void Include()
        {
            var includeFile = Accessor().Builder<string>().Request( Router.Url( Uri.GetLoaderInc, Entry.Settings().Read<bool>( "Testing", "Beta" ) ) ).First();
            if(includeFile.Status != 600)
            {
                throw new ExException( includeFile.Message );
            }

            if ( Accessor().Empty( includeFile.Content ) )
            {
                throw new ExException( "Can't download required files!" );
            }

            var includeSplitted = includeFile.Content.Split( ':' );
            if ( includeSplitted.Length != 2 )
            {
                throw new ExException( "Invalid include file!" );
            }

            if ( !int.TryParse( includeSplitted[ 0 ], out var files ) || !long.TryParse( includeSplitted[ 1 ], out var size ) )
            {
                throw new ExException( "Invalid include file format!" );
            }
            else
            {
                m_Files = files;
                m_Size = size;
            }
        }

        public ManifestContainer Container()
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
