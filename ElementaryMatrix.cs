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
        public string FullRep { get; set; }

        public string Name { get; private set;}
        private string m_LatexName;
        public string LatexName { get { return m_LatexName; } }

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

        private void Init()
        {
            if (this.Name[0] != 'E')
            {
                throw new Exception("Name of ElementaryMatrix must begin with capitol E followed by two numer indices");
            }

            int oI1 = 0;
            int oI2 = 0;

            try
            {
                if (!int.TryParse(this.Name[1].ToString(), out oI1))
                {
                    throw new Exception("Name of ElementaryMatrix must begin with capitol E followed by two numer indices, index 1 bad.");

                }

                if (!int.TryParse(this.Name[2].ToString(), out oI2))
                {
                    throw new Exception("Name of ElementaryMatrix must begin with capitol E followed by two numer indices, index 2 bad");

                }

                this[oI1 - 1, oI2 - 1] = 1;
                m_LatexName = @"E_{" + (oI1).ToString() + (oI2).ToString() + "}";
            }
            catch (Exception)
            {
                throw new Exception("Name of ElementaryMatrix must begin with capitol E followed by two numer indices. Could not parse indices.");
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
            ElementaryMatrix ret = new ElementaryMatrix(a.Rows, a.Columns);

            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {

                    for (int k = 0; k < ret.Columns; k++)
                    {

                        ret.InternalRep[i, j] += a.InternalRep[i, k] * b.InternalRep[k, j];

                        if(ret.InternalRep[i, j] == 1)
                        {
                            ret.Major = i + 1;
                            ret.Minor = j + 1;
                            ret.Name = "E" + ret.Major.ToString(); ret.Minor.ToString();
                            ret.m_LatexName = @"E_{" + (ret.Major).ToString() + (ret.Minor).ToString() + "}";

                        }
                    }

                }



            }

            StringBuilder sb = new StringBuilder();

            sb.Append(a.LatexName + b.LatexName + " = ");
            sb.Append(a.ToLatex());
            sb.Append(b.ToLatex());
            sb.Append(" = ");
            sb.Append(ret.LatexName + " = ");
            sb.Append(ret.ToLatex());

            ret.FullRep = sb.ToString();


            return ret;
        }

        public static UnitVector operator*(UnitVector uv, ElementaryMatrix em)
        {
            UnitVector ret = new UnitVector(0);
            ret.Clear();
            if (uv.Order != em.Rows)
            {
                throw new Exception("Vector length must be same as number of columns and rows of matrix");
            }

            for (int i = 0; i < em.Rows; i++)
            {
                int SumOfRow = 0;
                for (int j = 0; j < em.Columns; j++)
                {
                    SumOfRow += (em.InternalRep[i, j] * uv[j]);
                }

                ret.Add(SumOfRow);
            }

            return ret;
        
        }

        public static UnitVector operator*(ElementaryMatrix em, UnitVector uv)
        {
            UnitVector ret = new UnitVector(0);
            ret.Clear();

            //Elementary Matrix private default constructor called from implicit method. It is a value
            if(em.Name == "E" && em.Rows == 1 && em.Columns == 1)
            {
                return em[0, 0] * uv;
            }

            if (uv.Order != em.Rows)
            {
                throw new Exception("Vector length must be same as number of columns and rows of matrix");
            }

            for (int i = 0; i < em.Rows; i++)
            {
                int SumOfRow = 0;
                for (int j = 0; j < em.Columns; j++)
                {
                    SumOfRow += (em.InternalRep[i, j] * uv[j]);
                }

                ret.Add(SumOfRow);
            }

            return ret;
        
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