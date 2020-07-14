using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdelProekt
{
    delegate void Действие(); // обьявление делегата 
    public partial class Form1 : Form // создание обьектов в форме
    {

        Шлюпка[] Шлюпки = new Шлюпка[5]; // массив 5 шлюпки

        Пират[,] Пираты;
        public Form1()
        {
            InitializeComponent();

            Пираты = new Пират[Шлюпки.Length, 3]; // массив 3 пиратов

            for (int i = 0; i < Шлюпки.Length; i++)

            {

                Шлюпки[i] = new Шлюпка(new Point(this.Size.Width / 2, 80 * i + 50), new Point(0, 80 * i + 50), i + 2);//создаем объект шлюпки

                int j;

                for (j = 0; j < Пираты.GetLength(1); j++)

                {

                    Пираты[i, j] = new Пират(new Point(this.Size.Width / 2, 80 * i + 60), new Point(this.Size.Width + 10, (-1) * j * (-30) + (80 * i + 60)));//создаем обект пират

                    Шлюпки[i].Высадка += new Действие(Пираты[i, j].Оповещение);//подписка пиратов на окончание движения шлюпки вызывается метод оповещение

                }

                Пираты[i, Пираты.GetLength(1) - 1].КонецВысадки += new Действие(Шлюпки[i].ОповещеиеОВозврате);//подписка шлюпки на окончание движения пиратов для возврата кораблей

            }

            Paint += new PaintEventHandler(Drows);

            Timer timer = new Timer();

            timer.Interval = 40;

            timer.Tick += new EventHandler(Drow);

            timer.Start();

        }

        public void Drow(object s, EventArgs e) // перерисовка окна

        {

            Invalidate();

        }

        private void Drows(object sender, PaintEventArgs e)// перерисовка шлюпок и пиратов

        {

            for (int i = 0; i < Шлюпки.Length; i++)

            {

                Шлюпки[i].Перерисовать(sender, e);

                for (int j = 0; j < Пираты.GetLength(1); j++)

                {

                    Пираты[i, j].Перерисовать(sender, e);

                }

            }

        }

    }

    class Шлюпка

    {

        public event Действие Высадка;

        int x = 0;

        int y = 0;

        int Speed = 0;

        bool оповещениоОтЭкипажа = false;

        Point End_point;

        public Шлюпка(Point end_point, Point beginn_point, int speed) // конструктор для инициализации переменных

        {

            End_point = end_point;

            x = beginn_point.X;

            y = beginn_point.Y;

            Speed = speed;

        }

        public void Перерисовать(object o, PaintEventArgs e) //метод движения шлюпки

        {

            if (!оповещениоОтЭкипажа)

            {

                if (x < End_point.X)

                    x += 3 * Speed;

                if (x >= End_point.X)

                {

                    Высадка();

                }

            }

            else

            {

                x -= 3 * Speed;

            }

            e.Graphics.FillRectangle(new SolidBrush(Color.Red), new Rectangle(x, y, 100, 50));

        }

        public void ОповещеиеОВозврате()

        {

            оповещениоОтЭкипажа = true;

        }

    }

    class Пират

    {

        public event Действие КонецВысадки;

        Point пират_point;

        Point end_point;

        bool ОповещениеЭкипажа = false;

        public Пират(Point start_point, Point end_point) // конструктор инициализируем начальная и конечная точка пиратов

        {

            this.пират_point = start_point;

            this.end_point = end_point;

        }

        public void Перерисовать(object o, PaintEventArgs e) // метод перерисовки экипажа

        {

            if (пират_point.X >= end_point.X && КонецВысадки != null)

                КонецВысадки();

            else

            {

                if (ОповещениеЭкипажа && пират_point.X < end_point.X)

                {

                    ВысадкаПиратов(); // вызов метода

                    e.Graphics.FillEllipse(new SolidBrush(Color.Black), new Rectangle(пират_point.X, пират_point.Y, 20, 20));

                }

            }

        }

        public void Оповещение() // метод

        {

            ОповещениеЭкипажа = true;

        }

        private void ВысадкаПиратов() // метож движения пиратов

        {

            if (пират_point.X < end_point.X)

                пират_point.X += 3;

            if (пират_point.Y < end_point.Y) пират_point.Y += 1;

        }

    }
}