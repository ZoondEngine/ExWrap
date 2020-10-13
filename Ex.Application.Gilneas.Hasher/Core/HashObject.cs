using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Ex.Application.Gilneas.Hasher.Core
{
    public class HashObject : ExObject
    {
        public HashObject()
            : base()
        { }

        public HashObject( string tag )
            : base( tag )
        { }

        public string MD5( string path )
            => Hash<MD5>( path );
        public async Task<string> MD5Async( string path )
            => await AsyncHash<MD5>( path );

        public Task<string> AsyncHash<T>( string path ) where T : HashAlgorithm
            => Task.Run( () => Hash<T>( path ) );

        public string Hash<T>( string path ) where T : HashAlgorithm
        {
            if(File.Exists(path))
            {
                using ( var stream = File.OpenRead( path ) )
                {
                    var stringBuilder = new StringBuilder();
                    MethodInfo create = typeof( T ).GetMethod( "Create", new Type[] { } );
                    if(create != null)
                    {
                        using ( T cryptor = ( T ) create.Invoke( null, null ) )
                        {
                            byte[] bytes = cryptor.ComputeHash( stream );
                            for(var it = 0; it < bytes.Length; it++ )
                            {
                                stringBuilder.Append( bytes[ it ].ToString( "X2" ) );
                            }
                        }
                    }

                    return stringBuilder.ToString();
                }
            }

            return "00000000000000000000000000";
        }
    }
}
