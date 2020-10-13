using Ex.Application.Gilneas.Hasher.Core.Command.Actions;

namespace Ex.Application.Gilneas.Hasher.Core.Command
{
    public sealed class ReadBehaviour : ExBehaviour
    {
        private TerminalObject m_TerminalObject;

        public override void Awake()
        {
            m_TerminalObject = Unbox<TerminalObject>( ParentObject );
        }

        public Reading Read()
            => new Reading();

        public TerminalObject Parent()
            => m_TerminalObject;
    }
}
