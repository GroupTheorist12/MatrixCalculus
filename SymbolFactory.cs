using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MatrixCalculus
{
    public class SymbolFactory
    {
        private SymbolFactory()
        {

        }

        public SymbolType FactorySymbolType { get; private set; }
        public RowColumn FactoryRowColumn {get;set;}

        public TokenFactory tokenFactory  {get;}
        public SymbolFactory(SymbolType st, TokenFactory epf = null)
        {
            FactorySymbolType = st;
            FactoryRowColumn = RowColumn.Column;
            tokenFactory = epf;
        }

        public Symbol this[string exp]
        {
            get
            {
                Symbol sym = new Symbol();
                sym.Expression = exp;
                sym.symbolType = FactorySymbolType;
                sym.Parent = this;

                if(tokenFactory != null)
                {
                    tokenFactory.ParseExpression(exp);
                    sym.Tokens = tokenFactory.TokenList;
                }
                sym.Discover();
                return sym;
            }
        }

        public SymbolVector this[params string[] exps]
        {
            get
            {
                SymbolVector sv = new SymbolVector(this.FactoryRowColumn);
                sv.Parent = this;
                foreach(string exp in exps)
                {
                    sv.Add(this[exp]);
                }


                return sv;
            }
        }
        public SymbolMatrix this[int Rows, int Columns, params string[] exps]
        {
            get
            {
                SymbolMatrix ret = new SymbolMatrix(Rows, Columns);
                ret.Parent = this;
                ret.SymbolMatrixSymbolType = FactorySymbolType;
                int cnt = 0;

                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        ret[i, j] = this[exps[cnt++]];
                    }
                }

                return ret;

            }
        }
    }
}
