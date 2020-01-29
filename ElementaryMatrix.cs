using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MatrixCalculus
{
    public class ElementaryMatrix
    {
        private int[,] InternalRep = null;
        private string m_FullRep = string.Empty;

        public int Rows = 0;
        public int Columns = 0;
        private List<double> Vector = null;
        private void Zero()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    InternalRep[i, j] = 0;
                }
            }

        }
        public ElementaryMatrix(int rows, int columns)
        {

            this.Rows = rows;
            this.Columns = columns;
            InternalRep = new int[this.Rows, this.Columns];

            Zero();

        }
        
        public int this[int r, int c]
        {
            get
            {
                if (!(r < Rows && c < Columns))
                {
                    throw new Exception("rows and columns out of range of square matrix");
                }
                return (InternalRep[r, c]);
            }
            set { InternalRep[r, c] = value; }
        }


    }
}