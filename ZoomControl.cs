using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZoomPanControl
{
    public partial class ZoomControl : UserControl
    {

        public float ZoomSensitivity { get; set; } = 0.2f;
        public float MaxZoom { get; set; } = 20;
        public float MinZoom { get; set; } = 0.1f;
        public double ZoomScale { get; set; } = 1;
        public IEnumerable<PointF> Points;



        private PointF Origin = new PointF(0, 0);
        private PointF LastMousePosition = new PointF(-1, -1);
        private MouseWheelEvent MouseWheelData { get; set; }
        private MouseMoveEvent MouseMoveData { get; set; }
        private Matrix Transformation { get; set; }

        public ZoomControl() : base()
        {
            InitializeComponent();

            this.MouseWheel += ZoomControl_MouseWheel;
            this.MouseDown += ZoomControl_MouseDown; ;
            this.MouseMove += ZoomControl_MouseMove;
            this.MouseDoubleClick += ZoomControl_MouseDoubleClick;
            this.Paint += new PaintEventHandler(ZoomControl_Paint);

            Transformation = new Matrix();
        }

        private void ZoomControl_MouseDown(object sender, MouseEventArgs e)
        {
            var mouseX = e.Location.X;
            var mouseY = e.Location.Y;
            LastMousePosition = new PointF(mouseX, mouseY);

            this.SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true);
        }

        private void ZoomControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && (ModifierKeys & Keys.Shift) != 0)
            {

                if (LastMousePosition.X == -1 && LastMousePosition.Y == -1)
                {
                    LastMousePosition = new PointF(e.Location.X, e.Location.Y);
                }
                else if (e.Location.X == LastMousePosition.X && e.Location.Y == LastMousePosition.Y)
                {
                    return;
                }

                var mouseX = e.Location.X;
                var mouseY = e.Location.Y;

                this.MouseMoveData = new MouseMoveEvent()
                {
                    Position = new Point(mouseX, mouseY),
                };


                Invalidate();
            }
        }

        private void ZoomControl_Paint(object sender, PaintEventArgs e)
        {
            // Get control size
            var width = this.Width;
            var height = this.Height;
            var g = e.Graphics;
            g.Transform = Transformation;

            // Handle mouse wheel events before drawing
            if (MouseWheelData != null)
            {
                var mouseX = MouseWheelData.Position.X;
                var mouseY = MouseWheelData.Position.Y;
                var zoom = (float)MouseWheelData.Zoom;

                g.TranslateTransform(Origin.X, Origin.Y);
                Origin.X -= (float)(mouseX / (ZoomScale * zoom) - mouseX / ZoomScale);
                Origin.Y -= (float)(mouseY / (ZoomScale * zoom) - mouseY / ZoomScale);

                g.ScaleTransform(zoom, zoom);
                g.TranslateTransform(-Origin.X, -Origin.Y);

                ZoomScale *= zoom;

                // Nullify to prevent unecessary calculations
                MouseWheelData = null;
            }
            else if (MouseMoveData != null)
            {
                var mouseX = MouseMoveData.Position.X;
                var mouseY = MouseMoveData.Position.Y;
                var dx = (float)((mouseX - LastMousePosition.X) / ZoomScale);
                var dy = (float)((mouseY - LastMousePosition.Y) / ZoomScale);
                g.TranslateTransform(dx, dy);
                Origin.X -= dx;
                Origin.Y -= dy;

                LastMousePosition = MouseMoveData.Position;
                MouseMoveData = null;
            }

            // Keep transofrmation record
            Transformation = g.Transform;

            // Paint whatever you need here
            var brush = new SolidBrush(Color.LightGray);
            var pen = new Pen(Color.Black);



            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.FillRectangle(brush, Origin.X, Origin.Y, (float)(width / ZoomScale), (float)(height / ZoomScale));
            //g.DrawRectangle(pen, new Rectangle(50, 50, 100, 100));
            if (Points != null && Points.Count() > 0)
            {
                g.DrawLines(pen, Points.ToArray());
            }

        }

        private void ZoomControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Middle && e.Clicks > 1)
            {
                Reset();
                return;
            }
        }


        private void ZoomControl_MouseWheel(object sender, MouseEventArgs e)
        {
            var mouseX = e.Location.X;
            var mouseY = e.Location.Y;

            var wheel = e.Delta < 0 ? -1 : 1;
            var zoom = Math.Exp(wheel * ZoomSensitivity);

            this.MouseWheelData = new MouseWheelEvent()
            {
                Position = new Point(mouseX, mouseY),
                Zoom = zoom,
            };

            Invalidate();
        }

        public void Reset()
        {
            Origin = new PointF(0, 0);
            ZoomScale = 1;
            Transformation = new Matrix();
            Invalidate();
        }


        private class MouseData
        {
            public Point Position { get; set; }
        }

        private class MouseMoveEvent : MouseData
        {

        }

        private class MouseWheelEvent : MouseData
        {
            public double Zoom { get; set; }

        }
    }
}
