using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixCalculus
{
    public class Symbol
    {
        public List<Token> Tokens { get; set; }

        public bool IsExpression { get; set; }
        public bool IsRational { get; set; }
        public bool IsOperator { get; set; }

        public string Name{get; set;}

        public Symbol()
        {
            Tokens = new List<Token>();
            IsOperator = false;
            IsExpression = false;
        }

        private Rational Exponent { get; set; }
        private Rational Literal { get; set; }

        private int ExponentIndex { get; set; }
        private int VariableIndex { get; set; }
        private int FunctionIndex { get; set; }
        private int BracketIndex { get; set; }
        private int LiteralIndex { get; set; }

        public string FunctionType { get; set; }
        public string Expression { get; set; }

        public void Discover()
        {
            ExponentIndex = IsPow();
            VariableIndex = IsVariable();
            FunctionIndex = IsFunction();
            BracketIndex = IsLeftBracket();
            LiteralIndex = IsLiteral();

            Exponent = 0;
            Literal = 1;

            if (TryRational())
            {
                return;
            }
            if (LiteralIndex != -1) // Literal value
            {
                FunctionType = "Literal";
                Literal = Rational.Parse(Tokens[0].Value);
                LatexString = Tokens[0].Value;
                return;
            }
            LatexString = NakedTokenString.Replace("*", "");

            if (ExponentIndex >= 0) // pow => ^
            {
                // Literal after ^
                if (ExponentIndex < Tokens.Count - 1 && Tokens[ExponentIndex + 1].Type == "Literal")
                {
                    Exponent = Rational.Parse(Tokens[ExponentIndex + 1].Value); // Literal value should be first token after ^
                }
            }

            if (VariableIndex >= 0) // Variable x^2 or x or 2*x or 2*x^2
            {
                if (VariableIndex - 2 >= 0 && Tokens[VariableIndex - 2].Type == "Literal") // Literal times variable such as 2*x^2
                {
                    if (Tokens[VariableIndex - 2].Value == "-")
                    {
                        Literal = -1;
                    }
                    else
                    {
                        Literal = Rational.Parse(Tokens[VariableIndex - 2].Value);
                    }
                }
                else // Naked variable aka x^2
                {
                    Literal = 1;
                }

            }

            if (FunctionIndex >= 0) // Function like sin(x), sin(x)^2 2*sin(x)
            {
                if (FunctionIndex - 2 >= 0 && Tokens[FunctionIndex - 2].Type == "Literal") // Literal times function such as 2*sin(x)^2
                {
                    Literal = Rational.Parse(Tokens[FunctionIndex - 2].Value);
                }
                else //naked variable aka x^2
                {
                    Literal = 1;
                }
            }

            //Variable Pow x^2
            if (ExponentIndex >= 0 && VariableIndex >= 0 && FunctionIndex == -1 && BracketIndex == -1)
            {
                FunctionType = "VariablePow";
            }
            else if (ExponentIndex >= 0 && FunctionIndex >= 0) // Function power sin(x)^2
            {
                FunctionType = "FunctionPow";
            }
            else if (ExponentIndex >= 0 && BracketIndex >= 0 && VariableIndex == -1 && FunctionIndex == -1) // Bracket power (x + 1)^2
            {
                FunctionType = "BracketPow";
            }
            else if (ExponentIndex == -1 && VariableIndex >= 0 && FunctionIndex == -1 && BracketIndex == -1) // Pure var => x, 2x
            {
                FunctionType = "Variable";
            }
            else if (ExponentIndex == -1 && FunctionIndex != -1) // Pure function => sin(x)
            {
                FunctionType = "Function";
            }
            else if (ExponentIndex == -1 && VariableIndex == -1 && FunctionIndex == -1 && BracketIndex != -1) // pure bracket (x + 1)
            {
                FunctionType = "Bracket";
            }

        }

        public string LatexString { get; set; }

        public SymbolFactory Parent { get; set; }
        public Symbol(string exp)
        {
            TokenFactory tokes = new TokenFactory();
            Expression = exp;
            tokes.ParseExpression(exp);
            Tokens = tokes.TokenList;
            symbolType = SymbolType.Expression;
            Discover();
            IsOperator = false;
            IsExpression = false;

            if (Parent == null)
            {
                Parent = new SymbolFactory(SymbolType.Expression);
            }
        }

        private SymbolType m_symbolType = SymbolType.Expression;
        public SymbolType symbolType
        {
            get
            {
                return m_symbolType;
            }
            set
            {
                m_symbolType = value;
            }
        }

        private bool TryRational()
        {
            bool retVal = false;
            try
            {
                Rational rExp = Rational.Parse(Expression);
                symbolType = SymbolType.Rational;
                LatexString = rExp.ToLatex();
                retVal = true;
            }
            catch
            {

            }

            return retVal;
        }
        public string NakedTokenString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int tokenCount = 0; tokenCount < Tokens.Count; tokenCount++)
                {
                    Token t = Tokens[tokenCount];
                    sb.Append(t.Value);

                }
                return sb.ToString();
            }
        }

        public string HashTokenString
        {
            get
            {
                return TokenString.
                Replace(" ", "_").
                Replace("*", "Mul").
                Replace("/", "Div").
                Replace("^", "Caret").
                Replace("+", "Plus").
                Replace("-", "Minus");

            }
        }
        public string TokenString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int tokenCount = 0; tokenCount < Tokens.Count; tokenCount++)
                {
                    Token t = Tokens[tokenCount];
                    sb.Append(t.Type);
                    if (t.Type == "Operator")
                    {
                        sb.Append(t.Value);
                    }

                }
                return sb.ToString();
            }
        }

        private static string ReturnFunctionString(Symbol s)
        {
            string retVal = string.Empty;
            if (s.FunctionIndex != -1 && s.IsLeftBracket() != -1 && s.IsRightBracket() != -1)
            {
                int i = s.FunctionIndex;
                while (i < s.IsRightBracket() + 1)
                {
                    retVal += s.Tokens[i].Value;
                    i++;
                }
            }
            return retVal;
        }

        private static Symbol Subtract(Symbol a, Symbol b)
        {

            return Add(a, b, true);
        }
        private static Symbol Add(Symbol a, Symbol b, bool Subtract = false)
        {
            if (a.Tokens[0].Value == "0") //Add a zero
            {
                return b;
            }

            if (b.Tokens[0].Value == "0") //Add a zero
            {
                return a;
            }

            if (a.FunctionType == "Literal" && b.FunctionType == "Literal") // Literal + Literal
            {
                return new Symbol((a.Literal + b.Literal).ToString());
            }

            Symbol combine = new Symbol(a.NakedTokenString + ((Subtract) ? " - " : " + ") + b.NakedTokenString);

            if (combine.NakedTokenString.IndexOf("+-") != -1)
            {
                string fixIt = combine.NakedTokenString.Replace("+-", " - ");
                combine = new Symbol(fixIt);
            }
            string NakedString = string.Empty;

            // Product of literals of two symbols aka ax * bx => a * b
            Rational rLiteralTotal = (Subtract) ? (a.Literal - b.Literal) : (a.Literal + b.Literal);
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
                    if (strA == strB)  // x + x
                    {
                        if (rLiteralTotal == 0) // -x + x
                        {
                            NakedString = "0";
                        }
                        else
                        {
                            NakedString = string.Format("{0}{1}", strLiteralTotal, strA);
                        }

                    }
                    break;

                case "VariablePow+VariablePow": // x^2 + x^2
                    strA = a.Tokens[a.VariableIndex].Value + strExpA;
                    strB = b.Tokens[b.VariableIndex].Value + strExpB;
                    if (strA == strB)  // x^2 + x^2
                    {
                        if (rLiteralTotal == 0) // -x^2 + x^2 == 0
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
                    if (strA == strB)  // x^2 + x^2
                    {
                        if (rLiteralTotal == 0) // -x^2 + x^2 == 0
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
                    if (strA == strB)  // x^2 + x^2
                    {
                        if (rLiteralTotal == 0) // -x^2 + x^2 == 0
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

        private static Symbol DivideRational(Symbol a, Symbol b)
        {
            Rational inter = Rational.Parse(a.Expression) / Rational.Parse(b.Expression);
            Symbol sym = a.Parent[inter.ToString()];
            sym.LatexString = inter.ToLatex();

            return sym;
        }

        private static Symbol MultiplyRational(Symbol a, Symbol b)
        {
            Rational inter = Rational.Parse(a.Expression) * Rational.Parse(b.Expression);
            Symbol sym = a.Parent[inter.ToString()];
            sym.LatexString = inter.ToLatex();

            return sym;
        }

        private static Symbol AddRational(Symbol a, Symbol b)
        {
            Rational inter = Rational.Parse(a.Expression) + Rational.Parse(b.Expression);
            Symbol sym = a.Parent[inter.ToString()];
            sym.LatexString = inter.ToLatex();

            return sym;
        }

        private static Symbol SubtractRational(Symbol a, Symbol b)
        {
            Rational inter = Rational.Parse(a.Expression) - Rational.Parse(b.Expression);
            Symbol sym = a.Parent[inter.ToString()];
            sym.LatexString = inter.ToLatex();

            return sym;
        }

        private static Symbol Multiply(Symbol a, Symbol b)
        {
            if(a.Parent.FactorySymbolType == SymbolType.Rational && b.Parent.FactorySymbolType == SymbolType.Rational)
            {
                return MultiplyRational(a, b);
            }
            if (a.Tokens[0].Value == "0" || b.Tokens[0].Value == "0") //multiply by zero
            {
                return new Symbol("0");
            }

            if (a.FunctionType == "Literal" && b.FunctionType == "Literal") //Literal * Literal
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
                            if (a.Exponent != 0 && b.Exponent == 0)
                            {
                                strExponentTotal = (a.Exponent + 1).ToString();
                            }
                            else if (a.Exponent == 0 && b.Exponent != 0)
                            {
                                strExponentTotal = (b.Exponent + 1).ToString();
                            }
                        }
                        NakedString = string.Format("{0}{1}{2}{3}", strLiteralTotal, strA, strExp, strExponentTotal);
                    }
                    else // xy or x^2 y^2
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

                    if (strA == strB)  // sin(x) * sin(x)
                    {
                        if (rExponentTotal == 0) // sin(x) * sin(x) => sin(x)^2
                        {
                            strExp = "^";
                            strExponentTotal = "2";
                        }
                        else
                        {
                            if (a.Exponent != 0 && b.Exponent == 0)
                            {
                                strExponentTotal = (a.Exponent + 1).ToString();
                            }
                            else if (a.Exponent == 0 && b.Exponent != 0)
                            {
                                strExponentTotal = (b.Exponent + 1).ToString();
                            }
                        }

                        NakedString = string.Format("{0}{1}{2}{3}", strLiteralTotal, strA, strExp, strExponentTotal);
                    }
                    else // xy or x^2 y^2
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

            if (Tokens.Count == 1 && Tokens[0].Type == "Literal")
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

        public static Symbol operator /(Symbol a, Symbol b)
        {
            if(a.Parent.FactorySymbolType == SymbolType.Rational && b.Parent.FactorySymbolType == SymbolType.Rational)
            {
                return DivideRational(a, b);
            }
            
            if (a.IsExpression && b.IsExpression)
            {
                return new Symbol(a.NakedTokenString + " * " + b.NakedTokenString);
            }
            return Multiply(a, b);
        }

        public static Symbol operator *(Symbol a, Symbol b)
        {
            if (a.IsExpression && b.IsExpression)
            {
                return new Symbol(a.NakedTokenString + " * " + b.NakedTokenString);
            }
            return Multiply(a, b);
        }

        public static Symbol operator +(Symbol a, Symbol b)
        {
            if(a.Parent.FactorySymbolType == SymbolType.Rational && b.Parent.FactorySymbolType == SymbolType.Rational)
            {
                return AddRational(a, b);
            }
            
            if (a.IsExpression && b.IsExpression)
            {
                return new Symbol(a.NakedTokenString + " + " + b.NakedTokenString);
            }
            return Add(a, b);
        }

        public static Symbol operator -(Symbol a, Symbol b)
        {
            if(a.Parent.FactorySymbolType == SymbolType.Rational && b.Parent.FactorySymbolType == SymbolType.Rational)
            {
                return SubtractRational(a, b);
            }

            if (a.IsExpression && b.IsExpression)
            {
                return new Symbol(a.NakedTokenString + " - " + b.NakedTokenString);
            }

            return Subtract(a, b);
        }
    }
}