using Ex.Plug;

using System;

namespace Ex.Plugin.Log
{
    public class LogExPlugin : IExPlugin
    {
        public T As<T>() where T : IExPlugin
        {
            if ( typeof( T ) != GetType() )
                return default;

            return ( T )( object )this;
        }

        public string GetIdentifier()
            => "Ex.Plugin.Log";

        public string GetName()
            => "ExLogger";

        public string GetVersion()
            => "1.0.0.0";

        public void OnLoad()
        {
            Console.WriteLine($"Addon: {GetName()}(v.{GetVersion()}--{GetIdentifier()}) loaded!");
        }

        public void OnUnload()
        {
            Console.WriteLine( $"Addon: {GetName()}(v.{GetVersion()}--{GetIdentifier()}) unloaded!" );
        }

        public void ToConsole(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.UtcNow}: {message}");
            Console.ResetColor();
        }
    }
}
