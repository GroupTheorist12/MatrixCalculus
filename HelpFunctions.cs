using System;

namespace MatrixCalculus
{
    public class HelpFunctions
    {
        public static int[,] IntArrayFromDouble(double[][] input)
        {
            int rows = input.GetLength(0);
            int columns = input.GetLength(1);

            int[,] retVal = new int[rows, columns];

            for (int rowCount = 0; rowCount < rows; rowCount++)
            {
                for (int colCount = 0; colCount < columns; colCount++)
                {
                    retVal[rowCount, colCount] = (int)input[rowCount][colCount];
                }
            }

            return retVal;
        }

        public static double[][] DoubleArrayFromInt(int[,] input)
        {
            int rows = input.GetLength(0);
            int columns = input.GetLength(1);

            double[][] retVal = MatrixCreate(rows, columns);

            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < columns; colIndex++)
                {
                    retVal[rowIndex][colIndex] = (double)input[rowIndex, colIndex];
                }
            }

            return retVal;
        }


        public static double[,] DoubleFromDoubleArray(double[][] input)
        {
            int rows = input.GetLength(0);
            int columns = input.GetLength(0);

            double[,] retVal = new double[rows, columns];

            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < columns; colIndex++)
                {
                    retVal[rowIndex, colIndex] = input[rowIndex][colIndex];
                }
            }

            return retVal;
        }

        public static double[][] DoubleArrayFromDouble(double[,] input)
        {
            int rows = input.GetLength(0);
            int columns = input.GetLength(1);

            double[][] retVal = MatrixCreate(rows, columns);

            for (int rowIndex = 0; rowIndex < rows; rowIndex++)
            {
                for (int colIndex = 0; colIndex < columns; colIndex++)
                {
                    retVal[rowIndex][colIndex] = input[rowIndex, colIndex];
                }
            }

            return retVal;
        }


        public static void Test()
        {
            Console.WriteLine("\nBegin matrix inverse using Crout LU decomp demo \n");

            double[][] m = MatrixCreate(4, 4);
            m[0][0] = 3.0; m[0][1] = 7.0; m[0][2] = 2.0; m[0][3] = 5.0;
            m[1][0] = 1.0; m[1][1] = 8.0; m[1][2] = 4.0; m[1][3] = 2.0;
            m[2][0] = 2.0; m[2][1] = 1.0; m[2][2] = 9.0; m[2][3] = 3.0;
            m[3][0] = 5.0; m[3][1] = 4.0; m[3][2] = 7.0; m[3][3] = 1.0;


            Console.WriteLine("Original matrix m is ");
            Console.WriteLine(MatrixAsString(m));

            double d = MatrixDeterminant(m);
            if (Math.Abs(d) < 1.0e-5)
                Console.WriteLine("Matrix has no inverse");

            double[][] inv = MatrixInverse(m);

            Console.WriteLine("Inverse matrix inv is ");
            Console.WriteLine(MatrixAsString(inv));

            double[][] prod = MatrixProduct(m, inv);
            Console.WriteLine("The product of m * inv is ");
            Console.WriteLine(MatrixAsString(prod));

            Console.WriteLine("========== \n");

            double[][] lum;
            int[] perm;
            int toggle = MatrixDecompose(m, out lum, out perm);
            Console.WriteLine("The combined lower-upper decomposition of m is");
            Console.WriteLine(MatrixAsString(lum));

            double[][] lower = ExtractLower(lum);
            double[][] upper = ExtractUpper(lum);

            Console.WriteLine("The lower part of LUM is");
            Console.WriteLine(MatrixAsString(lower));

            Console.WriteLine("The upper part of LUM is");
            Console.WriteLine(MatrixAsString(upper));

            Console.WriteLine("The perm[] array is");
            ShowVector(perm);

            double[][] lowTimesUp = MatrixProduct(lower, upper);
            Console.WriteLine("The product of lower * upper is ");
            Console.WriteLine(MatrixAsString(lowTimesUp));


            Console.WriteLine("\nEnd matrix inverse demo \n");
            Console.ReadLine();

        }

        public static void ShowVector(int[] vector)
        {
            Console.Write("   ");
            for (int vectorCount = 0; vectorCount < vector.Length; ++vectorCount)
                Console.Write(vector[vectorCount] + " ");
            Console.WriteLine("\n");
        }

        public static double[][] MatrixInverse(double[][] matrix)
        {
            // Assumes determinant is not 0
            // that is, the matrix does have an inverse
            int mtxLength = matrix.Length;
            double[][] result = MatrixCreate(mtxLength, mtxLength); // Make a copy of matrix
            for (int rowCount = 0; rowCount < mtxLength; ++rowCount)
                for (int colCount = 0; colCount < mtxLength; ++colCount)
                    result[rowCount][colCount] = matrix[rowCount][colCount];

            double[][] lum; // Combined lower & upper
            int[] perm;
            int toggle;
            toggle = MatrixDecompose(matrix, out lum, out perm);

            double[] b = new double[mtxLength];
            for (int rowCount = 0; rowCount < mtxLength; ++rowCount)
            {
                for (int colCount = 0; colCount < mtxLength; ++colCount)
                    if (rowCount == perm[colCount])
                        b[colCount] = 1.0;
                    else
                        b[colCount] = 0.0;

                double[] x = Helper(lum, b);  
                for (int colCount = 0; colCount < mtxLength; ++colCount)
                    result[colCount][rowCount] = x[colCount];
            }
            return result;
        }

        /// <summary>
        /// Crout's LU decomposition for matrix determinant and inverse
        /// stores combined lower & upper in lum[][]
        /// stores row permuations into perm[]
        /// returns +1 or -1 according to even or odd number of row permutations
        /// lower gets dummy 1.0s on diagonal (0.0s above)
        /// upper gets lum values on diagonal (0.0s below)
        /// </summary>
        /// <param name="m"></param>
        /// <param name="lum"></param>          //TODO: need more descriptive name for lum, not well explained in the code. GPG
        /// <param name="permutations"></param>
        /// <returns></returns>
        public static int MatrixDecompose(double[][] m, out double[][] lum, out int[] permutations)
        {
            int toggle = +1; // even (+1) or odd (-1) row permutatuions
            int mtxLength = m.Length;

            // make a copy of m[][] into result lu[][]
            lum = MatrixCreate(mtxLength, mtxLength);
            for (int rowCount = 0; rowCount < mtxLength; ++rowCount)
                for (int colCount = 0; colCount < mtxLength; ++colCount)
                    lum[rowCount][colCount] = m[rowCount][colCount];

            // make perm[]
            permutations = new int[mtxLength];
            for (int rowCount = 0; rowCount < mtxLength; ++rowCount)
                permutations[rowCount] = rowCount;

            for (int colCount = 0; colCount < mtxLength - 1; ++colCount) // Process by column. note n-1 
            {
                double max = Math.Abs(lum[colCount][colCount]);
                int piv = colCount;

                for (int i = colCount + 1; i < mtxLength; ++i) // Find pivot index
                {
                    double xij = Math.Abs(lum[i][colCount]);
                    if (xij > max)
                    {
                        max = xij;
                        piv = i;
                    }
                } // i

                if (piv != colCount)
                {
                    double[] tmp = lum[piv]; // Swap rows j, piv
                    lum[piv] = lum[colCount];
                    lum[colCount] = tmp;

                    int t = permutations[piv]; // Swap perm elements
                    permutations[piv] = permutations[colCount];
                    permutations[colCount] = t;

                    toggle = -toggle;
                }

                double xjj = lum[colCount][colCount];
                if (xjj != 0.0)
                {
                    for (int i = colCount + 1; i < mtxLength; ++i)
                    {
                        double xij = lum[i][colCount] / xjj;
                        lum[i][colCount] = xij;
                        for (int k = colCount + 1; k < mtxLength; ++k)
                            lum[i][k] -= xij * lum[colCount][k];
                    }
                }

            } // j

            return toggle;
        }

        public static double[] Helper(double[][] luMatrix, double[] b)
        {
            int mtxLength = luMatrix.Length;
            double[] x = new double[mtxLength];
            b.CopyTo(x, 0);

            for (int rowCount = 1; rowCount < mtxLength; ++rowCount)
            {
                double sum = x[rowCount];
                for (int colCount = 0; colCount < rowCount; ++colCount)
                    sum -= luMatrix[rowCount][colCount] * x[colCount];
                x[rowCount] = sum;
            }

            x[mtxLength - 1] /= luMatrix[mtxLength - 1][mtxLength - 1];
            for (int rowcount = mtxLength - 2; rowcount >= 0; --rowcount)
            {
                double sum = x[rowcount];
                for (int colCount = rowcount + 1; colCount < mtxLength; ++colCount)
                    sum -= luMatrix[rowcount][colCount] * x[colCount];
                x[rowcount] = sum / luMatrix[rowcount][rowcount];
            }

            return x;
        }

        //TODO: need more descriptive name for lum; can't suss out what we're iterating over here. GPG
        public static double MatrixDeterminant(double[][] matrix)
        {
            double[][] lum;
            int[] perm;
            int toggle = MatrixDecompose(matrix, out lum, out perm);
            double retVal = toggle;
            for (int elementCount = 0; elementCount < lum.Length; ++elementCount)
                retVal *= lum[elementCount][elementCount];
            return retVal;
        }

        public static double[][] MatrixCreate(int rows, int cols)
        {
            double[][] retVal = new double[rows][];
            for (int rowCount = 0; rowCount < rows; ++rowCount)
                retVal[rowCount] = new double[cols];
            return retVal;
        }

        public static double[][] MatrixProduct(double[][] matrixA, double[][] matrixB)
        {
            int aRows = matrixA.Length;
            int aCols = matrixA[0].Length;
            int bRows = matrixB.Length;
            int bCols = matrixB[0].Length;
            if (aCols != bRows)
                throw new Exception("Non-conformable matrices");

            double[][] result = MatrixCreate(aRows, bCols);

            for (int ARowCount = 0; ARowCount < aRows; ++ARowCount) // Each row of A
                for (int BColCount = 0; BColCount < bCols; ++BColCount) // Each col of B
                    for (int AColCount = 0; AColCount < aCols; ++AColCount) // Could use k < bRows
                        result[ARowCount][BColCount] += matrixA[ARowCount][AColCount] * matrixB[AColCount][BColCount];

            return result;
        }

        public static string MatrixAsString(double[][] matrix)
        {
            string retval = string.Empty;
            for (int rowCount = 0; rowCount < matrix.Length; ++rowCount)
            {
                for (int colCount = 0; colCount < matrix[rowCount].Length; ++colCount)
                {
                    retval += matrix[rowCount][colCount].ToString("F3").PadLeft(8) + " ";
                }

                retval += Environment.NewLine;
            }
            return retval;
        }

        /// <summary>
        /// Extracts the lower part of an LU Doolittle composition
        /// </summary>
        /// <param name="lum"></param>
        /// <returns>lower part of an LU Doolittle decomposition (dummy 1.0s on diagonal, 0.0s above)</returns>
        //TODO: need more descriptive name for lum; can't suss out what we're iterating over here. GPG
        //TODO: don't understand why you're starting with the 0th element of the row or column and then prefix-incrementing the counter of the loops GPG
        public static double[][] ExtractLower(double[][] lum)
        {
            int totalElements = lum.Length;
            double[][] retVal = MatrixCreate(totalElements, totalElements);
            for (int rowCount = 0; rowCount < totalElements; ++rowCount)
            {
                for (int colCount = 0; colCount < totalElements; ++colCount)
                {
                    if (rowCount == colCount)
                        retVal[rowCount][colCount] = 1.0;
                    else if (rowCount > colCount)
                        retVal[rowCount][colCount] = lum[rowCount][colCount];
                }
            }
            return retVal;
        }

        /// <summary>
        /// Extracts the upper part of an LU Doolittle composition
        /// </summary>
        /// <param name="lum"></param>
        /// <returns>upper part of an LU (lu values on diagional and above, 0.0s below)</returns>
        //TODO: need more descriptive name for lum; can't suss out what we're iterating over here. GPG
        //TODO: don't understand why you're starting with the 0th element of the row or column and then prefix-incrementing the counter of the loops GPG
        public static double[][] ExtractUpper(double[][] lum)
        {
            int totalElements = lum.Length;
            double[][] result = MatrixCreate(totalElements, totalElements);
            for (int rowCount = 0; rowCount < totalElements; ++rowCount)
            {
                for (int colCount = 0; colCount < totalElements; ++colCount)
                {
                    if (rowCount <= colCount)
                        result[rowCount][colCount] = lum[rowCount][colCount];
                }
            }
            return result;
        }
    }
}