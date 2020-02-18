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
    public class MethodRunner
    {
        private static Hashtable htTestFuncs = new Hashtable();

        public static int RunIt(string hashEntry)
        {
            MethodInfo mi = (MethodInfo)htTestFuncs[hashEntry];
            return (int)mi.Invoke(null, null);
        }

        static MethodRunner()
        {

            // get all public static methods of MethodRunner type
            MethodInfo[] methodInfos = typeof(MethodRunner).GetMethods(BindingFlags.Public |
                                                                BindingFlags.Static);
            // sort methods by name
            Array.Sort(methodInfos,
                    delegate (MethodInfo methodInfo1, MethodInfo methodInfo2)
                    { return methodInfo1.Name.CompareTo(methodInfo2.Name); });

            // write method names to hash
            foreach (MethodInfo methodInfo in methodInfos)
            {
                if (methodInfo.Name.IndexOf("Test_") == -1)
                {
                    continue;
                }

                string miKey = methodInfo.Name.Replace("Test_", "");
                //Console.WriteLine(miKey);

                htTestFuncs[miKey] = methodInfo;
            }


        }


        public static int Test_RealVector_ColumnVector()
        {
            RealVector rv = new RealVector { 1, 2, 3, 4 };

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(rv.ToLatex(), "Test_RealVector_ColumnVector.html");

            return 0;
        }

        public static int Test_UnitVector_UnitVector()
        {
            UnitVector uv = new UnitVector("e1", 3);
            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(uv.ToLatex("F"), "Test_UnitVector_UnitVector.html");

            return 0;

        }

        public static int Test_UnitVectorSpace_UnitVectorSpace()
        {
            UnitVectorSpace uvs = new UnitVectorSpace(3);
            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(uvs.ToLatex(), "Test_UnitVectorSpace_UnitVectorSpace.html");

            return 0;

        }

        public static int Test_OneVector_OneVector()
        {
            UnitVectorSpace uvs = new UnitVectorSpace(3);
            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(uvs.OneVector().ToLatex("F"), "Test_OneVector_OneVector.html");

            return 0;

        }

        public static int Test_ElementaryMatrix_ElementaryMatrix()
        {
            ElementaryMatrix em = new ElementaryMatrix(4, 4, "E11");
            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(em.ToLatex("F"), "Test_ElementaryMatrix_ElementaryMatrix.html");

            return 0;

        }

        public static int Test_UnitVectorProductTo_ElementaryMatrix()
        {
            UnitVectorSpace uvs = new UnitVectorSpace(4); //Order four Unit Vector Space
            UnitVector e2 = uvs["e2"]; // order 4 e2 unit vector
            UnitVector e1 = uvs["e1"]; //order 4 e1 unit operator
            ElementaryMatrix em = e2 * e1; //product of Unit Vectors
            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(em.ToLatex("F"), "Test_UnitVectorProductTo_ElementaryMatrix.html"); //display Latex via mathjax

            return 0;

        }

        public static int Test_UnitVectorProduct_ToInt()
        {
            UnitVectorSpace uvs = new UnitVectorSpace(4); //Order four Unit Vector Space
            UnitVector e1 = uvs["e1"]; //order 4 e1 unit operator
            e1.IsRowOrColumn = RowColumn.Row;

            UnitVector e2 = uvs["e2"]; // order 4 e2 unit vector
            int dp = e1 * e2; //product of Unit Vectors

            UnitVector e2prime = new UnitVector("e2", 4, RowColumn.Row);

            int dp2 = e2prime * e2;

            string latex = "e'_{1}e_{2} = " + dp.ToString() + @"\;,e'_{2}e_{2} = " + dp2.ToString();

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(latex, "Test_UnitVectorProduct_ToInt.html"); //display Latex via mathjax


            return 0;

        }

        public static int Test_ElementaryMatrix_Multiply_UnitVector()
        {
            /*
            Eij * er = ei*e'j*er = &jr *ei where & = Kronecker delta

            */
            StringBuilder sb = new StringBuilder();
            ElementaryMatrix E12 = new ElementaryMatrix(4, 4, "E12"); // 4 x 4 ElementaryMatrix
            UnitVector e1 = new UnitVector("e1", 4); //Order four unit vector e1
            UnitVector e2 = new UnitVector("e2", 4, RowColumn.Row); //Order four row unit vector e2
            UnitVector er = new UnitVector("e3", 4); //Order four unit vector e3 our er above
            /*
            i = 1, j = 2, r = 3
            */

            //Test matrix * unit vector, Eij * er
            UnitVector uvE1 = E12 * er;
            sb.Append(@"E_{12}e_3 = " + uvE1.ToLatex() + @"\;");

            //Test e1* e'j * er
            UnitVector uvE2 = e1 * e2 * er;
            sb.Append(@"e_1e'_2e_3 = " + uvE2.ToLatex() + @"\;");

            //Test &jr * ei
            UnitVector uvE3 = UnitVector.KroneckerDelta(2, 3) * e1;
            sb.Append(@" \delta_{2 3}e_1 = " + uvE3.ToLatex());

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_ElementaryMatrix_Multiply_UnitVector.html"); //display Latex via mathjax


            return 0;

        }

        public static int Test_UnitVector_Multiply_ElementaryMatrix()
        {
            /*
            e'r * Eij = e'r * ei * e'j = &ri *e'j where & = Kronecker delta

            */
            StringBuilder sb = new StringBuilder();
            ElementaryMatrix E12 = new ElementaryMatrix(4, 4, "E12"); // 4 x 4 ElementaryMatrix
            UnitVector e1 = new UnitVector("e1", 4); //Order four unit vector e1
            UnitVector e2 = new UnitVector("e2", 4, RowColumn.Row); //Order four row unit vector e2
            UnitVector er = new UnitVector("e3", 4, RowColumn.Row); //Order four unit row vector e3 our er above
            /*
            i = 1, j = 2, r = 3
            */

            sb.Append(@"e'_rE_{ij} = e'_re_ie'_j = \delta_{r j}e'j \\");
            //Test matrix unit vector * Elementary Matrix,  er * Eij
            UnitVector uvE1 = er * E12;
            sb.Append(@"e'_3E_{12} = " + uvE1.ToLatex() + @"\;");

            //Test e'r * ei * e'j 
            UnitVector uvE2 = er * e1 * e2;
            sb.Append(@"e'_3e_1e'_2 = " + uvE2.ToLatex() + @"\;");

            //Test &ri * e'j
            UnitVector uvE3 = UnitVector.KroneckerDelta(3, 1) * e2;
            sb.Append(@" \delta_{3 1}e_2 = " + uvE3.ToLatex() + @" \tag{1}");

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_UnitVector_Multiply_ElementaryMatrix.html"); //display Latex via mathjax

            return 0;

        }

        //From now on, our comments will use latex syntax
        public static int Test_ElementaryMatrix_Multiply_ElementaryMatrix()
        {
            /*
                E_{ij}E_{rs} = e_ie'_je_re'_s = \delta_{r j}e'_ie'_s = \delta_{j r}E_{is} \\
                if\;r = j \\
                E_{ij}E_{rs} = \delta_{j j}E_{is} = E_{is}

            */
            StringBuilder sb = new StringBuilder();
            ElementaryMatrix E12 = new ElementaryMatrix(4, 4, "E13"); // 4 x 4 ElementaryMatrix
            ElementaryMatrix E13 = new ElementaryMatrix(4, 4, "E32"); // 4 x 4 ElementaryMatrix

            sb.Append(@"E_{ij}E_{rs} = e_ie'_je_re'_s = \delta_{r j}e'_ie'_s = \delta_{j r}E_{is} \\");
            sb.Append(@"if\;r = j \\");
            sb.Append(@"E_{ij}E_{rs} = \delta_{j j}E_{is} = E_{is} \\");
            sb.Append(@"Output\;is: \\");
            //Test Elementary Matrix * Elementary Matrix

            ElementaryMatrix em = E12 * E13;
            sb.Append(em.ToLatex("F"));

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_ElementaryMatrix_Multiply_ElementaryMatrix.html"); //display Latex via mathjax

            return 0;

        }

        private static void FindVariable(string Expr)
        {
            Regex regEx = new Regex(@"[+-]?((\d+(\.\d*)?)|(\.\d+))?\w[xyzt]\^[+-]?((\d+(\.\d*)?)|(\.\d+))");

            Match m = regEx.Match(Expr, 0);

            if (m.Success)
            {
                Console.WriteLine("Found power variable times literal");
                return;
            }

            regEx = new Regex(@"[xyzt]\^[+-]?((\d+(\.\d*)?)|(\.\d+))");
            m = regEx.Match(Expr, 0);
            if (m.Success)
            {
                Console.WriteLine("Found naked power variable");
                return;
            }

        }
        public static int Test_Symbols_Tokens()
        {

            
            string[] funcs =
            {
                "x*x",
                "2x*x",
                "x*2x",
                "x*x^2",
                "2x^2",
                "2x*sin(x^2)"
            };

            foreach (string FunctionString in funcs)
            {
                //string FunctionString = "x^2+x";
                Tokenizer toke = new Tokenizer();
                List<Token> tokes = toke.tokenizeToSymbol(FunctionString);

                StringBuilder sb = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();

                int cnt = 0;

                /*    
                for (int i = 0; i < tokes.Count; i++)
                {
                    Token t = tokes[i];

                    sb.Append(t.Type);
                    //sb2.Append((cnt++).ToString() + "\t");
                    if (t.Type == "Operator")
                    {
                        sb.Append(t.Value);
                    }

                    if(t.SymbolEnd)
                    {
                        //Console.WriteLine("{0}. Type = {1}, value = {2}, Symbol End {3}, current {4}", cnt++, t.Type, t.Value, t.SymbolEnd, sb.ToString());
                        //Console.WriteLine("case \"{0}\":", sb.ToString());
                        //Console.WriteLine("//{0}", sb2.ToString());
                        //Console.WriteLine("\tbreak;");

                    }

                }
                */
                
            }
            

            /*
            SymbolList symL = new SymbolList(tokes);
            foreach(Symbol sym in symL)
            {
                Console.WriteLine("{0}", sym.NakedTokenString);
                Console.WriteLine("{0}", sym.TokenString);
            }
            

            
            Symbol symA = new Symbol("2x");
            Symbol symB = new Symbol("5x");

            Symbol symM = symA * symB;
            Console.WriteLine("{0} {1}", symM.TokenString, symM.NakedTokenString);
            */



            //FindVariable("2x^2");

            return 0;
        }
    }
}