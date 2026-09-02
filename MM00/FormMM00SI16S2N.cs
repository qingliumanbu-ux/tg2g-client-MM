using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MM
{
    public partial class FormMM00SI16S2N : EF.EFForm
    {
        public FormMM00SI16S2N()
        {
            InitializeComponent();
        }

        #region 绑定下拉框内容 BindDataSource()
        private void BindDataSource()
        {
            EI.EIInfo outBlock = EF.Utility.GetPartitionCodeClassValue(cs_formPartition, "M001", "M002", "M003");
            //物料产线类型
            Common.Utility.SetLookUpEditProperty(this.efDevLookUpEdit_MAT_LINE_TYPE, outBlock.Tables["M001"], true, true);
            //物料种类
            Common.Utility.SetLookUpEditProperty(this.efDevLookUpEdit_MAT_KIND, outBlock.Tables["M002"], true, true);
            //材料形态标志
            Common.Utility.SetLookUpEditProperty(this.efDevLookUpEdit_MAT_SHAPE_FLAG, outBlock.Tables["M003"], true, true);

            //机组号
            string sqlCode = string.Format("SELECT UNIT_CODE AS CODE,UNIT_CNAME AS CODE_NAME FROM TSI0015 ");
            EI.EIInfo outBlockCode = EF.Utility.ExecQueryPart(cs_formPartition, sqlCode);
            if (EF.Utility.IsBlockHasError(outBlockCode, this))
                return;
            outBlockCode.Tables[0].Columns[0].Caption = "机组";
            outBlockCode.Tables[0].Columns[1].Caption = "名称";

            Common.Utility.SetLookUpEditProperty(this.efDevLookUpEdit_UNIT_CODE, outBlockCode.Tables[0], true, "CODE_NAME", "CODE", "名称", "机组", true);          
        }
        #endregion

        #region <自定义程序用全局变量
        // 当前分区
        private string cs_formPartition = "";

        #endregion

        #region 多记录查询 Query()
        private void Query()
        {
            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Columns.Add("UNIT_CODE");             //机组号
            inBlock.Tables[0].Columns.Add("MAT_KIND");              //物料种类
            inBlock.Tables[0].Columns.Add("MAT_LINE_TYPE");         //物料产线类型
            inBlock.Tables[0].Columns.Add("MAT_SHAPE_FLAG");        //材料形态标志
            inBlock.Tables[0].Columns.Add("WHOLE_BACKLOG_CODE");    //全程工序代码
            inBlock.Tables[0].Columns.Add("WHOLE_BACKLOG_NAME");    //全程工序名称
            inBlock.Tables[0].Columns.Add("FACTORY_DIV");           //厂别区分
            inBlock.Tables[0].Columns.Add("IN_MAT_KIND");           //入口物料种类
            inBlock.Tables[0].Columns.Add("PROD_TABLE_NAME");       //数据库生产实绩表名
            inBlock.Tables[0].Rows.Add();
            inBlock.Tables[0].Rows[0]["UNIT_CODE"]          = this.efDevLookUpEdit_UNIT_CODE.EditValue;
            inBlock.Tables[0].Rows[0]["MAT_KIND"]           = this.efDevLookUpEdit_MAT_KIND.EditValue;
            inBlock.Tables[0].Rows[0]["MAT_LINE_TYPE"]      = this.efDevLookUpEdit_MAT_LINE_TYPE.EditValue;
            inBlock.Tables[0].Rows[0]["MAT_SHAPE_FLAG"]     = this.efDevLookUpEdit_MAT_SHAPE_FLAG.EditValue;
            inBlock.Tables[0].Rows[0]["WHOLE_BACKLOG_CODE"] = this.efDevTextEdit_WHOLE_BACKLOG_CODE.Text.Trim();
            inBlock.Tables[0].Rows[0]["WHOLE_BACKLOG_NAME"] = this.efDevTextEdit_WHOLE_BACKLOG_NAME.Text.Trim();
            inBlock.Tables[0].Rows[0]["FACTORY_DIV"]        = this.efDevTextEdit_FACTORY_DIV.Text.Trim();
            inBlock.Tables[0].Rows[0]["IN_MAT_KIND"]        = this.efDevTextEdit_IN_MAT_KIND.Text.Trim();
            inBlock.Tables[0].Rows[0]["PROD_TABLE_NAME"]    = this.efDevTextEdit_PROD_TABLE_NAME.Text.Trim();

            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm00si16f2_inq", inBlock);
            if (outBlock.sys_info.flag < 0)
            {
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                return;
            }

            //清空GRID的数据
            EFX.EFCGrid.GetEFCGridBase(this.efDevGrid_INQ).ClearGridData();

            //将信息压入指定的GRID的DataSource
            this.efDevGrid_INQ.DataSource = outBlock.Tables[0];

            //恢复光标为箭头
            this.Cursor = System.Windows.Forms.Cursors.Default;

            //设置列字段自动调节宽度
            this.efDevGrid_INQ.SetColumnsWidthAuto();

            this.EFMsgInfo = string.Format(GC.GCRS.GCRSC0000033/*查询到[{0}]条记录。*/, outBlock.blk_info[outBlock.blk_now].Row);
        }
        #endregion

        #region 画面载入 FormMM00SI16_Load
        private void FormMM00SI16_Load(object sender, EventArgs e)
        {
            //设置分区参数
            cs_formPartition = this.ef_args.formPartition;

            //设置Grid的ED54配置列
            EF.Utility.SetGridColumn(new EF.EFDevGrid[] { this.efDevGrid_INQ },
                new string[] { "MM00SI16_INQ" }, cs_formPartition);

            //绑定下拉框内容
            BindDataSource();

            //设置GRID不可编辑
            this.efDevGrid_INQ.SetAllColumnReadOnlyWithoutSelection(true);
        }
        #endregion

        #region F1 从SI0021中同步
        private void FormMM00SI16_EF_DO_F1(object sender, EF.EF_Args e)
        {
            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            //调用维护SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm00si16f1_pro", inBlock);
            this.EFSysInfo = outBlock.sys_info;

            //刷新页面,查询参数信息
            this.FormMM00SI16_EF_DO_F2(null, null);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }
        #endregion

        #region F2 查询
        private void FormMM00SI16_EF_DO_F2(object sender, EF.EF_Args e)
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

        #region F3 新增
        private void FormMM00SI16_EF_PRE_DO_F3(object sender, EF.EF_Args e)
        {
            //新增一行
            this.gridView_INQ.AddNewRow();
            
            //选中新增行,设置可编辑
            this.gridView_INQ.FocusedRowHandle = this.gridView_INQ.RowCount;
            this.efDevGrid_INQ.SetSelectedColumnChecked(this.gridView_INQ.FocusedRowHandle, true);
            this.efDevGrid_INQ.SetAllColumnReadOnlyWithoutSelection(false);
        }

        private void FormMM00SI16_EF_DO_F3(object sender, EF.EF_Args e)
        {
            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            this.gridView_INQ.FocusedRowHandle = this.gridView_INQ.RowCount;
            this.efDevGrid_INQ.SetSelectedColumnChecked(this.gridView_INQ.FocusedRowHandle, true);
            inBlock.Tables[0].Merge(this.efDevGrid_INQ.GetSelectedDataRow());

            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm00si16f3_ins", inBlock);
            this.EFSysInfo = outBlock.sys_info;

            //判断调用是否正确
            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            //设置GRID不可编辑
            this.efDevGrid_INQ.SetAllColumnReadOnlyWithoutSelection(true);

            //刷新页面,查询参数信息
            this.FormMM00SI16_EF_DO_F2(null, null);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }

        private void FormMM00SI16_EF_CANCEL_DO_F3(object sender, EF.EF_Args e)
        {
            //删除新增行
            this.gridView_INQ.DeleteSelectedRows();
            this.efDevGrid_INQ.SetAllColumnReadOnlyWithoutSelection(true);
            //刷新页面,查询参数信息
            this.FormMM00SI16_EF_DO_F2(null, null);
        }
        #endregion

        #region F4 修改
        private void FormMM00SI16_EF_PRE_DO_F4(object sender, EF.EF_Args e)
        {
            //设置GRID可编辑
            this.efDevGrid_INQ.SetAllColumnReadOnlyWithoutSelection(false);
        }

        private void FormMM00SI16_EF_DO_F4(object sender, EF.EF_Args e)
        {
            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            //维护事件数据 
            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            DataTable dataTable = this.efDevGrid_INQ.DataSource as DataTable;
            DataTable upd = dataTable.Clone();

            for (int i = 0; i < this.gridView_INQ.RowCount; i++)
            {
                if (this.efDevGrid_INQ.GetSelectedColumnChecked(i))
                {
                    if (this.gridView_INQ.GetDataRow(i).RowState == DataRowState.Modified)
                    {
                        upd.Rows.Add(this.gridView_INQ.GetDataRow(i).ItemArray);
                    }
                }
            }

            if (upd != null && upd.Rows.Count > 0)
            {
                upd.TableName = "MM00SI16_UPD";
                inBlock.Tables.Add(upd);
            }

            //判断是否有修改的数据
            if (inBlock.Tables.IndexOf("MM00SI16_UPD") < 0)
            {
                this.EFMsgInfo = "没有修改的记录。";
                this.ef_args.buttonStatusHold = true;
                return;
            }

            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm00si16f4_upd", inBlock);
            this.EFSysInfo = outBlock.sys_info;

            //判断调用是否正确
            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            //设置GRID不可编辑
            this.efDevGrid_INQ.SetAllColumnReadOnlyWithoutSelection(true);

            //刷新页面,查询参数信息
            this.FormMM00SI16_EF_DO_F2(null, null);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }

        private void FormMM00SI16_EF_CANCEL_DO_F4(object sender, EF.EF_Args e)
        {
            //设置GRID不可编辑
            this.efDevGrid_INQ.SetAllColumnReadOnlyWithoutSelection(true);

            //刷新页面 查询参数信息
            this.FormMM00SI16_EF_DO_F2(null, null);
        }
        #endregion

        #region F5 删除
        private void FormMM00SI16_EF_DO_F5(object sender, EF.EF_Args e)
        {
            // 校验grid是否有材料信息
            if (this.gridView_INQ.RowCount <= 0)
            {
                this.EFMsgInfo = "没有机组数据!";
                MessageBox.Show("没有机组数据!");
                this.ef_args.buttonStatusHold = true;
                return;
            }

            //校验grid是否有选中行
            if (this.efDevGrid_INQ.GetSelectedDataRow().Rows.Count <= 0)
            {
                this.EFMsgInfo = "请选择需操作的机组!";
                MessageBox.Show("请选择需操作的机组!");
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
            EI.EIInfo outBlock;

            inBlock.Tables[0].Merge(this.efDevGrid_INQ.GetSelectedDataRow());

            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm00si16f5_del", inBlock);
            this.EFSysInfo = outBlock.sys_info;

            //判断调用是否正确
            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            //刷新页面,查询参数信息
            this.FormMM00SI16_EF_DO_F2(null, null);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }
        #endregion
    }
}
