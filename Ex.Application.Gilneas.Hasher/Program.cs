using Ex.Application.Gilneas.Hasher.Core;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Ex.Application.Gilneas.Hasher
{
    class Program
    {

#if DEBUG
        private static bool m_Debug = true;
#else
        private static bool m_Debug = false;
#endif

        private static HashObject m_HashObject;
        private static TerminalObject m_TerminalObject;
        private static string m_Url = "https://cdn.firelands.su/downloads/";

        static void Main( string[] args )
        {
            if ( m_Debug )
                Console.WriteLine( "Building objects..." );

            m_HashObject = ExObject.Instantiate<HashObject>();
            m_TerminalObject = ExObject.Instantiate<TerminalObject>();

            if ( m_HashObject == null )
                Console.WriteLine( "Error. Can't build hash object!" );

            if ( m_TerminalObject == null )
                Console.WriteLine( "Error. Can't build terminal object!" );

            if(m_TerminalObject == null || m_HashObject == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine( "Required objects not builded!" );
                Console.Read();

                Environment.Exit( -1 );
            }
            else
            {
                m_TerminalObject.Writer().Trace( "URL (subfolder or http/s): " );
                m_TerminalObject.Writer().TraceL( $"" );

                var url = m_TerminalObject.Reader().Read().AsString();
                if(url.Contains("http"))
                {
                    m_Url = url;
                }
                else
                {
                    m_Url += url;
                }

                m_TerminalObject.Writer().Trace( $"Hash directory (default: {Environment.CurrentDirectory}): " );
                m_TerminalObject.Writer().TraceL( $"" );

                if ( Execute( m_TerminalObject.Reader().Read().AsString() ) )
                {
                    m_TerminalObject.Writer().Success( "Hashing done" );
                }
                else
                {
                    m_TerminalObject.Writer().Error( "Can't hash input directory!" );
                }

                m_TerminalObject.Pause();
            }
        }

        private static bool Execute( string dir )
        {
            var writer = m_TerminalObject.Writer();
            var reader = m_TerminalObject.Reader();

            if ( dir.Empty() )
                dir = Environment.CurrentDirectory;

            if ( !Directory.Exists( dir ) )
            {
                writer.Error( $"Directory = '{dir}' not found!" );
                return false;
            }

            Stopwatch globalStopwatch = null;
            Stopwatch currentStopwatch = null;

            if ( m_Debug )
            {
                writer.Debug( "Starting hash. Building stopwatch ..." );

                globalStopwatch = new Stopwatch();
                currentStopwatch = new Stopwatch();

                writer.Success( "Done" );
            }

            writer.Trace( "Preparing ..." );

            int count = 0;
            long size = 0;

            if ( File.Exists( ".manifest" ) )
                File.Delete( ".manifest" );

            if ( File.Exists( ".inc" ) )
                File.Delete( ".inc" );

            writer.Trace( "Start hashing ..." );

            if ( globalStopwatch != null )
                globalStopwatch.Start();

            using ( var manifestFile = new FileStream( ".manifest", FileMode.OpenOrCreate ) )
            {
                var files = Directory.GetFiles( dir, "*.*", SearchOption.AllDirectories );
                files.Shuffle();

                try
                {
                    files.AsParallel().ForAll( ( file ) =>
                    {
                        if ( file.Contains( ".manifest" ) || file.Contains( ".inc" ) || file.Contains( "Ex." ) )
                            return;

                        count++;
                        var clearFile = file.Replace( Environment.CurrentDirectory, "" );

                        long currentSize = 0;
                        using ( var fs = File.OpenRead( file ) )
                        {
                            currentSize = fs.Length;
                        }

                        size += currentSize;

                        writer.Trace( $"Hashing file: {clearFile} {currentSize} bytes" );

                        if ( m_Debug )
                            currentStopwatch.Start();

                        var remote = $"{m_Url}/{file.Replace( Environment.CurrentDirectory, "" ).Remove( 0, 1 ).Replace( "\\", "/" )}";
                        byte[] buffer = Encoding.UTF8.GetBytes( $"\"{remote}\":\"{clearFile}\":\"{m_HashObject.MD5( file )}\"" + Environment.NewLine );
                        manifestFile.Write( buffer, 0, buffer.Length );

                        if ( m_Debug )
                        {
                            currentStopwatch.Stop();

                            writer.Warning( $"File = '{clearFile}' hashed" );
                            writer.Warning( $"Elapsed time = '{currentStopwatch.ElapsedMilliseconds}ms'" );

                            currentStopwatch.Reset();
                        }
                    } );
                }
                catch(Exception ex)
                {
                    writer.Exception( ex );
                    return false;
                }
            }

            if ( m_Debug )
                writer.Debug( "Generating manifest files... " );

            using ( var incFile = new FileStream( ".inc", FileMode.OpenOrCreate ) )
            {
                byte[] buffer = Encoding.UTF8.GetBytes( $"{count}:{size}" );
                incFile.Write( buffer, 0, buffer.Length );
            }

            if ( m_Debug )
                writer.Success( "Done" );

            return true;
        }
    }
}
