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
        public Form1()
        {
            InitializeComponent();

            var points = new List<PointF>() {
                new PointF(10, 10),
                new PointF(10, 80),
                new PointF(100, 40),
                new PointF(10, 10),
            };

            zoomControl1.Points = points;
            zoomControl1.Invalidate();
        }



    }
}
