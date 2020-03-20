using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixCalculus
{

    public class RealVector : List<double>
    {
        public RowColumn IsRowOrColumn { get; set; }

        public string FullRep { get; set; }

        public string Name{get;set;}

        public RealVector()
        {
            this.IsRowOrColumn = RowColumn.Column;
            IsInteger = false;
        }

        public RealVector(RowColumn rc)
        {
            this.IsRowOrColumn = rc;
            IsInteger = false;
        }

        public RealVector(UnitVector vIn)
        {
            foreach(int i in vIn)
            {
                Add(vIn[i]);
            }

            IsInteger = true;
            Name = vIn.Name;
        }

        public bool IsInteger { get; set; }

        public double Norm()
        {
            return Math.Sqrt(DotProduct(this, this));
        }

        public static RealVector Normalize(RealVector v)
        {
            return v / Math.Sqrt(DotProduct(v, v));
        }

        public static RealVector operator *(double value, RealVector v)
        {
            RealVector vM = new RealVector();
            for (int vectorCount = 0; vectorCount < v.Count; vectorCount++)
            {
                vM.Add(value * v[vectorCount]);
            }
            return vM;
        }

        public static RealVector operator *(RealVector v, double value)
        {
            RealVector vM = new RealVector();
            for (int vectorCount = 0; vectorCount < v.Count; vectorCount++)
            {
                vM.Add(value * v[vectorCount]);
            }
            return vM;
        }

        public static RealVector operator /(RealVector v, double value)
        {
            RealVector vM = new RealVector();
            for (int vectorCount = 0; vectorCount < v.Count; vectorCount++)
            {
                vM.Add(v[vectorCount] / value);
            }
            return vM;
        }

        public static RealVector operator +(RealVector v, double value)
        {
            RealVector vM = new RealVector();
            for (int vectorCount = 0; vectorCount < v.Count; vectorCount++)
            {
                vM.Add(value + v[vectorCount]);
            }
            return vM;
        }

        public static RealVector operator +(double value, RealVector v)
        {
            RealVector vM = new RealVector();
            for (int vectorCount = 0; vectorCount < v.Count; vectorCount++)
            {
                vM.Add(value + v[vectorCount]);
            }
            return vM;
        }

        public static RealVector operator +(RealVector v1, RealVector v2)
        {
            RealVector vM = new RealVector();
            for (int vectorCount = 0; vectorCount < v1.Count; vectorCount++)
            {
                vM.Add(v1[vectorCount] + v2[vectorCount]);
            }
            return vM;
        }

        public static RealVector operator -(RealVector v1, RealVector v2)
        {
            RealVector vM = new RealVector();
            for (int vectorCount = 0; vectorCount < v1.Count; vectorCount++)
            {
                vM.Add(v1[vectorCount] - v2[vectorCount]);
            }
            return vM;
        }

        public static RealVector operator -(RealVector v, double value)
        {
            RealVector vM = new RealVector();
            for (int vectorCount = 0; vectorCount < v.Count; vectorCount++)
            {
                vM.Add(v[vectorCount] - value);
            }
            return vM;
        }

        public static RealVector operator -(double value, RealVector v)
        {
            RealVector vM = new RealVector();
            for (int vectorCount = 0; vectorCount < v.Count; vectorCount++)
            {
                vM.Add(value - v[vectorCount]);
            }
            return vM;
        }

        public static double DotProduct(RealVector v1, RealVector v2)
        {
            double retVal = 0;

            if (v1.Count != v2.Count)
            {
                throw new Exception("Vectors must be equal in length");
            }

            for (int vectorCount = 0; vectorCount < v1.Count; vectorCount++)
            {
                retVal += (v1[vectorCount] * v2[vectorCount]);
            }
            return retVal;
        }

        public string ToLatex(string Rep)
        {
            string retVal = ToLatex();
            switch (Rep)
            {
                default:
                    break;
                case "F":
                    retVal = FullRep;
                    break;
            }

            return retVal;
        }
        public string ToLatex()
        {
            string ret = string.Empty;

            string fill =
            "\\begin{bmatrix}" +
            "FILL_ME_UP_SIR" +
            "\\end{bmatrix}";


            string vType = (this.IsRowOrColumn == RowColumn.Column) ? "\\\\" : "&&\\!";
            StringBuilder sb = new StringBuilder();
            int counter = 0;
            for (counter = 0; counter < Count - 1; counter++)
            {
                if (!IsInteger)
                {
                    if (Math.Abs(this[counter] % 1) <= (Double.Epsilon * 100))
                    {
                        sb.AppendFormat("{0}{1}", this[counter], vType);
                    }
                    else
                    {
                        sb.AppendFormat("{0:0.0000}{1}", this[counter], vType);
                    }
                }
                else
                {
                    sb.AppendFormat("{0}{1}", this[counter], vType);

                }
            }

            if (!IsInteger)
            {
                if (Math.Abs(this[counter] % 1) <= (Double.Epsilon * 100))
                {
                    sb.AppendFormat("{0}{1}", this[counter], vType);
                }
                else
                {
                    sb.AppendFormat("{0:0.0000}{1}", this[counter], vType);
                }
            }
            else
            {
                sb.AppendFormat("{0}", this[counter]);
            }
            return fill.Replace("FILL_ME_UP_SIR", sb.ToString());
        }
    }
}