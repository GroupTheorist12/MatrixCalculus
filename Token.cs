using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MathematicalAlgrorithms
{


  public class Token
  {
    public string Type { get; set; }
    public string Value { get; set;}

    public Token(string t, string v)
    {
      this.Type = t;
      this.Value = v;
    }
  }

  public class Tokenizer
  {
    private List<Token> result = new List<Token>();
    private List<string> letterBuffer = new List<string>();
    private List<string> numberBuffer = new List<string>();

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

    private void emptyLetterBufferAsVariables()
    {
      var l = letterBuffer.Count;
      for (var i = 0; i < l; i++)
      {
        result.Add(new Token("Variable", letterBuffer[i]));
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
        if(t.Type == "Operator")
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

      switch(FactoryString)
      {
        case "VariableOperator*Variable": //a*b
          if(lstTokens[0].Value[0] > lstTokens[2].Value[0])
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
