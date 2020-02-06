using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;

namespace MatrixCalculus
{
    public class MethodRunner
    {
        public static Hashtable htTestFuncs = new Hashtable();

        public static int RunIt(string hashEntry)
        {
            TestRunner<int> test = (TestRunner<int>)htTestFuncs[hashEntry];
            return test.Value;
        }

        static MethodRunner()
        {
            htTestFuncs["ColumnVector"] = new TestRunner<int>(() => Test_RealVector_ColumnVector());
            htTestFuncs["UnitVector_UnitVector"] = new TestRunner<int>(() => Test_UnitVector_UnitVector());
            htTestFuncs["UnitVectorSpace_UnitVectorSpace"] = new TestRunner<int>(() => Test_UnitVectorSpace_UnitVectorSpace());
            htTestFuncs["OneVector_OneVector"] = new TestRunner<int>(() => Test_OneVector_OneVector());
            htTestFuncs["ElementaryMatrix_ElementaryMatrix"] = new TestRunner<int>(() => Test_ElementaryMatrix_ElementaryMatrix());
            htTestFuncs["UnitVectorProductTo_ElementaryMatrix"] = new TestRunner<int>(() => Test_UnitVectorProductTo_ElementaryMatrix());
            htTestFuncs["UnitVectorProduct_ToInt"] = new TestRunner<int>(() => Test_UnitVectorProduct_ToInt());

            
                        
                        

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

    }
}