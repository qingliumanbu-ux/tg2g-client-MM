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
    public partial class FormMM0001D1S2N : EF.EFForm
    {
        public FormMM0001D1S2N()
        {
            InitializeComponent();
        }

        #region <自定义程序用全局变量>
        // 当前分区
        private string cs_formPartition = "";
        // 当前页数
        private int iPageIndex = 1;
        // 总页数
        private int iPageCount = 0;
        //画面名称
        private string cs_form_ename = "";
        //物料种类
        private string cs_mat_kind = "";
        //产线类型
        private string cs_mat_line_type = "";
        //表名
        private string cs_table_ename = "";
        //厂别
        private string cs_factory_div = "";

        private string cs_function_name = "";

        #endregion

        #region 绑定下拉框内容 BindDataSource()
        private void BindDataSource()
        {

        }
        #endregion

        #region 多记录查询 QueryMat()
        private void QueryMat(int nPageNo)
        {
            ////必须项的内容校验。 
            //if (!EFX.EFCGrid.GetEFCGridBase(efDevGrid_Query_Plan).ValidateGridDataEx())
            //{ //若校验失败，则直接离开。
            //    this.EFMsgInfo = "请先选择查询条件。";
            //    return;
            //}
 
            //设置光标为沙漏
            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;

            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables.Clear();
            inBlock.Tables.Add(EF.Utility.GetSingleGridValue(efDevGrid_Query).Tables[0].Copy());
            if (inBlock.Tables[0].Columns.Contains("MAT_KIND") == false)
            {
                inBlock.Tables[0].Columns.Add("MAT_KIND");  //物料种类
            }

            inBlock.Tables[0].Columns.Add("TABLE_ENAME");  //表名
            inBlock.Tables[0].Columns.Add("MAT_LINE_TYPE");  //产线类型
            inBlock.Tables[0].Columns.Add("FUNCTION_NAME");  
            inBlock.Tables[0].Columns.Add("RECORD_COUNT_PER_PAGE"); //每页记录数
            inBlock.Tables[0].Columns.Add("CURRENT_PAGE_NO");       //需查询的页号

            inBlock.Tables[0].Rows[0]["MAT_KIND"] = cs_mat_kind.Trim();
            inBlock.Tables[0].Rows[0]["TABLE_ENAME"] = cs_table_ename.Trim();
            inBlock.Tables[0].Rows[0]["MAT_LINE_TYPE"] = cs_mat_line_type.Trim();
            inBlock.Tables[0].Rows[0]["FUNCTION_NAME"] = cs_function_name.Trim();
            inBlock.Tables[0].Rows[0]["RECORD_COUNT_PER_PAGE"] = this.efDevGrid1.PageSize;
            inBlock.Tables[0].Rows[0]["CURRENT_PAGE_NO"] = nPageNo;            

            //调用SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0001d1f2_inq", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            //设置返回信息
            if (outBlock.GetSys().flag == 0)
            {
                //清空多记录的数据
                EFX.EFCGrid.GetEFCGridBase(efDevGrid1).ClearGridData();

                if (outBlock.blk_info[outBlock.blk_now].Row > 0)
                {
                    EF.Utility.SetCustomGridValue(this.efDevGrid1, outBlock, false);
                    this.efDevGrid1.TotalRecordCount = Int32.Parse(outBlock.GetColVal("PAGEINFO", 1, "TOTAL_RECORD").Trim());
                    iPageCount = this.efDevGrid1.TotalRecordCount / this.efDevGrid1.PageSize;
                    if (this.efDevGrid1.TotalRecordCount % this.efDevGrid1.PageSize > 0)
                    {
                        iPageCount++;
                    }
                    this.efDevGrid1.RecordCountMessage = String.Format(GC.GCRS.GCRSC0000056/*第 {0}/{1} 页，共 {2} 条记录*/, iPageIndex, iPageCount, this.efDevGrid1.TotalRecordCount);
                    //此属性设置efDevGrid中前面的行号,开始值(一般都是从0 开始,如果查询的是其他页 ,那应该是从 (当前页 * 每页大小+1) 开始
                    this.efDevGrid1.InitRowOrdinal = (iPageIndex - 1) * this.efDevGrid1.PageSize + 1;
                    if (iPageIndex == iPageCount)
                    {
                        /*设置跳至最后一页的按钮属性为false*/
                        this.efDevGrid1.LastPageButtonEnable = false;
                        this.efDevGrid1.NextPageButtonEnable = false;
                    }
                    else
                    {
                        this.efDevGrid1.LastPageButtonEnable = true;
                        this.efDevGrid1.NextPageButtonEnable = true;
                    }
                    /*当起始页为1的时候，则设置第一页和上一页的属性为false*/
                    if (iPageIndex == 1)
                    {
                        this.efDevGrid1.FirstPageButtonEnable = false;
                        this.efDevGrid1.PrePageButtonEnable = false;
                    }
                    else
                    {
                        this.efDevGrid1.FirstPageButtonEnable = true;
                        this.efDevGrid1.PrePageButtonEnable = true;
                    }


                }
                else
                {
                    //清空单记录的数据
                    //EFX.EFCGrid.GetEFCGridBase(efDevGrid2).ClearGridData();

                    this.efDevGrid1.TotalRecordCount = 0;
                    this.efDevGrid1.RecordCountMessage = String.Format(GC.GCRS.GCRSC0000056/*第 {0}/{1} 页，共 {2} 条记录*/, 1, 1, 0);
                }
                this.EFMsgInfo = string.Format(GC.GCRS.GCRSC0000033/*查询到[{0}]条记录。*/, efDevGrid1.TotalRecordCount);
            }

            //恢复光标为箭头
            this.Cursor = System.Windows.Forms.Cursors.Default;
            //设置列字段自动调节宽度
            this.efDevGrid1.SetColumnsWidthAuto();
        }
        #endregion

        #region 单记录查询材料信息 QueryMatInfo()
        private void QueryMatInfo()
        {//单记录查询模式

            //EI.EIInfo inBlock = new EI.EIInfo();
            //EI.EIInfo outBlock;

            //inBlock.Tables[0].Columns.Add("MAT_NO");

            ////使用一个新行前，必须先调用新增操作
            //inBlock.Tables[0].Rows.Add();
            //inBlock.Tables[0].Rows[0]["MAT_NO"] = this.gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "MAT_NO").ToString();

            //outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0001d1a1_inq", inBlock);
            //this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            //if (outBlock.sys_info.flag < 0)
            //{
            //    return;
            //}

            ////将信息压入指定的GRID
            //EFX.EFCGrid.GetEFCGridBase(this.efDevGrid2).SetGridValue(outBlock);

        }
        #endregion     
        
        #region 逻辑画面调用 FormMM0001D1_EF_START_FORM_BY_EP
        private void FormMM0001D1_EF_START_FORM_BY_EP(object sender, EF.EF_Args i_args)
        {
            cs_form_ename = i_args.formEName.ToString();

            if (i_args.GetCallParamsByName("MAT_KIND") != null)
            {
                cs_mat_kind = i_args.GetCallParamsByName("MAT_KIND");
            }
            if (i_args.GetCallParamsByName("TABLE_ENAME") != null)
            {
                cs_table_ename = i_args.GetCallParamsByName("TABLE_ENAME");
            }
            if (i_args.GetCallParamsByName("MAT_LINE_TYPE") != null)
            {
                cs_mat_line_type = i_args.GetCallParamsByName("MAT_LINE_TYPE");
            }
            if (i_args.GetCallParamsByName("FACTORY_DIV") != null)
            {
                cs_factory_div = i_args.GetCallParamsByName("FACTORY_DIV");
            }
            if (i_args.GetCallParamsByName("FUNCTION_NAME") != null)
            {
                cs_function_name = i_args.GetCallParamsByName("FUNCTION_NAME");
            }
        }
        #endregion

        #region 画面被EF调用 FormMM0001D1_EF_START_FORM_BY_EF

        #endregion

        #region 画面载入 FormMM0001D1_Load
        private void FormMM0001D1_Load(object sender, EventArgs e)
        {
            //设置分区参数
            cs_formPartition = this.ef_args.formPartition;

            EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efDevGrid1 }, new string[] { cs_form_ename + "_INQ" },cs_formPartition);

            //EFX.EFCGrid.InitSingleGridColumn(efDevGrid2, cs_form_ename + "_INQ_DETAIL",cs_formPartition);

            EFX.EFCGrid.InitSingleGridColumn(efDevGrid_Query, cs_form_ename + "_QUERY",cs_formPartition);

            // 绑定下拉框内容
            BindDataSource();
            
            //设置grid列是否可编辑
            this.efDevGrid1.SetAllColumnReadOnlyWithoutSelection(true);

            //字段靠左冻结。
            gridView1.Columns.ColumnByFieldName("MAT_NO").Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;          
        }
        #endregion

        #region F2 查询
        private void FormMM0001D1_EF_DO_F2(object sender, EF.EF_Args e)
        {
            try
            {
                iPageIndex = 1;
                QueryMat(1);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }

        }
        #endregion

        #region F6 放冷
        private void FormMM0001D1_EF_DO_F6(object sender, EF.EF_Args e)
        {
            //判断是否有选中行
            if (this.efDevGrid1.GetSelectedDataRow().Rows.Count <= 0)
            {

                this.EFMsgInfo = "请选择需操作的记录。";
                MessageBox.Show("请选择需操作的记录。");
                this.ef_args.buttonStatusHold = true;
                return;
            }

           
            // 设置传入参数 
            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Merge(this.efDevGrid1.GetSelectedDataRow());
           // inBlock.Tables[0].Columns.Add("MAT_KIND");  //物料种类
           // inBlock.Tables[0].Columns.Add("TABLE_ENAME");  //表名
           // inBlock.Tables[0].Columns.Add("MAT_LINE_TYPE");  //产线类型
           //// inBlock.Tables[0].Columns.Add("SLAB_COLD_HOT_FLAG");  //冷热标记

           // inBlock.Tables[0].Rows[0]["MAT_KIND"] = cs_mat_kind.Trim();
           // inBlock.Tables[0].Rows[0]["TABLE_ENAME"] = cs_table_ename.Trim();
           // inBlock.Tables[0].Rows[0]["MAT_LINE_TYPE"] = cs_mat_line_type.Trim();
           inBlock.Tables[0].Rows[0]["SLAB_COLD_HOT_FLAG"] = "C";
            //调用 SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mmsm01d3smf6_pro", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            //判断调用是否正确
            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            // 按当前页刷新画面
            QueryMat(iPageIndex);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }
        #endregion

        #region F7 放冷取消
        private void FormMM0001D1_EF_DO_F7(object sender, EF.EF_Args e)
        {
            //判断是否有选中行
            if (this.efDevGrid1.GetSelectedDataRow().Rows.Count <= 0)
            {

                this.EFMsgInfo = "请选择需操作的记录。";
                MessageBox.Show("请选择需操作的记录。");
                this.ef_args.buttonStatusHold = true;
                return;
            }


            // 设置传入参数 
            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

             inBlock.Tables[0].Merge(this.efDevGrid1.GetSelectedDataRow());
            //inBlock.Tables[0].Columns.Add("MAT_KIND");  //物料种类
            //inBlock.Tables[0].Columns.Add("TABLE_ENAME");  //表名
            //inBlock.Tables[0].Columns.Add("MAT_LINE_TYPE");  //产线类型
          //  inBlock.Tables[0].Columns.Add("SLAB_COLD_HOT_FLAG");  //冷热标记

            //inBlock.Tables[0].Rows[0]["MAT_KIND"] = cs_mat_kind.Trim();
            //inBlock.Tables[0].Rows[0]["TABLE_ENAME"] = cs_table_ename.Trim();
            //inBlock.Tables[0].Rows[0]["MAT_LINE_TYPE"] = cs_mat_line_type.Trim();
            inBlock.Tables[0].Rows[0]["SLAB_COLD_HOT_FLAG"] = "H";
            //调用 SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mmsm01d3smf6_pro", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            //判断调用是否正确
            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            // 按当前页刷新画面
            QueryMat(iPageIndex);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }
        #endregion

        #region F8 在制品转成品 确定
        private void FormMM0001D1_EF_DO_F8(object sender, EF.EF_Args e)
        {
            //判断是否有选中行
            if (this.efDevGrid1.GetSelectedDataRow().Rows.Count <= 0)
            {

                this.EFMsgInfo = "请选择需操作的记录。";
                MessageBox.Show("请选择需操作的记录。");
                this.ef_args.buttonStatusHold = true;
                return;
            }

            // 检查数据合法性 
            for (int i = 0; i < this.efDevGrid1.GetSelectedDataRow().Rows.Count; i++)
            {
                if (efDevGrid1.GetSelectedDataRow().Rows[i]["MAT_STATUS"].ToString().Trim().Substring(0, 1) != "2"
                 && efDevGrid1.GetSelectedDataRow().Rows[i]["ORDER_NO"].ToString().Trim() != "")
                {
                    EF.EFMessageBox.Show("请选择在制品余材!", EF.EF_Args.epEname, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.ef_args.buttonStatusHold = true;
                    return;
                }

            }

            // 设置传入参数 
            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Merge(this.efDevGrid1.GetSelectedDataRow());
            inBlock.Tables[0].Columns.Add("MAT_KIND");  //物料种类
            inBlock.Tables[0].Columns.Add("TABLE_ENAME");  //表名
            inBlock.Tables[0].Columns.Add("MAT_LINE_TYPE");  //产线类型

            inBlock.Tables[0].Rows[0]["MAT_KIND"] = cs_mat_kind.Trim();
            inBlock.Tables[0].Rows[0]["TABLE_ENAME"] = cs_table_ename.Trim();
            inBlock.Tables[0].Rows[0]["MAT_LINE_TYPE"] = cs_mat_line_type.Trim();
            //调用 SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0001d1f8_pro", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            //判断调用是否正确
            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            // 按当前页刷新画面
            QueryMat(iPageIndex);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }
        #endregion

        #region F9 成品转在制品 确定
        private void FormMM0001D1_EF_DO_F9(object sender, EF.EF_Args e)
        {
            //判断是否有选中行
            if (this.efDevGrid1.GetSelectedDataRow().Rows.Count <= 0)
            {

                this.EFMsgInfo = "请选择需操作的记录。";
                MessageBox.Show("请选择需操作的记录。");
                this.ef_args.buttonStatusHold = true;
                return;
            }

            // 检查数据合法性 
            for (int i = 0; i < this.efDevGrid1.GetSelectedDataRow().Rows.Count; i++)
            {
                if (efDevGrid1.GetSelectedDataRow().Rows[i]["MAT_STATUS"].ToString().Trim().Substring(0, 1) != "3"
                 && efDevGrid1.GetSelectedDataRow().Rows[i]["ORDER_NO"].ToString().Trim() != "")
                {
                    EF.EFMessageBox.Show("请选择成品余材!", EF.EF_Args.epEname, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.ef_args.buttonStatusHold = true;
                    return;
                }
            }

            // 设置传入参数 
            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Merge(this.efDevGrid1.GetSelectedDataRow());
            inBlock.Tables[0].Columns.Add("MAT_KIND");  //物料种类
            inBlock.Tables[0].Columns.Add("TABLE_ENAME");  //表名
            inBlock.Tables[0].Columns.Add("MAT_LINE_TYPE");  //产线类型

            inBlock.Tables[0].Rows[0]["MAT_KIND"] = cs_mat_kind.Trim();
            inBlock.Tables[0].Rows[0]["TABLE_ENAME"] = cs_table_ename.Trim();
            inBlock.Tables[0].Rows[0]["MAT_LINE_TYPE"] = cs_mat_line_type.Trim();
            //调用 SERVICE
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0001d1f9_pro", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            //判断调用是否正确
            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            // 按当前页刷新画面
            QueryMat(iPageIndex);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }
        #endregion

        #region  F10 管理封锁 确定
        private void FormMM0001D1_EF_DO_FA(object sender, EF.EF_Args e)
        {
            //判断是否有选中行
            if (this.efDevGrid1.GetSelectedDataRow().Rows.Count <= 0)
            {

                this.EFMsgInfo = "请选择需操作的记录。";
                MessageBox.Show("请选择需操作的记录。");
                this.ef_args.buttonStatusHold = true;
                return;
            }

            // 检查数据合法性 
            for (int i = 0; i < this.efDevGrid1.GetSelectedDataRow().Rows.Count; i++)
            {
                if (efDevGrid1.GetSelectedDataRow().Rows[i]["HOLD_FLAG"].ToString() == "1"  //1-管理封锁
                 || efDevGrid1.GetSelectedDataRow().Rows[i]["HOLD_FLAG"].ToString() == "3") //3-双重封锁
                {
                    EF.EFMessageBox.Show("请选择未管理封锁的材料!", EF.EF_Args.epEname, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.ef_args.buttonStatusHold = true;
                    return;
                }
            } 
            
            // 弹出管理封锁画面
            EI.EIInfo inBlock = new EI.EIInfo();

            inBlock.Tables[0].Merge(this.efDevGrid1.GetSelectedDataRow());

            EF.EF_Args.common_object_1 = inBlock;
            EF.EF_Args.common_parameter_1 = "F10";//管理封锁
            EF.EF_Args.common_parameter_2 = cs_form_ename;//管理封锁
            EF.EF_Args.common_parameter_3 = cs_mat_kind;//管理封锁
            EF.EF_Args.common_parameter_4 = cs_table_ename;//管理封锁
            EF.EF_Args.common_parameter_5 = cs_mat_line_type;//管理封锁

            this.EFShowDialogForm("MM0001D2", new object[] { });

            // 按当前页刷新画面
            QueryMat(iPageIndex);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }
        #endregion

        #region  F11 管理释放 确定
        private void FormMM0001D1_EF_DO_FB(object sender, EF.EF_Args e)
        {
            //判断是否有选中行
            if (this.efDevGrid1.GetSelectedDataRow().Rows.Count <= 0)
            {

                this.EFMsgInfo = "请选择需操作的记录。";
                MessageBox.Show("请选择需操作的记录。");
                this.ef_args.buttonStatusHold = true;
                return;
            }

            // 检查数据合法性 
            for (int i = 0; i < this.efDevGrid1.GetSelectedDataRow().Rows.Count; i++)
            {
                if (efDevGrid1.GetSelectedDataRow().Rows[i]["HOLD_FLAG"].ToString() != "1"  //1-管理封锁
                 && efDevGrid1.GetSelectedDataRow().Rows[i]["HOLD_FLAG"].ToString() != "3") //3-双重封锁
                {
                    EF.EFMessageBox.Show("请选择管理封锁或双重封锁的材料!", EF.EF_Args.epEname, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.ef_args.buttonStatusHold = true;
                    return;
                }
            }   
            
            // 弹出管理释放画面
            EI.EIInfo inBlock = new EI.EIInfo();

            inBlock.Tables[0].Merge(this.efDevGrid1.GetSelectedDataRow());

            EF.EF_Args.common_object_1 = inBlock;
            EF.EF_Args.common_parameter_1 = "F11";//管理释放
            EF.EF_Args.common_parameter_2 = cs_form_ename;//管理封锁
            EF.EF_Args.common_parameter_3 = cs_mat_kind;//管理封锁
            EF.EF_Args.common_parameter_4 = cs_table_ename;//管理封锁
            EF.EF_Args.common_parameter_5 = cs_mat_line_type;//管理封锁

            this.EFShowDialogForm("MM0001D2", new object[] { });

            // 按当前页刷新画面
            QueryMat(iPageIndex);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }
        #endregion

        #region F12 去向变更 确定
        private void FormMM0001D1_EF_DO_FC(object sender, EF.EF_Args e)
        {
            //判断是否有选中行
            if (this.efDevGrid1.GetSelectedDataRow().Rows.Count <= 0)
            {

                this.EFMsgInfo = "请选择需操作的记录。";
                MessageBox.Show("请选择需操作的记录。");
                this.ef_args.buttonStatusHold = true;
                return;
            }

            // 弹出管理封锁画面
            EI.EIInfo inBlock = new EI.EIInfo();

            inBlock.Tables[0].Merge(this.efDevGrid1.GetSelectedDataRow());

            EF.EF_Args.common_object_1 = inBlock;
            EF.EF_Args.common_parameter_1 = "F12";//去向变更
            EF.EF_Args.common_parameter_2 = cs_form_ename;//去向变更
            EF.EF_Args.common_parameter_3 = cs_mat_kind;//去向变更
            EF.EF_Args.common_parameter_4 = cs_table_ename;//去向变更
            EF.EF_Args.common_parameter_5 = cs_mat_line_type;//去向变更

            this.EFShowDialogForm("MM0001D2", new object[] { });

            // 按当前页刷新画面
            QueryMat(iPageIndex);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;
        }
        #endregion

        #region 单击grid的材料 gridView1_FocusedRowChanged
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //if (this.gridView1.GetFocusedDataRow() != null)
            //{
            //    QueryMatInfo();
            //}
        }
        #endregion
        
        #region  翻页事件
        //关于翻页的首页 尾页 向上 向下翻页功能
        private void efDevGrid1_EF_GridBar_Fisrt_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                iPageIndex = 1;
                QueryMat(iPageIndex);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }
        }

        private void efDevGrid1_EF_GridBar_Last_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                iPageIndex = iPageCount;
                QueryMat(iPageIndex);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }
        }

        private void efDevGrid1_EF_GridBar_NextPage_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                iPageIndex++;
                QueryMat(iPageIndex);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }
        }

        private void efDevGrid1_EF_GridBar_PageTo_Event(object sender, EF.EFDevGrid.EFNavigatorButtonClickEventArgs e)
        {
            try
            {
                this.efDevGrid1.PageSize = e.PageSize;
                iPageIndex = e.PageTo;
                QueryMat(iPageIndex);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }

        }

        private void efDevGrid1_EF_GridBar_PrePage_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            try
            {
                iPageIndex--;
                QueryMat(iPageIndex);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }
        }

        private void efDevGrid1_EF_GridBar_Refresh_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            QueryMat(iPageIndex);
        }

        #endregion

    }
}
