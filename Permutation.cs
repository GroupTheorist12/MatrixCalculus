using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace MatrixCalculus
{
    public class Permutation
    {
        //For composite operations such as product operator such as A * B = C
        public string FullRep { get; set; }  

        public string ElementName { get; set; }  
        
        //permutation matrix represented by this permutation
        public PermutationMatrix Matrix{get;private set;} 
        
        //Top row of permutation
        public readonly int[] TopRow = null;
        
        //Bottpm row of permutation
        public readonly int[] BottomRow = null;

        public Permutation(bool isIdentity)
        {
            this.IsIdentity = isIdentity;

        }
        public bool IsIdentity { get; }

        public Permutation(int[] TR, int[] BR)
        {
            TopRow = TR;
            BottomRow = BR;

            IsIdentity = BR.SequenceEqual(TR);
            Matrix = new PermutationMatrix(this);
        }

        public Permutation Inverse()
        {
            Permutation ret = null;

            List<int> perm = new List<int>(this.TopRow);

            for (int i = 0; i < this.BottomRow.Length; i++)
            {
                int ib = this.BottomRow[i] - 1;
                int ia = this.TopRow[i];
                perm[ib] = ia;
            }

            ret = new Permutation(this.TopRow, perm.ToArray());
            ret.FullRep = this.ToLatex() + "^{-1}\\;=\\;" + ret.ToLatex();
            return ret;

        }
        public string ToString(string Op)
        {
            string ret = ToString();

            switch (Op.ToUpper())
            {
                case "F":
                    ret = FullRep;
                    break;
                case "L":
                    FullRep = ToLatex();
                    break;
                default:
                    break;
            }
            return ret;
        }
        public override string ToString()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            string text = string.Join(", ", this.TopRow);
            sb.AppendFormat("{0}\r\n", text);
            text = string.Join(", ", this.BottomRow);
            sb.AppendFormat("{0}\r\n", text);
            sb.Append("\r\n");

            return sb.ToString();
        }

        public static Permutation operator *(Permutation A, Permutation B)
        {
            Permutation ret = null;

            if (A.BottomRow.Length != B.BottomRow.Length)
            {
                throw new Exception("Permutations not of equal order");
            }

            List<int> perm = new List<int>();

            for (int i = 0; i < B.BottomRow.Length; i++)
            {
                int ib = B.BottomRow[i] - 1;
                int ia = A.BottomRow[ib];
                perm.Add(ia);
            }

            ret = new Permutation(A.TopRow, perm.ToArray());

            ret.FullRep = A.ToLatex() + "\\;\\cdot\\;" + B.ToLatex() + "\\;=\\;" + ret.ToLatex();

            return ret;
        }

        public string ToLatex()
        {
            string ret = @"
        \left(
        \begin{array}{REPLACE_THECC}
        REPLACE_MY_TOP_ROW\\
        REPLACE_MY_BOTTOM_ROW\\
        \end{array}
        \right)
        ";

            string cc = HtmlOutputMethods.RepeatCharacter('c', this.TopRow.Length);

            return ret.Replace("REPLACE_THECC", cc)
              .Replace("REPLACE_MY_TOP_ROW", string.Join("&", this.TopRow))
              .Replace("REPLACE_MY_BOTTOM_ROW", string.Join("&", this.BottomRow));
        }

    }
}