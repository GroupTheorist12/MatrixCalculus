using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;

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
        public string MinorName { get; set; }
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

        public string ToString(string Format)
        {
            switch (Format.ToUpper())
            {
                case "N": // natural
                    return ToString();
                case "L": // Tex
                    return ToLatex();
                case "F": // Full Representation
                    return FullRep;
            };

            return ToString();
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
                        while (ind < j)
                        {
                            Indexes.Add(new int[] { i, ind });
                            ind++;
                        }
                    }
                }
            }

            return Indexes;
        }

        public static List<int[]> UpperEchelonIndexes(RationalSquareMatrix A)
        {
            List<int[]> Indexes = new List<int[]>();
            for (int i = 0; i < A.Rows; i++)
            {
                for (int j = 1; j < A.Columns; j++)
                {
                    if (i == j)
                    {
                        int ind = j + 1;
                        while (ind < A.Columns)
                        {
                            Indexes.Add(new int[] { i, ind });
                            ind++;
                        }
                    }
                }
            }

            return Indexes;
        }

        public static RationalSquareMatrix KroneckerProduct(RationalSquareMatrix a, RationalSquareMatrix b)
        {
            int Rows = a.Rows * b.Rows;         // calculate number of rows.
            int Columns = a.Columns * b.Rows;   // calculate number of columns
            int incC = 0;                       // increment variable for column of b matrix
            int incR = 0;                       // increment variable for row of b matrix
            int incAMC = 0;                     // increment variable for column of a matrix
            int incAMR = 0;                     // increment variable for row of a matrix
            RationalSquareMatrix retVal = new RationalSquareMatrix(Rows, Columns);
            int rowCount;
            int colCount;
            string exp = string.Empty;

            for(rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                if(incR > b.Rows - 1)           // reached end of rows of b matrix
                {
                    incR = 0;
                    incAMR++; 
                }
                incAMC = 0;
                for(colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    incC++;
                    if(incC > b.Columns - 1)    // reached end of columns of b matrix
                    {
                        incC = 0;
                        incAMC++;    
                    }

                    retVal[rowCount, colCount] = a[incAMR, incAMC] * b[incR, incC];
                }
                incR++;

            }

            retVal.FullRep = a.ToLatex() + @"\;\otimes\;" + b.ToLatex() + " = " + retVal.ToLatex(); //produce latex string
            return retVal;
        }

        public static string DetFullRep(RationalSquareMatrix A)
        {
            StringBuilder sb = new StringBuilder();//Start building latex

            Rational ret = 1;
            RationalSquareMatrix I = IdentityMatrix(A.Rows);

            int i = 0;
            int j = 0;

            sb.Append(@"\begin{aligned}");

            sb.AppendFormat(@"&A = {0} I = {1}", A.ToLatex(), I.ToLatex() + @"\textrm{Initial matrix and identity matrix} \\ \\");//display A and I matrices before

            List<int[]> inds = LowerEchelonIndexes(I);

            foreach (int[] arr in inds)
            {
                i = arr[0];
                j = arr[1];
                Rational r = Rational.Abs(A[i, j]) / Rational.Abs(A[j, j]);
                if (A[i, j] > 0 && A[j, j] > 0)
                {
                    r = r * -1;
                }
                if (A[i, j] < 0 && A[j, j] < 0)
                {
                    r = r * -1;
                }

                string comment = string.Format(@"R_{0} \rightarrow R_{0} + {1}R_{2}", (i + 1), r.ToLatex(), (j + 1));
                I[i, j] = r;
                sb.AppendFormat(@"&{0}{1} = ", A.ToLatex(), I.ToLatex());//display A matrix and augmented I matrix

                A = I * A;
                sb.AppendFormat(@"{0}", A.ToLatex() + comment + @" \\ \\");//display A matrix after elementary operation

                I[i, j] = 0;

            }
            string combo = string.Empty;
            for (i = 0; i < A.Rows; i++)
            {
                for (j = 0; j < A.Columns; j++)
                {
                    if (i == j)
                    {
                        combo += A[i, j].ToLatex() + @"\cdot";
                        ret *= A[i, j];
                    }
                }
            }

            sb.AppendFormat(@"&Det = {0} = {1}", combo.Substring(0, combo.Length - 5), ret.ToLatex());//display det of A matrix
            sb.Append(@"\end{aligned}");

            return sb.ToString();
        }
        public static Rational Det(RationalSquareMatrix A)
        {
            Rational ret = 1;
            RationalSquareMatrix I = IdentityMatrix(A.Rows);

            int i = 0;
            int j = 0;

            List<int[]> inds = LowerEchelonIndexes(I);

            foreach (int[] arr in inds)
            {
                i = arr[0];
                j = arr[1];
                Rational r = Rational.Abs(A[i, j]) / Rational.Abs(A[j, j]);
                if (A[i, j] > 0 && A[j, j] > 0)
                {
                    r = r * -1;
                }
                if (A[i, j] < 0 && A[j, j] < 0)
                {
                    r = r * -1;
                }


                I[i, j] = r;
                A = I * A;
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

        public Rational Trace()
        {
            Rational retVal = 0;
            for (int rowCount = 0; rowCount < Rows; rowCount++)
            {
                retVal += this[rowCount, rowCount];
            }
            return retVal;
        }

        public RationalSquareMatrix Inverse()
        {
            RationalSquareMatrix Inv = null;
            RationalVector rv = new RationalVector();

            Inv = FaddevasMethod(this, out rv);

            return Inv;
        }
        public static RationalSquareMatrix FaddevasMethod(RationalSquareMatrix AIn, out RationalVector CharacteristicEquation)
        {
            RationalSquareMatrix A = AIn.Clone();
            CharacteristicEquation = new RationalVector();

            RationalSquareMatrix A_n = A;
            Rational b_n = 1;
            RationalSquareMatrix B_n = null;
            RationalSquareMatrix I = null;
            RationalSquareMatrix AInv = null;
            int i = 0;
            for(i = 1; i < A.Rows; i++)
            {
                b_n = -A_n.Trace() / (i);
                CharacteristicEquation.Add(b_n);
                I = RationalSquareMatrix.IdentityMatrix(A.Rows);
                
                B_n = A_n + b_n * I;

                A_n = A * B_n;
            }
            b_n = -A_n.Trace() / (i);
           CharacteristicEquation.Add(b_n);

            AInv = -1/b_n * B_n;
            AInv.FullRep = A.ToLatex() + "^{-1} = " + AInv.ToLatex();

            return AInv;
        }
        public RationalVector CramersRule(RationalVector VectorToSolve)
        {
            RationalVector Deltas = new RationalVector();

            RationalSquareMatrix A = this.Clone();
            Rational Delta = RationalSquareMatrix.Det(A);
            for(int i = 0; i < this.Rows; i++)
            {
                RationalVector rvSave = A[i];
                A[i] = VectorToSolve;
                Deltas.Add(RationalSquareMatrix.Det(A)/Delta);
                A[i] = rvSave;

            }
            return Deltas;
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


        public static RationalSquareMatrix operator +(RationalSquareMatrix a, RationalSquareMatrix b)
        {
            RationalSquareMatrix retVal = new RationalSquareMatrix(a.Rows, a.Columns);

            for (int i = 0; i < retVal.Rows; i++)
            {
                for (int j = 0; j < retVal.Columns; j++)
                {
                    retVal.InternalRep[i, j] = a.InternalRep[i, j] + b.InternalRep[i, j];
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(a.ToLatex());
            sb.Append(" + ");
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(retVal.ToLatex());

            retVal.FullRep = sb.ToString();
            return retVal;
        }

        public static RationalSquareMatrix operator +(Rational a, RationalSquareMatrix b)
        {
            RationalSquareMatrix retVal = new RationalSquareMatrix(b.Rows, b.Columns);

            for (int i = 0; i < retVal.Rows; i++)
            {
                for (int j = 0; j < retVal.Columns; j++)
                {
                    retVal.InternalRep[i, j] = a + b.InternalRep[i, j];
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(a.ToLatex());
            sb.Append(" + ");
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(retVal.ToLatex());

            retVal.FullRep = sb.ToString();
            return retVal;
        }

        public static RationalSquareMatrix operator +(RationalSquareMatrix a, Rational b)
        {
            RationalSquareMatrix retVal = new RationalSquareMatrix(a.Rows, a.Columns);

            for (int i = 0; i < retVal.Rows; i++)
            {
                for (int j = 0; j < retVal.Columns; j++)
                {
                    retVal.InternalRep[i, j] = a.InternalRep[i, j] + b;
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(a.ToLatex());
            sb.Append(" + ");
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(retVal.ToLatex());

            retVal.FullRep = sb.ToString();
            return retVal;
        }

        public static RationalSquareMatrix operator -(RationalSquareMatrix a, RationalSquareMatrix b)
        {
            RationalSquareMatrix retVal = new RationalSquareMatrix(a.Rows, a.Columns);

            for (int i = 0; i < retVal.Rows; i++)
            {
                for (int j = 0; j < retVal.Columns; j++)
                {
                    retVal.InternalRep[i, j] = a.InternalRep[i, j] - b.InternalRep[i, j];
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(a.ToLatex());
            sb.Append(" + ");
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(retVal.ToLatex());

            retVal.FullRep = sb.ToString();
            return retVal;
        }

        public static RationalSquareMatrix operator -(Rational a, RationalSquareMatrix b)
        {
            RationalSquareMatrix retVal = new RationalSquareMatrix(b.Rows, b.Columns);

            for (int i = 0; i < retVal.Rows; i++)
            {
                for (int j = 0; j < retVal.Columns; j++)
                {
                    retVal.InternalRep[i, j] = a - b.InternalRep[i, j];
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(a.ToLatex());
            sb.Append(" + ");
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(retVal.ToLatex());

            retVal.FullRep = sb.ToString();
            return retVal;
        }

        public static RationalSquareMatrix operator -(RationalSquareMatrix a, Rational b)
        {
            RationalSquareMatrix retVal = new RationalSquareMatrix(a.Rows, a.Columns);

            for (int i = 0; i < retVal.Rows; i++)
            {
                for (int j = 0; j < retVal.Columns; j++)
                {
                    retVal.InternalRep[i, j] = a.InternalRep[i, j] - b;
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(a.ToLatex());
            sb.Append(" + ");
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(retVal.ToLatex());

            retVal.FullRep = sb.ToString();
            return retVal;
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

        public static RationalSquareMatrix operator *(Rational a, RationalSquareMatrix b)
        {
            RationalSquareMatrix retVal = new RationalSquareMatrix(b.Rows, b.Columns);

            for (int i = 0; i < retVal.Rows; i++)
            {
                for (int j = 0; j < retVal.Columns; j++)
                {
                    retVal.InternalRep[i, j] = a * b.InternalRep[i, j];
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

        public static RationalSquareMatrix operator *(RationalSquareMatrix a, Rational b)
        {
            RationalSquareMatrix retVal = new RationalSquareMatrix(a.Rows, a.Columns);

            for (int i = 0; i < retVal.Rows; i++)
            {
                for (int j = 0; j < retVal.Columns; j++)
                {
                    retVal.InternalRep[i, j] = a.InternalRep[i, j] * b;
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(b.ToLatex());
            sb.Append(a.ToLatex());
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
            cfi.Minor.MinorName = symIn.MinorName;
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
            ParentMatrix.MinorName = "M1";
            if (cfList[0].Minor.Rows == 2) //At two go back
            {
                return cfList;
            }
            int inc = 0;
            RationalCoFactorInfo cfi = null;
            RationalCoFactorInfo cfiChild = null;
            List<RationalCoFactorInfo> cfListChild = null;
            string TopMinorName = string.Empty;

            while (inc < cfList.Count)
            {
                if (cfi == null)
                {
                    cfi = cfList[inc];
                    TopMinorName = "M" + (inc + 1).ToString();
                    cfi.Minor.MinorName = TopMinorName;
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
