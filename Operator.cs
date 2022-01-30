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
    public class Operator : IEquatable<Operator>, IExpressionType, ICloneable<Operator>
    {
        public string Name { get; init; }
        public string Symbol { get; init; }
        private Func<bool, bool, bool> Fn;
        public int Priority { get; init; }
        public bool IsUnary { get; init; }
        public Operator(
            string name, 
            string symbol, 
            Func<bool, bool, bool> fn,
            int priority,
            bool isUnary=false)
        {
            this.Name = name;
            this.Symbol = symbol;
            this.Priority = priority;
            this.IsUnary = isUnary;
            this.Fn = fn;
        }

        public bool Apply(bool left, bool right)
        {
            return this.Fn(left, right);
        }

        public override string ToString()
        {
            return $"[Operator] symbol: '{this.Symbol}'";
        }

        public bool Equals(Operator? other)
        {
            if (other == null) return false;
            return this.Name == other.Name;
        }

        public ExpressionType RevealType()
        {
            return ExpressionType.OPERATOR;
        }
        public string GetSymbol()
        {
            return this.Symbol;
        }

        public Operator Clone()
        {
            return new Operator
            (
                this.Name,
                this.Symbol,
                this.Fn,
                this.Priority,
                this.IsUnary
            );
            
        }
    }
}
