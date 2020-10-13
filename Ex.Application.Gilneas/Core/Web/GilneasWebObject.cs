using Ex.Attributes;
using Ex.Plugin.Log;
using Ex.Plugin.Log.Behaviours;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex.Application.Gilneas.Core.Web
{
    public enum RequestMethod
    {
        GET,
        POST,
        PUT,
        PATCH
    }

    [RequiredBehaviour(typeof(LogWriteBehaviour))]
    public class GilneasWebObject : ExObject, ILoggableObject
    {


        public LogWriteBehaviour Writer()
            => GetComponent<LogWriteBehaviour>();
    }
}
