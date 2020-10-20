using Ex.Application.Firelands.Service.Core.Driver.Behaviours.CommunicateData;

namespace Ex.Application.Firelands.Service.Core.Driver
{
    public static class IoControlCode
    {
        public static uint IO_CODE_STARTUP = Code( 0x00000022, 0x0700, 0, 0 );
        public static uint IO_CODE_STOP = Code( 0x00000022, 0x0701, 0, 0 );
        public static uint IO_CODE_HEARTBEAT = Code(0x00000022, 0x0702, 0, 0);

        private static uint Code( uint deviceType, uint function, uint method, uint access )
             => (( deviceType ) << 16) | (( access ) << 14) | (( function ) << 2) | ( method );

        public static bool NT_SUCCESS( NTSTATUS status )
            => status == NTSTATUS.Success;
    }
}
