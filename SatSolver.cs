/**
 * SAT solver using Bruteforce
 * 
 * @author : Princy Rasolonjatovo
 * @email  : princy.m.rasolonjatovo@gmail.com
 * @github : princy-rasolonjatovo
 * 
 * 
 * Solve a boolean expression using bruteforce
 * # Operators
 * - EQUA     ( == )
 * - IMPLY    ( => )
 * - OR       ( || )
 * - AND      ( && )
 * - NOT      ( ~  )
 * # example : 
 * - expression : '(p) AND (p IMPLY q) AND (q IMPLY r)'
 * - SatSolver solver = new SatSolver();
 * - solver.Solve(expression);
 * - OUTPUT >>> p(True) q(True)  r(True)
 **/



using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATBruteforce
{
    
     class SatSolver
    {
        private List<Operator> operators;
        public SatSolver()
        {
            // Feed the  Operators
            this.operators = new List<Operator>();
            this.operators.Add(new Operator(
                "or", "OR", (p, q) => p || q, 2, false)
            );
            this.operators.Add(new Operator(
                "and", "AND", (p, q) => p && q, 2, false)
            );
            this.operators.Add(new Operator(
                "not", "NOT", (p, q) => !p, 1, true)
            );
            this.operators.Add(new Operator(
                "imply", "IMPLY", (p, q) => (!p) || q, 3, false)
            );
            this.operators.Add(new Operator(
                "equal", "EQU", (p, q) => p == q, 3, false)
            );
        }

        public void Solve(string expression, List<string>? rules=null)
        {
            string fullExpression;
            if (rules != null && rules.Count > 0)
            {
                rules.ForEach(rule => rule = $"({rule})");
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Join(" AND ", rules));
                sb.Append(" AND ");
                sb.Append($"({expression})");
                fullExpression = sb.ToString();
            }
            else { fullExpression = expression; }

            Parser parser = new Parser(fullExpression, ref this.operators);
            List<Dictionary<string, bool>> solutions = parser.Bruteforce();

            
            Console.WriteLine(" The statement : ");
            Console.WriteLine($" >>> {fullExpression}");
            if (rules != null)
            {
                Console.WriteLine(" With the following rules : ");
                foreach(var rule in rules)
                {
                    Console.Write($"\t. {rule} \n");
                }
            }
            if (solutions.Count > 0)
            {
                Console.WriteLine(" Is satisfaisable with the following conditions : ");
                foreach (Dictionary<string, bool> solution in solutions)
                {
                    Console.Write("\t");
                    foreach (KeyValuePair<string, bool> variablePair in solution)
                    {
                        Console.Write($"{variablePair.Key}({variablePair.Value}) ");
                    }
                    Console.Write("\n");
                }
            }
            else
            {
                Console.WriteLine(" Is not satisfaisable");
            }
        }
    }
}
