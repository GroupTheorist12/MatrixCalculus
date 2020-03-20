using System;
using System.Collections.Generic;

namespace MatrixCalculus
{
    public class RowColumnExpression
    {
        public string Expression {get;set;}        

        public int Order{get;set;}

        private List<string> BaseExpressions = new List<string>();

        private RowColumnExpression()
        {
            Order = 2;
            SetBaseExpressions();
        }

        public RowColumnExpression(int Order, string Expression)
        {
            this.Order = Order;
            this.Expression = Expression;
            SetBaseExpressions();
        }

        public static RowColumnExpression operator *(RowColumnExpression a, RowColumnExpression b)
        {
            if(a.Order != b.Order)
            {
                throw new Exception("RowColumnExpression values must have same Order");
            }
            RowColumnExpression retVal = new RowColumnExpression(a.Order, a.Expression + " * " + b.Expression); //default it

            return retVal;
        }

        public static RowColumnExpression operator +(RowColumnExpression a, RowColumnExpression b)
        {
            if(a.Order != b.Order)
            {
                throw new Exception("RowColumnExpression values must have same Order");
            }
            RowColumnExpression retVal = new RowColumnExpression(a.Order, a.Expression + " + " + b.Expression); //default it

            return retVal;
        }

        private void SetBaseExpressions()
        {
            int Rows = this.Order;
            int Columns = this.Order;

            for (int rowCount = 0; rowCount < Rows; rowCount++)
            {
                for (int colCount = 0; colCount < Columns; colCount++)
                {
                    BaseExpressions.Add(string.Format("a{0}{1}", rowCount.ToString(), colCount.ToString()));
                }
            }
        }
    }
}
