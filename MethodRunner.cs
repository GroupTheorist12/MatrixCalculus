using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
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
        public static int Test_SRM_To_Elem()
        {
            SquareRealMatrix elem = SquareRealMatrix.ElementaryMatrix(4, 4, "E22"); //E22 elementary matrix
            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(elem.ToString("F"), "Test_SRM_To_Elem.html"); //display Latex via mathjax
            return 0;
        }

        public static int Test_Columns_Of_Matrix()
        {
            List<double> initer = new List<double> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 }; //init matrix vector
            SquareRealMatrix A = new SquareRealMatrix(4, 4, initer); //create 4X4 matrix
            A.Name = "A"; //give it a name
            RealVector rv = A[".2"]; //use accessor to get column 2 of matrix
            string outR = @"\begin{aligned}&A  = " + A.ToLatex() + @" \\ \\" + "&" + rv.ToLatex("F") + @"\end{aligned}";  //use column accessor A.2 which returns column 2.          
            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(outR, "TestColumns_Of_Matrix.html"); //display Latex via mathjax
            return 0;
        }

        public static int Test_Rows_Of_Matrix()
        {
            List<double> initer = new List<double> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };//init matrix vector
            SquareRealMatrix A = new SquareRealMatrix(4, 4, initer);//create 4X4 matrix
            A.Name = "A";//give it a name
            RealVector rvRow = A["2."]; //use accessor to get row 2 of matrix
            string outR = @"\begin{aligned}&A  = " + A.ToLatex() + @" \\ \\" + "&" + rvRow.ToLatex("F") + @"\end{aligned}";  //use row accessor A2. which returns row 2.          
            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(outR, "TestRows_Of_Matrix.html"); //display Latex via mathjax
            return 0;
        }

        public static int Test_Partioned_Matrix()
        {
            List<double> initer = new List<double> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };//init matrix vector
            SquareRealMatrix A = new SquareRealMatrix(4, 4, initer);//create 4X4 matrix
            A.Name = "A";//give it a name
            SquareRealMatrix AColumns = new SquareRealMatrix(new List<RealVector> { A[".1"], A[".2"], A[".3"], A[".4"] }); //Create partioned matrix from columns
            SquareRealMatrix ARows = new SquareRealMatrix(new List<RealVector> { A["1."], A["2."], A["3."], A["4."] }); //Create partioned matrix from rows

            StringBuilder sb = new StringBuilder();//Start building latex
            sb.Append(@"\begin{aligned}");
            sb.AppendFormat(@"&A = &{0} \\ \\", A.ToLatex()); //Display Original A matrix
            sb.Append(@"&A = \left[ A_{.1}\;A_{.2}\;A_{.3}\;A_{.4} \right] = \;\;" + "&" + AColumns.ToLatex() + @" \\ \\");//partioned matrix via columns of A
            sb.Append(@"&\left[ A_{1.}\;A_{2.}\;A_{3.}\;A_{4.} \right] = \;\;" + "&" + ARows.ToLatex() + @" \\ \\");//partioned matrix via rows of A
            sb.Append(@"&A = \left[ A_{1.}\;A_{2.}\;A_{3.}\;A_{4.} \right]' = \;\;" + "&" + ARows.Transpose().ToLatex() + @" \\ \\"); //transpose to get A
            sb.Append(@"\end{aligned}");
            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_Partioned_Matrix.html"); //display Latex via mathjax
            return 0;
        }

        public static int Test_UnitVector_Col_Row_Accessors()
        {
            List<double> initer = new List<double> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };//init matrix vector
            SquareRealMatrix A = new SquareRealMatrix(4, 4, initer);//create 4X4 matrix
            A.Name = "A";//give it a name

            UnitVectorSpace uvs = new UnitVectorSpace(4); //Create unit vector space to use
            UnitVector e2 = new UnitVector("e2", 4, RowColumn.Column);
            UnitVector e2_p = new UnitVector("e2", 4, RowColumn.Row);

            RealVector rv = A * e2;
            StringBuilder sb = new StringBuilder();//Start building latex
            sb.Append(@"\begin{aligned}");
            sb.AppendFormat(@"&A = &{0} \\ \\", A.ToLatex()); //Display Original A matrix
            sb.Append(@"&A_{.2} = &" + rv.ToLatex() + @" \\ \\");
            RealVector rvRow = e2_p * A;
            sb.Append(@"&A_{2.}' = &" + rvRow.ToLatex());

            sb.Append(@"\end{aligned}");
            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_UnitVector_Col_Row_Accessors.html"); //display Latex via mathjax
            return 0;
        }

        public static int Test_Symbol_Groups()
        {
            SymbolMatrix smK = SymbolMatrixUtilities.KleinGroup(); //Get kleingroup cayle table    
            SymbolMatrix smQL = SymbolMatrixUtilities.LeftChiralQuaternion();
            SymbolMatrix smQR = SymbolMatrixUtilities.RightChiralQuaternion();

            StringBuilder sb = new StringBuilder();//Start building latex
            sb.Append(@"\begin{aligned}");
            sb.AppendFormat(@"&{0} \\ \\", smK.ToLatex("F"));
            sb.AppendFormat(@"&{0} \\ \\", smQL.ToLatex("F"));
            sb.AppendFormat(@"&{0}", smQR.ToLatex("F"));
            sb.Append(@"\end{aligned}");

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_KleinGroup.html"); //display Latex via mathjax
            return 0;
        }

        public static int Test_Flip()
        {
            SymbolMatrix C3Flip = SymbolMatrixUtilities.C3().Flip().ReName(new List<string> { "d", "e", "f" });
            StringBuilder sb = new StringBuilder();//Start building latex
            sb.Append(@"\begin{aligned}");
            sb.AppendFormat(@"&{0} \\ \\", SymbolMatrixUtilities.C3().ToLatex());
            sb.AppendFormat(@"&{0}", C3Flip.ToLatex());
            sb.Append(@"\end{aligned}");

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_Flip.html"); //display Latex via mathjax

            return 0;
        }

        public static int Test_KroneckerSum()
        {
            RealFactory rf = new RealFactory();
            SquareRealMatrix A = rf[2, 2,
                    1, -1,
                    0, 2
            ];

            SquareRealMatrix B = rf[2, 2,
                    1, 0,
                    2, -1
            ];

            StringBuilder sb = new StringBuilder();//Start building latex
            sb.Append(@"\begin{aligned}");

            sb.AppendFormat(@"&A = {0}\;B = {1}", A.ToLatex(), B.ToLatex() + @" \\ \\");
            sb.AppendFormat(@"&C = A\;\oplus\;B = {0}", SquareRealMatrix.KroneckerSum(A, B).ToLatex());

            sb.Append(@"\end{aligned}");

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_KroneckerSum.html"); //display Latex via mathjax

            return 0;
        }
        public static int Test_KroneckerProduct()
        {
            SymbolMatrix A1 = new SymbolMatrix(2, 2, //create a 2 X 2 symbol matrix with symbols a,b,c,d
            new List<Symbol>
            {
                new Symbol("a"), new Symbol("b"),
                new Symbol("c"), new Symbol("d")
            });

            SymbolMatrix A2 = new SymbolMatrix(2, 2, //create a 2 X 2 symbol matrix with symbols e,f,g,h
            new List<Symbol>
            {
                new Symbol("e"), new Symbol("f"),
                new Symbol("g"), new Symbol("h")
            });
            StringBuilder sb = new StringBuilder();//Start building latex
            sb.AppendFormat(@"{0}", SymbolMatrixUtilities.KroneckerProduct(A1, A2).ToLatex("F"));

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_KroneckerProduct.html"); //display Latex via mathjax
            return 0;
        }
        public static int Test_C3()
        {
            SymbolMatrix C3 = SymbolMatrixUtilities.C3(new List<string> { "x", "y", "z" });
            StringBuilder sb = new StringBuilder();//Start building latex
            sb.AppendFormat(@"{0}", C3.ToLatex());

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_C3.html"); //display Latex via mathjax
            return 0;
        }

        public static int Test_ES()
        {
            EinsteinSummation es = new EinsteinSummation(3);
            //Symbol s = es["a{ij}x_ix_j"];
            List<ITensor> lstT = es.ExpandToList("a_{ij}x_ix_j");

            string s = es.FormatQuadratic(lstT);
            
            //HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(s.NakedTokenString, "Test_ES.html"); //display Latex via mathjax
            //Console.WriteLine(s);

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(s, "Test_ES.html"); //display Latex via mathjax

            //Console.WriteLine(p.Tensor.Expand());
            return 0;
        }
        public static int Test_D3()
        {
            SymbolMatrix D3 = SymbolMatrixUtilities.D3();
            StringBuilder sb = new StringBuilder();//Start building latex
            sb.AppendFormat(@"{0}", D3.ToLatex());

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_D3.html"); //display Latex via mathjax

            return 0;
        }

        public static int Test_MultiplyRationalSymbolMatrix()
        {
            SymbolFactory sf = new SymbolFactory(SymbolType.Rational);

            SymbolMatrix A1 = sf[2, 2, //create a 2 X 2 symbol matrix with rationals
                "1/2", "1/5",
                "2/3", "7/8"
            ];


            SymbolMatrix A2 = sf[2, 2, //create a 2 X 2 symbol with rationals
                "1/4", "7/9",
                "3/10", "4/7"
            ];

            SymbolMatrix symMul = A1 * A2; //multiply the matrices
            StringBuilder sb = new StringBuilder();//Start building latex
            sb.AppendFormat(@"{0}", symMul.ToLatex("F"));

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_Multiply_Rational_Symbol_Matrix.html"); //display Latex via mathjax
            return 0;
        }

        public static int Test_TraceFunction()
        {
            RealFactory rf = new RealFactory(); //create a real factory

            SquareRealMatrix A = rf[3, 3, //create a 3 X 3 real matrix
            1, 2, 3,
            7, 8, 9,
            6, 5, 4
            ];

            StringBuilder sb = new StringBuilder();//Start building latex
            sb.Append(@"\begin{aligned}");

            sb.AppendFormat(@"&A = {0}", A.ToLatex() + @" \\ \\");//display A matrix
            sb.AppendFormat(@"&tr A = {0}", A.Trace() + @" \\ \\"); //display trace
            sb.Append(@"\end{aligned}");

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_TraceFunction.html"); //display Latex via mathjax

            return 0;
        }

        public static int Test_RationalSquareMatrix()
        {
            RationalFactory rf = new RationalFactory();
            RealFactory rff = new RealFactory();

            RationalSquareMatrix A = rf[3, 3,
            "1", "2", "-2",
            "-1", "1", "3",
            "2", "-1", "2"
            ];

            SquareRealMatrix SA = rff[3, 3,
            7, 2, -2,
            3, 1, 3,
            8, -1, 2
            ];
            RationalVector rv = rf["7", "3", "8"];

            StringBuilder sb = new StringBuilder();//Start building latex
            sb.Append(@"\begin{aligned}");

            sb.AppendFormat(@"&A = {0}", A.ToLatex() + @" \\ \\");//display A matrix
            A[0] = rv;
            sb.AppendFormat(@"&Ab = {0}", A.ToLatex()); //display Vec A
            sb.AppendFormat(@"&Det = {0}", SA.Determinant()); //display Vec A
            sb.Append(@"\end{aligned}");

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_VecOperator.html"); //display Latex via mathjax

            return 0;
        }
        public static int Test_VecOperator()
        {
            RealFactory rf = new RealFactory(); //create a real factory

            SquareRealMatrix A = rf[2, 2, //create a 2 X 2 real matrix
            1, 2,
            3, 4
            ];

            RealVector VecA = A.Vec("A"); //use Vec operator giving the matrix a name
            StringBuilder sb = new StringBuilder();//Start building latex
            sb.Append(@"\begin{aligned}");

            sb.AppendFormat(@"&A = {0}", A.ToLatex() + @" \\ \\");//display A matrix
            sb.AppendFormat(@"&{0}", VecA.ToLatex("F")); //display Vec A
            sb.Append(@"\end{aligned}");

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_VecOperator.html"); //display Latex via mathjax

            return 0;

        }
        public static int Test_RationalSymbolMatrixOperations()
        {
            SymbolFactory sf = new SymbolFactory(SymbolType.Rational);

            SymbolMatrix A1 = sf[2, 2, //create a 2 X 2 symbol matrix with rationals
                "1/2", "1/5",
                "2/3", "7/8"
            ];


            SymbolMatrix A2 = sf[2, 2, //create a 2 X 2 symbol with rationals
                "1/4", "7/9",
                "3/10", "4/7"
            ];

            SymbolMatrix symMul = A1 * A2; //multiply the matrices
            StringBuilder sb = new StringBuilder();//Start building latex
            sb.Append(@"\begin{aligned}");

            sb.AppendFormat(@"&{0}", (A1 * A2).ToLatex("F") + @" \\ \\");
            sb.AppendFormat(@"&{0}", (A1 + A2).ToLatex("F") + @" \\ \\");
            sb.AppendFormat(@"&{0}", (A1 - A2).ToLatex("F"));

            sb.Append(@"\end{aligned}");

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_RationalSymbolMatrixOperations.html"); //display Latex via mathjax
            return 0;
        }

        public static int Test_ParseSymbols()
        {
            TokenFactory tokes = new TokenFactory();
            //TokenFactory tokes = new TokenFactory();
            tokes.Variables.Add("x");

            SymbolFactory sf = new SymbolFactory(SymbolType.Expression, tokes);

            Symbol sym = sf["e(xy^2)"];
            Console.WriteLine(sym.Expression);
            Console.WriteLine(sym.HashTokenString);
            //Console.WriteLine(ArithmeticStatePattern.Add(sym));

            //Console.WriteLine(DerivativeStatePattern.DF(sym));

            /*
            tokes.ParseExpression("2xsin(2x)");
            int cnt = 0;
            int j = 0;
            for (j = 0; j < tokes.TokenList.Count; j++)
            {
                Token t = tokes.TokenList[j];
                Console.WriteLine("{0}. Type = {1}, value = {2}, symbol end {3}", cnt++, t.Type, t.Value, t.SymbolEnd);
                                    
            }

            
            Symbol sym = tokes.DF(tokes.symbolList[0]);

            for (j = 0; j < sym.Tokens.Count; j++)
            {
                Token t = sym.Tokens[j];
                Console.WriteLine("{0}. Type = {1}, value = {2}, symbol end {3}", cnt++, t.Type, t.Value, t.SymbolEnd);
                                    
            }
            
            */


            return 0;
        }

        public static int Test_CramersRule()
        {
            RationalFactory rf = new RationalFactory();//Create rational factory
            RationalSquareMatrix ACopy = rf[3, 3, //Matrix to solve
            "1", "2", "-2",
            "-1", "1", "3",
            "2", "-1", "2"
            ];

            RationalVector rv = new RationalVector{-7, 3, 8}; //values to solve for
            StringBuilder sb = new StringBuilder();//Start building latex
            sb.Append(@"\begin{aligned}");

            //Start system of linear equations
            sb.AppendFormat(@"&x_1 + 2x_2 - 2x_3 = -7 \\ \\");
            sb.AppendFormat(@"&-x_1 + x_2 + 3x_3 = 3 \\ \\");
            sb.AppendFormat(@"&2x_1 - x_2 + 2x_3 = 8 \\ \\");
            RationalVector rvSolved = ACopy.CramersRule(rv); //solve system of linear equations

            sb.AppendFormat(@"&x_1 = {0}, x_2 = {1}, x_3 = {2}", rvSolved[0].ToLatex(), rvSolved[1].ToLatex(), rvSolved[2].ToLatex()); //output values

            sb.Append(@"\end{aligned}");

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_CramersRule.html"); //display Latex via mathjax

            return 0;
        }

        public static int Test_EMatrix()
        {
            RationalFactory rf = new RationalFactory();
            RationalSquareMatrix I = RationalSquareMatrix.IdentityMatrix(3);


            RationalSquareMatrix ACopy = rf[3, 3,
            "1", "2", "-2",
            "-1", "1", "3",
            "2", "-1", "2"
            ];

            RationalSquareMatrix ACopy2 = rf[3, 3,
            "-7", "2", "-2",
            "-3", "1", "3",
            "8", "-1", "2"
            ];

                 RationalSquareMatrix ACopy3 = rf[4, 4,

"4", "7", "2", "3",
"1", "3", "1", "2",
"2", "5", "3", "4",
"1", "4", "2", "3"
];
            string ll = RationalSquareMatrix.DetFullRep(ACopy);
            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(ll, "Test_EMatrix.html"); //display Latex via mathjax

            return 0;
        }

        public static int Test_LiteDB()
        {
            DFStore.CreateTestCases();
            return 0;
        }
        public static int Test_Permutations()
        {

            SymetricGroup sg = new SymetricGroup(4);
            sg.DisplayMatrix = true;

            PermutationList ps2 = new PermutationList(4);
            ps2.DisplayMatrix = true;            

            PermutationList ps = new PermutationList();
            ps.Order = 4;

            ps.Add(
                new Permutation(
                    new int[] {1,2,3,4}, 
                    new int[] {1,2,3,4}
                    )
            );

            ps.Add(
                new Permutation(
                    new int[] {1,2,3,4}, 
                    new int[] {4,1,2,3}
                    )
            );

            ps.Add(
                new Permutation(
                    new int[] {1,2,3,4}, 
                    new int[] {3,4,1,2}
                    )
            );
            ps.Add(
                new Permutation(
                    new int[] {1,2,3,4}, 
                    new int[] {2,3,4,1}
                    )
            );

            ps.Add(
                new Permutation(
                    new int[] {1,2,3,4}, 
                    new int[] {1,2,3,4}
                    )
            );

            ps.Add(
                new Permutation(
                    new int[] {1,2,3,4}, 
                    new int[] {2,1,4,3}
                    )
            );

            ps.Add(
                new Permutation(
                    new int[] {1,2,3,4}, 
                    new int[] {4,3,1,2}
                    )
            );
            ps.Add(
                new Permutation(
                    new int[] {1,2,3,4}, 
                    new int[] {3,4,2,1}
                    )
            );

            ps.Add(
                new Permutation(
                    new int[] {1,2,3,4}, 
                    new int[] {1,2,3,4}
                    )
            );

            ps.Add(
                new Permutation(
                    new int[] {1,2,3,4}, 
                    new int[] {3,1,4,2}
                    )
            );

            ps.Add(
                new Permutation(
                    new int[] {1,2,3,4}, 
                    new int[] {2,4,1,3}
                    )
            );
            ps.Add(
                new Permutation(
                    new int[] {1,2,3,4}, 
                    new int[] {4,3,2,1}
                    )
            );


            ps.DisplayMatrix = true;

            ps = ps2.CyclicPermutations();
            ps.DisplayMatrix = true;
            ps.Order = 4;

            ps = new PermutationList();
            ps.Order = 4;
            ps.DisplayMatrix = true;
            ps.AddRange(ps2.OrderBy(o => o.ElementNameValue).ToList());

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(ps.ToLatex(), "Test_Permutations.html"); //display Latex via mathjax

            return 0;
        }
        public static int Test_Rational_Determinant()
        {
            RationalFactory rf = new RationalFactory();

            RationalSquareMatrix A = rf[3, 3,
            "1", "2", "-2",
            "-1", "1", "3",
            "2", "-1", "2"
            ];

                 RationalSquareMatrix ACopy3 = rf[4, 4,
                "4", "7", "2", "3",
                "1", "3", "1", "2",
                "2", "5", "3", "4",
                "1", "4", "2", "3"
                ];

            List<RationalCoFactorInfo> cfList = RationalSquareMatrix.GetAllMatrixCoFactors(ACopy3);
            
            StringBuilder sb = new StringBuilder();//Start building latex
            sb.Append(@"\begin{aligned}");
            sb.AppendFormat(@"&{0} \\ \\", ACopy3.ToLatex());

            //foreach(IGrouping<string, RationalCoFactorInfo> funk in q.ToList())
            foreach (RationalCoFactorInfo ci in cfList)
            {

                sb.AppendFormat(@"&{0} \\ \\", ci.Minor.MinorName + " = " + ((ci.Sign < 0) ? "-" : "") + ci.CoFactor.ToLatex() + ci.Minor.ToLatex());

                foreach (List<RationalCoFactorInfo> lstChild in ci.ListOfLists)
                {
                    foreach (RationalCoFactorInfo ci2 in lstChild)
                    {
                        //if (ci2.Minor.Rows == 4)
                        {

                            sb.AppendFormat(@"&{0} \\ \\", ci.Minor.MinorName + " = " + ((ci.Sign < 0) ? "-" : "") + ci.CoFactor.ToLatex() + ci2.CoFactor.ToLatex() + ci2.Minor.ToLatex());
                        }
                    }

                }
            }
            sb.Append(@"&Det = " + RationalSquareMatrix.Det(ACopy3));
            sb.Append(@"\end{aligned}");

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_Rational_Determinant.html"); //display Latex via mathjax


            return 0;
        }
        public static int Test_Symbol_Determinant()
        {
            //SymbolMatrix C3 = SymbolMatrixUtilities.C3RowColumn();
            //Symbol det = C3.Determinant();
            //Symbol det = SymbolMatrixUtilities.C4RowColumn().Determinant();
            //SymbolMatrix smK = SymbolMatrixUtilities.KleinGroup(); //Get kleingroup cayle table    
            //Symbol det = smK.Determinant();


            SymbolMatrix smK = SymbolMatrixUtilities.C3RowColumn();
            List<CoFactorInfo> cfList = SymbolMatrix.GetAllMatrixCoFactors(smK);
            StringBuilder sb = new StringBuilder();//Start building latex
            sb.Append(@"\begin{aligned}");
            sb.AppendFormat(@"&{0} \\ \\", smK.ToLatex());

            /*
            int inc = 0;
            CoFactorInfo cfi = null;

            while(inc < cfList.Count)
            {
                if(cfi == null)
                {
                    cfi = cfList[inc];
                }
                else if(cfi != null && cfi.ListOfLists.Count == 0) //init value
                {
                    List<CoFactorInfo> cfListChild = SymbolMatrix.GetCoFactors(cfi.Minor);
                    cfi.ListOfLists.Add(cfListChild);

                    if(cfListChild[0].Minor.Rows == 2) //end of line
                    {
                        cfList[inc] = cfi;
                        cfi = null;
                        inc++;
                    }    
                }
                else if(cfi != null && cfi.ListOfLists.Count > 0) //have values
                {
                    List<CoFactorInfo> cfListChild = SymbolMatrix.GetCoFactors(cfi.Minor);
                    cfi.ListOfLists.Add(cfListChild);

                    if(cfListChild[0].Minor.Rows == 2) //end of line
                    {
                        cfList[inc] = cfi;
                        cfi = null;
                        inc++;
                    }    

                }
            }

            */


            foreach (CoFactorInfo ci in cfList)
            {

                sb.AppendFormat(@"&{0} \\ \\", ci.CoFactor.Tokens[0].Value + ci.Minor.ToLatex());
                /*
                if (ci.Minor.Rows > 2)
                {
                    List<CoFactorInfo> cfList2 = SymbolMatrix.GetCoFactors(ci.Minor);

                    foreach (CoFactorInfo ci2 in cfList2)
                    {
                        sb.AppendFormat(@"&{0} \\ \\", ci.CoFactor.Tokens[0].Value + ci2.CoFactor.Tokens[0].Value + ci2.Minor.ToLatex());
                    }
                }
                */

                foreach (List<CoFactorInfo> lstChild in ci.ListOfLists)
                {
                    foreach (CoFactorInfo ci2 in lstChild)
                    {
                        //if (ci2.Minor.Rows == 4)
                        {

                            sb.AppendFormat(@"&{0} \\ \\", ci.CoFactor.Tokens[0].Value + ci2.CoFactor.Tokens[0].Value + ci2.Minor.ToLatex());
                            /*
                            string det = string.Format("({0} - {1})", 
                            ci2.Minor[0, 0].Tokens[0]. Value + ci2.Minor[1, 1].Tokens[0]. Value,
                            ci2.Minor[1, 0].Tokens[0]. Value + ci2.Minor[0, 1].Tokens[0]. Value);

                            sb.AppendFormat(@"&{0} \\ \\", ci.CoFactor.Tokens[0].Value + ci2.CoFactor.Tokens[0].Value + det);
                            */
                        }
                    }

                }
            }
            sb.Append(@"\end{aligned}");

            HtmlOutputMethods.WriteLatexEqToHtmlAndLaunch(sb.ToString(), "Test_Symbol_Determinant.html"); //display Latex via mathjax


            return 0;
        }

        public static int Test_RowColumnExpressions()
        {
            RowColumnExpression rc1 = new RowColumnExpression(4, "a11");
            RowColumnExpression rc2 = new RowColumnExpression(4, "a11");

            RowColumnExpression ret = rc1 * rc2;
            Console.WriteLine("{0}", ret.Expression);
            return 0;
        }
        public static int Test_Symbols_Tokens()
        {


            string[] funcs =
            {
                "x",
                "2x",
                "x^2",
                "2x^2",
                "2x^2",
                "sin(x)",
                "sin(x)^2",
                "2sin(x)",
                "2sin(x)^2",
                "(x+1)^2"
            };
            /* TODO: remove this altogether? GPG
                        foreach (string FunctionString in funcs)
                        {
                            TokenFactory toke = new TokenFactory();
                            toke.ParseExpression(FunctionString);
                            foreach (Symbol sym in toke.symbolList)
                            {
                                Console.WriteLine("{0}", sym.NakedTokenString);
                                Console.WriteLine("{0}", sym.TokenString);
                            }

                            //string FunctionString = "x^2+x";
                            //Tokenizer toke = new Tokenizer();
                            //List<Token> tokes = toke.tokenizeToSymbol(FunctionString);

                            //StringBuilder sb = new StringBuilder();
                            //StringBuilder sb2 = new StringBuilder();

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


                        }
            */

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
            /*
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            int cnt = 0;

            TokenFactory tf = new TokenFactory();
            tf.ParseExpression("-2sin(x + 1)/x^2");

            for (int i = 0; i < tf.TokenList.Count; i++)
            {
                Token t = tf.TokenList[i];

                sb.Append(t.Type);
                if (t.Type == "Operator")
                {
                    sb.Append(t.Value);
                }

                Console.WriteLine("{0}. Type = {1}, value = {2}, Symbol End {3}, current {4}", cnt, t.Type, t.Value, t.SymbolEnd, sb.ToString());
                sb2.Append((cnt).ToString() + "\t");
                cnt++;
            }

            /*
            Console.WriteLine(@"{0}", sb.ToString());
            Console.WriteLine("//{0}", sb2.ToString());
            */


            Symbol symA = new Symbol("15x^2 - 5x");
            Symbol symB = new Symbol("-5x^2");
            Symbol symM = symA;
            Console.WriteLine("{0} {1}", symM.TokenString, symM.NakedTokenString);





            return 0;
        }
    }
}