using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

namespace MatrixCalculus
{
    public class PSymbolFactory
    {
        public List<string> Variables = new List<string>();
        public Hashtable htSymbols = new Hashtable();
        public PSymbol this[string variable]
        {
            get
            {
                PSymbol ret = new PSymbol(variable);
                Variables.Add(variable);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         ret.Variable = variable;
                ret.parent = this;
                return ret;

            }
        }
        
    }
}
