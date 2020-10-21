using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex.Application.Firelands.Service.Core.Access.Behaviours
{
    public class AccessListenerBehaviour : ExBehaviour
    {
        private ExAccessObject m_AccessObject;

        public override void Awake()
        {
            m_AccessObject = Unbox<ExAccessObject>();
        }

        public void Listen()
        {

        }

        public void Shutdown()
        {

        }
    }
}
