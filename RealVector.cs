using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MatrixCalculus
{

    public enum RowColumn
    {
        Row,
        Column
    }

    public class RealVector : List<double>
    {
        public RowColumn IsRowOrColumn { get; set; }
        public RealVector()
        {
            this.IsRowOrColumn = RowColumn.Column;
        }

        public RealVector(RowColumn rc)
        {
            this.IsRowOrColumn = rc;

        }

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
            for (int i = 0; i < v.Count; i++)
            {
                vM.Add(value * v[i]);
            }
            return vM;
        }
        public static RealVector operator *(RealVector v, double value)
        {
            RealVector vM = new RealVector();
            for (int i = 0; i < v.Count; i++)
            {
                vM.Add(value * v[i]);
            }
            return vM;
        }

        public static RealVector operator /(RealVector v, double value)
        {
            RealVector vM = new RealVector();
            for (int i = 0; i < v.Count; i++)
            {
                vM.Add(v[i] / value);
            }
            return vM;
        }

        public static RealVector operator +(RealVector v, double value)
        {
            RealVector vM = new RealVector();
            for (int i = 0; i < v.Count; i++)
            {
                vM.Add(value + v[i]);
            }
            return vM;
        }

        public static RealVector operator +(double value, RealVector v)
        {
            RealVector vM = new RealVector();
            for (int i = 0; i < v.Count; i++)
            {
                vM.Add(value + v[i]);
            }
            return vM;
        }

        public static RealVector operator +(RealVector v1, RealVector v2)
        {
            RealVector vM = new RealVector();
            for (int i = 0; i < v1.Count; i++)
            {
                vM.Add(v1[i] + v2[i]);
            }
            return vM;
        }

        public static RealVector operator -(RealVector v1, RealVector v2)
        {
            RealVector vM = new RealVector();
            for (int i = 0; i < v1.Count; i++)
            {
                vM.Add(v1[i] - v2[i]);
            }
            return vM;
        }

        public static RealVector operator -(RealVector v, double value)
        {
            RealVector vM = new RealVector();
            for (int i = 0; i < v.Count; i++)
            {
                vM.Add(v[i] - value);
            }
            return vM;
        }

        public static RealVector operator -(double value, RealVector v)
        {
            RealVector vM = new RealVector();
            for (int i = 0; i < v.Count; i++)
            {
                vM.Add(value - v[i]);
            }
            return vM;
        }

        public static double DotProduct(RealVector v1, RealVector v2)
        {
            double ret = 0;

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
                sb.AppendFormat("{0:0.0000}{1}", this[i], vType);
            }


            sb.AppendFormat("{0:0.0000}", this[i]);

            return fill.Replace("FILL_ME_UP_SIR", sb.ToString());
        }
    }
}