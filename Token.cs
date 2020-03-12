using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MatrixCalculus
{


    public class Token
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public bool SymbolEnd { get; set; }
        public Token(string t, string v)
        {
            this.Type = t;
            this.Value = v;
            SymbolEnd = false;
        }
    }

    public class TokenFactory
    {
        public List<Token> TokenList { get; set; }
        public SymbolList symbolList { get; set; }
        public List<string> letterBuffer = new List<string>();
        public List<string> variableBuffer = new List<string>();
        public List<string> numberBuffer = new List<string>();

        public enum CoordinateSystem
        {
            x,
            y,

            z,

            Cartesian,
            Polar,

            Subscript
        }


        public class Coordinates
        {
            public int Dimension { get; set; }
            public CoordinateSystem coordinateSystem { get; set; }
            public string VariableStringValues { get; set; }

            public Coordinates(CoordinateSystem cs, int dim, string variableStringRep)
            {
                this.coordinateSystem = cs;
                this.Dimension = dim;

                VariableStringValues = variableStringRep;
            }
        }
        public Coordinates coordinates { get; set; }

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
            return new Regex(@"\_").IsMatch(ch.ToString());
        }


        public TokenFactory() //default constructor
        {
            coordinates = new Coordinates(CoordinateSystem.x, 1, "x");
            TokenList = new List<Token>();
            symbolList = new SymbolList();
        }

        public void ParseExpression(string FunctionString)
        {

            int i = 0;
            TokenList.Clear();

            Symbol sym = new Symbol();

            bool InBracket = false;
            Tokenizer tokes = new Tokenizer();
            while (i < FunctionString.Length)
            {
                char ch = FunctionString[i];
                if(ch == '-' && FunctionString[i + 1] != ' ') //negative number. Parser needs space for +-*/
                {
                    numberBuffer.Add(ch.ToString());

                }
                else if (tokes.isDigit(ch))
                {
                    numberBuffer.Add(ch.ToString());
                }
                else if (ch == '.')
                {
                    numberBuffer.Add(ch.ToString());
                }
                else if (tokes.isLetter(ch))
                {
                    if (numberBuffer.Count > 0)
                    {
                        emptyNumberBufferAsLiteral();
                        TokenList.Add(new Token("Operator", "*"));
                    }
                    letterBuffer.Add(ch.ToString());
                }
                else if (tokes.isOperator(ch))
                {
                    emptyNumberBufferAsLiteral();
                    emptyLetterBufferAsVariables();
                    char ch2  = FunctionString[i - 1];
                    if (ch2 == ' ' && !InBracket)//(ch != '^')
                    {
                        TokenList[TokenList.Count - 1].SymbolEnd = true;
                    }
                    if(ch == '^')
                    {
                        TokenList.Add(new Token("Operator", ch.ToString()));
                    }
                    else
                    {
                        TokenList.Add(new Token("Operator", " " + ch.ToString() + " "));

                    }
                }
                else if (tokes.isLeftParenthesis(ch))
                {
                    InBracket = true;
                    if (letterBuffer.Count > 0)
                    {
                        TokenList.Add(new Token("Function", string.Join("", letterBuffer.ToArray())));
                        letterBuffer.Clear();
                    }
                    else if (numberBuffer.Count > 0)
                    {
                        emptyNumberBufferAsLiteral();
                        TokenList.Add(new Token("Operator", "*"));
                    }
                    TokenList.Add(new Token("Left Parenthesis", ch.ToString()));
                }
                else if (tokes.isRightParenthesis(ch))
                {
                    InBracket = false;
                    emptyLetterBufferAsVariables();
                    emptyNumberBufferAsLiteral();
                    TokenList.Add(new Token("Right Parenthesis", ch.ToString()));
                }
                else if (tokes.isComma(ch))
                {
                    emptyNumberBufferAsLiteral();
                    emptyLetterBufferAsVariables();
                    TokenList.Add(new Token("Function Argument Separator", ch.ToString()));
                }


                i++;
            }

            emptyNumberBufferAsLiteral(true);  
            emptyLetterBufferAsVariables(true);         

            symbolList  = new SymbolList(this.TokenList); 
        }

        public void emptyNumberBufferAsLiteral(bool SymbolEnd = false)
        {
            if (numberBuffer.Count > 0)
            {
                Token t = new Token("Literal", string.Join("", numberBuffer.ToArray()));
                t.SymbolEnd = SymbolEnd;
                TokenList.Add(t);
                numberBuffer.Clear();
            }
        }

        public void emptyLetterBufferAsVariables(bool SymbolEnd = false)
        {
            var l = letterBuffer.Count;
            for (var i = 0; i < l; i++)
            {
                Token t = new Token("Variable", letterBuffer[i]);
                t.SymbolEnd = SymbolEnd;
                TokenList.Add(t);
                if (i < l - 1)
                { //there are more Variables left
                    TokenList.Add(new Token("Operator", "*"));
                }
            }
            letterBuffer.Clear();
        }

    }
    public class Tokenizer
    {
        public List<Token> result = new List<Token>();
        public List<string> letterBuffer = new List<string>();
        public List<string> numberBuffer = new List<string>();

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
            return new Regex(@"\_").IsMatch(ch.ToString());
        }

        private void emptyLetterBufferAsVariables(bool SymbolEnd = false)
        {
            var l = letterBuffer.Count;
            for (var i = 0; i < l; i++)
            {
                Token t = new Token("Variable", letterBuffer[i]);
                t.SymbolEnd = SymbolEnd;
                result.Add(t);
                if (i < l - 1)
                { //there are more Variables left
                    result.Add(new Token("Operator", "*"));
                }
            }
            letterBuffer.Clear();
        }

        private void emptyNumberBufferAsLiteral()
        {
            if (numberBuffer.Count > 0)
            {
                result.Add(new Token("Literal", string.Join("", numberBuffer.ToArray())));
                numberBuffer.Clear();
            }
        }

        public List<Token> tokenizeToSymbol(string str)
        {
            result.Clear();
            letterBuffer.Clear();
            numberBuffer.Clear();

            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                if (isDigit(ch))
                {
                    numberBuffer.Add(ch.ToString());
                }
                else if (ch == '.')
                {
                    numberBuffer.Add(ch.ToString());
                }
                else if (isLetter(ch))
                {
                    if (numberBuffer.Count > 0)
                    {
                        emptyNumberBufferAsLiteral();
                        result.Add(new Token("Operator", "*"));
                    }
                    letterBuffer.Add(ch.ToString());
                }
                else if (isOperator(ch))
                {
                    emptyNumberBufferAsLiteral();
                    emptyLetterBufferAsVariables();
                    if (ch != '^')
                    {
                        result[result.Count - 1].SymbolEnd = true;
                    }
                    result.Add(new Token("Operator", ch.ToString()));
                }
                else if (isLeftParenthesis(ch))
                {
                    if (letterBuffer.Count > 0)
                    {
                        result.Add(new Token("Function", string.Join("", letterBuffer.ToArray())));
                        letterBuffer.Clear();
                    }
                    else if (numberBuffer.Count > 0)
                    {
                        emptyNumberBufferAsLiteral();
                        result.Add(new Token("Operator", "*"));
                    }
                    result.Add(new Token("Left Parenthesis", ch.ToString()));
                }
                else if (isRightParenthesis(ch))
                {
                    emptyLetterBufferAsVariables();
                    emptyNumberBufferAsLiteral();
                    result.Add(new Token("Right Parenthesis", ch.ToString()));
                }
                else if (isComma(ch))
                {
                    emptyNumberBufferAsLiteral();
                    emptyLetterBufferAsVariables();
                    result.Add(new Token("Function Argument Separator", ch.ToString()));
                }
            }

            if (numberBuffer.Count > 0)
            {
                emptyNumberBufferAsLiteral();
            }
            if (letterBuffer.Count > 0)
            {
                emptyLetterBufferAsVariables();
            }

            result[result.Count - 1].SymbolEnd = true;

            return result;

        }

        public List<Token> tokenize(string str)
        {
            result.Clear();
            letterBuffer.Clear();
            numberBuffer.Clear();

            foreach (char ch in str.ToCharArray())
            {
                if (isDigit(ch))
                {
                    numberBuffer.Add(ch.ToString());
                }
                else if (ch == '.')
                {
                    numberBuffer.Add(ch.ToString());
                }
                else if (isLetter(ch))
                {
                    if (numberBuffer.Count > 0)
                    {
                        emptyNumberBufferAsLiteral();
                        result.Add(new Token("Operator", "*"));
                    }
                    letterBuffer.Add(ch.ToString());
                }
                else if (isOperator(ch))
                {
                    emptyNumberBufferAsLiteral();
                    emptyLetterBufferAsVariables();
                    result.Add(new Token("Operator", ch.ToString()));
                }
                else if (isLeftParenthesis(ch))
                {
                    if (letterBuffer.Count > 0)
                    {
                        result.Add(new Token("Function", string.Join("", letterBuffer.ToArray())));
                        letterBuffer.Clear();
                    }
                    else if (numberBuffer.Count > 0)
                    {
                        emptyNumberBufferAsLiteral();
                        result.Add(new Token("Operator", "*"));
                    }
                    result.Add(new Token("Left Parenthesis", ch.ToString()));
                }
                else if (isRightParenthesis(ch))
                {
                    emptyLetterBufferAsVariables();
                    emptyNumberBufferAsLiteral();
                    result.Add(new Token("Right Parenthesis", ch.ToString()));
                }
                else if (isComma(ch))
                {
                    emptyNumberBufferAsLiteral();
                    emptyLetterBufferAsVariables();
                    result.Add(new Token("Function Argument Separator", ch.ToString()));
                }
            }

            if (numberBuffer.Count > 0)
            {
                emptyNumberBufferAsLiteral();
            }
            if (letterBuffer.Count > 0)
            {
                emptyLetterBufferAsVariables();
            }
            return result;

        }
    }

    public class TokenParser
    {
        private List<Token> lstTokens = null;
        public string FactoryString { get; }
        public TokenParser(List<Token> lt)
        {
            lstTokens = lt;
            StringBuilder sb = new StringBuilder();
            foreach (Token t in lstTokens)
            {
                if (t.Type == "Operator")
                {
                    sb.Append(t.Type);
                    sb.Append(t.Value);

                }
                else
                {
                    sb.Append(t.Type);
                }
            }

            FactoryString = sb.ToString();

        }

        public string Parse()
        {
            string ret = string.Empty;

            switch (FactoryString)
            {
                case "VariableOperator*Variable": //a*b
                    if (lstTokens[0].Value[0] > lstTokens[2].Value[0])
                    {
                        ret = lstTokens[2].Value + lstTokens[0].Value;

                    }
                    else
                    {
                        ret = lstTokens[0].Value + lstTokens[2].Value;

                    }
                    break;
            }
            return ret;
        }
    }
    public class RPN
    {
        private List<Token> Tokens = new List<Token>();

        public RPN(List<Token> t)
        {
            Tokens = t;
        }


    }
}
