using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MatrixCalculus
{
    public class SymbolMatrixUtilities
    {
        public static Symbol[,] SymbolFromSymbolArray(Symbol[][] input)
        {
            int rows = input.GetLength(0);
            int columns = input.GetLength(0);

            Symbol[,] ret = new Symbol[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    ret[i, j] = input[i][j];
                }
            }

            return ret;
        }
        
        public static Symbol[][] MatrixCreate(int rows, int cols)
        {
            Symbol[][] result = new Symbol[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new Symbol[cols];
            return result;
        }

        public static SymbolMatrix C3()
        {
            List<string> kgL = new List<string>
            {
                "a", "b", "c",
                "c", "a", "b",
                "b", "c", "a" 
            };

            SymbolMatrix ret = new SymbolMatrix(3, 3);
            int cnt = 0;
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = new Symbol(kgL[cnt++]);
                }
            }
            
            ret.FullRep = @"C_2 = " + ret.ToLatex();
            return ret;

        }

        public static SymbolMatrix C2()
        {
            List<string> kgL = new List<string>
            {
                "a", "b", 
                "b", "a" 
            };

            SymbolMatrix ret = new SymbolMatrix(2, 2);
            int cnt = 0;
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = new Symbol(kgL[cnt++]);
                }
            }
            
            ret.FullRep = @"C_2 = " + ret.ToLatex();
            return ret;

        }
        public static SymbolMatrix KleinGroup()
        {
            List<string> kgL = new List<string>
            {
                "a", "b", "c", "d", 
                "b", "a", "d", "c", 
                "c", "d", "a", "b", 
                "d", "c", "b", "a"
            };

            SymbolMatrix ret = new SymbolMatrix(4, 4);

            int cnt = 0;
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = new Symbol(kgL[cnt++]);
                }
            }
            
            ret.FullRep = @"C_2\;\times\;C_2 = " + ret.ToLatex();
            return ret;
        }

        public static SymbolMatrix LeftChiralQuaternion()
        {
            List<string> kgL = new List<string>
            {
                "a", "b", "c", "d", 
                "-b", "a", "-d", "c", 
                "-c", "d", "a", "-b", 
                "-d", "-c", "b", "a"
            };

            SymbolMatrix ret = new SymbolMatrix(4, 4);

            int cnt = 0;
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = new Symbol(kgL[cnt++]);
                }
            }
            
            ret.FullRep = @"\mathbb{H}_{L\chi} = " + ret.ToLatex();
            return ret;
        }

        public static SymbolMatrix RightChiralQuaternion()
        {
            List<string> kgL = new List<string>
            {
                "a", "b", "c", "d", 
                "-b", "a", "d", "-c", 
                "-c", "-d", "a", "b", 
                "-d", "c", "-b", "a"
            };

            SymbolMatrix ret = new SymbolMatrix(4, 4);

            int cnt = 0;
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    ret[i, j] = new Symbol(kgL[cnt++]);
                }
            }
            
            ret.FullRep = @"\mathbb{H}_{R\chi} = " + ret.ToLatex();
            return ret;
        }

    }

    
}
