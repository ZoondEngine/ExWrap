using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ex.Plug
{
    public delegate void MessageChangedDelegate( string message );

    internal static class ExPluginLoader
    {
        private static List<IExPlugin> m_LoadedPlugins;

        public static event MessageChangedDelegate OnMessageChanged;

        public static int Count()
            => m_LoadedPlugins.Count;
        public static void Clear()
        {
            OnMessageChanged?.Invoke( FormatMessage( $"Inf: Clearing global registry" ) );
            m_LoadedPlugins.Clear();
        }

        public static T Get<T>() where T : IExPlugin
        {
            OnMessageChanged?.Invoke(FormatMessage($"Inf: Trying to get plugin = '{typeof(T)}'"));
            var found = m_LoadedPlugins.FirstOrDefault( ( x ) => x.GetType() == typeof( T ) );
            if(found != default)
            {
                OnMessageChanged?.Invoke( FormatMessage( $"Inf: Plugin = '{typeof( T )}' found!" ) );
                return ( T )found;
            }
            else
            {
                OnMessageChanged?.Invoke( FormatMessage( $"Err: Plugin = '{typeof( T )}' not found!" ) );
                return default;
            }
        }
        public static void Add<T>() where T : IExPlugin, new()
        {
            OnMessageChanged( FormatMessage( $"Inf: Creating instance for plugin = '{typeof( T )}'" ) );
            m_LoadedPlugins.Add( new T() );
        }
        public static void Remove<T>() where T : IExPlugin
        {
            OnMessageChanged( FormatMessage( $"Inf: Remove instance for plugin = '{typeof( T )}'" ) );
            m_LoadedPlugins.Remove( Get<T>() );
        }

        public static void Load<T>() where T : IExPlugin
        {
            OnMessageChanged( FormatMessage( $"Inf: Execute 'OnLoad' for plugin = '{typeof( T )}'" ) );
            Execute<T>( ( e ) => e.OnLoad() );
        }
        public static void Load()
        {
            OnMessageChanged( FormatMessage( $"Inf: Execute 'OnLoad' for all plugins instances" ) );
            Execute( ( e ) => e.OnLoad() );
        }

        public static void Unload<T>() where T : IExPlugin
        {
            OnMessageChanged( FormatMessage( $"Inf: Execute 'OnUnload' for plugin = '{typeof( T )}'" ) );
            Execute<T>( ( x ) => x.OnUnload() );
        }
        public static void Unload()
        {
            OnMessageChanged( FormatMessage( $"Inf: Execute 'OnUnload' for all plugins instances" ) );
            Execute( ( e ) => e.OnUnload() );
        }

        public static void Read(string pluginPath = "plugins\\")
        {
            OnMessageChanged( FormatMessage( $"Inf: Reading directory = '{pluginPath}' for getting assemblies" ) );
            var files = Directory.GetFiles( pluginPath, "Ex.Plugin.*.dll" );
            if(files.Length > 0)
            {
                OnMessageChanged( FormatMessage( $"Inf: Found files = '{files.Length}'" ) );
                foreach (var file in files)
                {
                    OnMessageChanged( FormatMessage( $"Inf: Trying to load = '{file}'" ) );
                    var fileAssembly = Assembly.LoadFrom( file );
                    if(fileAssembly != null)
                    {
                        OnMessageChanged( FormatMessage( $"Inf: Assembly = '{fileAssembly.FullName}' loaded. Searching plugins" ) );
                        var subList = fileAssembly.GetTypes()
                                    .Where( m => m.GetInterfaces().Contains( typeof( IExPlugin ) ) )
                                    .Select( m => m.GetConstructor( Type.EmptyTypes ).Invoke( null ) as IExPlugin )
                                    .ToList();

                        OnMessageChanged( FormatMessage( $"Inf: Found = '{subList.Count}' plugins in assembly = '{fileAssembly.FullName}'" ) );

                        if (m_LoadedPlugins == null)
                        {
                            OnMessageChanged( FormatMessage( $"Inf: Plugins registry not initialized. Initialize" ) );
                            m_LoadedPlugins = new List<IExPlugin>();
                        }

                        OnMessageChanged( FormatMessage( $"Inf: Add '{subList.Count}' plugins from assembly = '{fileAssembly.FullName}' to global registry" ) );
                        m_LoadedPlugins.AddRange( subList );
                    }
                    else
                    {
                        OnMessageChanged( FormatMessage( $"Err: Can't load = '{file}' - Not assembly!" ) );
                    }
                }
            }
            else
            {
                OnMessageChanged( FormatMessage( $"Err: Directory '{pluginPath}' don't contains available plugins for read" ) );
                m_LoadedPlugins = new List<IExPlugin>();
            }
        }

        private static void Execute<T>(Action<T> element) where T : IExPlugin
        {
            var get = Get<T>();
            if ( get != null )
            {
                element( get );
            }
        }
        private static void Execute(Action<IExPlugin> element)
        {
            for ( var it = 0; it < m_LoadedPlugins.Count; it++ )
            {
                element( m_LoadedPlugins[ it ] );
            }
        }

        private static string FormatMessage( string outString )
            => $"[{DateTime.Now}][PLUGIN]: {outString}";
    }
}
