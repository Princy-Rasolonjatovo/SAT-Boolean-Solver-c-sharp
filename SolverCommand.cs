using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;



namespace SATBruteforce
{
    [Verb("solve", HelpText = "Solve the given statement")]
    public class SolverCommand 
    {
        static string HelpText =
@"

Solve a boolean expression using bruteforce
# Operators
- EQUA     ( == )
- IMPLY    ( => )
- OR       ( || )
- AND      ( && )
- NOT      ( ~  )

Examples:
- With conditions 
    SATSolver.exe -e (r IMPLY p) -c (p IMPLY p) (q IMPLY p) (q IMPLY r) (r IMPLY q)
- Without Conditions
    a) SATSolver.exe -e (p) AND (p IMPLY q) AND (q IMPLY r)  
    b) SATSolver.exe -e a AND NOT b
";
        [Option('e', "expression", Required = true, HelpText = "the logic expression with known verbs (NOT, IMPLY, EQU, AND, OR)")]
        public string Expression { get; set; } = "p IMPLY q";  // Default expression 
        [Option('c', "conditions", Required = false, HelpText = "List of additionnal constraints")]
        public IEnumerable<string> Conditions { get; set; } = null;
        public void Execute()
        {
            SatSolver solver = new SatSolver();
            List<string> conditions = new List<string>();
            StringBuilder sb = new StringBuilder();

            if (this.Conditions != null)
            {
                foreach (string condition in this.Conditions)
                {
                    sb.Append("(");
                    sb.Append(condition.Trim());
                    sb.Append(")");
                    
                    conditions.Add(sb.ToString());
                    
                    sb.Clear();

                    //conditions.Add(condition.Trim());
                }
            }

            solver.Solve(Expression, conditions.Count > 0 ? conditions : null);
        }

        public static void HandleParseError(IEnumerable<Error> errs)
        {
            
            //handle errors
            if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError))
            {
                Console.WriteLine(HelpText);
            }
                
        }
    }
}
