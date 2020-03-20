using System;

namespace MatrixCalculus
{
    public class RationalFactory
    {
        public RationalSquareMatrix this[int Rows, int Columns, params string[] exps]
        {
            get
            {
                RationalSquareMatrix ret = new RationalSquareMatrix(Rows, Columns);
                ret.Parent = this;

                int cnt = 0;

                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        ret[i, j] = Rational.Parse(exps[cnt++]);
                    }
                }

                return ret;

            }
        }
        
        public RationalVector this[params string[] exps]
        {
            get
            {
                RationalVector rv = new RationalVector();
                foreach(string r in exps)
                {
                    rv.Add(Rational.Parse(r));
                }

                return rv;
            }
        }
    }
}
