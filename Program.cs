/**
 * SAT solver using Bruteforce
 * 
 * @author : Princy Rasolonjatovo
 * @email  : princy.m.rasolonjatovo@gmail.com
 * @github : princy-rasolonjatovo
 **/


using System;
using CommandLine;


namespace SATBruteforce
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CommandLine.Parser.Default.ParseArguments<SolverCommand>(args)
                   .WithParsed<SolverCommand>(o =>
                   {
                       o.Execute();
                   }).WithNotParsed(SolverCommand.HandleParseError);

        }
        

    }
}
