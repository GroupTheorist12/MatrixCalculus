using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;

namespace MathematicalAlgorithmsDotNetCore
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
            htTestFuncs["SetStream"] = new TestRunner<int>(() => Test_SetStream());
        }

        protected class BigIntegerSetStream : SetStream<BigInteger>
        {
            public BigIntegerSetStream(BigInteger val) : base(val)
            {
                value = val;
            }

            public override SetStream<BigInteger> next
            {
                get
                {
                    return new BigIntegerSetStream(++value);
                }
            }

            public override List<BigInteger> takeUntil(BigInteger n, List<BigInteger> acc = null)
            {
                List<BigInteger> accumulator = (acc == null) ? new List<BigInteger>() : acc;

                if (n < this.value)
                {
                    return (List<BigInteger>)null;
                }

                if (n == this.value)
                {
                    return accumulator;
                }


                accumulator.Add(this.value);
                return this.next.takeUntil(n, accumulator);

            }

        }

        public static int Test_SetStream()
        {
            List<BigInteger> set = (new BigIntegerSetStream(10)).takeUntil(20);


            for (int i = 0; i < set.Count(); i++)
            {
                if (i == 0)
                {
                    Console.Write("{0}{1}", "{", set[i]);
                }
                else if (i > 0 && i <= set.Count() - 2)
                {
                    Console.Write(",{0}", set[i]);

                }
                else if (i == set.Count() - 1)
                {
                    Console.Write(",{0}{1}", set[i], "}");
                }
            }
            return 0;
        }

        public static int Test_RealVector_ColumnVector()
        {
            RealVector rv = new RealVector { 1, 2, 3, 4 };

            HtmlOutputMethods.WriteLatexToHtmlAndLaunch(rv.ToLatex(), "Test_RealVector_ColumnVector.html");

            return 0;
        }

    }
}