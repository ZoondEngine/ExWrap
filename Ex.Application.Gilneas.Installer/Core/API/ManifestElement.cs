namespace Ex.Application.Gilneas.Installer.Core.API
{
    public class ManifestElement
    {
        private string m_Remote;
        private string m_Local;
        private string m_Hash;

        public ManifestElement(string remote, string local, string hash)
        {
            m_Remote = remote;
            m_Local = local;
            m_Hash = hash;
        }

        public static ManifestElement Parse(string line)
        {
            var parsed = line.Split( ':' );
            if(parsed.Length == 4)
            {
                return new ManifestElement( "http:" + parsed[ 1 ], parsed[ 2 ], parsed[ 3 ] );
            }

            return null;
        }

        public string Remote()
            => m_Remote;
        public string Local()
            => m_Local;
        public string Hash()
            => m_Hash;
    }
}
