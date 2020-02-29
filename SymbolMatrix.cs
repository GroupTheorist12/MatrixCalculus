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

        public string FullRep {get;set;}
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
