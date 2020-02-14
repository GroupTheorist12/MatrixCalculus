using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MatrixCalculus
{
 public static  class RationalCollectionHelpers
    {
     /// <summary>
     /// Return a list of the multiples of a rational number starting at the lowerbound and up to the upperbound
     /// </summary>
     /// <param name="q">The base rational number</param>
     /// <param name="lowerbound">The lowerbound (included as first item)</param>
     /// <param name="upperBound">The upperbound (might not be included)</param>
     /// <returns>The list of rationals</returns>
     public static  List<Rational> Range(Rational q, Rational lowerbound, Rational upperBound)
     {
         Rational lower = Rational.Ceiling(lowerbound, q);
         Rational current =lower;
         Rational upper = Rational.Ceiling(upperBound, q);
         List<Rational > rationals = new List<Rational>();
         do
         {
             rationals.Add(current);
             current += q;

         } while (current < upperBound);
         rationals.Add(upperBound);

         return rationals;
     }

     /// <summary>
     /// Returns a range of rationals between 0 and 1
     /// </summary>
     /// <param name="q">Base rational number</param>
     /// <returns>List of rationals</returns>
     public static  List<Rational> Range(Rational q)
     {
         return Range(q, (Rational)0, (Rational)1);
     }


     /// <summary>
     /// Compares two lists of rational numbers
     /// </summary>
     /// <param name="list1">first list</param>
     /// <param name="list2">second lis</param>
     /// <returns>Are they identical</returns>
     public static bool Compare(List<Rational> list1, List<Rational> list2)
     {
         if (list1.Count != list2.Count)
         {
             return false;

         }

         for (int i = 0; i < list1.Count; i++)
         {
             if (list1[i] != list2[i])
             {

                 return false;
             }

         }
         return true; 


     }

     /// <summary>
     /// Returns the floor for the rational within the collection of rationals (can be null)
     /// </summary>
     /// <param name="value">the value to be floored</param>
     /// <param name="rationals">This list of rationals to be floored to</param>
     /// <returns>the floored value</returns>
     public static  Rational?  Floor(Rational value, IEnumerable<Rational> rationals)
     {
         try{
             return value - (from r in rationals
                             where value >= r
                             select value - r).Min();
         }catch{

            return null;
         }
     }

     /// <summary>
     /// Returns the ceiling for the rational within the collection of rationals (can be null)
     /// </summary>
     /// <param name="value">the value to be ceilinged</param>
     /// <param name="rationals">This list of rationals to be ceilinged to</param>
     /// <returns>the ceilinged value</returns>
     public static Rational?  Ceiling(Rational value, IEnumerable<Rational> rationals)
     {
           try{
                 return   value + (from r in rationals
                                 where r >= value
                                 select r-value).Min();
           }
           catch
           {
               return null;
           }
     }

     /// <summary>
     /// Rounds the rational to the nearest value in the collection of rationals
     /// </summary>
     /// <param name="value">the value to be rounded</param>
     /// <param name="rationals">This list of rationals to be rounded to</param>
     /// <returns>The rounded value</returns>
     public static Rational Round(Rational value, IEnumerable<Rational> rationals)
     {
        
         Rational? nfloor = Floor(value, rationals);
         Rational? nceiling = Ceiling(value, rationals);

         if (nfloor == null) { return (Rational)nceiling; }
         if (nceiling == null) { return (Rational)nfloor; }

         Rational floor = (Rational)nfloor;
         Rational ceiling = (Rational)nceiling;

         if (value - floor <= ceiling - value)
         {

             return (Rational)floor;
         }
         else
         {
             return (Rational)ceiling;
         }
     }
       
    

     }
   
}
