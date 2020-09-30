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
            throw new NotImplementedException();
        }

        public string GetIdentifier()
        {
            throw new NotImplementedException();
        }

        public string GetName()
        {
            throw new NotImplementedException();
        }

        public string GetVersion()
        {
            throw new NotImplementedException();
        }

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
