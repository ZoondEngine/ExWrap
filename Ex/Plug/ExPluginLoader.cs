using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ex.Plug
{
    internal static class ExPluginLoader
    {
        private static List<IExPlugin> m_LoadedPlugins;

        public static int Count()
            => m_LoadedPlugins.Count;
        public static void Clear()
            => m_LoadedPlugins.Clear();

        public static T Get<T>() where T : IExPlugin
            => (T)m_LoadedPlugins.FirstOrDefault( ( x ) => x.GetType() == typeof( T ) );
        public static void Add<T>() where T : IExPlugin, new()
            => m_LoadedPlugins.Add( new T() );

        public static void Load<T>() where T : IExPlugin
            => Execute<T>( ( e ) => e.OnLoad() );
        public static void Load()
            => Execute( ( e ) => e.OnLoad() );

        public static void Unload<T>() where T : IExPlugin
            => Execute<T>( ( x ) => x.OnUnload() );
        public static void Unload()
            => Execute( ( e ) => e.OnUnload() );

        public static void Read(string pluginPath = "plugins\\")
        {
            var files = Directory.GetFiles( pluginPath, "Ex.Plugin.*.dll" );
            if(files.Length > 0)
            {
                foreach(var file in files)
                {
                    var fileAssembly = Assembly.LoadFrom( file );
                    if(fileAssembly != null)
                    {
                        var subList = fileAssembly.GetTypes()
                                    .Where( m => m.GetInterfaces().Contains( typeof( IExPlugin ) ) )
                                    .Select( m => m.GetConstructor( Type.EmptyTypes ).Invoke( null ) as IExPlugin )
                                    .ToList();

                        if(m_LoadedPlugins == null)
                        {
                            m_LoadedPlugins = new List<IExPlugin>();
                        }

                        m_LoadedPlugins.AddRange( subList );
                    }
                }
            }
            else
            {
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
    }
}
