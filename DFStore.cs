using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using LiteDB;
using System.IO;

namespace MatrixCalculus
{
    public class DFTestCase
    {
        public int DFTestCaseId { get; set; }
        public string Expression { get; set; }
        public string HashedToken { get; set; }
    }
    public class DFStore
    {
        public static string[] DF1TestCaseList =
        {
            "2",
            "x",
            "2x",
            "x^2",
            "2x^2",
            "1/x",
            "1/2x",
            "1/x^2",
            "1/2x^2",
            "sin(x)",
            "sin(2x)",
            "sin(x)^2",
            "sin(2x)^2",
            "(x + 5)^3",
            "exp(x)",
            "exp(2x)",
            "exp(x^2)"


        };

        //
        public static void CreateTestCases()
        {
            string path = Directory.GetCurrentDirectory() + "/DB/";

            using (var db = new LiteDatabase(Path.Combine(path, "DFExp.db")))
            {
                 var col = db.GetCollection<DFTestCase>("dftestcases");
                foreach(string exp in DF1TestCaseList)
                {

                   Symbol sym = new Symbol(exp);

                    
                    var testCase = new DFTestCase
                    {
                        Expression = exp,
                        HashedToken = sym.HashTokenString

                    };

                    col.Insert(testCase);
                }

                col.EnsureIndex(d => d.HashedToken);

                var result = col.FindAll();

                foreach(DFTestCase df in result.ToList())
                {
                    Console.WriteLine("{0} => {1}", df.Expression, df.HashedToken);
                }
            }

            
        }
    }
}
