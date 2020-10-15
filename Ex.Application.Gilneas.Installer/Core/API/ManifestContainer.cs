using Ex.Exceptions;

using System.Collections.Generic;

namespace Ex.Application.Gilneas.Installer.Core.API
{
    public class ManifestContainer
    {
        private List<ManifestElement> m_ManifestElements;

        public ManifestContainer()
        {
            m_ManifestElements = new List<ManifestElement>();
        }
        public ManifestContainer(string[] lines)
            : this()
        {
            BuildManifest( lines );
        }

        public void BuildManifest(string[] lines)
        {
            foreach(var line in lines)
            {
                var element = ManifestElement.Parse( line );
                if ( element == null )
                    throw new ExException( "Incorrect manifest file!" );

                m_ManifestElements.Add( element );
            }
        }
    }
}
