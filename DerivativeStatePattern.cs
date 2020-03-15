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
        private static Dictionary<string, string> dicLookUp = new Dictionary<string, string>
        {
            {"a", "Literal"},
            {"X", "Variable"},
            {"aX", "LiteralOperator*Variable"},
            {"XDivLiteral", "VariableOperator / Literal"}
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
                string miKey2 = dicLookUp[miKey];
                //Console.WriteLine(miKey2);

                htTestFuncs[miKey2] = methodInfo;
            }


        }
        public static string DF(Symbol sym)
        {
            MethodInfo mi = (MethodInfo)htTestFuncs[sym.TokenString];
            if (mi != null)
            {
                return (string)mi.Invoke(null, new object[] { sym });

            }

            return sym.Expression;
        }

        public static string DF_a(Symbol sym)
        {
            if (sym.Tokens.Count > 1)
            {
                throw new Exception("Bad token");
            }

            return string.Empty;
        }

        public static string DF_X(Symbol sym)
        {
            if (sym.Tokens.Count > 1)
            {
                throw new Exception("Bad token");
            }

            return "1";
        }

        public static string DF_aX(Symbol sym)
        {
            string DerivativeString = string.Empty;
            DerivativeString = Rational.Parse(sym.Tokens[0].Value).ToString();

            return DerivativeString;
        }


    }
}
