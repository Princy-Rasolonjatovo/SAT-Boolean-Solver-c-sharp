/**
 * SAT solver using Bruteforce
 * 
 * @author : Princy Rasolonjatovo
 * @email  : princy.m.rasolonjatovo@gmail.com
 * @github : princy-rasolonjatovo
 **/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATBruteforce
{
    /// <summary>
    /// Unsure the Created Token is unique
    /// </summary>
    public static class TokenTracker
    {
        private static Dictionary<string, Token> Tokens = new Dictionary<string, Token>();

        
        public static Token CreateToken(string name, bool value=false)
        {
            Token t;
            string clean_name = name.Trim();
            if (!Tokens.ContainsKey(clean_name))
            {
                t = new Token(clean_name, value);
                Tokens.Add(clean_name, t);
            } else {
                t = Tokens[clean_name];
            }
            return t;
        }
        public static int GetInstancesCount() { return Tokens.Count; }
        public static List<Token> GetTokens() { return Tokens.Values.ToList(); }
        public static void RemoveAll() { Tokens.Clear(); }
    }
}
