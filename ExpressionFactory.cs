using System.Collections;
using System.Collections.Generic;
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
            Hashtable ht = new Hashtable();
            ht["sin"] = "cos";
            ht["cos"] = "-sin";
            return (string)ht[trigValue];
        }

        public Symbol DF(Symbol sym)
        {
            Symbol retVal = new Symbol();
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

            int counter = 0;
            Rational pow = 0;
            Rational accum = 0;

            while (counter < lstCopy.Count)
            {
                Token t = lstCopy[counter];
                if (t.Type == "Literal")
                {
                    LiteralBuffer.Add(t.Value);
                }
                else if (t.Type == "Operator")
                {
                    if (t.Value == "^") // Exponent
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
                counter++;
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
            else if (accum > 0)
            {
                newList.Add(new Token("Literal", accum.ToString()));
            }
            newList.Reverse();
            newList[newList.Count - 1].SymbolEnd = true;
            retVal.Tokens = newList;
            return retVal;
        }
        public void ParseExpression(string FunctionString)
        {
            int element = 0;
            TokenList.Clear();

            Symbol sym = new Symbol();

            bool InBracket = false;
            bool InUnderScore = false;

            while (element < FunctionString.Length)
            {
                char ch = FunctionString[element];
                if (ch == '-' && FunctionString[element + 1] != ' ') // Negative number. Parser needs space for +-*/
                {
                    numberBuffer.Add("-1");

                }
                else if (isDigit(ch))
                {
                    if (InUnderScore) // Subscript variables such as x_1
                    {
                        InUnderScore = false;
                        letterBuffer.Clear(); // Contained in variable buffer. subscript
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
                        letterBuffer.Clear(); // Contained in variable buffer. subscript
                        variableBuffer.Add(ch.ToString());
                        Token t = new Token("Variable", string.Join("", variableBuffer.ToArray()));
                        t.SymbolEnd = false;
                        TokenList.Add(t);
                        if (element != FunctionString.Length - 1 && !InBracket)
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
                else if (ch == '^') // Number's coming
                {
                    TokenList.Add(new Token("Operator", ch.ToString()));
                }
                else if (ch == '/')
                {
                    emptyNumberBufferAsLiteral();
                    TokenList.Add(new Token("Operator", ch.ToString()));
                }
                else if (ch == ' ') // Space denotes polynomial, everything before is complete
                {
                    //TokenList[TokenList.Count - 1].SymbolEnd = true;
                    // Is this else if block necessary? GPG
                }
                else if (ch == '+')
                {
                    emptyNumberBufferAsLiteral();
                    TokenList[TokenList.Count - 1].SymbolEnd = (InBracket) ? false : true;
                    TokenList.Add(new Token("Operator", " " + ch.ToString() + " "));
                }
                else if (ch == '-' && FunctionString[element + 1] == ' ')
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

                element++;
            }

            emptyNumberBufferAsLiteral();
            TokenList[TokenList.Count - 1].SymbolEnd = true;

            this.symbolList = new SymbolList(TokenList);
        }
    }
}
