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
    public enum ExpressionType { TOKEN, OPERATOR}
    public  interface IExpressionType
    {
        ExpressionType RevealType();
        string GetSymbol();
    }
}
