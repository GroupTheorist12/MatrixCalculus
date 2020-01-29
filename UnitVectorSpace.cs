using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;

namespace MatrixCalculus
{
    public class UnitVectorSpace : Hashtable
    {
        public int Order{get;}
        public UnitVectorSpace(int Order)
        {
           this.Order = Order;

           for(int i = 0; i < this.Order; i++)
           {
               string e = "e" + (i + 1).ToString();
               this.Add(e, new UnitVector(e, this.Order));
           }

        }

        public UnitVector this[string e]
        {
            get
            {
                return (UnitVector)this[e];
            }
        }
    }
}