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
    public partial class FormMM0097A1S2N : EF.EFForm
    {
        public FormMM0097A1S2N()
        {
            InitializeComponent();
        }

        #region <自定义程序用全局变量>
        // 当前分区
        private string cs_formPartition = "TGT8Z";
        // 当前页数
        private int iPageIndex = 1;
        // 总页数
        private int iPageCount = 0;
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

            //生效
            Common.Utility.SetLookUpEditProperty(eVENT_USE_FLAGEFDevLookUpEdit, outBlock.Tables["M095"], true, true);
        }
        #endregion

        #region 多记录查询 Query()
        //多记录查询
        private void Query(int nPageNo)
        {

            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Columns.Add("EVENT_ID");				//事件号
            inBlock.Tables[0].Columns.Add("MAT_KIND");				//物料种类
            inBlock.Tables[0].Columns.Add("EVENT_LINE_TYPE");       //事件产线类型
            inBlock.Tables[0].Columns.Add("EVENT_SUB_SYSTEM");		//事件子系统
            inBlock.Tables[0].Columns.Add("EVENT_USE_FLAG");		//事件使用标记
            inBlock.Tables[0].Columns.Add("EVENT_NAME");            //事件名称
            inBlock.Tables[0].Columns.Add("ITEM_NAME");            //字段名   

            //使用一个新行前，必须先调用新增操作
            inBlock.Tables[0].Rows.Add();

            inBlock.Tables[0].Rows[0]["EVENT_ID"] = this.eVENT_IDEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["MAT_KIND"] = this.mAT_KINDEFDevLookUpEdit.EditValue;
            inBlock.Tables[0].Rows[0]["EVENT_LINE_TYPE"] = this.eVENT_LINE_TYPEDEFDevLookUpEdit.EditValue;
            inBlock.Tables[0].Rows[0]["EVENT_SUB_SYSTEM"] = this.eVENT_SUB_SYSTEMEFDevLookUpEdit.EditValue;
            inBlock.Tables[0].Rows[0]["EVENT_USE_FLAG"] = this.eVENT_USE_FLAGEFDevLookUpEdit.EditValue;
            inBlock.Tables[0].Rows[0]["EVENT_NAME"] = this.eVENT_NAMEEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["ITEM_NAME"] = this.iTEM_NAMEEFDevTextEdit.Text.Trim();

            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0097a1f2_inq", inBlock);

            if (outBlock.sys_info.flag < 0)
            {
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                return;
            }

            //清空Grid的数据
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_97).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_99).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_9A).ClearGridData();
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
            inBlock.Tables[0].Rows[0]["EVENT_ID"] = this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "EVENT_ID").ToString();
            inBlock.Tables[0].Rows[0]["MAT_KIND"] = this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "MAT_KIND").ToString();
            inBlock.Tables[0].Rows[0]["EVENT_LINE_TYPE"] = this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "EVENT_LINE_TYPE").ToString();

            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0097a1a1_inq", inBlock);

            if (outBlock.sys_info.flag < 0)
            {
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                return;
            }

            //清空Grid的数据
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_99).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_9A).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_99_PZCS).ClearGridData();

            //将信息压入指定的GRID
            EFX.EFCGrid.GetEFCGridBase(this.efDevGrid_99_PZCS).SetGridValue(outBlock); //抛帐参数

            //将信息压入指定的GRID的DataSource 便于对grid2进行增删改
            this.efDevGrid_99.DataSource = outBlock.Tables[1];
            this.efDevGrid_9A.DataSource = outBlock.Tables[2];

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
            this.efDevGrid_99_PZCS.SetColumnsWidthAuto();

        }
        #endregion

        #region 查询待选参数_ORA&DB2环境 QueryParaInfo()
        private void QueryParaInfo()
        {
            // 校验grid是否有事件信息
            if (this.gridView_97.RowCount <= 0)
            {
                this.EFMsgInfo = "请选择需操作的事件!";
                MessageBox.Show("请选择需操作的事件!");
                this.ef_args.buttonStatusHold = true;
                return;
            }
            //前台查询方式有问题，现改成service查询数据
            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;
            inBlock.Tables[0].Columns.Add("COLUMN_NAME");				//字段名
            inBlock.Tables[0].Columns.Add("MAT_KIND");				//物料种类

            inBlock.Tables[0].Rows.Add();

            inBlock.Tables[0].Rows[0]["COLUMN_NAME"] = this.efDevTextEdit_Ename.Text.Trim();
            inBlock.Tables[0].Rows[0]["MAT_KIND"] = this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "MAT_KIND").ToString();

            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0097code_inq", inBlock);

            if (outBlock.sys_info.flag < 0) {
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                return;
            }

            //string s_mat_kind = this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "MAT_KIND").ToString();
            //string sqlCode_ORA = string.Format("SELECT  A.COLUMN_NAME AS ITEM_ENAME," +
            //                               "        B.COMMENTS AS ITEM_CNAME," +
            //                               "        CASE A.DATA_TYPE WHEN 'VARCHAR2' THEN 'C' ELSE 'N' END AS ITEM_TYPE," +
            //                               "        A.DATA_LENGTH AS ITEM_LEN" +
            //                               "  FROM USER_TAB_COLUMNS A, USER_COL_COMMENTS B" +
            //                               " WHERE A.TABLE_NAME = B.TABLE_NAME" +
            //                               "   AND A.COLUMN_NAME = B.COLUMN_NAME" +
            //                               "   AND A.COLUMN_NAME LIKE '%" + this.efDevTextEdit_Ename.Text.Trim() + "%'" +
            //                               "   AND A.TABLE_NAME = 'TMM" + s_mat_kind + "96' ORDER BY A.COLUMN_ID ");
            //string sqlCode_DB2 = string.Format("SELECT  COLNAME AS ITEM_ENAME," +
            //                    "        REMARKS AS ITEM_CNAME," +
            //                    "        CASE TYPENAME WHEN 'VARCHAR' THEN 'C' ELSE 'N' END AS ITEM_TYPE," +
            //                    "        LENGTH AS ITEM_LEN" +
            //                    "  FROM SYSCAT.COLUMNS" +
            //                    " WHERE COLNAME LIKE '%" + this.efDevTextEdit_Ename.Text.Trim() + "%'" +
            //                    "   AND TABNAME = 'TMM" + s_mat_kind + "96' ORDER BY COLNAME ");
            //查当前数据库类型
            //因下面语句执行报错(查不到小代码，原因未知)，暂时更改写死
            //string sqlCode_DATAKIND = string.Format("SELECT CODE_DESC_1_CONTENT  FROM TEP0002 WHERE CODE_CLASS = 'M09S2N' AND CODE_DESC_2_CONTENT = 'Y'");
            //EI.EIInfo outBlockCode = EF.Utility.ExecQuery(sqlCode_DATAKIND);
            //if (EF.Utility.IsBlockHasError(outBlockCode, this))
            //    return;
            //string s_datakind = outBlockCode.GetColVal(1, "CODE_DESC_1_CONTENT");
            //string s_datakind = "ORA";
            //string sqlCode = "";
            //if (s_datakind.Trim() == "DB2")
            //{
            //    sqlCode = sqlCode_DB2;
            //}
            //else
            //{
            //    sqlCode = sqlCode_ORA;
            //}
            //EI.EIInfo outBlock = EF.Utility.ExecQuery(sqlCode);
            //if (EF.Utility.IsBlockHasError(outBlock, this))
            //    return;

            //将信息压入指定的GRID的DataSource 便于对grid2进行增删改
            this.efDevGrid_Para.DataSource = outBlock.Tables[0];

            //字段靠左冻结。
            gridView_Para.Columns.ColumnByFieldName("ITEM_ENAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //设置列字段自动调节宽度
            this.efDevGrid_Para.SetColumnsWidthAuto();

        }
        #endregion

        #region 画面加载事件 FormMM0097A1_Load
        private void FormMM0097A1_Load(object sender, EventArgs e)
        {
            //设置分区参数
            cs_formPartition = this.ef_args.formPartition;

            //根据ED54配置,显示EFDevGrid列标题
            EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efDevGrid_97, efDevGrid_99, efDevGrid_9A, efDevGrid_99_PZCS, efDevGrid_Para },
                new string[] { "MM0097A1_97", "MM0097A1_99", "MM0097A1_9A", "MM0097A1_99_PZCS", "MM0097A1_PARA" }, cs_formPartition);

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

            //// Grid不可过滤 ，不可排序。
            //gridView_99.OptionsCustomization.AllowFilter = false;
            //gridView_99.OptionsCustomization.AllowSort = false;

            tabbedControlGroup_97.SelectedTabPage = layoutControlGroup_97;
            tabbedControlGroup_99.SelectedTabPage = layoutControlGroup_99_PZCS;

        }
        #endregion

        #region F2 查询
        private void FormMM0097A1_EF_DO_F2(object sender, EF.EF_Args e)
        {
            try
            {
                Query(1);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }

        }
        #endregion

        #region F6 事件维护 准备
        private void FormMM0097A1_EF_PRE_DO_F6(object sender, EF.EF_Args e)
        {
            // 设置当前TAB页是参数维护
            tabbedControlGroup_99.SelectedTabPage = layoutControlGroup_99;

            // 设置grid 增删按钮属性
            this.efDevGrid_97.ShowAddCopyRowButton = true;
            this.efDevGrid_97.ShowAddRowButton = true;
            this.efDevGrid_97.ShowDeleteRowButton = true;

            this.efDevGrid_99.ShowAddCopyRowButton = true;
            this.efDevGrid_99.ShowAddRowButton = true;
            this.efDevGrid_99.ShowDeleteRowButton = true;

            this.efDevGrid_9A.ShowAddCopyRowButton = true;
            this.efDevGrid_9A.ShowAddRowButton = true;
            this.efDevGrid_9A.ShowDeleteRowButton = true;

        }
        #endregion

        #region F6 事件维护 确定
        private void FormMM0097A1_EF_DO_F6(object sender, EF.EF_Args e)
        {
            // 存放原来选定的行的行号 
            int i_focusedRow = Math.Max(this.gridView_97.FocusedRowHandle, 0);

            // 维护事件数据 
            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            DataTable dataTable_97 = this.efDevGrid_97.DataSource as DataTable;

            DataTable delTable_97 = null;
            DataTable updTable_97 = dataTable_97.Clone();
            DataTable insTable_97 = dataTable_97.Clone();

            DataTable dataTable_99 = this.efDevGrid_99.DataSource as DataTable;
            DataTable delTable_99 = null;
            DataTable updTable_99 = dataTable_99.Clone();
            DataTable insTable_99 = dataTable_99.Clone();

            DataTable dataTable_9A = this.efDevGrid_9A.DataSource as DataTable;
            DataTable delTable_9A = null;
            DataTable updTable_9A = dataTable_9A.Clone();
            DataTable insTable_9A = dataTable_9A.Clone();

            for (int i = 0; i < gridView_97.RowCount; i++)
            {
                if (efDevGrid_97.GetSelectedColumnChecked(i))
                {
                    if (this.gridView_97.GetDataRow(i).RowState == DataRowState.Added)
                    {
                        insTable_97.Rows.Add(this.gridView_97.GetDataRow(i).ItemArray);
                    }
                    else if (this.gridView_97.GetDataRow(i).RowState == DataRowState.Modified)
                    {
                        updTable_97.Rows.Add(this.gridView_97.GetDataRow(i).ItemArray);
                    }
                }
            }
            for (int i = 0; i < gridView_99.RowCount; i++)
            {
                if (efDevGrid_99.GetSelectedColumnChecked(i))
                {
                    if (this.gridView_99.GetDataRow(i).RowState == DataRowState.Added)
                    {
                        insTable_99.Rows.Add(this.gridView_99.GetDataRow(i).ItemArray);
                    }
                    else if (this.gridView_99.GetDataRow(i).RowState == DataRowState.Modified)
                    {
                        updTable_99.Rows.Add(this.gridView_99.GetDataRow(i).ItemArray);
                    }
                }
            }
            for (int i = 0; i < gridView_9A.RowCount; i++)
            {
                if (efDevGrid_9A.GetSelectedColumnChecked(i))
                {
                    if (this.gridView_9A.GetDataRow(i).RowState == DataRowState.Added)
                    {
                        insTable_9A.Rows.Add(this.gridView_9A.GetDataRow(i).ItemArray);
                    }
                    else if (this.gridView_9A.GetDataRow(i).RowState == DataRowState.Modified)
                    {
                        updTable_9A.Rows.Add(this.gridView_9A.GetDataRow(i).ItemArray);
                    }
                }
            }

            delTable_97 = dataTable_97.GetChanges(DataRowState.Deleted);
            delTable_99 = dataTable_99.GetChanges(DataRowState.Deleted);
            delTable_9A = dataTable_9A.GetChanges(DataRowState.Deleted);

            if (insTable_97 != null && insTable_97.Rows.Count > 0)
            {
                insTable_97.TableName = "MM0097A1_97_INS";
                inBlock.Tables.Add(insTable_97);
            }
            if (delTable_97 != null && delTable_97.Rows.Count > 0)
            {
                delTable_97.RejectChanges();
                delTable_97.TableName = "MM0097A1_97_DEL";
                inBlock.Tables.Add(delTable_97);
            }
            if (updTable_97 != null && updTable_97.Rows.Count > 0)
            {
                updTable_97.TableName = "MM0097A1_97_UPD";
                inBlock.Tables.Add(updTable_97);
            }

            if (insTable_99 != null && insTable_99.Rows.Count > 0)
            {
                insTable_99.TableName = "MM0097A1_99_INS";
                inBlock.Tables.Add(insTable_99);
            }
            if (delTable_99 != null && delTable_99.Rows.Count > 0)
            {
                delTable_99.RejectChanges();
                delTable_99.TableName = "MM0097A1_99_DEL";
                inBlock.Tables.Add(delTable_99);
            }
            if (updTable_99 != null && updTable_99.Rows.Count > 0)
            {
                updTable_99.TableName = "MM0097A1_99_UPD";
                inBlock.Tables.Add(updTable_99);
            }

            if (insTable_9A != null && insTable_9A.Rows.Count > 0)
            {
                insTable_9A.TableName = "MM0097A1_9A_INS";
                inBlock.Tables.Add(insTable_9A);
            }
            if (delTable_9A != null && delTable_9A.Rows.Count > 0)
            {
                delTable_9A.RejectChanges();
                delTable_9A.TableName = "MM0097A1_9A_DEL";
                inBlock.Tables.Add(delTable_9A);
            }
            if (updTable_9A != null && updTable_9A.Rows.Count > 0)
            {
                updTable_9A.TableName = "MM0097A1_9A_UPD";
                inBlock.Tables.Add(updTable_9A);
            }
            //判断是否有增删改的数据
            if (inBlock.Tables.IndexOf("MM0097A1_97_INS") < 0
             && inBlock.Tables.IndexOf("MM0097A1_97_DEL") < 0
             && inBlock.Tables.IndexOf("MM0097A1_97_UPD") < 0
             && inBlock.Tables.IndexOf("MM0097A1_99_INS") < 0
             && inBlock.Tables.IndexOf("MM0097A1_99_DEL") < 0
             && inBlock.Tables.IndexOf("MM0097A1_99_UPD") < 0
             && inBlock.Tables.IndexOf("MM0097A1_9A_INS") < 0
             && inBlock.Tables.IndexOf("MM0097A1_9A_DEL") < 0
             && inBlock.Tables.IndexOf("MM0097A1_9A_UPD") < 0)
            {
                this.EFMsgInfo = "没有增删改的记录。";
                this.ef_args.buttonStatusHold = true;
                return;
            }

            //调用新增SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0097a1f6_pro", inBlock);
            this.EFSysInfo = outBlock.sys_info;

            //判断调用是否正确
            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            // 设置grid 增删按钮属性
            this.efDevGrid_97.ShowAddCopyRowButton = false;
            this.efDevGrid_97.ShowAddRowButton = false;
            this.efDevGrid_97.ShowDeleteRowButton = false;

            this.efDevGrid_99.ShowAddCopyRowButton = false;
            this.efDevGrid_99.ShowAddRowButton = false;
            this.efDevGrid_99.ShowDeleteRowButton = false;

            this.efDevGrid_9A.ShowAddCopyRowButton = false;
            this.efDevGrid_9A.ShowAddRowButton = false;
            this.efDevGrid_9A.ShowDeleteRowButton = false;

            //刷新页面 查询参数信息
            Query(1);

            // 取消系统默认选定的当前行
            gridView_97.UnselectRow(this.gridView_97.FocusedRowHandle);

            // 选定行在原来的位置
            this.gridView_97.FocusedRowHandle = Math.Min(i_focusedRow, this.gridView_97.RowCount);

            // 当前行被选中
            efDevGrid_97.SetSelectedColumnChecked(this.gridView_97.FocusedRowHandle, true);


            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }
        #endregion

        #region F6 事件维护 取消
        private void FormMM0097A1_EF_CANCEL_DO_F6(object sender, EF.EF_Args e)
        {
            // 设置当前TAB页是抛帐维护
            tabbedControlGroup_99.SelectedTabPage = layoutControlGroup_99_PZCS;

            // 设置grid 增删按钮属性
            this.efDevGrid_97.ShowAddCopyRowButton = false;
            this.efDevGrid_97.ShowAddRowButton = false;
            this.efDevGrid_97.ShowDeleteRowButton = false;

            this.efDevGrid_99.ShowAddCopyRowButton = false;
            this.efDevGrid_99.ShowAddRowButton = false;
            this.efDevGrid_99.ShowDeleteRowButton = false;

            this.efDevGrid_9A.ShowAddCopyRowButton = false;
            this.efDevGrid_9A.ShowAddRowButton = false;
            this.efDevGrid_9A.ShowDeleteRowButton = false;


            //刷新页面 查询参数信息
            Query(1);

        }
        #endregion

        #region F7 事件复制 确定
        private void FormMM0097A1_EF_DO_F7(object sender, EF.EF_Args e)
        {

            // 判断是否有选中行
            if (this.efDevGrid_97.GetSelectedDataRow().Rows.Count != 1)
            {
                this.EFMsgInfo = "请选择单个事件。";
                MessageBox.Show("请选择单个事件。");
                this.ef_args.buttonStatusHold = true;
                return;
            }

            // 弹出画面
            EI.EIInfo inBlock = new EI.EIInfo();

            inBlock.Tables[0].Merge(this.efDevGrid_97.GetSelectedDataRow());

            //将材料信息区放在第1块
            inBlock.Tables.Add();
            EF.EF_Args.common_object_1 = inBlock;
            EF.EF_Args.common_parameter_1 = "F7";
            EF.EF_Args.common_parameter_2 = cs_formPartition;

            this.EFShowDialogForm("MM0097A2S2N", new object[] { });

            // 刷新画面
            this.FormMM0097A1_EF_DO_F2(null, null);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }
        #endregion

        #region F8 参数顺序调整 确定
        private void FormMM0097A1_EF_DO_F8(object sender, EF.EF_Args e)
        {
            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            // 当前Grid全选
            for (int i = 0; i < this.gridView_99.RowCount; i++)
            {
                this.efDevGrid_99.SetSelectedColumnChecked(i, true);
            }

            inBlock.Tables[0].Merge(efDevGrid_99.GetSelectedDataRow());

            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0097a1f8_pro", inBlock);
            if (outBlock.sys_info.flag < 0)
            {
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                return;
            }

            //刷新页面 查询参数信息
            Query(1);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }
        #endregion

        #region F9 主档表重构 确定
        private void FormMM0097A1_EF_DO_F9(object sender, EF.EF_Args e)
        {
            if (this.mAT_KINDEFDevLookUpEdit.EditValue.ToString() == "")
            {
                this.EFMsgInfo = "请选择物料种类!";
                MessageBox.Show("请选择物料种类!");
                this.ef_args.buttonStatusHold = true;
                return;
            }

            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Columns.Add("TABLE_NAME");			//表名
            inBlock.Tables[0].Columns.Add("MAT_KIND");				//物料种类

            //使用一个新行前，必须先调用新增操作
            inBlock.Tables[0].Rows.Add();

            inBlock.Tables[0].Rows[0]["TABLE_NAME"] = " ";
            inBlock.Tables[0].Rows[0]["MAT_KIND"] = this.mAT_KINDEFDevLookUpEdit.EditValue;

            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mmtp_matTableColSave", inBlock);

            if (outBlock.sys_info.flag < 0)
            {
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                return;
            }

            //恢复光标为箭头
            this.Cursor = System.Windows.Forms.Cursors.Default;

            this.EFMsgInfo = "主档表重构!";

        }
        #endregion

        #region F10 路径表重构 确定
        private void FormMM0097A1_EF_DO_FA(object sender, EF.EF_Args e)
        {
            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Columns.Add("TABLE_NAME");			//表名

            //使用一个新行前，必须先调用新增操作
            inBlock.Tables[0].Rows.Add();

            inBlock.Tables[0].Rows[0]["TABLE_NAME"] = "TMM0005";

            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mmtp_matTableColSave", inBlock);

            if (outBlock.sys_info.flag < 0)
            {
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                return;
            }

            //恢复光标为箭头
            this.Cursor = System.Windows.Forms.Cursors.Default;

            this.EFMsgInfo = "路径表重构!";

        }
        #endregion

        #region 单击查询按钮 efButton_Query_Click
        private void efButton_Query_Click(object sender, EventArgs e)
        {
            QueryParaInfo();
        }
        #endregion

        #region 单击左移按钮 efButton_To_Left_Click
        private void efButton_To_Left_Click(object sender, EventArgs e)
        {
            // 判断是否有选中行
            if (this.efDevGrid_Para.GetSelectedDataRow().Rows.Count <= 0)
            {
                this.EFMsgInfo = "请选择待选参数。";
                MessageBox.Show("请选择待选参数。");
                this.ef_args.buttonStatusHold = true;
                return;
            }

            // 将待选参数的Grid行移入参数维护Grid行
            DataTable dt_Para = this.efDevGrid_Para.GetSelectedDataRow();
            for (int i = 0; i < this.efDevGrid_Para.GetSelectedDataRow().Rows.Count; i++)
            {
                //获取参数维护区最后一行 DataRow
                DataRow prevRow_99 = null;
                if (this.gridView_99.RowCount > 0)
                {
                    prevRow_99 = this.gridView_99.GetDataRow(this.gridView_99.RowCount - 1);
                }

                gridView_99.ClearSorting();

                //参数维护区 新增行
                this.gridView_99.AddNewRow();
                this.gridView_99.AddNewRow();  //这一行，针对的DEV19平台特意加的，否则会报错
                this.gridView_99.RefreshData();

                //获取参数维护区新增行 DataRow
                DataRow newRow_99 = this.gridView_99.GetDataRow(this.gridView_99.RowCount - 2); //改为-2，针对的DEV19平台特意改的，否则会报错

                //复制新增行
                if (prevRow_99 != null)
                {
                    newRow_99.ItemArray = prevRow_99.ItemArray;

                    //新增行序号 取上一行的序号加1
                    newRow_99["SEQ_NO"] = Convert.ToDecimal(prevRow_99["SEQ_NO"].ToString()) + 1;
                }


                //设置新增行默认值
                newRow_99["ITEM_PARA"] = "Y"; //接口参数           Y-是
                newRow_99["ITEM_PARA_ALLOW_NULL"] = "N"; //接口参数是否为空   N-不可未空
                newRow_99["ITEM_UPD_TYPE"] = "3"; //字段修改类型       3-修改类
                newRow_99["ITEM_UPD_MODE"] = "1"; //字段修改方式       1-接口值
                newRow_99["ITEM_TYPE"] = "C"; //字段类型           C-关键字段不可改                                     

                // 获取待选参数 焦点行
                DataRow paraRow = dt_Para.Rows[i];
                newRow_99["ITEM_ENAME"] = paraRow["ITEM_ENAME"];
                newRow_99["ITEM_CNAME"] = paraRow["ITEM_CNAME"];
                if (paraRow["ITEM_TYPE"].ToString().Trim() == "C")
                {
                    newRow_99["ITEM_KIND"] = "S";
                }
                else if (paraRow["ITEM_TYPE"].ToString().Trim() == "N")
                {
                    newRow_99["ITEM_KIND"] = "D";
                }
                newRow_99["ITEM_LEN"] = paraRow["ITEM_LEN"];

                this.gridView_99.BestFitColumns();
                efDevGrid_99.SetSelectedColumnChecked(this.gridView_99.RowCount - 1, true);
                this.gridView_99.RefreshData();                                 //这一行，针对的DEV19平台特意加的，否则会报错
                this.gridView_99.DeleteRow(gridView_99.FocusedRowHandle);       //这一行，针对的DEV19平台特意加的，否则会报错
                this.gridView_99.RefreshData();                                 //这一行，针对的DEV19平台特意加的，否则会报错
            }
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

        #region  电文事件配置表新增行 efDevGrid_9A_EF_GridBar_AddRow_Event
        private void efDevGrid_9A_EF_GridBar_AddRow_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            // gridView新增一行
            this.gridView_9A.AddNewRow();

            // 设置gridView焦点行
            this.gridView_9A.FocusedRowHandle = this.gridView_9A.RowCount - 1;

            // 设置焦点行 选中
            this.efDevGrid_9A.SetSelectedColumnChecked(this.gridView_9A.RowCount - 1, true);

            // gridView刷新数据
            this.gridView_9A.RefreshData();

            // 获取事件信息
            if (this.gridView_97.GetFocusedDataRow() != null)
            {
                this.gridView_9A.GetDataRow(this.gridView_9A.RowCount - 1)["MAT_KIND"] = this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "MAT_KIND").ToString();
                this.gridView_9A.GetDataRow(this.gridView_9A.RowCount - 1)["EVENT_LINE_TYPE"] = this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "EVENT_LINE_TYPE").ToString();
                this.gridView_9A.GetDataRow(this.gridView_9A.RowCount - 1)["EVENT_ID"] = this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "EVENT_ID").ToString();
            }
        }
        #endregion

        #region 单击某行编辑后该行自动选中 gridView_97_CellValueChanging
        private void gridView_97_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            var gridView = sender as GridView;
            var efDevGrid = gridView.GridControl as EF.EFDevGrid;
            if (e.Column != efDevGrid.SelectionColumn)
            {
                efDevGrid.SetSelectedColumnChecked(e.RowHandle, true);
            }
        }
        #endregion

        #region 单击某行编辑后该行自动选中 gridView_99_CellValueChanging
        private void gridView_99_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            var gridView = sender as GridView;
            var efDevGrid = gridView.GridControl as EF.EFDevGrid;
            if (e.Column != efDevGrid.SelectionColumn)
            {
                efDevGrid.SetSelectedColumnChecked(e.RowHandle, true);
            }
        }
        #endregion

        #region 修改行变粗体-新增行红色 gridView_97_RowStyle
        private void gridView_97_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle < 0)
                return;

            var gridView = sender as GridView;

            if (((DataRowView)gridView.GetRow(e.RowHandle)).Row.RowState == DataRowState.Modified)
            {
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            }
            if (((DataRowView)gridView.GetRow(e.RowHandle)).Row.RowState == DataRowState.Added)
            {
                e.Appearance.ForeColor = Color.Red;
            }

        }
        #endregion

        #region 修改行变粗体-新增行红色 gridView_99_RowStyle
        private void gridView_99_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle < 0)
                return;

            var gridView = sender as GridView;

            if (((DataRowView)gridView.GetRow(e.RowHandle)).Row.RowState == DataRowState.Modified)
            {
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            }
            if (((DataRowView)gridView.GetRow(e.RowHandle)).Row.RowState == DataRowState.Added)
            {
                e.Appearance.ForeColor = Color.Red;
            }

        }
        #endregion

        #region 单击某行编辑后该行自动选中 gridView_99_CellValueChanging
        private void gridView_9A_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            var gridView = sender as GridView;
            var efDevGrid = gridView.GridControl as EF.EFDevGrid;
            if (e.Column != efDevGrid.SelectionColumn)
            {
                efDevGrid.SetSelectedColumnChecked(e.RowHandle, true);
            }
        }
        #endregion

        #region 修改行变粗体-新增行红色 gridView_9A_RowStyle
        private void gridView_9A_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (e.RowHandle < 0)
                return;

            var gridView = sender as GridView;

            if (((DataRowView)gridView.GetRow(e.RowHandle)).Row.RowState == DataRowState.Modified)
            {
                e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
            }
            if (((DataRowView)gridView.GetRow(e.RowHandle)).Row.RowState == DataRowState.Added)
            {
                e.Appearance.ForeColor = Color.Red;
            }

        }
        #endregion

    }
}
