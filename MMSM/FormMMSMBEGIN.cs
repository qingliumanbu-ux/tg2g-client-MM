using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Collections;

namespace MM
{
    public partial class FormMMSMBEGIN : EF.EFForm
    {
        // 声明变量
        private BE2.FormConfig.FormConfigHelper formConfigHelper;
        private BE2.Common.LayoutDataChecker layoutDataCheckerMain = new BE2.Common.LayoutDataChecker();
        private string districtName = "SGCXM";
        private DataSet DS;
        private BindingSource tTMMSMBGBS;
        private BindingSource tTMMSMBGEBS;
        Thread aThread;
        delegate void SetTextCallback(string text);

        private System.Collections.ArrayList Error_S0001 = new System.Collections.ArrayList();//卷号重复
        private System.Collections.ArrayList Error_S0002 = new System.Collections.ArrayList();//炉号错误
        private System.Collections.ArrayList Error_S0003 = new System.Collections.ArrayList();//外购材料号为空
        private System.Collections.ArrayList Error_S0004 = new System.Collections.ArrayList();//外购材料号重复
        private System.Collections.ArrayList Error_S0005 = new System.Collections.ArrayList();//厚度错误
        private System.Collections.ArrayList Error_S0006 = new System.Collections.ArrayList();//宽度错误
        private System.Collections.ArrayList Error_S0007 = new System.Collections.ArrayList();//长度错误
        private System.Collections.ArrayList Error_S0008 = new System.Collections.ArrayList();//实际厚度错误
        private System.Collections.ArrayList Error_S0009 = new System.Collections.ArrayList();//实际宽度错误
        private System.Collections.ArrayList Error_S0010 = new System.Collections.ArrayList();//实际长度错误
        private System.Collections.ArrayList Error_S0011 = new System.Collections.ArrayList();//卷厚度错误
        private System.Collections.ArrayList Error_S0012 = new System.Collections.ArrayList();//卷实际厚度错误
        private System.Collections.ArrayList Error_S0013 = new System.Collections.ArrayList();//件数为0
        private System.Collections.ArrayList Error_S0014 = new System.Collections.ArrayList();//卷、中间坯件数不为1
        private System.Collections.ArrayList Error_S0015 = new System.Collections.ArrayList();//合同不存在
        private System.Collections.ArrayList Error_S0016 = new System.Collections.ArrayList();//无理重
        private System.Collections.ArrayList Error_S0017 = new System.Collections.ArrayList();//已称重但无实重
        private System.Collections.ArrayList Error_S0018 = new System.Collections.ArrayList();//包装类型不存在
        private System.Collections.ArrayList Error_S0019 = new System.Collections.ArrayList();//内部钢种不存在
        private System.Collections.ArrayList Error_S0020 = new System.Collections.ArrayList();//生产时刻错误
        private System.Collections.ArrayList Error_S0021 = new System.Collections.ArrayList();//牌号未维护
        private System.Collections.ArrayList Error_S0022 = new System.Collections.ArrayList();//产品规范码为空
        private System.Collections.ArrayList Error_S0023 = new System.Collections.ArrayList();//库号为空
        private System.Collections.ArrayList Error_S0024 = new System.Collections.ArrayList();//跺位为空
        private System.Collections.ArrayList Error_S0025 = new System.Collections.ArrayList();//入库时刻错误
        private System.Collections.ArrayList Error_S0026 = new System.Collections.ArrayList();//标准错误

        public FormMMSMBEGIN()
        {
            InitializeComponent();
        }

        #region 画面加载事件
        private void FormMMSMBEGIN_Load(object sender, EventArgs e)
        {
            // 初始化画面
            InitializePage();
        }
        #endregion

        #region F2 查询
        private void FormMMSMBEGIN_EF_DO_F2(object sender, EF.EF_Args e)
        {
            Query();
        }
        #endregion

        #region F3 导入
        private void FormMMSMBEGIN_EF_PRE_DO_F3(object sender, EF.EF_Args e)
        {

        }

