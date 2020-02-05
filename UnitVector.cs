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


        public static ElementaryMatrix operator*(UnitVector e1, UnitVector e2)
        {
            ElementaryMatrix em = null;
            RowColumn rc = e2.IsRowOrColumn;

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