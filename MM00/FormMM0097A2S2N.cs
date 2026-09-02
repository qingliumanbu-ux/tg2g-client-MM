using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MM
{
    public partial class FormMM0097A2S2N : EF.EFFormMain
    {
        public FormMM0097A2S2N()
        {
            InitializeComponent();
        }

        #region <自定义程序用全局变量>
        // 当前分区
        private string cs_formPartition = "";
        // 传入参数 Grid
        EI.EIInfo outBlock_Grid = new EI.EIInfo();
        #endregion

        #region 绑定下拉框内容 BindDataSource()
        private void BindDataSource()
        {
            ////查询 PONO
            //string sqlCode = string.Format("SELECT DISTINCT  PONO AS CODE FROM TQMTQQ0 ORDER BY  PONO  ASC");
            //EI.EIInfo outBlockCode = EF.Utility.ExecQueryPart(cs_formPartition,sqlCode);
            //if (EF.Utility.IsBlockHasError(outBlockCode, this))
            //    return;
            //outBlockCode.Tables[0].Columns[0].Caption = "制造命令号";
            //(EFX.EFCGrid.GetEFCGridBase(efDevGrid_MatInfo).Columns["PONO"]).SetPopupGridDataSource(outBlockCode.Tables[0], "CODE", new string[] {  });
        }
        #endregion
        
        #region 窗体加载 FormMM0097A2_Load
        private void FormMM0097A2_Load(object sender, EventArgs e)
        {    
            //设置分区参数
            cs_formPartition = EF.EF_Args.common_parameter_2;

            outBlock_Grid = (EI.EIInfo)EF.EF_Args.common_object_1;
            
            //将材料信息压入指定的GRID的DataSource 便于后台传入
            this.efDevGrid_97.DataSource = outBlock_Grid.Tables[0];

            if (EF.EF_Args.common_parameter_1 == "F7")  //事件复制
            {
                //设置画面标题
                this.Text = "复制事件";

                //设置efDevGrid_InPut的动态配置列
                EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efDevGrid_97 }, new string[] { "MM0097A1_97" },cs_formPartition);

                //设置efDevGrid_InPut的动态配置列
                EF.Utility.SetSingleGridColumn(efDevGrid_InPut, "MM0097A1_F7",cs_formPartition);

                // 绑定下拉框内容
                BindDataSource();

                //获取传入参数
                EF.Utility.SetCustomGridValue(this.efDevGrid_97, outBlock_Grid, false);
                EFX.EFCGrid.GetEFCGridBase(this.efDevGrid_InPut).SetGridValue(outBlock_Grid);

                //设置实绩录入区 默认值
                (EFX.EFCGrid.GetEFCGridBase(efDevGrid_InPut) as EFX.EFCGridImp.SingleDev.EFCGridSingleDev).ResetGridValue();

                //设置grid列是否可编辑
                this.efDevGrid_97.SetAllColumnReadOnly(true);
                //设置列字段自动调节宽度
                this.efDevGrid_97.SetColumnsWidthAuto();
            }
        }
        #endregion

        #region 确定操作 efButton1_Click
        private void efButton1_Click(object sender, EventArgs e)
        {

            //必须项的内容校验。 
            if (!EFX.EFCGrid.GetEFCGridBase(efDevGrid_InPut).ValidateGridDataEx())
            { //若校验失败，则直接离开。
                this.EFMsgInfo = "请输入必输信息。";
                return;
            }
            
            EI.EIInfo inBlock = new EI.EIInfo();
            inBlock.Tables.Clear();

            //将新增事件放在第0块
            inBlock.Tables.Add(EF.Utility.GetSingleGridValue(efDevGrid_InPut).Tables[0].Copy());

            //将参考事件放在第1块
            inBlock.Tables.Add();
            DataTable dataTable_Info = this.efDevGrid_97.DataSource as DataTable;
            inBlock.Tables[1].Merge(dataTable_Info);


            if (EF.EF_Args.common_parameter_1 == "F7")  //事件复制
            {
                //调用查询Service
                EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097a1f7_pro", inBlock);
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

                //设置调用出错信息
                if (outBlock.sys_info.flag < 0)
                {
                    MessageBox.Show(this.EFMsgInfo);
                    return;
                }
            }           
            
            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
            this.Close();
        }
        #endregion

        #region 退出 efButton2_Click
        private void efButton2_Click(object sender, EventArgs e)
        {
            EF.EF_Args.common_object_1 = null;
            EF.EF_Args.common_parameter_1 = "";
            EF.EF_Args.common_parameter_2 = "";
            this.Close();
        }
        #endregion
    }
}
