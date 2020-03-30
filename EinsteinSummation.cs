using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace MatrixCalculus
{
    public enum SubScript
    {
        i,
        j,
        k,
        l,
        m,
        n,
        o,
        p,
        r
    }
    public enum SuperScript
    {
        i,
        j,
        k,
        l,
        m,
        n,
        o,
        p,
        r
    }

    public class EinsteinSummation
    {
        public int Dimension { get; private set; }
        public List<string> Coordinates = new List<string>() { "x", "y", "z" };
        public List<string> CoEfficents = new List<string>() { "a", "b", "c", "d" };

        private List<string> Indices = new List<string>();

        public EinsteinSummation(int d)
        {
            Dimension = d;
        }

        public string this[string Coefficient, string Variable, params SubScript[] subscripts]
        {
            get
            {
                return "";
            }
        }

        private Symbol ParseExpression(string FunctionString)
        {
            Indices.Clear();
            Symbol sym = new Symbol();
            int charcounter = 0;
            TokenFactory tf = new TokenFactory();

            List<string> VariableBuffer = new List<string>();
            List<string> CoefficentBuffer = new List<string>();
            List<string> IndicesBuffer = new List<string>();


            while (charcounter < FunctionString.Length)
            {
                char ch = FunctionString[charcounter];
                //Is variable?
                if (Coordinates.Exists(c => c == ch.ToString()))
                {
                    if (VariableBuffer.Count > 0)
                    {
                        sym.Tokens.Add(new Token("VariableSum", string.Join("", VariableBuffer.ToArray())));
                        VariableBuffer.Clear();
                    }
                    if (CoefficentBuffer.Count > 0)
                    {
                        sym.Tokens.Add(new Token("CoefficentSum", string.Join("", CoefficentBuffer.ToArray())));
                        CoefficentBuffer.Clear();
                    }

                    VariableBuffer.Add(ch.ToString());
                }
                else if (CoEfficents.Exists(c => c == ch.ToString()))
                {
                    if (VariableBuffer.Count > 0)
                    {
                        sym.Tokens.Add(new Token("VariableSum", string.Join("", VariableBuffer.ToArray())));
                        VariableBuffer.Clear();
                    }
                    if (CoefficentBuffer.Count > 0)
                    {
                        sym.Tokens.Add(new Token("CoefficentSum", string.Join("", CoefficentBuffer.ToArray())));
                        CoefficentBuffer.Clear();
                    }
                    CoefficentBuffer.Add(ch.ToString());
                }
                else if (ch == '_') //me under score
                {
                    if (VariableBuffer.Count > 0)
                    {
                        VariableBuffer.Add(ch.ToString());
                    }
                    else if (CoefficentBuffer.Count > 0)
                    {
                        CoefficentBuffer.Add(ch.ToString());
                    }
                }
                else if (tf.isLetter(ch) &&
                !Coordinates.Exists(c => c == ch.ToString()) &&
                !CoEfficents.Exists(c => c == ch.ToString()))
                {
                    Indices.Add(ch.ToString());
                    IndicesBuffer.Add(ch.ToString());
                    if (VariableBuffer.Count > 0)
                    {
                        VariableBuffer.Add(ch.ToString());
                    }
                    else if (CoefficentBuffer.Count > 0)
                    {
                        CoefficentBuffer.Add(ch.ToString());
                    }

                }
                else if (ch == '=')
                {
                    if (VariableBuffer.Count > 0)
                    {
                        sym.Tokens.Add(new Token("VariableSum", string.Join("", VariableBuffer.ToArray())));
                        VariableBuffer.Clear();
                    }
                    if (CoefficentBuffer.Count > 0)
                    {
                        sym.Tokens.Add(new Token("CoefficentSum", string.Join("", CoefficentBuffer.ToArray())));
                        CoefficentBuffer.Clear();
                    }

                    if (IndicesBuffer.Count > 0)
                    {
                        sym.Tokens[sym.Tokens.Count - 1].MetaData = string.Join(",", IndicesBuffer.ToArray());
                        IndicesBuffer.Clear();
                    }
                    sym.Tokens.Add(new Token("Assignment", " = "));
                }
                else if (ch == '{')
                {
                    if (VariableBuffer.Count > 0)
                    {
                        VariableBuffer.Add(ch.ToString());
                    }
                    else if (CoefficentBuffer.Count > 0)
                    {
                        CoefficentBuffer.Add(ch.ToString());
                    }

                }
                else if (ch == '}')
                {
                    if (VariableBuffer.Count > 0)
                    {
                        VariableBuffer.Add(ch.ToString());
                        sym.Tokens.Add(new Token("VariableSum", string.Join("", VariableBuffer.ToArray())));
                        VariableBuffer.Clear();
                    }
                    if (CoefficentBuffer.Count > 0)
                    {
                        CoefficentBuffer.Add(ch.ToString());
                        sym.Tokens.Add(new Token("CoefficentSum", string.Join("", CoefficentBuffer.ToArray())));
                        CoefficentBuffer.Clear();
                    }

                    if (IndicesBuffer.Count > 0)
                    {
                        sym.Tokens[sym.Tokens.Count - 1].MetaData = string.Join(",", IndicesBuffer.ToArray());
                        IndicesBuffer.Clear();
                    }

                }
                charcounter++;
            }

            if (VariableBuffer.Count > 0)
            {
                sym.Tokens.Add(new Token("VariableSum", string.Join("", VariableBuffer.ToArray())));
                VariableBuffer.Clear();
            }
            if (CoefficentBuffer.Count > 0)
            {
                sym.Tokens.Add(new Token("CoefficentSum", string.Join("", CoefficentBuffer.ToArray())));
                CoefficentBuffer.Clear();
            }
            if (IndicesBuffer.Count > 0)
            {
                sym.Tokens[sym.Tokens.Count - 1].MetaData = string.Join(",", IndicesBuffer.ToArray());
                IndicesBuffer.Clear();
            }

            return sym;
        }

        public string Expand(string Expression)
        {
            string ret = string.Empty;
            Symbol sym = ParseExpression(Expression);

            SymbolList symList = new SymbolList();
            foreach(Token t in sym.Tokens)
            {
                string[] arr = t.MetaData.Split(",".ToCharArray());
                if(arr.Length == 1) // one indice
                {
                    for(int i = 1; i < Dimension + 1; i++)
                    {
                        Symbol symNew = new Symbol();
                        symNew.Tokens.Add(new Token("Variable", t.Value.Replace(arr[0], i.ToString())));
                        symList.Add(symNew);

                    }
                }
            }
            return ret;
        }
        
        public Symbol this[string Expression]
        {
            get
            {
                Symbol sym = ParseExpression(Expression);
                sym.Expression = Expression;
                return sym;
            }
        }
        

    }
}
