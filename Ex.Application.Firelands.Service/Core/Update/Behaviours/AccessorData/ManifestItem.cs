namespace Ex.Application.Firelands.Service.Core.Update.Behaviours.AccessorData
{
    public class ManifestItem
    {
        private string m_Remote;
        private string m_Local;
        private string m_Hash;

        public ManifestItem( string remote, string local, string hash )
        {
            m_Remote = remote;
            m_Local = local;
            m_Hash = hash;
        }

        public static ManifestItem Parse( string line )
        {
            var parsed = line.Split( ':' );
            if ( parsed.Length == 4 )
            {
                return new ManifestItem( "http:" + parsed[ 1 ], parsed[ 2 ], parsed[ 3 ] );
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
