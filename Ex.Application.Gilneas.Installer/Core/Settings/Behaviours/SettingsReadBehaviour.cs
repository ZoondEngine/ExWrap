using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex.Application.Gilneas.Installer.Core.Settings.Behaviours
{
    public class SettingsReadBehaviour : ExBehaviour
    {
        private SettingsCoreObject m_SettingsCoreObject;
        private Dictionary<string, List<string>> m_SettingsContent;
        private Dictionary<string, List<string>> m_SettingsDefault;

        public override void Awake()
        {
            m_SettingsCoreObject = Unbox<SettingsCoreObject>( ParentObject );
            m_SettingsContent = new Dictionary<string, List<string>>();
            m_SettingsDefault = new Dictionary<string, List<string>>()
            {
                ["Testing"] = new List<string>()
                {
                    "Beta=0"
                }
            };
        }

        public void Load()
        {
            if ( !Directory.Exists( "settings" ) )
                Default();

            if ( !File.Exists( "settings\\base.covered.dat" ) )
                Default();

            Parse();
        }

        public void Save()
        {
            Save( m_SettingsContent );
        }

        private void Parse()
        {
            var data = File.ReadAllLines( "settings\\base.covered.dat" );

            string currentSection = "";

            foreach(var line in data)
            {
                if ( string.IsNullOrEmpty( line ) )
                    continue;

                if ( line.StartsWith( ";" ) )
                    continue;

                if ( line.StartsWith( "{" ) && line.EndsWith( "}" ) )
                {
                    currentSection = line.Replace( "{", "" ).Replace( "}", "" );

                    if(!m_SettingsContent.ContainsKey(currentSection))
                        m_SettingsContent.Add( currentSection, new List<string>() );
                }
                
                if(currentSection != "")
                {
                    m_SettingsContent[ currentSection ].Add( line );
                }
            }
        }

        public void Default()
        {
            Save( m_SettingsDefault );
        }

        private void Save(Dictionary<string, List<string>> data)
        {
            if ( !Directory.Exists( "settings" ) )
            {
                Directory.CreateDirectory( "settings" );
            }

            if ( File.Exists( "settings\\base.covered.dat" ) )
            {
                File.Delete( "settings\\base.covered.dat" );
            }

            using(FileStream fs = File.OpenWrite("settings\\base.covered.dat"))
            {
                foreach(var section in data )
                {
                    byte[] buffer = Encoding.UTF8.GetBytes( "{" + section.Key + "}" + Environment.NewLine );
                    fs.Write( buffer, 0, buffer.Length );

                    foreach(var val in section.Value)
                    {
                        buffer = Encoding.UTF8.GetBytes( val + Environment.NewLine );
                        fs.Write( buffer, 0, buffer.Length );
                    }
                }
            }
        }

        private bool SafeType<T>()
        {
            return typeof( T ) == typeof( bool )
                || typeof( T ) == typeof( string )
                || typeof( T ) == typeof( int );
        }

        private T SafeValue<T>(string val)
        {
            if ( typeof( T ) == typeof( bool ) )
            {
                if ( val.ToLower() == "true" || val == "1" )
                    return ( T ) ( object ) true;

                if ( val.ToLower() == "false" || val == "0" )
                    return ( T ) ( object ) false;
            }

            if ( typeof( T ) == typeof( int ) )
            {
                if ( int.TryParse( val, out var num ) )
                    return ( T ) ( object ) num;
            }

            if ( typeof( T ) == typeof( string ) )
                return (T)(object)val;

            return default;
        }
        private string Writable<T>(T data)
        {
            if ( SafeType<T>() )
                return ( ( T ) ( object ) data ).ToString();

            return "unknown";
        }

        private bool Valid(string line)
        {
            return line.Split( '=' ).Length == 2;
        }

        public T Read<T>(string section, string key)
        {
            string val = null;
            if(m_SettingsContent.ContainsKey(section))
            {
                val = m_SettingsContent[ section ].FirstOrDefault( ( x ) => x.Split( '=' )[ 0 ].ToLower() == key.ToLower() );
            }
            else
            {
                if(m_SettingsDefault.ContainsKey(section))
                {
                    val = m_SettingsDefault[ section ].FirstOrDefault( ( x ) => x.Split( '=' )[ 0 ].ToLower() == key.ToLower() );
                }
            }

            if ( val != null )
            {
                if ( SafeType<T>() )
                {
                    if ( Valid( val ) )
                    {
                        return SafeValue<T>( val.Split( '=' )[ 1 ] );
                    }
                }
            }

            return default;
        }
        public void Write<T>(string section, string key, T val)
        {
            if ( SafeType<T>() )
            {
                if ( !m_SettingsContent.ContainsKey( section ) )
                    m_SettingsContent.Add( section, new List<string>() );

                var item = m_SettingsContent[ section ].FirstOrDefault( ( x ) => x.Split( '=' )[ 0 ].ToLower() == key.ToLower() );
                if ( item != null)
                {
                    m_SettingsContent[ section ].Remove( item );
                }

                m_SettingsContent[ section ].Add( $"key={Writable( val )}" );
            }
        }

        public void WriteSave<T>(string section, string key, T val)
        {
            Write( section, key, val );
            Save();
        }
    }
}
