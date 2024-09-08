using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Laba1
{
    public delegate void MeetBulletRect(int y1, int y2);//создание делегата

    public class Rectangle
    {
        public event MeetBulletRect meetBulletRect;

        public int Size;
        public Brush Color;

        private int x;
        private int y;

        //Form window = new Form();
        public Form1 window;
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        //public int Y { get; set; }

        public Rectangle(Brush color, int x, int y, int size, Form1 form)
        {
            Size = size;
            Color = color;
            this.x = x;
            this.y = y;
            window = form;
        }

        public void Draw(Graphics g)
        {
            g.FillRectangle(Color, x, y, Size, Size);
        }

        public async Task MoveRectangle(Ellipse r, Bullet b1, Bullet b2)
        {
            window.Invoke((Action)delegate
            {
                window.Text = "Квадрат двигается";
            });
            await Task.Run(() =>
            {
                while (x < 510)
                {
                    Thread.Sleep(100);
                    x += 10;

                    r.Podpiska(r, b1, b2, this.x);

                }
            });
            await Task.Run(() =>
            {
                while(true)
                {
                    if (this.y == b1.Y)
                    {
                        MeetBulletRect();
                        break;
                    }
                }
            });

        }

        public void MeetBulletRect() //обработчик события встречи
        {
            Color = Brushes.Red;
        }

    }
}
