using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MatrixCalculus
{
    // I assume the following Main() declaration is test code; I would remove but cannot be sure GPG.
    /*
    using System.CodeDom.Compiler;
    using System.Reflection;
    using System;
    public class J
    {
        public static void Main()
        {       
            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = "AutoGen.dll";

            CompilerResults r = CodeDomProvider.CreateProvider("CSharp").CompileAssemblyFromSource(parameters, "public class B {public static int k=7;}");

            //verify generation
            Console.WriteLine(Assembly.LoadFrom("AutoGen.dll").GetType("B").GetField("k").GetValue(null));
        }
    }
    */
    public class ArithmeticStatePattern
    {
        private static Hashtable htTestFuncs = new Hashtable();

        public static void CompileArithmetic()
        {
            System.CodeDom.Compiler.CompilerParameters parameters = new CompilerParameters();
        }

        static ArithmeticStatePattern()
        {
            MethodInfo[] methodInfos = typeof(ArithmeticStatePattern).GetMethods(BindingFlags.Public |
                                                                BindingFlags.Static);
            // sort methods by name
            Array.Sort(methodInfos,
                    delegate (MethodInfo methodInfo1, MethodInfo methodInfo2)
                    { return methodInfo1.Name.CompareTo(methodInfo2.Name); });

            // write method names to hash
            foreach (MethodInfo methodInfo in methodInfos)
            {
                List<string> ArithmeticOps = new List<string>() { "Add_", "Multiply_", "Subtract_", "Divide_" };

                int ind = ArithmeticOps.FindIndex(a => methodInfo.Name.StartsWith(a));
                if (ind == -1)
                {
                    continue;
                }

                string miKey = methodInfo.Name.Replace(ArithmeticOps[ind], "");

                htTestFuncs[miKey] = methodInfo;
            }
        }

        public static string Add(Symbol sym, bool SkipPoly = false)
        {
            SymbolList symList = new SymbolList(sym.Tokens);

            if (!SkipPoly && symList.Count > 1) // Do polynomials 
            {
                //return Poly(symList);     //TODO:Work in progress, maybe?  GPG
            }
            MethodInfo mi = (MethodInfo)htTestFuncs[sym.HashTokenString];
            if (mi != null)
            {
                return (string)mi.Invoke(null, new object[] { sym });
            }

            return sym.NakedTokenString;
        }

        public static string AddVars(Symbol sym)
        {
            string ret = string.Empty;
            string LeftL = string.Empty;
            string RightL = string.Empty;
            string LeftV = string.Empty;
            string RightV = string.Empty;
            List<string> LitBuf = new List<string>();
            List<string> VarBuf = new List<string>();

            for (int i = 0; i < sym.Tokens.Count; i++)
            {
                Token t = sym.Tokens[i];

                if (t.Type == "Literal")
                {
                    LitBuf.Add(t.Value);
                }
                else if (t.Type == "Variable")
                {
                    VarBuf.Add(t.Value);
                }
            }
            return ret;
        }
        public static string Add_VariableOperator_Plus_Variable(Symbol sym)
        {
            string FunctionString = sym.NakedTokenString;

            if (sym.Tokens[0].Value == sym.Tokens[2].Value)
            {
                FunctionString = string.Format("2{0}", sym.Tokens[0].Value);
            }
            return FunctionString;
        }

        public static string Add_VariableOperator_Minus_Variable(Symbol sym)
        {
            string FunctionString = sym.NakedTokenString;

            if (sym.Tokens[0].Value == sym.Tokens[2].Value)
            {
                FunctionString = "0";
            }
            return FunctionString;
        }

        public static string Add_LiteralOperatorMulVariableOperator_Plus_Variable(Symbol sym)
        {
            string AddString = sym.Expression;

            return AddString;
        }

        public static string Add_VariableOperator_Plus_LiteralOperatorMulVariable(Symbol sym)
        {
            string AddString = sym.Expression;

            return AddString;
        }

        public static string Add_LiteralOperatorMulVariableOperator_Plus_LiteralOperatorMulVariable(Symbol sym)
        {
            string AddString = sym.Expression;

            return AddString;
        }

        public static string Add_VariableOperatorCaretLiteralOperator_Plus_VariableOperatorCaretLiteral(Symbol sym)
        {
            string AddString = sym.Expression;

            return AddString;
        }

        public static string Add_LiteralOperatorMulVariableOperatorCaretLiteralOperator_Plus_VariableOperatorCaretLiteral(Symbol sym)
        {
            string AddString = sym.Expression;

            return AddString;
        }

        public static string Add_VariableOperatorCaretLiteralOperator_Plus_LiteralOperatorMulVariableOperatorCaretLiteral(Symbol sym)
        {
            string AddString = sym.Expression;

            return AddString;
        }

        public static string Add_LiteralOperatorMulVariableOperatorCaretLiteralOperator_Plus_LiteralOperatorMulVariableOperatorCaretLiteral(Symbol sym)
        {
            string AddString = sym.Expression;

            return AddString;
        }


    }
}
