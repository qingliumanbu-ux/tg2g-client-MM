using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MM
{
    public partial class FormMM0005A1S2N : EF.EFForm
    {
        public FormMM0005A1S2N()
        {
            InitializeComponent();
        }

        #region <自定义程序用全局变量>
        // 当前分区
        private string cs_formPartition = "";
        // 当前页数
        private int iPageIndex = 1;
        // 总页数
        private int iPageCount = 0;
        // 履历 当前页数
        private int iPageIndex_96 = 1;
        // 履历 总页数
        private int iPageCount_96 = 0;
        #endregion

        #region 绑定下拉框内容 BindDataSource()
        private void BindDataSource()
        {
            ////材料类型
            //string sqlstr = string.Format("SELECT CODE,CODE_DESC_1_CONTENT FROM TEP0002 WHERE CODE_CLASS = 'M002' AND CODE_DESC_4_CONTENT = '在用'");
            //EI.EIInfo outBlock1 = EF.Utility.ExecQueryPart(cs_formPartition,sqlstr);
            //if (EF.Utility.IsBlockHasError(outBlock1, this))
            //    return;
            //this.efDevLookUpEdit1.Properties.DisplayMember = "CODE";
            //this.efDevLookUpEdit1.Properties.ValueMember = "CODE";
            //this.efDevLookUpEdit1.Properties.Columns.Clear();
            //this.efDevLookUpEdit1.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "代码"));
            //this.efDevLookUpEdit1.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE_DESC_1_CONTENT", "代码描述"));
            //this.efDevLookUpEdit1.Properties.DataSource = outBlock1.Tables[0];
            //this.efDevLookUpEdit1.EditValue = outBlock1.Tables[0].Rows[0]["UNIT_CODE"].ToString();

            ////EI.EIInfo outBlock = EF.Utility.GetPartitionCodeClassValue(cs_formPartition,"M002");
            ////材料类型
            //this.efDevLookUpEdit1.Properties.DataSource = outBlock1.Tables[0];
            //this.efDevLookUpEdit1.Properties.DisplayMember = "CODE";
            //this.efDevLookUpEdit1.Properties.ValueMember = "CODE";
            //this.efDevLookUpEdit1.Properties.Columns.Clear();
            //this.efDevLookUpEdit1.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "代码"));
            //this.efDevLookUpEdit1.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE_DESC_1_CONTENT", "代码描述"));

        }
        #endregion

        #region 多记录查询 QueryMat()
        private void QueryMat(int nPageNo)
        {
             // 判断材料号不能为空
            if (this.efDevMatNo.Text.Trim() == "")
            {
                MessageBox.Show("材料号不能为空!");
                this.EFMsgInfo = "材料号不能为空!";
                //控件旁边的惊叹号。
                this.efDevMatNo.ErrorIcon = (new ErrorProvider()).Icon.ToBitmap(); //设置惊叹号
                this.efDevMatNo.ErrorText = this.EFMsgInfo;
                return;
            }
            
            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            inBlock.Tables[0].Columns.Add("MAT_NO");				//材料号
            inBlock.Tables[0].Columns.Add("RECORD_COUNT_PER_PAGE"); //每页记录数
            inBlock.Tables[0].Columns.Add("CURRENT_PAGE_NO");       //需查询的页号

            //使用一个新行前，必须先调用新增操作
            inBlock.Tables[0].Rows.Add();
            inBlock.Tables[0].Rows[0]["MAT_NO"] = this.efDevMatNo.Text.Trim();
            inBlock.Tables[0].Rows[0]["RECORD_COUNT_PER_PAGE"] = this.efDevGrid1.PageSize;
            inBlock.Tables[0].Rows[0]["CURRENT_PAGE_NO"] = nPageNo;


            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0005a1f2_inq", inBlock);

            //设置返回信息
            if (outBlock.GetSys().flag == 0)
            {
                //清空多记录的数据
                EFX.EFCGrid.GetEFCGridBase(efDevGrid1).ClearGridData();
                if (outBlock.blk_info[outBlock.blk_now].Row > 0)
                {
                    EF.Utility.SetCustomGridValue(this.efDevGrid1, outBlock, false);
                    this.efDevGrid1.TotalRecordCount = Int32.Parse(outBlock.GetColVal("PAGEINFO", 1, "TOTAL_RECORD").Trim());
                    iPageCount = this.efDevGrid1.TotalRecordCount / this.efDevGrid1.PageSize;
                    if (this.efDevGrid1.TotalRecordCount % this.efDevGrid1.PageSize > 0)
                    {
                        iPageCount++;
                    }
                    this.efDevGrid1.RecordCountMessage = String.Format(GC.GCRS.GCRSC0000056/*第 {0}/{1} 页，共 {2} 条记录*/, iPageIndex, iPageCount, this.efDevGrid1.TotalRecordCount);
                    //此属性设置efDevGrid中前面的行号,开始值(一般都是从0 开始,如果查询的是其他页 ,那应该是从 (当前页 * 每页大小+1) 开始
                    this.efDevGrid1.InitRowOrdinal = (iPageIndex - 1) * this.efDevGrid1.PageSize + 1;
                    if (iPageIndex == iPageCount)
                    {
                        /*设置跳至最后一页的按钮属性为false*/
                        this.efDevGrid1.LastPageButtonEnable = false;
                        this.efDevGrid1.NextPageButtonEnable = false;
                    }
                    else
                    {
                        this.efDevGrid1.LastPageButtonEnable = true;
                        this.efDevGrid1.NextPageButtonEnable = true;
                    }
                    /*当起始页为1的时候，则设置第一页和上一页的属性为false*/
                    if (iPageIndex == 1)
                    {
                        this.efDevGrid1.FirstPageButtonEnable = false;
                        this.efDevGrid1.PrePageButtonEnable = false;
                    }
                    else
                    {
                        this.efDevGrid1.FirstPageButtonEnable = true;
                        this.efDevGrid1.PrePageButtonEnable = true;
                    }
                    this.EFMsgInfo = GC.GCRS.GCRSC0000001;

                }
                else
                {
                    this.efDevGrid1.TotalRecordCount = 0;

                    //清空多记录的数据
                    EFX.EFCGrid.GetEFCGridBase(efDevGrid2).ClearGridData();
                    EFX.EFCGrid.GetEFCGridBase(efDevGrid3).ClearGridData();
                    EFX.EFCGrid.GetEFCGridBase(efDevGrid4).ClearGridData();
                    this.efDevGrid1.RecordCountMessage = String.Format(GC.GCRS.GCRSC0000056/*第 {0}/{1} 页，共 {2} 条记录*/, 1, 1, 0);
                }
            }
            else
            {
                this.EFSysInfo = outBlock.sys_info;
            }

            //恢复光标为箭头
            this.Cursor = System.Windows.Forms.Cursors.Default;
            //设置列字段自动调节宽度
            this.efDevGrid1.SetColumnsWidthAuto();
        }
        #endregion

        #region 查询材料明细信息 QueryMatInfo()
        private void QueryMatInfo()
        {
            if (this.gridView1.RowCount == 0)
                return;
            
            EI.EIInfo inBlock = new EI.EIInfo();
            inBlock.Tables[0].Columns.Add("MAT_TRACK_NO");

            //压值--给变量赋值
            inBlock.Tables[0].Rows.Add();
            inBlock.Tables[0].Rows[0]["MAT_TRACK_NO"] = this.gridView1.GetRowCellValue(this.gridView1.FocusedRowHandle, "MAT_TRACK_NO").ToString();

            EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0005a1a1_inq", inBlock);

            //设置返回信息
            if (outBlock.GetSys().flag == 0)
            {
                //将信息压入指定的GRID, 
                EF.Utility.SetCustomGridValue(efDevGrid2, outBlock, outBlock.Tables["YUANLIAO"].TableName, false);
                EF.Utility.SetCustomGridValue(efDevGrid3, outBlock, outBlock.Tables["CHENGPIN"].TableName, false);

            }
            else
            {
                this.EFSysInfo = outBlock.sys_info;
            }

            //设置列字段自动调节宽度
            this.efDevGrid2.SetColumnsWidthAuto();
            this.efDevGrid3.SetColumnsWidthAuto();
        }
        #endregion

        #region 查询材料履历信息 QueryMatLog()
        private void QueryMatLog(int nPageNo)
        {
            
            EI.EIInfo inBlock = new EI.EIInfo();

            inBlock.Tables[0].Columns.Add("MAT_NO");
            inBlock.Tables[0].Columns.Add("MAT_KIND");
            inBlock.Tables[0].Columns.Add("RECORD_COUNT_PER_PAGE"); //每页记录数
            inBlock.Tables[0].Columns.Add("CURRENT_PAGE_NO");       //需查询的页号

            //压值--给变量赋值
            inBlock.Tables[0].Rows.Add();
            inBlock.Tables[0].Rows[0]["MAT_NO"]   = this.gridView1.GetRowCellValue(this.gridView1.FocusedRowHandle, "MAT_NO").ToString();
            inBlock.Tables[0].Rows[0]["MAT_KIND"] = this.gridView1.GetRowCellValue(this.gridView1.FocusedRowHandle, "MAT_KIND").ToString();
            inBlock.Tables[0].Rows[0]["RECORD_COUNT_PER_PAGE"] = this.efDevGrid4.PageSize;
            inBlock.Tables[0].Rows[0]["CURRENT_PAGE_NO"] = nPageNo; 

            EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0005a1b1_inq", inBlock);

            //设置返回信息
            if (outBlock.GetSys().flag == 0)
            {
                //列宽自动调整。
                this.gridView4.BestFitColumns();
                //清空多记录的数据
                EFX.EFCGrid.GetEFCGridBase(efDevGrid4).ClearGridData();
                if (outBlock.blk_info[0].Row > 0)
                {
                    EF.Utility.SetCustomGridValue(this.efDevGrid4, outBlock, false);
                    this.efDevGrid4.TotalRecordCount = Int32.Parse(outBlock.GetColVal("PAGEINFO", 1, "TOTAL_RECORD").Trim());
                    iPageCount_96 = this.efDevGrid4.TotalRecordCount / this.efDevGrid4.PageSize;
                    if (this.efDevGrid4.TotalRecordCount % this.efDevGrid4.PageSize > 0)
                    {
                        iPageCount_96++;
                    }
                    this.efDevGrid4.RecordCountMessage = String.Format(GC.GCRS.GCRSC0000056/*第 {0}/{1} 页，共 {2} 条记录*/, iPageIndex_96, iPageCount_96, this.efDevGrid4.TotalRecordCount);
                    //此属性设置efDevGrid中前面的行号,开始值(一般都是从0 开始,如果查询的是其他页 ,那应该是从 (当前页 * 每页大小+1) 开始
                    this.efDevGrid4.InitRowOrdinal = (iPageIndex_96 - 1) * this.efDevGrid4.PageSize + 1;
                    if (iPageIndex_96 == iPageCount_96)
                    {
                        /*设置跳至最后一页的按钮属性为false*/
                        this.efDevGrid4.LastPageButtonEnable = false;
                        this.efDevGrid4.NextPageButtonEnable = false;
                    }
                    else
                    {
                        this.efDevGrid4.LastPageButtonEnable = true;
                        this.efDevGrid4.NextPageButtonEnable = true;
                    }
                    /*当起始页为1的时候，则设置第一页和上一页的属性为false*/
                    if (iPageIndex_96 == 1)
                    {
                        this.efDevGrid4.FirstPageButtonEnable = false;
                        this.efDevGrid4.PrePageButtonEnable = false;
                    }
                    else
                    {
                        this.efDevGrid4.FirstPageButtonEnable = true;
                        this.efDevGrid4.PrePageButtonEnable = true;
                    }
                    this.EFMsgInfo = GC.GCRS.GCRSC0000001;

                }
                else
                {
                    this.efDevGrid4.TotalRecordCount = 0;
                    this.efDevGrid4.RecordCountMessage = String.Format(GC.GCRS.GCRSC0000056/*第 {0}/{1} 页，共 {2} 条记录*/, 1, 1, 0);
                }
            }
            else
            {
                this.EFSysInfo = outBlock.sys_info;
            }

            //设置列字段自动调节宽度
            this.efDevGrid4.SetColumnsWidthAuto();
        }
        #endregion
        
        #region 画面加载事件 FormMM0005A1_Load
        private void FormMM0005A1_Load(object sender, EventArgs e)
        {
            //设置分区参数
            cs_formPartition = this.ef_args.formPartition;

            EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efDevGrid1, efDevGrid2, efDevGrid3, efDevGrid4 },
                new string[] { "MM0005A1_TMM0005", "MM0005A1_YUANLIAO", "MM0005A1_CHENGPIN", "MM0005A1_LVLI" },cs_formPartition);

            // 绑定下拉框内容
            BindDataSource();

            //设置grid列是否可编辑
            this.efDevGrid1.SetAllColumnReadOnlyWithoutSelection(true);
            this.efDevGrid2.SetAllColumnReadOnlyWithoutSelection(true);
            this.efDevGrid3.SetAllColumnReadOnlyWithoutSelection(true);
            this.efDevGrid4.SetAllColumnReadOnlyWithoutSelection(true);
        }
        #endregion
        
        #region F2 查询
        private void FormMM0005A1_EF_DO_F2(object sender, EF.EF_Args e)
        {
            try
            {
                iPageIndex    = 1;
                QueryMat(1);
                QueryMatInfo(); 
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region F6 转熔炼信息
        private void FormMM0005A1_EF_DO_F6(object sender, EF.EF_Args e)
        {
            EF.EF_Args.common_parameter_1 = "";
            EF.EF_Args.common_parameter_2 = "";

            if (this.gridView2.GetFocusedDataRow() != null)
            {
                EF.EF_Args.common_parameter_1
                    = this.gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "HEAT_NO").ToString();
                EF.EF_Args.common_parameter_2
                    = this.gridView2.GetRowCellValue(gridView2.FocusedRowHandle, "PONO").ToString();
            }

            this.EFCallForm("QMTQQ0");
        }
        #endregion

        #region F7 转实绩
        private void FormMM0005A1_EF_DO_F7(object sender, EF.EF_Args e)
        {
            EF.EF_Args.common_parameter_1 = "";
            EF.EF_Args.common_parameter_2 = "";

            if (this.gridView1.GetFocusedDataRow() != null)
            {
                EF.EF_Args.common_parameter_1
                    = this.gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MAT_NO").ToString();
                EF.EF_Args.common_parameter_2
                   = this.gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "UNIT_CODE").ToString();
            }

            this.EFCallForm("MMCR22A1");
        }
        #endregion

        #region 点击路径 gridView1_FocusedRowObjectChanged
        private void gridView1_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            if (this.gridView1.GetFocusedDataRow() != null)
            {
                iPageIndex_96 = 1;
                QueryMatLog(iPageIndex_96);
            }

        }
    
        #endregion

        #region  翻页事件
        //关于翻页的首页 尾页 向上 向下翻页功能
        private void efDevGrid1_EF_GridBar_Fisrt_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                iPageIndex = 1;
                QueryMat(iPageIndex);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }
        }

        private void efDevGrid1_EF_GridBar_Last_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                iPageIndex = iPageCount;
                QueryMat(iPageIndex);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }
        }

        private void efDevGrid1_EF_GridBar_NextPage_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                iPageIndex++;
                QueryMat(iPageIndex);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }
        }

        private void efDevGrid1_EF_GridBar_PageTo_Event(object sender, EF.EFDevGrid.EFNavigatorButtonClickEventArgs e)
        {
            try
            {
                this.efDevGrid1.PageSize = e.PageSize;
                iPageIndex = e.PageTo;
                QueryMat(iPageIndex);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }

        }

        private void efDevGrid1_EF_GridBar_PrePage_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                iPageIndex--;
                QueryMat(iPageIndex);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }
        }

        private void efDevGrid1_EF_GridBar_Refresh_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            QueryMat(iPageIndex);
        }
        #endregion

        #region  履历信息 翻页事件
        //关于翻页的首页 尾页 向上 向下翻页功能
        private void efDevGrid4_EF_GridBar_Fisrt_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                iPageIndex_96 = 1;
                QueryMatLog(iPageIndex_96);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }
        }

        private void efDevGrid4_EF_GridBar_Last_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try 
            {
                iPageIndex_96 = iPageCount_96;
                QueryMatLog(iPageIndex_96);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }
        }

        private void efDevGrid4_EF_GridBar_NextPage_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                iPageIndex_96++;
                QueryMatLog(iPageIndex_96);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }
        }

        private void efDevGrid4_EF_GridBar_PageTo_Event(object sender, EF.EFDevGrid.EFNavigatorButtonClickEventArgs e)
        {
            try
            {
                this.efDevGrid4.PageSize = e.PageSize;
                iPageIndex_96 = e.PageTo;
                QueryMatLog(iPageIndex_96);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }

        }

        private void efDevGrid4_EF_GridBar_PrePage_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                iPageIndex_96--;
                QueryMatLog(iPageIndex_96);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }
        }

        private void efDevGrid4_EF_GridBar_Refresh_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
                QueryMatLog(iPageIndex_96);
        }
        #endregion

        private void FormMM0005A1_EF_DO_F3(object sender, EF.EF_Args e)
        {           
             // 判断材料号不能为空
            if (this.efDevMatNo.Text.Trim() == "")
            {
                MessageBox.Show("材料号不能为空!");
                this.EFMsgInfo = "材料号不能为空!";
                //控件旁边的惊叹号。
                this.efDevMatNo.ErrorIcon = (new ErrorProvider()).Icon.ToBitmap(); //设置惊叹号
                this.efDevMatNo.ErrorText = this.EFMsgInfo;
                return;
            }
            
            EI.EIInfo inBlock = new EI.EIInfo();

            inBlock.Tables[0].Columns.Add("MAT_NO");

            //使用一个新行前，必须先调用新增操作
            inBlock.Tables[0].Rows.Add();

            inBlock.Tables[0].Rows[0]["MAT_NO"] = this.efDevMatNo.Text;

            //调用 SERVICE
            EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0005a1f3_pro", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            // 刷新画面
            this.FormMM0005A1_EF_DO_F2(null, null);
                
            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }


    }
}
