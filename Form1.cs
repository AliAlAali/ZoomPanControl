using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZoomPanControl
{
    public partial class Form1 : Form
    {
        IEnumerable<PointF> Points;
        public Form1()
        {
            InitializeComponent();

            Points = new List<PointF>() {
                new PointF(10, 10),
                new PointF(10, 80),
                new PointF(100, 40),
                new PointF(10, 10),
            };

            zoomControl1.OnDraw += ZoomControl1_OnDraw;
            zoomControl1.MinZoom = 0.1f;
            zoomControl1.MaxZoom = 20f;
            zoomControl1.ZoomSensitivity = 0.2f;
            zoomControl1.Invalidate();
        }

        private void ZoomControl1_OnDraw(object sender, ZoomControl.DrawEventArgs e)
        {
            var graphics = e.Graphics;
            var pen = new Pen(Color.Black);
            graphics.DrawLines(pen, Points.ToArray());
        }
    }
}
