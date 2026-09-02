using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MM
{
    public partial class ModelColorSlabView : UserControl
    {
        public string width = "1264.00 mm";
        public string length = "1264.00 mm";
        public string weight = "22330 kg";
        public string thickness = "3.082 mm";
        public string mat = "2030227066";
        public string status = "锁定";
        public bool isChosen = false;//是否被选中
        private Brush brush = new SolidBrush(Color.Black);
        private Brush brush_red = new SolidBrush(Color.Red);
        private Brush brush_yellow = new SolidBrush(Color.Yellow);
        private Brush brush_white = new SolidBrush(Color.White);
        public ModelColorSlabView()
        {
            InitializeComponent();
        }

        public void updateClick()
        {
            isChosen = !isChosen;
            ModelColorSlabView_SizeChanged(null, null);
        }

        private void ModelColorSlabView_SizeChanged(object sender, EventArgs e)
        {
            Bitmap bm = new Bitmap(346, 257);
            Graphics g = Graphics.FromImage(bm);
            if (isChosen) g.DrawImage(Properties.Resources.slab_red1, 0, 0, 346, 257);
            else g.DrawImage(Properties.Resources.slab_brown11, 0, 0, 346, 257);
            float font_size = (float)0.06 * this.Width <= 0 ? 1 : (float)0.06 * this.Width;

            SizeF strSize = g.MeasureString(thickness, new Font("宋体", font_size, FontStyle.Regular));
            double pointx = 30.00;
            double pointy = 121.00 - strSize.Height;
            g.DrawString(thickness, new Font("宋体", font_size, FontStyle.Regular), brush, new PointF((int)pointx, (int)pointy));

            strSize = g.MeasureString(width, new Font("宋体", font_size, FontStyle.Regular));
            pointx = 40;
            pointy = 166 - strSize.Height;
            g.DrawString(width, new Font("宋体", font_size, FontStyle.Regular), brush, new PointF((int)pointx, (int)pointy));

            strSize = g.MeasureString(length, new Font("宋体", font_size, FontStyle.Regular));
            pointx = 214.00;
            pointy = 172 - strSize.Height;
            g.DrawString(length, new Font("宋体", font_size, FontStyle.Regular), brush, new PointF((int)pointx, (int)pointy));

            strSize = g.MeasureString(weight, new Font("宋体", font_size, FontStyle.Regular));
            pointx = 226;
            pointy = 243 - strSize.Height;
            g.DrawString(weight, new Font("宋体", font_size, FontStyle.Regular), brush, new PointF((int)pointx, (int)pointy));

            this.BackgroundImage = bm.Clone() as Bitmap;
            bm.Dispose();
            bm = null;
        }
    }
}
