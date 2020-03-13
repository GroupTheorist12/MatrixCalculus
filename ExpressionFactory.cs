using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
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

        private string TrigDF(string trigValue)
        {
            string ret = string.Empty;
            Hashtable ht = new Hashtable();
            ht["sin"] = "cos";
            ht["cos"] = "-sin";
            return (string)ht[trigValue];
        }
        public Symbol DF(Symbol sym)
        {
            Symbol ret = new Symbol();
            List<Token> lstCopy = new List<Token>();
            List<string> LiteralBuffer = new List<string>();
            List<string> OperatorBuffer = new List<string>();
            List<string> VariableBuffer = new List<string>();
            List<string> FunctionBuffer = new List<string>();
            List<Token> newList = new List<Token>();
            foreach (Token t in sym.Tokens)
            {
                lstCopy.Add(t);
            }

            lstCopy.Reverse();

            int i = 0;
            Rational pow = 0;
            Rational accum = 0;

            while (i < lstCopy.Count)
            {
                Token t = lstCopy[i];
                if (t.Type == "Literal")
                {
                    LiteralBuffer.Add(t.Value);
                }
                else if (t.Type == "Operator")
                {
                    if (t.Value == "^") //Power
                    {
                        pow = Rational.Parse(string.Join("", LiteralBuffer.ToArray())) - 1;
                        accum += Rational.Parse(string.Join("", LiteralBuffer.ToArray()));
                        if (pow > 1)
                        {
                            newList.Add(new Token("Literal", pow.ToString()));
                            newList.Add(new Token("Operator", "^"));

                        }
                        LiteralBuffer.Clear();
                    }
                }
                else if (t.Type == "Variable")
                {
                    if (pow > 0)
                    {
                        newList.Add(new Token("Variable", t.Value));
                    }
                    else if (FunctionBuffer.Count > 0)
                    {
                        newList.Add(new Token(" Right Parenthesis", ")"));
                        newList.Add(new Token("Variable", t.Value));
                        FunctionBuffer.Clear();
                    }
                }
                else if (t.Type == "Function")
                {
                    newList.Add(new Token(" Left Parenthesis", "("));
                    newList.Add(new Token("Function", TrigDF(t.Value)));

                }
                else if (t.Type == "Right Parenthesis")
                {
                    FunctionBuffer.Add(t.Value);
                }
                else if (t.Type == "Left Parenthesis")
                {
                    if (LiteralBuffer.Count > 0)
                    {
                        accum += Rational.Parse(string.Join("", LiteralBuffer.ToArray()));
                        Rational tmp = Rational.Parse(string.Join("", LiteralBuffer.ToArray()));
                        newList.Add(new Token("Literal", tmp.ToString()));
                        LiteralBuffer.Clear();

                    }

                }
                i++;
            }

            if (LiteralBuffer.Count > 0)
            {
                accum *= Rational.Parse(string.Join("", LiteralBuffer.ToArray()));

                if (pow > 0)
                {
                    newList.Add(new Token("Operator", "*"));

                }

                newList.Add(new Token("Literal", accum.ToString()));

            }
            else if(accum > 0)
            {
                newList.Add(new Token("Literal", accum.ToString()));

            }
            newList.Reverse();
            newList[newList.Count - 1].SymbolEnd = true;
            ret.Tokens = newList;
            return ret;
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
                        if(i != FunctionString.Length - 1 && !InBracket)
                        {
                            TokenList.Add(new Token("Operator", "*"));
                        }
                        variableBuffer.Clear();
                    }
                    else
                    {
                        letterBuffer.Add(ch.ToString());
                        variableBuffer.Add(ch.ToString());
                    }

                }
                else if (ch == '^') //numbers coming
                {
                    TokenList.Add(new Token("Operator", ch.ToString()));
                }
                else if (ch == '/')
                {
                    emptyNumberBufferAsLiteral();
                    TokenList.Add(new Token("Operator", ch.ToString()));
                }
                else if (ch == ' ') //space denotes polynomial. everything before is complete
                {
                    //TokenList[TokenList.Count - 1].SymbolEnd = true;

                }
                else if (ch == '+')
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

            this.symbolList = new SymbolList(TokenList);
        }
    }
}
