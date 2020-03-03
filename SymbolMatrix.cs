using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MatrixCalculus
{
    public class SymbolMatrix
    {
        private Symbol[,] InternalRep = null;

        public string FullRep { get; set; }
        public int Rows = 0;
        public int Columns = 0;
        private List<Symbol> Vector = null;

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
