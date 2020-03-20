using System.Collections.Generic;

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
            for (int tokenCount = 0; tokenCount < tokens.Count; tokenCount++)
            {
                Token t = tokens[tokenCount];
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
                    sym.Expression = sym.NakedTokenString;
                    Add(sym);
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
                sym.Expression = sym.NakedTokenString;
                Add(sym);
            }
        }
    }
}