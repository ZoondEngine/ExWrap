using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex.Application.Gilneas.Installer.Core.Install
{
    public interface IInstallScript
    {
        bool Execute();

        string Name();
    }
}
