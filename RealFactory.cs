using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MatrixCalculus
{
    public class RealFactory
    {
        public SquareRealMatrix this[int Rows, int Columns, params double[] exps]
        {
            get
            {
                SquareRealMatrix ret = new SquareRealMatrix(Rows, Columns);
                ret.Parent = this;
                
                int cnt = 0;

                for (int i = 0; i < Rows; i++)
                {
                    for (int j = 0; j < Columns; j++)
                    {
                        ret[i, j] = exps[cnt++];
                    }
                }

                return ret;

            }
        }
        
    }
}
