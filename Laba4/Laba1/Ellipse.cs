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
    public delegate void CheckBullet(int x);        //создание делегата
    public class Ellipse
    {
        public CancellationTokenSource cancelToken = new CancellationTokenSource();

        public event CheckBullet SendBullet;     // создал событие отправить пулю


        public int Size;
        public Brush Color;

        private int x;
        private int y;

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

        public Ellipse(Brush color, int x, int y, int size, Form1 form)
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



        public async void StartCheck(Rectangle r)
        {
            window.Text = await ReachedСenter(r.X, r.Y);

        }

        public void Podpiska(Ellipse e, Bullet b1, Bullet b2, int x)
        {
            if (x == 400)
            {
                e.SendBullet += new CheckBullet(b1.MoveBullet);
                e.SendBullet += new CheckBullet(b2.MoveBullet);
            }
        }

        public Task<string> ReachedСenter(int x, int y)
        {
            return Task.Run(() =>
            {
                while (true)
                {
                    if (SendBullet != null)
                    {
                        window.Invoke((Action)delegate
                        {
                            window.Text = "Запущена пуля";
                        });
                        try
                        {
                            ParallelOptions parOpts = new ParallelOptions();

                            parOpts.CancellationToken = cancelToken.Token;

                            Delegate[] delList = SendBullet.GetInvocationList();

                            //window.Text = "Запущена пуля";

                            //Parallel.ForEach(delList, parOpts, del =>
                            //{
                            //    parOpts.CancellationToken.ThrowIfCancellationRequested();

                            //    CheckBullet deleg = (CheckBullet)del; // Текущий делегат
                            //    deleg.Invoke(y); // Выполнить
                            //});

                            Parallel.Invoke(
                            () =>
                            {
                                CheckBullet deleg = (CheckBullet)delList[0]; // Текущий делегат
                                deleg.Invoke(y); // Выполнить
                            },
                            () =>
                            {
                                CheckBullet deleg = (CheckBullet)delList[1]; // Текущий делегат
                                deleg.Invoke(y); // Выполнить
                            });

                        }
                        catch (OperationCanceledException ex)
                        {
                            window.Invoke((Action)delegate
                            {
                                window.Text = ex.Message;
                            });
                        }

                        break;

                    }

                }

                return "Success";
            });
        }

    }
}
