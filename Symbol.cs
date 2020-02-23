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

        private Rational Exponent { get; set; }
        private Rational Literal { get; set; }

        private int ExponentIndex { get; set; }
        private int VariableIndex { get; set; }
        private int FunctionIndex { get; set; }
        private int BracketIndex { get; set; }

        public string FunctionType { get; set; }
        private void Discover()
        {
            this.ExponentIndex = IsPow();
            this.VariableIndex = IsVariable();
            this.FunctionIndex = IsFunction();
            this.BracketIndex = IsLeftBracket();
            this.Exponent = 0;
            this.Literal = 1;

            if (this.ExponentIndex >= 0) // pow => ^
            {
                //Literal after ^
                if (this.ExponentIndex < Tokens.Count - 1 && Tokens[this.ExponentIndex + 1].Type == "Literal")
                {
                    this.Exponent = Rational.Parse(Tokens[this.ExponentIndex + 1].Value); //Literal value should be first token after ^
                }
            }

            if (this.VariableIndex >= 0) //variable x^2 or x or 2*x or 2*x^2
            {
                if (this.VariableIndex - 2 >= 0 && Tokens[this.VariableIndex - 2].Type == "Literal") //literal times variable such as 2*x^2
                {
                    this.Literal = Rational.Parse(Tokens[this.VariableIndex - 2].Value);
                }
                else //naked variable aka x^2
                {
                    this.Literal = 1;
                }

            }

            if (this.FunctionIndex >= 0) //function like sin(x), sin(x)^2 2*sin(x)
            {
                if (this.FunctionIndex - 2 >= 0 && Tokens[this.FunctionIndex - 2].Type == "Literal") //literal times function such as 2*sin(x)^2
                {
                    this.Literal = Rational.Parse(Tokens[this.FunctionIndex - 2].Value);
                }
                else //naked variable aka x^2
                {
                    this.Literal = 1;
                }

            }

            //Variable Pow x^2
            if (this.ExponentIndex >= 0 && this.VariableIndex >= 0 && this.FunctionIndex == -1 && this.BracketIndex == -1)
            {
                this.FunctionType = "VariablePow";
            }
            else if (this.ExponentIndex >= 0 && this.VariableIndex == -1 && this.FunctionIndex >= 0)//Function power sin(x)^2
            {
                this.FunctionType = "FunctionPow";
            }
            else if (this.ExponentIndex >= 0 && this.BracketIndex >= 0 && this.VariableIndex == -1 && this.FunctionIndex == -1)//bracket power (x + 1)^2
            {
                this.FunctionType = "BracketPow";
            }
            else if (this.ExponentIndex == -1 && this.VariableIndex >= 0 && this.FunctionIndex == -1 && this.BracketIndex == -1) // pure var => x, 2x
            {
                this.FunctionType = "Variable";
            }
            else if (this.ExponentIndex == -1 && this.VariableIndex == -1 && this.FunctionIndex != -1 && this.BracketIndex == -1) // pure function => sin(x)
            {
                this.FunctionType = "Function";
            }
            else if (this.ExponentIndex == -1 && this.VariableIndex == -1 && this.FunctionIndex == -1 && this.BracketIndex != -1) // pure bracket (x + 1)
            {
                this.FunctionType = "Bracket";
            }

        }
        public Symbol(string exp)
        {
            TokenFactory tokes = new TokenFactory();
            tokes.ParseExpression(exp);
            this.Tokens = tokes.TokenList;
            Discover();
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

            string NakedString = string.Empty;
            //Product of literals of two symbols aka ax * bx => a * b
            Rational rLiteralTotal = a.Literal * b.Literal;
            Rational rExponentTotal = a.Exponent + b.Exponent;

            string strLiteralTotal = (rLiteralTotal == 1) ? "" : rLiteralTotal.ToString();
            string strExponentTotal = (rExponentTotal == 0) ? "" : rExponentTotal.ToString();
            string strExp = (rExponentTotal == 0) ? "" : "^";
            string strExpA = (a.Exponent == 0) ? "" : "^" + a.Exponent.ToString();
            string strExpB = (b.Exponent == 0) ? "" : "^" + b.Exponent.ToString();

            string strA = string.Empty;
            string strB = string.Empty;

            string strCheck = a.FunctionType + " * " + b.FunctionType;

            if (a.VariableIndex != -1 && b.VariableIndex != -1) // ax^2 * bx^2
            {
                strA = a.Tokens[a.VariableIndex].Value;
                strB = b.Tokens[b.VariableIndex].Value;
                if (strA == strB)  //x * x
                {
                    if (rExponentTotal == 0) // x * x => x^2
                    {
                        strExp = "^";
                        strExponentTotal = "2";
                    }

                    NakedString = string.Format("{0}{1}{2}{3}", strLiteralTotal, strA, strExp, strExponentTotal);
                }
                else //xy or x^2 y^2
                {
                    NakedString = string.Format("{0}{1}{2}{3}{4}", strLiteralTotal, strA, strExpA, strB, strExpB);
                }
            }

            if (NakedString != string.Empty)
            {
                combine = new Symbol(NakedString);
            }
            return combine;
        }

        public int IsPow()
        {
            int index = Tokens.FindIndex(t => t.Value == "^");
            return index;
        }

        public bool Pow(Symbol a, Symbol b, out string strCombined)
        {
            strCombined = string.Empty;
            return false;
        }
        public int IsVariable()
        {
            int index = Tokens.FindIndex(t => t.Type == "Variable");
            return index;
        }

        public int IsFunction()
        {
            int index = Tokens.FindIndex(t => t.Type == "Function");
            return index;
        }

        public int IsLeftBracket()
        {
            int index = Tokens.FindIndex(t => t.Type == "Left Parenthesis");
            return index;

        }
        public static Symbol operator *(Symbol a, Symbol b)
        {
            return Multiply(a, b);
        }
    }
}