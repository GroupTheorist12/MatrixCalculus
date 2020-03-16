using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MatrixCalculus
{
    public class DerivativeStatePattern
    {
        private static Hashtable htTestFuncs = new Hashtable();

        private static Dictionary<string, string> dicTrigFunctions = new Dictionary<string, string>
        {
            {"sin", "cos"},
            {"cos", "-sin"}
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

        private static string Poly(SymbolList symAgg)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Symbol sym in symAgg)
            {
                sb.Append(DF(sym));
            }

            //Check for literal plus literal and literal minus literal
            Symbol symCheck = new Symbol(sb.ToString());
            if (symCheck.HashTokenString == "LiteralOperator_Plus_Literal" ||
            symCheck.HashTokenString == "LiteralOperator_Minus_Literal"
            )
            {
                sb.Clear();
                sb.Append(DF(symCheck, true));
            }
            return sb.ToString();
        }
        public static string DF(Symbol sym, bool SkipPoly = false)
        {
            SymbolList symList = new SymbolList(sym.Tokens);

            if (!SkipPoly && symList.Count > 1) //Do polynomials 
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
            string strExp = (exp == 1) ? "" : "^" + exp.ToString();

            return string.Format("{0}{1}", sym.Tokens[0].Value, strExp);
        }

        public static string DF_LiteralOperatorMulVariable(Symbol sym)
        {
            string DerivativeString = string.Empty;
            DerivativeString = Rational.Parse(sym.Tokens[0].Value).ToString();

            return DerivativeString;
        }

        public static string DF_LiteralOperatorMulVariableOperatorCaretLiteral(Symbol sym)
        {
            string DerivativeString = string.Empty;
            Rational exp = Rational.Parse(sym.Tokens[4].Value) - 1;
            Rational literal = Rational.Parse(sym.Tokens[0].Value) * Rational.Parse(sym.Tokens[4].Value);

            string strExp = (exp == 1) ? "" : "^" + exp.ToString();

            DerivativeString = string.Format("{0}{1}{2}", literal.ToString(), sym.Tokens[2].Value, strExp);

            return DerivativeString;
        }

        public static string DF_VariableOperator_Div_Literal(Symbol sym)
        {
            string DerivativeString = string.Empty;
            DerivativeString = Rational.Parse("1/" + sym.Tokens[2].Value).ToString();

            return DerivativeString;
        }

        public static string DF_LiteralOperatorMulVariableOperator_Div_Literal(Symbol sym)
        {
            string DerivativeString = string.Empty;
            DerivativeString = Rational.Parse(sym.Tokens[0].Value + "/" + sym.Tokens[4].Value).ToString();

            return DerivativeString;
        }

        public static string DF_LiteralOperator_Plus_Literal(Symbol sym)
        {
            string DerivativeString = string.Empty;

            DerivativeString = (Rational.Parse(sym.Tokens[0].Value) + Rational.Parse(sym.Tokens[2].Value)).ToString();

            return DerivativeString;
        }

        public static string DF_LiteralOperator_Minus_Literal(Symbol sym)
        {
            string DerivativeString = string.Empty;

            DerivativeString = (Rational.Parse(sym.Tokens[0].Value) - Rational.Parse(sym.Tokens[2].Value)).ToString();

            return DerivativeString;
        }

        public static string DF_FunctionLeft_ParenthesisVariableRight_Parenthesis(Symbol sym)
        {
            string DerivativeString = string.Empty;
            string funcDF = sym.Tokens[0].Value.ToLower();
            if(dicTrigFunctions.ContainsKey(sym.Tokens[0].Value.ToLower()))
            {
                funcDF = dicTrigFunctions[sym.Tokens[0].Value.ToLower()];
            }

            DerivativeString = string.Format("{0}({1})", funcDF, sym.Tokens[2].Value);
            return DerivativeString;

        }

    }
}
