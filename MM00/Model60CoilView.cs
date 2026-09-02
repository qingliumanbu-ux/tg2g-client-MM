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
    public partial class Model60CoilView : UserControl
    {
        public string mat = "2030227066";
        public string status = "锁定";
        public string nh_path = "745 mm";
        public string wh_path = "1,650 mm";
        public string length = "1,650 mm";
        public string weight = "22330 kg";
        public bool isChosen = false;//是否被选中
        private Brush brush = new SolidBrush(Color.Black);
        private Brush brush_red = new SolidBrush(Color.Red);
        private Brush brush_yellow = new SolidBrush(Color.Yellow);
        private Brush brush_white = new SolidBrush(Color.White);
        public Model60CoilView()
        {
            InitializeComponent();
        }

        public void updateClick()
        {
            isChosen = !isChosen;
            Model60CoilView_SizeChanged(null, null);
        }

        private void Model60CoilView_SizeChanged(object sender, EventArgs e)
        {
            Bitmap bm = new Bitmap(346, 257);
            Graphics g = Graphics.FromImage(bm);
            if (isChosen) g.DrawImage(Properties.Resources._60卷选中1, 0, 0, 346, 257);
            else g.DrawImage(Properties.Resources._60卷1, 0, 0, 346, 257);
            float font_size = (float)0.06 * this.Width <= 0 ? 1 : (float)0.06 * this.Width;

            SizeF strSize = g.MeasureString(wh_path, new Font("宋体", font_size, FontStyle.Regular));
            double pointx = 50;
            double pointy = 55 - strSize.Height;
            g.DrawString(wh_path, new Font("宋体", font_size, FontStyle.Regular), brush, new PointF((int)pointx, (int)pointy));

            strSize = g.MeasureString(nh_path, new Font("宋体", font_size, FontStyle.Regular));
            pointx = 53;
            pointy = 96 - strSize.Height;
            g.DrawString(nh_path, new Font("宋体", font_size, FontStyle.Regular), brush, new PointF((int)pointx, (int)pointy));

            strSize = g.MeasureString(length, new Font("宋体", font_size, FontStyle.Regular));
            pointx = 225;
            pointy = 193 - strSize.Height;
            g.DrawString(length, new Font("宋体", font_size, FontStyle.Regular), brush, new PointF((int)pointx, (int)pointy));

            strSize = g.MeasureString(weight, new Font("宋体", font_size, FontStyle.Regular));
            pointx = 224;
            pointy = 245 - strSize.Height;
            g.DrawString(weight, new Font("宋体", font_size, FontStyle.Regular), brush, new PointF((int)pointx, (int)pointy));

            this.BackgroundImage = bm.Clone() as Bitmap;
            bm.Dispose();
            bm = null;
        }
    }
}
