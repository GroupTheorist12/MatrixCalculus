using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;
namespace MatrixCalculus
{
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