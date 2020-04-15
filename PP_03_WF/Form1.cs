using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PP_03_WF
{
    public partial class Form1 : Form
    {
        private int[] radiusArray;
        private int[,] coordsArray;
        private bool[] checkArray;
        private Thread[] threadArray;
        private bool check = false;
        private int count = 3;              // КОЛИЧЕСТВО КРУГОВ

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen pen = new Pen(Color.Blue);

            if (check)
            {
                for (int i = 0; i < count; i++)
                {
                    g.DrawEllipse(pen, coordsArray[0, i] - radiusArray[i], coordsArray[1, i] - radiusArray[i], radiusArray[i]*2, radiusArray[i]*2);
                }

                for (int i = 0; i < count; i++)
                {
                    if (checkArray[i])
                    {
                        checkArray[i] = false;
                        Random randY = new Random();
                        Random randX = new Random();
                        coordsArray[0, i] = randX.Next(500*(i + 1));
                        coordsArray[1, i] = randX.Next(500);
                        radiusArray[i] = 0;
                        threadArray[i].Abort();
                        threadArray[i] = new Thread(threadPaint);
                        threadArray[i].Start(i);
                        
                    }
                }
            }
        }

        private void threadPaint(object index)
        {
            while (true)
            {
                Thread.Sleep(10);
                radiusArray[(int)index]++;
                if (check)
                {
                    Invalidate();
                }
            }
        }

        private int rast(int x1, int y1, int x2, int y2)
        {
            double temp = Math.Pow(x2 - x1, 2);
            temp += Math.Pow(y2 - y1, 2);
            return (int)Math.Sqrt(temp);
        }

        private void threadCheck()
        {
            while (true)
            {
                for (int i = 0; i < count; i++)
                {
                    for (int j = 0; j < count; j++)
                    {
                        if ((rast(coordsArray[0, i], coordsArray[1, i], coordsArray[0, j], coordsArray[1, j]) < (radiusArray[i] + radiusArray[j])) && (i != j))
                        {
                            if (radiusArray[i] > radiusArray[j])
                            {
                                checkArray[i] = true;
                            }
                        }
                    }

                    if ( (((coordsArray[0, i] - radiusArray[i]) < 0) || ((coordsArray[1, i] - radiusArray[i]) < 0)) || ((coordsArray[0, i] + radiusArray[i]) > this.Width) || ((coordsArray[1, i] + radiusArray[i]) > this.Height) )
                    {
                        checkArray[i] = true;
                    }
                }
                Thread.Sleep(10);
            }
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            check = true;
            radiusArray = new int[count];
            coordsArray = new int[2, count];
            checkArray = new bool[count];
            threadArray = new Thread[count];
            radiusArray[0] = 50;
            Random randY = new Random();
            Random randX = new Random();

            for (int i = 0; i < count; i++)
            {
                coordsArray[0, i] = randX.Next(500 * (i + 1));
                coordsArray[1, i] = randX.Next(500);
                checkArray[i] = false;
            }

            for (int i = 0; i < count; i++)
            {
                threadArray[i] = new Thread(threadPaint);
                threadArray[i].Start(i);
            }

            Thread thread_Check = new Thread(threadCheck);
            thread_Check.Start();
        }
    }
}
