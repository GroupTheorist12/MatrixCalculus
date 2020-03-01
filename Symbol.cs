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
        private int LiteralIndex { get; set; }

        public string FunctionType { get; set; }
        private void Discover()
        {
            this.ExponentIndex = IsPow();
            this.VariableIndex = IsVariable();
            this.FunctionIndex = IsFunction();
            this.BracketIndex = IsLeftBracket();
            this.LiteralIndex = IsLiteral();

            this.Exponent = 0;
            this.Literal = 1;

            if(this.LiteralIndex != -1) //literal value
            {
                this.FunctionType = "Literal";
                this.Literal = Rational.Parse(this.Tokens[0].Value);
                return;
            }
            this.LatexString = this.NakedTokenString.Replace("*", "");

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
                    if (Tokens[this.VariableIndex - 2].Value == "-")
                    {
                        this.Literal = -1;
                    }
                    else
                    {
                        this.Literal = Rational.Parse(Tokens[this.VariableIndex - 2].Value);
                    }
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
            else if (this.ExponentIndex >= 0 && this.FunctionIndex >= 0)//Function power sin(x)^2
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
            else if (this.ExponentIndex == -1 &&  this.FunctionIndex != -1) // pure function => sin(x)
            {
                this.FunctionType = "Function";
            }
            else if (this.ExponentIndex == -1 && this.VariableIndex == -1 && this.FunctionIndex == -1 && this.BracketIndex != -1) // pure bracket (x + 1)
            {
                this.FunctionType = "Bracket";
            }

        }

        public string LatexString { get; set; }
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

        private static string ReturnFunctionString(Symbol s)
        {
            string ret = string.Empty;
            if (s.FunctionIndex != -1 && s.IsLeftBracket() != -1 && s.IsRightBracket() != -1)
            {
                int i = s.FunctionIndex;
                while (i < s.IsRightBracket() + 1)
                {
                    ret += s.Tokens[i].Value;
                    i++;
                }
            }
            return ret;
        }
        
        private static Symbol Subtract(Symbol a, Symbol b)
        {
            Symbol bMinus = new Symbol("-" + b.NakedTokenString);

            return Add(a, bMinus);
        }
        private static Symbol Add(Symbol a, Symbol b)
        {
            if(a.Tokens[0].Value == "0") //Add a  zero
            {
                return  b;
            }

            if(b.Tokens[0].Value == "0") //Add a  zero
            {
                return  a;
            }

            if(a.FunctionType == "Literal" && b.FunctionType == "Literal") //Literal + Literal
            {
                return new Symbol((a.Literal + b.Literal).ToString());
            }

            Symbol combine = new Symbol(a.NakedTokenString + " + " + b.NakedTokenString);

            string NakedString = string.Empty;
            //Product of literals of two symbols aka ax * bx => a * b
            Rational rLiteralTotal = a.Literal + b.Literal;
            Rational rExponentTotal = (a.Exponent == b.Exponent) ? a.Exponent : a.Exponent + b.Exponent;

            string strLiteralTotal = (rLiteralTotal == 1) ? "" : rLiteralTotal.ToString();
            string strExponentTotal = (rExponentTotal == 0) ? "" : rExponentTotal.ToString();
            string strExp = (rExponentTotal == 0) ? "" : "^";
            string strExpA = (a.Exponent == 0) ? "" : "^" + a.Exponent.ToString();
            string strExpB = (b.Exponent == 0) ? "" : "^" + b.Exponent.ToString();

            string strA = string.Empty;
            string strB = string.Empty;

            string strCheck = a.FunctionType + "+" + b.FunctionType;

            switch (strCheck)
            {
                case "Variable+Variable":
                    strA = a.Tokens[a.VariableIndex].Value;
                    strB = b.Tokens[b.VariableIndex].Value;
                    if (strA == strB)  //x + x
                    {
                        if(rLiteralTotal == 0) //-x + x
                        {
                            NakedString = "0";
                        }
                        else
                        {
                            NakedString = string.Format("{0}{1}", strLiteralTotal, strA);
                        }
                    
                    }       
                    break;

                case "VariablePow+VariablePow": //x^2 + x^2
                    strA = a.Tokens[a.VariableIndex].Value + strExpA;
                    strB = b.Tokens[b.VariableIndex].Value + strExpB;
                    if (strA == strB)  //x^2 + x^2
                    {
                        if(rLiteralTotal == 0) //-x^2 + x^2 == 0
                        {
                            NakedString = "0";
                        }
                        else
                        {
                            NakedString = string.Format("{0}{1}", strLiteralTotal, strA);
                        }
                    }       
                    break;

                case "Function+Function":
                    strA = ReturnFunctionString(a);
                    strB = ReturnFunctionString(b);
                    if (strA == strB)  //x^2 + x^2
                    {
                        if(rLiteralTotal == 0) //-x^2 + x^2 == 0
                        {
                            NakedString = "0";
                        }
                        else
                        {
                            NakedString = string.Format("{0}{1}", strLiteralTotal, strA);
                        }
                    }       
                    break;
                case "FunctionPow+FunctionPow":
                    strA = ReturnFunctionString(a) + strExpA;
                    strB = ReturnFunctionString(b) + strExpB;
                    if (strA == strB)  //x^2 + x^2
                    {
                        if(rLiteralTotal == 0) //-x^2 + x^2 == 0
                        {
                            NakedString = "0";
                        }
                        else
                        {
                            NakedString = string.Format("{0}{1}", strLiteralTotal, strA);
                        }
                    }       
                    break;

                default:
                    break;
            }        

            if (NakedString != string.Empty)
            {
                combine = new Symbol(NakedString);
            }

            return combine;
        }
        private static Symbol Multiply(Symbol a, Symbol b)
        {
            if(a.Tokens[0].Value == "0" || b.Tokens[0].Value == "0") //multiply by zero
            {
                return  new Symbol("0");
            }

            if(a.FunctionType == "Literal" && b.FunctionType == "Literal") //Literal * Literal
            {
                return new Symbol((a.Literal * b.Literal).ToString());
            }

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

            string strCheck = a.FunctionType + "*" + b.FunctionType;

            switch (strCheck)
            {
                case "Literal*Variable":
                case "Literal*VariablePow":
                    strB = b.Tokens[b.VariableIndex].Value;
                    NakedString = string.Format("{0}{1}{2}{3}", strLiteralTotal, strB, strExp, strExponentTotal);
                    break;

                case "Variable*Literal":
                case "VariablePow*Literal":
                    strA = a.Tokens[a.VariableIndex].Value;
                    NakedString = string.Format("{0}{1}{2}{3}", strLiteralTotal, strA, strExp, strExponentTotal);
                    break;
                case "Variable*Variable":
                case "VariablePow*VariablePow":
                case "Variable*VariablePow":
                case "VariablePow*Variable":
                    strA = a.Tokens[a.VariableIndex].Value;
                    strB = b.Tokens[b.VariableIndex].Value;
                    if (strA == strB)  //x * x
                    {
                        if (rExponentTotal == 0) // x * x => x^2
                        {
                            strExp = "^";
                            strExponentTotal = "2";
                        }
                        else 
                        {
                            if(a.Exponent != 0 && b.Exponent == 0)
                            {
                                strExponentTotal = (a.Exponent + 1).ToString();
                            }
                            else if(a.Exponent == 0 && b.Exponent != 0)
                            {
                                strExponentTotal = (b.Exponent + 1).ToString();
                            }
                        }
                        NakedString = string.Format("{0}{1}{2}{3}", strLiteralTotal, strA, strExp, strExponentTotal);
                    }
                    else //xy or x^2 y^2
                    {
                        NakedString = string.Format("{0}{1}{2}{3}{4}", strLiteralTotal, strA, strExpA, strB, strExpB);
                    }

                    break;
                case "Literal*Function":
                case "Literal*FunctionPow":
                    strB = ReturnFunctionString(b);
                    NakedString = string.Format("{0}{1}{2}{3}", strLiteralTotal, strB, strExp, strExponentTotal);
                    break;
                case "FunctionPow*Literal":
                case "Function*Literal":
                    strA = ReturnFunctionString(a);
                    NakedString = string.Format("{0}{1}{2}{3}", strLiteralTotal, strA, strExp, strExponentTotal);
                break;
                case "Function*Function":
                case "FunctionPow*FunctionPow":
                case "FunctionPow*Function":
                case "Function*FunctionPow":
                    strA = ReturnFunctionString(a);
                    strB = ReturnFunctionString(b);

                    if (strA == strB)  //sin(x) * sin(x)
                    {
                        if (rExponentTotal == 0) // sin(x) * sin(x) => sin(x)^2
                        {
                            strExp = "^";
                            strExponentTotal = "2";
                        }
                        else 
                        {
                            if(a.Exponent != 0 && b.Exponent == 0)
                            {
                                strExponentTotal = (a.Exponent + 1).ToString();
                            }
                            else if(a.Exponent == 0 && b.Exponent != 0)
                            {
                                strExponentTotal = (b.Exponent + 1).ToString();
                            }
                        }

                        NakedString = string.Format("{0}{1}{2}{3}", strLiteralTotal, strA, strExp, strExponentTotal);
                    }
                    else //xy or x^2 y^2
                    {
                        NakedString = string.Format("{0}{1}{2}*{3}{4}", strLiteralTotal, strA, strExpA, strB, strExpB);
                    }

                    break;
                default:
                    break;
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
  
        public int IsLiteral()
        {
            int index = -1;

            if(this.Tokens.Count == 1 && this.Tokens[0].Type == "Literal")
            {
                index = 0;
            }
            return index;
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
        public int IsRightBracket()
        {
            int index = Tokens.FindIndex(t => t.Type == "Right Parenthesis");
            return index;

        }

        public static Symbol operator *(Symbol a, Symbol b)
        {
            return Multiply(a, b);
        }

        public static Symbol operator +(Symbol a, Symbol b)
        {
            return Add(a, b);
        }

        public static Symbol operator -(Symbol a, Symbol b)
        {
            return Subtract(a, b);
        }

    }
}