using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MatrixCalculus
{
    public class RationalVector : List<Rational>
    {
        public RowColumn IsRowOrColumn { get; set; }
        public string FullRep { get; set; }

        public string Name { get; set; }
        public RationalVector()
        {
            this.IsRowOrColumn = RowColumn.Column;
        }

        public RationalVector(RowColumn rc)
        {
            this.IsRowOrColumn = rc;
        }

        public RationalVector(UnitVector vIn)
        {
            foreach (int i in vIn)
            {
                this.Add(vIn[i]);
            }

            this.Name = vIn.Name;
        }

        public Rational Norm()
        {
            return new Rational(Math.Sqrt(DotProduct(this, this).ToDouble()));
        }

        public static RationalVector Normalize(RationalVector v)
        {
            return v / new Rational(Math.Sqrt(DotProduct(v, v).ToDouble()));
        }

        public static RationalVector operator *(Rational value, RationalVector v)
        {
            RationalVector vM = new RationalVector();
            for (int i = 0; i < v.Count; i++)
            {
                vM.Add(value * v[i]);
            }
            return vM;
        }
        public static RationalVector operator *(RationalVector v, Rational value)
        {
            RationalVector vM = new RationalVector();
            for (int i = 0; i < v.Count; i++)
            {
                vM.Add(value * v[i]);
            }
            return vM;
        }

        public static RationalVector operator /(RationalVector v, Rational value)
        {
            RationalVector vM = new RationalVector();
            for (int i = 0; i < v.Count; i++)
            {
                vM.Add(v[i] / value);
            }
            return vM;
        }

        public static RationalVector operator +(RationalVector v, Rational value)
        {
            RationalVector vM = new RationalVector();
            for (int i = 0; i < v.Count; i++)
            {
                vM.Add(value + v[i]);
            }
            return vM;
        }

        public static RationalVector operator +(Rational value, RationalVector v)
        {
            RationalVector vM = new RationalVector();
            for (int i = 0; i < v.Count; i++)
            {
                vM.Add(value + v[i]);
            }
            return vM;
        }

        public static RationalVector operator +(RationalVector v1, RationalVector v2)
        {
            RationalVector vM = new RationalVector();
            for (int i = 0; i < v1.Count; i++)
            {
                vM.Add(v1[i] + v2[i]);
            }
            return vM;
        }

        public static RationalVector operator -(RationalVector v1, RationalVector v2)
        {
            RationalVector vM = new RationalVector();
            for (int i = 0; i < v1.Count; i++)
            {
                vM.Add(v1[i] - v2[i]);
            }
            return vM;
        }

        public static RationalVector operator -(RationalVector v, Rational value)
        {
            RationalVector vM = new RationalVector();
            for (int i = 0; i < v.Count; i++)
            {
                vM.Add(v[i] - value);
            }
            return vM;
        }

        public static RationalVector operator -(Rational value, RationalVector v)
        {
            RationalVector vM = new RationalVector();
            for (int i = 0; i < v.Count; i++)
            {
                vM.Add(value - v[i]);
            }
            return vM;
        }

        public static Rational DotProduct(RationalVector v1, RationalVector v2)
        {
            Rational ret = 0;

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
            "\\begin{bmatrix}" +
            "FILL_ME_UP_SIR" +
            "\\end{bmatrix}";


            string vType = (this.IsRowOrColumn == RowColumn.Column) ? "\\\\" : "&&\\!";
            StringBuilder sb = new StringBuilder();
            int i = 0;
            for (i = 0; i < this.Count - 1; i++)
            {
                sb.AppendFormat("{0}{1}", this[i].ToLatex(), vType);
            }

            sb.AppendFormat("{0}{1}", this[i].ToLatex(), vType);

            return fill.Replace("FILL_ME_UP_SIR", sb.ToString());
        }

        public string ToLatex(string Rep)
        {
            string ret = this.ToLatex();
            switch (Rep)
            {
                default:
                    break;
                case "F":
                    ret = FullRep;
                    break;
            }

            return ret;
        }

    }
}
