using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using EPRP;

namespace MM
{
    public partial class FormMM0089A1S2N : EF.EFForm
    {
        public FormMM0089A1S2N()
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
        #endregion

        #region 多记录查询 Query()
        private void Query(int nPageNo)
        {

            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables.Clear();
            inBlock.Tables.Add(EF.Utility.GetSingleGridValue(efDevGrid_Query).Tables[0].Copy());

            inBlock.Tables[0].Columns.Add("RECORD_COUNT_PER_PAGE"); //每页记录数
            inBlock.Tables[0].Columns.Add("CURRENT_PAGE_NO");       //需查询的页号

            inBlock.Tables[0].Rows[0]["RECORD_COUNT_PER_PAGE"] = this.efDevGrid1.PageSize;
            inBlock.Tables[0].Rows[0]["CURRENT_PAGE_NO"] = nPageNo;

            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0089a1f2_inq", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            //设置返回信息
            if (outBlock.GetSys().flag == 0)
            {
                //清空多记录的数据

                //EFX.EFCGrid.GetEFCGridBase(efDevGrid_Mat).ClearGridData();

                ////清空单记录的数据               
                //EFX.EFCGrid.GetEFCGridBase(efDevGrid_Detail).ClearGridData();

                if (outBlock.blk_info[outBlock.blk_now].Row >= 0)
                {
                    EF.Utility.SetCustomGridValue(this.efDevGrid1, outBlock, false , true);
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
                }
                else
                {
                    this.efDevGrid1.TotalRecordCount = 0;
                    this.efDevGrid1.RecordCountMessage = String.Format(GC.GCRS.GCRSC0000056/*第 {0}/{1} 页，共 {2} 条记录*/, 1, 1, 0);
                }
                this.EFMsgInfo = string.Format(GC.GCRS.GCRSC0000033/*查询到[{0}]条记录。*/, efDevGrid1.TotalRecordCount);
            }

            //恢复光标为箭头
            this.Cursor = System.Windows.Forms.Cursors.Default;
            //设置列字段自动调节宽度
            this.efDevGrid1.SetColumnsWidthAuto();
        }
        #endregion

        #region 打印报表 Print()
        private void Print()
        {

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Columns.Add("IN_MAT_NO");				//入口材料号
            inBlock.Tables[0].Columns.Add("SAMPLE_LOT_NO");			//试批号

            //查询未打印的数据
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0089a1a1_inq", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            if (outBlock.GetSys().flag == 0)
            {
                // 打印标签
                for (int i = 0; i < outBlock.Tables[0].Rows.Count; i++)
                {
                    EP.IPrintReport myViewRpt = new EP.GeneralPrintReport();
                    Hashtable paramList = new Hashtable();
                    paramList.Add("plan_no", outBlock.Tables[0].Rows[i]["PLAN_NO"].ToString());
                    paramList.Add("V_PLAN_NO", outBlock.Tables[0].Rows[i]["PLAN_NO"].ToString());
                    paramList.Add("V_PLAN_BACKLOG_CODE", outBlock.Tables[0].Rows[i]["UNIT_CODE"].ToString());
                    paramList.Add("V_PLAN_STATUS_START", "");
                    paramList.Add("V_PLAN_STATUS_END", "");
                    paramList.Add("V_ENAME", "");
                    paramList.Add("V_FUNCTION_NUM", "0");

                    myViewRpt.PrintReportAfterView("EPRH0ULF7R5", paramList, false);

                    //使用一个新行前，必须先调用新增操作
                    inBlock.Tables[0].Rows.Add();

                    inBlock.Tables[0].Rows[i]["IN_MAT_NO"] = outBlock.Tables[0].Rows[i]["IN_MAT_NO"].ToString();
                    inBlock.Tables[0].Rows[i]["SAMPLE_LOT_NO"] = outBlock.Tables[0].Rows[i]["SAMPLE_LOT_NO"].ToString();

                }

                //将已打印的数据，设置为已打印
                if (inBlock.Tables[0].Rows.Count > 0)
                {
                    EI.EIInfo outBlock2 = EI.EIManager.Instance.CallService(cs_formPartition, "mm0089a1b1_pro", inBlock);
                }
            }

        }
        #endregion
        
        #region 画面载入 FormMM0089A1_Load
        private void FormMM0089A1_Load(object sender, EventArgs e)
        {            
            //设置分区参数
            cs_formPartition = this.ef_args.formPartition;
            
            //设置Grid的ED54配置列
            EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efDevGrid1 }, new string[] { "MM0089A1_INQ" },cs_formPartition);

