using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;

namespace MatrixCalculus
{
    public class UnitVector : List<int>
    {
        
        public string Name{get;} 
        private string m_LatexName = string.Empty;
        public string LatexName{get {return m_LatexName;}} 
        
        public int Order{get;}
        public RowColumn IsRowOrColumn { get; set; }

        private void Init()
        {
            string val = this.Name.Replace("e", string.Empty);

            int iVal = 0;

            if(!int.TryParse(val, out iVal))
            {
                throw new Exception("Invalid basis vector name");
            }

            if(iVal > this.Order)
            {
                throw new Exception("Invalid basis vector dimension");

            }

            m_LatexName = "e_{" + iVal.ToString() + "}";
            for(int i = 0; i < this.Order; i++)
            {
                this.Add(0);
            }

            this[iVal - 1] = 1; 

        }
        public UnitVector(string Name, int Order)
        {
            this.Name = Name;
            this.Order = Order;
            this.IsRowOrColumn = RowColumn.Column;
            Init();

        }

        public UnitVector(string Name, int Order, RowColumn rc)
        {
            this.Name = Name;
            this.Order = Order;
            this.IsRowOrColumn = rc;
            Init();

        }

        public UnitVector(int value)
        {
            this.Name = "e";
            this.Order = 1;
            this.IsRowOrColumn = RowColumn.Column;
            this.Add(value);
        }

        public static int DotProduct(UnitVector v1, UnitVector v2)
        {
            int ret = 0;

            if (v1.Count != v2.Count)
            {
                throw new Exception("Vectors must be equal in length");
            }

            for (int i = 0; i < v1.Count; i++)
            {
                ret += (v1[i] * v2[i]);
            }
            return ret;
        }

        //Kronecker Delta
        public static int KroneckerDelta(int ind1, int ind2)
        {
            return (ind1 == ind2) ? 1 : 0;
        }

        public static bool operator ==(UnitVector e1, UnitVector e2)
        {
            if(e1.Order != e2.Order)
            {
                throw new Exception("Unit vector orders not the same");
            }
            return (e1.Name == e2.Name);
        }
        public static bool operator !=(UnitVector e1, UnitVector e2)
        {
            if(e1.Order != e2.Order)
            {
                throw new Exception("Unit vector orders not the same");
            }
            return e1.Name != e2.Name;
        }

        public static implicit operator int(UnitVector uv) => uv[0];
        public static explicit operator UnitVector(int value) => new UnitVector(value);        

        public static UnitVector operator*(int value, UnitVector uv)
        {
            UnitVector uvOut = new UnitVector(0);
            uvOut.Clear();
            for(int i = 0; i < uv.Count; i++)
            {
                uvOut.Add(value * uv[i]); 
            }
            return uvOut;
        }

        public static UnitVector operator*( UnitVector uv, int value)
        {
            UnitVector uvOut = new UnitVector(0);
            uvOut.Clear();
            for(int i = 0; i < uv.Count; i++)
            {
                uvOut.Add(uv[i] * value); 
            }
            return uvOut;
        }

        /*
        public static UnitVector operator*(ElementaryMatrix em, UnitVector uv)
        {
            UnitVector uvOut = null;
            //using following realtionships
            //Eij * er = ei * e'j * er 
            // = &jr * ei where & is the Kronecker Delta
            UnitVectorSpace uvp = new UnitVectorSpace(uv.Order);
            UnitVector ei = uvp["e" + em.Major.ToString()];
            UnitVector ej = uvp["e" + em.Minor.ToString()];
            RowColumn rc = ej.IsRowOrColumn;
            ej.IsRowOrColumn = RowColumn.Row;

            uvOut = ei * ej * uv;
            ej.IsRowOrColumn = rc; //Set it back to what it was
            return uvOut;
        }
        */

        public static ElementaryMatrix operator*(UnitVector e1, UnitVector e2)
        {
            ElementaryMatrix em = null;
            RowColumn rc = e2.IsRowOrColumn;

            
            if(e1.IsRowOrColumn == RowColumn.Row) //Dot product
            {
                return (ElementaryMatrix)DotProduct(e1, e2);
            }
            

            e2.IsRowOrColumn = RowColumn.Row; //transpose

            string emName = "E" + e1.Name[1].ToString() + e2.Name[1].ToString(); 
            em = new ElementaryMatrix(e1.Order, e2.Order, emName);
            em.FullRep = em.LatexName + " = " + e1.ToLatex() + e2.ToLatex() + " = " + em.ToLatex();

            e2.IsRowOrColumn = rc; //set back to what it was
            return em;
        }

        
        public string ToLatex()
        {
            string ret = string.Empty;

            string vType = (this.IsRowOrColumn == RowColumn.Column) ? "\\\\" : "&";
            string fill =
            "\\begin{bmatrix}" +
            "FILL_ME_UP_SIR" +
            "\\end{bmatrix}";

            StringBuilder sb = new StringBuilder();
            int i = 0;
            for (i = 0; i < this.Count - 1; i++)
            {
                sb.AppendFormat("{0}{1}", this[i], vType);
            }


            sb.AppendFormat("{0}", this[i]);
            return fill.Replace("FILL_ME_UP_SIR", sb.ToString());
        }

        public string ToLatex(string TheType)
        {
            string ret = this.ToLatex();
            StringBuilder sb = new StringBuilder();

            switch(TheType)
            {
                case "F":
                    ret = this.LatexName + " = " + this.ToLatex();
                    break;
            }
            return ret;
        }

    }
}