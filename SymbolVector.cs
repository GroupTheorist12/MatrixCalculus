using System;
using System.Collections.Generic;
using System.Text;

namespace MatrixCalculus
{
    public class SymbolVector : List<Symbol>
    {
        public RowColumn IsRowOrColumn { get; set; }
        public SymbolFactory Parent { get; set; }
        public string FullRep { get; set; }
        public string Name { get; set; }

        public bool IsInteger { get; set; }

        public SymbolVector()
        {
            IsRowOrColumn = RowColumn.Column;
            IsInteger = false;
        }

        public SymbolVector(RowColumn rc)
        {
            IsRowOrColumn = rc;
            IsInteger = false;
        }

        public SymbolVector(UnitVector vIn)
        {
            foreach (int vectorCount in vIn)
            {
                Add(new Symbol(vIn[vectorCount].ToString()));
            }

            IsInteger = true;
            Name = vIn.Name;
        }


        public static Symbol DotProduct(SymbolVector v1, SymbolVector v2)
        {
            Symbol retVal = new Symbol();

            if (v1.Count != v2.Count)
            {
                throw new Exception("Vectors must be equal in length");
            }

            for (int vectorCounter = 0; vectorCounter < v1.Count; vectorCounter++)
            {
                retVal += (v1[vectorCounter] * v2[vectorCounter]);
            }
            return retVal;
        }

        public string ToLatex()
        {
            string fill =
            "\\begin{bmatrix}" +
            "FILL_ME_UP_SIR" +
            "\\end{bmatrix}";

            string vType = (IsRowOrColumn == RowColumn.Column) ? "\\\\" : "&&\\!";
            StringBuilder sb = new StringBuilder();
            int counter = 0;
            for (counter = 0; counter < Count - 1; counter++)
            {
                sb.AppendFormat("{0}{1}", this[counter].LatexString, vType);
            }

            sb.AppendFormat("{0}", this[counter].LatexString);
            return fill.Replace("FILL_ME_UP_SIR", sb.ToString());
        }
    }
}
