using Ex.Application.Gilneas.Installer.Content.Controls;
using Ex.Application.Gilneas.Installer.Core.Installer.Behaviours;
using Ex.Attributes;

using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Ex.Application.Gilneas.Installer.Core.Installer
{
    [RequiredBehaviour(typeof(ShellInstallExecutingBehaviour))]
    public class InstallCoreObject : ExObject
    {
        private Stack<InstallElement> m_InstallElements;
        private InstallElement m_CurrentElement;
        private List<InstallElement> m_ElementsList;

        public InstallCoreObject()
            : base()
        {
            Setup();
        }

        public InstallCoreObject(string tag)
            : base(tag)
        {
            Setup();
        }

        private void Setup()
        {
            m_InstallElements = new Stack<InstallElement>();
            m_CurrentElement = null;
            m_ElementsList = new List<InstallElement>()
            {
                new InstallElement(new SelectionControl(), false, 1),
                new InstallElement(new InstallControl(), true, 2)
            };
        }

        public void Install()
        {
            if(m_CurrentElement.Final())
            {
                if ( ( ( IFinalInstallElement ) m_CurrentElement.Child() ).Install() )
                {
                    return;
                }
            }

            MessageBox.Show( "Установка не удалась! Повторите попытку позже" );
        }

        public InstallElement Current()
        {
            if(m_CurrentElement == null)
            {
                m_CurrentElement = m_ElementsList.First();
                return m_CurrentElement;
            }

            return m_CurrentElement;
        }

        public InstallElement Next()
        {
            if ( m_CurrentElement.Final() )
                return null;

            m_InstallElements.Push( m_CurrentElement );
            m_CurrentElement = m_ElementsList.FirstOrDefault( ( x ) => x.Order() == m_CurrentElement.Order() + 1 );

            return Current();
        }
        public InstallElement Previous()
        {
            if ( m_InstallElements.Count <= 0 )
                return Current();

            m_CurrentElement = m_InstallElements.Pop();
            return Current();
        }

        public ShellInstallExecutingBehaviour Shell()
            => GetComponent<ShellInstallExecutingBehaviour>();
    }
}
