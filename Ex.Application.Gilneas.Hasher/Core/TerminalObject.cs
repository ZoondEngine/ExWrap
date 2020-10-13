using Ex.Application.Gilneas.Hasher.Core.Command;
using Ex.Attributes;

using System;

namespace Ex.Application.Gilneas.Hasher.Core
{
    [RequiredBehaviour(typeof(ReadBehaviour))]
    [RequiredBehaviour(typeof(WriteBehaviour))]
    public sealed class TerminalObject : ExObject
    {
        public TerminalObject()
            : base()
        { }

        public TerminalObject( string tag )
            : base( tag )
        { }

        public void Pause()
            => Console.ReadKey();

        public ReadBehaviour Reader()
            => GetComponent<ReadBehaviour>();

        public WriteBehaviour Writer()
            => GetComponent<WriteBehaviour>();
    }
}
