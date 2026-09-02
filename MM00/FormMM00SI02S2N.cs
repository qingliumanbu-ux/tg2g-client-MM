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
    public partial class FormMM00SI02S2N : EF.EFForm
    {
        public FormMM00SI02S2N()
        {
            InitializeComponent();
        }

        #region <自定义程序用全局变量
        // 当前分区
        private string cs_formPartition = "";

        #endregion

        #region 多记录查询  Query()
        private void Query()
        {
            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables.Clear();
            inBlock.Tables.Add(EF.Utility.GetSingleGridValue(this.efDevGrid_QUERY).Tables[0].Copy());

            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm00si02f2_inq", inBlock);
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

        #region 画面载入 FormMM00SI02_Load
        private void FormMM00SI02_Load(object sender, EventArgs e)
        {
            //设置分区参数
            cs_formPartition = this.ef_args.formPartition;

            //设置Grid的ED54配置列
            EF.Utility.SetGridColumn(new EF.EFDevGrid[] { this.efDevGrid_INQ },
                new string[] { "MM00SI02_INQ" }, cs_formPartition);

            //设置Grid的ED54配置列 单记录模式
            EFX.EFCGrid.InitSingleGridColumn(this.efDevGrid_QUERY, "MM00SI02_QUERY", cs_formPartition);

            //EPED54配置模式的查询条件的GROUP的尺寸大小控制
            GC.PM_utility2.DEV_Init_LayoutGroup_where_size(this.layoutControlGroup2, this.efDevGrid_QUERY);
        }
        #endregion

        #region F2 查询
        private void FormMM00SI02_EF_DO_F2(object sender, EF.EF_Args e)
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

        #region F6 维护
        private void FormMM00SI02_EF_PRE_DO_F6(object sender, EF.EF_Args e)
        {
            //设置GRID增删按钮属性
            this.efDevGrid_INQ.SetAllColumnEditable(true);
            this.efDevGrid_INQ.ShowAddCopyRowButton = true;
            this.efDevGrid_INQ.ShowAddRowButton = true;
            this.efDevGrid_INQ.ShowDeleteRowButton = true;
            this.efDevGrid_INQ.ShowSelectedColumn = true;
        }

        private void FormMM00SI02_EF_DO_F6(object sender, EF.EF_Args e)
        {
            //存放原来选定的行的行号 
            int i_focusedRow = Math.Max(this.gridView_INQ.FocusedRowHandle, 0);

            //维护事件数据 
            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            DataTable dataTable = this.efDevGrid_INQ.DataSource as DataTable;

            DataTable del = null;
            DataTable upd = dataTable.Clone();
            DataTable ins = dataTable.Clone();

            for (int i = 0; i < this.gridView_INQ.RowCount; i++)
            {
                if (this.efDevGrid_INQ.GetSelectedColumnChecked(i))
                {
                    if (this.gridView_INQ.GetDataRow(i).RowState == DataRowState.Added)
                    {
                        ins.Rows.Add(this.gridView_INQ.GetDataRow(i).ItemArray);
                    }
                    else if (this.gridView_INQ.GetDataRow(i).RowState == DataRowState.Modified)
                    {
                        upd.Rows.Add(this.gridView_INQ.GetDataRow(i).ItemArray);
                    }
                }
            }
            del = dataTable.GetChanges(DataRowState.Deleted);

            if (ins != null && ins.Rows.Count > 0)
            {
                ins.TableName = "MM00SI02_INS";
                inBlock.Tables.Add(ins);
            }
            if (del != null && del.Rows.Count > 0)
            {
                del.RejectChanges();
                del.TableName = "MM00SI02_DEL";
                inBlock.Tables.Add(del);
            }
            if (upd != null && upd.Rows.Count > 0)
            {
                upd.TableName = "MM00SI02_UPD";
                inBlock.Tables.Add(upd);
            }

            //判断是否有增删改的数据
            if (inBlock.Tables.IndexOf("MM00SI02_INS") < 0
             && inBlock.Tables.IndexOf("MM00SI02_DEL") < 0
             && inBlock.Tables.IndexOf("MM00SI02_UPD") < 0)
            {
                this.EFMsgInfo = "没有增删改的记录。";
                this.ef_args.buttonStatusHold = true;
                return;
            }

            //调用维护SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm00si02f6_pro", inBlock);
            this.EFSysInfo = outBlock.sys_info;

            //判断调用是否正确
            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            //设置GRID增删按钮属性
            this.efDevGrid_INQ.SetAllColumnEditable(false);
            this.efDevGrid_INQ.ShowAddCopyRowButton = false;
            this.efDevGrid_INQ.ShowAddRowButton = false;
            this.efDevGrid_INQ.ShowDeleteRowButton = false;
            this.efDevGrid_INQ.ShowSelectedColumn = false;

            //刷新页面,查询参数信息
            this.FormMM00SI02_EF_DO_F2(null, null);

            //取消系统默认选定的当前行
            this.gridView_INQ.UnselectRow(this.gridView_INQ.FocusedRowHandle);

            //选定行在原来的位置
            this.gridView_INQ.FocusedRowHandle = Math.Min(i_focusedRow, this.gridView_INQ.RowCount);

            //当前行被选中
            this.efDevGrid_INQ.SetSelectedColumnChecked(this.gridView_INQ.FocusedRowHandle, true);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }

        private void FormMM00SI02_EF_CANCEL_DO_F6(object sender, EF.EF_Args e)
        {
            //设置grid 增删按钮属性
            this.efDevGrid_INQ.SetAllColumnEditable(false);
            this.efDevGrid_INQ.ShowAddCopyRowButton = false;
            this.efDevGrid_INQ.ShowAddRowButton = false;
            this.efDevGrid_INQ.ShowDeleteRowButton = false;
            this.efDevGrid_INQ.ShowSelectedColumn = false;

            //刷新页面 查询参数信息
            this.FormMM00SI02_EF_DO_F2(null, null);
        }
        #endregion

    }
}
