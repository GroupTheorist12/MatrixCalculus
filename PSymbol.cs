using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;

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

        private static Hashtable htSymbols = new Hashtable();
        private static List<long> lstIds = new List<long>();
        private static void SetId(PSymbol sym)
        {
            long id = 1;
            if(lstIds.Count == 0)
            {
                lstIds.Add(1);
            }
            else
            {
                id = lstIds[lstIds.Count - 1] + 1;
                lstIds.Add(id);    
            }

            sym.PSymbolId = id;

            htSymbols[id] = sym;
        }
        public PSymbolFactory parent = null;
        public string Expression
        {
            get; protected set;
        }

        public long PSymbolId
        {
            get; private set;
        }
        public string Variable
        {
            get; set;
        }

        public PSymbol()
        {
            SetId(this);
        }

        public PSymbol(string v)
        {
            Expression = v;
            SetId(this);
        }

        private PSymbol(Rational r)
        {
            Expression = r.ToString();
            lstLiterals.Add(r);
            SetId(this);
        }
        private PSymbol(int value)
        {
            Expression = value.ToString();
            lstLiterals.Add(new Rational(value));
            SetId(this);
        }

        public static PSymbol DF(PSymbol psym)
        {
            PSymbol ret = null;

            TokenFactory tf = new TokenFactory();
            tf.ParseExpression(psym.Expression);

            foreach(Symbol sym in tf.symbolList)
            {
                
            }
            return ret;
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

        private static void SetParent(PSymbol a, PSymbol b)
        {
            if(a.parent != null && b.parent != null)
            {
                return;
            }
            
            if(a.parent == null && b.parent == null)
            {
                a.parent = b.parent = new PSymbolFactory();
            }
            else if(a.parent != null && b.parent == null)
            {
                b.parent = a.parent;
            }
            else if(b.parent != null && a.parent == null)
            {
                a.parent = b.parent;
            }

        }
        public static PSymbol operator +(PSymbol a, PSymbol b)
        {
            PSymbol ret = null;
            SetParent(a, b);
            if (a.Expression == b.Expression)
            {
                ret = new PSymbol("2" + a.Expression);
            }
            else
            {
                ret = new PSymbol(a.Expression + " + " + b.Expression);
            }

            ret.parent = a.parent;
            ret.lstSymbols.Add(a);
            ret.lstSymbols.Add(b);

            return ret;
        }

        public static PSymbol operator *(PSymbol a, PSymbol b)
        {
            PSymbol ret = null;
            SetParent(a, b);
            if (a.Expression == b.Expression)
            {
                ret = new PSymbol(a.Expression + "^2");
            }
            else
            {
                ret = new PSymbol(a.Expression + "*" + b.Expression);
            }

            ret.parent = a.parent;
            ret.lstSymbols.Add(a);
            ret.lstSymbols.Add(b);
            return ret;
        }

        public static PSymbol operator /(PSymbol a, PSymbol b)
        {
            PSymbol ret = null;
            SetParent(a, b);
            if (a.Expression == b.Expression)
            {
                ret = new PSymbol("1");
            }
            else
            {
                ret = new PSymbol(a.Expression + "/" + b.Expression);
            }

            ret.parent = a.parent;
            ret.lstSymbols.Add(a);
            ret.lstSymbols.Add(b);
            return ret;
        }

        public static PSymbol operator ^(PSymbol a, PSymbol b)
        {
            SetParent(a, b);
            PSymbol ret = new PSymbol(a.Expression + "^" + b.Expression);

            if(a.Expression.IndexOf(" + ") != -1)
            {
                ret = new PSymbol("(" + a.Expression + ")^" + b.Expression);
            }
            ret.parent = a.parent;
            ret.lstSymbols.Add(a);
            ret.lstSymbols.Add(b);
            return ret;
        }

    }
}
