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
        public string FullRep{get;set;}

        public string Name{get;}
        private string m_LatexName;
        public string LatexName{get{return m_LatexName;}}

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
            if(this.Name[0] != 'E')
            {
                throw new Exception("Name of ElementaryMatrix must begin with capitol E followed by two numer indices");
            }

            int oI1 = 0;
            int oI2 = 0;

            try
            {
                if(!int.TryParse(this.Name[1].ToString(), out oI1))
                {
                    throw new Exception("Name of ElementaryMatrix must begin with capitol E followed by two numer indices, index 1 bad.");

                }

                if(!int.TryParse(this.Name[2].ToString(), out oI2))
                {
                    throw new Exception("Name of ElementaryMatrix must begin with capitol E followed by two numer indices, index 2 bad");

                }

                this[oI1 - 1, oI2 - 1] = 1;
                m_LatexName = @"E_{" + (oI1).ToString() + (oI2).ToString() + "}";
            }
            catch(Exception)
            {
                throw new Exception("Name of ElementaryMatrix must begin with capitol E followed by two numer indices. Could not parse indices.");
            }

        }
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

                if(value != 1)
                {
                    throw new Exception("Value must be one");
                }
                InternalRep[r, c] = value;
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

        public string ToLatex(string Rep)
        {
            string ret = ToLatex();

            switch(Rep)
            {
                case "F":
                    if(FullRep != string.Empty) //Set outside current object
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