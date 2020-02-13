using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatrixCalculus
{
    public class Symbol
    {
        public List<Token> Tokens
        {
            get;
            set;
        }

        public bool IsOperator{get;set;}
        public Symbol()
        {
            this.Tokens = new List<Token>();
            IsOperator = false;
        }
        
        public string NakedTokenString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < Tokens.Count; i++)
                {
                    Token t = Tokens[i];
                    sb.Append(t.Value);

                }
                return sb.ToString();
            }
        }

    }
}