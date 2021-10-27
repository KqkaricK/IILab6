using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using DarkUI.Forms;


namespace IILab5
{
    public partial class Form1 : DarkForm
    {
        Random rnd = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        double func1(double x)
        {
            return Math.Cos(x) + 2 * x * Math.Cos(x * x);
        }
        double func2(double x)
        {
            return 4 * x * x * x - 18 * x * x + 12;
        }
        double func3a(double x, double y)
        {
            return -400 * x * (-(x * x) + y) + 2 * x + 2;
        }
        double func3b(double x, double y)
        {
            return -200 * x * x + 200 * y;
        }
        double func4a(double x, double y)
        {
            return 2 * x * y * y * y;
        }
        double func4b(double x, double y)
        {
            return 3 * x * x * y * y;
        }
        double mutation(double x0, double x1)  // мутация: генерация случайной величины
        {
            const int NUM = 100000000;
            return Math.Abs((double)((rnd.Next() * NUM) % (int)((x1 - x0) * NUM) + 1) / NUM) + x0;
        }
        double inversion(double x, double eps)  // инверсия: поиск в окрестностях точки
        {
            int sign = 0;
            sign++;
            sign %= 2;
            if (sign == 0) return x - eps;
            else return x + eps;
        }
        void crossover(double[] x, double eps, double x0, double x1)  // кроссовер: среднее арифметическое
        {
            int k = 99;
            for (int i = 0; i < 8; i++)
                for (int j = i + 1; j < 8; j++)
                {
                    x[k] = (x[i] + x[j]) / 2;
                    k--;
                }
            for (int i = 0; i < 8; i++)
            {
                x[k] = inversion(x[i], eps); k--;
                x[k] = inversion(x[i], eps); k--;
            }
            for (int i = 8; i < k; i++)
                x[i] = mutation(x0, x1);
        }
        void sort(double[] x, double[] y)  // сортировка
        {
            for (int i = 0; i < 100; i++)
                for (int j = i + 1; j < 100; j++)
                    if (Math.Abs(y[j]) < Math.Abs(y[i]))
                    {
                        double temp = y[i];
                        y[i] = y[j];
                        y[j] = temp;
                        temp = x[i];
                        x[i] = x[j];
                        x[j] = temp;
                    }
        }

