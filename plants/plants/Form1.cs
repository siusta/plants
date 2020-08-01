using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace plants
{
    public partial class Form1 : Form
    {
        bool plantclick;

        private string generate(int n, string result = "[X]")
        {
            for (int i = 0; i < n; i++)
            {
                result = result.Replace("F", textBox1.Text);        //FF
                result = result.Replace("X", textBox2.Text);        //F[-XF]-F[[+X][X][+X]F]
            }
            return result;
        }

        private void coords(Point point, double angle, double size)
        {
            point.X = point.X + Convert.ToInt32(Math.Cos(angle * Math.PI / 180) * size);
            point.Y = point.Y + Convert.ToInt32(Math.Sin(angle * Math.PI / 180) * size);
        }

        private void draw(PaintEventArgs e, string plant, double size)
        {
            char[] cmd;
            cmd = plant.ToCharArray();

            Pen pen = new Pen(Brushes.ForestGreen);
            pen.Width = (float)trackBar1.Value/5;
            pen.LineJoin = System.Drawing.Drawing2D.LineJoin.Bevel;

            Stack<double> angleReserve = new Stack<double>();
            Stack<int> xReserve = new Stack<int>();
            Stack<int> yReserve = new Stack<int>();

            double angle = 180;
            int x0, y0, x, y;
            x0 = panel1.Width / 2;
            y0 = panel1.Height;
            x = x0 + Convert.ToInt32(Math.Sin(angle * Math.PI / 180) * size);
            y = y0 + Convert.ToInt32(Math.Cos(angle * Math.PI / 180) * size);

            for (int i = 0; i < cmd.Length; i++)
            {
                if (cmd[i] == 'F')
                {
                    x = x0 + Convert.ToInt32(Math.Sin(angle * Math.PI / 180) * size);
                    y = y0 + Convert.ToInt32(Math.Cos(angle * Math.PI / 180) * size);
                    e.Graphics.DrawLine(pen, x0, y0, x, y);
                    x0 = x;
                    y0 = y;
                }
                else if (cmd[i] == '-') angle = angle + trackBar5.Value;         //left
                else if (cmd[i] == '+') angle = angle - trackBar4.Value;         //right
                else if (cmd[i] == 'X') continue;
                else if (cmd[i] == '[')
                {
                    angleReserve.Push(angle);
                    xReserve.Push(x);
                    yReserve.Push(y);
                }
                else if (cmd[i] == ']')
                {
                    if (angleReserve.Any<double>()) 
                    {
                        angle = angleReserve.Pop();
                        x0 = xReserve.Pop();
                        y0 = yReserve.Pop();
                    }
                }
                else break;
            }

            pen.Dispose();
        }

        public Form1()
        {
            InitializeComponent();
            label6.Text = ((float)trackBar1.Value/5).ToString();
            label7.Text = trackBar2.Value.ToString();
            label8.Text = trackBar3.Value.ToString();
            label9.Text = trackBar4.Value.ToString();
            label10.Text = trackBar5.Value.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            plantclick = true;
            panel1.Invalidate();
        }
        

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (plantclick)
            {
                string plant;
                plant = generate(trackBar3.Value);
                richTextBox1.Text = plant;
                draw(e, plant, trackBar2.Value);
            }
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            label8.Text = trackBar3.Value.ToString();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            label9.Text = trackBar4.Value.ToString();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label6.Text = ((float)trackBar1.Value/5).ToString();
        }

        private void trackBar2_Scroll_1(object sender, EventArgs e)
        {
            label7.Text = trackBar2.Value.ToString();
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            label10.Text = trackBar5.Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + "F";
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + "X";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + "+";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + "-";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + "[";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox2.Text + "]";
        }


    }
}
