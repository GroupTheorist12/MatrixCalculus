using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixCalculus
{
    public class Symbol
    {
        public List<Token> Tokens
        {
            get;
            set;
        }

        public bool IsOperator { get; set; }
        public Symbol()
        {
            this.Tokens = new List<Token>();
            IsOperator = false;
        }

        public Symbol(string exp)
        {
            TokenFactory tokes = new TokenFactory();
            tokes.ParseExpression(exp);
            this.Tokens = tokes.TokenList;
            IsOperator = false;
        }
        public string NakedTokenString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < Tokens.Count; i++)
                {
                    Token t = Tokens[i];
                    sb.Append(t.Value);

                }
                return sb.ToString();
            }
        }

        public string TokenString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < Tokens.Count; i++)
                {
                    Token t = Tokens[i];
                    sb.Append(t.Type);
                    if (t.Type == "Operator" || t.Type == "Function")
                    {
                        sb.Append(t.Value);
                    }

                }
                return sb.ToString();
            }
        }

        private static Symbol Multiply(Symbol a, Symbol b)
        {
            Symbol combine = new Symbol(a.NakedTokenString + " * " + b.NakedTokenString);
            Console.WriteLine("{0}", combine.TokenString);

            Symbol ret = new Symbol();

            Rational rA;
            Rational rB;
            Rational rSum;
            Rational rExp;

            switch (combine.TokenString)
            {
                case "VariableOperator*Variable":
                    if (combine.Tokens[0].Value == combine.Tokens[2].Value) // x * x
                    {
                        ret = new Symbol(combine.Tokens[0].Value + "^2");
                    }
                    else
                    {
                        ret = new Symbol(combine.Tokens[0].Value + combine.Tokens[2].Value);
                    }
                    break;
                case "VariableOperator*LiteralOperator*Variable":
                    if (combine.Tokens[0].Value == combine.Tokens[4].Value) // x * ax
                    {
                        ret = new Symbol(combine.Tokens[2].Value + combine.Tokens[0].Value + "^2");
                    }
                    else
                    {
                        ret = new Symbol(combine.Tokens[2].Value + combine.Tokens[0].Value + combine.Tokens[4].Value);
                    }
                    break;
                case "LiteralOperator*VariableOperator*Variable":
                    if (combine.Tokens[2].Value == combine.Tokens[4].Value) // ax * x
                    {
                        ret = new Symbol(combine.Tokens[0].Value + combine.Tokens[2].Value + "^2");
                    }
                    else
                    {
                        ret = new Symbol(combine.Tokens[0].Value + combine.Tokens[2].Value + combine.Tokens[4].Value);
                    }
                    break;
                case "LiteralOperator*VariableOperator*LiteralOperator*Variable": //ax * bx = abx^2
                    rA = Rational.Parse(combine.Tokens[0].Value);
                    rB = Rational.Parse(combine.Tokens[4].Value);
                    rSum = rA * rB;

                    if (combine.Tokens[2].Value == combine.Tokens[6].Value)
                    {
                        ret = new Symbol(rSum.ToString() + combine.Tokens[2].Value + "^2");
                    }
                    else
                    {
                        ret = new Symbol(rSum.ToString() + combine.Tokens[2].Value + combine.Tokens[6].Value);
                    }
                    break;
                case "VariableOperator*VariableOperator^Literal":
                    //     0	    1	    2	    3	    4
                    rExp = Rational.Parse(combine.Tokens[4].Value) + 1;
                    if (combine.Tokens[0].Value == combine.Tokens[2].Value) // x * x^2
                    {
                        ret = new Symbol(combine.Tokens[0].Value + "^" + rExp.ToString());
                    }
                    else
                    {
                        ret = new Symbol(combine.Tokens[0].Value + combine.Tokens[2].Value);
                    }
                    break;

                case "VariableOperator^LiteralOperator*Variable":
                    //  0	    1	    2	    3	    4 
                    rExp = Rational.Parse(combine.Tokens[2].Value) + 1;
                    if (combine.Tokens[0].Value == combine.Tokens[4].Value) // x^2 * x
                    {
                        ret = new Symbol(combine.Tokens[0].Value + "^" + rExp.ToString());
                    }
                    else
                    {
                        ret = new Symbol(combine.Tokens[0].Value + combine.Tokens[2].Value);
                    }
                    break;
                default:
                    ret = combine;
                    break;
            }

            return ret;
        }
        public static Symbol operator *(Symbol a, Symbol b)
        {

            /*
            int i = 0;
            if(a.Tokens.Count > b.Tokens.Count)
            {
                for(i = 0; i < a.Tokens.Count; i++)
                {
                    b.Tokens.Add(new Token("Null", "0"));
                }
            }
            else if(b.Tokens.Count > a.Tokens.Count)
            {
                for(i = 0; i < b.Tokens.Count; i++)
                {
                    a.Tokens.Add(new Token("Null", "0"));
                }

            }

            for(i = 0; i < a.Tokens.Count; i++)
            {
                if(a.Tokens[i].Type == "Null" || b.Tokens[i].Type == "Null")
                {
                    break;
                }
            }
            */

            return Multiply(a, b);
        }
    }
}