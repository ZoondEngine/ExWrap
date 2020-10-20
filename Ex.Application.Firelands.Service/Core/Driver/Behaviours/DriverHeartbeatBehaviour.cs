using Ex.Application.Firelands.Service.Core.Driver.Structures.Heartbeat;
using System;

namespace Ex.Application.Firelands.Service.Core.Driver.Behaviours
{
    public class DriverHeartbeatBehaviour : ExBehaviour
    {
        private ExDriverObject m_DriverObject;
        private TimeSpan m_InternalTick;
        private HeartbeatResponse m_LastResponse;

        public override void Awake()
        {
            m_DriverObject = Unbox<ExDriverObject>();    
        }

        public override void Update()
        {
            if ( m_InternalTick.Ticks == 0 )
                m_InternalTick = CurrentTickTime;

            if ( ( CurrentTickTime - m_InternalTick ).TotalSeconds > 2D )
            {
                if ( !m_DriverObject.Request( IoControlCode.IO_CODE_HEARTBEAT, new HeartbeatRequest(), m_LastResponse ) )
                {
                    m_DriverObject.Log().ErrorImmediate($"{CurrentTickTime.Ticks} error heartbeat!");
                    m_DriverObject.Call();
                }
                else
                {
                    m_DriverObject.Log().TraceImmediate( $"cr - {CurrentTickTime.Ticks} pv - {m_InternalTick.Ticks} -- heartbeat" );
                }
            }
        }
    }
}
