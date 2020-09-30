using Ex.Plug;
using Ex.Plugin.Log;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex.Application.Server
{
    class Program
    {
        static void Main( string[] args )
        {
            var plugins = ExObject.Instantiate<ExPluginObject>();
            if( plugins != null)
            {
                plugins.Load( Directory.GetCurrentDirectory() );

                Console.WriteLine($"Loaded plugins count: {plugins.Count()}");

                var logger = plugins.Get<LogExPlugin>();
                if(logger != null)
                {
                    logger.ToConsole( "Testim ebat" );
                }
            }

            Console.ReadLine();
        }
    }
}
