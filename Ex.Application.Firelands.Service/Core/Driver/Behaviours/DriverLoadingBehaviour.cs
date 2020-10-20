using Ex.Application.Firelands.Service.Core.Driver.Behaviours.CommunicateData;

using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Ex.Application.Firelands.Service.Core.Driver.Behaviours
{
    public class DriverLoadingBehaviour : ExBehaviour
    {
        private class Imports
        {
            [StructLayout( LayoutKind.Sequential )]
            public struct UNICODE_STRING : IDisposable
            {
                public ushort Length;
                public ushort MaximumLength;
                private IntPtr buffer;

                public UNICODE_STRING( string s )
                {
                    Length = ( ushort ) ( s.Length * 2 );
                    MaximumLength = ( ushort ) ( Length + 2 );
                    buffer = Marshal.StringToHGlobalUni( s );
                }

                public void Dispose()
                {
                    Marshal.FreeHGlobal( buffer );
                    buffer = IntPtr.Zero;
                }

                public override string ToString()
                {
                    return Marshal.PtrToStringUni( buffer );
                }
            }

            [DllImport( "ntdll.dll" )]
            public static extern NTSTATUS RtlInitUnicodeString( ref UNICODE_STRING DestinationString,
                 [MarshalAs( UnmanagedType.LPWStr )] String SourceString );

            public enum ADJUST_PRIVILEGE_TYPE
            {
                AdjustCurrentProcess,
                AdjustCurrentThread
            };

            public const int SeLoadDriverPrivilege = 10;
            public const int SeDebugPrivilege = 20;

            [DllImport( "ntdll.dll", SetLastError = true )]
            public static extern NTSTATUS RtlAdjustPrivilege( int Privilege, bool Enable,
                ADJUST_PRIVILEGE_TYPE CurrentThread, out bool Enabled );

            [DllImport( "ntdll.dll", SetLastError = true )]
            public static extern NTSTATUS NtLoadDriver( ref UNICODE_STRING DriverServiceName );

            [DllImport( "ntdll.dll", SetLastError = true )]
            public static extern NTSTATUS NtUnloadDriver( ref UNICODE_STRING DriverServiceName );

            [DllImport( "Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto )]
            public static extern SafeFileHandle CreateFile(
                string fileName,
                [MarshalAs( UnmanagedType.U4 )] FileAccess fileAccess,
                [MarshalAs( UnmanagedType.U4 )] FileShare fileShare,
                IntPtr securityAttributes,
                [MarshalAs( UnmanagedType.U4 )] FileMode creationDisposition,
                [MarshalAs( UnmanagedType.U4 )] FileAttributes flagsAndAttributes,
                IntPtr template );
        }

        private string m_Path;
        private string m_Service;
        private string m_Display;
        private string m_Symbolic;

        private ExDriverObject m_DriverObject;
        private SafeHandle m_DriverHandle;

        public override void Awake()
        {
            m_DriverObject = Unbox<ExDriverObject>( ParentObject );
        }

        public bool Loaded()
            => m_DriverHandle != null && m_DriverHandle.IsInvalid == false;

        public bool Load(string file, string service, string display, string symbolic)
        {
            if ( Loaded() )
                return true;

            m_Path = file;
            m_Service = service;
            m_Display = display;
            m_Symbolic = symbolic;

            m_DriverHandle = Imports.CreateFile( m_Symbolic, FileAccess.ReadWrite, FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero );
            if(m_DriverHandle == null || m_DriverHandle.IsInvalid)
            {
                return IoControlCode.NT_SUCCESS( ReloadDriver() );
            }

            return true;
        }

        public bool Unload()
        {
            if(Loaded())
            {
                m_DriverHandle.Close();
                m_DriverHandle = null;
            }

            return IoControlCode.NT_SUCCESS( UnloadDriver() );
        }

        private NTSTATUS ReloadDriver()
        {
            UnloadDriver();

            if ( string.IsNullOrWhiteSpace( m_Path ) )
            {
                return NTSTATUS.ObjectPathNotFound;
            }

            var status = LoadDriver();
            if ( IoControlCode.NT_SUCCESS( status ) )
            {
               m_DriverHandle = Imports.CreateFile( m_Symbolic, FileAccess.ReadWrite, FileShare.ReadWrite,
                    IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero );

                if ( m_DriverHandle == null || m_DriverHandle.IsInvalid )
                {
                    return ( NTSTATUS ) Marshal.GetLastWin32Error();
                }
            }

            return NTSTATUS.Success;
        }

        private NTSTATUS LoadDriver()
        {
            string registry = $"CurrentControlSet\\Services\\{m_Service}";
            RegistryKey serviceKey = Registry.LocalMachine.CreateSubKey( $"SYSTEM\\{registry}" );
            serviceKey.SetValue( "ImagePath", $"\\??\\{m_Path}" );
            serviceKey.SetValue( "Type", 1 );

            Imports.UNICODE_STRING regUnicodeString = new Imports.UNICODE_STRING();
            var status = Imports.RtlAdjustPrivilege( Imports.SeLoadDriverPrivilege, true, Imports.ADJUST_PRIVILEGE_TYPE.AdjustCurrentProcess, out var _ );
            if ( IoControlCode.NT_SUCCESS( status ) )
            {
                status = Imports.RtlInitUnicodeString( ref regUnicodeString, $"\\Registry\\Machine\\SYSTEM\\{registry}" );
                if(IoControlCode.NT_SUCCESS(status))
                {
                    status = Imports.NtLoadDriver( ref regUnicodeString );
                }
            }

            return status;
        }

        private NTSTATUS UnloadDriver()
        {
            string regPath = "CurrentControlSet\\Services\\" + m_Service;

            Imports.UNICODE_STRING uRegPath = new Imports.UNICODE_STRING();

            var status = Imports.RtlAdjustPrivilege( Imports.SeLoadDriverPrivilege, true,
                Imports.ADJUST_PRIVILEGE_TYPE.AdjustCurrentProcess, out var _ );

            if(IoControlCode.NT_SUCCESS(status))
            {
                status = Imports.RtlInitUnicodeString( ref uRegPath, "\\Registry\\Machine\\SYSTEM\\" + regPath );
                if(IoControlCode.NT_SUCCESS(status))
                {
                    status = Imports.NtUnloadDriver( ref uRegPath );
                    if(IoControlCode.NT_SUCCESS(status))
                    {
                        Registry.LocalMachine.DeleteSubKeyTree( "SYSTEM\\" + regPath, false );
                    }
                }
            }

            return status;
        }

        public string Path()
            => m_Path;
        public string Service()
            => m_Service;
        public string Display()
            => m_Display;

        public SafeHandle Handle()
            => m_DriverHandle;
        public ExDriverObject Parent()
            => m_DriverObject;
    }
}
