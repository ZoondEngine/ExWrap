using Ex.Application.Gilneas.Installer.Core.Install.Scripts;

using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Ex.Application.Gilneas.Installer.Core.Install
{
    public delegate void OnStepFinish();
    public delegate void OnScriptChange(string name);

    public class InstallerObject : ExObject
    {
        private event OnStepFinish m_OnFinish;
        private event OnScriptChange m_ScriptChanged;

        private InstallStepList m_Steps = new InstallStepList();
        private List<InstallStep> m_StepsOrder = new List<InstallStep>();
        private List<IInstallScript> m_Scripts = new List<IInstallScript>();

        private Window m_Window = null;

        public InstallerObject()
             : base()
        {
            Setup();
        }

        public InstallerObject( string tag )
            : base( tag )
        {
            Setup();
        }

        private void Setup()
        {
            m_StepsOrder.Add( new InstallStep( "choose-folder", null, false ) );
            m_StepsOrder.Add( new InstallStep( "checking-space", null, false ) );
            m_StepsOrder.Add( new InstallStep( "installing", null, true ) );

            //Scripts
            m_Scripts.Add( new CreateShortcutInstallScript() );
            m_Scripts.Add( new SetRegistryInstallScript() );
        }

        public void Target( Window window )
            => m_Window = window;
        public Window Target()
            => m_Window;

        public void Subscribe( OnStepFinish onFinishHandler )
            => m_OnFinish += onFinishHandler;
        public void Subscribe( OnScriptChange onScriptChangeHandler )
            => m_ScriptChanged += onScriptChangeHandler;

        public void Unsubscribe( OnStepFinish onFinishHandler )
            => m_OnFinish -= onFinishHandler;
        public void Unsubscribe( OnScriptChange onScriptChangeHandler )
            => m_ScriptChanged -= onScriptChangeHandler;

        public void Finish()
            => m_OnFinish?.Invoke();
        public void ScriptChange( string name )
            => m_ScriptChanged?.Invoke( name );

        public bool Scripts()
        {
            for(var it = 0; it < m_Scripts.Count; it++ )
            {
                var script = m_Scripts[ it ];
                ScriptChange( script.Name() );

                if ( !script.Execute() )
                    return false;
            }

            return true;
        }
        public InstallStep Next()
        {
            var reference = m_Steps.Reference();
            if ( !reference.IsLast() )
            {
                var order = m_StepsOrder.FirstOrDefault( ( x ) => x.Name().ToLower() == reference.Name().ToLower() );
                if(order != null)
                {
                    var idx = m_StepsOrder.IndexOf( order ) + 1;
                    if(m_StepsOrder.Count >= idx)
                    {
                        m_Steps.Push( m_StepsOrder[ idx ] );
                        return m_Steps.Get();
                    }    
                }
            }

            return null;
        }
        public InstallStep Previous()
        {
            var reference = m_Steps.Reference();
            if(reference != null)
            {
                return m_Steps.Get();
            }

            m_Steps.Push( m_StepsOrder.First() );
            return m_StepsOrder.First();
        }
    }
}
