using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixCalculus
{
    public class ElementaryMatrix
    {
        private int[,] InternalRep = null;
        public string FullRep { get; set; }

        public string Name { get; private set;}
        private string m_LatexName;
        public string LatexName { get { return m_LatexName; } }

        public int Rows = 0;
        public int Columns = 0;
        private List<double> Vector = null;
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

        private void Init()
        {
            if (this.Name[0] != 'E')
            {
                throw new Exception("Name of ElementaryMatrix must begin with a capital 'E' followed by two number indices");
            }

            int oI1 = 0;
            int oI2 = 0;

            try
            {
                if (!int.TryParse(this.Name[1].ToString(), out oI1))
                {
                    throw new Exception("Name of ElementaryMatrix must begin with a capital 'E' followed by two number indices, index 1 bad.");

                }

                if (!int.TryParse(this.Name[2].ToString(), out oI2))
                {
                    throw new Exception("Name of ElementaryMatrix must begin with a capital 'E' followed by two number indices, index 2 bad");

                }

                this[oI1 - 1, oI2 - 1] = 1;
                m_LatexName = @"E_{" + (oI1).ToString() + (oI2).ToString() + "}";
            }
            catch (Exception)
            {
                throw new Exception("Name of ElementaryMatrix must begin with a capital 'E' followed by two number indices. Could not parse indices.");
            }

            Major = oI1;
            Minor = oI2;
        }


        public int Major { get; private set; } //left most subscript indices
        public int Minor { get; private set; } //right most subscript indices
        private ElementaryMatrix(int value)
        {
            this.Rows = 1;
            this.Columns = 1;
            InternalRep = new int[this.Rows, this.Columns];
            this.Name = "E";
            this.FullRep = string.Empty;
            Major = 0;
            Minor = 0;
            Zero();

            this[0, 0] = value;
        }

        private ElementaryMatrix(int Rows, int Columns)
        {
            this.Rows = Rows;
            this.Columns = Columns;
            InternalRep = new int[this.Rows, this.Columns];
            this.Name = "E";
            this.FullRep = string.Empty;
            Major = 0;
            Minor = 0;
            Zero();
        }

        public static implicit operator int(ElementaryMatrix em) => em[0, 0];
        public static explicit operator ElementaryMatrix(int value) => new ElementaryMatrix(value);
        public ElementaryMatrix(int rows, int columns, string Name)
        {

            this.Rows = rows;
            this.Columns = columns;
            InternalRep = new int[this.Rows, this.Columns];
            this.Name = Name;
            this.FullRep = string.Empty;

            Zero();
            Init();
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
            set
            {
                Zero();

                InternalRep[r, c] = value;
            }
        }

        public static ElementaryMatrix operator *(ElementaryMatrix a, ElementaryMatrix b)
        {
            ElementaryMatrix retVal = new ElementaryMatrix(a.Rows, a.Columns);

            for (int rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retVal.Columns; colCount++)
                {

                    for (int k = 0; k < retVal.Columns; k++)
                    {

                        retVal.InternalRep[rowCount, colCount] += a.InternalRep[rowCount, k] * b.InternalRep[k, colCount];

                        if(retVal.InternalRep[rowCount, colCount] == 1)
                        {
                            retVal.Major = rowCount + 1;
                            retVal.Minor = colCount + 1;
                            retVal.Name = $"E{retVal.Major.ToString()}{retVal.Minor.ToString()}";
                            retVal.m_LatexName = $"E_{{{(retVal.Major).ToString() + (retVal.Minor).ToString()}}}";
                        }
                    }
                }
            }

            StringBuilder sb = new StringBuilder();

            sb.Append(a.LatexName + b.LatexName + " = ");
            sb.Append(a.ToLatex());
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(retVal.LatexName + " = ");
            sb.Append(retVal.ToLatex());

            retVal.FullRep = sb.ToString();

            return retVal;
        }

        public static UnitVector operator*(UnitVector uv, ElementaryMatrix em)
        {
            UnitVector retVal = new UnitVector(0);
            retVal.Clear();
            if (uv.Order != em.Rows)
            {
                throw new Exception("Vector length must be same as number of columns and rows of matrix");
            }

            for (int rowCount = 0; rowCount < em.Rows; rowCount++)
            {
                int SumOfRow = 0;
                for (int colCount = 0; colCount < em.Columns; colCount++)
                {
                    SumOfRow += (em.InternalRep[rowCount, colCount] * uv[colCount]);
                }

                retVal.Add(SumOfRow);
            }

            return retVal;
        }

        public static UnitVector operator*(ElementaryMatrix em, UnitVector uv)
        {
            UnitVector retVal = new UnitVector(0);
            retVal.Clear();

            // Elementary Matrix private default constructor called from implicit method. It is a value.
            if(em.Name == "E" && em.Rows == 1 && em.Columns == 1)
            {
                return em[0, 0] * uv;
            }

            if (uv.Order != em.Rows)
            {
                throw new Exception("Vector length must be same as number of columns and rows of matrix");
            }

            for (int rowCount = 0; rowCount < em.Rows; rowCount++)
            {
                int SumOfRow = 0;
                for (int colCount = 0; colCount < em.Columns; colCount++)
                {
                    SumOfRow += (em.InternalRep[rowCount, colCount] * uv[colCount]);
                }

                retVal.Add(SumOfRow);
            }

            return retVal;  
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

        public string ToLatex(string Rep)
        {
            string retVal = ToLatex();

            switch (Rep)
            {
                case "F":
                    if (FullRep != string.Empty) // Set outside current object
                    {
                        retVal = FullRep;
                    }
                    else
                    {
                        retVal = LatexName + " = " + ToLatex();
                    }
                    break;
                default:
                    break;
            }

            return retVal;
        }
    }
}