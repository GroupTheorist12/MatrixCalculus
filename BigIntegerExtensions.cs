using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace MatrixCalculus
{
  public static  class BigIntegerExtensions
    {
      /// <summary>
        /// Returns the integer that would be assined as the exponent if put in scientific notation.
      /// </summary>
      /// <param name="value">The BigInteger to be tested</param>
      /// <returns></returns>
      public static int Magnitude(this BigInteger value)
      {
               return (value !=0)?(int)Math.Floor(BigInteger.Log10(BigInteger.Abs(value))):0;
      }

      internal static int Places(this BigInteger value)
      {
          if (value == 0)
          {
              return 0;
          }
          else
          {
              return BigInteger.Abs(value).ToString().Length;
          }
      }

      /// <summary>
      /// The exponent of the factor under prime decomposition
      /// </summary>
      /// <param name="value">The Biginteger to be factored</param>
      /// <param name="factor">the number to be factored, does not need to be a prime</param>
      /// <returns></returns>
      public static int FactorExponent(this BigInteger value, int factor)
      {
          if (factor == 1)
          {
              throw new Exception("1 is not an acceptable value");
          }
          int count = 0;
          BigInteger remaining = BigInteger.Abs(value);
          while (true)
          {
              if ( remaining % factor ==0)
                  {
                      remaining /= factor;
                      count++;
                  }
                  else
                  {
                      break;

                  }

          }  ;

          return count;

      }
    }
}
