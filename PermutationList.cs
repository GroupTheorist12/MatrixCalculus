using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MatrixCalculus
{
    public class PermutationList : List<Permutation>
    {
        public string FullRep { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public bool DisplayMatrix { get; set; }
        public PermutationList()
        {
            Order = 0;
            DisplayMatrix = false;
        }

        public PermutationList(int O)
        {
            Order = O;
            Permute();
            DisplayMatrix = false;
        }
        public PermutationList CyclicPermutations()
        {
            PermutationList perms = new PermutationList();

            foreach (Permutation p in this)
            {
                if (p.Matrix.Trace() == 0)
                {
                    perms.Add(p);
                }
            }
            return perms;
        }

        public string ToLatex()
        {
            int rows = 1;
            int cols = 4;

            if (Order <= 4)
            {
                cols = Order;
            }

            if (Order > 2)
            {
                rows = (int)this.Count() / cols;
            }

            StringBuilder sbTab = new StringBuilder();
            sbTab.Append("\\begin{array}{REPLACE_ME}".Replace("REPLACE_ME", HtmlOutputMethods.RepeatString("p{4em}", cols)));
            int cnt = 0;
            for (int i = 0; i < rows; i++)
            {
                StringBuilder sb = new StringBuilder();
                for (int j = 0; j < cols; j++)
                {
                    if (j < cols - 1)
                    {
                        if (DisplayMatrix)
                        {
                            sb.Append(this.ToArray()[cnt].Matrix.ToLatex() + @"\;" +
                            this.ToArray()[cnt].Matrix.Trace().ToString() + @"\;"
                            + this.ToArray()[cnt].ToLatex() + "&");
                            cnt++;
                        }
                        else
                        {
                            sb.Append(this.ToArray()[cnt++].ToLatex() + "&");

                        }
                    }
                    else
                    {
                        if (DisplayMatrix)
                        {
                            sb.Append(this.ToArray()[cnt].Matrix.ToLatex() + @"\;" +
                            this.ToArray()[cnt].Matrix.Trace().ToString() + @"\;"
                            + this.ToArray()[cnt].ToLatex());
                            cnt++;
                        }
                        else
                        {
                            sb.Append(this.ToArray()[cnt++].ToLatex());
                        }
                    }
                }

                if (i < rows - 1)
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
            if (cnt < this.Count)
            {
                sbTab.Append(@"\\ \\");

                while (cnt < this.Count)
                {
                    if (DisplayMatrix)
                    {
                        sbTab.Append(this.ToArray()[cnt].Matrix.ToLatex() + @"\;" +
                        this.ToArray()[cnt].Matrix.Trace().ToString() + @"\;"
                        + this.ToArray()[cnt].ToLatex());
                        cnt++;
                    }
                    else
                    {
                        sbTab.Append(this.ToArray()[cnt].ToLatex());
                        cnt++;
                    }

                }
            }

            return sbTab.ToString();
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

        public static PermutationList Permute(int start)
        {
            PermutationList gList = new PermutationList();
            List<int> set = new List<int>();

            for (int i = 0; i < start; i++)
            {
                set.Add(i + 1);
            }

            List<int[]> ret = Permute(set);

            foreach (int[] perm in ret)
            {
                gList.Add(new Permutation(set.ToArray(), perm));
            }

            return gList;
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

    }

}
