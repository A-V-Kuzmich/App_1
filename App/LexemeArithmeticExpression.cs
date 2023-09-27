using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexemeArithmeticExpression
{
    public enum LexemeType
    {
        Number,
        Variable,
        Operator,
        Function,
        LeftParenthesis,
        RightParenthesis
    }
    public class Lexeme
    {
        public string Value { get; set; }
        public LexemeType Type { get; set; }

        public Lexeme(string value, LexemeType type)
        {
            Value = value;
            Type = type;
        }
    }
    public class AriphmeticExpression
    {
        public string ExpressionText { get; set; }
        private List<Lexeme> lexemes;

        public AriphmeticExpression(string expressionText)
        {
            ExpressionText = expressionText;
            lexemes = ParseExpression();
        }

        public double Calculate(Dictionary<string, double> variables)
        {
            Stack<double> values = new Stack<double>();
            Stack<Lexeme> operators = new Stack<Lexeme>();

            foreach (var lexeme in lexemes)
            {
                if (lexeme.Type == LexemeType.Number)
                {
                    values.Push(double.Parse(lexeme.Value));
                }
                else if (lexeme.Type == LexemeType.Variable)
                {
                    if (variables.ContainsKey(lexeme.Value))
                    {
                        values.Push(variables[lexeme.Value]);
                    }
                    else
                    {
                        throw new Exception($"змінна {lexeme.Value} не визначена.");
                    }
                }
                else if (lexeme.Type == LexemeType.Operator)
                {
                    while (operators.Count > 0 &&
                           GetOperatorPrecedence(operators.Peek()) >= GetOperatorPrecedence(lexeme))
                    {
                        double rightOperand = values.Pop();
                        double leftOperand = values.Pop();
                        Lexeme op = operators.Pop();
                        double result = ApplyOperator(op, leftOperand, rightOperand);
                        values.Push(result);
                    }
                    operators.Push(lexeme);
                }
                else if (lexeme.Type == LexemeType.LeftParenthesis)
                {
                    operators.Push(lexeme);
                }
                else if (lexeme.Type == LexemeType.RightParenthesis)
                {
                    while (operators.Count > 0 && operators.Peek().Type != LexemeType.LeftParenthesis)
                    {
                        double rightOperand = values.Pop();
                        double leftOperand = values.Pop();
                        Lexeme op = operators.Pop();
                        double result = ApplyOperator(op, leftOperand, rightOperand);
                        values.Push(result);
                    }
                    if (operators.Count > 0 && operators.Peek().Type == LexemeType.LeftParenthesis)
                    {
                        operators.Pop();
                    }
                    else
                    {
                        throw new Exception("невідповідність дужок");
                    }
                }
            }

            while (operators.Count > 0)
            {
                double rightOperand = values.Pop();
                double leftOperand = values.Pop();
                Lexeme op = operators.Pop();
                double result = ApplyOperator(op, leftOperand, rightOperand);
                values.Push(result);
            }

            if (values.Count == 1)
            {
                return values.Pop();
            }
            else
            {
                throw new Exception("Помилка");
            }
        }

        private int GetOperatorPrecedence(Lexeme lexeme)
        {
            switch (lexeme.Value)
            {
                case "+":
                case "-":
                    return 1;
                case "*":
                case "/":
                    return 2;
                default:
                    return 0;
            }
        }

        private double ApplyOperator(Lexeme op, double leftOperand, double rightOperand)
        {
            switch (op.Value)
            {
                case "+":
                    return leftOperand + rightOperand;
                case "-":
                    return leftOperand - rightOperand;
                case "*":
                    return leftOperand * rightOperand;
                case "/":
                    if (rightOperand != 0)
                    {
                        return leftOperand / rightOperand;
                    }
                    else
                    {
                        throw new Exception("Ділення на 0.");
                    }
                default:
                    throw new Exception($"не відомий оператор {op.Value}.");
            }
        }

        public List<Lexeme> ParseExpression()
        {
            return new List<Lexeme>();
        }
    }
}
