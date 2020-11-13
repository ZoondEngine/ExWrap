namespace Ex.Network.Security
{
    internal class Cryptor
    {
        private ICryptoCipher m_Cipher;

        public Cryptor()
        {
            ChangeCipher( new Rijndael() );
        }

        public void ChangeCipher( ICryptoCipher cipher )
            => m_Cipher = cipher;
        public string Encrypt( string data, string key )
            => m_Cipher.Encrypt( data, key );
        public string Decrypt( string data, string key )
            => m_Cipher.Decrypt( data, key );

        public static string StaticEncrypt( string data, string key )
            => Instance().Encrypt( data, key );
        public static string StaticDecrypt( string data, string key )
            => Instance().Decrypt( data, key );

        private static Cryptor m_Instance;
        public static Cryptor Instance()
        {
            lock ( m_Instance )
            {
                if ( m_Instance == null )
                {
                    m_Instance = new Cryptor();
                }

                return m_Instance;
            }
        }
    }
}
