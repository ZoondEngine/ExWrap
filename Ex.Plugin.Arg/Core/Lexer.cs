using Ex.Attributes;
using Ex.Plugin.Arg.Core.LexerBehaviours;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex.Plugin.Arg.Core
{
    [RequiredBehaviour(typeof(StoreUpdateBehaviour))]
    internal class Lexer : ExObject
    {
        private Store m_Store;

        public Lexer(Store store)
            : base()
        {
            m_Store = store;
            Initiailize();
        }

        public Lexer(Store store, string tag)
            : base(tag)
        {
            m_Store = store;
            Initiailize();
        }

        private void Initiailize()
        {
            m_Store.Initialize();
        }

        public Store Store()
            => m_Store;
    }
}
