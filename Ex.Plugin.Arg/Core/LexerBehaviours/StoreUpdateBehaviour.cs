using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ex.Plugin.Arg.Core.LexerBehaviours
{
    internal class StoreUpdateBehaviour : ExBehaviour
    {
        private Lexer m_Lexer;

        public override void Awake()
        {
            m_Lexer = Unbox<Lexer>( ParentObject );
        }

        public override void Update()
        {
            
        }
    }
}
