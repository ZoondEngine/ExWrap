using Ex.Attributes;
using Ex.Plugin.Log.Behaviours;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex.Application.Server
{
    [RequiredBehaviour(typeof(LogWriteBehaviour))]
    public class ExSpamObject : ExObject
    {
        public ExSpamObject()
            : base()
        { }

        public LogWriteBehaviour Writer()
            => GetComponent<LogWriteBehaviour>();
    }
}
