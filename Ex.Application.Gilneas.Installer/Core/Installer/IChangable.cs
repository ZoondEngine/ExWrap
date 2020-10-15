using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Ex.Application.Gilneas.Installer.Core.Installer
{
    public interface IChangable
    {
        void ChangeContent( UserControl control );
    }
}
