using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Ex.Application.Gilneas.Installer.Core.Install
{
    public class InstallStepList
    {
        private List<InstallStep> m_Steps;

        public InstallStepList()
        {
            m_Steps = new List<InstallStep>();
        }

        public void Push( InstallStep step )
            => m_Steps.Add( step );

        public InstallStep Get()
        {
            var last = m_Steps.Last();
            if(last != null)
            {
                var clone = ( InstallStep ) m_Steps.Last().Clone();
                m_Steps.Remove( clone );

                return clone;
            }

            return last;
        }

        public InstallStep Reference()
            => m_Steps.Last();

        public int Count()
            => m_Steps.Count;
    }
}
