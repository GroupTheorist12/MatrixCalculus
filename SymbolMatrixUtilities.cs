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
                    Symbol sym = new Symbol(kgL[cnt++]); 
                    sym.IsExpression = true;
                    ret[i, j] = sym;
                }
            }
            
            ret.FullRep = @"C_3 = " + ret.ToLatex();
            return ret;

        }

        public static SymbolMatrix C3(List<string> variables)
        {
            List<string> kgL = new List<string>
            {
                "a", "b", "c",
                "c", "a", "b",
                "b", "c", "a" 
            };

            List<string> mapper = kgL.Take(3).ToList();

            SymbolMatrix ret = new SymbolMatrix(3, 3);
            int cnt = 0;
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    int ind = mapper.FindIndex(t => t == kgL[cnt]);

                    Symbol sym = new Symbol(variables[ind]); 
                    cnt++;
                    sym.IsExpression = true;
                    ret[i, j] = sym;
                }
            }
            
            ret.FullRep = @"C_3 = " + ret.ToLatex();
            return ret;

        }

        public static SymbolMatrix C3RowColumn()
        {
            List<string> kgL = new List<string>
            {
                "a", "b", "c",
                "d", "e", "f",
                "g", "h", "i" 
            };

            SymbolMatrix ret = new SymbolMatrix(3, 3);
            int cnt = 0;
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    Symbol sym = new Symbol(kgL[cnt++]); 
                    sym.IsExpression = true;
                    ret[i, j] = sym;
                }
            }
            
            ret.FullRep = @"C_2 = " + ret.ToLatex();
            return ret;

        }

        public static SymbolMatrix C4RowColumn()
        {
            List<string> kgL = new List<string>
            {
                "a", "b", "c", "d",
                "e", "f", "g", "h", 
                "i", "j", "k", "l",
                "m", "n", "o", "p",
            };

            SymbolMatrix ret = new SymbolMatrix(4, 4);
            int cnt = 0;
            for (int i = 0; i < ret.Rows; i++)
            {
                for (int j = 0; j < ret.Columns; j++)
                {
                    Symbol sym = new Symbol(kgL[cnt++]); 
                    sym.IsExpression = true;
                    ret[i, j] = sym;
                }
            }
            
            ret.FullRep = @"C_4 = " + ret.ToLatex();
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
                    Symbol sym = new Symbol(kgL[cnt++]); 
                    sym.IsExpression = true;
                    ret[i, j] = sym;
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
                    Symbol sym = new Symbol(kgL[cnt++]); 
                    sym.IsExpression = true;
                    ret[i, j] = sym;
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
                    Symbol sym = new Symbol(kgL[cnt++]); 
                    sym.IsExpression = true;
                    ret[i, j] = sym;
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
                    Symbol sym = new Symbol(kgL[cnt++]); 
                    sym.IsExpression = true;
                    ret[i, j] = sym;
                }
            }
            
            ret.FullRep = @"\mathbb{H}_{R\chi} = " + ret.ToLatex();
            return ret;
        }

        public static SymbolMatrix KroneckerProduct(SymbolMatrix a, SymbolMatrix b)
        {
            int Rows = a.Rows * b.Rows;
            int Columns = a.Columns * b.Rows;
            int incC = 0;
            int incR = 0;
            int incAMC = 0;
            int incAMR = 0;
            SymbolMatrix ret = new SymbolMatrix(Rows, Columns);
            int i = 0;
            int j = 0;
            string exp = string.Empty;

            for(i = 0; i < ret.Rows; i++)
            {
                if(incR > b.Rows - 1)
                {
                    incR = 0;
                    incAMR++;
                }
                incAMC = 0;
                for(j = 0; j < ret.Columns; j++)
                {
                    exp = a[incAMR, incAMC].Expression + b[incR, incC].Expression;
                    incC++;
                    if(incC > b.Columns - 1)
                    {
                        incC = 0;
                        incAMC++;    
                    }

                    //exp = a[i, j].Expression + b[i, j].Expression;
                    ret[i, j] = new Symbol(exp);
                }
                incR++;

            }

            ret.FullRep = a.ToLatex() + @"\;\otimes\;" + b.ToLatex() + " = " + ret.ToLatex();
            return ret;
        }
        
        public static SymbolMatrix D3()
        {
            SymbolMatrix ret = new SymbolMatrix(6, 6);
            
            SymbolMatrix smC3 = C3();
            SymbolMatrix C3Flip = SymbolMatrixUtilities.C3().Flip().ReName(new List<string>{"d", "e", "f"});

            int i = 0;
            int j = 0;
            for(i = 0; i < smC3.Rows; i++)
            {
                for(j = 0; j < smC3.Columns; j++)
                {
                    ret[i, j] = smC3[i, j];
                }
            }

            for(i = 3; i < smC3.Rows + 3; i++)
            {
                for(j = 0; j < smC3.Columns; j++)
                {
                    ret[i, j] = C3Flip[i - 3, j];
                }
            }

            for(i = 3; i < smC3.Rows + 3; i++)
            {
                for(j = 3; j < smC3.Columns + 3; j++)
                {
                    ret[i, j] = smC3[i - 3, j - 3];
                }
            }

            for(i = 0; i < smC3.Rows; i++)
            {
                for(j = 3; j < smC3.Columns + 3; j++)
                {
                    ret[i, j] = C3Flip[i, j - 3];
                }
            }
            
            return ret;
        }
    }

    
}
