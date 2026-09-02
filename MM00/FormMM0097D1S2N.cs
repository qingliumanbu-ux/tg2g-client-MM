using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;

namespace MM
{
    public partial class FormMM0097D1S2N : EF.EFForm
    {
        public FormMM0097D1S2N()
        {
            InitializeComponent();
        }
        #region <自定义程序用全局变量>
        // 当前分区
        private string cs_formPartition = "";
        #endregion

        #region 绑定下拉框内容 BindDataSource()
        private void BindDataSource()
        {
            EI.EIInfo outBlock = EF.Utility.GetPartitionCodeClassValue(cs_formPartition, "M002", "M091", "M093", "M095");

            //物料种类
            Common.Utility.SetLookUpEditProperty(mAT_KINDEFDevLookUpEdit, outBlock.Tables["M002"], true, true);

            //事件产线类型 
            Common.Utility.SetLookUpEditProperty(eVENT_LINE_TYPEDEFDevLookUpEdit, outBlock.Tables["M091"], true, true);

            //事件子系统 
            Common.Utility.SetLookUpEditProperty(eVENT_SUB_SYSTEMEFDevLookUpEdit, outBlock.Tables["M093"], true, true);
        }
        #endregion

        #region 多记录查询 Query()
        private void Query()
        {
            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Columns.Add("EVENT_ID");				//事件号
            inBlock.Tables[0].Columns.Add("MAT_KIND");				//物料种类
            inBlock.Tables[0].Columns.Add("EVENT_NAME");            //事件名称
            inBlock.Tables[0].Columns.Add("ITEM_NAME");             //字段名 
            inBlock.Tables[0].Columns.Add("EVENT_LINE_TYPE");       //事件产线类型
            inBlock.Tables[0].Columns.Add("EVENT_SUB_SYSTEM");		//事件子系统
  
            //使用一个新行前，必须先调用新增操作
            inBlock.Tables[0].Rows.Add();
            inBlock.Tables[0].Rows[0]["EVENT_ID"]           = this.eVENT_IDEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["MAT_KIND"]           = this.mAT_KINDEFDevLookUpEdit.EditValue;
            inBlock.Tables[0].Rows[0]["EVENT_NAME"]         = this.eVENT_NAMEEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["ITEM_NAME"]          = this.iTEM_NAMEEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["EVENT_LINE_TYPE"]    = this.eVENT_LINE_TYPEDEFDevLookUpEdit.EditValue;
            inBlock.Tables[0].Rows[0]["EVENT_SUB_SYSTEM"]   = this.eVENT_SUB_SYSTEMEFDevLookUpEdit.EditValue;

            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0097d1f2_inq", inBlock);

            if (outBlock.sys_info.flag < 0)
            {
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                return;
            }

            //清空Grid的数据
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_97).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_99).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_9A).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_9B).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_99_PZCS).ClearGridData();

            //将信息压入指定的GRID的DataSource 便于对grid2进行增删改
            this.efDevGrid_97.DataSource = outBlock.Tables[0];

            //恢复光标为箭头
            this.Cursor = System.Windows.Forms.Cursors.Default;

            //字段靠左冻结。
            gridView_97.Columns.ColumnByFieldName("MAT_KIND").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97.Columns.ColumnByFieldName("EVENT_LINE_TYPE").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97.Columns.ColumnByFieldName("EVENT_ID").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97.Columns.ColumnByFieldName("EVENT_NAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //设置列字段自动调节宽度
            this.efDevGrid_97.SetColumnsWidthAuto();

            this.EFMsgInfo = string.Format(GC.GCRS.GCRSC0000033/*查询到[{0}]条记录。*/, outBlock.blk_info[outBlock.blk_now].Row);
        }
        #endregion

        #region 查询事件参数等信息 QueryInfo()
        private void QueryInfo()
        {
            //焦点行不为空但是行号小于0,比如对材料号进行分组时,焦点行但是行号为负数
            if (gridView_97.FocusedRowHandle < 0)
                return;

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Columns.Add("EVENT_ID");
            inBlock.Tables[0].Columns.Add("MAT_KIND");
            inBlock.Tables[0].Columns.Add("EVENT_LINE_TYPE");

            //使用一个新行前，必须先调用新增操作
            inBlock.Tables[0].Rows.Add();
            inBlock.Tables[0].Rows[0]["EVENT_ID"]        = this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "EVENT_ID").ToString();
            inBlock.Tables[0].Rows[0]["MAT_KIND"]        = this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "MAT_KIND").ToString();
            inBlock.Tables[0].Rows[0]["EVENT_LINE_TYPE"] = this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "EVENT_LINE_TYPE").ToString();

            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0097d1a1_inq", inBlock);

            if (outBlock.sys_info.flag < 0)
            {
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                return;
            }

            //清空Grid的数据
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_99).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_9A).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_9B).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_99_PZCS).ClearGridData();

            //将信息压入指定的GRID
            EFX.EFCGrid.GetEFCGridBase(this.efDevGrid_99_PZCS).SetGridValue(outBlock); //抛帐参数

            //将信息压入指定的GRID的DataSource 便于对grid2进行增删改
            this.efDevGrid_99.DataSource = outBlock.Tables[1];
            this.efDevGrid_9A.DataSource = outBlock.Tables[2];
            this.efDevGrid_9B.DataSource = outBlock.Tables[3];

            //字段靠左冻结。
            gridView_99.Columns.ColumnByFieldName("EVENT_ID").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_99.Columns.ColumnByFieldName("ITEM_ENAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_99.Columns.ColumnByFieldName("ITEM_CNAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //字段靠左冻结。
            gridView_99_PZCS.Columns.ColumnByFieldName("ITEM_ENAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_99_PZCS.Columns.ColumnByFieldName("ITEM_CNAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_99_PZCS.Columns.ColumnByFieldName("ITEM_PARA_ALLOW_NULL").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //设置列字段自动调节宽度
            this.efDevGrid_99.SetColumnsWidthAuto();
            this.efDevGrid_9A.SetColumnsWidthAuto();
            this.efDevGrid_9B.SetColumnsWidthAuto();
            this.efDevGrid_99_PZCS.SetColumnsWidthAuto();
        }
        #endregion

        #region 画面加载事件 FormMM0097D1_Load
        private void FormMM0097D1_Load(object sender, EventArgs e)
        {
            //设置分区参数
            cs_formPartition = this.ef_args.formPartition;

            //根据ED54配置,显示EFDevGrid列标题
            EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efDevGrid_97, efDevGrid_99, efDevGrid_9A, efDevGrid_9B, efDevGrid_99_PZCS },
                new string[] { "MM0097A1_97", "MM0097A1_99", "MM0097A1_9A", "MM0097A1_9B", "MM0097A1_99_PZCS", }, cs_formPartition);

            // 绑定下拉框内容
            BindDataSource();

            // 字段靠左冻结。
            gridView_97.Columns.ColumnByFieldName("MAT_KIND").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97.Columns.ColumnByFieldName("EVENT_LINE_TYPE").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97.Columns.ColumnByFieldName("EVENT_ID").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97.Columns.ColumnByFieldName("EVENT_NAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            gridView_99.Columns.ColumnByFieldName("EVENT_ID").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_99.Columns.ColumnByFieldName("ITEM_ENAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_99.Columns.ColumnByFieldName("ITEM_CNAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            gridView_99_PZCS.Columns.ColumnByFieldName("ITEM_ENAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_99_PZCS.Columns.ColumnByFieldName("ITEM_CNAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_99_PZCS.Columns.ColumnByFieldName("ITEM_PARA_ALLOW_NULL").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            // Grid允许拖动
            efDevGrid_99.AllowDragRow = true;

            this.tabbedControlGroup1.SelectedTabPage = this.layoutControlGroup5;
        }
        #endregion

        #region F2 查询
        private void FormMM0097D1_EF_DO_F2(object sender, EF.EF_Args e)
        {
            try
            {
                Query();
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }

        }
        #endregion

        #region F3 返回当前表
        private void FormMM0097D1_EF_DO_F3(object sender, EF.EF_Args e)
        {
            //焦点行不为空但是行号小于0,比如对材料号进行分组时,焦点行但是行号为负数
            if (gridView_97.FocusedRowHandle < 0)
                return;

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Columns.Add("EVENT_ID");
            inBlock.Tables[0].Columns.Add("MAT_KIND");
            inBlock.Tables[0].Columns.Add("EVENT_LINE_TYPE");

            //使用一个新行前，必须先调用新增操作
            inBlock.Tables[0].Rows.Add();
            inBlock.Tables[0].Rows[0]["EVENT_ID"]        = this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "EVENT_ID").ToString();
            inBlock.Tables[0].Rows[0]["MAT_KIND"]        = this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "MAT_KIND").ToString();
            inBlock.Tables[0].Rows[0]["EVENT_LINE_TYPE"] = this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "EVENT_LINE_TYPE").ToString();

            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0097d1f3_pro", inBlock);

            if (outBlock.sys_info.flag < 0)
            {
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                return;
            }

            //刷新页面,查询参数信息
            this.FormMM0097D1_EF_DO_F2(null, null);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }
        #endregion

        #region 单击grid的信息 gridView_97_FocusedRowObjectChanged
        private void gridView_97_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            if (this.gridView_97.GetFocusedDataRow() != null)
            {
                QueryInfo();
            }
        }
        #endregion
    }
}
