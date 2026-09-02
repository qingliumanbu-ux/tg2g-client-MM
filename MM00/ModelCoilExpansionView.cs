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
    public partial class ModelCoilExpansionView : UserControl
    {
        public string with = "1264.00 mm";
        public string jc = "742.00 m";
        public string nh_path = "745 mm";
        public string wh_path = "1,650 mm";
        public string weight = "22330 kg";
        public string thickness = "3.082 mm";
        public string mat = "2030227066";
        public string status = "锁定";
        public bool isChosen = false;//是否被选中
        private Brush brush = new SolidBrush(Color.Black);
        private Brush brush_red = new SolidBrush(Color.Red);
        private Brush brush_yellow = new SolidBrush(Color.Yellow);
        private Brush brush_white = new SolidBrush(Color.White);
        public ModelCoilExpansionView()
        {
            InitializeComponent();
        }

        public void updateClick()
        {
            isChosen = !isChosen;
            ModelCoilExpansionView_Resize(null, null);
        }

        private void ModelCoilExpansionView_Resize(object sender, EventArgs e)
        {
            Bitmap bm = new Bitmap(346, 256);
            Graphics g = Graphics.FromImage(bm);
            if (isChosen) g.DrawImage(Properties.Resources.钢卷信息_03, 0, 0, 346, 256);
            else g.DrawImage(Properties.Resources.钢卷信息, 0, 0, 346, 256);
            float font_size = (float)0.06 * this.Width <= 0 ? 1 : (float)0.06 * this.Width;

            SizeF strSize = g.MeasureString(with, new Font("宋体", font_size, FontStyle.Regular));
            double pointx = 108;
            double pointy = 45 - strSize.Height;
            g.DrawString(with, new Font("宋体", font_size, FontStyle.Regular), brush, new PointF((int)pointx, (int)pointy));

            strSize = g.MeasureString(wh_path, new Font("宋体", font_size, FontStyle.Regular));
            pointx = 26;
            pointy = 58 - strSize.Height;
            g.DrawString(wh_path, new Font("宋体", font_size, FontStyle.Regular), brush, new PointF((int)pointx, (int)pointy));

            strSize = g.MeasureString(nh_path, new Font("宋体", font_size, FontStyle.Regular));
            pointx = 25;
            pointy = 106 - strSize.Height;
            g.DrawString(nh_path, new Font("宋体", font_size, FontStyle.Regular), brush, new PointF((int)pointx, (int)pointy));

            strSize = g.MeasureString(jc, new Font("宋体", font_size, FontStyle.Regular));
            pointx = 270;
            pointy = 84 - strSize.Height;
            g.DrawString(jc, new Font("宋体", font_size, FontStyle.Regular), brush, new PointF((int)pointx, (int)pointy));

            strSize = g.MeasureString(thickness, new Font("宋体", font_size, FontStyle.Regular));
            pointx = 269;
            pointy = 190 - strSize.Height;
            g.DrawString(thickness, new Font("宋体", font_size, FontStyle.Regular), brush, new PointF((int)pointx, (int)pointy));

            strSize = g.MeasureString(weight, new Font("宋体", font_size, FontStyle.Regular));
            pointx = 225;
            pointy = 245 - strSize.Height;
            g.DrawString(weight, new Font("宋体", font_size, FontStyle.Regular), brush, new PointF((int)pointx, (int)pointy));

            //strSize = g.MeasureString(mat, new Font("微软雅黑", font_size, FontStyle.Regular));
            //pointx = 220.00;
            //pointy = 400.00;
            //g.RotateTransform(-18);
            //g.DrawString(mat, new Font("微软雅黑", font_size, FontStyle.Regular), brush, new PointF((int)pointx, (int)pointy));

            //double mat_width = strSize.Width;
            //strSize = g.MeasureString(status, new Font("微软雅黑", font_size, FontStyle.Regular));
            //pointy = pointy + strSize.Height + 10;
            //pointx += (mat_width - strSize.Width) / 2;
            //g.DrawString(status, new Font("微软雅黑", font_size, FontStyle.Regular), brush_red, new PointF((int)pointx, (int)pointy));

            this.BackgroundImage = bm.Clone() as Bitmap;
            bm.Dispose();
            bm = null;
        }
    }
}
