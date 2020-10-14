using System;
using System.Windows.Controls;

namespace Ex.Application.Gilneas.Installer.Core.Install
{
    public class InstallStep : ICloneable
    {
        private string m_Name;
        private UserControl m_Control;
        private bool m_IsLast;

        public InstallStep( string name, UserControl control, bool isLast )
        {
            m_Name = name;
            m_Control = control;
            m_IsLast = isLast;
        }

        public string Name()
            => m_Name;
        public UserControl Control()
            => m_Control;
        public bool IsLast()
            => m_IsLast;

        public object Clone()
            => new InstallStep( Name(), Control(), IsLast() );
    }
}
