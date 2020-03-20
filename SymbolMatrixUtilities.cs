using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace MatrixCalculus
{
    public enum SymbolType
    {
        Expression,
        Formula,
        Equation,
        GroupMember,
        Rational,
        Real,
        Integer
    }
    public class SymbolMatrixUtilities
    {
        public static Symbol[,] SymbolFromSymbolArray(Symbol[][] input)
        {
            int rows = input.GetLength(0);
            int columns = input.GetLength(0);

            Symbol[,] retVal = new Symbol[rows, columns];

            for (int rowCount = 0; rowCount < rows; rowCount++)
            {
                for (int colCount = 0; colCount < columns; colCount++)
                {
                    retVal[rowCount, colCount] = input[rowCount][colCount];
                }
            }

            return retVal;
        }
        
        public static Symbol[][] MatrixCreate(int rows, int cols)
        {
            Symbol[][] retVal = new Symbol[rows][];
            for (int rowCount = 0; rowCount < rows; ++rowCount)
                retVal[rowCount] = new Symbol[cols];
            return retVal;
        }

        public static SymbolMatrix C3()
        {
            List<string> kgL = new List<string>
            {
                "a", "b", "c",
                "c", "a", "b",
                "b", "c", "a" 
            };

            SymbolMatrix retVal = new SymbolMatrix(3, 3);
            int cnt = 0;
            for (int rows = 0; rows < retVal.Rows; rows++)
            {
                for (int cols = 0; cols < retVal.Columns; cols++)
                {
                    Symbol sym = new Symbol(kgL[cnt++]); 
                    sym.IsExpression = true;
                    retVal[rows, cols] = sym;
                }
            }
            
            retVal.FullRep = @"C_3 = " + retVal.ToLatex();
            return retVal;
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

            SymbolMatrix retVal = new SymbolMatrix(3, 3);
            int cnt = 0;
            for (int rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    int ind = mapper.FindIndex(t => t == kgL[cnt]);

                    Symbol sym = new Symbol(variables[ind]); 
                    cnt++;
                    sym.IsExpression = true;
                    retVal[rowCount, colCount] = sym;
                }
            }
            
            retVal.FullRep = @"C_3 = " + retVal.ToLatex();
            return retVal;
        }

        public static SymbolMatrix C3RowColumn()
        {
            List<string> kgL = new List<string>
            {
                "a", "b", "c",
                "d", "e", "f",
                "g", "h", "i" 
            };

            SymbolMatrix retVal = new SymbolMatrix(3, 3);
            int cnt = 0;
            for (int rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    Symbol sym = new Symbol(kgL[cnt++]); 
                    sym.IsExpression = true;
                    retVal[rowCount, colCount] = sym;
                }
            }
            
            retVal.FullRep = @"C_2 = " + retVal.ToLatex();
            return retVal;
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

            SymbolMatrix retVal = new SymbolMatrix(4, 4);
            int cnt = 0;
            for (int rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    Symbol sym = new Symbol(kgL[cnt++]); 
                    sym.IsExpression = true;
                    retVal[rowCount, colCount] = sym;
                }
            }
            
            retVal.FullRep = @"C_4 = " + retVal.ToLatex();
            return retVal;
        }


        public static SymbolMatrix C2()
        {
            List<string> kgL = new List<string>
            {
                "a", "b", 
                "b", "a" 
            };

            SymbolMatrix retVal = new SymbolMatrix(2, 2);
            int cnt = 0;
            for (int rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    Symbol sym = new Symbol(kgL[cnt++]); 
                    sym.IsExpression = true;
                    retVal[rowCount, colCount] = sym;
                }
            }
            
            retVal.FullRep = @"C_2 = " + retVal.ToLatex();
            return retVal;

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

            SymbolMatrix retVal = new SymbolMatrix(4, 4);

            int cnt = 0;
            for (int rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    Symbol sym = new Symbol(kgL[cnt++]); 
                    sym.IsExpression = true;
                    retVal[rowCount, colCount] = sym;
                }
            }
            
            retVal.FullRep = @"C_2\;\times\;C_2 = " + retVal.ToLatex();
            return retVal;
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

            SymbolMatrix retVal = new SymbolMatrix(4, 4);

            int cnt = 0;
            for (int rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    Symbol sym = new Symbol(kgL[cnt++]); 
                    sym.IsExpression = true;
                    retVal[rowCount, colCount] = sym;
                }
            }
            
            retVal.FullRep = @"\mathbb{H}_{L\chi} = " + retVal.ToLatex();
            return retVal;
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

            SymbolMatrix retval = new SymbolMatrix(4, 4);

            int cnt = 0;
            for (int rowCount = 0; rowCount < retval.Rows; rowCount++)
            {
                for (int colCount = 0; colCount < retval.Columns; colCount++)
                {
                    Symbol sym = new Symbol(kgL[cnt++]); 
                    sym.IsExpression = true;
                    retval[rowCount, colCount] = sym;
                }
            }
            
            retval.FullRep = @"\mathbb{H}_{R\chi} = " + retval.ToLatex();
            return retval;
        }

        public static SymbolMatrix KroneckerProduct(SymbolMatrix a, SymbolMatrix b)
        {
            int Rows = a.Rows * b.Rows;         // calculate number of rows.
            int Columns = a.Columns * b.Rows;   // calculate number of columns
            int incC = 0;                       // increment variable for column of b matrix
            int incR = 0;                       // increment variable for row of b matrix
            int incAMC = 0;                     // increment variable for column of a matrix
            int incAMR = 0;                     // increment variable for row of a matrix
            SymbolMatrix retVal = new SymbolMatrix(Rows, Columns);
            int rowCount;
            int colCount;
            string exp = string.Empty;

            for(rowCount = 0; rowCount < retVal.Rows; rowCount++)
            {
                if(incR > b.Rows - 1)           // reached end of rows of b matrix
                {
                    incR = 0;
                    incAMR++; 
                }
                incAMC = 0;
                for(colCount = 0; colCount < retVal.Columns; colCount++)
                {
                    exp = a[incAMR, incAMC].Expression + b[incR, incC].Expression;
                    incC++;
                    if(incC > b.Columns - 1)    // reached end of columns of b matrix
                    {
                        incC = 0;
                        incAMC++;    
                    }

                    retVal[rowCount, colCount] = new Symbol(exp);
                }
                incR++;

            }

            retVal.FullRep = a.ToLatex() + @"\;\otimes\;" + b.ToLatex() + " = " + retVal.ToLatex(); //produce latex string
            return retVal;
        }
        
        public static SymbolMatrix D3()
        {
            SymbolMatrix retVal = new SymbolMatrix(6, 6);
            
            SymbolMatrix smC3 = C3();
            SymbolMatrix C3Flip = SymbolMatrixUtilities.C3().Flip().ReName(new List<string>{"d", "e", "f"});

            int rowCount;
            int colCount;
            for(rowCount = 0; rowCount < smC3.Rows; rowCount++)
            {
                for(colCount = 0; colCount < smC3.Columns; colCount++)
                {
                    retVal[rowCount, colCount] = smC3[rowCount, colCount];
                }
            }

            for(rowCount = 3; rowCount < smC3.Rows + 3; rowCount++)
            {
                for(colCount = 0; colCount < smC3.Columns; colCount++)
                {
                    retVal[rowCount, colCount] = C3Flip[rowCount - 3, colCount];
                }
            }

            for(rowCount = 3; rowCount < smC3.Rows + 3; rowCount++)
            {
                for(colCount = 3; colCount < smC3.Columns + 3; colCount++)
                {
                    retVal[rowCount, colCount] = smC3[rowCount - 3, colCount - 3];
                }
            }

            for(rowCount = 0; rowCount < smC3.Rows; rowCount++)
            {
                for(colCount = 3; colCount < smC3.Columns + 3; colCount++)
                {
                    retVal[rowCount, colCount] = C3Flip[rowCount, colCount - 3];
                }
            }
            
            return retVal;
        }
    }
}
