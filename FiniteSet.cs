using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MatrixCalculus
{
  public interface IFiniteSet
  {
    string ToLatex();//Set will output latex
    string ToString(string Op); //ToString method that takes parameters
    string Name { get; set; } //Name of the set
    string FullRep { get; set; }//Used to create a fuller Latex output
  }
  public abstract class FiniteSet<T> : HashSet<T>, IFiniteSet //derive from .NET HashSet as well
  {
    public abstract string FullRep { get; set; }//must implment since we are inheriting from IFiniteSet
    public abstract string Name { get; set; } //must implment since we are inheriting from IFiniteSet
    public abstract string ToLatex(); //must implment since we are inheriting from IFiniteSet
    public abstract string ToString(string Op); //must implment since we are inheriting from IFiniteSet
    public abstract FiniteSet<T> Union(FiniteSet<T> SetIn); //All sets have an union operation so force deriving class to implment it
    public abstract FiniteSet<T> Intersection(FiniteSet<T> SetIn);//All sets have an intersection operation so force deriving class to implment it

  }

  public class IntegerFiniteSet : FiniteSet<int>
  {

    public override string FullRep { get; set; }
    public override string Name { get; set; }

    public IntegerFiniteSet(string N)
    {
      Name = N;
    }

    public IntegerFiniteSet()
    {
      Name = "S";
    }

    public IntegerFiniteSet(IntegerFiniteSet setIn)
    {
      foreach(int i in setIn)
      {
        Add(i);
      }
      Name = setIn.Name;  
    }
    public override string ToLatex()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("\\{");
      foreach (int i in this)
      {
        sb.AppendFormat("\\;{0}", i);
      }

      sb.Append("\\;\\}");
      return sb.ToString();
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();
      sb.Append("{");
      foreach (int i in this)
      {
        sb.AppendFormat(" {0}", i);
      }

      sb.Append(" }");
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
          if(FullRep == null || FullRep == string.Empty)
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

    public override FiniteSet<int> Union(FiniteSet<int> SetIn)
    {
      IntegerFiniteSet outS = new IntegerFiniteSet(this);
      outS.Name = "U";
      outS.UnionWith(SetIn);
      outS.FullRep = string.Format("{0}\\;\\cup\\;{1}\\;=\\;", this.Name, SetIn.Name) + outS.ToLatex();
        
      return outS;
    }
    public override FiniteSet<int> Intersection(FiniteSet<int> SetIn)
    {
      IntegerFiniteSet outS = new IntegerFiniteSet(this);
      outS.Name = "I";
      outS.IntersectWith(SetIn);
      outS.FullRep = string.Format("{0}\\;\\cap\\;{1}\\;=\\;", this.Name, SetIn.Name) + outS.ToLatex();
        
      return outS;
    }


  }

}
