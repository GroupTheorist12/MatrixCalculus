using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;

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
            RowColumnExpression ret = new RowColumnExpression(a.Order, a.Expression + " * " + b.Expression); //default it

            /*
            //aij aik passed in
            if(a.BaseExpressions.Exists(e => e == a.Expression) && b.BaseExpressions.Exists(e => e == b.Expression))
            {
                if(a.Expression == b.Expression)
                {
                    ret = new RowColumnExpression(a.Order, a.Expression + "^2");
                }
            }
            */
            return ret;

        }
        public static RowColumnExpression operator +(RowColumnExpression a, RowColumnExpression b)
        {
            if(a.Order != b.Order)
            {
                throw new Exception("RowColumnExpression values must have same Order");
            }
            RowColumnExpression ret = new RowColumnExpression(a.Order, a.Expression + " + " + b.Expression); //default it

            /*
            //aij aik passed in
            if(a.BaseExpressions.Exists(e => e == a.Expression) && b.BaseExpressions.Exists(e => e == b.Expression))
            {
                if(a.Expression == b.Expression)
                {
                    ret = new RowColumnExpression(a.Order, "2*" + a.Expression);
                }
            }
            */

            return ret;
        }
        private void SetBaseExpressions()
        {
            int Rows = this.Order;
            int Columns = this.Order;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    BaseExpressions.Add(string.Format("a{0}{1}", i.ToString(), j.ToString()));
                }
            }
        }


    }
}
