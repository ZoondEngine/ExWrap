using System;

namespace Ex.Application.Gilneas.Hasher.Core.Command
{
    public sealed class WriteBehaviour : ExBehaviour
    {
        private enum LogLevel
        {
            Trace,
            Debug,
            Warning,
            Error,
            Exception,
            Success
        }

        private TerminalObject m_TerminalObject;

        public override void Awake()
        {
            m_TerminalObject = Unbox<TerminalObject>( ParentObject );
        }

        public void SuccessL( string message )
           => Out( Format( message, LogLevel.Success ), ConsoleColor.DarkGreen );
        public void TraceL( string message )
            => Out( Format( message, LogLevel.Trace ), ConsoleColor.Gray );
        public void DebugL( string message )
            => Out( Format( message, LogLevel.Debug ), ConsoleColor.White );
        public void WarningL( string message )
            => Out( Format( message, LogLevel.Warning ), ConsoleColor.DarkYellow );
        public void ErrorL( string message )
            => Out( Format( message, LogLevel.Error ), ConsoleColor.DarkRed );
        public void ExceptionL( Exception ex )
            => Out( Format( ex.Message, LogLevel.Exception ), ConsoleColor.DarkCyan );

        public void Success( string message )
           => Line( Format( message, LogLevel.Success ), ConsoleColor.DarkGreen );
        public void Trace( string message )
            => Line( Format( message, LogLevel.Trace ), ConsoleColor.Gray );
        public void Debug( string message )
            => Line( Format( message, LogLevel.Debug ), ConsoleColor.White );
        public void Warning( string message )
            => Line( Format( message, LogLevel.Warning ), ConsoleColor.DarkYellow );
        public void Error( string message )
            => Line( Format( message, LogLevel.Error ), ConsoleColor.DarkRed );
        public void Exception( Exception ex )
            => Line( Format( ex.Message, LogLevel.Exception ), ConsoleColor.DarkCyan );

        public TerminalObject Parent()
            => m_TerminalObject;

        private string Format(string message, LogLevel level)
            => $"{DateTime.Now:dd/MM/yyyy, HH:mm::ss.f} > {message}";

        private void Line( string message, ConsoleColor color )
            => Out( message + Environment.NewLine, color );

        private void Out(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write( message );
            Console.ResetColor();
        }
    }
}
