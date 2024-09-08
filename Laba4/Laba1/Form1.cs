using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba1
{
    public partial class Form1 : Form
    {
        Graphics g;
        Ellipse ellipse;
        Bullet bullet1;
        Bullet bullet2;
        Rectangle rect ;

        public Form1()
        {
            InitializeComponent();
            g = CreateGraphics();

            timer1.Start();
            timer1.Tick += Form1_Paint;
            this.Size = new Size(800, 500);

            ellipse = new Ellipse(Brushes.Green, 550, 400, 50, this);

            rect = new Rectangle(Brushes.Yellow, 100, 80, 50, this);

            bullet1 = new Bullet(Brushes.Black, 550, 400, 10, this);
            bullet2 = new Bullet(Brushes.Black, 540, 400, 10, this);

            ellipse.StartCheck(rect);
            Start();
        }

       public async void Start()
       {
            await rect.MoveRectangle(ellipse, bullet1, bullet2);
       }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_Paint(object sender, EventArgs e)
        {
            g.Clear(Color.White);

            ellipse.Draw(g, ellipse.Color, ellipse.X, ellipse.Y, ellipse.Size);

            bullet1.Draw(g, bullet1.Color, bullet1.X, bullet1.Y, bullet1.Size);
            bullet2.Draw(g, bullet2.Color, bullet2.X, bullet2.Y, bullet2.Size);

            rect.Draw(g);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ellipse.cancelToken.Cancel();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
