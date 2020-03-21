namespace MatrixCalculus
{
    public class RealFactory
    {
        public SquareRealMatrix this[int Rows, int Columns, params double[] exps]
        {
            get
            {
                SquareRealMatrix retVal = new SquareRealMatrix(Rows, Columns);
                retVal.Parent = this;
                int cnt = 0;

                for (int rowCount = 0; rowCount < Rows; rowCount++)
                {
                    for (int colCount = 0; colCount < Columns; colCount++)
                    {
                        retVal[rowCount, colCount] = exps[cnt++];
                    }
                }

                return retVal;
            }
        }
    }
}
