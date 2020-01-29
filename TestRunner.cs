using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;

namespace MathematicalAlgorithmsDotNetCore
{
    class TestRunner<T> where T : struct
    {
        private Nullable<T> val;
        private Func<T> getValue;

        // Constructor.
        public TestRunner(Func<T> func)
        {
            val = null;
            getValue = func;
        }

        public T Value
        {
            get
            {
                if (val == null)
                    // Execute the delegate.
                    val = getValue();
                return (T)val;
            }
        }
    }
}