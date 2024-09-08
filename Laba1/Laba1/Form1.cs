using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba1
{
    public partial class Form1 : Form
    {

        Ellipse ellipse = new Ellipse(Brushes.Green, 500, 400, 50);
        Ellipse bullet = new Ellipse(Brushes.Black, 500, 400, 10);
        Rectangle rect = new Rectangle(Brushes.Yellow, 50, 80, 50);

        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(800, 500);

            // Получить объект делегата для асинхронного вызова функции Move()
            Delegate del = new Delegate(rect.Move);
            // Инициировать вызов асинхронной функции Move
            IAsyncResult asyncResult = del.BeginInvoke(new AsyncCallback(CallbackF), del);
 
            rect.SendBullet += new CheckBullet(Rect_SendBullet); //подписываемся на событие отправки пули
            ellipse.meetBulletRect += new MeetBulletRect(Ellipse_meetBulletRect);

        }

        //функция обратного вызова
        static void CallbackF(IAsyncResult asyncResult)
        {
            // Получить ссылку на объект делегата CheckBullet
            Delegate del = (Delegate)asyncResult.AsyncState;
            // Вызвать функцию EndInvoke() для получения результата
            int endMoveRectangl = 0;
            endMoveRectangl = del.EndInvoke(asyncResult);
            if (endMoveRectangl == 1)
            {
                MessageBox.Show("Закончилось");
            }
            
        }

        private void Rect_SendBullet() //обработчик события отправки пули
        {
            while (bullet.Y > 80)
            {
                bullet.MoveEllipse();
                Thread.Sleep(100);
            }
        }

        private void Ellipse_meetBulletRect(int y1, int y2) //обработчик события встречи
        {
            rect.Color = Brushes.Red;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
           
            g.FillEllipse(ellipse.Color, ellipse.X, ellipse.Y, ellipse.Size, ellipse.Size);
            g.FillEllipse(bullet.Color, bullet.X, bullet.Y, bullet.Size, bullet.Size); ;
            g.FillRectangle(rect.Color, rect.X, rect.Y, rect.Size, rect.Size);

            //генерация события
            ellipse.Meet(bullet.Y, rect.Y);

            //генерация события
            rect.ReachedСenter();

            Invalidate();
            Thread.Sleep(100);

        }


    }


    


    
}
