using Ex.Plug;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex.Plugin.Arg
{
    public class ExArgPlugin : IExPlugin
    {
        public T As<T>() where T : IExPlugin
        {
            if ( typeof( T ) != GetType() )
                return default;

            return ( T ) ( object ) this;
        }

        public string GetIdentifier()
            => "Ex.Plugin.Arg";

        public string GetName() 
            => "Ex.Arg";

        public string GetVersion()
            => "1.0.0.0";

        public void OnLoad()
        {
            throw new NotImplementedException();
        }

        public void OnUnload()
        {
            throw new NotImplementedException();
        }
    }
}
