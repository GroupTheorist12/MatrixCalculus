using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixCalculus
{
    public class PSymbol
    {
        private List<Rational> lstLiterals = new List<Rational>();
        public string Variable
        {
            get;private set;
        }
        public PSymbol()
        {

        }

        public PSymbol(string v)
        {
            Variable = v;
        }

        private PSymbol(Rational r)
        {
            Variable = r.ToString();
        }
        private PSymbol(int value)
        {
            Variable = value.ToString();
            lstLiterals.Add(new Rational(value));
        }

        static public implicit  operator PSymbol(Rational value)
        {
            return new PSymbol(value);
        }

        static public implicit  operator PSymbol(int value)
        {
            return new PSymbol(value);
        }

        public static PSymbol operator +(PSymbol a, PSymbol b)
        {
            PSymbol ret = null;
            string strA = nameof(a);
            string strB = nameof(b);
            if(strA == strB)
            {
                ret = new PSymbol("2" + strA);
            }
            else
            {
                ret = new PSymbol(strA + " + " + strB);
            }
            return ret;
        }
    }
}
