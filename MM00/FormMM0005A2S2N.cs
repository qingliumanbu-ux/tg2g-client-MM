using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraLayout;

namespace MM
{
    public partial class FormMM0005A2S2N : EF.EFForm
    {
        private const int matWidth = 250;
        private const int matHeight = 220;
        private const int arrowoWidth = 40;
        private const int arrowoHeight = 50;
        private int directFlag;

        private string selectResumeSeqNo;

        private List<Control> listLabel;
        private List<LayoutControlItem> listLayoutCtrlItemMatRoot;
        private List<LayoutControlItem> listLayoutCtrlItemMat;
        private List<LayoutControlItem> listLayoutCtrlItemOther;
        private List<EmptySpaceItem> listEmptySpaceRow;
        private List<EmptySpaceItem> listEmptySpaceCol;

        private EI.EIInfo outBlock;
        private DataTable dtMatTrace;
        private Control last_click_ctrl = null;
        private string mat_kind = "";
        private string mat_shape_flag = "";

        public FormMM0005A2S2N()
        {
            InitializeComponent();

            this.listLabel = new List<Control>();
            this.listLayoutCtrlItemMatRoot = new List<LayoutControlItem>();
            this.listLayoutCtrlItemMat = new List<LayoutControlItem>();
            this.listLayoutCtrlItemOther = new List<LayoutControlItem>();
            this.listEmptySpaceRow = new List<EmptySpaceItem>();
            this.listEmptySpaceCol = new List<EmptySpaceItem>();

            this.selectResumeSeqNo = string.Empty;

            this.directFlag = 0;
            this.layoutControlGroup5.Width = 500;

            this.outBlock = new EI.EIInfo();
            this.dtMatTrace = new DataTable();
        }

        private void ClearCtrl()
        {       
            layoutControlGroupMatTree.Clear();

            for (int i = listLabel.Count - 1; i >= 0; i--)
            {
                if (listLabel[i] != null)
                {
                    listLabel[i].Dispose();
                    listLabel.RemoveAt(i);
                }
            }

            for (int i = listLayoutCtrlItemMatRoot.Count - 1; i >= 0; i--)
            {
                if (listLayoutCtrlItemMatRoot[i] != null)
                {
                    listLayoutCtrlItemMatRoot[i].Dispose();
                    listLayoutCtrlItemMatRoot.RemoveAt(i);
                }
            }

            for (int i = listLayoutCtrlItemMat.Count - 1; i >= 0; i--)
            {
                if (listLayoutCtrlItemMat[i] != null)
                {
                    listLayoutCtrlItemMat[i].Dispose();
                    listLayoutCtrlItemMat.RemoveAt(i);
                }
            }

            for (int i = listLayoutCtrlItemOther.Count - 1; i >= 0; i--)
            {
                if (listLayoutCtrlItemOther[i] != null)
                {
                    listLayoutCtrlItemOther[i].Dispose();
                    listLayoutCtrlItemOther.RemoveAt(i);
                }
            }

            for (int i = listEmptySpaceRow.Count - 1; i >= 0; i--)
            {
                if (listEmptySpaceRow[i] != null)
                {
                    listEmptySpaceRow[i].Dispose();
                    listEmptySpaceRow.RemoveAt(i);
                }
            }

            for (int i = listEmptySpaceCol.Count - 1; i >= 0; i--)
            {
                if (listEmptySpaceCol[i] != null)
                {
                    listEmptySpaceCol[i].Dispose();
                    listEmptySpaceCol.RemoveAt(i);
                }
            }  
        }

