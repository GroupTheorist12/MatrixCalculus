using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MatrixCalculus
{
    public class SymbolVector : List<Symbol>
    {
        public RowColumn IsRowOrColumn { get; set; }
        public SymbolFactory Parent{get;set;}
        public string FullRep { get; set; }
        public string Name { get; set; }

        public bool IsInteger { get; set; }

        public SymbolVector()
        {
            this.IsRowOrColumn = RowColumn.Column;
            IsInteger = false;
        }

        public SymbolVector(RowColumn rc)
        {
            this.IsRowOrColumn = rc;
            IsInteger = false;
        }

        public SymbolVector(UnitVector vIn)
        {
            foreach (int i in vIn)
            {
                this.Add(new Symbol(vIn[i].ToString()));
            }

            this.IsInteger = true;
            this.Name = vIn.Name;
        }


        public static Symbol DotProduct(SymbolVector v1, SymbolVector v2)
        {
            Symbol ret = new Symbol();

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
                sb.AppendFormat("{0}{1}", this[i].LatexString, vType);
            }

            sb.AppendFormat("{0}", this[i].LatexString);
            return fill.Replace("FILL_ME_UP_SIR", sb.ToString());
        }

    }


}
