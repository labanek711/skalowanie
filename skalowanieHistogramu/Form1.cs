using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace skalowanieHistogramu
{
    public partial class Form1 : Form
    {
        private int[] red = null;
        private int[] green = null;
        private int[] blue = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Bitmap box1 = (Bitmap)pictureBox1.Image;
            int r, g, b;
            red = new int[256];
            green = new int[256];
            blue = new int[256];
            for (int x = 0; x < pictureBox1.Image.Width; x++)
            {
                for (int y = 0; y < pictureBox1.Image.Height; y++)
                {
                    Color picture1 = box1.GetPixel(x, y);
                    r = picture1.R;
                    g = picture1.G;
                    b = picture1.B;
                    red[r]++;
                    green[g]++;
                    blue[b]++;
                }
            }
            chart1.Series["red"].Points.Clear();
            chart1.Series["green"].Points.Clear();
            chart1.Series["blue"].Points.Clear();

            for (int i = 0; i < 256; i++)

            {
                chart1.Series["red"].Points.AddXY(i, red[i]);
                chart1.Series["green"].Points.AddXY(i, green[i]);
                chart1.Series["blue"].Points.AddXY(i, blue[i]);
            }
            chart1.Invalidate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap box1 = (Bitmap)pictureBox1.Image;

            Color k;
            int r, g, b;

            int[] RED = skalowanie(red);
            int[] GREEN = skalowanie(green);
            int[] BLUE = skalowanie(blue);

            red = new int[256];
            green = new int[256];
            blue = new int[256];

            for (int x = 0; x < pictureBox1.Width; x++)
            {
                for (int y = 0; y < pictureBox1.Height; y++)
                {
                    k = box1.GetPixel(x, y);
                    r = k.R;
                    g = k.G;
                    b = k.B;

                    box1.SetPixel(x, y, Color.FromArgb(RED[r], GREEN[g], BLUE[b]));
                    red[r]++;
                    green[g]++;
                    blue[b]++;
                }
            }
            pictureBox2.Refresh();

            chart2.Series["red"].Points.Clear();
            chart2.Series["green"].Points.Clear();
            chart2.Series["blue"].Points.Clear();
            for (int i = 0; i < 256; i++)
            {
                chart2.Series["red"].Points.AddXY(i, RED[i]);
                chart2.Series["green"].Points.AddXY(i, GREEN[i]);
                chart2.Series["blue"].Points.AddXY(i, BLUE[i]);
            }
            chart2.Invalidate();
        }

        private int[] skalowanie(int[] values)
        {
            int minValue = 0;
            for (int i = 0; i < 256; i++)
            {
                if (values[i] != 0)
                {
                    minValue = i;
                    break;
                }
            }

            int maxValue = 255;
            for (int i = 255; i >= 0; i--)
            {
                if (values[i] != 0)
                {
                    maxValue = i;
                    break;
                }
            }

            int[] result = new int[256];
            double a = 255.0 / (maxValue - minValue);
            for (int i = 0; i < 256; i++)
            {
                result[i] = (int)(a * (i - minValue));
            }

            return result;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap b1 = (Bitmap)pictureBox2.Image;

            Color[,] k = new Color[3, 3];
            int r = 0, g = 0, b = 0, suma_maski = 0;
            int[,] maska = new int[3, 3];

            maska[0, 0] = Convert.ToInt32(textBox1.Text);
            maska[0, 1] = Convert.ToInt32(textBox2.Text);
            maska[0, 2] = Convert.ToInt32(textBox3.Text);
            maska[1, 0] = Convert.ToInt32(textBox4.Text);
            maska[1, 1] = Convert.ToInt32(textBox5.Text);
            maska[1, 2] = Convert.ToInt32(textBox6.Text);
            maska[2, 0] = Convert.ToInt32(textBox7.Text);
            maska[2, 1] = Convert.ToInt32(textBox8.Text);
            maska[2, 2] = Convert.ToInt32(textBox9.Text);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    suma_maski += maska[i, j];
                }
            }

            for (int x = 0; x < pictureBox2.Image.Width; x++)
            {
                if (x == 0 || x == pictureBox2.Image.Width - 1)
                {
                    continue;
                }
                for (int y = 0; y < pictureBox2.Image.Height; y++)
                {
                    if (y == 0 || y == pictureBox2.Image.Height - 1)
                    {
                        continue;
                    }
                    for (int i = -1; i < 2; i++)
                    {

                        for (int j = -1; j < 2; j++)
                        {

                            k[i + 1, j + 1] = b1.GetPixel(x + i, y + j);

                            r += (k[i + 1, j + 1].R * maska[i + 1, j + 1]);
                            g += (k[i + 1, j + 1].G * maska[i + 1, j + 1]);
                            b += (k[i + 1, j + 1].B * maska[i + 1, j + 1]);
                        }
                    }
                    if (suma_maski != 0)
                    {
                        r = r / suma_maski;
                        g = g / suma_maski;
                        b = b / suma_maski;

                    }

                    if (r > 255)
                    {
                        r = 255;
                    }
                    else if (r < 0)
                    {
                        r = 0;
                    }


                    if (g > 255)
                    {
                        g = 255;
                    }
                    else if (g < 0)
                    {
                        g = 0;
                    }

                    if (b > 255)
                    {
                        b = 255;
                    }
                    else if (b < 0)
                    {
                        b = 0;
                    }

                    b1.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            pictureBox2.Refresh();
        }
    }
}