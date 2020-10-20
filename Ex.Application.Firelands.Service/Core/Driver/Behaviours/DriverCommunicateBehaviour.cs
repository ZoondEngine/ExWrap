using Ex.Application.Firelands.Service.Core.Driver.Behaviours.CommunicateData;
using Ex.Application.Firelands.Service.Core.Driver.Structures;

using System;
using System.Runtime.InteropServices;

namespace Ex.Application.Firelands.Service.Core.Driver.Behaviours
{
    public class DriverCommunicateBehaviour : ExBehaviour
    {
        private class Imports
        {
            [DllImport( "Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto )]
            public static extern NTSTATUS DeviceIoControl( SafeHandle hDevice, uint IoControlCode,
            [MarshalAs( UnmanagedType.AsAny )][In] object InBuffer, uint nInBufferSize,
            [MarshalAs( UnmanagedType.AsAny )][Out] object OutBuffer, uint nOutBufferSize,
            ref int pBytesReturned, IntPtr Overlapped );
        }

        private ExDriverObject m_DriverObject;
        private SafeHandle m_DriverHandle;

        public override void Awake()
        {
            m_DriverObject = Unbox<ExDriverObject>( ParentObject );
        }

        public bool Send<TIn, TOut>(uint code, TIn request, TOut response) 
            where TIn : BaseRequest 
            where TOut : BaseResponse
        {
            if(m_DriverHandle.IsInvalid)
            {
                if(m_DriverObject.Loader().Loaded())
                {
                    m_DriverHandle = m_DriverObject.Loader().Handle();
                }
                else
                {
                    m_DriverObject.Log().ErrorImmediate("Trying to send request to not loaded driver!");
                    return false;
                }
            }

            int bytes = 0;
            if( IoControlCode.NT_SUCCESS(Imports.DeviceIoControl(m_DriverHandle, code, request, (uint)Marshal.SizeOf(request), response, (uint)Marshal.SizeOf(response), ref bytes, IntPtr.Zero)))
            {
                return bytes > 0;
            }
            else
            {
                m_DriverObject.Log().ErrorImmediate( $"Error while sending request = '{code}'!" );
            }

            return false;
        }
    }
}
