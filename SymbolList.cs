using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixCalculus
{
    public class SymbolList : List<Symbol>
    {
        public SymbolList()
        {
            
        }
        public SymbolList(List<Token> tokens)        
        {
            bool SymbolEnd = false;
            Symbol sym = new Symbol();
            for (int i = 0; i < tokens.Count; i++)
            {
                Token t = tokens[i];
                if(sym == null && SymbolEnd)
                {
                    sym = new Symbol();
                    sym.IsOperator = true;    
                    sym.Tokens.Add(t);
                    this.Add(sym);
                    sym = null;
                    SymbolEnd = false;
                }
                else if(sym == null)
                {
                    sym = new Symbol();
                    sym.Tokens.Add(t);

                }
                else if(sym != null && t.SymbolEnd)
                {
                    sym.Tokens.Add(t);
                    this.Add(sym);
                    SymbolEnd = true;
                    sym = null;
                }
                else if(sym!= null && !t.SymbolEnd)
                {
                    sym.Tokens.Add(t);
                }
            }

            if(sym != null && sym.Tokens.Count > 0)
            {
                this.Add(sym);
            }

        }
    }
}