using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixCalculus
{
    public class PSymbolFactory
    {
        public List<string> Variables = new List<string>();

        public PSymbol this[string variable]
        {
            get
            {
                PSymbol ret = new PSymbol();
                ret.Variable = variable;
                ret.parent = this;
                return ret;
            }
        }
    }
}
