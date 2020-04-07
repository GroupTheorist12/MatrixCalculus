using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;

namespace MatrixCalculus
{
    public class SymetricGroup : PermutationSet
    {

        public SymetricGroup(int GO) : base(GO)
        {

        }

        public IEnumerable<Permutation> SortedGroup()
        {
            IEnumerable<Permutation> ret = null;
            List<Permutation> lstGather = new List<Permutation>();

            IEnumerable<Permutation> id = this.ToList().FindAll(d => d.IsIdentity);
            IEnumerable<Permutation> cyclic = this.ToList().FindAll(d => d.Matrix.Trace() == 0);
            IEnumerable<Permutation> therest = this.ToList().FindAll(d => !d.IsIdentity && d.Matrix.Trace() != 0);

            lstGather.Add(id.ToList()[0]);  //Identity is first

            int i = 0;
            for (i = 1; i < this.Order; i++)
            {
                IEnumerable<Permutation> tmp = cyclic.ToList().FindAll(f => f.Matrix[0, i] == 1);
                lstGather.Add(tmp.ToList()[0]);
            }

            for (i = 0; i < this.Order; i++)
            {
                IEnumerable<Permutation> tmp = therest.ToList().FindAll(f => f.Matrix[0, i] == 1);
                lstGather.Add(tmp.ToList()[0]);
            }

            ret = lstGather;
            return ret;
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

            IEnumerable<Permutation> perms = this.SortedGroup();

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
                            sb.Append(perms.ToArray()[cnt++].Matrix.ToLatex() + "&");        
                        }
                        else
                        {
                            sb.Append(perms.ToArray()[cnt++].ToLatex() + "&");        

                        }
                    }    
                    else
                    {
                        if(DisplayMatrix)
                        {
                            sb.Append(perms.ToArray()[cnt++].Matrix.ToLatex());        
                        }
                        else
                        {
                            sb.Append(perms.ToArray()[cnt++].ToLatex());        
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

    }
}
