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
            ElementaryMatrix em = new ElementaryMatrix(e1.Order, e1.Order);

            return em;
        }
        public string ToLatex()
        {
            string ret = string.Empty;

            string fill =
            "\\begin{pmatrix}" +
            "FILL_ME_UP_SIR" +
            "\\end{pmatrix}";


            string vType = (this.IsRowOrColumn == RowColumn.Column) ? "\\\\" : "&&\\!";
            StringBuilder sb = new StringBuilder();
            int i = 0;
            for (i = 0; i < this.Count - 1; i++)
            {
                sb.AppendFormat("{0}{1}", this[i], vType);
            }


            sb.AppendFormat("{0}", this[i]);

            return fill.Replace("FILL_ME_UP_SIR", sb.ToString());
        }


    }
    public class KroneckerProductSpace
    {
       private Hashtable htUnitVectors = new Hashtable();
        public int Order{get;}

       public KroneckerProductSpace(int Order)
       {
           this.Order = Order;

           for(int i = 0; i < this.Order; i++)
           {
               string e = "e" + (i + 1).ToString();
               htUnitVectors[e] = new UnitVector(e, this.Order);
           }
       }
        
        public UnitVector  this[string e]
        {
            get
            {
                return (UnitVector)htUnitVectors[e];
            }
        }
    }

    public class KroneckerProducts
    {


    }
}