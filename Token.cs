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
    public class Token: IEquatable<Token>, IExpressionType, ICloneable<Token>
    {
        public string Name { get; init; }
        public bool Value { get; set; }
        public Token(string name, bool value)
        {
            this.Name = name;
            this.Value = value;
        }
        public override string ToString()
        {
            return $"Token<{this.Name}>";
        }

        //! WARNING: experimental
        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public bool Equals(Token? other)
        {
            if (other == null) return false;
            return this.GetHashCode() == other?.GetHashCode();
        }

        public ExpressionType RevealType()
        {
            return ExpressionType.TOKEN;
        }

        public string GetSymbol()
        {
            return this.Name;
        }

        public Token Clone()
        {
            return new Token(this.Name, this.Value);
        }
    }
}
