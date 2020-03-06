using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace MatrixCalculus
{
    public class SquareRealMatrix
    {
        private double[,] InternalRep = null;
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

        private void FromVector()
        {
            int cnt = 0;
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    InternalRep[i, j] = Vector[cnt++];
                }
            }
        }

        public static string MultipliedVectorLatex(SquareRealMatrix A, double[] VectorIn, double[] VectorOut)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(A.ToLatex());
            sb.Append(VectorToLatex(VectorIn));
            sb.Append(" = ");
            sb.Append(VectorToLatex(VectorOut));

            return sb.ToString();

        }
        public double[] MultiplyVector(double[] VectorIn)
        {
            List<double> ret = new List<double>();
            if (VectorIn.Length != this.Rows)
            {
                throw new Exception("Vector length must be same as number of columns and rows of matrix");
            }

            for (int i = 0; i < Rows; i++)
            {
                double SumOfRow = 0;
                for (int j = 0; j < Columns; j++)
                {
                    SumOfRow += (InternalRep[i, j] * VectorIn[j]);
                }

                ret.Add(SumOfRow);
            }

            return ret.ToArray();
        }

        private int SignOfElement(int i, int j)
        {
            if ((i + j) % 2 == 0)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        public SquareRealMatrix(List<RealVector> rvList)
        {
            this.Rows = rvList[0].Count;
            this.Columns = rvList.Count;

            InternalRep = new double[this.Rows, this.Columns];

            Zero();
            for (int i = 0; i < this.Columns; i++)
            {
                int rows = rvList[i].Count;
                if (rows != this.Columns)
                {
                    throw new Exception("rows and columns must be equal for square matrix");

                }

                this[i] = rvList[i];
            }
        }
        public SquareRealMatrix(double[,] RC)
        {
            int rows = RC.GetLength(0);
            int columns = RC.GetLength(1);

            if (rows != columns)
            {
                throw new Exception("rows and columns must be equal for square matrix");

            }

            this.Rows = rows;
            this.Columns = columns;
            InternalRep = RC;
        }

        //this method determines the sub matrix corresponding to a given element
        private double[,] CreateSmallerMatrix(double[,] input, int i, int j)
        {
            int order = int.Parse(System.Math.Sqrt(input.Length).ToString());
            double[,] output = new double[order - 1, order - 1];
            int x = 0, y = 0;
            for (int m = 0; m < order; m++, x++)
            {
                if (m != i)
                {
                    y = 0;
                    for (int n = 0; n < order; n++)
                    {
                        if (n != j)
                        {
                            output[x, y] = input[m, n];
                            y++;
                        }
                    }
                }
                else
                {
                    x--;
                }
            }
            return output;
        }

        public double Determinant()
        {
            return Determinant(this.InternalRep);
        }

        private double Determinant(double[,] input)
        {
            int order = int.Parse(System.Math.Sqrt(input.Length).ToString());
            if (order > 2)
            {
                double value = 0;
                for (int j = 0; j < order; j++)
                {
                    double[,] Temp = CreateSmallerMatrix(input, 0, j);
                    value = value + input[0, j] * (SignOfElement(0, j) * Determinant(Temp));
                }
                return value;
            }
            else if (order == 2)
            {
                return ((input[0, 0] * input[1, 1]) - (input[1, 0] * input[0, 1]));
            }
            else
            {
                return (input[0, 0]);
            }
        }

        public SquareRealMatrix(int rows, int columns)
        {
            if (rows != columns)
            {
                throw new Exception("rows and columns must be equal for square matrix");

            }

            this.Rows = rows;
            this.Columns = columns;
            InternalRep = new double[this.Rows, this.Columns];

            Zero();
        }

        public string Name { get; set; }
        public string LatexName { get; private set; }
        public static SquareRealMatrix ElementaryMatrix(int rows, int columns, string Name)
        {
            SquareRealMatrix ret = new SquareRealMatrix(rows, columns);
            ret.Name = Name;

            if (ret.Name[0] != 'E')
            {
                throw new Exception("Name of ElementaryMatrix must begin with capitol E followed by two numer indices");
            }

            int oI1 = 0;
            int oI2 = 0;

            try
            {
                if (!int.TryParse(ret.Name[1].ToString(), out oI1))
                {
                    throw new Exception("Name of ElementaryMatrix must begin with capitol E followed by two numer indices, index 1 bad.");

                }

                if (!int.TryParse(ret.Name[2].ToString(), out oI2))
                {
                    throw new Exception("Name of ElementaryMatrix must begin with capitol E followed by two numer indices, index 2 bad");

                }

                ret[oI1 - 1, oI2 - 1] = 1;
                ret.LatexName = @"E_{" + (oI1).ToString() + (oI2).ToString() + "}";
            }
            catch (Exception)
            {
                throw new Exception("Name of ElementaryMatrix must begin with capitol E followed by two numer indices. Could not parse indices.");
            }


            ret.FullRep = ret.LatexName + @"\;=\;" + ret.ToLatex();
            return ret;
        }
        public SquareRealMatrix(int rows, int columns, List<double> V)
        {
            if (rows != columns)
            {
                throw new Exception("rows and columns must be equal for square matrix");

            }

            if (V.Count % rows != 0)
            {
                throw new Exception("Vector does not contain even row count");
            }


            Vector = V;

            this.Rows = rows;
            this.Columns = columns;
            InternalRep = new double[this.Rows, this.Columns];

            FromVector();
        }

        public SquareRealMatrix Inverse()
        {
            double[][] m = HelpFunctions.DoubleArrayFromDouble(this.InternalRep);
            double[][] inv = HelpFunctions.MatrixInverse(m);

            double[,] mi = HelpFunctions.DoubleFromDoubleArray(inv);

            return new SquareRealMatrix(mi);
        }


        public static SquareRealMatrix operator +(SquareRealMatrix a, SquareRealMatrix b)
        {
            SquareRealMatrix ret = new SquareRealMatrix(a.Rows, a.Columns);

            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {

                    ret.InternalRep[i, j] = a.InternalRep[i, j] + b.InternalRep[i, j];

                }

            }

            StringBuilder sb = new StringBuilder();

            sb.Append("$$");
            sb.Append(a.ToLatex());
            sb.Append(" + ");
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(ret.ToLatex());
            sb.Append("$$");

            ret.m_FullRep = sb.ToString();
            return ret;
        }

        public static SquareRealMatrix operator +(SquareRealMatrix a, double b)
        {
            SquareRealMatrix ret = new SquareRealMatrix(a.Rows, a.Columns);

            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {

                    ret.InternalRep[i, j] = a.InternalRep[i, j] + b;

                }

            }

            StringBuilder sb = new StringBuilder();

            sb.Append("$$");
            sb.Append(a.ToLatex());
            sb.Append(" + ");
            sb.Append(b.ToString());
            sb.Append(" = ");
            sb.Append(ret.ToLatex());
            sb.Append("$$");

            ret.m_FullRep = sb.ToString();
            return ret;
        }

        public static SquareRealMatrix operator +(double b, SquareRealMatrix a)
        {
            SquareRealMatrix ret = new SquareRealMatrix(a.Rows, a.Columns);

            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {

                    ret.InternalRep[i, j] = a.InternalRep[i, j] + b;

                }

            }

            StringBuilder sb = new StringBuilder();

            sb.Append("$$");
            sb.Append(a.ToLatex());
            sb.Append(" + ");
            sb.Append(b.ToString());
            sb.Append(" = ");
            sb.Append(ret.ToLatex());
            sb.Append("$$");

            ret.m_FullRep = sb.ToString();
            return ret;
        }

        public static SquareRealMatrix operator -(SquareRealMatrix a, SquareRealMatrix b)
        {
            SquareRealMatrix ret = new SquareRealMatrix(a.Rows, a.Columns);

            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {

                    ret.InternalRep[i, j] = a.InternalRep[i, j] - b.InternalRep[i, j];

                }

            }

            StringBuilder sb = new StringBuilder();

            sb.Append("$$");
            sb.Append(a.ToLatex());
            sb.Append(" + ");
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(ret.ToLatex());
            sb.Append("$$");

            ret.m_FullRep = sb.ToString();
            return ret;
        }

        public static SquareRealMatrix operator -(SquareRealMatrix a, double b)
        {
            SquareRealMatrix ret = new SquareRealMatrix(a.Rows, a.Columns);

            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {

                    ret.InternalRep[i, j] = a.InternalRep[i, j] - b;

                }

            }

            StringBuilder sb = new StringBuilder();

            sb.Append("$$");
            sb.Append(a.ToLatex());
            sb.Append(" - ");
            sb.Append(b.ToString());
            sb.Append(" = ");
            sb.Append(ret.ToLatex());
            sb.Append("$$");

            ret.m_FullRep = sb.ToString();
            return ret;
        }

        public static SquareRealMatrix operator -(double b, SquareRealMatrix a)
        {
            SquareRealMatrix ret = new SquareRealMatrix(a.Rows, a.Columns);

            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {

                    ret.InternalRep[i, j] = b - a.InternalRep[i, j];

                }

            }

            StringBuilder sb = new StringBuilder();

            sb.Append("$$");
            sb.Append(b.ToString());
            sb.Append(" - ");
            sb.Append(a.ToLatex());
            sb.Append(" = ");
            sb.Append(ret.ToLatex());
            sb.Append("$$");

            ret.m_FullRep = sb.ToString();
            return ret;
        }


        public static SquareRealMatrix operator *(SquareRealMatrix a, SquareRealMatrix b)
        {
            SquareRealMatrix ret = new SquareRealMatrix(a.Rows, a.Columns);

            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {

                    for (int k = 0; k < ret.Columns; k++)
                    {

                        ret.InternalRep[i, j] += a.InternalRep[i, k] * b.InternalRep[k, j];
                        double sens = ret.InternalRep[i, j];

                        if (Math.Abs(sens) < 1.0e-8d) //f**ked up value set to zero
                        {
                            ret.InternalRep[i, j] = 0;
                        }
                    }

                }



            }

            StringBuilder sb = new StringBuilder();

            sb.Append("$$");
            sb.Append(a.ToLatex());
            sb.Append(" \\cdot ");
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(ret.ToLatex());
            sb.Append("$$");

            ret.m_FullRep = sb.ToString();


            return ret;
        }

        public RealVector this[string ColumnsOrRows]
        {
            get
            {
                RealVector ret = new RealVector();
                if (ColumnsOrRows.Length != 2 && ColumnsOrRows.IndexOf(".") == -1)
                {
                    throw new Exception("Bad indexer. Should by .j or i. with i or j being numeric such as .1 or 2.");
                }

                RowOrColumn rc = new RowOrColumn();
                try
                {
                    if (ColumnsOrRows[0] == '.') //Column
                    {
                        rc.rowColumn = RowColumn.Column;
                        rc.Val = int.Parse(ColumnsOrRows[1].ToString()) - 1;
                    }
                    else//row
                    {
                        rc.rowColumn = RowColumn.Row;
                        rc.Val = int.Parse(ColumnsOrRows[0].ToString()) - 1;
                    }
                }
                catch
                {
                    throw new Exception("Bad indexer. Should by .j or i. with i or j being numeric such as .1 or 2.");

                }
                ret = this[rc];
                ret.FullRep = this.Name + "_" + ColumnsOrRows + @"\;=\;" + ret.ToLatex();
                ret.IsRowOrColumn = rc.rowColumn;
                return ret;
            }
        }
        public RealVector this[RowOrColumn rc]
        {
            get
            {
                RealVector ret = new RealVector();

                if (rc.rowColumn == RowColumn.Column)
                {
                    ret = this[rc.Val];
                }
                else
                {
                    for (int i = 0; i < this.Columns; i++)
                    {
                        ret.Add(InternalRep[rc.Val, i]);
                    }

                }
                return ret;
            }
            set
            {
                if (rc.rowColumn == RowColumn.Column)
                {
                    this[rc.Val] = value;
                }
                else
                {
                    for (int i = 0; i < this.Columns; i++)
                    {
                        InternalRep[rc.Val, i] = value[i];
                    }

                }

            }
        }
        public RealVector this[int Column]
        {
            get
            {
                RealVector ret = new RealVector();

                for (int i = 0; i < this.Rows; i++)
                {
                    ret.Add(InternalRep[i, Column]);
                }

                return ret;
            }
            set
            {
                for (int i = 0; i < this.Rows; i++)
                {
                    InternalRep[i, Column] = value[i];
                }

            }
        }
        public double this[int r, int c]
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

        /*
    \begin{bmatrix} 1 & 2 & 3\\ 4 & 5 & 6\\5 & 6 & 7 \end{bmatrix}

        */
        public SquareRealMatrix MultiplyByScalar(double Scalar)
        {
            SquareRealMatrix ret = new SquareRealMatrix(this.Columns, this.Rows);
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {

                    ret.InternalRep[i, j] = this.InternalRep[i, j] * Scalar;

                }

            }

            return ret;
        }

        static public SquareRealMatrix operator *(SquareRealMatrix A, double value)
        {
            return A.MultiplyByScalar(value);
        }

        static public SquareRealMatrix operator *(double value, SquareRealMatrix A)
        {
            return A.MultiplyByScalar(value);
        }

       public static RealVector operator*(UnitVector uv, SquareRealMatrix em)
        {
            RealVector ret = new RealVector();
            ret.IsRowOrColumn = uv.IsRowOrColumn;
            if (uv.Order != em.Rows)
            {
                throw new Exception("Vector length must be same as number of columns and rows of matrix");
            }

            for (int i = 0; i < em.Rows; i++)
            {
                double SumOfRow = 0;
                for (int j = 0; j < em.Columns; j++)
                {
                    SumOfRow += (em.InternalRep[i, j] * uv[j]);
                }

                ret.Add(SumOfRow);
            }

            return ret;
        
        }

        public static RealVector operator*(SquareRealMatrix em, UnitVector uv)
        {
            RealVector ret = new RealVector();
            ret.Clear();
            ret.IsRowOrColumn = uv.IsRowOrColumn;

            if (uv.Order != em.Rows)
            {
                throw new Exception("Vector length must be same as number of columns and rows of matrix");
            }

            for (int i = 0; i < em.Rows; i++)
            {
                double SumOfRow = 0;
                for (int j = 0; j < em.Columns; j++)
                {
                    SumOfRow += (em.InternalRep[i, j] * uv[j]);
                }

                ret.Add(SumOfRow);
            }

            return ret;
        
        }
 
        public double Trace()
        {
            double ret = 0;
            for (int i = 0; i < this.Rows; i++)
            {
                ret += this[i, i];
            }
            return ret;
        }

        public SquareRealMatrix Transpose()
        {
            SquareRealMatrix ret = new SquareRealMatrix(this.Rows, this.Columns);
            RowOrColumn rc = new RowOrColumn();
            rc.rowColumn = RowColumn.Row;
            rc.Val = 0;
            for (int i = 0; i < this.Rows; i++)
            {
                RealVector rv = this[i];
                rc.Val = i;
                ret[rc] = rv;
            }

            return ret;
        }

        public RealVector Diagonal()
        {
            RealVector ret = new RealVector();
            for (int i = 0; i < this.Columns; i++)
            {
                ret.Add(this[i, i]);
            }

            return ret;

        }
        public static SquareRealMatrix DiagonalMatrix(int Dim, double[] arr)
        {
            SquareRealMatrix ret = new SquareRealMatrix(Dim, Dim);
            for (int i = 0; i < Dim; i++)
            {
                ret[i, i] = arr[i];
            }

            return ret;
        }
        public string FullRep
        {
            get
            {
                if (m_FullRep == string.Empty)
                {
                    m_FullRep = ToLatex();
                }
                return m_FullRep;
            }
            set
            {
                m_FullRep = value;
            }
        }

        public static string VectorToLatex(double[] Vector)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\\begin{bmatrix}");
            int ColumnCount = Vector.Length;
            for (int i = 0; i < ColumnCount; i++)
            {
                sb.AppendFormat("{0}", Vector[i]);
                sb.Append("\\\\");

            }
            sb.Append(" \\end{bmatrix}");
            return sb.ToString();

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

        public string ToMathML()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            for (int i = 0; i < Rows; i++)
            {
                sb.Append("[");
                for (int j = 0; j < Columns; j++)
                {
                    if (j < Columns - 1)
                    {
                        sb.AppendFormat("{0},", InternalRep[i, j]);
                    }
                    else
                    {
                        sb.AppendFormat("{0}", InternalRep[i, j]);
                    }
                }

                if (i < Rows - 1)
                {
                    sb.Append("],");
                }
                else
                {
                    sb.Append("]");
                }
            }

            sb.Append("]");
            return sb.ToString();
        }

        public string ToString(string Format)
        {
            switch (Format.ToUpper())
            {
                case "N": //natural
                    return ToString();
                case "X": //Tex
                    return ToLatex();
                case "A": //Ascii
                    return ToMathML();
                case "F": //Ful Representation
                    return FullRep;
            };

            return ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    sb.AppendFormat("{0:0.0000}\t", InternalRep[i, j]);
                }

                sb.Append("\r\n");

            }


            return sb.ToString();
        }

    }
}