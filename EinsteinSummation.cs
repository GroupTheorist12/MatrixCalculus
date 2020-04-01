using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;

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
                    if (IndicesBuffer.Count > 0)
                    {
                        sym.Tokens[sym.Tokens.Count - 1].MetaData = string.Join(",", IndicesBuffer.ToArray());
                        IndicesBuffer.Clear();
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
                    if (IndicesBuffer.Count > 0)
                    {
                        sym.Tokens[sym.Tokens.Count - 1].MetaData = string.Join(",", IndicesBuffer.ToArray());
                        IndicesBuffer.Clear();
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

        public string FormatQuadratic(List<ITensor> lstT)
        {
            StringBuilder sb = new StringBuilder();

            if (lstT.Count == 3)
            {
                var sortL = from element in lstT
                            orderby element.Rank
                            select element;

                PseudoRank1Tensor p1 = (PseudoRank1Tensor)sortL.ToList()[0];
                p1.IsRowOrColumn = RowColumn.Row;

                PseudoRank1Tensor p2 = (PseudoRank1Tensor)sortL.ToList()[1];
                PseudoRank2Tensor p3 = (PseudoRank2Tensor)sortL.ToList()[2];
                sb.Append(p1.Expand());
                sb.Append(p3.Expand());
                sb.Append(p2.Expand());

            }

            return sb.ToString();
        }
        public List<ITensor> ExpandToList(string Expression)
        {
            List<ITensor> lstT = new List<ITensor>();
            Symbol sym = ParseExpression(Expression);

            foreach (Token t in sym.Tokens)
            {
                string[] arr = t.MetaData.Split(",".ToCharArray());

                if (arr.Length == 1)
                {
                    PseudoTensor p = new PseudoTensor(t.Value[0].ToString(), 1, this.Dimension, TensorType.Covariant);

                    lstT.Add(p.Tensor);
                }

                if (arr.Length == 2)
                {
                    PseudoTensor p = new PseudoTensor(t.Value[0].ToString(), 2, this.Dimension, TensorType.Covariant);
                    lstT.Add(p.Tensor);
                }
            }

            return lstT;
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
