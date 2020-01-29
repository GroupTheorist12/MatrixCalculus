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
            

        }


        public static int Test_RealVector_ColumnVector()
        {
            RealVector rv = new RealVector { 1, 2, 3, 4 };

            HtmlOutputMethods.WriteLatexToHtmlAndLaunch(rv.ToLatex(), "Test_RealVector_ColumnVector.html");

            return 0;
        }

        public static int Test_UnitVector_UnitVector()
        {
            UnitVector uv = new UnitVector("e1", 3);
            HtmlOutputMethods.WriteLatexToHtmlAndLaunch(uv.ToLatex("F"), "Test_UnitVector_UnitVector.html");

            return 0;

        }

        public static int Test_UnitVectorSpace_UnitVectorSpace()
        {
            UnitVectorSpace uvs = new UnitVectorSpace(3);
            HtmlOutputMethods.WriteLatexToHtmlAndLaunch(uvs.ToLatex(), "Test_UnitVectorSpace_UnitVectorSpace.html");

            return 0;

        }
    }
}