using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;

using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;

namespace Laba1
{
    public class Bullet
    {
        public int Size;
        public Brush Color;
       
        public Form1 window;
       
        private int x;
        private int y;

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

        public Bullet(Brush color, int x, int y, int size, Form1 form)
        {
            Size = size;
            Color = color;
            this.x = x;
            this.y = y;
            window = form;
        }
        public void Draw(Graphics g, Brush color, int x, int y, int size)
        {
            g.FillEllipse(Color, x, y, size, size);
        }

        public void MoveBullet(int yRect)
        {
            while (y > yRect)
            {
                y -= 20;
                Thread.Sleep(75);
                
            }

        }
    }
}