            //设置Grid的ED54配置列 单记录模式
            EFX.EFCGrid.InitSingleGridColumn(efDevGrid_Query, "MM0089A1_QUERY", cs_formPartition);            
            
            //EPED54中初始化缺省信息。
            //(EFX.EFCGrid.GetEFCGridBase(efDevGrid1) as EFX.EFCGridImp.SingleDev.EFCGridSingleDev).ResetGridValue();

            ////设置默认值
            //if (EFX.EFCGrid.GetEFCGridBase(efDevGrid_Query).Columns.ContainsKey("PROD_TIME_FROM") == true)
            //{
            //    EFX.EFCGrid.GetEFCGridBase(efDevGrid_Query).SetGridCellValue(0, "PROD_TIME_FROM", System.DateTime.Now.AddDays(-7));
            //}
            //if (EFX.EFCGrid.GetEFCGridBase(efDevGrid_Query).Columns.ContainsKey("PROD_TIME_TO") == true)
            //{
            //    EFX.EFCGrid.GetEFCGridBase(efDevGrid_Query).SetGridCellValue(0, "PROD_TIME_TO", System.DateTime.Now);
            //}

            //EPED54配置模式的查询条件的GROUP的尺寸大小控制
            //GC.PM_utility2.DEV_Init_LayoutGroup_where_size(layoutControlGroup_Query, this.efDevGrid_Query);

            //设置FORM的属性
            GC.PM_utility2.DEV_Init_Form2(this, this.layoutControl1, this.layoutControlGroup1);