        private void FormMMSMBEGIN_EF_DO_F3(object sender, EF.EF_Args e)
        {
            efDevListBox1.Items.Clear();
            if (tabbedControlGroup1.SelectedTabPage == this.layoutControlGroupELE)
            {
                aThread = new Thread(eleimport);
            }
            else
            {
                aThread = new Thread(import);
            }
            aThread.Start();
        }

        private void FormMMSMBEGIN_EF_CANCEL_DO_F3(object sender, EF.EF_Args e)
        {

        }
        #endregion

        #region F4 暂停
        private void FormMMSMBEGIN_EF_DO_F4(object sender, EF.EF_Args e)
        {
            if (aThread.IsAlive)
            {
                aThread.Interrupt();
            }
            else
            {
                aThread.Resume();
            }
        }
        #endregion

        #region F5 停止
        private void FormMMSMBEGIN_EF_PRE_DO_F5(object sender, EF.EF_Args e)
        {
            
        }

        private void FormMMSMBEGIN_EF_DO_F5(object sender, EF.EF_Args e)
        {
            aThread.Abort();
        }

        private void FormMMSMBEGIN_EF_CANCEL_DO_F5(object sender, EF.EF_Args e)
        {

        }
        #endregion

        #region F10 数据验证
        private void FormMMSMBEGIN_EF_PRE_DO_FA(object sender, EF.EF_Args e)
        {

        }

        private void FormMMSMBEGIN_EF_DO_FA(object sender, EF.EF_Args e)
        {
            efDevListBox1.Items.Clear();
            if (tabbedControlGroup1.SelectedTabPage==this.layoutControlGroupELE)
            {
                aThread = new Thread(eledatavalidation);
            }
            else
            {
                aThread = new Thread(datavalidation);
            }
            aThread.Start();
        }

        private void FormMMSMBEGIN_EF_CANCEL_DO_FA(object sender, EF.EF_Args e)
        {

        }
        #endregion

        #region 初始化方法InitializePage
        private void InitializePage()
        {
            // 初始化画面显示配置
            formConfigHelper = new BE2.FormConfig.FormConfigHelper(districtName, "MMSMBEGIN", string.Empty, "DataSet_SM001", "TMMSMBG,TMMSMBGE");

            DS = formConfigHelper.FormDataSet;

            tTMMSMBGBS = BE2.FormConfig.Utility.CreatBindingSource(DS, "TMMSMBG");
            tTMMSMBGEBS = BE2.FormConfig.Utility.CreatBindingSource(DS, "TMMSMBGE");

            //加载界面配置
            formConfigHelper.LoadGridView(this, "gridViewMat", this.efDevGrid_MAT, this.gridView_MAT, tTMMSMBGBS, layoutControlGroup2);
            formConfigHelper.LoadGridView(this, "gridViewEle", this.efDevGrid_ELE, this.gridView_ELE, tTMMSMBGEBS, layoutControlGroup2);

        }
        #endregion