        private void AddMat(string matId, string matNo, string matKind, string matShapeFlag, int rowNum, decimal MAT_THICK, decimal MAT_WIDTH, int MAT_LEN, decimal MAT_OUTER_DIA, decimal MAT_INNER_DIA, decimal MAT_WT, int rootMatFlag = 0)
        {
            Control childCtrl = null;
            if (matKind == "SM")
            {
                //Label labelMat = new Label();
                //labelMat.Image = global::MM.Properties.Resources.slab_brown;
                ModelColorSlabView slabView = new ModelColorSlabView();
                slabView.mat = matNo;
                slabView.length = string.Format("{0:N0}", MAT_LEN) + " mm";
                slabView.thickness = string.Format("{0:N3}", MAT_THICK) + " mm";
                slabView.weight = string.Format("{0:N3}", MAT_WT) + " t";
                childCtrl = slabView;
            }
            else if (matKind == "HR")
            {
                if (matShapeFlag == "2")
                {
                    //Label labelMat = new Label();
                    //labelMat.BackColor = System.Drawing.Color.Transparent;
                    //labelMat.Image = global::MM.Properties.Resources.plates_silvery;
                    ModelPlatesView platesView = new ModelPlatesView();
                    platesView.mat = matNo;
                    platesView.length = string.Format("{0:N0}", MAT_LEN) + " mm";
                    platesView.thickness = string.Format("{0:N3}", MAT_THICK) + " mm";
                    platesView.weight = string.Format("{0:N3}", MAT_WT) + " t";
                    childCtrl = platesView;
                }
                else
                {
                    //Label labelMat = new Label();
                    //labelMat.Image = global::MM.Properties.Resources._60卷;
                    Model60CoilView coil = new Model60CoilView();
                    coil.length = string.Format("{0:N0}", MAT_LEN) + " mm";
                    coil.mat = matNo;
                    coil.nh_path = string.Format("{0:N0}", MAT_INNER_DIA) + " mm";
                    coil.wh_path = string.Format("{0:N0}", MAT_OUTER_DIA) + " mm";
                    coil.weight = string.Format("{0:N3}", MAT_WT) + " t";
                    childCtrl = coil;
                }
            }
            else if (matKind == "CR")
            {
                if (matShapeFlag == "2")
                {
                    //Label labelMat = new Label();
                    //labelMat.BackColor = System.Drawing.Color.Transparent;
                    //labelMat.Image = global::MM.Properties.Resources.plates_silvery;
                    ModelPlatesView platesView = new ModelPlatesView();
                    platesView.mat = matNo;
                    platesView.length = string.Format("{0:N0}", MAT_LEN) + " mm";
                    platesView.thickness = string.Format("{0:N3}", MAT_THICK) + " mm";
                    platesView.weight = string.Format("{0:N3}", MAT_WT) + " t";
                    childCtrl = platesView;
                }
                else
                {
                    
                    //labelMat.Image = global::MM.Properties.Resources.钢卷展开;
                    ModelCoilExpansionView coilMat = new ModelCoilExpansionView();
                    coilMat.BackColor = System.Drawing.Color.Transparent;
                    coilMat.mat = matNo;

                    coilMat.weight = string.Format("{0:N3}", MAT_WT) + " t";
                    coilMat.with = string.Format("{0:N0}", MAT_WIDTH) + " mm";
                    coilMat.jc = "0 mm";
                    coilMat.nh_path = string.Format("{0:N0}", MAT_INNER_DIA) + " mm";
                    coilMat.wh_path = string.Format("{0:N0}", MAT_OUTER_DIA) + " mm";
                    coilMat.thickness = string.Format("{0:N3}", MAT_THICK) + " mm";
                    childCtrl = coilMat;
                }
            }
            else if (matKind == "HP")
            {
                //Label labelMat = new Label();
                //labelMat.Image = global::MM.Properties.Resources.plate_silver;
                ModelPlateView plateView = new ModelPlateView();
                plateView.mat = matNo;
                plateView.length = string.Format("{0:N0}", MAT_LEN) + " mm";
                plateView.thickness = string.Format("{0:N3}", MAT_THICK) + " mm";
                plateView.weight = string.Format("{0:N3}", MAT_WT) + " t";
                childCtrl = plateView;
            }
            else
            {
                return;
            }

            //labelMat.Tag = matId;
            //labelMat.Click += new System.EventHandler(this.labelMat_Click);
            //listLabel.Add(labelMat);
            childCtrl.Tag = matId;
            childCtrl.Click += new System.EventHandler(this.labelMat_Click);
            listLabel.Add(childCtrl);

            LayoutControlItem layoutCtrlMat = new LayoutControlItem();
            layoutCtrlMat.Control = childCtrl;
            layoutCtrlMat.ControlAlignment = ContentAlignment.MiddleLeft;
            layoutCtrlMat.MaxSize = new System.Drawing.Size(matWidth, matHeight);
            layoutCtrlMat.MinSize = new System.Drawing.Size(matWidth, matHeight);
            layoutCtrlMat.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 10, 10);
            layoutCtrlMat.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            layoutCtrlMat.TextVisible = false;
            layoutCtrlMat.Tag = matNo;

