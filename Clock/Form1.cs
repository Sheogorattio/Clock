using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Clock
{
    public partial class Form1 : Form
    {
        Matrix matrix;
        Point _origin;

        Pen pen = new Pen(Color.Black, 10);
        Bitmap bm;
        Graphics g;
        Timer timer;
        double[] radAngles = new double[60];
        PointF[] SecWatchPoints = new PointF[60];
        PointF[] HourWatchPoints = new PointF[60];

        int radius = 300;
        public Form1()
        {
            InitializeComponent();
            this.Height = 800;
            this.Width = 800;

            matrix = new Matrix();
            _origin = new Point(Height/2, Width/2);
            matrix.Translate(_origin.X, _origin.Y);
            matrix.Scale(1, -1);

            bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image = bm); 
            g.Transform = matrix;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;

            for(int i=0; i < 60; i++)
            {
                radAngles[i] =  Math.PI * (360+90-(i * 6)) / 180;
            }


            for (int i = 0; i < 60; i ++)
            {
                
                SecWatchPoints[i].X = (float)Math.Cos(radAngles[i]) * (radius-20);
                SecWatchPoints[i].Y = (float)Math.Sin(radAngles[i]) * (radius-20);
                HourWatchPoints[i].X = (float)Math.Cos(radAngles[i]) * (radius - 80);
                HourWatchPoints[i].Y = (float)Math.Sin(radAngles[i]) * (radius - 80);
            }

            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            g.Clear(BackColor);

            #region Printing base
            g.FillRectangle(Brushes.RosyBrown, -_origin.X, -_origin.Y, _origin.X * 2, _origin.Y * 2);
            g.FillEllipse(Brushes.LightGray, -radius, -radius, radius * 2, radius * 2);
            pen.Color = Color.Black;
            pen.Width = 5;
            g.DrawEllipse(pen, -radius, -radius, radius * 2, radius * 2);

            pen.Color = Color.LightSlateGray;
            pen.Width = 8;
            for(int i=0; i<SecWatchPoints.Length; i+=5)
            {
                g.DrawLine(pen, SecWatchPoints[i].X, SecWatchPoints[i].Y, (float)Math.Cos(radAngles[i]) * (radius - 60), (float)Math.Sin(radAngles[i]) * (radius - 60));
            }
            #endregion

            DateTime time = DateTime.Now;
            this.Text = $"{time.Hour:D2}:{time.Minute:D2}:{time.Second:D2}";

            #region Printing hour hand
            pen.Color = Color.Black;
            pen.Width = 8;
            g.DrawLine(pen, 0, 0, HourWatchPoints[time.Hour*5+time.Minute/12].X, HourWatchPoints[time.Hour*5+time.Minute / 12].Y);

            #endregion

            #region Printing minute hand
            pen.Color = Color.Black;
            pen.Width = 6;
            g.DrawLine(pen, 0, 0, SecWatchPoints[time.Minute].X, SecWatchPoints[time.Minute].Y);
            #endregion


            #region Printing second hand
            pen.Color = Color.Red; 
            pen.Width = 2;
            g.DrawLine(pen, 0,0, SecWatchPoints[time.Second].X, SecWatchPoints[time.Second].Y);
            #endregion

            g.FillEllipse(Brushes.LightSlateGray, -12, -12, 24, 24);

            pictureBox1.Refresh();
        }
    }
}
