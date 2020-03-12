using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MatrixCalculus
{
    public class ExpressionFactory
    {
        public List<string> Variables = new List<string>();
        public List<Token> TokenList { get; set; }
        public SymbolList symbolList { get; set; }
        public List<string> letterBuffer = new List<string>();
        public List<string> variableBuffer = new List<string>();
        public List<string> numberBuffer = new List<string>();

        public ExpressionFactory()
        {
            TokenList = new List<Token>();
            symbolList = new SymbolList();
        }

        public bool isComma(char ch)
        {
            return new Regex(@"/,/").IsMatch(ch.ToString());
        }

        public bool isDigit(char ch)
        {
            return new Regex(@"\d").IsMatch(ch.ToString());
        }

        public bool isLetter(char ch)
        {
            return char.IsLetter(ch);
        }

        public bool isOperator(char ch)
        {
            return new Regex(@"\+|\-|\*|\/|\^").IsMatch(ch.ToString());
        }

        public bool isLeftParenthesis(char ch)
        {
            return new Regex(@"\(").IsMatch(ch.ToString());
        }

        public bool isRightParenthesis(char ch)
        {
            return new Regex(@"\)").IsMatch(ch.ToString());
        }

        public bool isUnderScore(char ch)
        {
            return new Regex(@"\\_").IsMatch(ch.ToString());
        }

        private void emptyNumberBufferAsLiteral()
        {
            if (numberBuffer.Count > 0)
            {
                TokenList.Add(new Token("Literal", string.Join("", numberBuffer.ToArray())));
                numberBuffer.Clear();
            }
        }

        public void ParseExpression(string FunctionString)
        {
            int i = 0;
            TokenList.Clear();

            Symbol sym = new Symbol();

            bool InBracket = false;
            bool InUnderScore = false;

            while (i < FunctionString.Length)
            {
                char ch = FunctionString[i];
                if (ch == '-' && FunctionString[i + 1] != ' ') //negative number. Parser needs space for +-*/
                {
                    numberBuffer.Add("-1");

                }
                else if (isDigit(ch))
                {
                    if (InUnderScore)//subscript variables such as x_1
                    {
                        InUnderScore = false;
                        letterBuffer.Clear(); //contained in variable buffer. subscript
                        variableBuffer.Add(ch.ToString());
                        Token t = new Token("Variable", string.Join("", variableBuffer.ToArray()));
                        t.SymbolEnd = false;
                        TokenList.Add(t);
                        variableBuffer.Clear();
                    }
                    else
                    {
                        numberBuffer.Add(ch.ToString());
                    }
                }
                else if (ch == '.')
                {
                    numberBuffer.Add(ch.ToString());
                }
                else if (ch == '_')
                {
                    InUnderScore = true;
                    variableBuffer.Add(ch.ToString());
                }
                else if (isLetter(ch))
                {
                    if (numberBuffer.Count > 0)
                    {
                        emptyNumberBufferAsLiteral();
                        TokenList.Add(new Token("Operator", "*"));
                    }

                    if (Variables.Exists(v => v == ch.ToString()))
                    {
                        letterBuffer.Clear(); //contained in variable buffer. subscript
                        variableBuffer.Add(ch.ToString());
                        Token t = new Token("Variable", string.Join("", variableBuffer.ToArray()));
                        t.SymbolEnd = false;
                        TokenList.Add(t);
                        variableBuffer.Clear();
                    }
                    else
                    {
                        letterBuffer.Add(ch.ToString());
                        variableBuffer.Add(ch.ToString());
                    }

                }
                else if(ch == '^') //numbers coming
                {
                    TokenList.Add(new Token("Operator", ch.ToString()));
                }
                else if(ch == '/')
                {
                    emptyNumberBufferAsLiteral();
                    TokenList.Add(new Token("Operator", ch.ToString()));
                }
                else if(ch == ' ') //space denotes polynomial. everything before is complete
                {
                    //TokenList[TokenList.Count - 1].SymbolEnd = true;

                }
                else if(ch == '+')
                {
                    emptyNumberBufferAsLiteral();
                    TokenList[TokenList.Count - 1].SymbolEnd = (InBracket) ? false : true;
                    TokenList.Add(new Token("Operator", " " + ch.ToString() + " "));
                }
                else if (ch == '-' && FunctionString[i + 1] == ' ')
                {
                    emptyNumberBufferAsLiteral();
                    TokenList[TokenList.Count - 1].SymbolEnd = true;
                    TokenList.Add(new Token("Operator", " " + ch.ToString() + " "));

                }
                else if (isLeftParenthesis(ch))
                {
                    InBracket = true;
                    if (letterBuffer.Count > 0)
                    {
                        TokenList.Add(new Token("Function", string.Join("", letterBuffer.ToArray())));
                        letterBuffer.Clear();
                        variableBuffer.Clear();
                    }
                    else if (numberBuffer.Count > 0)
                    {
                        emptyNumberBufferAsLiteral();
                        TokenList.Add(new Token("Operator", "*"));
                    }
                    TokenList.Add(new Token("Left Parenthesis", ch.ToString()));
                }
                else if (isRightParenthesis(ch))
                {
                    InBracket = false;
                    emptyNumberBufferAsLiteral();
                    TokenList.Add(new Token("Right Parenthesis", ch.ToString()));
                }


                i++;
            }

            emptyNumberBufferAsLiteral();
            TokenList[TokenList.Count - 1].SymbolEnd = true;


        }
    }
}
