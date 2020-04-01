using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Collections;

namespace MatrixCalculus
{
    public enum TensorType
    {
        Covariant,
        Contravariant,
        Mixed
    }

    public interface ITensor
    {
        int Rank { get; set; }

        int Dimension { get; set; }

        TensorType tensorType { get; set; }
        string Expand();

    }

    public class PseudoRank0Tensor : Symbol, ITensor
    {
        public int Rank { get; set; }

        public int Dimension { get; set; }

        public TensorType tensorType { get; set; }

        public PseudoRank0Tensor()
        {

        }

        public PseudoRank0Tensor(string Exp) : base(Exp)
        {

        }

        public string Expand()
        {
            return this.LatexString;
        }
    }

    public class PseudoRank1Tensor : SymbolVector, ITensor
    {
        public int Rank { get; set; }

        public int Dimension { get; set; }

        public TensorType tensorType { get; set; }

        public PseudoRank1Tensor()
        {

        }

        public PseudoRank1Tensor(RowColumn rc) : base(rc)
        {

        }

        public string Expand()
        {
            return this.ToLatex();
        }

    }
    public class PseudoRank2Tensor : SymbolMatrix, ITensor
    {
        public int Rank { get; set; }

        public int Dimension { get; set; }

        public TensorType tensorType { get; set; }

        public PseudoRank2Tensor(int rows, int columns, List<Symbol> V) : base(rows, columns, V)
        {

        }
        public PseudoRank2Tensor(int rows, int columns) : base(rows, columns)
        {

        }

        public string Expand()
        {
            return this.ToLatex();
        }

    }

    public class PseudoTensor
    {
        public int Rank { get; private set; }

        public int Dimension { get; private set; }

        public TensorType tensorType { get; private set; }

        private PseudoRank0Tensor Rank0Tensor = null;

        private PseudoRank1Tensor Rank1Tensor = null;

        private PseudoRank2Tensor Rank2Tensor = null;

        public ITensor Tensor
        {
            get
            {
                ITensor ret = null;
                if (Rank0Tensor != null)
                {
                    ret = Rank0Tensor;
                }

                if (Rank1Tensor != null)
                {
                    ret = Rank1Tensor;
                }

                if (Rank2Tensor != null)
                {
                    ret = Rank2Tensor;
                }

                return ret;
            }
        }
        public PseudoTensor(string n, int rank, int dim, TensorType tt)
        {
            Rank = rank;
            Dimension = dim;

            tensorType = tt;

            if (rank == 0)
            {
                Rank0Tensor = new PseudoRank0Tensor(n);
                Rank0Tensor.tensorType = this.tensorType;
                Rank0Tensor.Dimension = this.Dimension;
                Rank0Tensor.Rank = this.Rank;

            }

            if (rank == 1)
            {
                Rank1Tensor = new PseudoRank1Tensor();

                Rank1Tensor.tensorType = this.tensorType;
                Rank1Tensor.Dimension = this.Dimension;
                Rank1Tensor.Rank = this.Rank;

                string strTensorType = (tensorType == TensorType.Contravariant) ? "^" : "_";
                for (int i = 0; i < this.Dimension; i++)
                {
                    string val = $"{n}{strTensorType}{(i + 1).ToString()}";
                    Symbol sym = new Symbol();
                    sym.Expression = val;
                    sym.LatexString = val;
                    Rank1Tensor.Add(sym);
                }
            }

            if (rank == 2)
            {
                Rank2Tensor = new PseudoRank2Tensor(Dimension, Dimension);
                Rank2Tensor.tensorType = this.tensorType;
                Rank2Tensor.Dimension = this.Dimension;
                Rank2Tensor.Rank = this.Rank;

                string strTensorType = "_{ij}";

                if (tensorType == TensorType.Contravariant)
                {
                    strTensorType = "^{ij}";
                }

                if (tensorType == TensorType.Mixed)
                {
                    strTensorType = "_i^j";
                }
                for (int i = 0; i < Rank2Tensor.Rows; i++)
                {
                    for (int j = 0; j < Rank2Tensor.Columns; j++)
                    {
                        string val = n + strTensorType;
                        val = val.Replace("i", (i + 1).ToString()).Replace("j", (j + 1).ToString());

                        Symbol sym = new Symbol();
                        sym.Expression = val;
                        sym.LatexString = val;
                        Rank2Tensor[i, j] = sym;
                    }
                }
            }

        }

    }
}
