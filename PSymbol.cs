using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixCalculus
{

    public class Sin
    {
        public PSymbol this[PSymbol value]
        {
            get
            {
                PSymbol ret = new PSymbol("sin(" + value.Expression + ")");
                return ret;
            }
        }
    }
    public class PSymbol
    {
        private List<Rational> lstLiterals = new List<Rational>();
        private List<PSymbol> lstSymbols = new List<PSymbol>();

        public PSymbolFactory parent = null;
        public string Expression
        {
            get; protected set;
        }

        public string Variable
        {
            get; set;
        }

        public PSymbol()
        {

        }

        public PSymbol(string v)
        {
            Expression = v;
        }

        private PSymbol(Rational r)
        {
            Expression = r.ToString();
        }
        private PSymbol(int value)
        {
            Expression = value.ToString();
            lstLiterals.Add(new Rational(value));
        }

        static public implicit operator PSymbol(Rational value)
        {
            PSymbol psym = new PSymbol(value);
            return psym;
        }

        static public implicit operator PSymbol(int value)
        {
            return new PSymbol(value);
        }

        public static PSymbol operator +(PSymbol a, PSymbol b)
        {
            PSymbol ret = null;
            if (a.Expression == b.Expression)
            {
                ret = new PSymbol("2" + a.Expression);
            }
            else
            {
                ret = new PSymbol(a.Expression + " + " + b.Expression);
            }

            return ret;
        }

        public static PSymbol operator *(PSymbol a, PSymbol b)
        {
            PSymbol ret = null;
            if (a.Expression == b.Expression)
            {
                ret = new PSymbol(a.Expression + "^2");
            }
            else
            {
                ret = new PSymbol(a.Expression + "*" + b.Expression);
            }

            return ret;
        }

        public static PSymbol operator ^(PSymbol a, PSymbol b)
        {
            PSymbol ret = new PSymbol(a.Expression + "^" + b.Expression);

            if(a.Expression.IndexOf(" + ") != -1)
            {
                ret = new PSymbol("(" + a.Expression + ")^" + b.Expression);
            }
            return ret;
        }

    }
}
