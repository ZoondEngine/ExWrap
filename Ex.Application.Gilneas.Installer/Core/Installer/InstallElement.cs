using System.Windows.Controls;

namespace Ex.Application.Gilneas.Installer.Core.Installer
{
    public class InstallElement
    {
        private UserControl m_Child;
        private bool m_Final;
        private int m_Order;

        public InstallElement(UserControl child, bool final, int order)
        {
            m_Child = child;
            m_Final = final;
            m_Order = order;
        }

        public UserControl Child()
            => m_Child;
        public bool Final()
            => m_Final;
        public int Order()
            => m_Order;
    }
}
