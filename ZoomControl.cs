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
    /// <summary>
    ///  User control which allows zoom (ctrl + mouse wheel) & pan (shift + mouse left button)
    ///  double click mouse middle button to reset to original view.
    /// </summary>
    public partial class ZoomControl : UserControl
    {

        public float ZoomSensitivity { get; set; } = 0.2f; /// Controls how much zoom per zoom mouse scroll
        public float MaxZoom { get; set; } = 20;
        public float MinZoom { get; set; } = 0.1f;

        public event EventHandler<DrawEventArgs> OnDraw; /// Called on everytime control is painted and after zoom calculations. Can be used to draw custom paints.

        public double ZoomScale { get; set; } = 1; /// Current Zoom value
        private PointF Origin = new PointF(0, 0); /// Origin is the left most point of a viewport
        private PointF LastMousePosition = new PointF(-1, -1);
        private MouseWheelEvent MouseWheelData { get; set; }
        private MouseMoveEvent MouseMoveData { get; set; }
        private Matrix Transformation { get; set; } /// Keeps track of zoom transformations

        public ZoomControl() : base()
        {
            InitializeComponent();

            this.MouseWheel += ZoomControl_MouseWheel;
            this.MouseDown += ZoomControl_MouseDown; ;
            this.MouseMove += ZoomControl_MouseMove;
            this.MouseDoubleClick += ZoomControl_MouseDoubleClick;
            this.Paint += new PaintEventHandler(ZoomControl_Paint);

            Transformation = new Matrix();

            // Utilize double buffer to smooth out flickering when re-painting
            this.SetStyle(
                System.Windows.Forms.ControlStyles.UserPaint |
                System.Windows.Forms.ControlStyles.AllPaintingInWmPaint |
                System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer,
                true);
        }

        private void ZoomControl_MouseDown(object sender, MouseEventArgs e)
        {
            var mouseX = e.Location.X;
            var mouseY = e.Location.Y;
            LastMousePosition = new PointF(mouseX, mouseY);
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

                var newZoom = zoom * ZoomScale;
                if (MinZoom <= newZoom && newZoom <= MaxZoom)
                {
                    g.TranslateTransform(Origin.X, Origin.Y);
                    Origin.X -= (float)(mouseX / (ZoomScale * zoom) - mouseX / ZoomScale);
                    Origin.Y -= (float)(mouseY / (ZoomScale * zoom) - mouseY / ZoomScale);

                    g.ScaleTransform(zoom, zoom);
                    g.TranslateTransform(-Origin.X, -Origin.Y);

                    ZoomScale *= zoom;
                }

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

            OnDraw?.Invoke(this, new DrawEventArgs()
            {
                Graphics = g,
                Zoom = ZoomScale,
                ViewPort = Origin
            });

        }


        /// <summary>
        /// Handles mouse double clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ZoomControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle && e.Clicks > 1)
            {
                Reset();
                return;
            }
        }

        /// <summary>
        /// Handle mouse wheel spin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Resets all transformations and zoom for this control
        /// </summary>
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

        public class DrawEventArgs : EventArgs
        {
            public Graphics Graphics { get; set; }
            public double Zoom { get; set; }
            public PointF ViewPort { get; set; }
        }
    }
}
