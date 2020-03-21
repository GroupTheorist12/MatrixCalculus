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
                    sym.Expression = exp;
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
                SymbolMatrix retVal = new SymbolMatrix(Rows, Columns);
                retVal.Parent = this;
                retVal.SymbolMatrixSymbolType = FactorySymbolType;
                int cnt = 0;

                for (int rowCount = 0; rowCount < Rows; rowCount++)
                {
                    for (int colCount = 0; colCount < Columns; colCount++)
                    {
                        retVal[rowCount, colCount] = this[exps[cnt++]];
                    }
                }

                return retVal;
            }
        }
    }
}