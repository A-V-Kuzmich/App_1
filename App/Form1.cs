using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using LexemeArithmeticExpression;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace App
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                label5.Text = "X";
                label6.Text = "Y";
            }
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                label5.Text = "r";
                label6.Text = "кутθ";
            }
        }
        private void closeBtn_Click(object sender, EventArgs e)
        {
            //this.Close();
            Application.Exit();
        }
        private void calc_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton1.Checked == true)
                {
                    // Створення двох об'єктів класу Point
                    double x1, x2, y1, y2;
                    x1 = Convert.ToDouble(textBox1.Text);
                    y1 = Convert.ToDouble(textBox2.Text);
                    x2 = Convert.ToDouble(textBox3.Text);
                    y2 = Convert.ToDouble(textBox4.Text);
                    Point point1 = new Point(x1, y1);
                    Point point2 = new Point(x2, y2);

                    // Виклик методу Distance для обчислення відстані між точками
                    double distance = point1.Distance(point2);

                    // Виведення результату на консоль
                    result.Text = Convert.ToString(distance);
                }
                else if (radioButton2.Checked == true)
                {
                    double r1, theta1, r2, theta2;
                    r1 = Convert.ToDouble(textBox1.Text);
                    theta1 = Convert.ToDouble(textBox2.Text);
                    r2 = Convert.ToDouble(textBox3.Text);
                    theta2 = Convert.ToDouble(textBox4.Text);

                    Point point1 = new Point(r1, theta1, isPolar: true);
                    Point point2 = new Point(r2, theta2, isPolar: true);

                    // Виклик методу Distance для обчислення відстані між точками
                    double distance = point1.Distance(point2);

                    // Виведення результату на консоль
                    result.Text = Convert.ToString(distance);
                }
            }
            catch
            {
                MessageBox.Show("Введіть коректно дані");
                return;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string expressionText = textBox5.Text;
            AriphmeticExpression expression = new AriphmeticExpression(expressionText);

            Dictionary<string, double> variables = new Dictionary<string, double>
            {
            { "x", Convert.ToDouble(txtX.Text) },
            { "y", Convert.ToDouble(txtY.Text) }
            };

            try
            {
                double result = expression.Calculate(variables);
                textBox9.Text = $"Результат: {result}";
            }
            catch (Exception ex)
            {
                textBox9.Text = $"Помилка: {ex.Message}";
            }
        }
//lab_2
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string inputText = textBox10.Text;

                LexicalAnalyzer analyzer = new LexicalAnalyzer();

                List<Lexeme> lexemes = analyzer.Analyze(inputText);

                textBox6.Clear();

                foreach (Lexeme lexeme in lexemes)
                {
                    textBox6.AppendText($"Token: {lexeme.Token}, Type: {lexeme.LexemeType}{Environment.NewLine}");
                }
            }
            catch
            {
                MessageBox.Show("Виникла помилка, перевірте введення даних");
            }

        }
    }
   
}
//lab_1.1
class Point
{
    private double x;
    private double y;
    private double r;
    private double theta;
    // Конструктор для створення об'єкту Point за декартовими координатами
    public Point(double x, double y)
    {
        this.x = x;
        this.y = y;
        // Обчислюємо полярні координати
        this.r = Math.Sqrt(x * x + y * y);
        this.theta = Math.Atan2(y, x);
    }
    // Конструктор для створення об'єкту Point за полярними координатами
    public Point(double r, double theta, bool isPolar = true)
    {
        if (isPolar)
        {
            this.r = r;
            this.theta = theta;
            // Обчислюємо декартові координати
            this.x = r * Math.Cos(theta);
            this.y = r * Math.Sin(theta);
        }
        else
        {
            this.x = r;
            this.y = theta;
            // Обчислюємо полярні координати
            this.r = Math.Sqrt(x * x + y * y);
            this.theta = Math.Atan2(y, x);
        }
    }
    // Метод для обчислення відстані між двома точками
    public double Distance(Point otherPoint)
    {
        double dx = this.x - otherPoint.x;
        double dy = this.y - otherPoint.y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}
//lab_2
public class Lexeme
{
    public string Token { get; set; }
    public string LexemeType { get; set; }
    public Lexeme(string token, string lexemeType)
    {
        Token = token;
        LexemeType = lexemeType;
    }
}
public class LexicalAnalyzer
{
    public List<Lexeme> Analyze(string inputText)
    {
        List<Lexeme> lexemes = new List<Lexeme>();
        var tokenPatterns = new List<Tuple<string, string>>
        {
           // new Tuple<string, string>(@"\bint\b", "Keyword"),
           // new Tuple<string, string>(@"\bstring\b", "Keyword"),
           // new Tuple<string, string>(@"\bif\b", "Keyword"),
           // new Tuple<string, string>(@"\belse\b", "Keyword"),
            new Tuple<string, string>(@"\b(if|else|for|while|int|double|return)\b", "ControlStatements"),
            new Tuple<string, string>(@"\b[a-zA-Z_]\w*\b(?![(])", "letter"),
            new Tuple<string, string>(@"\d+", "Number"),
            new Tuple<string, string>(@"\+|\-|\*|\/|\=|\==|\!=|\<|\>|\<=|\>=|\&\&|\|\||\!|\%", "Symbol"),
            new Tuple<string, string>(@"\s+|,|;|\(|\)|\[|\]|\{|\}|\.|:", "SeparatorSymbol"),
            //new Tuple<string, string>(@".", "WrongData"),
            // add another rules.   
        };

        foreach (var pattern in tokenPatterns)
        {
            var regex = new Regex(pattern.Item1);
            var matches = regex.Matches(inputText);

            foreach (Match match in matches)
            {
                lexemes.Add(new Lexeme(match.Value, pattern.Item2));
            }

        }
        return lexemes;
    }
}

//lab_1.2
//private void textbox1_keypress(object sender, keypresseventargs e)
//{
//    if (!char.iscontrol(e.keychar) && !char.isdigit(e.keychar))
//    {
//        e.handled = true; // відхилити символ від введення
//    }
//}

