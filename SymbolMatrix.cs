using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MatrixCalculus
{

    public struct CoFactorInfoList
    {
        public CoFactorInfo cfiTop;
        public List<CoFactorInfo> cfLinks;
        public List<CoFactorInfoList> Links;
    }
    public class CoFactorInfo
    {
        public SymbolMatrix Minor = null;
        public int Sign = 0;

        public Symbol CoFactor = null;

        public List<List<CoFactorInfo>> ListOfLists = new List<List<CoFactorInfo>>();
    }
    public class SymbolMatrix
    {
        private Symbol[,] InternalRep = null;

        public string FullRep { get; set; }
        public int Rows = 0;
        public int Columns = 0;
        private List<Symbol> Vector = null;

        public SymbolType SymbolMatrixSymbolType { get; set; }

        public SymbolFactory Parent { get; set; }

        private void Zero()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    InternalRep[i, j] = new Symbol("0");
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
        public string Name { get; set; }
        public string LatexName { get; private set; }

        public SymbolMatrix(int rows, int columns)
        {
            if (rows != columns)
            {
                throw new Exception("rows and columns must be equal for square matrix");

            }

            this.Rows = rows;
            this.Columns = columns;
            InternalRep = new Symbol[this.Rows, this.Columns];
            SymbolMatrixSymbolType = SymbolType.Expression;

            Zero();
        }

        public SymbolMatrix(int rows, int columns, List<Symbol> V)
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
            InternalRep = new Symbol[this.Rows, this.Columns];
            SymbolMatrixSymbolType = V[0].symbolType;
            this.Parent = V[0].Parent;
            FromVector();
        }

        public Symbol this[int r, int c]
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

        public SymbolVector this[RowOrColumn rc]
        {
            get
            {
                SymbolVector ret = new SymbolVector();

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

        public SymbolVector this[int Column]
        {
            get
            {
                SymbolVector ret = new SymbolVector();

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
        private Symbol SignOfElement(int i, int j)
        {
            if ((i + j) % 2 == 0)
            {
                return new Symbol("1");
            }
            else
            {
                return new Symbol("-1");
            }
        }

        //this method determines the sub matrix corresponding to a given element
        private Symbol[,] CreateSmallerMatrix(Symbol[,] input, int i, int j)
        {
            int order = int.Parse(System.Math.Sqrt(input.Length).ToString());
            Symbol[,] output = new Symbol[order - 1, order - 1];
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

        public static List<CoFactorInfo> GetAllMatrixCoFactors(SymbolMatrix ParentMatrix)
        {
            List<CoFactorInfo> cfList = SymbolMatrix.GetCoFactors(ParentMatrix);
            int inc = 0;
            CoFactorInfo cfi = null;
            CoFactorInfo cfiChild = null;
            List<CoFactorInfo> cfListChild = null;
            while (inc < cfList.Count)
            {
                if (cfi == null)
                {
                    cfi = cfList[inc];
                }
                else if (cfi != null && cfi.ListOfLists.Count == 0) //init value
                {
                    List<CoFactorInfo> cfListTmp = SymbolMatrix.GetCoFactors(cfi.Minor);
                    cfi.ListOfLists.Add(cfListTmp);
                    cfListChild = new List<CoFactorInfo>(cfListTmp);
                    if (cfListChild[0].Minor.Rows == 2) //end of line
                    {
                        cfList[inc] = cfi;
                        cfi = null;
                        inc++;
                    }
                }
                else if (cfi != null && cfi.ListOfLists.Count > 0) //have value
                {
                    List<CoFactorInfo> cfListTmp = null;
                    cfiChild = new CoFactorInfo();
                    foreach (CoFactorInfo cfiC in cfListChild)
                    {
                        cfListTmp = SymbolMatrix.GetCoFactors(cfiC.Minor);
                        cfi.ListOfLists.Add(cfListTmp);
                        cfiChild.ListOfLists.Add(cfListTmp);
                    }

                    cfListChild = new List<CoFactorInfo>();
                    foreach (List<CoFactorInfo> cfl in cfiChild.ListOfLists)
                    {
                        foreach (CoFactorInfo cfiC in cfl)
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
        public static List<CoFactorInfo> GetCoFactors(SymbolMatrix ParentMatrix)
        {
            List<CoFactorInfo> cfiL = new List<CoFactorInfo>();
            int Order = ParentMatrix.Rows;

            for (int i = 0; i < ParentMatrix.Columns; i++)
            {
                cfiL.Add(GetCoFactor(ParentMatrix, i + 1));
            }
            return cfiL;
        }
        public static CoFactorInfo GetCoFactor(SymbolMatrix symIn, int Column)
        {
            CoFactorInfo cfi = new CoFactorInfo();
            cfi.Sign = (int)Math.Pow(-1, Column + 1);
            SymbolVector col = symIn[Column - 1];
            cfi.CoFactor = col[0];
            List<Symbol> symList = new List<Symbol>();

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

            cfi.Minor = new SymbolMatrix(symIn.Rows - 1, symIn.Columns - 1, symList);
            return cfi;
        }
        public Symbol Determinant()
        {
            return Determinant(this.InternalRep);
        }

        private Symbol Determinant(Symbol[,] input)
        {
            int order = int.Parse(System.Math.Sqrt(input.Length).ToString());
            if (order > 2)
            {
                Symbol value = new Symbol("0");
                for (int j = 0; j < order; j++)
                {
                    Symbol[,] Temp = CreateSmallerMatrix(input, 0, j);
                    string strPlusMunus = (SignOfElement(0, j).Tokens[0].Value == "1") ? "" : "-";

                    Console.WriteLine(strPlusMunus + input[0, j].NakedTokenString + "(" + Determinant(Temp).NakedTokenString + ")" + " " + order.ToString());
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

        public SymbolMatrix ReName(List<string> newSymbols)
        {
            SymbolMatrix flipper = new SymbolMatrix(this.Rows, this.Columns);
            RowOrColumn rc = new RowOrColumn();
            rc.rowColumn = RowColumn.Row;
            rc.Val = 0;

            SymbolVector oldSymbols = this[rc];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    int ind = oldSymbols.FindIndex(f => f.Expression == this[i, j].Expression);
                    Symbol sym = new Symbol(newSymbols[ind]);
                    sym.IsExpression = true;
                    flipper[i, j] = sym;
                }
            }
            return flipper;
        }

        /***************************Operators**********************************/

        private static SymbolMatrix MultiplyRational(SymbolMatrix a, SymbolMatrix b)
        {
            SymbolMatrix ret = new SymbolMatrix(a.Rows, a.Columns);
            ret.SymbolMatrixSymbolType = SymbolType.Rational;

            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {

                    for (int k = 0; k < ret.Columns; k++)
                    {

                        Rational inter = Rational.Parse(ret.InternalRep[i, j].Expression);
                        inter += Rational.Parse(a.InternalRep[i, k].Expression) * Rational.Parse(b.InternalRep[k, j].Expression);
                        Symbol sym = new Symbol(inter.ToString());
                        sym.LatexString = inter.ToLatex();
                        ret.InternalRep[i, j] = sym;

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

        public static SymbolMatrix operator +(SymbolMatrix a, SymbolMatrix b)
        {
            SymbolMatrix ret = new SymbolMatrix(a.Rows, a.Columns);

            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {

                    ret.InternalRep[i, j] = a.InternalRep[i, j] + b.InternalRep[i, j];

                }

            }

            StringBuilder sb = new StringBuilder();

            sb.Append(a.ToLatex());
            sb.Append(@"\;+\;");
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(ret.ToLatex());

            ret.FullRep = sb.ToString();

            return ret;

        }
        public static SymbolMatrix operator -(SymbolMatrix a, SymbolMatrix b)
        {
            SymbolMatrix ret = new SymbolMatrix(a.Rows, a.Columns);

            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {

                    ret.InternalRep[i, j] = a.InternalRep[i, j] - b.InternalRep[i, j];

                }

            }

            StringBuilder sb = new StringBuilder();

            sb.Append(a.ToLatex());
            sb.Append(@"\;-\;");
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(ret.ToLatex());

            ret.FullRep = sb.ToString();

            return ret;

        }

        public static SymbolMatrix operator *(SymbolMatrix a, SymbolMatrix b)
        {
            SymbolMatrix ret = new SymbolMatrix(a.Rows, a.Columns);


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

        public static SymbolMatrix operator /(SymbolMatrix a, SymbolMatrix b)
        {
            SymbolMatrix ret = new SymbolMatrix(a.Rows, a.Columns);


            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {

                    for (int k = 0; k < ret.Columns; k++)
                    {

                        ret.InternalRep[i, j] += a.InternalRep[i, k] / b.InternalRep[k, j];

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

        /***************************End of Operators**********************************/

        public SymbolMatrix Flip()
        {
            SymbolMatrix flipper = new SymbolMatrix(this.Rows, this.Columns);
            for (int i = 0; i < Rows; i++)
            {
                int MaxColumn = this.Columns - 1;
                for (int j = 0; j < Columns; j++)
                {
                    flipper[i, MaxColumn] = this[i, j];
                    --MaxColumn;
                }
            }
            return flipper;
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
                        sb.AppendFormat("{0} &", InternalRep[i, j].LatexString);
                    }
                    else
                    {
                        sb.AppendFormat("{0}", InternalRep[i, j].LatexString);
                    }

                }
                sb.Append("\\\\");
            }

            sb.Append(" \\end{bmatrix}");
            return sb.ToString();

        }

        public string ToLatex(string Rep)
        {
            string ret = ToLatex();

            switch (Rep)
            {
                case "F":
                    if (FullRep != string.Empty) //Set outside current object
                    {
                        ret = FullRep;
                    }
                    else
                    {
                        ret = LatexName + " = " + ToLatex();
                    }
                    break;
                default:
                    break;
            }

            return ret;
        }

    }
}