            /*材料信息配置可调整*/
            efDevGrid1.CanConfigGridCaption = false;
            efDevGrid1.IsUseCustomPageBar = true;
            efDevGrid1.ShowSaveLayoutButton = true;
            efDevGrid1.LoadLayout();

        }
        #endregion

        #region F2 查询
        private void FormMM0089A1_EF_DO_F2(object sender, EF.EF_Args e)
        {
            try
            {
                iPageIndex = 1;
                Query(1);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }

        }
        #endregion

        #region F3 打印标签 确定
        private void FormMM0089A1_EF_DO_F3(object sender, EF.EF_Args e)
        {
            //判断是否有选中行
            if (this.efDevGrid1.GetSelectedDataRow().Rows.Count <= 0)
            {
                this.EFMsgInfo = "请选择需操作的记录。";
                MessageBox.Show("请选择需操作的记录。");
                this.ef_args.buttonStatusHold = true;
                return;
            }

            // 打印标签
            for (int i = 0; i < this.efDevGrid1.GetSelectedDataRow().Rows.Count; i++)
            {
                try
                {
                    EP.IPrintReport myViewRpt = new EP.GeneralPrintReport();
                    Hashtable paramList = new Hashtable();
                    paramList.Add("plan_no", efDevGrid1.GetSelectedDataRow().Rows[i]["PLAN_NO"].ToString());
                    paramList.Add("V_PLAN_NO", efDevGrid1.GetSelectedDataRow().Rows[i]["PLAN_NO"].ToString());
                    paramList.Add("V_PLAN_BACKLOG_CODE", efDevGrid1.GetSelectedDataRow().Rows[i]["UNIT_CODE"].ToString());
                    paramList.Add("V_PLAN_STATUS_START", "");
                    paramList.Add("V_PLAN_STATUS_END", "");
                    paramList.Add("V_ENAME", "");
                    paramList.Add("V_FUNCTION_NUM", "0");

                    myViewRpt.PrintReportAfterView("EPRH0ULF7R5", paramList, false);
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }

            // 设置传入参数 
            EI.EIInfo inBlock = new EI.EIInfo();
            inBlock.Tables[0].Merge(this.efDevGrid1.GetSelectedDataRow());
                
            //调用 SERVICE
            EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0089a1f3_pro", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                
            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }



            // 刷新画面
            this.FormMM0089A1_EF_DO_F2(null, null);

            EFMsgInfo = "操作成功";
            this.ef_args.buttonStatusHold = false;  //按钮恢复

        }
        #endregion

        #region F6 标签维护 确定
        private void FormMM0089A1_EF_DO_F6(object sender, EF.EF_Args e)
        {
            // 存放原来选定的行的行号 
            int i_focusedRow = Math.Max(this.gridView1.FocusedRowHandle, 0);

            // 维护事件数据 
            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            DataTable dataTable_1 = this.efDevGrid1.DataSource as DataTable;

            DataTable delTable_1 = null;
            DataTable updTable_1 = dataTable_1.Clone();
            DataTable insTable_1 = dataTable_1.Clone();

            for (int i = 0; i < gridView1.RowCount; i++)
            {
                if (efDevGrid1.GetSelectedColumnChecked(i))
                {
                    if (this.gridView1.GetDataRow(i).RowState == DataRowState.Added)
                    {
                        insTable_1.Rows.Add(this.gridView1.GetDataRow(i).ItemArray);
                    }
                    else if (this.gridView1.GetDataRow(i).RowState == DataRowState.Modified)
                    {
                        updTable_1.Rows.Add(this.gridView1.GetDataRow(i).ItemArray);
                    }
                }
            }

            delTable_1 = dataTable_1.GetChanges(DataRowState.Deleted);

            if (insTable_1 != null && insTable_1.Rows.Count > 0)
            {
                insTable_1.TableName = "MM0089A1_INS";
                inBlock.Tables.Add(insTable_1);
            }
            if (delTable_1 != null && delTable_1.Rows.Count > 0)
            {
                delTable_1.RejectChanges();
                delTable_1.TableName = "MM0089A1_DEL";
                inBlock.Tables.Add(delTable_1);
            }
            if (updTable_1 != null && updTable_1.Rows.Count > 0)
            {
                updTable_1.TableName = "MM0089A1_UPD";
                inBlock.Tables.Add(updTable_1);
            }

            //判断是否有增删改的数据
            if (inBlock.Tables.IndexOf("MM0089A1_INS") < 0
             && inBlock.Tables.IndexOf("MM0089A1_DEL") < 0
             && inBlock.Tables.IndexOf("MM0089A1_UPD") < 0)
            {
                this.EFMsgInfo = "没有增删改的记录。";
                this.ef_args.buttonStatusHold = true;
                return;
            }

            //调用新增SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0089a1f6_pro", inBlock);
            this.EFSysInfo = outBlock.sys_info;

            //判断调用是否正确
            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            // 设置grid 增删按钮属性
            this.efDevGrid1.ShowAddCopyRowButton = false;
            this.efDevGrid1.ShowAddRowButton = false;
            this.efDevGrid1.ShowDeleteRowButton = false;

            //刷新页面 查询参数信息
            Query(1);

            // 取消系统默认选定的当前行
            gridView1.UnselectRow(this.gridView1.FocusedRowHandle);

            // 选定行在原来的位置
            this.gridView1.FocusedRowHandle = Math.Min(i_focusedRow, this.gridView1.RowCount);

            // 当前行被选中
            efDevGrid1.SetSelectedColumnChecked(this.gridView1.FocusedRowHandle, true);


            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }
        #endregion

        #region F6 标签维护 准备
        private void FormMM0089A1_EF_PRE_DO_F6(object sender, EF.EF_Args e)
        {
            // 设置grid 增删按钮属性
            this.efDevGrid1.ShowAddCopyRowButton = true;
            this.efDevGrid1.ShowAddRowButton = true;
            this.efDevGrid1.ShowDeleteRowButton = true;

        }
        #endregion

        #region F6 标签维护 取消
        private void FormMM0089A1_EF_CANCEL_DO_F6(object sender, EF.EF_Args e)
        {
            // 设置grid 增删按钮属性
            this.efDevGrid1.ShowAddCopyRowButton = false;
            this.efDevGrid1.ShowAddRowButton = false;
            this.efDevGrid1.ShowDeleteRowButton = false;


            //刷新页面 查询参数信息
            Query(1);

        }
        #endregion

        #region  翻页事件
        //关于翻页的首页 尾页 向上 向下翻页功能
        private void efDevGrid1_EF_GridBar_Fisrt_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                iPageIndex = 1;
                Query(iPageIndex);
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
                Query(iPageIndex);
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
                Query(iPageIndex);
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
                Query(iPageIndex);
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
                Query(iPageIndex);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }
        }

        private void efDevGrid1_EF_GridBar_Refresh_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            Query(iPageIndex);
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            Print();
        }

        private void FormMM0089A1_EF_DO_F7(object sender, EF.EF_Args e)
        {
            Print();

        }
    }
}
