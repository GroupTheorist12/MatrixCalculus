using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MatrixCalculus
{
    public class PermutationSet : FiniteSet<Permutation>
    {
        public override string FullRep { get; set; }
        public override string Name { get; set; }
        public int Order { get; set; }

        public bool DisplayMatrix{get;set;}
        public PermutationSet()
        {
            Order = 0;
            DisplayMatrix = false;
        }

        public PermutationSet(int o)
        {
            Order = o;
            Permute();
            DisplayMatrix = false;

        }
        private static void Swap(List<int> set, int index1, int index2)
        {
            int temp = set[index1];
            set[index1] = set[index2];
            set[index2] = temp;
        }

        public void Permute()
        {
            List<int> set = new List<int>();

            for (int i = 0; i < this.Order; i++)
            {
                set.Add(i + 1);
            }

            List<int[]> ret = Permute(set);

            foreach (int[] perm in ret)
            {
                this.Add(new Permutation(set.ToArray(), perm));
            }

        }

        public static PermutationSet Permute(int start)
        {
            PermutationSet gSet = new PermutationSet();
            List<int> set = new List<int>();

            for (int i = 0; i < start; i++)
            {
                set.Add(i + 1);
            }

            List<int[]> ret = Permute(set);

            foreach (int[] perm in ret)
            {
                gSet.Add(new Permutation(set.ToArray(), perm));
            }

            return gSet;
        }

        private static List<int[]> Permute(List<int> set)
        {
            List<int[]> result = new List<int[]>();

            Action<int> permute = null;
            permute = start =>
            {
                if (start == set.Count)
                {
                    result.Add(set.ToArray());
                }
                else
                {
                    List<int> swaps = new List<int>();
                    for (int i = start; i < set.Count; i++)
                    {
                        if (swaps.Contains(set[i])) continue; // skip if we already done swap with this item
                        swaps.Add(set[i]);

                        Swap(set, start, i);
                        permute(start + 1);
                        Swap(set, start, i);
                    }
                }
            };

            permute(0);

            return result;
        }

        public override string ToLatex()
        {
            int rows = 1;
            int cols = 4;

            if(Order <= 4)
            {
                cols = Order;
            }

            if(Order > 2)
            {
                rows = (int)this.Count() / cols;
            }

            StringBuilder sbTab = new StringBuilder();
            sbTab.Append("\\begin{array}{REPLACE_ME}".Replace("REPLACE_ME", HtmlOutputMethods.RepeatString("p{4em}", cols)));
            int cnt = 0;
            for(int i = 0; i < rows; i++)
            {
                StringBuilder sb = new StringBuilder();
                for(int j = 0; j < cols; j++)
                {
                    if(j < cols - 1)
                    {
                        if(DisplayMatrix)
                        {
                            sb.Append(this.ToArray()[cnt++].Matrix.ToLatex() + "&");        
                        }
                        else
                        {
                            sb.Append(this.ToArray()[cnt++].ToLatex() + "&");        

                        }
                    }    
                    else
                    {
                        if(DisplayMatrix)
                        {
                            sb.Append(this.ToArray()[cnt++].Matrix.ToLatex());        
                        }
                        else
                        {
                            sb.Append(this.ToArray()[cnt++].ToLatex());        
                        }
                    }    
                }

                if(i < rows - 1)
                {
                    sbTab.Append(sb.ToString());
                    sbTab.Append("\\\\\\\\");
                }
                else
                {
                    sbTab.Append(sb.ToString());

                }
            }

            sbTab.Append("\\end{array}");
            return sbTab.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            /* 
            sb.Append("{");
            foreach (int i in this)
            {
                sb.AppendFormat(" {0}", i);
            }

            sb.Append(" }");
            */
            return sb.ToString();
        }


        public override string ToString(string Op)
        {
            string ret = ToString();

            switch (Op.ToUpper())
            {
                case "L":
                    ret = ToLatex();
                    FullRep = ret;
                    break;
                case "F":
                    if (FullRep == null || FullRep == string.Empty)
                    {
                        FullRep = Name + "\\;=\\;" + ToLatex();
                    }
                    ret = FullRep;
                    break;
                default:
                    break;
            }

            return ret;
        }

        public override FiniteSet<Permutation> Union(FiniteSet<Permutation> SetIn)
        {
            /*
            IntegerFiniteSet outS = new IntegerFiniteSet(this);
            outS.Name = "U";
            outS.UnionWith(SetIn);
            outS.FullRep = string.Format("{0}\\;\\cup\\;{1}\\;=\\;", this.Name, SetIn.Name) + outS.ToLatex();
            */
            return this;
        }
        public override FiniteSet<Permutation> Intersection(FiniteSet<Permutation> SetIn)
        {
            /*       IntegerFiniteSet outS = new IntegerFiniteSet(this);
                  outS.Name = "I";
                  outS.IntersectWith(SetIn);
                  outS.FullRep = string.Format("{0}\\;\\cap\\;{1}\\;=\\;", this.Name, SetIn.Name) + outS.ToLatex();
             */
            return this;
        }

    }
}