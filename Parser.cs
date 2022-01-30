/**
 * SAT solver using Bruteforce
 * Expression rule : PEMDAS  |  written in reverse polish notation
 * Parser
 *  -> Postfix // Shunting-yard algorithm
 *  -> EvalExpression // Reverse polish reader
 *  
 * @author : Princy Rasolonjatovo
 * @email  : princy.m.rasolonjatovo@gmail.com
 * @github : princy-rasolonjatovo
 **/



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SATBruteforce
{
    public class Parser
    {
        public string Input { get; init; }
        private List<Operator> Operators;
        public int TokenCount { get; set; }
        public List<IExpressionType> Postfixed { get; init; }
        public Parser(string s, ref List<Operator> operators)
        {
            // Clean Tokens
            TokenTracker.RemoveAll();

            this.Input = s;
            this.Operators = operators;
            this.Postfixed = this.Postfix(s);
        }

        private List<string> OperationToList(string s)
        {
            List<string> operatorsSymbols = new List<string>();
            this.Operators.ForEach(op => operatorsSymbols.Add(op.Symbol));

            
            string pattern = $@"(\(|\)|{String.Join('|', operatorsSymbols)})";
            string[] raw_splited = Regex.Split(s, pattern);

            List<string> splited = new List<string>();
            foreach (string raw in raw_splited)
            {
                string _raw = raw.Replace(" ", "").Trim();
                if (_raw.Length > 0)
                {
                    splited.Add(_raw);
                }
            }
            return splited;
        }
        
        private List<IExpressionType> Postfix(string s)
        {
            Stack<IExpressionType> stack = new Stack<IExpressionType>();
            Dictionary<string, Operator> operators = new Dictionary<string, Operator>();
            foreach (Operator op in this.Operators)
            {
                operators.Add(op.Symbol, op);
            }

            // Parenthesis markers
            Operator OPENPARENTHESIS = new Operator("(", "(", null, -1, false);
            Operator CLOSEPARENTHESIS = new Operator(")", ")", null, -1, false); ;
            List<IExpressionType> operations = new List<IExpressionType>();

            List<string> splited = this.OperationToList(s);
            string val;
            while (splited.Count > 0 || stack.Count > 0)
            {
                // Left to Right (LR)
                if (splited.Count > 0)
                {
                    // Get the current element from input(Token | Operator | parenthesis)
                    val = splited.First();
                    splited.RemoveAt(0);
                }
                else
                {
                    // No more tokens to process in input
                    // Add remaining operations(in the stack) to the main queue
                    while (stack.Count > 0)
                    {
                        operations.Add(stack.Pop());
                    }
                    continue;
                }
                // Check if current element is an operator
                if (operators.ContainsKey(val))
                {
                    Operator op = operators[val];
                    // is the stack (containing operator) empty?
                    if (stack.Count > 0)
                    {
                        // the stack is not empty
                        // get the top of the stack
                        IExpressionType top = stack.Pop();
                        // Is the topest element a openparenthesis ?
                        if (top.GetSymbol() != OPENPARENTHESIS.GetSymbol())
                        {
                            // The top element in operator_stack is not a parenthesis
                            // priority is DESC (rmin has higher priority)
                            if ((top as Operator)?.Priority <= op.Priority)
                            {
                                // add higher priority element into the queue
                                operations.Add(top);
                                stack.Push(op);
                                continue;
                            }
                            else
                            {
                                stack.Push(top);
                                stack.Push(op);
                                continue;
                            }
                        }
                        else
                        {
                            // Is parenthesis
                            stack.Push(top);
                            stack.Push(op);
                            continue;
                        }
                    }
                    else
                    {
                        // The stack is empty
                        stack.Push(op);
                        continue;
                    }
                }
                else if (val == OPENPARENTHESIS.GetSymbol())
                {
                    stack.Push(OPENPARENTHESIS);
                    continue;
                }
                else if (val == CLOSEPARENTHESIS.GetSymbol())
                {
                    // Remove all element from the stack until OPENPARENTHESIS
                    while (stack.Count > 0)
                    {
                        IExpressionType top = stack.Pop();
                        if (top.GetSymbol() == OPENPARENTHESIS.GetSymbol())
                        {
                            break;
                        }
                        else
                        {
                            operations.Add(top);
                        }
                    }
                    continue;
                }
                else 
                {
                    // The current element is a variable | constant
                    operations.Add(TokenTracker.CreateToken(val, false));
                }
            }

            this.TokenCount = TokenTracker.GetInstancesCount();

            return operations;
        }

        private bool EvalExpression(List<bool> values)
        {
            if(this.TokenCount != values.Count)
            {
                throw new Exception(
                    $"[NotEnoughValuesError] numbers of variables in expression: {this.TokenCount} number of variable on input: {values.Count}"
                );
            }

            int k = 0;
            TokenTracker.GetTokens().ForEach(t => t.Value = values.ElementAt(k++));
            
            Func<IExpressionType, int, IExpressionType> fn_cloner = (el, i) => el.RevealType() == ExpressionType.OPERATOR ? (el as Operator)?.Clone() : (el as Token)?.Clone();
            List<IExpressionType> expression = this.Postfixed.Select(fn_cloner).ToList();
            Stack<bool> stack = new Stack<bool>();

            while (expression.Count > 0)
            {
                IExpressionType val = expression.First();
                expression.RemoveAt(0);
                // Check if its a Token
                if (val.RevealType() == ExpressionType.TOKEN)
                {
                    stack.Push((val as Token).Value);
                }
                else 
                {
                    // Three address code
                    try
                    {
                        Operator op = (val as Operator);
                        if (op.IsUnary)
                        {
                            // Is The stack empty ?
                            if (stack.Count == 0)
                            {
                                Stack<IExpressionType> temp_stack = new Stack<IExpressionType>();
                                // '(' is just a marker
                                temp_stack.Push(new Operator("(", "(", null, -1, false));
                                IExpressionType _val = expression.First();
                                expression.RemoveAt(0);
                                // POP all unary operators till getting a Token
                                while (_val.RevealType() != ExpressionType.TOKEN)
                                {
                                    temp_stack.Push(_val);
                                    _val = expression.First();
                                    expression.RemoveAt(0);
                                }
                                IExpressionType _ = temp_stack.Pop();
                                bool _val_value = (_val as Token).Value;
                                while (_.GetSymbol() != "(")
                                {
                                    _val_value = (_ as Operator).Apply(_val_value, false);
                                    _ = temp_stack.Pop();
                                }
                                stack.Push(op.Apply(_val_value, false));
                            }
                            else
                            {
                                stack.Push(op.Apply(stack.Pop(), false));
                            }
                            continue;
                        }
                        // it must be an operator here if the poped elements(left=operator,right=operator) is an operator(like in ----1-----3)
                        // for example
                        // what are the operand of the current operator
                        bool right;
                        bool left;
                        right = stack.TryPop(out right) ? right : false;
                        left = stack.TryPop(out left) ? left : false;
                        stack.Push(op.Apply(left, right));
                        continue;

                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        // Solved using stack.TryPop
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"[compileStringError] unknown operator: {val} Error: {ex.Message}");
                        throw new Exception("[OperationAborted]");
                    }
                }
            }
            return stack.Pop();
        }
        /// <summary>
        /// Used to generate bool values for Token variables
        /// </summary>
        /// <param name="count">dimension of the boolean vector</param>
        /// <returns>List{true, false, ...(count)}</returns>
        private IEnumerable<List<bool>> GenerateTokenValues(int count)
        {
            Func<int, int, List<bool>> divide_mod = (int number, int autosize) => {
                List<bool> result = new List<bool>();
                while (number > 0)
                {
                    int mod = number % 2;
                    result.Add(mod == 1);
                    number = number / 2;
                }
                while (result.Count < autosize)
                {
                    result.Add(false);
                }
                return result;
            };

            for (int i = 0; i < Math.Pow(2, count); i++)
            {
                yield return divide_mod(i, count);
            }
        }
        public List<Dictionary<string, bool>> Bruteforce()
        {
            List<Dictionary<string, bool>> solutions = new List<Dictionary<string, bool>>();
            foreach (List<bool> values in GenerateTokenValues(this.TokenCount))
            {
                bool ret = this.EvalExpression(values);
                if (ret)
                {
                    Dictionary<string, bool> expression = new Dictionary<string, bool>();
                    //Func<Token, int, Token> feed_expression= (Token token, int i) => { expression.Add(token.Name, values[i]); return token; };
                    //TokenTracker.GetTokens().Select(feed_expression);
                    int i = 0;
                    foreach (Token token in TokenTracker.GetTokens())
                    {
                        expression.Add(token.Name, values[i]);
                        i++;
                    }
                    
                    solutions.Add(expression);
                }
            }
            return solutions;
        }

    }
}
