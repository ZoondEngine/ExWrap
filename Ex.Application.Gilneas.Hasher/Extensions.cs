using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex.Application.Gilneas.Hasher
{
    public static class Extensions
    {
        public static bool Empty( this string str )
            => string.IsNullOrEmpty( str ) && string.IsNullOrWhiteSpace( str );

        public static void Shuffle( this string[] array )
        {
            if ( array.Length < 1 ) return;
            var random = new Random();
            for ( var i = 0; i < array.Length; i++ )
            {
                var key = array[ i ];
                var rnd = random.Next( i, array.Length );
                array[ i ] = array[ rnd ];
                array[ rnd ] = key;
            }
        }
    }
}
