using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;

namespace MatrixCalculus
{
    public class PermutationMatrix
    {
        private int[,] InternalRep = null;
        public string FullRep { get; set; }

        public string ElementName { get; set; }  

        public Permutation Permutation { get; }
        public int Rows { get; }
        public int Columns { get; }

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

        public PermutationMatrix(int R, int C)
        {
            this.Rows = R;
            this.Columns = C;
            InternalRep = new int[this.Rows, this.Columns];

            Zero();
        }

        public PermutationMatrix(Permutation gp)
        {
            Permutation = gp;
            this.Rows = gp.BottomRow.Length;
            this.Columns = gp.BottomRow.Length;
            InternalRep = new int[this.Rows, this.Columns];

            Zero();

            for (int k = 0; k < gp.BottomRow.Length; k++)
            {
                int colN = gp.BottomRow[k] - 1;

                InternalRep[k, colN] = 1;
            }

        }

        public string ToLatex()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\\begin{bmatrix}");
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (j < Columns - 1)
                    {
                        sb.AppendFormat("{0} &", InternalRep[i, j]);
                    }
                    else
                    {
                        sb.AppendFormat("{0}", InternalRep[i, j]);
                    }

                }
                sb.Append("\\\\");
            }

            sb.Append(" \\end{bmatrix}");
            return sb.ToString();

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    sb.AppendFormat("{0}\t", InternalRep[i, j]);
                }

                sb.Append("\r\n");

            }

            return sb.ToString();
        }

        public string ToString(string Op)
        {
            string ret = ToString();

            switch (Op)
            {
                case "L":
                    ret = ToLatex();
                    break;

                case "F":
                    ret = FullRep;
                    break;

                default:
                    break;
            }

            return ret;
        }

        public PermutationMatrix Inverse()
        {
            PermutationMatrix ret = this.Transpose();
            StringBuilder sb = new StringBuilder();

            sb.Append(this.ToLatex() + "^{-1}");
            sb.Append(" = ");
            sb.Append(ret.ToLatex());

            ret.FullRep = sb.ToString();

            return ret;
        }

        public int this[int r, int c]
        {
            get
            {
                if (!(r < Rows && c < Columns))
                {
                    throw new Exception("Rows and Columns are out of range of square matrix");
                }
                return (InternalRep[r, c]);
            }
        }

        public int Trace()
        {
            int retVal = 0;
            for (int rowCount = 0; rowCount < Rows; rowCount++)
            {
                retVal += this[rowCount, rowCount];
            }
            return retVal;
        }

        public PermutationMatrix Transpose()
        {
            PermutationMatrix ret = new PermutationMatrix(this.Rows, this.Columns);

            for (int i = 0; i < this.Columns; i++)
            {
                for (int j = 0; j < this.Rows; j++)
                {
                    ret.InternalRep[j, i] = this.InternalRep[i, j];
                }
            }
            return ret;
        }
        public static PermutationMatrix operator *(PermutationMatrix a, PermutationMatrix b)
        {
            PermutationMatrix ret = new PermutationMatrix(a.Rows, a.Columns);
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {

                    for (int k = 0; k < ret.Columns; k++)
                    {

                        ret.InternalRep[i, j] += a.InternalRep[i, k] * b.InternalRep[k, j];
                    }

                }

            }

            StringBuilder sb = new StringBuilder();

            sb.Append(a.ToLatex());
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(ret.ToLatex());

            ret.FullRep = sb.ToString();


            return ret;
        }


    }
}