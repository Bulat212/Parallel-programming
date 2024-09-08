using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Laba1
{
    public delegate void MeetBulletRect(int y1, int y2);//создание делегата

    public class Ellipse
    {
        public event MeetBulletRect meetBulletRect;

        public int Size;
        public Brush Color;
        
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
        //public int Y { get; set; }

        public Ellipse(Brush color, int x, int y, int size)
        {
            Size = size;
            Color = color;
            this.x = x;
            this.y = y;
        }

        public void MoveEllipse()
        {
            y -= 10;
        }

        public void Meet(int y1, int y2)
        {
            if(y1 == y2)
            {
                meetBulletRect?.BeginInvoke(y1, y2, null, null);

                //if (meetBulletRect != null) meetBulletRect(y1, y2); //проверка есть ли в делегаты методы, если нет то получим null
            }
        }

    }
}
