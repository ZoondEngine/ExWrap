using Ex.Plug;
using Ex.Plugin.Log;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Ex.Application.Server
{
    class Program
    {
        static void Main( string[] args )
        {
            var plugins = ExObject.Instantiate<ExPluginObject>();
            if( plugins != null)
            {
                plugins.Subscribe( ( x ) => Console.WriteLine( x ) );
                plugins.Load( Directory.GetCurrentDirectory() );

                Console.WriteLine($"Loaded plugins count: {plugins.Count()}");

                var logger = plugins.Get<LogExPlugin>();
                if(logger != null)
                {
                    logger.Writer().ErrorImmediate("testim ebat");
                }
            }

            Console.ReadLine();
        }
    }
}
