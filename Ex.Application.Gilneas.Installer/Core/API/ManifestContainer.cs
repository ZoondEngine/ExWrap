using Ex.Exceptions;

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ex.Application.Gilneas.Installer.Core.API
{
    public class ManifestContainer : IEnumerable<ManifestElement>
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
            lines = lines.Where((x) => x != "").ToArray();

            foreach(var line in lines)
            {
                var element = ManifestElement.Parse( line );
                if ( element == null )
                    throw new ExException( "Incorrect manifest file!" );

                m_ManifestElements.Add( element );
            }
        }

        public IEnumerator GetEnumerator()
        {
            return m_ManifestElements.GetEnumerator();
        }

        IEnumerator<ManifestElement> IEnumerable<ManifestElement>.GetEnumerator()
        {
            return m_ManifestElements.GetEnumerator();
        }
    }
}