            layoutControlGroupMatTree.AddItem(layoutCtrlMat);

            if (this.directFlag == 0)
            {
                layoutCtrlMat.Move(listEmptySpaceRow[rowNum], DevExpress.XtraLayout.Utils.InsertType.Top);
            }
            else if (this.directFlag == 1)
            {
                layoutCtrlMat.Move(listEmptySpaceRow[rowNum], DevExpress.XtraLayout.Utils.InsertType.Left);
            }

            if (rootMatFlag == 1)
            {
                listLayoutCtrlItemMatRoot.Add(layoutCtrlMat);
            }
            else
            {
                listLayoutCtrlItemMat.Add(layoutCtrlMat);
            }
        }

        private void AddArrow()
        {
            foreach (LayoutControlItem layoutCtrl in this.listLayoutCtrlItemMat)
            {
                Label labelArrow = new Label();
                labelArrow.BackColor = System.Drawing.Color.Transparent;

                if (this.directFlag == 0)
                {
                    labelArrow.Image = global::MM.Properties.Resources.down;
                }
                else
                {
                    labelArrow.Image = global::MM.Properties.Resources.right;
                }
                
                listLabel.Add(labelArrow);

                LayoutControlItem layoutCtrlArrow = new LayoutControlItem();
                layoutCtrlArrow.Control = labelArrow;
                layoutCtrlArrow.ControlAlignment = ContentAlignment.MiddleLeft;
                
                layoutCtrlArrow.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 10, 10);
                layoutCtrlArrow.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
                layoutCtrlArrow.TextVisible = false;
                listLayoutCtrlItemOther.Add(layoutCtrlArrow);

                layoutControlGroupMatTree.AddItem(layoutCtrlArrow);

                if (this.directFlag == 0)
                {
                    layoutCtrlArrow.Move(layoutCtrl, DevExpress.XtraLayout.Utils.InsertType.Top);
                    layoutCtrlArrow.MaxSize = new System.Drawing.Size(matWidth, arrowoHeight);
                    layoutCtrlArrow.MinSize = new System.Drawing.Size(matWidth, arrowoHeight);
                }
                else if (this.directFlag == 1)
                {
                    layoutCtrlArrow.Move(layoutCtrl, DevExpress.XtraLayout.Utils.InsertType.Left);
                    layoutCtrlArrow.MaxSize = new System.Drawing.Size(arrowoWidth, matHeight);
                    layoutCtrlArrow.MinSize = new System.Drawing.Size(arrowoWidth, matHeight);
                }
            }
        }

        private void AddMatNo()
        {
            foreach (LayoutControlItem layoutCtrl in this.listLayoutCtrlItemMatRoot)
            {
                Label labelMatNo = new Label();
                labelMatNo.BackColor = System.Drawing.Color.Transparent;
                labelMatNo.Text = layoutCtrl.Tag.ToString();
                labelMatNo.TextAlign = ContentAlignment.MiddleCenter;
                listLabel.Add(labelMatNo);

                LayoutControlItem layoutCtrlMatNo = new LayoutControlItem();
                layoutCtrlMatNo.Control = labelMatNo;
                layoutCtrlMatNo.ControlAlignment = ContentAlignment.MiddleLeft;
                layoutCtrlMatNo.MaxSize = new System.Drawing.Size(matWidth, 30);
                layoutCtrlMatNo.MinSize = new System.Drawing.Size(matWidth, 30);
                layoutCtrlMatNo.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 10, 10);
                layoutCtrlMatNo.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
                layoutCtrlMatNo.TextVisible = false;
                listLayoutCtrlItemOther.Add(layoutCtrlMatNo);

                layoutControlGroupMatTree.AddItem(layoutCtrlMatNo);
                layoutCtrlMatNo.Move(layoutCtrl, DevExpress.XtraLayout.Utils.InsertType.Bottom);
            }

            foreach (LayoutControlItem layoutCtrl in this.listLayoutCtrlItemMat)
            {
                Label labelMatNo = new Label();
                labelMatNo.BackColor = System.Drawing.Color.Transparent;
                labelMatNo.Text = layoutCtrl.Tag.ToString();
                labelMatNo.TextAlign = ContentAlignment.MiddleCenter;
                listLabel.Add(labelMatNo);

                LayoutControlItem layoutCtrlMatNo = new LayoutControlItem();
                layoutCtrlMatNo.Control = labelMatNo;
                layoutCtrlMatNo.ControlAlignment = ContentAlignment.MiddleLeft;
                layoutCtrlMatNo.MaxSize = new System.Drawing.Size(matWidth, 30);
                layoutCtrlMatNo.MinSize = new System.Drawing.Size(matWidth, 30);
                layoutCtrlMatNo.Padding = new DevExpress.XtraLayout.Utils.Padding(5, 5, 10, 10);
                layoutCtrlMatNo.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
                layoutCtrlMatNo.TextVisible = false;
                listLayoutCtrlItemOther.Add(layoutCtrlMatNo);

                layoutControlGroupMatTree.AddItem(layoutCtrlMatNo);
                layoutCtrlMatNo.Move(layoutCtrl, DevExpress.XtraLayout.Utils.InsertType.Bottom);
            }
        }

        private void QueryMatTree(int queryType)
        {
            EI.EIInfo inBlock = new EI.EIInfo();
            
            int minSeqNo = -1;
            int maxSeqNo = 0;
            int maxCutNum = 0;
            int rowNum = 0;

            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            inBlock.Tables[0].Columns.Add("MAT_NO");
            inBlock.Tables[0].Columns.Add("RECORD_COUNT_PER_PAGE");
            inBlock.Tables[0].Columns.Add("CURRENT_PAGE_NO");

            //使用一个新行前，必须先调用新增操作
            inBlock.Tables[0].Rows.Add();
            inBlock.Tables[0].Rows[0]["MAT_NO"] = this.efDevMatNo.Text.Trim();
            inBlock.Tables[0].Rows[0]["RECORD_COUNT_PER_PAGE"] = 1000;
            inBlock.Tables[0].Rows[0]["CURRENT_PAGE_NO"] = 1;

            outBlock.Clear();
            dtMatTrace.Clear();

            //调用SERVICE
            outBlock = EI.EITuxedo.CallService("mm0005a1f2_inq", inBlock);         
            if (outBlock.Tables[0].Rows.Count > 0)
            {
                dtMatTrace = outBlock.Tables[0].Copy();
                dtMatTrace.Columns.Remove("PASS_BACKLOG_SEQ_NO");
                dtMatTrace.Columns.Add("PASS_BACKLOG_SEQ_NO", typeof(decimal));
                for (int i = 0; i < dtMatTrace.Rows.Count; i++)
                {
                    dtMatTrace.Rows[i]["PASS_BACKLOG_SEQ_NO"] = outBlock.Tables[0].Rows[i]["PASS_BACKLOG_SEQ_NO"];
                }

                EF.Utility.SetCustomGridValue(this.efDevGrid1, outBlock, false);
                this.efDevGrid1.SetColumnsWidthAuto();

                var query = from t in dtMatTrace.AsEnumerable()
                            group t by t.Field<decimal>("PASS_BACKLOG_SEQ_NO") into g
                            orderby g.Key
                            select new
                            {
                                pass_backlog_seq_no = g.Key,
                                cut_num = g.Count()
                            };

                foreach (var item in query)
                {
                    if (minSeqNo == -1)
                    {
                        minSeqNo = Convert.ToInt32(item.pass_backlog_seq_no);
                    }
                    
                    if (item.pass_backlog_seq_no > maxSeqNo)
                    {
                        maxSeqNo = Convert.ToInt32(item.pass_backlog_seq_no);
                    }

                    if (item.cut_num > maxCutNum)
                    {
                        maxCutNum = item.cut_num;
                    }
                }

                if (queryType == 0)
                {
                    this.layoutControlGroupMatTree.DefaultLayoutType = DevExpress.XtraLayout.Utils.LayoutType.Horizontal;
                }
                else if (queryType == 1)
                {
                    this.layoutControlGroupMatTree.DefaultLayoutType = DevExpress.XtraLayout.Utils.LayoutType.Vertical;
                }

                for (int i = 0; i < maxCutNum; i++)
                {
                    EmptySpaceItem emptySpaceItem = new EmptySpaceItem();
                    layoutControlGroupMatTree.AddItem(emptySpaceItem);
                    listEmptySpaceRow.Add(emptySpaceItem);
                }

                for (int i = minSeqNo; i <= maxSeqNo; i++)
                {
                    rowNum = 0;
                    List<int> listAddMatRowNo = new List<int>();
                    var queryMat = from t in dtMatTrace.AsEnumerable()
                                   where t.Field<decimal>("PASS_BACKLOG_SEQ_NO") == i
                                    select new
                                    {
                                        mat_id = t.Field<string>("MAT_ID"),
                                        mat_no = t.Field<string>("MAT_NO"),
                                        mat_kind = t.Field<string>("MAT_KIND"),
                                        mat_shape_flag = t.Field<string>("MAT_SHAPE_FLAG"),
                                        MAT_ACT_INNER_DIA = t.Field<decimal>("MAT_ACT_INNER_DIA"),
                                        MAT_ACT_OUTER_DIA = t.Field<decimal>("MAT_ACT_OUTER_DIA"),
                                        MAT_THICK = t.Field<decimal>("MAT_THICK"),
                                        MAT_WIDTH = t.Field<decimal>("MAT_WIDTH"),
                                        MAT_LEN = t.Field<int>("MAT_LEN"),
                                        MAT_WT = t.Field<decimal>("MAT_WT")
                                    };

                    foreach (var item in queryMat)
                    {
                        if (i == minSeqNo)
                        {
                            AddMat(item.mat_id, item.mat_no, item.mat_kind, item.mat_shape_flag, rowNum, item.MAT_THICK, item.MAT_WIDTH, item.MAT_LEN, item.MAT_ACT_OUTER_DIA, item.MAT_ACT_INNER_DIA, item.MAT_WT, 1);
                        }
                        else
                        {
                            AddMat(item.mat_id, item.mat_no, item.mat_kind, item.mat_shape_flag, rowNum, item.MAT_THICK, item.MAT_WIDTH, item.MAT_LEN, item.MAT_ACT_OUTER_DIA, item.MAT_ACT_INNER_DIA, item.MAT_WT);
                        }

                        listAddMatRowNo.Add(rowNum);
                        rowNum++;
                    }

                    for (int j = 0; j < maxCutNum; j++)
                    {
                        int existsMat = 0;
                        foreach (int rowNo in listAddMatRowNo)
                        {
                            if (rowNo == j)
                            {
                                existsMat = 1;
                                break;
                            }
                        }

                        if (existsMat == 0)
                        {
                            EmptySpaceItem emptySpaceItem = new EmptySpaceItem();
                            emptySpaceItem.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;

                            emptySpaceItem.MaxSize = new System.Drawing.Size(matWidth, matHeight);
                            emptySpaceItem.MinSize = new System.Drawing.Size(matWidth, matHeight);

                            layoutControlGroupMatTree.AddItem(emptySpaceItem);
                            listEmptySpaceCol.Add(emptySpaceItem);

                            if (queryType == 0)
                            {
                                emptySpaceItem.Move(listEmptySpaceRow[j], DevExpress.XtraLayout.Utils.InsertType.Top);
                            }
                            else if (queryType == 1)
                            {
                                emptySpaceItem.Move(listEmptySpaceRow[j], DevExpress.XtraLayout.Utils.InsertType.Left);
                            }
                        }
                    }
                }

                AddArrow();
                AddMatNo();
                
            }
        }

        private void RefreshMap()
        {
            layoutControlGroupMatTree.BeginUpdate();
            ClearCtrl();

            //判断材料号不能为空
            if (this.efDevMatNo.Text.Trim() != "")            
            {
                QueryMatTree(this.directFlag);
            }

            layoutControlGroupMatTree.EndUpdate();
        }

        private void FormMM0005A2_Load(object sender, EventArgs e)
        {
            ClearCtrl();
            EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efDevGrid1}, new string[] { "MM0005A1_TMM0005"});

            if (this.efDevMatNo.Text.Trim() != "")
            {
                layoutControlGroupMatTree.BeginUpdate();
                QueryMatTree(0);
                layoutControlGroupMatTree.EndUpdate();
            }
        }

        private void FormMM0005A2_EF_DO_F2(object sender, EF.EF_Args e)
        {
            RefreshMap();
        }

        private void FormMM0005A2_EF_DO_F3(object sender, EF.EF_Args e)
        {
            splitterItem1.Move(layoutControlGroup4, DevExpress.XtraLayout.Utils.InsertType.Right);
            layoutControlGroupGrid.Move(splitterItem1, DevExpress.XtraLayout.Utils.InsertType.Right);

            layoutControlGroupMatTree.BeginUpdate();
            ClearCtrl();
            layoutControlGroupMatTree.EndUpdate();
        }

        private void labelMat_Click(object sender, EventArgs e)
        {
            var queryMat = from t in dtMatTrace.AsEnumerable()
                           where t.Field<string>("MAT_ID") == (sender as Control).Tag.ToString()
                           select new
                           {
                               mat_kind = t.Field<string>("MAT_KIND"),
                               mat_shape_flag = t.Field<string>("MAT_SHAPE_FLAG"),
                               mat_thick = t.Field<decimal>("MAT_THICK"),
                               mat_width = t.Field<decimal>("MAT_WIDTH"),
                               mat_len = t.Field<int>("MAT_LEN"),
                               mat_wt = t.Field<decimal>("MAT_WT"),
                               unit_cname = t.Field<string>("UNIT_CNAME"),
                               resume_seq_no = t.Field<string>("RESUME_SEQ_NO")
                           };
            if (this.last_click_ctrl != null)
            {
                if (mat_kind == "SM")
                {
                    (last_click_ctrl as ModelColorSlabView).updateClick();
                }
                else if (mat_kind == "HR")
                {
                    if (mat_shape_flag == "2")
                    {
                        (last_click_ctrl as ModelPlatesView).updateClick();
                    }
                    else
                    {
                        (last_click_ctrl as Model60CoilView).updateClick();
                    }

                }
                else if (mat_kind == "CR")
                {
                    if (mat_shape_flag == "2")
                    {
                        (last_click_ctrl as ModelPlatesView).updateClick();
                    }
                    else
                    {
                        (last_click_ctrl as ModelCoilExpansionView).updateClick();
                    }
                }
                else if (mat_kind == "HP")
                {
                    (last_click_ctrl as ModelPlateView).updateClick();
                }

            }

            this.last_click_ctrl = (sender as Control);
             foreach (var item in queryMat)
             {
                 this.selectResumeSeqNo = item.resume_seq_no;
                 EF.Utility.SetCustomGridValue(this.efDevGrid1, outBlock, false);

                 this.efDevSpinEdit1.Value = item.mat_thick;
                 this.efDevSpinEdit2.Value = item.mat_width;
                 this.efDevSpinEdit3.Value = item.mat_len;
                 this.efDevSpinEdit4.Value = item.mat_wt;
                 this.efDevTextEdit1.Text = item.unit_cname;

                 if (item.mat_kind == "SM")
                 {
                     (sender as ModelColorSlabView).updateClick();
                     mat_kind = item.mat_kind;
                     mat_shape_flag = item.mat_shape_flag;
                 }
                 else if (item.mat_kind == "HR")
                 {
                     if (item.mat_shape_flag == "2")
                     {
                         (sender as ModelPlatesView).updateClick();
                         mat_kind = item.mat_kind;
                         mat_shape_flag = item.mat_shape_flag;
                     }
                     else
                     {
                         (sender as Model60CoilView).updateClick();
                         mat_kind = item.mat_kind;
                         mat_shape_flag = item.mat_shape_flag;
                     }

                 }
                 else if (item.mat_kind == "CR")
                 {
                     if (item.mat_shape_flag == "2")
                     {
                         (sender as ModelPlatesView).updateClick();
                         mat_kind = item.mat_kind;
                         mat_shape_flag = item.mat_shape_flag;
                     }
                     else
                     {
                         (sender as ModelCoilExpansionView).updateClick();
                         mat_kind = item.mat_kind;
                         mat_shape_flag = item.mat_shape_flag;
                     }
                 }
                 else if (item.mat_kind == "HP")
                 {
                     (sender as ModelPlateView).updateClick();
                     mat_kind = item.mat_kind;
                     mat_shape_flag = item.mat_shape_flag;
                 }

                 //foreach (Label label in this.listLabel)
                 //{
                 //    if (label.Tag != null && label.Tag.ToString() != (sender as Label).Tag.ToString())
                 //    {
                 //        var queryMatOther = from t in dtMatTrace.AsEnumerable()
                 //                            where t.Field<string>("MAT_ID") == label.Tag.ToString()
                 //                            select new
                 //                            {
                 //                                mat_kind = t.Field<string>("MAT_KIND"),
                 //                                mat_shape_flag = t.Field<string>("MAT_SHAPE_FLAG")
                 //                            };

                 //        foreach (var itemOther in queryMatOther)
                 //        {
                 //            if (itemOther.mat_shape_flag == "2" && itemOther.mat_kind != "HP")
                 //            {
                 //                label.Image = global::MM.Properties.Resources.plates_silvery;
                 //            }
                 //            else if (itemOther.mat_kind == "SM")
                 //            {
                 //                label.Image = global::MM.Properties.Resources.slab_brown;
                 //            }
                 //            else if (itemOther.mat_kind == "HR")
                 //            {
                 //               label.Image = global::MM.Properties.Resources._60卷;
                 //            }
                 //            else if (itemOther.mat_kind == "CR")
                 //            {
                 //               label.Image = global::MM.Properties.Resources.钢卷展开;
                 //            }
                 //            else if (itemOther.mat_kind == "HP")
                 //            {
                 //                label.Image = global::MM.Properties.Resources.plate_silvery;
                 //            }
                 //        }
                 //    }
                 //}
             }
        }

        private void efButton1_Click(object sender, EventArgs e)
        {
            if (directFlag == 0)
            {
                directFlag = 1;
                this.splitterItem1.Move(this.layoutControlGroup5, DevExpress.XtraLayout.Utils.InsertType.Bottom);
                this.layoutControlGroupGrid.Move(this.splitterItem1, DevExpress.XtraLayout.Utils.InsertType.Bottom);
                this.layoutControlGroup5.Height = 300;
            }
            else
            {
                directFlag = 0;
                this.splitterItem1.Move(this.layoutControlGroup5, DevExpress.XtraLayout.Utils.InsertType.Right);
                this.layoutControlGroupGrid.Move(this.splitterItem1, DevExpress.XtraLayout.Utils.InsertType.Right);
                this.layoutControlGroup5.Width = 500;
            }

            RefreshMap();

            //恢复光标为箭头
            this.Cursor = System.Windows.Forms.Cursors.Default;
        }

        private void gridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (this.selectResumeSeqNo.Trim() != "" && this.gridView1.RowCount > 0 &&
                this.gridView1.GetRowCellValue(e.RowHandle, "RESUME_SEQ_NO") != null &&
                this.gridView1.GetRowCellValue(e.RowHandle, "RESUME_SEQ_NO").ToString() == this.selectResumeSeqNo)
            {
                e.Appearance.BackColor = Color.YellowGreen;
            }
        }
    }
}
