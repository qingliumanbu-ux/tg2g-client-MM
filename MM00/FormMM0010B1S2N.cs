using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;//
using System.Text.RegularExpressions;//

namespace MM
{
    public partial class FormMM0010B1S2N : EF.EFForm
    {
        public FormMM0010B1S2N()
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

        #region 绑定下拉框内容 BindDataSource()
        private void BindDataSource()
        {
            //计划状态
            EI.EIInfo outBlock = EF.Utility.GetPartitionCodeClassValue(cs_formPartition, "M00K");
            Common.Utility.SetLookUpEditProperty(pLAN_STATUSEFDevLookUpEdit, outBlock.Tables["M00K"], true, true);
           
        }
        #endregion

        #region 多记录查询计划信息 QueryPlan()
        private void QueryPlan(int nPageNo)
        {
            
            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Columns.Add("RAW_ORIGIN");         //原料来源
            inBlock.Tables[0].Columns.Add("DEMAND_PLAN_NO");     //进料需求计划号-计划号
            inBlock.Tables[0].Columns.Add("PLAN_MAKER");         //计划责任者-计划员
            inBlock.Tables[0].Columns.Add("PLAN_STATUS");        //计划状态

            inBlock.Tables[0].Columns.Add("RECORD_COUNT_PER_PAGE");
            inBlock.Tables[0].Columns.Add("CURRENT_PAGE_NO");

            //使用一个新行前，必须先调用新增操作
            inBlock.Tables[0].Rows.Add();

            inBlock.Tables[0].Rows[0]["RAW_ORIGIN"]     = this.rAW_ORIGINEFDevRadioGroup.EditValue;
            inBlock.Tables[0].Rows[0]["DEMAND_PLAN_NO"] = this.pLAN_NOEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["PLAN_MAKER"]     = this.pLAN_MAKEREFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["PLAN_STATUS"]    = this.pLAN_STATUSEFDevLookUpEdit.EditValue;

            inBlock.Tables[0].Rows[0]["RECORD_COUNT_PER_PAGE"] = this.efDevGrid_Plan.PageSize;
            inBlock.Tables[0].Rows[0]["CURRENT_PAGE_NO"] = nPageNo;

            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0010b1f2_inq", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            //设置返回信息
            if (outBlock.GetSys().flag == 0)
            {
                //清空多记录的数据
                EFX.EFCGrid.GetEFCGridBase(efDevGrid_Plan).ClearGridData();

                if (outBlock.blk_info[outBlock.blk_now].Row > 0)
                {
                    EF.Utility.SetCustomGridValue(this.efDevGrid_Plan, outBlock, false);
                    this.efDevGrid_Plan.TotalRecordCount = Int32.Parse(outBlock.GetColVal("PAGEINFO", 1, "TOTAL_RECORD").Trim());
                    iPageCount = this.efDevGrid_Plan.TotalRecordCount / this.efDevGrid_Plan.PageSize;
                    if (this.efDevGrid_Plan.TotalRecordCount % this.efDevGrid_Plan.PageSize > 0)
                    {
                        iPageCount++;
                    }
                    this.efDevGrid_Plan.RecordCountMessage = String.Format(GC.GCRS.GCRSC0000056/*第 {0}/{1} 页，共 {2} 条记录*/, iPageIndex, iPageCount, this.efDevGrid_Plan.TotalRecordCount);
                    //此属性设置efDevGrid中前面的行号,开始值(一般都是从0 开始,如果查询的是其他页 ,那应该是从 (当前页 * 每页大小+1) 开始
                    this.efDevGrid_Plan.InitRowOrdinal = (iPageIndex - 1) * this.efDevGrid_Plan.PageSize + 1;
                    if (iPageIndex == iPageCount)
                    {
                        /*设置跳至最后一页的按钮属性为false*/
                        this.efDevGrid_Plan.LastPageButtonEnable = false;
                        this.efDevGrid_Plan.NextPageButtonEnable = false;
                    }
                    else
                    {
                        this.efDevGrid_Plan.LastPageButtonEnable = true;
                        this.efDevGrid_Plan.NextPageButtonEnable = true;
                    }
                    /*当起始页为1的时候，则设置第一页和上一页的属性为false*/
                    if (iPageIndex == 1)
                    {
                        this.efDevGrid_Plan.FirstPageButtonEnable = false;
                        this.efDevGrid_Plan.PrePageButtonEnable = false;
                    }
                    else
                    {
                        this.efDevGrid_Plan.FirstPageButtonEnable = true;
                        this.efDevGrid_Plan.PrePageButtonEnable = true;
                    }
                }
                else
                {
                    this.efDevGrid_Plan.TotalRecordCount = 0;
                    this.efDevGrid_Plan.RecordCountMessage = String.Format(GC.GCRS.GCRSC0000056/*第 {0}/{1} 页，共 {2} 条记录*/, 1, 1, 0);
                }
                this.EFMsgInfo = string.Format(GC.GCRS.GCRSC0000033/*查询到[{0}]条记录。*/, efDevGrid_Plan.TotalRecordCount);
            }

            //恢复光标为箭头
            this.Cursor = System.Windows.Forms.Cursors.Default;

            //设置列字段自动调节宽度
            this.efDevGrid_Plan.SetColumnsWidthAuto();
        }
        #endregion

