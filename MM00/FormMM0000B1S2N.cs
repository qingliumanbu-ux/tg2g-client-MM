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
    public partial class FormMM0000B1S2N : EF.EFForm
    {
        public FormMM0000B1S2N()
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
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0000b1f2_inq", inBlock);
            if (outBlock.sys_info.flag < 0)
            {
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                return;
            }

            //清空GRID的数据
            //EFX.EFCGrid.GetEFCGridBase(this.efDevGrid_MMS).ClearGridData();
            //EFX.EFCGrid.GetEFCGridBase(this.efDevGrid_PES).ClearGridData();

            //将信息压入指定的GRID的DataSource
            this.efDevGrid_MMS.DataSource = null;
            this.gridView_MMS.Columns.Clear();
            this.efDevGrid_MMS.DataSource = outBlock.Tables["MMS"];

            this.efDevGrid_PES.DataSource = null;
            this.gridView_PES.Columns.Clear();
            this.efDevGrid_PES.DataSource = outBlock.Tables["PES"];

            //恢复光标为箭头
            this.Cursor = System.Windows.Forms.Cursors.Default;

            //设置列字段自动调节宽度
            this.efDevGrid_MMS.SetColumnsWidthAuto();
            this.efDevGrid_PES.SetColumnsWidthAuto();

            this.EFMsgInfo = string.Format(GC.GCRS.GCRSC0000033/*查询到[{0}]条记录。*/, outBlock.blk_info[outBlock.blk_now].Row);
        }
        #endregion

        #region 画面载入 FormMM0000B1_Load
        private void FormMM0000B1_Load(object sender, EventArgs e)
        {
            //设置分区参数
            cs_formPartition = this.ef_args.formPartition;

            //设置Grid的ED54配置列
            //EF.Utility.SetGridColumn(new EF.EFDevGrid[] { this.efDevGrid_MMS, this.efDevGrid_PES },
            //    new string[] { "MM0000B1_INQ_MMS", "MM0000B1_INQ_PES" }, cs_formPartition);

            //设置Grid的ED54配置列 单记录模式
            EFX.EFCGrid.InitSingleGridColumn(this.efDevGrid_QUERY, "MM0000B1_QUERY", cs_formPartition);

            //EPED54配置模式的查询条件的GROUP的尺寸大小控制
            GC.PM_utility2.DEV_Init_LayoutGroup_where_size(this.layoutControlGroup2, this.efDevGrid_QUERY);
        }
        #endregion

        #region F2 查询
        private void FormMM0000B1_EF_DO_F2(object sender, EF.EF_Args e)
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


    }
}
