using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MM
{
    public partial class FormMM0001D2S2N : EF.EFFormMain
    {
        public FormMM0001D2S2N()
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
            //查询 PONO
        }
        #endregion

        #region 窗体加载 FormMM0001D2_Load
        private void FormMM0001D2_Load(object sender, EventArgs e)
        {
            //设置分区参数
            cs_formPartition = this.ef_args.formPartition;

            outBlock_Grid = (EI.EIInfo)EF.EF_Args.common_object_1;

            if (EF.EF_Args.common_parameter_1 == "F10")//管理封锁
            {
                //设置画面标题
                this.Text = "管理封锁";
                
                //设置efDevGrid1的动态配置列
                EF.Utility.SetSingleGridColumn(efDevGrid1, EF.EF_Args.common_parameter_2.Trim() + "_F10");
            }
            if (EF.EF_Args.common_parameter_1 == "F11")//管理释放
            {
                //设置画面标题
                this.Text = "管理释放";

                EF.Utility.SetSingleGridColumn(efDevGrid1, EF.EF_Args.common_parameter_2.Trim() + "_F11");
            }

            if (EF.EF_Args.common_parameter_1 == "F12")//去向变更
            {
                //设置画面标题
                this.Text = "去向变更";

                EF.Utility.SetSingleGridColumn(efDevGrid1, EF.EF_Args.common_parameter_2.Trim() + "_F12");
            }

            //设置默认值
            (EFX.EFCGrid.GetEFCGridBase(efDevGrid1) as EFX.EFCGridImp.SingleDev.EFCGridSingleDev).ResetGridValue();

            //获取传入参数
            EF.Utility.SetSingleGridValue(efDevGrid1, outBlock_Grid, 0);

            ////根据ED54配置的Grid大小调整画面的高度和宽度
            //GC.PM_utility2.DEV_AdjustFormMainWidthAndHeightByGrid(efDevGrid1, this);

        }
        #endregion

        #region 确定操作 efButton1_Click
        private void efButton1_Click(object sender, EventArgs e)
        {
            //必须项的内容校验。 
            if (!EFX.EFCGrid.GetEFCGridBase(efDevGrid1).ValidateGridDataEx())
            { //若校验失败，则直接离开。
                EFMsgInfo = "请输入必输信息。";
                return;
            } 
            
            EI.EIInfo inBlock = new EI.EIInfo();

            inBlock.Tables.Clear(); 
            inBlock.Tables.Add(EF.Utility.GetSingleGridValue(efDevGrid1).Tables[0].Copy());
            
            if (EF.EF_Args.common_parameter_1 == "F10")//管理封锁
            {
                //设置传入材料号
                inBlock.Tables.Add(outBlock_Grid.Tables[0].Copy());
                inBlock.Tables[0].Columns.Add("MAT_KIND");  //物料种类
                inBlock.Tables[0].Columns.Add("TABLE_ENAME");  //表名
                inBlock.Tables[0].Columns.Add("MAT_LINE_TYPE");  //产线类型

                inBlock.Tables[0].Rows[0]["MAT_KIND"] = EF.EF_Args.common_parameter_3.Trim();
                inBlock.Tables[0].Rows[0]["TABLE_ENAME"] = EF.EF_Args.common_parameter_4.Trim();
                inBlock.Tables[0].Rows[0]["MAT_LINE_TYPE"] = EF.EF_Args.common_parameter_5.Trim();                

                //调用查询Service
                EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0001d1f10_pro", inBlock);
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

                //如果新增成功，更新Grid中的数据
                if (outBlock.sys_info.flag == 0)
                {
                    EFMsgInfo = "操作成功！";
                    this.Close();
                }
                else
                {
                    //显示执行结果信息
                    MessageBox.Show(this.EFMsgInfo);
                }
            }
            if (EF.EF_Args.common_parameter_1 == "F11")//管理释放
            {
                //设置传入材料号
                inBlock.Tables.Add(outBlock_Grid.Tables[0].Copy());
                inBlock.Tables[0].Columns.Add("MAT_KIND");  //物料种类
                inBlock.Tables[0].Columns.Add("TABLE_ENAME");  //表名
                inBlock.Tables[0].Columns.Add("MAT_LINE_TYPE");  //产线类型

                inBlock.Tables[0].Rows[0]["MAT_KIND"] = EF.EF_Args.common_parameter_3.Trim();
                inBlock.Tables[0].Rows[0]["TABLE_ENAME"] = EF.EF_Args.common_parameter_4.Trim();
                inBlock.Tables[0].Rows[0]["MAT_LINE_TYPE"] = EF.EF_Args.common_parameter_5.Trim(); 
                //调用查询Service
                EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0001d1f11_pro", inBlock);
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

                //如果新增成功，更新Grid中的数据
                if (outBlock.sys_info.flag == 0)
                {
                    EFMsgInfo = "操作成功！";
                    this.Close();
                }
                else
                {
                    //显示执行结果信息
                    MessageBox.Show(this.EFMsgInfo);
                }
            }

            if (EF.EF_Args.common_parameter_1 == "F12")//去向变更
            {
                //设置传入材料号
                inBlock.Tables.Add(outBlock_Grid.Tables[0].Copy());
                inBlock.Tables[0].Columns.Add("MAT_KIND");  //物料种类
                inBlock.Tables[0].Columns.Add("TABLE_ENAME");  //表名
                inBlock.Tables[0].Columns.Add("MAT_LINE_TYPE");  //产线类型

                inBlock.Tables[0].Rows[0]["MAT_KIND"] = EF.EF_Args.common_parameter_3.Trim();
                inBlock.Tables[0].Rows[0]["TABLE_ENAME"] = EF.EF_Args.common_parameter_4.Trim();
                inBlock.Tables[0].Rows[0]["MAT_LINE_TYPE"] = EF.EF_Args.common_parameter_5.Trim();
                //调用查询Service
                EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0001d1f12_pro", inBlock);
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

                //如果新增成功，更新Grid中的数据
                if (outBlock.sys_info.flag == 0)
                {
                    EFMsgInfo = "操作成功！";
                    this.Close();
                }
                else
                {
                    //显示执行结果信息
                    MessageBox.Show(this.EFMsgInfo);
                }
            }
        }
        #endregion

        #region 退出 efButton2_Click
        private void efButton2_Click(object sender, EventArgs e)
        {
            EF.EF_Args.common_object_1 = null;
            EF.EF_Args.common_parameter_1 = "";
            EF.EF_Args.common_parameter_2 = "";
            EF.EF_Args.common_parameter_3 = "";
            EF.EF_Args.common_parameter_4 = "";
            this.Close();
        }
        #endregion
    }
}
