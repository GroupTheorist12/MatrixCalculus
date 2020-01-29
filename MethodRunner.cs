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
        }


        public static int Test_RealVector_ColumnVector()
        {
            RealVector rv = new RealVector { 1, 2, 3, 4 };

            HtmlOutputMethods.WriteLatexToHtmlAndLaunch(rv.ToLatex(), "Test_RealVector_ColumnVector.html");

            return 0;
        }

    }
}