        #region 查询材料信息 QueryMatInfo()
        private void QueryMatInfo()
        {//单记录查询模式

            //焦点行不为空但是行号小于0,比如对材料号进行分组时,焦点行但是行号为负数
            if (gridView_Plan.FocusedRowHandle < 0)
                return;

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Columns.Add("DEMAND_PLAN_NO");    //进料需求计划号

            //使用一个新行前，必须先调用新增操作
            inBlock.Tables[0].Rows.Add();
            inBlock.Tables[0].Rows[0]["DEMAND_PLAN_NO"] = this.gridView_Plan.GetRowCellValue(gridView_Plan.FocusedRowHandle, "DEMAND_PLAN_NO").ToString();

            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0010b1a1_inq", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            if (outBlock.sys_info.flag < 0)
            {
                return;
            }

            //清空单记录的数据
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_Mat).ClearGridData();

            //将信息压入指定的GRID
            EFX.EFCGrid.GetEFCGridBase(this.efDevGrid_Mat).SetGridValue(outBlock);

        }
        #endregion

        #region 画面被调用 FormMM0010B1_EF_START_FORM_BY_EF
        private void FormMM0010B1_EF_START_FORM_BY_EF(object sender, EF.EF_Args i_args)
        {
            this.pLAN_NOEFDevTextEdit.Text = EF.EF_Args.common_parameter_1;

            EF.EF_Args.common_parameter_1 = "";
 
        }
        #endregion

