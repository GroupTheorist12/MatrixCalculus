using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;
using System.Runtime.InteropServices;

namespace MatrixCalculus
{
#if !SILVERLIGHT
    [Serializable, StructLayout(LayoutKind.Sequential)]
#endif
    public struct Rational : IComparable, IEquatable<Rational>, IComparable<Rational>

    {
        private readonly BigInteger mNumerator;
        private readonly BigInteger mDenominator;

        /// <summary>
        /// Returns the Numerator for the Rational, can be positive or negative.
        /// </summary>
        public BigInteger Numerator
        {
            get { return mNumerator; }
        }

        /// <summary>
        /// Returns the Denominator for the Rational, always non-negative
        /// </summary>
        public BigInteger Denominator
        {
            get { return mDenominator; }
        }


        /// <summary>
        /// Returns the multiplicative inverse of the Rational, the Inverse of 0 is 0
        /// </summary>
        public Rational Inverse
        {
            get
            {
                if (Numerator == 0)
                {
                    throw new DivideByZeroException();
                }
                return new Rational(Denominator, Numerator, true);
            }
        }

        /// <summary>
        /// Returns the integer that would be assigned as the exponent if put in scientific notation.
        /// </summary>
        public int Magnitude
        {
            get
            {
                Rational numeratorSig = (Rational)Numerator / BigInteger.Pow(10, Numerator.Magnitude());
                Rational denominatorSig = (Rational)Denominator / BigInteger.Pow(10, Denominator.Magnitude());
                Rational significand = numeratorSig / denominatorSig;//between 9.9999 and .100000000
                if (Abs(significand) < 1)
                {
                    return Numerator.Magnitude() - Denominator.Magnitude() - 1;
                }
                return Numerator.Magnitude() - Denominator.Magnitude() + significand.BigIntegerPart.Magnitude();
            }
        }

        #region Constructors 

        private Rational(BigInteger numerator, BigInteger denominator, bool safe)
        {
            if (numerator == 0)
            {
                mNumerator = 0;
                mDenominator = 0;
                return;
            }
            if (denominator == 0)
            {
                throw new DivideByZeroException();
            }

            int sign = ((numerator > 0 && denominator > 0) || (numerator < 0 && denominator < 0)) ? 1 : -1;

            if (safe)
            {
                mNumerator = sign * BigInteger.Abs(numerator);
                mDenominator = BigInteger.Abs(denominator);
            }
            else
            {
                BigInteger gcd = BigInteger.GreatestCommonDivisor(numerator, denominator);//Potentially expensive
                mNumerator = sign * BigInteger.Abs(numerator / gcd);
                mDenominator = BigInteger.Abs(denominator / gcd);
            }
        }

        public Rational(int numerator) : this(numerator, 1, true) { }

        public Rational(BigInteger numerator) : this(numerator, 1, true) { }

        public Rational(BigInteger numerator, BigInteger denominator) : this(numerator, denominator, false) { }



        public Rational(double value)
        {
            this = Parse(value.ToString("R"));
        }

        public Rational(Decimal value)
        {
            this = Parse(value.ToString());
        }

