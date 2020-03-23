using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace MatrixCalculus
{

    public class RationalCoFactorInfo
    {
        public RationalSquareMatrix Minor = null;
        public int Sign = 0;

        public Rational CoFactor = 1;

        public List<List<RationalCoFactorInfo>> ListOfLists = new List<List<RationalCoFactorInfo>>();
    }
    public class RationalSquareMatrix
    {
        private Rational[,] InternalRep = null;
        public string FullRep { get; set; }

        public RationalFactory Parent { get; set; }
        public int Rows = 0;
        public int Columns = 0;
        private List<Rational> Vector = null;
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

        public Rational this[int r, int c]
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

        public RationalSquareMatrix(List<RationalVector> rvList)
        {
            this.Rows = rvList[0].Count;
            this.Columns = rvList.Count;

            InternalRep = new Rational[this.Rows, this.Columns];

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

        public RationalSquareMatrix Clone()
        {
            RationalSquareMatrix ret = new RationalSquareMatrix(this.Rows, this.Columns);
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    ret.InternalRep[i, j] = this.InternalRep[i, j];
                }
            }

            ret.Name = this.Name;
            ret.LatexName = this.LatexName;
            return ret;
        }
        public RationalSquareMatrix(int rows, int columns, List<Rational> V)
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
            InternalRep = new Rational[this.Rows, this.Columns];

            FromVector();
        }

        public RationalSquareMatrix(int rows, int columns)
        {
            if (rows != columns)
            {
                throw new Exception("rows and columns must be equal for square matrix");

            }

            this.Rows = rows;
            this.Columns = columns;
            InternalRep = new Rational[this.Rows, this.Columns];

            Zero();
        }

        public RationalVector this[int Column]
        {
            get
            {
                RationalVector ret = new RationalVector();

                for (int i = 0; i < this.Rows; i++)
                {
                    ret.Add(this[i, Column]);
                }

                return ret;
            }
            set
            {
                for (int i = 0; i < this.Rows; i++)
                {
                    this[i, Column] = value[i];
                }

            }
        }

        public string Name { get; set; }
        public string LatexName { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int rowCount = 0; rowCount < Rows; rowCount++)
            {
                for (int colCount = 0; colCount < Columns; colCount++)
                {
                    sb.AppendFormat("{0}\t", InternalRep[rowCount, colCount]);
                }

                sb.Append("\r\n");
            }

            return sb.ToString();
        }

        public static List<int[]> LowerEchelonIndexes(RationalSquareMatrix A)
        {
            List<int[]> Indexes = new List<int[]>();
            for (int i = 1; i < A.Rows; i++)
            {
                for (int j = 0; j < A.Columns; j++)
                {
                    if (i == j)
                    {
                        int ind = 0;
                        while(ind < j)
                        {
                            Indexes.Add(new int[] { i, ind });
                            ind++;
                        }
                    }
                }
            }

            return Indexes;
        }
        public static Rational Det(RationalSquareMatrix A)
        {
            Rational ret = 1;
            RationalSquareMatrix I = IdentityMatrix(A.Rows);
            
            int i = 0;
            int j = 0;

            List<int[]> inds = LowerEchelonIndexes(I);
            Console.Write(A);
            Console.WriteLine();

            foreach (int[] arr in inds)
            {
                i = arr[0];
                j = arr[1];
                Rational r = Rational.Abs(A[i, j]) / Rational.Abs(A[j, j]);
                if(A[i, j] > 0 && A[j, j] > 0)
                {
                    r = r * -1;
                }
                if(A[i, j] < 0 && A[j, j] < 0)
                {
                    r = r * -1;
                }


                I[i, j] = r;
                Console.Write(I);
                Console.WriteLine();
                A = I * A;
                Console.Write(A);
                Console.WriteLine();
                I[i, j] = 0;

            }
            for (i = 0; i < A.Rows; i++)
            {
                for (j = 0; j < A.Columns; j++)
                {
                    if (i == j)
                    {
                        ret *= A[i, j];
                    }
                }
            }

            return ret;
        }

        public RationalVector KramersRule(RationalVector VectorToSolve)
        {
            return Solve(new SubRationalMatrix(this.InternalRep, VectorToSolve));
        }

        private static RationalVector Solve(SubRationalMatrix matrix)
        {
            Rational det = matrix.Det();
            if (det == 0) throw new ArgumentException("The determinant is zero.");

            RationalVector answer = new RationalVector();
            for (int i = 0; i < matrix.Size; i++)
            {
                matrix.ColumnIndex = i;
                answer.Add(matrix.Det() / det);
            }
            return answer;
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
                        sb.AppendFormat("{0} &", InternalRep[i, j].ToLatex());
                    }
                    else
                    {
                        sb.AppendFormat("{0}", InternalRep[i, j].ToLatex());
                    }

                }
                sb.Append("\\\\");
            }

            sb.Append(" \\end{bmatrix}");
            return sb.ToString();

        }

        private class SubRationalMatrix
        {
            private Rational[,] source;
            private SubRationalMatrix prev;
            private RationalVector replaceColumn;

            public SubRationalMatrix(Rational[,] source, RationalVector replaceColumn)
            {
                this.source = source;
                this.replaceColumn = replaceColumn;
                this.prev = null;
                this.ColumnIndex = -1;
                Size = replaceColumn.Count;
            }

            private SubRationalMatrix(SubRationalMatrix prev, int deletedColumnIndex = -1)
            {
                this.source = null;
                this.prev = prev;
                this.ColumnIndex = deletedColumnIndex;
                Size = prev.Size - 1;
            }

            public int ColumnIndex { get; set; }
            public int Size { get; }

            public Rational this[int row, int column]
            {
                get
                {
                    if (source != null) return column == ColumnIndex ? replaceColumn[row] : source[row, column];
                    return prev[row + 1, column < ColumnIndex ? column : column + 1];
                }
            }


            public Rational Det()
            {
                if (Size == 1) return this[0, 0];
                if (Size == 2) return this[0, 0] * this[1, 1] - this[0, 1] * this[1, 0];
                SubRationalMatrix m = new SubRationalMatrix(this);
                Rational det = 0;
                int sign = 1;
                for (int c = 0; c < Size; c++)
                {
                    m.ColumnIndex = c;
                    Rational d = m.Det();
                    det += this[0, c] * d * sign;
                    sign = -sign;
                }
                return det;
            }

        }

        public static RationalSquareMatrix operator *(RationalSquareMatrix a, RationalSquareMatrix b)
        {
            RationalSquareMatrix retVal = new RationalSquareMatrix(a.Rows, a.Columns);

            for (int rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    for (int retColCount = 0; retColCount < retVal.Columns; retColCount++)
                    {
                        retVal.InternalRep[rowCount, colCount] += a.InternalRep[rowCount, retColCount] * b.InternalRep[retColCount, colCount];
                    }
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(a.ToLatex());
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(retVal.ToLatex());

            retVal.FullRep = sb.ToString();

            return retVal;
        }

        public static RationalSquareMatrix IdentityMatrix(int Order)
        {
            RationalSquareMatrix retVal = new RationalSquareMatrix(Order, Order);
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

        public static Rational Det2X2(RationalSquareMatrix rsm2X2, string CoFactor = "1")
        {
            Rational rCoFactor = Rational.Parse(CoFactor);
            Rational ret = (rsm2X2[0, 0] * rsm2X2[1, 1] - rsm2X2[0, 1] * rsm2X2[1, 0]) * rCoFactor;

            return ret;
        }
        public static RationalCoFactorInfo GetCoFactor(RationalSquareMatrix symIn, int Column)
        {
            RationalCoFactorInfo cfi = new RationalCoFactorInfo();
            cfi.Sign = (int)Math.Pow(-1, Column + 1);
            RationalVector col = symIn[Column - 1];
            cfi.CoFactor = col[0];
            List<Rational> symList = new List<Rational>();

            for (int i = 1; i < symIn.Rows; i++)
            {
                for (int j = 0; j < symIn.Columns; j++)
                {
                    if (j + 1 != Column)
                    {
                        symList.Add(symIn[i, j]);
                    }
                }
            }

            cfi.Minor = new RationalSquareMatrix(symIn.Rows - 1, symIn.Columns - 1, symList);
            return cfi;
        }

        public static List<RationalCoFactorInfo> GetCoFactors(RationalSquareMatrix ParentMatrix)
        {
            List<RationalCoFactorInfo> cfiL = new List<RationalCoFactorInfo>();
            int Order = ParentMatrix.Rows;

            for (int i = 0; i < ParentMatrix.Columns; i++)
            {
                cfiL.Add(GetCoFactor(ParentMatrix, i + 1));
            }
            return cfiL;
        }

        public static List<RationalCoFactorInfo> GetAllMatrixCoFactors(RationalSquareMatrix ParentMatrix)
        {
            List<RationalCoFactorInfo> cfList = RationalSquareMatrix.GetCoFactors(ParentMatrix);
            if (cfList[0].Minor.Rows == 2) //At two go back
            {
                return cfList;
            }
            int inc = 0;
            RationalCoFactorInfo cfi = null;
            RationalCoFactorInfo cfiChild = null;
            List<RationalCoFactorInfo> cfListChild = null;
            while (inc < cfList.Count)
            {
                if (cfi == null)
                {
                    cfi = cfList[inc];
                }
                else if (cfi != null && cfi.ListOfLists.Count == 0) //init value
                {
                    List<RationalCoFactorInfo> cfListTmp = RationalSquareMatrix.GetCoFactors(cfi.Minor);
                    cfi.ListOfLists.Add(cfListTmp);
                    cfListChild = new List<RationalCoFactorInfo>(cfListTmp);
                    if (cfListChild[0].Minor.Rows == 2) //end of line
                    {
                        cfList[inc] = cfi;
                        cfi = null;
                        inc++;
                    }
                }
                else if (cfi != null && cfi.ListOfLists.Count > 0) //have value
                {
                    List<RationalCoFactorInfo> cfListTmp = null;
                    cfiChild = new RationalCoFactorInfo();
                    foreach (RationalCoFactorInfo cfiC in cfListChild)
                    {
                        cfListTmp = RationalSquareMatrix.GetCoFactors(cfiC.Minor);
                        cfi.ListOfLists.Add(cfListTmp);
                        cfiChild.ListOfLists.Add(cfListTmp);
                    }

                    cfListChild = new List<RationalCoFactorInfo>();
                    foreach (List<RationalCoFactorInfo> cfl in cfiChild.ListOfLists)
                    {
                        foreach (RationalCoFactorInfo cfiC in cfl)
                        {
                            cfListChild.Add(cfiC);
                        }
                    }

                    cfiChild = null;

                    if (cfListChild[0].Minor.Rows == 2) //end of line
                    {
                        cfList[inc] = cfi;
                        cfi = null;
                        inc++;
                    }
                }

            }

            return cfList;
        }

    }
}