        #region 画面载入 FormMM0010B1_Load
        private void FormMM0010B1_Load(object sender, EventArgs e)
        {
            //设置分区参数
            cs_formPartition = this.ef_args.formPartition;
           
            //根据ED54配置,显示EFDevGrid列标题              
            EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efDevGrid_Plan,efDevGrid_Mat }, 
                new string[] { "MM0010B1_INQ_PLAN","MM0010B1_INQ_MAT" }, cs_formPartition);

            // 绑定下拉框内容
            this.BindDataSource();

            //设置EFDevGrid除选择列外，其他列都不可编辑
            efDevGrid_Plan.SetAllColumnEditableWithoutSelection(false);

            // 画面被调用，计划号不为空，查询计划
            if (this.pLAN_NOEFDevTextEdit.Text != null && this.pLAN_NOEFDevTextEdit.Text.ToString().Trim() != "")
            {
                // 刷新画面
                this.FormMM0010B1_EF_DO_F2(null, null);
            }
        }
        #endregion

        #region F2 查询
        private void FormMM0010B1_EF_DO_F2(object sender, EF.EF_Args e)
        {
            try
            {
                iPageIndex = 1;
                QueryPlan(1);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region F4 计划删除 准备
        private void FormMM0010B1_EF_PRE_DO_F4(object sender, EF.EF_Args e)
        {
            this.EFMsgInfo = "请选择计划信息的计划进行操作!";
        }
        #endregion

        #region F4 计划删除 确定
        private void FormMM0010B1_EF_DO_F4(object sender, EF.EF_Args e)
        {
             // 判断是否有选中行
            if (this.efDevGrid_Plan.GetSelectedDataRow().Rows.Count <= 0)
            {
                this.EFMsgInfo = "请选择需操作的计划。";
                MessageBox.Show("请选择需操作的计划。");
                this.ef_args.buttonStatusHold = true;
                return;

            }

            // 警告 是否删除
            if (EF.EFMessageBox.Show(this, GC.GCRS.GCRSC0000036/*选中的记录将被永久删除， 是否继续？*/
            , GC.GCRS.GCRSC0000025/*警告*/, MessageBoxButtons.YesNo, MessageBoxIcon.Warning
            , MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            // 设置传入参数 
            EI.EIInfo inBlock = new EI.EIInfo();
            inBlock.Tables[0].Merge(this.efDevGrid_Plan.GetSelectedDataRow());

            //调用 SERVICE
            EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0010b1f4_del", inBlock);          
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            // 刷新画面
            this.FormMM0010B1_EF_DO_F2(null, null);

            EFMsgInfo = GC.GCRS.GCRSC0000003/*删除成功。*/;
            this.ef_args.buttonStatusHold = false;  //按钮恢复

        }
        #endregion

        #region F4 计划删除 取消
        private void FormMM0010B1_EF_CANCEL_DO_F4(object sender, EF.EF_Args e)
        {
            this.EFMsgInfo = "计划删除取消!"; ;
        }
        #endregion   

        #region F5 材料删除 准备
        private void FormMM0010B1_EF_PRE_DO_F5(object sender, EF.EF_Args e)
        {
            this.EFMsgInfo = "请选择材料信息的材料进行操作!";
        }
        #endregion

        #region F5 材料删除 确定
        private void FormMM0010B1_EF_DO_F5(object sender, EF.EF_Args e)
        {
             // 判断是否有选中行
            if (this.efDevGrid_Mat.GetSelectedDataRow().Rows.Count <= 0)
            {
                this.EFMsgInfo = "请选择需操作的材料。";
                MessageBox.Show("请选择需操作的材料。");
                this.ef_args.buttonStatusHold = true;
                return;

            }

            // 警告 是否删除
            if (EF.EFMessageBox.Show(this, GC.GCRS.GCRSC0000036/*选中的记录将被永久删除， 是否继续？*/
            , GC.GCRS.GCRSC0000025/*警告*/, MessageBoxButtons.YesNo, MessageBoxIcon.Warning
            , MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            {
                return;
            }

            // 设置传入参数 
            EI.EIInfo inBlock = new EI.EIInfo();
            inBlock.Tables[0].Merge(this.efDevGrid_Mat.GetSelectedDataRow());

            //调用 SERVICE
            EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0010b1f5_del", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            // 刷新画面
            this.FormMM0010B1_EF_DO_F2(null, null);

            EFMsgInfo = GC.GCRS.GCRSC0000003/*删除成功。*/;
            this.ef_args.buttonStatusHold = false;  //按钮恢复

        }
        #endregion

        #region F5 材料删除 取消
        private void FormMM0010B1_EF_CANCEL_DO_F5(object sender, EF.EF_Args e)
        {
            this.EFMsgInfo = "材料删除取消!"; ;
        }
        #endregion   

        #region F6 计划确定 
        private void FormMM0010B1_EF_DO_F6(object sender, EF.EF_Args e)
        {
            //判断是否有选中行,并限制只选中单个计划进行操作
            if (this.efDevGrid_Plan.GetSelectedDataRow().Rows.Count <= 0 
             || this.efDevGrid_Plan.GetSelectedDataRow().Rows.Count > 1)
            {
                this.EFMsgInfo = "请选择计划信息的单个计划进行操作。";
                MessageBox.Show("请选择计划信息的单个计划进行操作。");
                this.ef_args.buttonStatusHold = true;
                return;

            }

            // 设置传入参数 
            EI.EIInfo inBlock = new EI.EIInfo();
            inBlock.Tables[0].Merge(this.efDevGrid_Plan.GetSelectedDataRow());

            //调用 SERVICE
            EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0010b1f6_pro", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            //判断调用是否正确
            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            // 刷新画面
            this.FormMM0010B1_EF_DO_F2(sender, e);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;

        }
        #endregion

        #region 单击计划Grid的信息 gridView_Plan_FocusedRowObjectChanged
        private void gridView_Plan_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            if (this.gridView_Plan.GetFocusedDataRow() != null)
            {
                QueryMatInfo();   //调用单记录查询
            }
        }
        #endregion
        
        #region  翻页事件
        //关于翻页的首页 尾页 向上 向下翻页功能
        private void efDevGrid1_EF_GridBar_First_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                iPageIndex = 1;
                QueryPlan(iPageIndex);
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
                QueryPlan(iPageIndex);
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
                QueryPlan(iPageIndex);
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
                this.efDevGrid_Plan.PageSize = e.PageSize;
                iPageIndex = e.PageTo;
                QueryPlan(iPageIndex);
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
                QueryPlan(iPageIndex);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }

        }

        private void efDevGrid1_EF_GridBar_Refresh_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            QueryPlan(iPageIndex);
        }
        #endregion

 

    }
}
