using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MatrixCalculus
{
    public class DerivativeStatePattern
    {
        private static Hashtable htTestFuncs = new Hashtable();

        private static Dictionary<string, string> dicFunctions = new Dictionary<string, string>
        {
            {"sin", "cos"},
            {"cos", "-sin"},
            {"tan", "sec,2"},
            {"e", "e"},
            {"ln", "1/"}
        };
        static DerivativeStatePattern()
        {
            // get all public static methods of MethodRunner type
            MethodInfo[] methodInfos = typeof(DerivativeStatePattern).GetMethods(BindingFlags.Public |
                                                                BindingFlags.Static);
            // sort methods by name
            Array.Sort(methodInfos,
                    delegate (MethodInfo methodInfo1, MethodInfo methodInfo2)
                    { return methodInfo1.Name.CompareTo(methodInfo2.Name); });

            // write method names to hash
            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (methodInfo.Name.IndexOf("DF_") == -1)
                {
                    continue;
                }

                string miKey = methodInfo.Name.Replace("DF_", "");
                //Console.WriteLine(miKey);

                htTestFuncs[miKey] = methodInfo;
            }
        }

        private static string CleanUpEnds(string Exp)
        {
            string ret = Exp.Trim();
            if(ret.LastIndexOf('+') != -1 && ret.LastIndexOf('+') == ret.Length - 1)
            {
                return ret.Substring(0, ret.Length - 1);
            }

            if(ret.LastIndexOf('-') != -1 && ret.LastIndexOf('-') == ret.Length - 1)
            {
                return ret.Substring(0, ret.Length - 1);
            }

            if(ret.LastIndexOf('*') != -1 && ret.LastIndexOf('*') == ret.Length - 1)
            {
                return ret.Substring(0, ret.Length - 1);
            }

            if(ret.LastIndexOf('/') != -1 && ret.LastIndexOf('/') == ret.Length - 1)
            {
                return ret.Substring(0, ret.Length - 1);
            }

            return ret;
        }
        private static string Poly(SymbolList symAgg)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Symbol sym in symAgg)
            {
                sb.Append(DF(sym));
            }

            // Check for literal plus literal and literal minus literal
            Symbol symCheck = new Symbol(sb.ToString());
            if (symCheck.HashTokenString == "LiteralOperator_Plus_Literal" ||
                symCheck.HashTokenString == "LiteralOperator_Minus_Literal")
            {
                sb.Clear();
                sb.Append(DF(symCheck, true));
            }
            return CleanUpEnds(sb.ToString());
        }

        public static string DF(PSymbol psym)
        {
            string exp = psym.Expression.Replace("*", "");
            return DF(new Symbol(exp), true);
        }
        public static string DF(Symbol sym, bool SkipPoly = false)
        {
            SymbolList symList = new SymbolList(sym.Tokens);

            if (!SkipPoly && symList.Count > 1) // Do polynomials 
            {
                return Poly(symList);
            }
            MethodInfo mi = (MethodInfo)htTestFuncs[sym.HashTokenString];
            if (mi != null)
            {
                return (string)mi.Invoke(null, new object[] { sym });

            }

            return sym.NakedTokenString;
        }

        public static string DF_Literal(Symbol sym)
        {
            if (sym.Tokens.Count > 1)
            {
                throw new Exception("Bad token");
            }

            return string.Empty;
        }

        public static string DF_Variable(Symbol sym)
        {
            if (sym.Tokens.Count > 1)
            {
                throw new Exception("Bad token");
            }

            return "1";
        }

        public static string DF_VariableOperatorCaretLiteral(Symbol sym)
        {
            Rational exp = Rational.Parse(sym.Tokens[2].Value) - 1;
            Rational lit = Rational.Parse(sym.Tokens[2].Value);
            string strExp = (exp == 1) ? "" : "^" + exp.ToString();

            return $"{lit}{sym.Tokens[0].Value}{strExp}";
        }

        public static string DF_LiteralOperatorMulVariable(Symbol sym)
        {
            string DerivativeString = Rational.Parse(sym.Tokens[0].Value).ToString();

            return DerivativeString;
        }

        public static string DF_LiteralOperatorMulVariableOperatorCaretLiteral(Symbol sym)
        {
            string DerivativeString = string.Empty;
            Rational exp = Rational.Parse(sym.Tokens[4].Value) - 1;
            Rational literal = Rational.Parse(sym.Tokens[0].Value) * Rational.Parse(sym.Tokens[4].Value);

            string strExp = (exp == 1) ? "" : "^" + exp.ToString();

            DerivativeString = $"{literal.ToString()}{sym.Tokens[2].Value}{strExp}";

            return DerivativeString;
        }

        public static string DF_VariableOperator_Div_Literal(Symbol sym)
        {
            string DerivativeString = Rational.Parse("1/" + sym.Tokens[2].Value).ToString();

            return DerivativeString;
        }

        public static string DF_LiteralOperatorMulVariableOperator_Div_Literal(Symbol sym)
        {
            string DerivativeString = Rational.Parse(sym.Tokens[0].Value + "/" + sym.Tokens[4].Value).ToString();

            return DerivativeString;
        }

        public static string DF_LiteralOperator_Plus_Literal(Symbol sym)
        {
            string DerivativeString = (Rational.Parse(sym.Tokens[0].Value) + Rational.Parse(sym.Tokens[2].Value)).ToString();

            return DerivativeString;
        }

        public static string DF_LiteralOperator_Minus_Literal(Symbol sym)
        {
            string DerivativeString = (Rational.Parse(sym.Tokens[0].Value) - Rational.Parse(sym.Tokens[2].Value)).ToString();

            return DerivativeString;
        }

        public static string DF_FunctionLeft_ParenthesisVariableRight_Parenthesis(Symbol sym)
        {
            string funcDF = sym.Tokens[0].Value.ToLower();
            string exp = "";
            if(dicFunctions.ContainsKey(sym.Tokens[0].Value.ToLower()))
            {
                string[] arr =  dicFunctions[sym.Tokens[0].Value.ToLower()].Split(",");
                if(arr != null && arr.Length > 1) //power
                {
                    funcDF = arr[0];
                    exp = "^" + arr[1];
                }
                else
                {
                    funcDF = dicFunctions[sym.Tokens[0].Value.ToLower()];
                }
            }

            string DerivativeString = $"{funcDF}({sym.Tokens[2].Value}){exp}";
            return DerivativeString;

        }

        //sin(x^2) => 2xcos(x^2)
        public static string DF_FunctionLeft_ParenthesisVariableOperatorCaretLiteralRight_Parenthesis(Symbol sym)
        {
            string funcDF = sym.Tokens[0].Value.ToLower();
            string exp = "";
            if(dicFunctions.ContainsKey(sym.Tokens[0].Value.ToLower()))
            {
                string[] arr =  dicFunctions[sym.Tokens[0].Value.ToLower()].Split(",");
                if(arr != null && arr.Length > 1) //power
                {
                    funcDF = arr[0];
                    exp = "^" + arr[1];
                }
                else
                {
                    funcDF = dicFunctions[sym.Tokens[0].Value.ToLower()];
                }
            }

            Symbol symInner = new Symbol();
            symInner.Tokens.Add(sym.Tokens[2]); //variable
            symInner.Tokens.Add(sym.Tokens[3]); //caret
            symInner.Tokens.Add(sym.Tokens[4]); //power

            
            string pow = DF(symInner);
            string DerivativeString = $"{pow}{funcDF}({symInner.NakedTokenString}){exp}";
            return DerivativeString;
        }
        // TODO: Holy fuckballs, what a method name! GPG. Product rule 2x^2sin(x) => 2x^2cos(x) + 4xsin(x)
        public static string DF_LiteralOperatorMulVariableOperatorCaretLiteralOperatorMulFunctionLeft_ParenthesisVariableRight_Parenthesis(Symbol sym)
        {
            string DerivativeString = sym.NakedTokenString;
            int split = 5;
            int tokenCount = 0;
            Symbol symLeft = new Symbol();

            for(tokenCount = 0; tokenCount < split; tokenCount++)
            {
                symLeft.Tokens.Add(sym.Tokens[tokenCount]);
            }
            string funcLeft = symLeft.NakedTokenString;
            string DFFuncLeft = DF(symLeft);

            Symbol symRight = new Symbol();

            for(tokenCount = split + 1; tokenCount < sym.Tokens.Count; tokenCount++)
            {
                symRight.Tokens.Add(sym.Tokens[tokenCount]);
            }
            string funcRight = symRight.NakedTokenString;
            string DFFuncRight = DF(symRight);

            StringBuilder sb = new StringBuilder();
            sb.Append(funcLeft);
            sb.Append(DFFuncRight);
            sb.Append(" + ");
            sb.Append(funcRight);
            sb.Append(DFFuncLeft);

            DerivativeString = sb.ToString();

            return DerivativeString;
        }
    }
}
