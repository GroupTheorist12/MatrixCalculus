using System.Collections;
namespace MatrixCalculus
{
    public class KroneckerProductSpace
    {
        private Hashtable htUnitVectors = new Hashtable();

        /// <summary>
        /// TODO: sure could use a description of what this is. GPG
        /// </summary>
        public int Order { get; }

        public UnitVector this[string e]
        {
            get
            {
                return (UnitVector)htUnitVectors[e];
            }
        }

        public class KroneckerProducts
        {
        }

        public KroneckerProductSpace(int Order)
        {
            this.Order = Order;

            for (int counter = 0; counter < this.Order; counter++)
            {
                string e = "e" + (counter + 1).ToString();
                htUnitVectors[e] = new UnitVector(e, this.Order);
            }
        }
    }
}