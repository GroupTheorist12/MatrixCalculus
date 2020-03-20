using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixCalculus
{
    public class SquareRealMatrix
    {
        private double[,] InternalRep = null;
        private string m_FullRep = string.Empty;

        public RealFactory Parent { get; set; }
        public int Rows = 0;
        public int Columns = 0;
        private List<double> Vector = null;

        public string Name { get; set; }
        public string LatexName { get; private set; }

        private void Zero()
        {
            for (int rowCount = 0; rowCount < Rows; rowCount++)
            {
                for (int colCount = 0; colCount < Columns; colCount++)
                {
                    InternalRep[rowCount, colCount] = 0;
                }
            }
        }

        private void FromVector()
        {
            int cnt = 0;
            for (int rowCount = 0; rowCount < Rows; rowCount++)
            {
                for (int colCount = 0; colCount < Columns; colCount++)
                {
                    InternalRep[rowCount, colCount] = Vector[cnt++];
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

            for (int rowCount = 0; rowCount < Rows; rowCount++)
            {
                double SumOfRow = 0;
                for (int colCount = 0; colCount < Columns; colCount++)
                {
                    SumOfRow += (InternalRep[rowCount, colCount] * VectorIn[colCount]);
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
            Rows = rvList[0].Count;
            Columns = rvList.Count;

            InternalRep = new double[Rows, Columns];

            Zero();
            for (int colCount = 0; colCount < Columns; colCount++)
            {
                int rows = rvList[colCount].Count;
                if (rows != this.Columns)
                {
                    throw new Exception("rows and columns must be equal for square matrix");
                }

                this[colCount] = rvList[colCount];
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

            Rows = rows;
            Columns = columns;
            InternalRep = RC;
        }

        // This method determines the sub matrix corresponding to a given element
        // TODO: these single letter vars aren't descriptive enough for dumbasses like me. GPG
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
                for (int counter = 0; counter < order; counter++)
                {
                    double[,] Temp = CreateSmallerMatrix(input, 0, counter);
                    value += input[0, counter] * (SignOfElement(0, counter) * Determinant(Temp));
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

            Rows = rows;
            Columns = columns;
            InternalRep = new double[Rows, Columns];

            Zero();
        }

        public static SquareRealMatrix ElementaryMatrix(int rows, int columns, string Name)
        {
            SquareRealMatrix retVal = new SquareRealMatrix(rows, columns);
            retVal.Name = Name;

            if (retVal.Name[0] != 'E')
            {
                throw new Exception("Name of ElementaryMatrix must begin with a capital E followed by two number indices");
            }

            int oI1 = 0;
            int oI2 = 0;

            try
            {
                if (!int.TryParse(retVal.Name[1].ToString(), out oI1))
                {
                    throw new Exception("Name of ElementaryMatrix must begin with a capital E followed by two number indices, index 1 bad.");
                }

                if (!int.TryParse(retVal.Name[2].ToString(), out oI2))
                {
                    throw new Exception("Name of ElementaryMatrix must begin with a capital E followed by two number indices, index 2 bad");
                }

                retVal[oI1 - 1, oI2 - 1] = 1;
                retVal.LatexName = @"E_{" + (oI1).ToString() + (oI2).ToString() + "}";
            }
            catch (Exception)
            {
                throw new Exception("Name of ElementaryMatrix must begin with a capital E followed by two number indices. Could not parse indices.");
            }

            retVal.FullRep = retVal.LatexName + @"\;=\;" + retVal.ToLatex();
            return retVal;
        }

        public SquareRealMatrix(int rows, int columns, List<double> V)
        {
            if (rows != columns)
            {
                throw new Exception("Row count must equal the column count for a square matrix");

            }

            if (V.Count % rows != 0)
            {
                throw new Exception("Vector does not contain an even row count");
            }


            Vector = V;

            Rows = rows;
            Columns = columns;
            InternalRep = new double[Rows, Columns];

            FromVector();
        }

        public SquareRealMatrix Inverse()
        {
            double[][] m = HelpFunctions.DoubleArrayFromDouble(InternalRep);
            double[][] inv = HelpFunctions.MatrixInverse(m);

            double[,] mi = HelpFunctions.DoubleFromDoubleArray(inv);

            return new SquareRealMatrix(mi);
        }

        public static SquareRealMatrix operator +(SquareRealMatrix a, SquareRealMatrix b)
        {
            SquareRealMatrix retVal = new SquareRealMatrix(a.Rows, a.Columns);

            for (int i = 0; i < retVal.Rows; i++)
            {
                for (int j = 0; j < retVal.Columns; j++)
                {
                    retVal.InternalRep[i, j] = a.InternalRep[i, j] + b.InternalRep[i, j];
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("$$");
            sb.Append(a.ToLatex());
            sb.Append(" + ");
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(retVal.ToLatex());
            sb.Append("$$");

            retVal.m_FullRep = sb.ToString();
            return retVal;
        }

        public static SquareRealMatrix operator +(SquareRealMatrix a, double b)
        {
            SquareRealMatrix retVal = new SquareRealMatrix(a.Rows, a.Columns);

            for (int rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    retVal.InternalRep[rowCount, colCount] = a.InternalRep[rowCount, colCount] + b;
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("$$");
            sb.Append(a.ToLatex());
            sb.Append(" + ");
            sb.Append(b.ToString());
            sb.Append(" = ");
            sb.Append(retVal.ToLatex());
            sb.Append("$$");

            retVal.m_FullRep = sb.ToString();
            return retVal;
        }

        public static SquareRealMatrix operator +(double b, SquareRealMatrix a)
        {
            SquareRealMatrix retVal = new SquareRealMatrix(a.Rows, a.Columns);

            for (int rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    retVal.InternalRep[rowCount, colCount] = a.InternalRep[rowCount, colCount] + b;
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("$$");
            sb.Append(a.ToLatex());
            sb.Append(" + ");
            sb.Append(b.ToString());
            sb.Append(" = ");
            sb.Append(retVal.ToLatex());
            sb.Append("$$");

            retVal.m_FullRep = sb.ToString();
            return retVal;
        }

        public static SquareRealMatrix operator -(SquareRealMatrix a, SquareRealMatrix b)
        {
            SquareRealMatrix retVal = new SquareRealMatrix(a.Rows, a.Columns);

            for (int rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    retVal.InternalRep[rowCount, colCount] = a.InternalRep[rowCount, colCount] - b.InternalRep[rowCount, colCount];
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("$$");
            sb.Append(a.ToLatex());
            sb.Append(" + ");
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(retVal.ToLatex());
            sb.Append("$$");

            retVal.m_FullRep = sb.ToString();
            return retVal;
        }

        public static SquareRealMatrix operator -(SquareRealMatrix a, double b)
        {
            SquareRealMatrix retVal = new SquareRealMatrix(a.Rows, a.Columns);

            for (int rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    retVal.InternalRep[rowCount, colCount] = a.InternalRep[rowCount, colCount] - b;
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("$$");
            sb.Append(a.ToLatex());
            sb.Append(" - ");
            sb.Append(b.ToString());
            sb.Append(" = ");
            sb.Append(retVal.ToLatex());
            sb.Append("$$");

            retVal.m_FullRep = sb.ToString();
            return retVal;
        }

        public static SquareRealMatrix operator -(double b, SquareRealMatrix a)
        {
            SquareRealMatrix retVal = new SquareRealMatrix(a.Rows, a.Columns);

            for (int rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    retVal.InternalRep[rowCount, colCount] = b - a.InternalRep[rowCount, colCount];
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append("$$");
            sb.Append(b.ToString());
            sb.Append(" - ");
            sb.Append(a.ToLatex());
            sb.Append(" = ");
            sb.Append(retVal.ToLatex());
            sb.Append("$$");

            retVal.m_FullRep = sb.ToString();
            return retVal;
        }

        public static SquareRealMatrix operator *(SquareRealMatrix a, SquareRealMatrix b)
        {
            SquareRealMatrix retVal = new SquareRealMatrix(a.Rows, a.Columns);

            for (int rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    for (int retColCount = 0; retColCount < retVal.Columns; retColCount++)
                    {
                        retVal.InternalRep[rowCount, colCount] += a.InternalRep[rowCount, retColCount] * b.InternalRep[retColCount, colCount];
                        double sens = retVal.InternalRep[rowCount, colCount];

                        if (Math.Abs(sens) < 1.0e-8d) //f**ked up value set to zero  // TODO: tell us how you really feel about it, Brad.  GPG
                        {
                            retVal.InternalRep[rowCount, colCount] = 0;
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
            sb.Append(retVal.ToLatex());
            sb.Append("$$");

            retVal.m_FullRep = sb.ToString();

            return retVal;
        }

        public RealVector this[string ColumnsOrRows]
        {
            get
            {
                RealVector retVal = new RealVector();
                if (ColumnsOrRows.Length != 2 && ColumnsOrRows.IndexOf(".") == -1)
                {
                    throw new Exception("Bad indexer. Should by .j or i. with i or j being numeric such as .1 or 2.");
                }

                RowOrColumn rc = new RowOrColumn();
                try
                {
                    if (ColumnsOrRows[0] == '.') // Column
                    {
                        rc.rowColumn = RowColumn.Column;
                        rc.Val = int.Parse(ColumnsOrRows[1].ToString()) - 1;
                    }
                    else // Row
                    {
                        rc.rowColumn = RowColumn.Row;
                        rc.Val = int.Parse(ColumnsOrRows[0].ToString()) - 1;
                    }
                }
                catch
                {
                    throw new Exception("Bad indexer. Should by .j or i. with i or j being numeric such as .1 or 2.");
                }
                retVal = this[rc];
                retVal.FullRep = this.Name + "_" + ColumnsOrRows + @"\;=\;" + retVal.ToLatex();
                retVal.IsRowOrColumn = rc.rowColumn;
                return retVal;
            }
        }
        public RealVector this[RowOrColumn rc]
        {
            get
            {
                RealVector retVal = new RealVector();

                if (rc.rowColumn == RowColumn.Column)
                {
                    retVal = this[rc.Val];
                }
                else
                {
                    for (int rowCount = 0; rowCount < Columns; rowCount++)
                    {
                        retVal.Add(InternalRep[rc.Val, rowCount]);
                    }
                }
                return retVal;
            }
            set
            {
                if (rc.rowColumn == RowColumn.Column)
                {
                    this[rc.Val] = value;
                }
                else
                {
                    for (int colcount = 0; colcount < Columns; colcount++)
                    {
                        InternalRep[rc.Val, colcount] = value[colcount];
                    }
                }
            }
        }

        public RealVector this[int Column]
        {
            get
            {
                RealVector retVal = new RealVector();

                for (int rowCount = 0; rowCount < Rows; rowCount++)
                {
                    retVal.Add(InternalRep[rowCount, Column]);
                }

                return retVal;
            }
            set
            {
                for (int colCount = 0; colCount < Rows; colCount++)
                {
                    InternalRep[colCount, Column] = value[colCount];
                }
            }
        }

        public double this[int r, int c]
        {
            get
            {
                if (!(r < Rows && c < Columns))
                {
                    throw new Exception("Rows and Columns are out of range of square matrix");
                }
                return (InternalRep[r, c]);
            }
            set { InternalRep[r, c] = value; }
        }

        public SquareRealMatrix MultiplyByScalar(double Scalar)
        {
            SquareRealMatrix retVal = new SquareRealMatrix(this.Columns, this.Rows);
            for (int rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    retVal.InternalRep[rowCount, colCount] = this.InternalRep[rowCount, colCount] * Scalar;
                }
            }

            return retVal;
        }

        public static SquareRealMatrix operator *(SquareRealMatrix A, double value)
        {
            return A.MultiplyByScalar(value);
        }

        public static SquareRealMatrix operator *(double value, SquareRealMatrix A)
        {
            return A.MultiplyByScalar(value);
        }

        public static RealVector operator *(UnitVector uv, SquareRealMatrix em)
        {
            RealVector retVal = new RealVector();
            retVal.IsRowOrColumn = uv.IsRowOrColumn;
            if (uv.Order != em.Rows)
            {
                throw new Exception("Vector length must be equal to the number of rows and columns in the matrix");
            }

            for (int rowCount = 0; rowCount < em.Rows; rowCount++)
            {
                double SumOfRow = 0;
                for (int colCount = 0; colCount < em.Columns; colCount++)
                {
                    SumOfRow += (em.InternalRep[rowCount, colCount] * uv[colCount]);
                }

                retVal.Add(SumOfRow);
            }

            return retVal;
        }

        public static RealVector operator *(SquareRealMatrix em, UnitVector uv)
        {
            RealVector retVal = new RealVector();
            retVal.Clear();
            retVal.IsRowOrColumn = uv.IsRowOrColumn;

            if (uv.Order != em.Rows)
            {
                throw new Exception("Vector length must be equal to the number of rows and columns in the matrix");
            }

            for (int rowCount = 0; rowCount < em.Rows; rowCount++)
            {
                double SumOfRow = 0;
                for (int colCount = 0; colCount < em.Columns; colCount++)
                {
                    SumOfRow += (em.InternalRep[rowCount, colCount] * uv[colCount]);
                }

                retVal.Add(SumOfRow);
            }

            return retVal;
        }

        public RealVector Vec(string MatrixName = "A")
        {
            RealVector retVal = new RealVector(); //create return vector
            string MN = (this.Name == string.Empty || this.Name == null) ? MatrixName : this.Name;
            for (int rowCount = 0; rowCount < this.Columns; rowCount++)
            {
                RealVector rv = this["." + (rowCount + 1).ToString()]; //use column accessor
                retVal.AddRange(rv); //add to return vector
            }

            retVal.FullRep = @"Vec\;" + MN + " = " + retVal.ToLatex();
            return retVal;
        }

        public double Trace()
        {
            double retVal = 0;
            for (int rowCount = 0; rowCount < Rows; rowCount++)
            {
                retVal += this[rowCount, rowCount];
            }
            return retVal;
        }

        public SquareRealMatrix Transpose()
        {
            SquareRealMatrix retVal = new SquareRealMatrix(this.Rows, this.Columns);
            RowOrColumn rc = new RowOrColumn();
            rc.rowColumn = RowColumn.Row;
            rc.Val = 0;
            for (int rowCount = 0; rowCount < Rows; rowCount++)
            {
                RealVector rv = this[rowCount];
                rc.Val = rowCount;
                retVal[rc] = rv;
            }

            return retVal;
        }

        public RealVector Diagonal()
        {
            RealVector retVal = new RealVector();
            for (int colCount = 0; colCount < Columns; colCount++)
            {
                retVal.Add(this[colCount, colCount]);
            }

            return retVal;
        }

        public static SquareRealMatrix DiagonalMatrix(int Dim, double[] arr)
        {
            SquareRealMatrix retVal = new SquareRealMatrix(Dim, Dim);
            for (int dimCount = 0; dimCount < Dim; dimCount++)
            {
                retVal[dimCount, dimCount] = arr[dimCount];
            }

            return retVal;
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
            for (int colCount = 0; colCount < ColumnCount; colCount++)
            {
                sb.AppendFormat("{0}", Vector[colCount]);
                sb.Append("\\\\");

            }
            sb.Append(" \\end{bmatrix}");
            return sb.ToString();
        }

        public string ToLatex()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\\begin{bmatrix}");
            for (int rowCount = 0; rowCount < Rows; rowCount++)
            {
                for (int colCount = 0; colCount < Columns; colCount++)
                {
                    if (colCount < Columns - 1)
                    {
                        sb.AppendFormat("{0} &", InternalRep[rowCount, colCount]);
                    }
                    else
                    {
                        sb.AppendFormat("{0}", InternalRep[rowCount, colCount]);
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
            for (int rowCount = 0; rowCount < Rows; rowCount++)
            {
                sb.Append("[");
                for (int colCount = 0; colCount < Columns; colCount++)
                {
                    if (colCount < Columns - 1)
                    {
                        sb.AppendFormat("{0},", InternalRep[rowCount, colCount]);
                    }
                    else
                    {
                        sb.AppendFormat("{0}", InternalRep[rowCount, colCount]);
                    }
                }

                if (rowCount < Rows - 1)
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
                case "N": // natural
                    return ToString();
                case "X": // Tex
                    return ToLatex();
                case "A": // ASCII
                    return ToMathML();
                case "F": // Full Representation
                    return FullRep;
            };

            return ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int rowCount = 0; rowCount < Rows; rowCount++)
            {
                for (int colCount = 0; colCount < Columns; colCount++)
                {
                    sb.AppendFormat("{0:0.0000}\t", InternalRep[rowCount, colCount]);
                }

                sb.Append("\r\n");
            }

            return sb.ToString();
        }

        public static SquareRealMatrix KroneckerSum(SquareRealMatrix a, SquareRealMatrix b)
        {
            SquareRealMatrix retVal = null;
            SquareRealMatrix id = IdentityMatrix(a.Rows);

            retVal = KroneckerProduct(a, id) + KroneckerProduct(id, b);

            retVal.FullRep = a.ToLatex() + @"\;\oplus\;" + b.ToLatex() + " = " + retVal.ToLatex(); //produce latex string

            return retVal;
        }

        public static SquareRealMatrix IdentityMatrix(int Order)
        {
            SquareRealMatrix retVal = new SquareRealMatrix(Order, Order);
            for (int rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    if (rowCount == colCount)
                    {
                        retVal[rowCount, colCount] = 1;
                    }
                }
            }
            return retVal;
        }

        public static SquareRealMatrix KroneckerProduct(SquareRealMatrix a, SquareRealMatrix b)
        {
            int Rows = a.Rows * b.Rows; //calculate number of rows.
            int Columns = a.Columns * b.Rows; // calculate number of columns
            int incC = 0; //increment variable for column of b matrix
            int incR = 0; //increment variable for row of b matrix
            int incAMC = 0;//increment variable for column of a matrix
            int incAMR = 0;//increment variable for row of a matrix
            SquareRealMatrix retVal = new SquareRealMatrix(Rows, Columns);
            int rowCount = 0;
            int colCount = 0;
            double exp = 0;

            for (rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                if (incR > b.Rows - 1)//reached end of rows of b matrix
                {
                    incR = 0;
                    incAMR++;
                }
                incAMC = 0;
                for (colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    exp = a[incAMR, incAMC] * b[incR, incC];
                    incC++;
                    if (incC > b.Columns - 1)////reached end of columns of b matrix
                    {
                        incC = 0;
                        incAMC++;
                    }

                    retVal[rowCount, colCount] = exp;
                }
                incR++;
            }

            retVal.FullRep = a.ToLatex() + @"\;\otimes\;" + b.ToLatex() + " = " + retVal.ToLatex(); //produce latex string
            return retVal;
        }
    }
}