        #endregion


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Rational))
            {
                return false;
            }
            return Equals((Rational)obj);
        }

        /// <summary>
        /// Returns the hashcode for the rational
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (mNumerator.GetHashCode() * 397) ^ mDenominator.GetHashCode();
            }
        }


        /// <summary>
        /// Returns the string representation of the Rational in Fractional form
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (mNumerator == 0)
            {
                return "0";
            }
            if (mDenominator == 1)
            {
                return mNumerator.ToString();
            }
            return mNumerator + "/" + mDenominator;
        }

        public string ToLatex()
        {
            if (mNumerator == 0)
            {
                return "0";
            }
            if (mDenominator == 1)
            {
                return mNumerator.ToString();
            }
            return @"\dfrac{" + mNumerator + "}{" + mDenominator + "}";

        }

        /// <summary>
        /// Returns the decimal approximation of the string in standard form.
        /// </summary>
        /// <param name="approximation">Are the places decimal places or significant figures</param>
        /// <param name="places">Number of places</param>
        /// <param name="padWithZeroes">Is the string approximation paddded with zeros</param>
        /// <returns>String approximation</returns>
        public string ToString(EApproxmationType approximation, int places, bool padWithZeroes)
        {
            StringBuilder sbRetVal = new StringBuilder();
            if (Numerator < 0)
            {
                sbRetVal.Append("-");
            }
            int pointIndex = BigIntegerPart.Magnitude() + 1;
            if (BigIntegerPart == 0)
            {
                sbRetVal.Append("0.");
                pointIndex = -1;
            }
            Rational working = this;
            while (working.BigIntegerPart == 0)
            {
                working *= 10;
                if (working.BigIntegerPart == 0)
                {
                    sbRetVal.Append("0");
                }
            }

            bool sf = approximation == EApproxmationType.SignificantFigures;
            int digitsNeeded = (sf) ? Math.Max(BigIntegerPart.Places(), places) : BigIntegerPart.Places() + places;

            int digitsToExtract = (sf) ? places : places + BigIntegerPart.Places();
            List<string> digits = Digits(working, digitsToExtract);
            if (digits.Count < digitsToExtract && !padWithZeroes)
            {
                digitsNeeded -= (digitsToExtract - digits.Count);
            }

            for (int digitCounter = 0; digitCounter < digits.Count; digitCounter++)
            {
                if (digitCounter == pointIndex)
                {
                    sbRetVal.Append(".");
                }
                sbRetVal.Append(digits[digitCounter]);
            }
            for (int digitCounter = digits.Count(); digitCounter < digitsNeeded; digitCounter++)
            {
                if (digitCounter == pointIndex)
                {
                    sbRetVal.Append(".");
                }
                sbRetVal.Append("0");
            }
            return sbRetVal.ToString();
        }


        internal static List<string> Digits(Rational r, int n)
        {
            List<string> digits = new List<string>();

            //Divide into integral and fractional parts
            BigInteger intPart = Abs(r).BigIntegerPart;
            Rational fracPart = Abs(r).FractionalPart;
            int intplaces = intPart.Magnitude() + 1;

            char[] chars = intPart.ToString().ToCharArray();
            for (int counter = 0; counter < Math.Min(intplaces, n); counter++) // Get digits from integral part
            {
                if (digits.Count > 0 || chars[counter] != '0') // First digit not zero
                {
                    digits.Add(chars[counter].ToString());
                }
            }
            while (digits.Count() < n) // Get digits from fractional part
            {
                if (fracPart == 0)
                {
                    break;
                }
                fracPart *= 10;
                if (digits.Count > 0 || fracPart.IntegerPart != 0) // First digit not zero
                {
                    digits.Add(fracPart.IntegerPart.ToString());
                }
                fracPart = fracPart.FractionalPart;
            }
            return digits;
        }


        /// <summary>
        /// Returns the decimal approximation of the string in Scientific Notation
        /// </summary>
        /// <param name="places">Number of places</param>
        /// <param name="padwithZeroes">Is the string approximation paddded with zeros</param>
        /// <returns>String approximation</returns>
        public string ToScientific(int places, bool padwithZeroes)
        {

            StringBuilder sb = new StringBuilder();
            if (Numerator < 0)
            {
                sb.Append("-");
            }
            List<string> digits = Digits(this, places);

            sb.Append(digits[0]);
            if (digits.Count > 1 || (places > 1 && padwithZeroes))
            {
                sb.Append(".");
            }
            for (int i = 1; i < digits.Count; i++)
            {
                sb.Append(digits[i]);
            }

            if (padwithZeroes)
            {
                for (int i = digits.Count; i < places; i++)
                {
                    sb.Append("0");
                }
            }
            sb.Append("E");

            if (BigIntegerPart != 0)
            {
                sb.Append("+");
                sb.Append(Magnitude);
            }
            else
            {
                sb.Append(Magnitude);
            }
            return sb.ToString();
        }


        #region Conversions To Rational

        static public implicit operator Rational(int value)
        {
            return new Rational(value);
        }

        static public implicit operator Rational(BigInteger value)
        {
            return new Rational(value);
        }

        static public implicit operator Rational(double value)
        {
            return new Rational(value);
        }

        static public implicit operator Rational(decimal value)
        {
            return new Rational(value);
        }

        #endregion

        #region Conversions from Rational
        static public explicit operator sbyte(Rational value)
        {
            return (sbyte)value.BigIntegerPart;
        }

        static public explicit operator byte(Rational value)
        {
            return (byte)value.BigIntegerPart;
        }

        static public explicit operator short(Rational value)
        {
            return (short)value.BigIntegerPart;
        }

        static public explicit operator ushort(Rational value)
        {
            return (ushort)value.BigIntegerPart;
        }

        static public explicit operator int(Rational value)
        {
            return (int)value.BigIntegerPart;
        }

        static public explicit operator uint(Rational value)
        {
            return (uint)value.BigIntegerPart;
        }

        static public explicit operator long(Rational value)
        {
            return (long)value.BigIntegerPart;
        }

        static public explicit operator ulong(Rational value)
        {
            return (ulong)value.BigIntegerPart;
        }

        static public explicit operator BigInteger(Rational value)
        {
            return value.BigIntegerPart;
        }

        static public explicit operator Single(Rational value)
        {

            return Single.Parse(value.ToScientific(8, false));
        }

        static public explicit operator double(Rational value)
        {
            return Double.Parse(value.ToScientific(17, false));
        }

        static public explicit operator decimal(Rational value)
        {
            return decimal.Parse(value.ToString(EApproxmationType.DecimalPlaces, 29, false));
        }

        #endregion

        #region Operators
        public static bool operator ==(Rational r1, Rational r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(Rational r1, Rational r2)
        {
            return !r1.Equals(r2);
        }

        public static Rational operator +(Rational r1, Rational r2)
        {
            if (r1 == 0) { return r2; }
            if (r2 == 0) { return r1; }
            if (r1.Denominator == r2.Denominator)
            {
                return new Rational(r1.Numerator + r2.Numerator, r1.Denominator, false);
            }
            return new Rational(r1.Numerator * r2.Denominator + r2.Numerator * r1.Denominator, r1.Denominator * r2.Denominator, false);
        }

        public static Rational operator +(Rational r1, int i)
        {
            if (r1 == 0) { return i; }
            if (i == 0) { return r1; }
            return new Rational(r1.Numerator + i * r1.Denominator, r1.Denominator, true);
        }

        public static Rational operator +(int i, Rational r1)
        {
            if (r1 == 0) { return i; }
            if (i == 0) { return r1; }
            return new Rational(r1.Numerator + i * r1.Denominator, r1.Denominator, true);
        }

        public static Rational operator -(Rational r1, Rational r2)
        {
            if (r1 == 0)
            {
                return -r2;
            }

            if (r2 == 0)
            {
                return r1;
            }
            if (r1.Denominator == r2.Denominator)
            {
                return new Rational(r1.Numerator - r2.Numerator, r1.Denominator, false);
            }
            return new Rational(r1.Numerator * r2.Denominator - r2.Numerator * r1.Denominator, r1.Denominator * r2.Denominator, false);
        }

        public static Rational operator -(Rational r)
        {
            return new Rational(-1 * r.Numerator, r.Denominator, true);
        }

        public static Rational operator *(Rational r1, Rational r2)
        {
            if (r1 == 0 || r2 == 0)
            {
                return 0;
            }
            return new Rational(r1.Numerator * r2.Numerator, r1.Denominator * r2.Denominator, false);
        }

        public static Rational operator *(Rational r, int i)
        {
            if (r == 0 || i == 0)
            {
                return 0;
            }
            return new Rational(r.Numerator * i, r.Denominator, false);
        }

        public static Rational operator *(int i, Rational r)
        {
            if (r == 0 || i == 0)
            {
                return 0;
            }
            return new Rational(r.Numerator * i, r.Denominator, false);
        }

        public static Rational operator *(Rational r, long i)
        {
            if (r == 0 || i == 0)
            {
                return 0;
            }
            return new Rational(r.Numerator * i, r.Denominator, false);
        }

        public static Rational operator *(long i, Rational r)
        {
            if (r == 0 || i == 0)
            {
                return 0;
            }
            return new Rational(r.Numerator * i, r.Denominator, false);
        }

        public static Rational operator /(Rational r1, Rational r2)
        {
            if (r1 == 0)
            {
                return 0;
            }
            if (r2 == 0)
            {
                throw new DivideByZeroException();
            }
            return new Rational(r1.Numerator * r2.Denominator, r1.Denominator * r2.Numerator, false);
        }

        public static Rational operator %(Rational r1, Rational r2)
        {
            return r1 - (r1 / r2).BigIntegerPart * r2;
        }


        public static Rational operator /(Rational r, int i)
        {
            if (r == 0)
            {
                return 0;
            }
            if (i == 0)
            {
                throw new DivideByZeroException();
            }
            return new Rational(r.Numerator, r.Denominator * i, false);
        }

        public static Rational operator /(int i, Rational r)
        {
            if (i == 0)
            {
                return 0;
            }
            if (r == 0)
            {
                throw new DivideByZeroException();
            }
            return new Rational(r.Denominator * i, r.Numerator, false);
        }

        public static bool operator <(Rational r1, Rational r2)
        {
            return r1.CompareTo(r2) < 0;
        }

        public static bool operator <=(Rational r1, Rational r2)
        {
            return r1.CompareTo(r2) <= 0;
        }

        public static bool operator >(Rational r1, Rational r2)
        {
            return r1.CompareTo(r2) > 0;
        }

        public static bool operator >=(Rational r1, Rational r2)
        {
            return r1.CompareTo(r2) >= 0;
        }
        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            if (!(obj is Rational))
            {
                throw new ArgumentException();
            }
            return CompareTo((Rational)obj);
        }

        #endregion

        /// <summary>
        /// Returns the Integer part of the rational as an Int32
        /// </summary>
        public int IntegerPart
        {
            get
            {
                if (Numerator == 0)
                {
                    return 0;
                }
                return (int)(Numerator / Denominator);
            }
        }

        /// <summary>
        /// Returns the Integer part of the Rational expressed as a BigInteger
        /// </summary>
        public BigInteger BigIntegerPart
        {
            get
            {
                if (Numerator == 0)
                {
                    return 0;
                }
                return Numerator / Denominator;
            }
        }

        /// <summary>
        /// returns the Fractional part of a rational
        /// </summary>
        public Rational FractionalPart
        {
            get
            {
                return this - BigIntegerPart;
            }
        }



        #region System.Math Members

        public static Rational Abs(Rational value)
        {
            return new Rational(BigInteger.Abs(value.Numerator), BigInteger.Abs(value.Denominator), true);
        }

        public static Rational Ceiling(Rational value)
        {
            BigInteger bi = value.BigIntegerPart;
            if (value == bi)
            {
                return value;
            }
            if (value >= 0)
            {
                return (Rational)bi + 1;
            }
            return bi;
        }

        public static Rational Floor(Rational value)
        {
            BigInteger bi = value.BigIntegerPart;
            if (value == bi)
            {
                return value;
            }
            if (value >= 0)
            {
                return bi;
            }
            return (Rational)bi - 1;
        }

        public static Rational Max(Rational val1, Rational val2)
        {
            return val2 >= val1 ? val2 : val1;
        }

        public static Rational Min(Rational val1, Rational val2)
        {
            return val1 <= val2 ? val1 : val2;
        }

        public static Rational Pow(Rational value, int power)
        {
            if (power < 0)
            {
                return new Rational(BigInteger.Pow(value.Denominator, -power), BigInteger.Pow(value.Numerator, -power), true);
            }
            if (power == 0)
            {
                return (new Rational(1));
            }
            return new Rational(BigInteger.Pow(value.Numerator, power), BigInteger.Pow(value.Denominator, power), true);
        }

        public static Rational Round(Rational value)
        {
            BigInteger bi = value.BigIntegerPart;
            if (value == bi)
            {
                return value;
            }
            if (value < 0)
            {
                return bi;
            }
            if (value.FractionalPart <= (Rational)1 / 2)
            {
                return bi;
            }
            return (Rational)bi + 1;
        }

        public static int Sign(Rational value)
        {
            return value.Numerator.Sign;
        }


        #endregion


        #region Extended Math Members

        public static Rational Floor(Rational value, int denominator)
        {
            return Floor(value * denominator) / denominator;
        }

        public static Rational Floor(Rational value, BigInteger denominator)
        {
            return Floor(value * denominator) / denominator;
        }


        public static Rational Floor(Rational value, Rational denominator)
        {
            return Floor(value / denominator) * denominator;
        }


        public static Rational Ceiling(Rational value, int denominator)
        {
            return Ceiling(value * denominator) / denominator;
        }

        public static Rational Ceiling(Rational value, BigInteger denominator)
        {
            return Ceiling(value * denominator) / denominator;
        }


        public static Rational Ceiling(Rational value, Rational denominator)
        {
            return Ceiling(value / denominator) * denominator;
        }

        public static Rational Round(Rational value, int denominator)
        {
            return Round(value * denominator) / denominator;
        }

        public static Rational Round(Rational value, BigInteger denominator)
        {
            return Round(value * denominator) / denominator;
        }

        public static Rational Round(Rational value, Rational denominator)
        {
            return Round(value / denominator) * denominator;
        }

        #endregion


        public static Rational Parse(String s)
        {
            int periodIndex = s.IndexOf(".");
            int eIndeix = s.IndexOf("E");
            int slashIndex = s.IndexOf("/");
            if (periodIndex == -1 && eIndeix == -1 && slashIndex == -1) // An integer such as 7
            {
                return new Rational(ParseBigInteger(s));
            }

            if (periodIndex == -1 && eIndeix == -1 && slashIndex != -1) // A fraction such as 3/7
            {

                return new Rational(ParseBigInteger(s.Substring(0, slashIndex)),
                                    ParseBigInteger(s.Substring(slashIndex + 1)));
            }

            if (eIndeix == -1) // No scientific notation such as 5.997
            {
                BigInteger n = ParseBigInteger(s.Replace(".", ""));
                BigInteger d = (BigInteger)Math.Pow(10, s.Length - periodIndex - 1);
                return new Rational(n, d);
            }

            // In scientific notation such as 2.4556E-2
            int characteristic = int.Parse(s.Substring(eIndeix + 1));
            BigInteger ten = 10;
            BigInteger numerator = ParseBigInteger(s.Substring(0, eIndeix).Replace(".", ""));
            BigInteger denominator = new BigInteger(Math.Pow(10, eIndeix - periodIndex - 1));
            BigInteger charPower = BigInteger.Pow(ten, Math.Abs(characteristic));
            if (characteristic > 0)
            {
                numerator = numerator * charPower;
            }
            else
            {
                denominator = denominator * charPower;
            }
            return new Rational(numerator, denominator);
        }

        internal static BigInteger ParseBigInteger(string s)
        {
#if !SILVERLIGHT
            return BigInteger.Parse(s);
#else // for unknown reasons the Silverlight version of Biginteger lacks a Parse Method
                    bool isNegative = s.StartsWith("-");
                    if (isNegative) s = s.Substring(1);

                    int periodIndex = s.IndexOf(".");
                    int eIndeix = s.IndexOf("E");
                    BigInteger ten = 10;
                    BigInteger bi = 0;
                    if (periodIndex == -1 && eIndeix == -1)// an integer such as 7
                    {

                        int parts = s.Length / 18;
                        for (int i = 0; i < parts; i++)
                        {
                            long l = long.Parse(s.Substring(i * 18, 18));
                            bi += l * BigInteger.Pow(ten, s.Length - (i + 1) * 18);
                        }
                        bi += long.Parse(s.Substring(parts * 18));
                    }
                    return (isNegative) ? -1 * bi : bi;
#endif
        }

        public double ToDouble()
        {
            const int significantFigures = 17;
            string doublestring = ToScientific(significantFigures, false);
            return Double.Parse(doublestring);
        }


        #region IEquatable<Rational> Members

        public bool Equals(Rational other)
        {
            if (Numerator == 0)
            {
                return other.Numerator == 0;
            }
            return Numerator == other.Numerator && Denominator == other.Denominator;
        }

        #endregion


        #region IComparable<Rational> Members

        public int CompareTo(Rational other)
        {
            if (this == other)
            {
                return 0;
            }
            if (Sign(this) < Sign(other))
            {
                return -1;
            }
            if (Sign(this) > Sign(other))
            {
                return 1;
            }

            if (Sign(this) > 0 && Sign(other) > 0)
            {
                if (Numerator >= other.Numerator && Denominator <= other.Denominator)
                {
                    return 1;
                }
                if (Numerator <= other.Numerator && Denominator >= other.Denominator)
                {
                    return -1;
                }

                return Sign(Numerator * other.Denominator - other.Numerator * Denominator);
            }
            else
            {
                if (BigInteger.Abs(Numerator) <= BigInteger.Abs(other.Numerator) && Denominator >= other.Denominator)
                {
                    return 1;
                }
                if (BigInteger.Abs(Numerator) <= BigInteger.Abs(other.Numerator) && Denominator <= other.Denominator)
                {
                    return -1;
                }
                return -Sign(BigInteger.Abs(Numerator) * other.Denominator - BigInteger.Abs(other.Numerator) * Denominator);
            }
        }
        #endregion
    }

    public enum EApproxmationType
    {
        DecimalPlaces,
        SignificantFigures
    }
}
