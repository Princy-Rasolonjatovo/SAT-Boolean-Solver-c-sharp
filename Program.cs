/**
 * SAT solver using Bruteforce
 * 
 * @author : Princy Rasolonjatovo
 * @email  : princy.m.rasolonjatovo@gmail.com
 * @github : princy-rasolonjatovo
 **/


using System;

namespace SATBruteforce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            SatSolver solver = new SatSolver();
            // Test 1
            List<string> rules = new List<string>();
            rules.Add("(p IMPLY p)");
            rules.Add("(q IMPLY p)");
            rules.Add("(q IMPLY r)");
            rules.Add("(r IMPLY q)");
            solver = new SatSolver();
            solver.Solve(" r IMPLY p", rules);
            Console.WriteLine();

            // Test 2
            string s2 = "(p) AND (p IMPLY q) AND (q IMPLY r)";
            solver.Solve(s2);
            Console.WriteLine();

            // Test 3
            solver.Solve("a AND NOT b");
            Console.WriteLine();


            Console.ReadKey();
        }
        

    }
}