        double genetic1(double x0, double x1, double eps)  // поиск решения с использованием ГА
        {
            double[] population = new double[100];
            double[] f = new double[100];
            int iter = 0;
            for (int i = 0; i < 100; i++)   // Формирование начальной популяции
            {
                population[i] = mutation(x0, x1);
                f[i] = func1(population[i]);
            }
            sort(population, f);
            do
            {
                iter++;
                crossover(population, eps, x0, x1);
                for (int i = 0; i < 100; i++)
                    f[i] = func1(population[i]);
                sort(population, f);
                chart1.Series[0].Points.AddXY(iter, population[0]);
            } while (Math.Abs(f[0]) > eps && iter < 100);
            darkLabel2.Text = iter.ToString();
            return population[0];
        }
        double genetic2(double x0, double x1, double eps)  // поиск решения с использованием ГА
        {
            double[] population = new double[100];
            double[] f = new double[100];
            int iter = 0;
            for (int i = 0; i < 100; i++)   // Формирование начальной популяции
            {
                population[i] = mutation(x0, x1);
                f[i] = func2(population[i]);
            }
            sort(population, f);
            do
            {
                iter++;
                crossover(population, eps, x0, x1);
                for (int i = 0; i < 100; i++)
                    f[i] = func2(population[i]);
                sort(population, f);
                chart1.Series[0].Points.AddXY(iter, population[0]);
            } while (Math.Abs(f[0]) > eps && iter < 100);
            darkLabel2.Text = iter.ToString();
            return population[0];
        }
        string genetic3(double x0, double x1, double y0, double y1, double eps)  // поиск решения с использованием ГА
        {
            double[] population1 = new double[100];
            double[] population2 = new double[100];
            double[] f1 = new double[100];
            double[] f2 = new double[100];
            int iter = 0;
            for (int i = 0; i < 100; i++)   // Формирование начальной популяции
            {
                population1[i] = mutation(x0, x1);
                population2[i] = mutation(y0, y1);
                f1[i] = func3a(population1[i], population2[i]);
                f2[i] = func3b(population1[i], population2[i]);
            }
            sort(population1, f1);
            sort(population2, f2);
            do
            {
                iter++;
                crossover(population1, eps, x0, x1);
                crossover(population2, eps, y0, y1);
                for (int i = 0; i < 100; i++)
                {
                    f1[i] = func3a(population1[i], population2[i]);
                    f2[i] = func3b(population1[i], population2[i]);
                }
                sort(population1, f1);
                sort(population2, f2);
                chart1.Series[0].Points.AddXY(iter, population1[0]);
                chart1.Series[1].Points.AddXY(iter, population2[0]);
            } while (Math.Abs(f1[0]) > eps && Math.Abs(f2[0]) > eps && iter < 100);
            darkLabel2.Text = iter.ToString();
            return population1[0].ToString() + " y= " + population2[0].ToString();
        }
        string genetic4(double x0, double x1, double y0, double y1, double eps)  // поиск решения с использованием ГА
        {
            double[] population1 = new double[100];
            double[] population2 = new double[100];
            double[] f1 = new double[100];
            double[] f2 = new double[100];
            int iter = 0;
            for (int i = 0; i < 100; i++)   // Формирование начальной популяции
            {
                population1[i] = mutation(x0, x1);
                population2[i] = mutation(y0, y1);
                f1[i] = func4a(population1[i], population2[i]);
                f2[i] = func4b(population1[i], population2[i]);
            }
            sort(population1, f1);
            sort(population2, f2);
            do
            {
                iter++;
                crossover(population1, eps, x0, x1);
                crossover(population2, eps, y0, y1);
                for (int i = 0; i < 100; i++)
                {
                    f1[i] = func4a(population1[i], population2[i]);
                    f2[i] = func4b(population1[i], population2[i]);
                }
                sort(population1, f1);
                sort(population2, f2);
                chart1.Series[0].Points.AddXY(iter, population1[0]);
                chart1.Series[1].Points.AddXY(iter, population2[0]);
            } while (Math.Abs(f1[0]) > eps && Math.Abs(f2[0]) > eps && iter < 100);
            darkLabel2.Text = iter.ToString();
            return population1[0].ToString() + " y= " + population2[0].ToString();
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            double x0 = Convert.ToDouble(darkTextBox1.Text);
            double x1 = Convert.ToDouble(darkTextBox2.Text);
            double E = Convert.ToDouble("0," + t_e.Text);
            chart1.Series[0].Points.Clear();
            chart1.Series[1].Points.Clear();
            if (darkRadioButton1.Checked == true)
            {
                darkLabel1.Text = genetic1(x0, x1, E).ToString();
            }
            else if (darkRadioButton2.Checked == true)
            {
                darkLabel1.Text = genetic2(x0, x1, E).ToString();
            }
            else if (darkRadioButton3.Checked == true)
            {
                darkLabel1.Text = genetic3(x0, x1, x0, x1, E);
            }
            else 
            {
                darkLabel1.Text = genetic4(x0, x1, x0, x1, E);
            }
        }
        private void darkTextBox1_KeyPress(object sender, KeyPressEventArgs e) //запрет на ввод
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 45 && number != 44)
            {
                e.Handled = true;
            }
        }
        private void darkTextBox2_KeyPress(object sender, KeyPressEventArgs e) //запрет на ввод
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8)
            {
                e.Handled = true;
            }
        }
        void Copyraight()
        {
            MessageBox.Show("   Created and Development by AK. \n«Однажды произошел взрыв, он породил пространство и время. \n Однажды произошел взрыв и планета начала вращаться в пространстве. \n Однажды произошел взрыв, он породил жизнь, какой мы её знаем.\n Затем ещё один взрыв… \n                                                        … и для нас он станет последним». \n                                                                            — Death Stranding");
        } 

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F11)
            {
                Copyraight();
            }
        }
    }
}
