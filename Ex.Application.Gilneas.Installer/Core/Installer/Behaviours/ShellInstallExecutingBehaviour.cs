using System.Collections.Generic;

namespace Ex.Application.Gilneas.Installer.Core.Installer.Behaviours
{
    public delegate void NextShellExecuteDelegate( string description );

    public class ShellInstallExecutingBehaviour : ExBehaviour
    {
        private InstallCoreObject m_InstallCoreObject;
        private List<IShell> m_InstallShells;

        private event NextShellExecuteDelegate m_OnNextShell;

        public override void Awake()
        {
            m_InstallCoreObject = Unbox<InstallCoreObject>( ParentObject );
            m_InstallShells = new List<IShell>();
        }

        public void Subscribe( NextShellExecuteDelegate next )
            => m_OnNextShell += next;
        public void Unsubscribe( NextShellExecuteDelegate next )
            => m_OnNextShell -= next;

        public bool Execute()
        {
            for(var it = 0; it < m_InstallShells.Count; it++ )
            {
                var current = m_InstallShells[ it ];
                if(current != null)
                {
                    m_OnNextShell?.Invoke( current.Description() );

                    if ( !current.Execute() )
                        return false;
                }
            }

            return true;
        }
    }
}
