using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Laba1
{
    public delegate void CheckBullet();        //создание делегата

    public delegate int Delegate(); //создание делегата для движения квадрата

    public class Rectangle
    {
        
        public event Action MoveRectangle; 
        

        public event CheckBullet SendBullet;     // создал событие отправить пулю

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

        public Rectangle(Brush color, int x, int y, int size)
        {
            Size = size;
            Color = color;
            this.x = x;
            this.y = y;
        }

        //асинхронная функция
        public int Move()
        {
            while (x < 480)
            {
                x += 10;
                Thread.Sleep(100);
            }
            return 1;
        }


        public void ReachedСenter() //квадрат дошел до центра и пуля отправляется
        {
            if (x > 400)
            {
                if (SendBullet != null) SendBullet.BeginInvoke(null, null);
            }

        }

        public void PrintNumbers()
        {
            while (x < 480)
            {
                x += 10;
                Thread.Sleep(100);
            }
        }
    }
}
