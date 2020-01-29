using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;

namespace MatrixCalculus
{
    public class UnitVectorSpace : List<UnitVector>
    {
        public int Order{get;}
        private Hashtable htVectors = new Hashtable();
        public UnitVectorSpace(int Order)
        {
           this.Order = Order;

           for(int i = 0; i < this.Order; i++)
           {
               string e = "e" + (i + 1).ToString();
               UnitVector uv = new UnitVector(e, this.Order);
               this.Add(uv);
               htVectors[e] = uv;
           }

        }

        public UnitVector this[string e]
        {
            get
            {
                return (UnitVector)htVectors[e];
            }
        }

        public string ToLatex()
        {
            string ret = string.Empty;
            StringBuilder sb = new StringBuilder();
           for(int i = 0; i < this.Order; i++)
           {
               UnitVector uv = this[i];
                sb.AppendFormat(" {0} ", uv.ToLatex("F"));
           }
            ret = sb.ToString();

            return ret;
        }
    }
}