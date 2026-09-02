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
    public partial class FormMM0097B1S2N : EF.EFForm
    {
        public FormMM0097B1S2N()
        {
            InitializeComponent();
        }

        #region <自定义程序用全局变量>
        // 当前分区
        private string cs_formPartition = "";
        //查询信息类型 
        private string cs_query_type = "";
        #endregion

        #region 绑定下拉框内容 BindDataSource()
        private void BindDataSource()
        {
            EI.EIInfo outBlock = EF.Utility.GetPartitionCodeClassValue(cs_formPartition,"M002", "M091", "M093", "M095", "M09N");

            //物料种类
            Common.Utility.SetLookUpEditProperty(mAT_KINDEFDevLookUpEdit, outBlock.Tables["M002"], true, true);
            Common.Utility.SetLookUpEditProperty(mAT_KIND_INS_EFDevLookUpEdit, outBlock.Tables["M002"], true, true);

            //事件产线类型 
            Common.Utility.SetLookUpEditProperty(eVENT_LINE_TYPEDEFDevLookUpEdit, outBlock.Tables["M091"], true, true);

            //事件子系统 
            Common.Utility.SetLookUpEditProperty(eVENT_SUB_SYSTEMEFDevLookUpEdit, outBlock.Tables["M093"], true, true);

            //生效
            Common.Utility.SetLookUpEditProperty(eVENT_USE_FLAGEFDevLookUpEdit, outBlock.Tables["M095"], true, true);

        }
        #endregion

        #region 多记录查询 Query_97()
        //多记录查询
        private void Query_97()
        {

            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Columns.Add("TABLE_NAME");		    //表名
            inBlock.Tables[0].Columns.Add("EVENT_ID");				//事件号
            inBlock.Tables[0].Columns.Add("MAT_KIND");				//物料种类
            inBlock.Tables[0].Columns.Add("EVENT_LINE_TYPE");
            inBlock.Tables[0].Columns.Add("EVENT_SUB_SYSTEM");		//事件子系统
            inBlock.Tables[0].Columns.Add("EVENT_USE_FLAG");		//EVENT_USE_FLAG
            //inBlock.Tables[0].Columns.Add("RECORD_COUNT_PER_PAGE"); //每页记录数
            //inBlock.Tables[0].Columns.Add("CURRENT_PAGE_NO");       //需查询的页号

            //使用一个新行前，必须先调用新增操作
            inBlock.Tables[0].Rows.Add();

            inBlock.Tables[0].Rows[0]["TABLE_NAME"]= "TMM0097";
            inBlock.Tables[0].Rows[0]["EVENT_ID"] = this.eVENT_IDEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["MAT_KIND"] = this.mAT_KINDEFDevLookUpEdit.EditValue;
            inBlock.Tables[0].Rows[0]["EVENT_LINE_TYPE"] = this.eVENT_LINE_TYPEDEFDevLookUpEdit.EditValue;
            inBlock.Tables[0].Rows[0]["EVENT_SUB_SYSTEM"] = this.eVENT_SUB_SYSTEMEFDevLookUpEdit.EditValue;
            inBlock.Tables[0].Rows[0]["EVENT_USE_FLAG"] = this.eVENT_USE_FLAGEFDevLookUpEdit.EditValue;
            //inBlock.Tables[0].Rows[0]["RECORD_COUNT_PER_PAGE"] = this.efDevGrid1.PageSize;
            //inBlock.Tables[0].Rows[0]["CURRENT_PAGE_NO"] = nPageNo;

            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097b1f2_inq", inBlock);

            if (outBlock.sys_info.flag < 0)
            {
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                return;
            }

            //清空Grid的数据
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_97).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_99).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_9A).ClearGridData();
            
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

        #region 多记录查询 Query_97_INS()
        //多记录查询
        private void Query_97_INS()
        {

            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Columns.Add("TABLE_NAME");		    //表名
            inBlock.Tables[0].Columns.Add("EVENT_ID");				//事件号
            inBlock.Tables[0].Columns.Add("MAT_KIND");				//物料种类
            inBlock.Tables[0].Columns.Add("EVENT_LINE_TYPE");
            inBlock.Tables[0].Columns.Add("EVENT_SUB_SYSTEM");		//事件子系统
            inBlock.Tables[0].Columns.Add("EVENT_USE_FLAG");		//EVENT_USE_FLAG
            //inBlock.Tables[0].Columns.Add("RECORD_COUNT_PER_PAGE"); //每页记录数
            //inBlock.Tables[0].Columns.Add("CURRENT_PAGE_NO");       //需查询的页号

            //使用一个新行前，必须先调用新增操作
            inBlock.Tables[0].Rows.Add();

            inBlock.Tables[0].Rows[0]["TABLE_NAME"]= "TMM009701";
            inBlock.Tables[0].Rows[0]["EVENT_ID"] = this.eVENT_ID_INS_EFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["MAT_KIND"] = this.mAT_KIND_INS_EFDevLookUpEdit.EditValue;
            //inBlock.Tables[0].Rows[0]["EVENT_LINE_TYPE"] = this.eVENT_LINE_TYPEDEFDevLookUpEdit.EditValue;
            //inBlock.Tables[0].Rows[0]["EVENT_SUB_SYSTEM"] = this.eVENT_SUB_SYSTEM_97_EFDevLookUpEdit.EditValue;
            //inBlock.Tables[0].Rows[0]["EVENT_USE_FLAG"] = this.eVENT_USE_FLAG_97_EFDevLookUpEdit.EditValue;
            //inBlock.Tables[0].Rows[0]["RECORD_COUNT_PER_PAGE"] = this.efDevGrid1.PageSize;
            //inBlock.Tables[0].Rows[0]["CURRENT_PAGE_NO"] = nPageNo;

            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097b1f2_inq", inBlock);

            if (outBlock.sys_info.flag < 0)
            {
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                return;
            }

            //清空Grid的数据
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_97_INS).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_99_INS).ClearGridData();
            
            //将信息压入指定的GRID的DataSource 便于对grid2进行增删改
            this.efDevGrid_97_INS.DataSource = outBlock.Tables[0];

            //恢复光标为箭头
            this.Cursor = System.Windows.Forms.Cursors.Default;

            //字段靠左冻结。
            gridView_97_INS.Columns.ColumnByFieldName("MAT_KIND").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97_INS.Columns.ColumnByFieldName("EVENT_LINE_TYPE").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97_INS.Columns.ColumnByFieldName("EVENT_ID").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97_INS.Columns.ColumnByFieldName("EVENT_NAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //设置列字段自动调节宽度
            this.efDevGrid_97_INS.SetColumnsWidthAuto();

            this.EFMsgInfo = string.Format(GC.GCRS.GCRSC0000033/*查询到[{0}]条记录。*/, outBlock.blk_info[outBlock.blk_now].Row);

        }
        #endregion

        #region 多记录查询 Query_97_DIFF()
        //多记录查询
        private void Query_97_DIFF()
        {
            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            inBlock.Tables.Clear();
            inBlock.Tables.Add(EF.Utility.GetSingleGridValue(efDevGrid_DIFF_Where).Tables[0].Copy());

            inBlock.Tables[0].Columns.Add("TABLE_NAME");		    //表名
            //inBlock.Tables[0].Columns.Add("EVENT_SUB_SYSTEM");		//事件子系统
            //inBlock.Tables[0].Columns.Add("EVENT_USE_FLAG");		//EVENT_USE_FLAG
            //inBlock.Tables[0].Columns.Add("RECORD_COUNT_PER_PAGE"); //每页记录数
            //inBlock.Tables[0].Columns.Add("CURRENT_PAGE_NO");       //需查询的页号

            //使用一个新行前，必须先调用新增操作
            inBlock.Tables[0].Rows.Add();

            inBlock.Tables[0].Rows[0]["TABLE_NAME"]= "TMM009702";
            //inBlock.Tables[0].Rows[0]["EVENT_LINE_TYPE"] = this.eVENT_LINE_TYPEDEFDevLookUpEdit.EditValue;
            //inBlock.Tables[0].Rows[0]["EVENT_SUB_SYSTEM"] = this.eVENT_SUB_SYSTEM_97_EFDevLookUpEdit.EditValue;
            //inBlock.Tables[0].Rows[0]["EVENT_USE_FLAG"] = this.eVENT_USE_FLAG_97_EFDevLookUpEdit.EditValue;
            //inBlock.Tables[0].Rows[0]["RECORD_COUNT_PER_PAGE"] = this.efDevGrid1.PageSize;
            //inBlock.Tables[0].Rows[0]["CURRENT_PAGE_NO"] = nPageNo;

            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097b1f2_inq", inBlock);

            if (outBlock.sys_info.flag < 0)
            {
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                return;
            }

            //清空Grid的数据
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_97_DIFF).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_99_DIFF).ClearGridData();
            
            //将信息压入指定的GRID的DataSource 便于对grid2进行增删改
            this.efDevGrid_97_DIFF.DataSource = outBlock.Tables[0];

            //恢复光标为箭头
            this.Cursor = System.Windows.Forms.Cursors.Default;

            //字段靠左冻结。
            gridView_97_DIFF.Columns.ColumnByFieldName("EVENT_DIFF_TYPE").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97_DIFF.Columns.ColumnByFieldName("EVENT_DIFF_DESC").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97_DIFF.Columns.ColumnByFieldName("MAT_KIND").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97_DIFF.Columns.ColumnByFieldName("EVENT_LINE_TYPE").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97_DIFF.Columns.ColumnByFieldName("EVENT_ID").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97_DIFF.Columns.ColumnByFieldName("EVENT_NAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //设置列字段自动调节宽度
            this.efDevGrid_97_DIFF.SetColumnsWidthAuto();

            this.EFMsgInfo = string.Format(GC.GCRS.GCRSC0000033/*查询到[{0}]条记录。*/, outBlock.blk_info[outBlock.blk_now].Row);

        }
        #endregion

        #region 查询事件参数等信息 QueryInfo()
        private void QueryInfo(String s_table_name,String s_event_id,String s_mat_kind,String s_event_line_type)
        {

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Columns.Add("EVENT_ID");
            inBlock.Tables[0].Columns.Add("MAT_KIND");
            inBlock.Tables[0].Columns.Add("EVENT_LINE_TYPE");
            inBlock.Tables[0].Columns.Add("TABLE_NAME");

            //使用一个新行前，必须先调用新增操作
            inBlock.Tables[0].Rows.Add();
            inBlock.Tables[0].Rows[0]["EVENT_ID"]           = s_event_id;
            inBlock.Tables[0].Rows[0]["MAT_KIND"]           = s_mat_kind;
            inBlock.Tables[0].Rows[0]["EVENT_LINE_TYPE"]    = s_event_line_type; 
            inBlock.Tables[0].Rows[0]["TABLE_NAME"]         = s_table_name;

            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097b1a1_inq", inBlock);

            if (outBlock.sys_info.flag < 0)
            {
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                return;
            }

            if (s_table_name.Trim() == "TMM0099")
            {
                //清空Grid的数据
                EFX.EFCGrid.GetEFCGridBase(efDevGrid_99).ClearGridData();

                //将信息压入指定的GRID的DataSource 便于对grid2进行增删改
                this.efDevGrid_99.DataSource = outBlock.Tables[0];

                //字段靠左冻结。
                gridView_99.Columns.ColumnByFieldName("EVENT_ID").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView_99.Columns.ColumnByFieldName("ITEM_ENAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView_99.Columns.ColumnByFieldName("ITEM_CNAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                //设置列字段自动调节宽度
                this.efDevGrid_99.SetColumnsWidthAuto();
            }
            if (s_table_name.Trim() == "TMM009901") 
            { 
                //清空Grid的数据
                EFX.EFCGrid.GetEFCGridBase(efDevGrid_99_INS).ClearGridData();

                //将信息压入指定的GRID的DataSource 便于对grid2进行增删改
                this.efDevGrid_99_INS.DataSource = outBlock.Tables[0];
 
                //字段靠左冻结。
                gridView_99_INS.Columns.ColumnByFieldName("EVENT_ID").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView_99_INS.Columns.ColumnByFieldName("ITEM_ENAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView_99_INS.Columns.ColumnByFieldName("ITEM_CNAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                //设置列字段自动调节宽度
                this.efDevGrid_99_INS.SetColumnsWidthAuto();
            }
            if (s_table_name.Trim() == "TMM009902") 
            { 
                //清空Grid的数据
                EFX.EFCGrid.GetEFCGridBase(efDevGrid_99_DIFF).ClearGridData();

                //将信息压入指定的GRID的DataSource 便于对grid2进行增删改
                this.efDevGrid_99_DIFF.DataSource = outBlock.Tables[0];
 
                //字段靠左冻结。
                gridView_99_DIFF.Columns.ColumnByFieldName("EVENT_DIFF_TYPE").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView_99_DIFF.Columns.ColumnByFieldName("EVENT_DIFF_DESC").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView_99_DIFF.Columns.ColumnByFieldName("EVENT_ID").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView_99_DIFF.Columns.ColumnByFieldName("ITEM_ENAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                gridView_99_DIFF.Columns.ColumnByFieldName("ITEM_CNAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

                //设置列字段自动调节宽度
                this.efDevGrid_99_DIFF.SetColumnsWidthAuto();
           }

        }
        #endregion

        #region 画面加载事件 FormMM0097B1_Load
        private void FormMM0097B1_Load(object sender, EventArgs e)
        {
            //设置分区参数
            cs_formPartition = this.ef_args.formPartition;

            //根据ED54配置,显示EFDevGrid列标题
            EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efDevGrid_97, efDevGrid_99, efDevGrid_9A, efDevGrid_97_INS, efDevGrid_99_INS,efDevGrid_97_DIFF,efDevGrid_99_DIFF },
                new string[] { "MM0097B1_97", "MM0097B1_99", "MM0097B1_9A", "MM0097B1_97_INS", "MM0097B1_99_INS", "MM0097B1_97_DIFF", "MM0097B1_99_DIFF" },cs_formPartition);

            //查询条件配置
            EFX.EFCGrid.InitSingleGridColumn(this.efDevGrid_DIFF_Where, "MM0097B1_DIFF_WHERE",cs_formPartition);
            (EFX.EFCGrid.GetEFCGridBase(efDevGrid_DIFF_Where) as EFX.EFCGridImp.SingleDev.EFCGridSingleDev).ResetGridValue();

            // 绑定下拉框内容
            BindDataSource();

            //设置当前查询TAB页是命令查询
            cs_query_type = "00";
            tabbedControlGroup_Main.SelectedTabPage = this.layoutControlGroup_00;

            //字段靠左冻结。
            gridView_97.Columns.ColumnByFieldName("MAT_KIND").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97.Columns.ColumnByFieldName("EVENT_LINE_TYPE").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97.Columns.ColumnByFieldName("EVENT_ID").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_97.Columns.ColumnByFieldName("EVENT_NAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            gridView_99.Columns.ColumnByFieldName("EVENT_ID").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_99.Columns.ColumnByFieldName("ITEM_ENAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            gridView_99.Columns.ColumnByFieldName("ITEM_CNAME").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;

            //Grid允许拖动
            efDevGrid_99.AllowDragRow = true;

        }
        #endregion

        #region F2 查询
        private void FormMM0097B1_EF_DO_F2(object sender, EF.EF_Args e)
        {
            try
            {
                if (cs_query_type.Trim() == "00")
                { 
                    Query_97();
                }
                else if (cs_query_type.Trim() == "INS")
                { 
                    Query_97_INS();
                }
                else if (cs_query_type.Trim() == "DIFF")
                { 
                    Query_97_DIFF();
                }
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }

        }
        #endregion

        #region F3 项目事件新增 准备
        private void FormMM0097B1_EF_PRE_DO_F3(object sender, EF.EF_Args e)
        {
            // gridView新增一行
            this.gridView_97_INS.AddNewRow();

            // 设置gridView焦点行
            this.gridView_97_INS.FocusedRowHandle = this.gridView_97_INS.RowCount - 1;

            // 设置焦点行 选中
            this.efDevGrid_97_INS.SetSelectedColumnChecked(this.gridView_97_INS.RowCount - 1, true);

            // gridView刷新数据
            this.gridView_97_INS.RefreshData();

        }
        #endregion

        #region F3 项目事件新增 确定
        private void FormMM0097B1_EF_DO_F3(object sender, EF.EF_Args e)
        {            

            //判断是否有选中行
            if (this.efDevGrid_97_INS.GetSelectedDataRow().Rows.Count <= 0)
            {
                this.EFMsgInfo = "请选择需操作的记录。";
                MessageBox.Show("请选择需操作的记录。");
                this.ef_args.buttonStatusHold = true;
                return;

            }

            // 设置传入参数 
            EI.EIInfo inBlock = new EI.EIInfo();
            inBlock.Tables[0].Merge(this.efDevGrid_97_INS.GetSelectedDataRow());

            //调用 SERVICE
            EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097b1f3_ins", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                
            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            // 刷新画面
            this.FormMM0097B1_EF_DO_F2(null, null);

            EFMsgInfo = "项目事件新增成功!";
            this.ef_args.buttonStatusHold = false;  //按钮恢复

        }
        #endregion

        #region F3 项目事件新增 取消
        private void FormMM0097B1_EF_CANCEL_DO_F3(object sender, EF.EF_Args e)
        {

        }
        #endregion

        #region F4 项目事件参数新增 准备
        private void FormMM0097B1_EF_PRE_DO_F4(object sender, EF.EF_Args e)
        {
            // gridView新增一行
            this.gridView_99_INS.AddNewRow();

            // 设置gridView焦点行
            this.gridView_99_INS.FocusedRowHandle = this.gridView_99_INS.RowCount - 1;

            // 设置焦点行 选中
            this.efDevGrid_99_INS.SetSelectedColumnChecked(this.gridView_99_INS.RowCount - 1, true);

            // gridView刷新数据
            this.gridView_99_INS.RefreshData();

        }
        #endregion
        
        #region F4 项目事件参数新增 确定

        private void FormMM0097B1_EF_DO_F4(object sender, EF.EF_Args e)
        {            

            //判断是否有选中行
            if (this.efDevGrid_99_INS.GetSelectedDataRow().Rows.Count <= 0)
            {
                this.EFMsgInfo = "请选择需操作的记录。";
                MessageBox.Show("请选择需操作的记录。");
                this.ef_args.buttonStatusHold = true;
                return;

            }

            // 设置传入参数 
            EI.EIInfo inBlock = new EI.EIInfo();
            inBlock.Tables[0].Merge(this.efDevGrid_99_INS.GetSelectedDataRow());

            //调用 SERVICE
            EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097b1f4_ins", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                
            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            // 刷新画面
            this.FormMM0097B1_EF_DO_F2(null, null);

            EFMsgInfo = "项目事件参数新增成功";
            this.ef_args.buttonStatusHold = false;  //按钮恢复

        }
        #endregion

        #region F4 项目事件参数新增 取消
        private void FormMM0097B1_EF_CANCEL_DO_F4(object sender, EF.EF_Args e)
        {

        }
        #endregion

        #region F5 删除 确定
        private void FormMM0097B1_EF_DO_F5(object sender, EF.EF_Args e)
        {            

            //判断是否有选中行
            if (this.efDevGrid_97_INS.GetSelectedDataRow().Rows.Count <= 0
             && this.efDevGrid_99_INS.GetSelectedDataRow().Rows.Count <= 0 )
            {
                this.EFMsgInfo = "请选择需操作的事件或事件参数。";
                MessageBox.Show("请选择需操作的事件或事件参数。");
                this.ef_args.buttonStatusHold = true;
                return;

            }

            // 设置传入参数 
            EI.EIInfo inBlock = new EI.EIInfo();
            if (this.efDevGrid_97_INS.GetSelectedDataRow().Rows.Count > 0)
            {
                inBlock.Tables[0].Merge(this.efDevGrid_97_INS.GetSelectedDataRow());
            }
            inBlock.Tables.Add();
            if (this.efDevGrid_99_INS.GetSelectedDataRow().Rows.Count > 0)
            {
                inBlock.Tables[1].Merge(this.efDevGrid_99_INS.GetSelectedDataRow());
            }

            //调用 SERVICE
            EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097b1f5_del", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                
            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            // 刷新画面
            this.FormMM0097B1_EF_DO_F2(null, null);

            EFMsgInfo = GC.GCRS.GCRSC0000003/*删除成功。*/;
            this.ef_args.buttonStatusHold = false;  //按钮恢复

        }
        #endregion

        #region F6 差异比对 确定
        private void FormMM0097B1_EF_DO_F6(object sender, EF.EF_Args e)
        {
           //以下方法进行：必输项校验+数据超长校验
            if (!EFX.EFCGrid.GetEFCGridBase(efDevGrid_DIFF_Where).ValidateGridDataEx())
            { //若校验失败，则直接离开。
                this.ef_args.buttonStatusHold = true;//HOLD-BUTTON的控制权。
                this.EFMsgInfo = "请输入或选择查询条件中的带*号项目（必输项）。";
                return;
            }

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables.Clear();
            inBlock.Tables.Add(EF.Utility.GetSingleGridValue(efDevGrid_DIFF_Where).Tables[0].Copy());

            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097b1f6_pro", inBlock);

            if (outBlock.sys_info.flag < 0)
            {
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                return;
            }

            //清空Grid的数据
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_97).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_99).ClearGridData();
            EFX.EFCGrid.GetEFCGridBase(efDevGrid_9A).ClearGridData();

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
               
        #region 单击grid的信息 gridView_97_FocusedRowObjectChanged
        private void gridView_97_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            if (this.gridView_97.GetFocusedDataRow() != null && gridView_97.FocusedRowHandle >= 0)
            {
                QueryInfo("TMM0099",this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "EVENT_ID").ToString(),
                    this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "MAT_KIND").ToString(),
                    this.gridView_97.GetRowCellValue(gridView_97.FocusedRowHandle, "EVENT_LINE_TYPE").ToString());
            }
        }
        #endregion

        #region 单击grid的信息 gridView_97_INS_FocusedRowObjectChanged
        private void gridView_97_INS_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            if (this.gridView_97_INS.GetFocusedDataRow() != null)
            {
                QueryInfo("TMM009901",this.gridView_97_INS.GetRowCellValue(gridView_97_INS.FocusedRowHandle, "EVENT_ID").ToString(),
                    this.gridView_97_INS.GetRowCellValue(gridView_97_INS.FocusedRowHandle, "MAT_KIND").ToString(),
                    this.gridView_97_INS.GetRowCellValue(gridView_97_INS.FocusedRowHandle, "EVENT_LINE_TYPE").ToString());
            }

        }
        #endregion
        
        #region 单击grid的信息 gridView_97_DIFF_FocusedRowObjectChanged
        private void gridView_97_DIFF_FocusedRowObjectChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowObjectChangedEventArgs e)
        {
            if (this.gridView_97_DIFF.GetFocusedDataRow() != null)
            {
                QueryInfo("TMM009902",this.gridView_97_DIFF.GetRowCellValue(gridView_97_DIFF.FocusedRowHandle, "EVENT_ID").ToString(),
                    this.gridView_97_DIFF.GetRowCellValue(gridView_97_DIFF.FocusedRowHandle, "MAT_KIND").ToString(),
                    this.gridView_97_DIFF.GetRowCellValue(gridView_97_DIFF.FocusedRowHandle, "EVENT_LINE_TYPE").ToString());
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

        #region 查询TAB页转换 tabbedControlGroup_Main_SelectedPageChanged
        private void tabbedControlGroup_Main_SelectedPageChanged(object sender, DevExpress.XtraLayout.LayoutTabPageChangedEventArgs e)
        {
            //当前查询TAB页是命令查询
            if (tabbedControlGroup_Main.SelectedTabPage == this.layoutControlGroup_00)
            {
                cs_query_type = "00";
            }
            else if(tabbedControlGroup_Main.SelectedTabPage == this.layoutControlGroup_INS)
            {
                cs_query_type = "INS";
            }
            else if (tabbedControlGroup_Main.SelectedTabPage == this.layoutControlGroup_DIFF)
            {
                cs_query_type = "DIFF";
            }
        }
        #endregion

 

    }
}
