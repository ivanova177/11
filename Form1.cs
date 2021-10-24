using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;
using Express = MathNet.Symbolics.SymbolicExpression;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        private void DrawGraph()
        {

            GraphPane pane = zedGraphControl1.GraphPane;// Получим панель для рисования
            pane.CurveList.Clear();
            AddCurve1(pane);
        }

        async Task<Double> method(double a, double b, double ep)
        {
            return await Task.Run(() => section(a, b, ep));
        }


        public PointPairList list1 = new PointPairList();
        public PointPairList list2 = new PointPairList();
        public double section(double a, double b, double ep)
        {

            var expr = Express.Parse(textBox4.Text);
            Func<double, double> f = expr.Compile("x");

            

            double goldenRatio = (-1 + Math.Sqrt(5)) / 2;
            double x1, x2;
            while (true)
            {
                x1 = b - (b - a) * goldenRatio;
                x2 =a + (b - a) * goldenRatio;
                if (f(x1) >= f(x2))
                {
                    a = x1;
                    PointPair g = new PointPair (x1, f(x1));
                    list1.Add(g);
                }
                else
                {
                    b = x2;
                    PointPair g2 = new PointPair (x2, f(x2));
                    list2.Add(g2);
                }
                if (Math.Abs(b - a) < ep)
                    break;

            }

            return (a + b) / 2;

        }

        private void AddCurve1(GraphPane pane)
        {
            try
            {
                double a = Convert.ToDouble(textBox1.Text);
                double b = Convert.ToDouble(textBox2.Text);
                double ep = Convert.ToDouble(textBox3.Text);

                double min = a;
                double max = b;
                double step = 1;
                double result = 0;


                var expr = Express.Parse(textBox4.Text);
                Func<double, double> f = expr.Compile("x");

                int d = (int)Math.Ceiling((b - a) / step) + 1;

                double[] xmin = new double[d];
                double[]fmin = new double[d];

                textBox5.Text = xmin.ToString();
                textBox6.Text = fmin.ToString();

                for (int i = 0; i <= d; i++)
                {
                    xmin[i] = a + step * i;
                    fmin[i] = f(xmin[i]);
                }
                pane.XAxis.Scale.Min = min;
                pane.XAxis.Scale.Min = max;
                pane.XAxis.Scale.MajorStep = step;
                list1.Add(xmin, fmin);
                LineItem myCurve = pane.AddCurve("Sinc", list1, Color.Pink, SymbolType.Star);



                zedGraphControl1.AxisChange();//обновляем данные
                zedGraphControl1.Invalidate();//обновляем график
            }
            catch(Exception)
            {
                MessageBox.Show("Некорректные данные!");
            }
        }

        private void HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DrawGraph();
        }

        private void очиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            textBox6.Clear();
            zedGraphControl1.Refresh();
        }
    }

    public class points
    {
        public double x;
        public double y;
        public points(double X, double Y)
        {
            this.x = X;
            this.y = Y;
        }
    }
}