        #region  自定义函数-查询
        private void Query()
        {
            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            //调用SERVICE
            outBlock = BE2.Common.EpesInfo.CallService(districtName, "mmsmbg_ing", inBlock);

            //设置返回信息
            EI.EIInfo.eiinfo_sys s = outBlock.GetSys();
            if (s.flag != 0)
            {
                this.EFMsgInfo = outBlock.sys_info.msg +
                    outBlock.sys_info.sqlcode +
                    outBlock.sys_info.sqlmes;
                EF.EFMessageBox.Show(this.EFMsgInfo, EF.EF_Args.epEname, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DS.Tables["TMMSMBG"].Rows.Clear();

            if (outBlock.blk_info[outBlock.blk_now].Row > 0)
            {
                this.EFMsgInfo = "查询成功!";

                DS.Tables["TMMSMBG"].Rows.Clear();
                outBlock.SetBlkName(1, "TMMSMBG");
                outBlock.ConvertToStrongType(this.DS);
                this.DS.AcceptChanges();
            }
            else
            {
                this.EFMsgInfo = "没有查询到符合条件的记录!";
            }

            // 恢复光标为箭头;
            this.Cursor = System.Windows.Forms.Cursors.Default;

            // 结束画面编辑状态;
            this.tTMMSMBGBS.EndEdit();


        }
        #endregion

        #region 进程安全用_设置信息输出控件
        private void SetText(string text)
        {
            if (aThread.IsAlive)
            {
                if (this.efDevListBox1.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    this.efDevListBox1.Invoke(d, new object[] { text });
                }
                else
                {
                    efDevListBox1.Items.Add(text);
                    efDevListBox1.SelectedIndex = efDevListBox1.Items.Count - 1;
                }
            }
        }
        #endregion

        #region 库存导入方法
        private void import()
        {            
            if (this.efDevGrid_MAT.EFChoiceCount == 0)
            {
                MessageBox.Show("请勾选要导入的库存数据！");
                return;
            }

            EI.EIInfo inBlock = new EI.EIInfo();
            DataRow dr;
            
            DataTable tableimport = this.efDevGrid_MAT.GetSelectedDataRow();
            DataTable dtimport = tableimport.Clone();

            progressBar_Main.Value = 0;
            progressBar_Main.Minimum = 0;
            progressBar_Main.Maximum = tableimport.Rows.Count;

            DateTime start_time = System.DateTime.Now;
            this.SetText("开始时刻:" + start_time.ToString("yyyyMMddHHmmss"));
            for (int i = 0; i < tableimport.Rows.Count; i++)
            {
                this.SetText("导入第"+(i+1)+"材料[" + tableimport.Rows[i]["MAT_NO"].ToString() + "]开始...");
                inBlock.Tables.Clear();
                dtimport.Rows.Clear();
                dr = tableimport.Rows[i];                
                dtimport.Rows.Add(dr.ItemArray);
                inBlock.Tables.Add(dtimport);
                progressBar_Main.Value++;
                EI.EIInfo outBlock = EI.EITuxedo.CallService("mmsmbg_import", inBlock);
                this.EFMsgInfo = outBlock.GetSys().msg;
                if (outBlock.sys_info.flag != 0)
                {
                    this.SetText("导入材料[" + tableimport.Rows[i]["MAT_NO"].ToString() + "]异常!" + outBlock.GetSys().msg);
                    //aThread.Interrupt();
                }
            }
            DateTime end_time = System.DateTime.Now;
            this.SetText("结束时刻:" + end_time.ToString("yyyyMMddHHmmss"));
            this.SetText("耗时:" + (end_time - start_time).Minutes.ToString() + "分" + (end_time - start_time).Seconds.ToString() + "秒!");
            this.SetText("导入完毕(〃'▽'〃)");
            setvaliinfo(efDevListBox1,"MAT_import");
        }
        #endregion

        #region 成分导入方法
        private void eleimport()
        {
            if (this.efDevGrid_ELE.EFChoiceCount == 0)
            {
                MessageBox.Show("请勾选要导入的成分数据！");
                return;
            }

            EI.EIInfo inBlock = new EI.EIInfo();
            DataRow dr;

            DataTable tableimport = this.efDevGrid_ELE.GetSelectedDataRow();
            DataTable dtimport = tableimport.Clone();

            progressBar_Main.Value = 0;
            progressBar_Main.Minimum = 0;
            progressBar_Main.Maximum = tableimport.Rows.Count;

            DateTime start_time = System.DateTime.Now;
            this.SetText("开始时刻:" + start_time.ToString("yyyyMMddHHmmss"));
            for (int i = 0; i < tableimport.Rows.Count; i++)
            {
                this.SetText("导入第" + (i + 1) + "炉次[" + tableimport.Rows[i]["HEAT_NO"].ToString() + "]开始...");
                inBlock.Tables.Clear();
                dtimport.Rows.Clear();
                dr = tableimport.Rows[i];
                dtimport.Rows.Add(dr.ItemArray);
                inBlock.Tables.Add(dtimport);
                progressBar_Main.Value++;
                EI.EIInfo outBlock = EI.EITuxedo.CallService("mmsmbg_import_ele", inBlock);
                this.EFMsgInfo = outBlock.GetSys().msg;
                if (outBlock.sys_info.flag != 0)
                {
                    this.SetText("导入材料[" + tableimport.Rows[i]["HEAT_NO"].ToString() + "]异常!" + outBlock.GetSys().msg);
                    //aThread.Interrupt();
                }
            }
            DateTime end_time = System.DateTime.Now;
            this.SetText("结束时刻:" + end_time.ToString("yyyyMMddHHmmss"));
            this.SetText("耗时:" + (end_time - start_time).Minutes.ToString() + "分" + (end_time - start_time).Seconds.ToString() + "秒!");
            this.SetText("导入完毕(〃'▽'〃)");
            setvaliinfo(efDevListBox1, "ELE_import");
        }
        #endregion

        #region 库存数据验证方法
        private void datavalidation()
        {
            if (this.efDevGrid_MAT.EFChoiceCount == 0)
            {
                MessageBox.Show("请勾选要导入的库存数据！");
                return;
            }
            int total_num = 0;
            int error_num = 0;
            EI.EIInfo inBlock = new EI.EIInfo();
            DataRow dr;

            DataTable tableimport = this.efDevGrid_MAT.GetSelectedDataRow();
            DataTable dtimport = tableimport.Clone();

            progressBar_Main.Value = 0;
            progressBar_Main.Minimum = 0;
            progressBar_Main.Maximum = tableimport.Rows.Count;

            DateTime start_time = System.DateTime.Now;
            this.SetText("开始时刻:" + start_time.ToString("yyyyMMddHHmmss"));
            for (int i = 0; i < tableimport.Rows.Count; i++)
            {
                this.SetText("数据验证：第" + (i + 1) + "材料[" + tableimport.Rows[i]["MAT_NO"].ToString() + "]开始...");
                inBlock.Tables.Clear();
                dtimport.Rows.Clear();
                dr = tableimport.Rows[i];
                dtimport.Rows.Add(dr.ItemArray);
                inBlock.Tables.Add(dtimport);
                progressBar_Main.Value++;
                EI.EIInfo outBlock = EI.EITuxedo.CallService("mmsmbg_vali", inBlock);
                this.EFMsgInfo = outBlock.GetSys().msg;
                if (outBlock.sys_info.flag != 0)
                {
                    this.SetText("数据验证材料[" + tableimport.Rows[i]["MAT_NO"].ToString() + "]异常!" + outBlock.GetSys().msg);
                }
                else
                {
                    if (outBlock.Tables[0].Rows[0]["ERRO_FLAG"].ToString() != "0")
                    {
                        error_num++;

                        string error_info = "";
                        string error_key = "";
                        error_info = outBlock.Tables[0].Rows[0]["ERRO_INFO"].ToString();

                        #region 错误汇总
                        if (error_info.Contains("[S0001]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0001]") + 7,
                                error_info.IndexOf("[E0001]") - error_info.IndexOf("[S0001]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0001.Count; j++)
                            {
                                if (Error_S0001[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0001.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0002]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0002]") + 7,
                                error_info.IndexOf("[E0002]") - error_info.IndexOf("[S0002]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0002.Count; j++)
                            {
                                if (Error_S0002[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0002.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0003]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0003]") + 7,
                                error_info.IndexOf("[E0003]") - error_info.IndexOf("[S0003]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0003.Count; j++)
                            {
                                if (Error_S0003[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0003.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0004]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0004]") + 7,
                                error_info.IndexOf("[E0004]") - error_info.IndexOf("[S0004]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0004.Count; j++)
                            {
                                if (Error_S0004[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0004.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0005]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0005]") + 7,
                                error_info.IndexOf("[E0005]") - error_info.IndexOf("[S0005]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0005.Count; j++)
                            {
                                if (Error_S0005[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0005.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0006]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0006]") + 7,
                                error_info.IndexOf("[E0006]") - error_info.IndexOf("[S0006]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0006.Count; j++)
                            {
                                if (Error_S0006[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0006.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0007]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0007]") + 7,
                                error_info.IndexOf("[E0007]") - error_info.IndexOf("[S0007]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0007.Count; j++)
                            {
                                if (Error_S0007[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0007.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0008]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0008]") + 7,
                                error_info.IndexOf("[E0008]") - error_info.IndexOf("[S0008]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0008.Count; j++)
                            {
                                if (Error_S0008[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0008.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0009]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0009]") + 7,
                                error_info.IndexOf("[E0009]") - error_info.IndexOf("[S0009]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0009.Count; j++)
                            {
                                if (Error_S0009[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0009.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0010]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0010]") + 7,
                                error_info.IndexOf("[E0010]") - error_info.IndexOf("[S0010]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0010.Count; j++)
                            {
                                if (Error_S0010[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0010.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0011]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0011]") + 7,
                                error_info.IndexOf("[E0011]") - error_info.IndexOf("[S0011]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0011.Count; j++)
                            {
                                if (Error_S0011[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0011.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0012]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0012]") + 7,
                                error_info.IndexOf("[E0012]") - error_info.IndexOf("[S0012]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0012.Count; j++)
                            {
                                if (Error_S0012[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0012.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0013]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0013]") + 7,
                                error_info.IndexOf("[E0013]") - error_info.IndexOf("[S0013]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0013.Count; j++)
                            {
                                if (Error_S0013[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0013.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0014]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0014]") + 7,
                                error_info.IndexOf("[E0014]") - error_info.IndexOf("[S0014]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0014.Count; j++)
                            {
                                if (Error_S0014[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0014.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0015]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0015]") + 7,
                                error_info.IndexOf("[E0015]") - error_info.IndexOf("[S0015]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0015.Count; j++)
                            {
                                if (Error_S0015[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0015.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0016]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0016]") + 7,
                                error_info.IndexOf("[E0016]") - error_info.IndexOf("[S0016]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0016.Count; j++)
                            {
                                if (Error_S0016[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0016.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0017]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0017]") + 7,
                                error_info.IndexOf("[E0017]") - error_info.IndexOf("[S0017]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0017.Count; j++)
                            {
                                if (Error_S0017[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0017.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0018]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0018]") + 7,
                                error_info.IndexOf("[E0018]") - error_info.IndexOf("[S0018]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0018.Count; j++)
                            {
                                if (Error_S0018[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0018.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0019]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0019]") + 7,
                                error_info.IndexOf("[E0019]") - error_info.IndexOf("[S0019]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0019.Count; j++)
                            {
                                if (Error_S0019[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0019.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0020]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0020]") + 7,
                                error_info.IndexOf("[E0020]") - error_info.IndexOf("[S0020]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0020.Count; j++)
                            {
                                if (Error_S0020[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0020.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0021]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0021]") + 7,
                                error_info.IndexOf("[E0021]") - error_info.IndexOf("[S0021]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0021.Count; j++)
                            {
                                if (Error_S0021[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0021.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S00220]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0022]") + 7,
                                error_info.IndexOf("[E0022]") - error_info.IndexOf("[S0022]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0022.Count; j++)
                            {
                                if (Error_S0022[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0022.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0023]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0023]") + 7,
                                error_info.IndexOf("[E0023]") - error_info.IndexOf("[S0023]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0023.Count; j++)
                            {
                                if (Error_S0023[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0023.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0024]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0024]") + 7,
                                error_info.IndexOf("[E0024]") - error_info.IndexOf("[S0024]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0024.Count; j++)
                            {
                                if (Error_S0024[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0024.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0025]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0025]") + 7,
                                error_info.IndexOf("[E0025]") - error_info.IndexOf("[S0025]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0025.Count; j++)
                            {
                                if (Error_S0025[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0025.Add(error_key);
                            }
                        }

                        if (error_info.Contains("[S0026]"))
                        {
                            error_key = error_info.Substring(error_info.IndexOf("[S0026]") + 7,
                                error_info.IndexOf("[E0026]") - error_info.IndexOf("[S0026]") - 7);

                            int err_flag = 0;
                            for (int j = 0; j < Error_S0026.Count; j++)
                            {
                                if (Error_S0026[j].ToString() == error_key)
                                {
                                    err_flag = 1;
                                    break;
                                }
                            }
                            if (err_flag == 0)
                            {
                                Error_S0026.Add(error_key);
                            }
                        }
                        #endregion
                    }
                    this.SetText("数据验证材料[" + tableimport.Rows[i]["MAT_NO"].ToString() + "]结果:" + outBlock.Tables[0].Rows[0]["ERRO_FLAG"].ToString());
                    this.SetText("信息:" + outBlock.Tables[0].Rows[0]["ERRO_INFO"].ToString());
                }
                total_num++;
            }
            this.SetText("验证错误总条数:" + error_num.ToString());
            this.SetText("错误汇总:");
            this.SetText("数据重复汇总(条数:" + Error_S0001.Count + "):");
            for (int j = 0; j < Error_S0001.Count; j++)
            {
                this.SetText("材料号：" + Error_S0001[j].ToString());
            }
            this.SetText("炉号为空汇总(条数:" + Error_S0002.Count + "):");
            for (int j = 0; j < Error_S0002.Count; j++)
            {
                this.SetText("材料号：" + Error_S0002[j].ToString());
            }
            this.SetText("外购材料号为空汇总(条数:" + Error_S0003.Count + "):");
            for (int j = 0; j < Error_S0003.Count; j++)
            {
                this.SetText("材料号：" + Error_S0003[j].ToString());
            }
            this.SetText("外购材料号重复汇总(条数:" + Error_S0004.Count + "):");
            for (int j = 0; j < Error_S0004.Count; j++)
            {
                this.SetText("材料号：" + Error_S0004[j].ToString());
            }
            this.SetText("厚度错误汇总(条数:" + Error_S0005.Count + "):");
            for (int j = 0; j < Error_S0005.Count; j++)
            {
                this.SetText("材料号：" + Error_S0005[j].ToString());
            }
            this.SetText("宽度错误汇总(条数:" + Error_S0006.Count + "):");
            for (int j = 0; j < Error_S0006.Count; j++)
            {
                this.SetText("材料号：" + Error_S0006[j].ToString());
            }
            this.SetText("长度错误汇总(条数:" + Error_S0007.Count + "):");
            for (int j = 0; j < Error_S0007.Count; j++)
            {
                this.SetText("材料号：" + Error_S0007[j].ToString());
            }
            this.SetText("实际厚度错误汇总(条数:" + Error_S0008.Count + "):");
            for (int j = 0; j < Error_S0008.Count; j++)
            {
                this.SetText("材料号：" + Error_S0008[j].ToString());
            }
            this.SetText("实际宽度错误汇总(条数:" + Error_S0009.Count + "):");
            for (int j = 0; j < Error_S0009.Count; j++)
            {
                this.SetText("材料号：" + Error_S0009[j].ToString());
            }
            this.SetText("实际长度错误汇总(条数:" + Error_S0010.Count + "):");
            for (int j = 0; j < Error_S0010.Count; j++)
            {
                this.SetText("材料号：" + Error_S0010[j].ToString());
            }
            this.SetText("卷厚度大于60汇总(条数:" + Error_S0011.Count + "):");
            for (int j = 0; j < Error_S0011.Count; j++)
            {
                this.SetText("材料号：" + Error_S0011[j].ToString());
            }
            this.SetText("卷实际厚度大于60汇总(条数:" + Error_S0012.Count + "):");
            for (int j = 0; j < Error_S0012.Count; j++)
            {
                this.SetText("材料号：" + Error_S0012[j].ToString());
            }
            this.SetText("件数为0汇总(条数:" + Error_S0013.Count + "):");
            for (int j = 0; j < Error_S0013.Count; j++)
            {
                this.SetText("材料号：" + Error_S0013[j].ToString());
            }
            this.SetText("中间坯、卷件数不为1汇总(条数:" + Error_S0014.Count + "):");
            for (int j = 0; j < Error_S0014.Count; j++)
            {
                this.SetText("材料号：" + Error_S0014[j].ToString());
            }
            this.SetText("合同对照不存在合同汇总(条数:" + Error_S0015.Count + "):");
            for (int j = 0; j < Error_S0015.Count; j++)
            {
                this.SetText("合同号：" + Error_S0015[j].ToString());
            }
            this.SetText("无理重汇总(条数:" + Error_S0016.Count + "):");
            for (int j = 0; j < Error_S0016.Count; j++)
            {
                this.SetText("材料号：" + Error_S0016[j].ToString());
            }
            this.SetText("已称重无实重汇总(条数:" + Error_S0017.Count + "):");
            for (int j = 0; j < Error_S0017.Count; j++)
            {
                this.SetText("材料号：" + Error_S0017[j].ToString());
            }
            this.SetText("包装类型不存在汇总(条数:" + Error_S0018.Count + "):");
            for (int j = 0; j < Error_S0018.Count; j++)
            {
                this.SetText("包装类型：" + Error_S0018[j].ToString());
            }
            this.SetText("内部钢种未维护汇总(条数:" + Error_S0019.Count + "):");
            for (int j = 0; j < Error_S0019.Count; j++)
            {
                this.SetText("内部钢种：" + Error_S0019[j].ToString());
            }
            this.SetText("生产时刻格式错误汇总(条数:" + Error_S0020.Count + "):");
            for (int j = 0; j < Error_S0020.Count; j++)
            {
                this.SetText("材料号：" + Error_S0020[j].ToString());
            }
            this.SetText("牌号未维护汇总(条数:" + Error_S0021.Count + "):");
            for (int j = 0; j < Error_S0021.Count; j++)
            {
                this.SetText("牌号：" + Error_S0021[j].ToString());
            }
            this.SetText("产品规范码为空汇总(条数:" + Error_S0022.Count + "):");
            for (int j = 0; j < Error_S0022.Count; j++)
            {
                this.SetText("材料号：" + Error_S0022[j].ToString());
            }
            this.SetText("库号为空或未配置汇总(条数:" + Error_S0023.Count + "):");
            for (int j = 0; j < Error_S0023.Count; j++)
            {
                this.SetText("材料号：" + Error_S0023[j].ToString());
            }
            this.SetText("跺位为空或未配置汇总(条数:" + Error_S0024.Count + "):");
            for (int j = 0; j < Error_S0024.Count; j++)
            {
                this.SetText("材料号：" + Error_S0024[j].ToString());
            }
            this.SetText("入库时刻为空汇总(条数:" + Error_S0025.Count + "):");
            for (int j = 0; j < Error_S0025.Count; j++)
            {
                this.SetText("材料号：" + Error_S0025[j].ToString());
            }
            this.SetText("标准未维护汇总(条数:" + Error_S0026.Count + "):");
            for (int j = 0; j < Error_S0026.Count; j++)
            {
                this.SetText("材料号：" + Error_S0026[j].ToString());
            }

            this.SetText("数据验证完毕(〃'▽'〃)");
            DateTime end_time = System.DateTime.Now;
            this.SetText("结束时刻:" + end_time.ToString("yyyyMMddHHmmss"));
            this.SetText("耗时:" + (end_time - start_time).Minutes.ToString() + "分" + (end_time - start_time).Seconds.ToString() + "秒!");
            setvaliinfo(efDevListBox1, "MAT_valiinfo");
        }
        #endregion

        #region 成分数据验证方法
        private void eledatavalidation()
        {
            if (this.efDevGrid_ELE.EFChoiceCount == 0)
            {
                MessageBox.Show("请勾选要导入的成分数据！");
                return;
            }
            int total_num = 0;
            int error_num = 0;
            EI.EIInfo inBlock = new EI.EIInfo();
            DataRow dr;

            DataTable tableimport = this.efDevGrid_ELE.GetSelectedDataRow();
            DataTable dtimport = tableimport.Clone();

            progressBar_Main.Value = 0;
            progressBar_Main.Minimum = 0;
            progressBar_Main.Maximum = tableimport.Rows.Count;

            DateTime start_time = System.DateTime.Now;
            this.SetText("开始时刻:" + start_time.ToString("yyyyMMddHHmmss"));

            var sums2 = from emp in tableimport.Rows.Cast<DataRow>()
                        group emp by new {炉号=emp.Field<string>("HEAT_NO")} into g
                        select new { Peo = g.Key, Count = g.Count() };
            foreach (var item in sums2.Where(A => A.Count > 1))
            {
                this.SetText("重复炉次数据[" + item.Peo + "]...");
            }

            this.SetText("数据验证完毕(〃'▽'〃)");
            DateTime end_time = System.DateTime.Now;
            this.SetText("结束时刻:" + end_time.ToString("yyyyMMddHHmmss"));
            this.SetText("耗时:" + (end_time - start_time).Minutes.ToString() + "分" + (end_time - start_time).Seconds.ToString() + "秒!");
            setvaliinfo(efDevListBox1, "ELE_valiinfo");
        }
        #endregion

        #region 验证信息输出方法
        private void setvaliinfo(EF.EFDevListBox ls,string name)
        {
            string curdate = System.DateTime.Now.ToString("yyyyMMddHHmmss") + name;
            if (!File.Exists("E:\\" + curdate + ".txt"))
            {
                FileStream fr = new FileStream("E:\\" + curdate + ".txt", FileMode.OpenOrCreate);
                StreamWriter wr = new StreamWriter(fr);
                wr.WriteLine(curdate);
                wr.Close();
                fr.Close();
            }

            StreamWriter sw3 = File.AppendText("E:\\" + curdate + ".txt");
            for (int i = 0; i < ls.Items.Count; i++)
            {
                sw3.WriteLine(ls.Items[i].ToString());
                sw3.Flush();                
            }
            sw3.Close();
            System.Diagnostics.Process.Start("E:\\" + curdate + ".txt");
        }
        #endregion

        #region F11生成物料编码
        private void FormMMSMBEGIN_EF_PRE_DO_FB(object sender, EF.EF_Args e)
        {

        }

        private void FormMMSMBEGIN_EF_DO_FB(object sender, EF.EF_Args e)
        {
            if (this.efDevGrid_MAT.EFChoiceCount == 0)
            {
                MessageBox.Show("请勾选要导入的库存数据！");
                return;
            }

            EI.EIInfo inBlock = new EI.EIInfo();

            inBlock.Tables[0].Merge(efDevGrid_MAT.GetSelectedDataRow());

            EI.EIInfo outBlock = EI.EITuxedo.CallService("mmsmbg_mat_code", inBlock);
            this.EFMsgInfo = outBlock.GetSys().msg;
            if (outBlock.sys_info.flag != 0)
            {
                MessageBox.Show("生成物料编码异常!" + outBlock.GetSys().msg);
                return;
            }
            DS.Tables["TMMSMBG"].Rows.Clear();
            outBlock.SetBlkName(1, "TMMSMBG");
            outBlock.ConvertToStrongType(this.DS);
            this.DS.AcceptChanges();

            DateTime end_time = System.DateTime.Now;
            //this.SetText("结束时刻:" + end_time.ToString("yyyyMMddHHmmss"));
            //this.SetText("耗时:" + (end_time - start_time).Minutes.ToString() + "分" + (end_time - start_time).Seconds.ToString() + "秒!");
            //this.SetText("生成物料编码完毕(〃'▽'〃)");
            //setvaliinfo(efDevListBox1, "MAT_CODE");
        }

        private void FormMMSMBEGIN_EF_CANCEL_DO_FB(object sender, EF.EF_Args e)
        {

        }
        #endregion
    }
}
