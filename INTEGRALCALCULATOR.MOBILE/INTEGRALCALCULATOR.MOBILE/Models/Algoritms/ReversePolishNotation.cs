using INTEGRALCALCULATOR.MOBILE.Models.Structs;
using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace INTEGRALCALCULATOR.MOBILE.Models
{
    public class ReversePolishNotation
    {
        private Queue _output;
        private Stack _ops;

        public string OriginalExpression { get; set; }
        private string TransitionExpression { get; set; }
        private string PostfixExpression { get; set; }

        public ReversePolishNotation()
        {
            OriginalExpression = string.Empty;
            TransitionExpression = string.Empty;
            PostfixExpression = string.Empty;
        }

        public void Parse(string expression)
        {
            _output = new Queue();
            _ops = new Stack();

            OriginalExpression = expression;

            string buffer = expression.ToLower();
            // Przechwytywanie liczb.
            buffer = Regex.Replace(buffer, @"(?<number>\d+(\.\d+)?)", " ${number} ");
            // Przechwytywnie symboli.
            buffer = Regex.Replace(buffer, @"(?<ops>[+\-*/^()])", " ${ops} ");
            // przechwytywanie stałych, funkcji trygonometrycznych i innych funkcji
            buffer = Regex.Replace(buffer, "(?<alpha>(pi|e|sin|cos|tan|sqrt))", " ${alpha} ");
            //Przechwytywanie minusa unarnego
            buffer = Regex.Replace(buffer, "-", "MINUS");
            buffer = Regex.Replace(buffer, @"(?<number>(pi|e|(\d+(\.\d+)?)))\s+MINUS", "${number} -");
            buffer = Regex.Replace(buffer, "MINUS", "~");

            TransitionExpression = buffer;

            //Przypisywanie do tokenów.

            buffer = buffer.Replace(".", ",");
            string[] parsed = buffer.Split(" ".ToCharArray());
            int i = 0;
            double tokenValue;
            RPNToken token, opstoken;

            for(i=0; i< parsed.Length; i++)
            {
                token = new RPNToken();
                token.TokenValue = parsed[i];
                token.TokenValueType = TokenTypeEnum.NONE;
                try
                {
                    tokenValue = double.Parse(parsed[i]);
                    token.TokenValueType = TokenTypeEnum.NUMBER;
                    _output.Enqueue(token);
                }
                catch
                {
                    switch (parsed[i])
                    {
                        case "+":
                            token.TokenValueType = TokenTypeEnum.PLUS;
                            if(_ops.Count > 0)
                            {
                                opstoken = (RPNToken)_ops.Peek();
                                while (IsOperatorToken(opstoken.TokenValueType))
                                {
                                    _output.Enqueue(_ops.Pop());
                                    if(_ops.Count > 0)
                                    {
                                        opstoken = (RPNToken)_ops.Peek();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            _ops.Push(token);
                            break;
                        case "-":
                            token.TokenValueType = TokenTypeEnum.MINUS;
                            if (_ops.Count > 0)
                            {
                                opstoken = (RPNToken)_ops.Peek();
                                while(IsOperatorToken(opstoken.TokenValueType))
                                {
                                    _output.Enqueue(_ops.Pop());
                                    if (_ops.Count > 0)
                                    {
                                        opstoken = (RPNToken)_ops.Peek();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            _ops.Push(token);
                            break;
                        case "*":
                            token.TokenValueType = TokenTypeEnum.MULTIPLY;
                            if(_ops.Count > 0)
                            {
                                opstoken = (RPNToken)_ops.Peek();
                                while (IsOperatorToken(opstoken.TokenValueType))
                                {
                                    if(opstoken.TokenValueType == TokenTypeEnum.PLUS || opstoken.TokenValueType == TokenTypeEnum.MINUS)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        _output.Enqueue(_ops.Pop());
                                        if (_ops.Count > 0)
                                        {
                                            opstoken = (RPNToken)_ops.Peek();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            _ops.Push(token);
                            break;
                        case "/":
                            token.TokenValueType = TokenTypeEnum.DIVIDE;
                            if(_ops.Count > 0)
                            {
                                opstoken = (RPNToken)_ops.Peek();
                                while (IsOperatorToken(opstoken.TokenValueType))
                                {
                                    if(opstoken.TokenValueType == TokenTypeEnum.PLUS || opstoken.TokenValueType == TokenTypeEnum.MINUS)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        _output.Enqueue(_ops.Pop());
                                        if(_ops.Count > 0)
                                        {
                                            opstoken = (RPNToken)_ops.Peek();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                            _ops.Push(token);
                            break;
                        case "^":
                            token.TokenValueType = TokenTypeEnum.POWER;
                            _ops.Push(token);
                            break;
                        case "~":
                            token.TokenValueType = TokenTypeEnum.UNARY_MINUS;
                            _ops.Push(token);
                            break;
                        case "(":
                            token.TokenValueType = TokenTypeEnum.LEFT_BRACKET;
                            _ops.Push(token);
                            break;
                        case ")":
                            token.TokenValueType = TokenTypeEnum.RIGHT_BRACKET;
                            if (_ops.Count > 0)
                            {
                                opstoken = (RPNToken)_ops.Peek();
                                while(opstoken.TokenValueType != TokenTypeEnum.LEFT_BRACKET)
                                {
                                    _output.Enqueue(_ops.Pop());
                                    if (_ops.Count > 0)
                                    {
                                        opstoken = (RPNToken)_ops.Peek();
                                    }
                                    else
                                    {
                                        throw new Exception("Unbalanced Parethesis!");
                                    }
                                }
                                _ops.Pop();
                            }

                            if(_ops.Count > 0)
                            {
                                opstoken = (RPNToken)_ops.Peek();

                                if (IsFunctionToken(opstoken.TokenValueType))
                                {
                                    _output.Enqueue(_ops.Pop());
                                }
                            }
                            break;
                        case "pi":
                            token.TokenValueType = TokenTypeEnum.CONSTANT;
                            _output.Enqueue(token);
                            break;
                        case "x":
                            token.TokenValueType = TokenTypeEnum.VARIABLE;
                            _output.Enqueue(token);
                            break;
                        case "e":
                            token.TokenValueType = TokenTypeEnum.CONSTANT;
                            _output.Enqueue(token);
                            break;
                        case "sqrt":
                            token.TokenValueType = TokenTypeEnum.SQRT;
                            _ops.Push(token);
                            break;
                        case "sin":
                            token.TokenValueType = TokenTypeEnum.SIN;
                            _ops.Push(token);
                            break;
                        case "cos":
                            token.TokenValueType = TokenTypeEnum.COS;
                            _ops.Push(token);
                            break;
                        case "tan":
                            token.TokenValueType = TokenTypeEnum.TAN;
                            _ops.Push(token);
                            break;

                    }
                }
            }

            while(_ops.Count != 0)
            {
                opstoken = (RPNToken)_ops.Pop();
                if(opstoken.TokenValueType == TokenTypeEnum.LEFT_BRACKET)
                {
                    throw new Exception("Unbalanced parenthesis!");
                }
                else
                {
                    _output.Enqueue(opstoken);
                }
            }

            PostfixExpression = string.Empty;
            foreach(object obj in _output)
            {
                opstoken = (RPNToken)obj;
                PostfixExpression += string.Format("{0} ", opstoken.TokenValue);
            }
        }

        public double Evaluate(double x=0)
        {
            Stack result = new Stack();
            double oper1 = 0.0, oper2 = 0.0;
            RPNToken token = new RPNToken();
            foreach(object obj in _output)
            {
                token = (RPNToken)obj;
                switch (token.TokenValueType)
                {
                    case TokenTypeEnum.NUMBER:
                        result.Push(double.Parse(token.TokenValue));
                        break;
                    case TokenTypeEnum.CONSTANT:
                        result.Push(EvaluateConstant(token.TokenValue));
                        break;
                    case TokenTypeEnum.VARIABLE:
                        result.Push(x);
                        break;
                    case TokenTypeEnum.PLUS:
                        if(result.Count >= 2)
                        {
                            oper2 = (double)result.Pop();
                            oper1 = (double)result.Pop();
                            result.Push(oper1 + oper2);
                        }
                        else
                        {
                            throw new Exception("Evaluate error!");
                        }
                        break;
                    case TokenTypeEnum.MINUS:
                        if(result.Count >= 2)
                        {
                            oper2 = (double)result.Pop();
                            oper1 = (double)result.Pop();
                            result.Push(oper1 - oper2);
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenTypeEnum.MULTIPLY:
                        if (result.Count >= 2)
                        {
                            oper2 = (double)result.Pop();
                            oper1 = (double)result.Pop();
                            result.Push(oper1 * oper2);
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenTypeEnum.POWER:
                        if (result.Count >= 2)
                        {
                            oper2 = (double)result.Pop();
                            oper1 = (double)result.Pop();
                            result.Push(Math.Pow(oper1, oper2));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenTypeEnum.DIVIDE:
                        if(result.Count >= 2)
                        {
                            oper2 = (double)result.Pop();
                            oper1 = (double)result.Pop();
                            result.Push(oper1 / oper2);
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenTypeEnum.SQRT:
                        if (result.Count >= 1)
                        {
                            oper1 = (double)result.Pop();
                            result.Push(Math.Sqrt(oper1));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenTypeEnum.UNARY_MINUS:
                        if(result.Count >= 1)
                        {
                            oper1 = (double)result.Pop();
                            result.Push(-oper1);
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenTypeEnum.SIN:
                        if(result.Count >= 1)
                        {
                            oper1 = (double)result.Pop();
                            result.Push(Math.Sin(oper1));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                    case TokenTypeEnum.COS:
                        if (result.Count >= 1)
                        {
                            oper1 = (double)result.Pop();
                            result.Push(Math.Tan(oper1));
                        }
                        else
                        {
                            throw new Exception("Evaluation error!");
                        }
                        break;
                }
            }
            if (result.Count == 1)
            {
                return (double)result.Pop();
            }
            else
            {
                throw new Exception("Evaluation error!");
            }
        }

        private bool IsOperatorToken(TokenTypeEnum t)
        {
            bool result = false;
            switch (t)
            {
                case TokenTypeEnum.PLUS:
                case TokenTypeEnum.MINUS:
                case TokenTypeEnum.MULTIPLY:
                case TokenTypeEnum.DIVIDE:
                case TokenTypeEnum.POWER:
                case TokenTypeEnum.UNARY_MINUS:
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        private bool IsFunctionToken(TokenTypeEnum t)
        {
            bool result = false;
            switch (t)
            {
                case TokenTypeEnum.SQRT:
                case TokenTypeEnum.SIN:
                case TokenTypeEnum.COS:
                case TokenTypeEnum.TAN:
                    result = true;
                    break;
                default:
                    result = false;
                    break;
            }
            return result;
        }

        private double EvaluateConstant(string TokenValue)
        {
            double result = 0.0;
            switch (TokenValue)
            {
                case "pi":
                    result = Math.PI;
                    break;
                case "e":
                    result = Math.E;
                    break;
            }
            return result;
        }
    }
}
