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
    public partial class FormMM0097C1S2N : EF.EFForm
    {

       #region <自定义程序用全局变量>
        // 当前分区
        private string cs_formPartition = "";
        private int v_show_change = 0; //是否刷新子信息。
        private string v_rule_code_pub = "";//规则代码。
        private string v_rule_desc_pub = "";//规则描述。
        #endregion

        public FormMM0097C1S2N()
        {
            InitializeComponent();
        }

        #region 规则主体信息查询 Query()
        //规则主体信息查询
        private void p_query_main(int show_flag)
        {
            try
            {
                EI.EIInfo inBlock = new EI.EIInfo();
                EI.EIInfo outBlock;
                int i = 0;

                EF.EFDevGrid efGrid_now = this.efDevGrid_rule;

                //获取 efGrid_nowd对应的View .
                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

                v_rule_code_pub = "";
                string service_name = "";

                //获取规则主信息。
                service_name = "mm0097c1f2_inq";
                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,service_name, inBlock);
                if (show_flag != 0)
                {
                    this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                }

                if (outBlock.sys_info.flag < 0)
                {
                    this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;			
                    return;
                }

                //将信息压入指定的GRID,
                GC.PM_utility2.DEV_setGridValue(efGrid_now, outBlock);

                //列宽自动调整。
                gridView_now.BestFitColumns();
                GC.PM_utility2.DEV_SetColEdit_allNot(this, efGrid_now);

                //计划主信息刷新的时候，进行计划明细信息的刷新。
                //========= 
                v_rule_code_pub = "";
                v_rule_desc_pub = "";
                if (gridView_now.RowCount >= 1)
                {
                    v_rule_code_pub = gridView_now.GetRowCellValue(gridView_now.FocusedRowHandle, "RULE_CODE").ToString();//使得当前行的[xx]被获取。
                    v_rule_desc_pub = gridView_now.GetRowCellValue(gridView_now.FocusedRowHandle, "RULE_DESC").ToString();//使得当前行的[xx]被获取。
                    this.p_query_detail(0);
                }

                this.RULE_CODEefDevTextEdit.Text = v_rule_code_pub;
                this.RULE_DESCefDevTextEdit.Text = v_rule_desc_pub;

                //主信息显示完成，允许刷新子信息。
                v_show_change = 1;
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }
        }
        #endregion

        #region 规则明细信息查询 Query()
        //规则明细信息查询
        private void p_query_detail(int show_flag)
        {
            try
            {
                //查询信息:材料属性+合同属性+转用规则
                EI.EIInfo inBlock = new EI.EIInfo();
                EI.EIInfo outBlock;

                EF.EFDevGrid efGrid_now = this.efDevGrid_rule_detail;
                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

                string v_sql = " SELECT t.*  FROM TMM009C t "
                                + " WHERE  t.RULE_CODE = '" + v_rule_code_pub + "'"
                                + " ORDER  BY t.RULE_SEQ_NO             ";

                string service_name = "eped_dyn_sql";  //采用框架提供的动态SQL 的service 。
                string para_name = "v_sql"; //入口参数的名字
                string para_value = v_sql;  //查询的SQL语句。

                inBlock.SetColName(1, para_name);
                inBlock.SetColVal(1, para_name, para_value);//入口参数信息
                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,service_name, inBlock);

                if (show_flag != 0)
                {
                    this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                }

                //判断调用是否正确
                if (outBlock.sys_info.flag < 0)
                {
                    this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                    return;
                }

                //将信息压入指定的GRID,
                EF.Utility.SetCustomGridValue(efGrid_now, outBlock, false);

                //列宽自动调整。
                gridView_now.BestFitColumns();

                //指定列信息靠左冻结。
                GC.PM_utility2.DEV_ColFixedLeft(efGrid_now, 3); //左3列冻结。

                //所有列不可编辑。
                GC.PM_utility2.DEV_SetColEdit_allNot(this, efGrid_now);
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }
        }
        #endregion

        #region 规则主信息创建
        private void p_mm0097c1_ins()
        {
            try
            {

                EI.EIInfo inBlock = new EI.EIInfo();
                EI.EIInfo outBlock;
                string str_msg = "";

                string v_rule_desc = this.RULE_DESCefDevTextEdit.Text.ToString();
                if (v_rule_desc.Trim() == "")
                {
                    str_msg = "请输入[规则描述]。";
                    GC.PM_utility2.Dev_messageBoxError(str_msg);
                    this.EFMsgInfo = str_msg;
                    //控件旁边的惊叹号。
                    this.RULE_DESCefDevTextEdit.ErrorIcon = (new ErrorProvider()).Icon.ToBitmap(); //设置惊叹号
                    this.RULE_DESCefDevTextEdit.ErrorText = this.EFMsgInfo;
                    return;
                }

                //grid1
                EF.EFDevGrid efGrid_now = this.efDevGrid_Mat;
                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

                //grid2
                EF.EFDevGrid efGrid_now2 = this.efDevGrid_Ord;
                DevExpress.XtraGrid.Views.Grid.GridView gridView_now2 = efGrid_now2.MainView as DevExpress.XtraGrid.Views.Grid.GridView;


                if (efGrid_now.EFChoiceCount != 1)
                {
                    this.EFMsgInfo = "请选择[一条]新增的[材料属性]";
                    GC.PM_utility2.Dev_messageBoxError(this.EFMsgInfo);
                    return;
                }

                //数据压入==左属性[材料属性]
                inBlock.Tables[0].Merge(efGrid_now.GetSelectedDataRow());
                //创建2#BLK==右属性[合同属性]
                inBlock.Tables.Add("ORDER_CHAR");
                inBlock.Tables["ORDER_CHAR"].Merge(efGrid_now2.GetSelectedDataRow());

                //创建3#BLK,存放PMOA规则的其他信息。
                string v_pmoa_flag = "INS"; //规则集创建。
                string v_table_name = "MM0097C1_QT";
                inBlock.Tables.Add(v_table_name);
                inBlock.Tables[v_table_name].Columns.Add("PMOA_FLAG", typeof(System.String));
                inBlock.Tables[v_table_name].Columns.Add("RULE_DESC", typeof(System.String));//规则描述
                inBlock.Tables[v_table_name].Rows.Add();
                inBlock.Tables[v_table_name].Rows[0]["PMOA_FLAG"] = v_pmoa_flag;//业务类型。
                inBlock.Tables[v_table_name].Rows[0]["RULE_DESC"] = v_rule_desc;//规则描述

                //调用新增SERVICE
                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097c1f3_ins", inBlock);
                //判断调用是否正确
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                if (outBlock.sys_info.flag < 0)
                {
                    GC.PM_utility2.Dev_messageBoxError(this.EFMsgInfo);
                    return;
                }

                //获取返回的规则号。
                //============
                v_rule_code_pub = outBlock.Tables[0].Rows[0]["RULE_CODE"].ToString();


                //刷新画面
                this.p_query_detail(0);

            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }
        }
        #endregion

        #region 规则主信息修改
        private void p_mm0097c1_main_upd()
        {
            try
            {
                EI.EIInfo inBlock = new EI.EIInfo();
                EI.EIInfo outBlock;
                int i;

                string str_msg = "";
                EF.EFDevGrid efGrid_now = this.efDevGrid_rule;

                if (efGrid_now.GetSelectedDataRow().Rows.Count <= 0)
                {//判断是否有选中行
                    str_msg = GC.GCRS.GCRSC0000014;/*请选择需操作的记录。*/
                    this.EFMsgInfo = str_msg;
                    GC.PM_utility2.Dev_messageBoxError(str_msg);
                    this.ef_args.buttonStatusHold = true;
                    return;

                }
                //this.EFMsgInfo = "修改操作开始...";	
                inBlock.Tables[0].Merge(efGrid_now.GetSelectedDataRow());

                //调用SERVICE
                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097c1f4_upd", inBlock);
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                //判断调用是否正确
                if (outBlock.sys_info.flag < 0)
                {
                    GC.PM_utility2.Dev_messageBoxError(this.EFMsgInfo);
                    this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                    return;
                }
                //刷新规则主信息。
                //=============
                this.p_query_main(0);
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }
        }
        #endregion

        #region 规则明细添加
        private void p_mm0097c1_app()
        {
            try
            {
                EI.EIInfo inBlock = new EI.EIInfo();
                EI.EIInfo outBlock;
                string str_msg = "";

                string v_rule_code = this.RULE_CODEefDevTextEdit.Text.ToString();
                if (v_rule_code.Trim() == "")
                {
                    str_msg = "请选择一条[规则]，再进行[明细添加]。";
                    GC.PM_utility2.Dev_messageBoxError(str_msg);
                    this.EFMsgInfo = str_msg;
                    //控件旁边的惊叹号。
                    this.RULE_DESCefDevTextEdit.ErrorIcon = (new ErrorProvider()).Icon.ToBitmap(); //设置惊叹号
                    this.RULE_DESCefDevTextEdit.ErrorText = this.EFMsgInfo;
                    return;
                }

                //grid1
                EF.EFDevGrid efGrid_now = this.efDevGrid_Mat;
                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

                //grid2
                EF.EFDevGrid efGrid_now2 = this.efDevGrid_Ord;
                DevExpress.XtraGrid.Views.Grid.GridView gridView_now2 = efGrid_now2.MainView as DevExpress.XtraGrid.Views.Grid.GridView;


                if (efGrid_now.EFChoiceCount != 1)
                {
                    this.EFMsgInfo = "请选择[一条]新增的[材料属性]。";
                    GC.PM_utility2.Dev_messageBoxError(this.EFMsgInfo);
                    return;
                }

                //数据压入
                inBlock.Tables[0].Merge(efGrid_now.GetSelectedDataRow());
                //创建2#BLK
                inBlock.Tables.Add("ORDER_CHAR");
                inBlock.Tables["ORDER_CHAR"].Merge(efGrid_now2.GetSelectedDataRow());

                //创建3#BLK,存放规则的其他信息。
                string v_pmoa_flag = "APP"; //明细添加
                string v_table_name = "MM0097C1_QT";
                inBlock.Tables.Add(v_table_name);
                inBlock.Tables[v_table_name].Columns.Add("PMOA_FLAG", typeof(System.String));
                inBlock.Tables[v_table_name].Columns.Add("RULE_CODE", typeof(System.String));//规则代码
                inBlock.Tables[v_table_name].Rows.Add();
                inBlock.Tables[v_table_name].Rows[0]["PMOA_FLAG"] = v_pmoa_flag;//业务类型。
                inBlock.Tables[v_table_name].Rows[0]["RULE_CODE"] = v_rule_code;//规则代码

                //调用新增SERVICE
                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097c1f3_ins", inBlock);
                //判断调用是否正确
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                if (outBlock.sys_info.flag < 0)
                {
                    GC.PM_utility2.Dev_messageBoxError(this.EFMsgInfo);
                    return;
                }

                //刷新主信息。
                this.p_query_main(0);
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }
        }
        #endregion

        #region 规则明细修改
        private void p_mm0097c1_upd()
        {
            try
            {
                EI.EIInfo inBlock = new EI.EIInfo();
                EI.EIInfo outBlock;


                string str_msg = "";
                EF.EFDevGrid efGrid_now = this.efDevGrid_rule_detail;

                if (efGrid_now.EFChoiceCount <= 0)
                {//判断是否有选中行
                    str_msg = GC.GCRS.GCRSC0000014;/*请选择需操作的记录。*/
                    this.EFMsgInfo = str_msg;
                    GC.PM_utility2.Dev_messageBoxError(str_msg);
                    this.ef_args.buttonStatusHold = true;
                    return;
                }

                //this.EFMsgInfo = "修改操作";	 
                //1#BLK 中存放信息。
                inBlock.Tables[0].Merge(efGrid_now.GetSelectedDataRow());


                //新增2#BLK,存放'OPER_FLAG'= 0/1=一般用户操作/MES操作
                inBlock.Tables.Add("OPER_FLAG");
                inBlock.Tables["OPER_FLAG"].Columns.Add("OPER_FLAG");
                inBlock.Tables["OPER_FLAG"].Rows.Add();
                inBlock.Tables["OPER_FLAG"].Rows[0]["OPER_FLAG"] = "1";

                //调用新增SERVICE
                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097c1f6_upd", inBlock);
                //判断调用是否正确
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                if (outBlock.sys_info.flag < 0)
                {
                    GC.PM_utility2.Dev_messageBoxError(this.EFMsgInfo);
                    return;
                }

                //刷新明细信息。	
                this.p_query_detail(0);//信息的查询 
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }


        }
        #endregion

        #region 规则明细删除
        private void p_mm0097c1_del()
        {
            try
            {
                EI.EIInfo inBlock = new EI.EIInfo();
                EI.EIInfo outBlock;

                string str_msg = "";
                string v_msg = "";

                EF.EFDevGrid efGrid_now = this.efDevGrid_rule_detail;

                if (efGrid_now.EFChoiceCount <= 0)
                {
                    str_msg = GC.GCRS.GCRSC0000014/*请选择需操作的记录。*/;
                    this.EFMsgInfo = str_msg;
                    GC.PM_utility2.Dev_messageBoxError(this.EFMsgInfo);
                    return;
                }

                //提示框,YES/NO.
                v_msg = GC.GCRS.GCRSC0000051;/*是否将选择的信息进行相关操作？*/
                if (GC.PM_utility2.Dev_messageBoxWarning(v_msg) == false)
                {
                    return;
                }

                DataTable dataTable = efDevGrid_rule_detail.GetSelectedDataRow();
                inBlock.Tables[0].Merge(dataTable, true);

                //新增2#BLK,存放'oper_flag'= 0/1=一般用户操作/MES操作
                inBlock.Tables.Add("OPER_FLAG");
                inBlock.Tables["OPER_FLAG"].Columns.Add("OPER_FLAG");
                inBlock.Tables["OPER_FLAG"].Rows.Add();
                inBlock.Tables["OPER_FLAG"].Rows[0]["OPER_FLAG"] = "1";

                //调用新增SERVICE
                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097c1f7_del", inBlock);
                //判断调用是否正确
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                if (outBlock.sys_info.flag < 0)
                {
                    GC.PM_utility2.Dev_messageBoxError(this.EFMsgInfo);
                    return;
                }

                //刷新规则主信息
                this.p_query_main(0);
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }
        }
        #endregion

        #region 规则明细顺序调整
        private void p_mm0097c1_seq()
        {
            try
            {
                EI.EIInfo inBlock = new EI.EIInfo();
                EI.EIInfo outBlock;

                string str_msg = "";
                EF.EFDevGrid efGrid_now = this.efDevGrid_rule_detail;

                //使得当前的GRID全选中。
                GC.PM_utility2.DEV_Choice_all_grid(efGrid_now);

                if (efGrid_now.EFChoiceCount <= 0)
                {
                    str_msg = GC.GCRS.GCRSC0000014/*请选择需操作的记录。*/;
                    this.EFMsgInfo = str_msg;
                    GC.PM_utility2.Dev_messageBoxError(this.EFMsgInfo);
                    this.ef_args.buttonStatusHold = true;
                    return;

                }
                efGrid_now.AllowDragRow = false;    //不允许拖拉

                //this.EFMsgInfo = "修改操作开始...";	
                DataTable dataTable = efDevGrid_rule_detail.GetSelectedDataRow();
                inBlock.Tables[0].Merge(dataTable, true);

                //新增2#BLK,存放'oper_flag'= 0/1=一般用户操作/MES操作
                inBlock.Tables.Add("OPER_FLAG");
                inBlock.Tables["OPER_FLAG"].Columns.Add("OPER_FLAG");
                inBlock.Tables["OPER_FLAG"].Rows.Add();
                inBlock.Tables["OPER_FLAG"].Rows[0]["OPER_FLAG"] = "1";

                //调用新增SERVICE
                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097c1f8_seq", inBlock);
                //判断调用是否正确
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                if (outBlock.sys_info.flag < 0)
                {

                    GC.PM_utility2.Dev_messageBoxError(this.EFMsgInfo);
                    return;
                }

                //刷新页面			
                this.p_query_detail(0);//信息的查询
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }
        }
        #endregion

        #region 规则主信息复制
        private void p_mm0097c1_cpy()
        {
            try
            {
                EI.EIInfo inBlock = new EI.EIInfo();
                EI.EIInfo outBlock;
                string str_msg = "";

                string v_rule_code = this.RULE_CODEefDevTextEdit.Text.ToString();
                if (v_rule_code.Trim() == "")
                {
                    str_msg = "请选择一条[规则]，再进行[明细添加]。";
                    GC.PM_utility2.Dev_messageBoxError(str_msg);
                    this.EFMsgInfo = str_msg;
                    //控件旁边的惊叹号。
                    this.RULE_DESCefDevTextEdit.ErrorIcon = (new ErrorProvider()).Icon.ToBitmap(); //设置惊叹号
                    this.RULE_DESCefDevTextEdit.ErrorText = this.EFMsgInfo;
                    return;
                }

                string v_rule_desc = this.RULE_DESCefDevTextEdit.Text.ToString();
                if (v_rule_desc.Trim() == "")
                {
                    str_msg = "请输入目的[规则描述]。";
                    GC.PM_utility2.Dev_messageBoxError(str_msg);
                    this.EFMsgInfo = str_msg;
                    //控件旁边的惊叹号。
                    this.RULE_DESCefDevTextEdit.ErrorIcon = (new ErrorProvider()).Icon.ToBitmap(); //设置惊叹号
                    this.RULE_DESCefDevTextEdit.ErrorText = this.EFMsgInfo;
                    return;
                }

                //创建2#BLK,存放PMOA规则的其他信息。
                string v_pmoa_flag = "CPY"; //规则集创建。
                string v_table_name = "MM0097C1_QT";
                inBlock.Tables.Add(v_table_name);
                inBlock.Tables[v_table_name].Columns.Add("RULE_CODE", typeof(System.String));//源规则代码
                inBlock.Tables[v_table_name].Columns.Add("RULE_DESC", typeof(System.String));//目的规则描述
                inBlock.Tables[v_table_name].Rows.Add();
                inBlock.Tables[v_table_name].Rows[0]["RULE_CODE"] = v_rule_code;//规则代码 
                inBlock.Tables[v_table_name].Rows[0]["RULE_DESC"] = v_rule_desc;//规则描述

                //调用新增SERVICE
                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097c1f9_cpy", inBlock);
                //判断调用是否正确
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                if (outBlock.sys_info.flag < 0)
                {
                    GC.PM_utility2.Dev_messageBoxError(this.EFMsgInfo);
                    return;
                }

                //获取返回的规则号。
                //============
                v_rule_code_pub = outBlock.Tables[0].Rows[0]["RULE_CODE"].ToString();

                //刷新画面
                this.p_query_detail(0);
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }

        }
        #endregion

        #region 画面加载事件 FormMM0097C1_Load
        /// <summary>
        /// 画面初始化
        /// <para>为查询条件在dataset中新建一行，以输入查询条件数</para>
        /// <para>初始化grid的列信息</para>
        ///  <para>将 下拉列表与数据源进行绑定</para>
        ///  <para>设置画面上控件的初始数据</para>
        ///  <para>设置grid中下拉列表的初始值</para>
        /// <para>对form 的大小进行设置</para>
        /// </summary>
        private void FormMM0097C1_Load(object sender, EventArgs e)
        {
             //设置分区参数
            cs_formPartition = this.ef_args.formPartition;
            try
            {
                //设置LayoutGroup的伸缩按钮。
                GC.PM_utility2.DEV_Init_LayoutGroup(this.layoutControlGroup2);
                GC.PM_utility2.DEV_Init_LayoutGroup(this.layoutControlGroup3);
                GC.PM_utility2.DEV_Init_LayoutGroup(this.layoutControlGroup4);
                GC.PM_utility2.DEV_Init_LayoutGroup(this.layoutControlGroup5);

                //画面风格确定。
                //====================
                GC.PM_utility2.DEV_Init_Form2(this, this.layoutControl1, this.layoutControlGroup1);
                             
                EF.EFDevGrid efGrid_now = this.efDevGrid_Mat;
                EF.EFDevGrid efGrid_now2 = this.efDevGrid_Ord;
                EF.EFDevGrid efGrid_now3 = this.efDevGrid_rule_detail;

                //初始化多记录的列信息。
                EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efGrid_now }, new string[] { "MM0097C1_INQ_CHAR" },cs_formPartition);//多行记录信息。
                EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efGrid_now2 }, new string[] { "MM0097C1_INQ_CHAR" },cs_formPartition);//多行记录信息。
                EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efGrid_now3 }, new string[] { "MM0097C1_INQ" },cs_formPartition);//多行记录信息。
                EF.Utility.SetGridColumn(new EF.EFDevGrid[] { this.efDevGrid_rule }, new string[] { "MM0097C1_MAIN_INQ" },cs_formPartition);//多行记录信息。//规则主信息

                //初始化GRID 的基本设置。
                GC.PM_utility2.DEV_Init_grid2(efGrid_now, "0", cs_formPartition);
                GC.PM_utility2.DEV_Init_grid2(efGrid_now2, "0", cs_formPartition);
                GC.PM_utility2.DEV_Init_grid2(efGrid_now3, "0", cs_formPartition);
                GC.PM_utility2.DEV_Init_grid2(efDevGrid_rule, "0", cs_formPartition);

                string v_sql2 = "";

                ////物料种类= M002=MAT_KIND
                ////PMOA规则类型= PMA2=PRODUCT_FLAG
                ////合同性质= M09R=RULE_TYPE_DESC
                //v_sql2 = "SELECT t.code as CODE ,t.code ||  '_' || t.code_desc_1_content as AABB from tep0002 t  where t.CODE_CLASS = 'M002'  order by t.code ";
                //GC.PM_utility2.DEV_initCol_LookUpEdit(efGrid_now3, "MAT_KIND", "eped_dyn_sql", "XX", v_sql2, "CODE", "AABB");//代码+名称

                //v_sql2 = "SELECT t.code as CODE , t.code_desc_1_content as AABB from tep0002 t  where t.CODE_CLASS = 'PMA2'  order by t.code ";
                //GC.PM_utility2.DEV_initCol_LookUpEdit(efGrid_now3, "PRODUCT_FLAG", "eped_dyn_sql", "XX", v_sql2, "CODE", "AABB");//代码+名称

                //v_sql2 = "SELECT t.code as CODE ,t.code ||  '_' || t.code_desc_1_content as AABB from tep0002 t  where t.CODE_CLASS = 'M09R'  order by t.code ";
                //GC.PM_utility2.DEV_initCol_LookUpEdit(efGrid_now3, "RULE_TYPE_DESC", "eped_dyn_sql", "XX", v_sql2, "CODE", "AABB");//代码+名称

                ////允许强制标志= PMA4==FORCE_MATCH_FLAG
                ////PMOA比较关系= PMA5==MATCH_RELATION
                ////PMOA比较类型= PMA6==RULE_TYPE
                //v_sql2 = "SELECT t.code as CODE , t.code_desc_1_content as AABB from tep0002 t  where t.CODE_CLASS = 'PMA4'  order by t.code ";
                //GC.PM_utility2.DEV_initCol_LookUpEdit(efGrid_now3, "FORCE_MATCH_FLAG", "eped_dyn_sql", "XX", v_sql2, "CODE", "AABB");//代码+名称

                //v_sql2 = "SELECT t.code as CODE ,  t.code_desc_1_content as AABB from tep0002 t  where t.CODE_CLASS = 'PMA5'  order by t.code ";
                //GC.PM_utility2.DEV_initCol_LookUpEdit(efGrid_now3, "MATCH_RELATION", "eped_dyn_sql", "XX", v_sql2, "CODE", "AABB");//代码+名称

                //v_sql2 = "SELECT t.code as CODE ,  t.code_desc_1_content as AABB from tep0002 t  where t.CODE_CLASS = 'PMA6'  order by t.code ";
                //GC.PM_utility2.DEV_initCol_LookUpEdit(efGrid_now3, "RULE_TYPE", "eped_dyn_sql", "XX", v_sql2, "CODE", "AABB");//代码+名称

                //GRID是只允许单选。
                this.efDevGrid_Mat.EFMultiSelect = false; //材料属性。
                this.efDevGrid_Ord.EFMultiSelect = false; //合同属性。 

                //规则代码，不可编辑。
                //规则描述，可编辑。
                this.RULE_CODEefDevTextEdit.Enabled = false;
                this.RULE_DESCefDevTextEdit.Enabled = true;
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }
        }
        #endregion

        #region F2 查询

        private void FormMM0097C1_EF_DO_F2(object sender, EF.EF_Args e)
        {
            this.p_query_main(1);
        }
        #endregion

        #region efButton1材料属性查询
        /// <summary>
        /// 材料属性查询
        ///  <para>检查物料种类是否为空,如果为空,则提示提示用户选择,并退出</para>
        ///  <para>调用pmoa10_inq_char完成转用充当规则材料属性查询操作,并将结果置于grid中</para>
        /// </summary>
        private void efButton1_Click(object sender, EventArgs e)
        { 
            //材料属性查询
            try
            {
                string item_ename = this.colName_Mat.Text;
                        
                EI.EIInfo inBlock = new EI.EIInfo();
                EI.EIInfo outBlock;

                EF.EFDevGrid efGrid_now = this.efDevGrid_Mat;
                //获取 efGrid_nowd对应的View .
                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

                string v_func_id = "MM0097C1_MAT_CHAR";

                //this.EFMsgInfo = "材料属性_查询开始...";	
                int i = 1;
                inBlock.SetColName(1, i++, "func_id");
                inBlock.SetColVal(1, 1, "func_id", v_func_id);

                inBlock.SetColName(1, i++, "item_ename");
                inBlock.SetColVal(1, 1, "item_ename", item_ename);

                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097c1a2_inq", inBlock);
                //判断调用是否正确
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                if (outBlock.sys_info.flag < 0)
                { 
                    GC.PM_utility2.Dev_messageBoxError(this.EFMsgInfo);
                    return;
                }

                //将信息压入指定的GRID,
                EF.Utility.SetCustomGridValue(efGrid_now, outBlock, false);
                //列宽自动调整。
                gridView_now.BestFitColumns();
                //指定左锁定列的列名到一个数组中。
                string[] col_name = new string[] {"ITEM_ENAME"};
                //指定列信息靠左冻结。
                GC.PM_utility2.DEV_ColFixedLeft(efGrid_now, col_name);

                //所有列不可编辑。
                GC.PM_utility2.DEV_SetColEdit_allNot(this, efGrid_now);
            
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }
        }

        #endregion

        #region efButton2合同属性查询
        /// <summary>
        /// 合同属性查询
        ///  <para>检查物料种类是否为空,如果为空,则提示提示用户选择,并退出</para>
        ///  <para>调用pmoa10_inq_char完成转用充当规则材料属性查询操作,并将结果置于grid中</para>
        /// </summary>
        private void efButton2_Click(object sender, EventArgs e)
        {
            //合同属性查询
            try
            {
                string item_ename = colName_Order.Text.Trim();
                
                EF.EFDevGrid efGrid_now = this.efDevGrid_Ord;
                //获取 efGrid_nowd对应的View .
                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

                EI.EIInfo inBlock = new EI.EIInfo();
                EI.EIInfo outBlock;

                string v_func_id = "MM0097C1_ORD_CHAR";

                int i = 1;
                inBlock.SetColName(1, i++, "func_id");
                inBlock.SetColVal(1, 1, "func_id", v_func_id);

                inBlock.SetColName(1, i++, "item_ename");
                inBlock.SetColVal(1, 1, "item_ename", item_ename);

                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0097c1a2_inq", inBlock);
                //判断调用是否正确
                this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;
                if (outBlock.sys_info.flag < 0)
                {

                    GC.PM_utility2.Dev_messageBoxError(this.EFMsgInfo);
                    return;
                }

                //将信息压入指定的GRID,
                EF.Utility.SetCustomGridValue(efGrid_now, outBlock, false);
                //列宽自动调整。
                gridView_now.BestFitColumns();
                //指定左锁定列的列名到一个数组中。
                string[] col_name = new string[] { "ITEM_ENAME" };
                //指定列信息靠左冻结。
                GC.PM_utility2.DEV_ColFixedLeft(efGrid_now, col_name);

                //所有列不可编辑。
                GC.PM_utility2.DEV_SetColEdit_allNot(this, efGrid_now);
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }
        }

        #endregion

        #region F2 规则创建
        /// <summary>
        /// 规则创建(F3)
        ///  <para>调用p_mm0097c1_ins完成双边规则新增操作</para>
        /// </summary>
        private void FormMM0097C1_EF_DO_F3(object sender, EF.EF_Args e)
        {
            //双边规则新增
            try  
            {
                //规则集创建。
                this.p_mm0097c1_ins();                
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }

        }
        #endregion

        #region F4 规则主信息修改 准备
        private void FormMM0097C1_EF_PRE_DO_F4(object sender, EF.EF_Args e)
        {
            try
            {
                EI.EIInfo inBlock = new EI.EIInfo();
                EF.EFDevGrid efGrid_now = this.efDevGrid_rule;

                //获取 efGrid_nowd对应的View .
                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;


                if (gridView_now.RowCount <= 0)
                {
                    GC.PM_utility2.Dev_messageBoxError("[信息列表]中没有信息，请先[查询]再进行操作。"); 
                    this.ef_args.buttonStatusHold = false;//释放BUTTON的控制权。
                    return;
                }

                //当前行被选中
                efGrid_now.SetSelectedColumnChecked(gridView_now.FocusedRowHandle, true);

                //根据功能号，设置可编辑列。
                GC.PM_utility2.DEV_SetColEdit_multi2(this, efGrid_now, "MM0097C1_MAIN_INQ", "2");
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }
        }

        #endregion

        #region F4 规则主信息修改 确定
        private void FormMM0097C1_EF_DO_F4(object sender, EF.EF_Args e)
        {
            this.p_mm0097c1_main_upd();
        }
        #endregion

        #region F4 规则主信息修改 取消
        private void FormMM0097C1_EF_CANCEL_DO_F4(object sender, EF.EF_Args e)
        {
            try
            {
                EF.EFDevGrid efGrid_now = this.efDevGrid_rule;
                //所有列不可编辑。
                GC.PM_utility2.DEV_SetColEdit_allNot(this, efGrid_now);
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }
        }
        #endregion

        #region F5 规则明细添加

        private void FormMM0097C1_EF_DO_F5(object sender, EF.EF_Args e)
        {
            // 规则明细添加 
            this.p_mm0097c1_app();           
        }
        #endregion

        #region F6 规则明细修改 准备
        private void FormMM0097C1_EF_PRE_DO_F6(object sender, EF.EF_Args e)
        {
            try
            {
                EI.EIInfo inBlock = new EI.EIInfo();

                EF.EFDevGrid efGrid_now = this.efDevGrid_rule_detail;
                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
                string v_msg = "[信息列表]中没有信息，请先[查询]再进行操作。";
                if (gridView_now.DataRowCount <= 0)
                {
                    //"[信息列表]中的没有信息，请先[查询]再进行操作。" );
                    GC.PM_utility2.Dev_messageBoxError(v_msg);
                    this.ef_args.buttonStatusHold = false;//释放BUTTON的控制权。
                    return;
                }

                //当前行被选中
                efGrid_now.SetSelectedColumnChecked(gridView_now.FocusedRowHandle, true);
                //根据功能号，设置可编辑列。
                GC.PM_utility2.DEV_SetColEdit_multi2(this, efGrid_now, "MM0097C1_INQ", "2");
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }
        }
        #endregion

        #region F6 规则明细修改 确定
        private void FormMM0097C1_EF_DO_F6(object sender, EF.EF_Args e)
        {
            this.p_mm0097c1_upd();   
        }
        #endregion

        #region F6 规则明细修改 取消
        private void FormMM0097C1_EF_CANCEL_DO_F6(object sender, EF.EF_Args e)
        {
            try
            {
                EF.EFDevGrid efGrid_now = this.efDevGrid_rule_detail;
                //所有列不可编辑。
                GC.PM_utility2.DEV_SetColEdit_allNot(this, efGrid_now);
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }
        }
        #endregion

        #region F7 规则明细删除
        private void FormMM0097C1_EF_DO_F7(object sender, EF.EF_Args e)
        {
            this.p_mm0097c1_del();
        }
        #endregion

        #region F8 规则明细顺序调整 准备
        private void FormMM0097C1_EF_PRE_DO_F8(object sender, EF.EF_Args e)
        {
            try
            {
                EI.EIInfo inBlock = new EI.EIInfo();
                EF.EFDevGrid efGrid_now = this.efDevGrid_rule_detail;
                //获取 efGrid_nowd对应的View .
                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

                if (gridView_now.RowCount <= 1)
                {
                    GC.PM_utility2.Dev_messageBoxError("[信息列表]中没有信息，请先[查询]再进行操作。");
                    this.ef_args.buttonStatusHold = false;//释放BUTTON的控制权。
                    return;
                }
                efGrid_now.AllowDragRow = true;    //允许拖拉
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }
        }
        #endregion

        #region F8 规则明细顺序调整 确定

        private void FormMM0097C1_EF_DO_F8(object sender, EF.EF_Args e)
        {
            this.p_mm0097c1_seq();
        }
        #endregion

        #region F9 规则集复制
        private void FormMM0097C1_EF_DO_F9(object sender, EF.EF_Args e)
        {
            this.p_mm0097c1_cpy();
        }
        #endregion

        #region 事件 gridView3_RowCellStyle
        //若是不允许强制的规则，进行颜色提醒。==FORCE_MATCH_FLAG
        //若某一个规则是不允许强制操作规则[FORCE_MATCH_FLAG = 0]，则[FORCE_MATCH_FLAG]和[MATCH_RELATION_DESC]单元格的字体标记为红色
        //==================
        private void gridView3_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            //若字段[FORCE_MATCH_FLAG] = 0 ，那么，字段[MAT_NO]变红色

            if (e.Column.FieldName == "MATCH_RELATION" || e.Column.FieldName == "RULE_SEQ_NO" || e.Column.FieldName == "FORCE_MATCH_FLAG") //匹配关系//规则序号//允许强制标志
            {
                object ojb = gridView_rule_detail.GetRowCellValue(e.RowHandle, "FORCE_MATCH_FLAG");
                if (ojb != null)
                {
                    if (ojb.ToString() == "0")
                    {
                        e.Appearance.ForeColor = Color.Red;
                    }
                }
            }
        }
        #endregion

        #region 事件 gridView3_RowCellStyle
        private void gridView4_rule_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (v_show_change != 1)
                {//若不要求刷新子信息，直接离开。
                    return;
                }

                //计划主信息刷新的时候，进行计划明细信息的刷新。
                //=========
                v_rule_code_pub = "";
                v_rule_desc_pub = "";
                if (gridView4_rule.RowCount >= 1)
                {
                    v_rule_code_pub = gridView4_rule.GetRowCellValue(gridView4_rule.FocusedRowHandle, "RULE_CODE").ToString();//使得当前行的[xx]被获取。
                    v_rule_desc_pub = gridView4_rule.GetRowCellValue(gridView4_rule.FocusedRowHandle, "RULE_DESC").ToString();//使得当前行的[xx]被获取。
                    
                }
                this.p_query_detail(0);
                this.RULE_CODEefDevTextEdit.Text = v_rule_code_pub;
                this.RULE_DESCefDevTextEdit.Text = v_rule_desc_pub; 

 
            }
            catch (Exception ex)
            {
                GC.PM_utility2.Dev_messageBoxError(ex.Message);
            }               
        }
        #endregion

    }
}
