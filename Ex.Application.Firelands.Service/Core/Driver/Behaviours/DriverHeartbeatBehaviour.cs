using Ex.Application.Firelands.Service.Core.Driver.Structures.Heartbeat;
using System;

namespace Ex.Application.Firelands.Service.Core.Driver.Behaviours
{
    public class DriverHeartbeatBehaviour : ExBehaviour
    {
        private ExDriverObject m_DriverObject;
        private TimeSpan m_InternalTick;
        private HeartbeatResponse m_LastResponse;
        private bool m_Enabled;

        public override void Awake()
        {
            m_DriverObject = Unbox<ExDriverObject>();
            m_Enabled = false;
        }

        public override void Update()
        {
            if ( m_InternalTick.Ticks == 0 )
                m_InternalTick = CurrentTickTime;

            if ( ( CurrentTickTime - m_InternalTick ).TotalSeconds > 1D )
            {
                m_InternalTick = CurrentTickTime;
                if(m_Enabled)
                {
                    if ( !m_DriverObject.Request( IoControlCode.IO_CODE_HEARTBEAT, new HeartbeatRequest(), m_LastResponse ) )
                    {
                        m_DriverObject.Log().ErrorImmediate( $"{CurrentTickTime.Ticks} error heartbeat!" );
                        m_DriverObject.Call();
                    }
                    else
                    {
                        m_DriverObject.Log().TraceImmediate( $"cr - {CurrentTickTime.Ticks} it - {m_InternalTick.Ticks} -- heartbeat" );
                    }
                }
            }
        }

        public void Switch( bool enabled )
            => m_Enabled = enabled;
    }
}
