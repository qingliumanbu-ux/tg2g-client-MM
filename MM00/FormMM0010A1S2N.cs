using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;//
using System.Text.RegularExpressions;//

namespace MM
{
    public partial class FormMM0010A1S2N : EF.EFForm
    {
        public FormMM0010A1S2N()
        {
            InitializeComponent();
        }

        #region <自定义程序用全局变量>
        // 当前分区
        private string cs_formPartition = "";

        //进料计划是否
        private string cs_demand_plan_no = "";
        //来料来源
        private string cs_raw_origin = "";
        //确认标记_外购料确认标记
        private string cs_affirm_flag = "";
        //材料类型
        private string cs_mat_kind = "";
        //Grid查询的ED54功能号
        private string cs_function_id_inq = "";
        //Grid导入的ED54功能号
        private string cs_function_id_ins = "";
        // 当前页数
        private int iPageIndex = 1;
        // 总页数
        private int iPageCount = 0;
        #endregion

        #region 绑定下拉框内容 BindDataSource()
        private void BindDataSource()
        {
            EI.EIInfo outBlock = EF.Utility.GetPartitionCodeClassValue(cs_formPartition,"M001","M00B");

            //材料去向_物料产线类型
            Common.Utility.SetLookUpEditProperty(mAT_LINE_TYPEEFDevLookUpEdit, outBlock.Tables["M001"], true, true);
            //确认标记
            Common.Utility.SetLookUpEditProperty(aFFIRM_FLAGEFDevLookUpEdit, outBlock.Tables["M00B"], true, true);

            //*材料类型 
            DataTable dt = new DataTable();
            dt.Columns.Add("CODE");
            dt.Columns.Add("CODE_DESC_1_CONTENT");

            DataRow row = dt.NewRow();
            row["CODE"] = "SM";
            row["CODE_DESC_1_CONTENT"] = "板坯";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["CODE"] = "HR";
            row["CODE_DESC_1_CONTENT"] = "热卷";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["CODE"] = "CR";
            row["CODE_DESC_1_CONTENT"] = "冷卷";
            dt.Rows.Add(row);
            Common.Utility.SetLookUpEditProperty(mAT_KINDEFDevLookUpEdit, dt, false, true);           

            //计划是否
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("CODE");
            dt1.Columns.Add("CODE_DESC_1_CONTENT");

            DataRow row1 = dt1.NewRow();
            row1["CODE"] = "Y";
            row1["CODE_DESC_1_CONTENT"] = "有计划";
            dt1.Rows.Add(row1);

            row1 = dt1.NewRow();
            row1["CODE"] = "N";
            row1["CODE_DESC_1_CONTENT"] = "无计划";
            dt1.Rows.Add(row1);
            Common.Utility.SetLookUpEditProperty(dEMAND_PLAN_NOEFDevLookUpEdit, dt1, true, true); 
           
        }
        #endregion

        #region 多记录查询 QueryMat()
        private void QueryMat(int nPageNo)
        {

            if (this.mAT_KINDEFDevLookUpEdit.Text.Trim() == "")
            {
                MessageBox.Show("材料类型不能为空!");
                this.EFMsgInfo = "材料类型不能为空!";
                //控件旁边的惊叹号。
                this.mAT_KINDEFDevLookUpEdit.ErrorIcon = (new ErrorProvider()).Icon.ToBitmap(); //设置惊叹号
                this.mAT_KINDEFDevLookUpEdit.ErrorText = this.EFMsgInfo;
                return;
            }

            
            EI.EIInfo inBlock = new EI.EIInfo();
            EI.EIInfo outBlock;

            inBlock.Tables[0].Columns.Add("RAW_ORIGIN");         //来料来源
            inBlock.Tables[0].Columns.Add("MAT_KIND");           //*材料类型
            inBlock.Tables[0].Columns.Add("MAT_LINE_TYPE");      //材料去向
            inBlock.Tables[0].Columns.Add("SHIP_LOT_NO");        //船批号
            inBlock.Tables[0].Columns.Add("STOCK_NO_TO");        //目标库区(号)   
            inBlock.Tables[0].Columns.Add("MAT_NO");             //材料号 
            inBlock.Tables[0].Columns.Add("ST_NO");              //出钢记号
            inBlock.Tables[0].Columns.Add("UNIT_NR_NEXT");       //下机组   
            inBlock.Tables[0].Columns.Add("MAT_THICK_FROM");     //厚度FROM   
            inBlock.Tables[0].Columns.Add("MAT_TIHCK_TO");       //厚度TO 
            inBlock.Tables[0].Columns.Add("MAT_WIDTH_FROM");     //宽度FROM
            inBlock.Tables[0].Columns.Add("MAT_WIDTH_TO");       //宽度TO
            inBlock.Tables[0].Columns.Add("MAT_WT_FROM");        //重量FROM
            inBlock.Tables[0].Columns.Add("MAT_WT_TO");          //重量TO
            inBlock.Tables[0].Columns.Add("AFFIRM_FLAG");        //确认标记
            inBlock.Tables[0].Columns.Add("DEMAND_PLAN_NO");     //进料计划是否

            inBlock.Tables[0].Columns.Add("RECORD_COUNT_PER_PAGE");    //每页记录数
            inBlock.Tables[0].Columns.Add("CURRENT_PAGE_NO");          //需查询的页号

            //使用一个新行前，必须先调用新增操作
            inBlock.Tables[0].Rows.Add();

            inBlock.Tables[0].Rows[0]["RAW_ORIGIN"]     = this.rAW_ORIGINEFDevRadioGroup.EditValue;
            inBlock.Tables[0].Rows[0]["MAT_KIND"]       = this.mAT_KINDEFDevLookUpEdit.EditValue;
            inBlock.Tables[0].Rows[0]["MAT_LINE_TYPE"]  = this.mAT_LINE_TYPEEFDevLookUpEdit.EditValue;
            inBlock.Tables[0].Rows[0]["SHIP_LOT_NO"]    = this.sHIP_LOT_NOEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["STOCK_NO_TO"]    = this.aIM_STOREEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["MAT_NO"]         = this.mAT_NOEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["ST_NO"]          = this.sT_NOEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["UNIT_NR_NEXT"]   = this.uNIT_NR_NEXTEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["MAT_THICK_FROM"] = this.mAT_THICK_FROMEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["MAT_TIHCK_TO"]   = this.mAT_TIHCK_TOEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["MAT_WIDTH_FROM"] = this.mAT_WIDTH_FROMEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["MAT_WIDTH_TO"]   = this.mAT_WIDTH_TOEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["MAT_WT_FROM"]    = this.mAT_WT_FROMEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["MAT_WT_TO"]      = this.mAT_WT_TOEFDevTextEdit.Text.Trim();
            inBlock.Tables[0].Rows[0]["AFFIRM_FLAG"]    = this.aFFIRM_FLAGEFDevLookUpEdit.EditValue;
            inBlock.Tables[0].Rows[0]["DEMAND_PLAN_NO"] = this.dEMAND_PLAN_NOEFDevLookUpEdit.EditValue;         //进料计划是否

            inBlock.Tables[0].Rows[0]["RECORD_COUNT_PER_PAGE"] = this.efDevGrid_Mat.PageSize;
            inBlock.Tables[0].Rows[0]["CURRENT_PAGE_NO"] = nPageNo;
            
            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0010a1f2_inq", inBlock);           
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            //设置返回信息
            if (outBlock.GetSys().flag == 0)
            {
                //清空多记录的数据
                EFX.EFCGrid.GetEFCGridBase(efDevGrid_Mat).ClearGridData();

                if (outBlock.blk_info[outBlock.blk_now].Row > 0)
                {
                    EF.Utility.SetCustomGridValue(this.efDevGrid_Mat, outBlock, false);
                    this.efDevGrid_Mat.TotalRecordCount = Int32.Parse(outBlock.GetColVal("PAGEINFO", 1, "TOTAL_RECORD").Trim());
                    iPageCount = this.efDevGrid_Mat.TotalRecordCount / this.efDevGrid_Mat.PageSize;
                    if (this.efDevGrid_Mat.TotalRecordCount % this.efDevGrid_Mat.PageSize > 0)
                    {
                        iPageCount++;
                    }
                    this.efDevGrid_Mat.RecordCountMessage = String.Format(GC.GCRS.GCRSC0000056/*第 {0}/{1} 页，共 {2} 条记录*/, iPageIndex, iPageCount, this.efDevGrid_Mat.TotalRecordCount);
                    //此属性设置efDevGrid中前面的行号,开始值(一般都是从0 开始,如果查询的是其他页 ,那应该是从 (当前页 * 每页大小+1) 开始
                    this.efDevGrid_Mat.InitRowOrdinal = (iPageIndex - 1) * this.efDevGrid_Mat.PageSize + 1;
                    if (iPageIndex == iPageCount)
                    {
                        /*设置跳至最后一页的按钮属性为false*/
                        this.efDevGrid_Mat.LastPageButtonEnable = false;
                        this.efDevGrid_Mat.NextPageButtonEnable = false;
                    }
                    else
                    {
                        this.efDevGrid_Mat.LastPageButtonEnable = true;
                        this.efDevGrid_Mat.NextPageButtonEnable = true;
                    }
                    /*当起始页为1的时候，则设置第一页和上一页的属性为false*/
                    if (iPageIndex == 1)
                    {
                        this.efDevGrid_Mat.FirstPageButtonEnable = false;
                        this.efDevGrid_Mat.PrePageButtonEnable = false;
                    }
                    else
                    {
                        this.efDevGrid_Mat.FirstPageButtonEnable = true;
                        this.efDevGrid_Mat.PrePageButtonEnable = true;
                    }
                }
                else
                {
                    this.efDevGrid_Mat.TotalRecordCount = 0;
                    this.efDevGrid_Mat.RecordCountMessage = String.Format(GC.GCRS.GCRSC0000056/*第 {0}/{1} 页，共 {2} 条记录*/, 1, 1, 0);
                }
                this.EFMsgInfo = string.Format(GC.GCRS.GCRSC0000033/*查询到[{0}]条记录。*/, efDevGrid_Mat.TotalRecordCount);
            }

            //恢复光标为箭头
            this.Cursor = System.Windows.Forms.Cursors.Default;

            //设置列字段自动调节宽度
            this.efDevGrid_Mat.SetColumnsWidthAuto();
        }
        #endregion

        #region 画面载入 FormMM0010A1_Load
        private void FormMM0010A1_Load(object sender, EventArgs e)
        {
            //设置分区参数
            cs_formPartition = this.ef_args.formPartition;

            //设置默认值
            cs_affirm_flag      = "N";      //N-未确认    确认标记
            cs_mat_kind         = "SM";     //SM-板坯     材料类型
            cs_raw_origin       = "1";      //外购料       原料来源
            cs_demand_plan_no   = "Y";      //Y-有计划     计划是否

            cs_function_id_inq = "MM0010A1_INQ" + "_" + cs_mat_kind;   //MM0010A1_INQ_SM  板坯外购料信息查询 
            cs_function_id_ins = "MM0010A1_INS" + "_" + cs_mat_kind;   //MM0010A1_INS_SM  板坯外购料信息导入 

            aFFIRM_FLAGEFDevLookUpEdit.EditValue    = cs_affirm_flag;         //确认标记
            mAT_KINDEFDevLookUpEdit.EditValue       = cs_mat_kind;            //材料类型
            rAW_ORIGINEFDevRadioGroup.EditValue     = cs_raw_origin;          //原料来源
            dEMAND_PLAN_NOEFDevLookUpEdit.EditValue = cs_demand_plan_no;      //计划是否
           
            //根据ED54配置,显示EFDevGrid列标题              
            EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efDevGrid_Mat }, new string[] { cs_function_id_inq }, cs_formPartition);
            
            // 绑定下拉框内容
            this.BindDataSource();

            //设置EFDevGrid除选择列外，其他列都不可编辑
            efDevGrid_Mat.SetAllColumnEditableWithoutSelection(false);
        }
        #endregion

        #region F2 查询
        private void FormMM0010A1_EF_DO_F2(object sender, EF.EF_Args e)
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

        #region F3 材料导入 准备
        private void FormMM0010A1_EF_PRE_DO_F3(object sender, EF.EF_Args e)
        {
            //设置材料导入信息的列标题
            EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efDevGrid_Mat }, new string[] { cs_function_id_ins }, cs_formPartition);

            this.EFMsgInfo = "材料导入 准备。";
        }
        #endregion

        #region F3 材料导入 确定
        private void FormMM0010A1_EF_DO_F3(object sender, EF.EF_Args e)
        {
            //判断是否有数据
            if (gridView_Mat.RowCount <= 0)
            {
                EF.EFMessageBox.Show("请先从EXCEL导入外购材料", EF.EF_Args.epEname, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            //判断是否有选中行
            if (efDevGrid_Mat.GetSelectedDataRow().Rows.Count <= 0)
            {
                this.EFMsgInfo = "请选择需操作的记录。";
                MessageBox.Show("请选择需操作的记录。");
                this.ef_args.buttonStatusHold = true;
                return;
            }

            // 设置传入参数 
            EI.EIInfo inBlock = new EI.EIInfo();
            inBlock.Tables[0].Merge(efDevGrid_Mat.GetSelectedDataRow());

            //调用 SERVICE
            EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0010a1f3_ins", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            //判断调用是否正确
            if (outBlock.sys_info.flag < 0)
            {
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            //需重设查询信息的列标题,不重设则会显示材料导入信息的列标题
            EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efDevGrid_Mat }, new string[] { cs_function_id_inq }, cs_formPartition);

            // 刷新画面
            this.FormMM0010A1_EF_DO_F2(sender, e);

            this.EFMsgInfo = "材料导入 成功。";

        }
        #endregion

        #region F3 材料导入 取消
        private void FormMM0010A1_EF_CANCEL_DO_F3(object sender, EF.EF_Args e)
        {
            //需重设查询信息的列标题,不重设则会显示材料导入信息的列标题
            EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efDevGrid_Mat }, new string[] { cs_function_id_inq }, cs_formPartition);

            // 按当前页刷新画面
            QueryMat(iPageIndex);

            this.EFMsgInfo = "材料导入 取消。";
        }
        #endregion

        #region F4 设定目标库区 确定
        private void FormMM0010A1_EF_DO_F4(object sender, EF.EF_Args e)
        {
            if (cs_affirm_flag.Trim() == "Y")
            {
                MessageBox.Show("当前材料是已确认数据,不能操作!请选择未确认数据进行操作!");
                this.EFMsgInfo = "当前材料是已确认数据,不能操作!请选择未确认数据进行操作!";
                //控件旁边的惊叹号。
                this.aFFIRM_FLAGEFDevLookUpEdit.ErrorIcon = (new ErrorProvider()).Icon.ToBitmap(); //设置惊叹号
                this.aFFIRM_FLAGEFDevLookUpEdit.ErrorText = this.EFMsgInfo;
                return;
            }

            if (this.sTOCK_NO_TOEFDevTextEdit.Text == null || this.sTOCK_NO_TOEFDevTextEdit.Text.Trim() == "")
            {
                this.EFMsgInfo = "目的库区不能为空!";
                //控件旁边的惊叹号。
                this.sTOCK_NO_TOEFDevTextEdit.ErrorIcon = (new ErrorProvider()).Icon.ToBitmap(); //设置惊叹号
                this.sTOCK_NO_TOEFDevTextEdit.ErrorText = this.EFMsgInfo;
                return;
            }

            // 判断是否有选中行
            if (this.efDevGrid_Mat.GetSelectedDataRow().Rows.Count <= 0)
            {
                this.EFMsgInfo = "请选择需操作的记录。";
                MessageBox.Show("请选择需操作的记录。");
                this.ef_args.buttonStatusHold = true;
                return;

            }

            // 设置传入参数 
            EI.EIInfo inBlock = new EI.EIInfo();
            inBlock.Tables[0].Merge(this.efDevGrid_Mat.GetSelectedDataRow());

            // 新增一个BLOCK 
            inBlock.Tables.Add();
            inBlock.Tables[1].Columns.Add("STOCK_NO_TO");          //目的库区
            inBlock.Tables[1].Rows.Add();

            inBlock.Tables[1].Rows[0]["STOCK_NO_TO"] = this.sTOCK_NO_TOEFDevTextEdit.Text;    

            //调用 SERVICE
            EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0010a1f4_pro", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            // 刷新画面
            this.FormMM0010A1_EF_DO_F2(null, null);

            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/; 
            this.ef_args.buttonStatusHold = false;  //按钮恢复

        }
        #endregion

        #region F5 材料删除 准备
        private void FormMM0010A1_EF_PRE_DO_F5(object sender, EF.EF_Args e)
        {
            if (cs_affirm_flag.Trim() == "Y")
            {
                MessageBox.Show("当前材料是已确认数据,不能操作!请选择未确认数据进行操作!");
                this.EFMsgInfo = "当前材料是已确认数据,不能操作!请选择未确认数据进行操作!";
                //控件旁边的惊叹号。
                this.aFFIRM_FLAGEFDevLookUpEdit.ErrorIcon = (new ErrorProvider()).Icon.ToBitmap(); //设置惊叹号
                this.aFFIRM_FLAGEFDevLookUpEdit.ErrorText = this.EFMsgInfo;
                return;
            }

            this.EFMsgInfo = "请选择未确认的材料进行操作!";
        }
        #endregion

        #region F5 材料删除 确定
        private void FormMM0010A1_EF_DO_F5(object sender, EF.EF_Args e)
        {
            if (cs_affirm_flag.Trim() == "Y")
            {
                MessageBox.Show("当前材料是已确认数据,不能操作!请选择未确认数据进行操作!");
                this.EFMsgInfo = "当前材料是已确认数据,不能操作!请选择未确认数据进行操作!";
                //控件旁边的惊叹号。
                this.aFFIRM_FLAGEFDevLookUpEdit.ErrorIcon = (new ErrorProvider()).Icon.ToBitmap(); //设置惊叹号
                this.aFFIRM_FLAGEFDevLookUpEdit.ErrorText = this.EFMsgInfo;
                return;
            }

            // 判断是否有选中行
            if (this.efDevGrid_Mat.GetSelectedDataRow().Rows.Count <= 0)
            {
                this.EFMsgInfo = "请选择需操作的记录。";
                MessageBox.Show("请选择需操作的记录。");
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
            inBlock.Tables[0].Merge(this.efDevGrid_Mat.GetSelectedDataRow());

            //调用 SERVICE
            EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition, "mm0010a1f5_del", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            // 刷新画面
            this.FormMM0010A1_EF_DO_F2(null, null);

            EFMsgInfo = GC.GCRS.GCRSC0000003/*删除成功。*/;
            this.ef_args.buttonStatusHold = false;  //按钮恢复

        }
        #endregion

        #region F5 材料删除 取消
        private void FormMM0010A1_EF_CANCEL_DO_F5(object sender, EF.EF_Args e)
        {
            this.EFMsgInfo = GC.GCRS.GCRSC0000024/*删除操作取消。*/;
        }
        #endregion   

        #region F6 进料收池 确定
        private void FormMM0010A1_EF_DO_F6(object sender, EF.EF_Args e)
        {
            if (cs_affirm_flag.Trim() == "Y")
            {
                MessageBox.Show("当前材料是已确认数据,不能操作!请选择未确认数据进行操作!");
                this.EFMsgInfo = "当前材料是已确认数据,不能操作!请选择未确认数据进行操作!";
                //控件旁边的惊叹号。
                this.aFFIRM_FLAGEFDevLookUpEdit.ErrorIcon = (new ErrorProvider()).Icon.ToBitmap(); //设置惊叹号
                this.aFFIRM_FLAGEFDevLookUpEdit.ErrorText = this.EFMsgInfo;
                return;
            }
            //判断是否有选中行
            if (this.efDevGrid_Mat.GetSelectedDataRow().Rows.Count <= 0)
            {
                this.EFMsgInfo = "请选择需操作的记录。";
                MessageBox.Show("请选择需操作的记录。");
                this.ef_args.buttonStatusHold = true;
                return;
            }
            //确认行不能大于100行
            if (this.efDevGrid_Mat.GetSelectedDataRow().Rows.Count > 100)
            {
                this.EFMsgInfo = "请选择小于100条记录进行确认操作。";
                MessageBox.Show("请选择小于100条记录进行确认操作。");
                this.ef_args.buttonStatusHold = true;
                return;
            }

            // 设置传入参数 
            EI.EIInfo inBlock = new EI.EIInfo();
            inBlock.Tables[0].Merge(this.efDevGrid_Mat.GetSelectedDataRow());

            //调用 SERVICE
            EI.EIInfo outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"mm0010a1f6_pro", inBlock);
            this.EFMsgInfo = outBlock.sys_info.msg + "\n" + outBlock.sys_info.sysmsg;

            //判断调用是否正确
            if (outBlock.sys_info.flag < 0)
            {
                MessageBox.Show(this.EFMsgInfo);
                this.ef_args.buttonStatusHold = true;//封锁BUTTON的控制权。
                return;
            }

            // 刷新画面
            this.FormMM0010A1_EF_DO_F2(sender, e);
            EFMsgInfo = GC.GCRS.GCRSC0000009/*处理成功。*/;

            // 调用进料计划号编辑画面
           EF.EF_Args.common_parameter_1 = outBlock.GetColVal(1, 1, "DEMAND_PLAN_NO").ToString().Trim();
           this.EFCallForm("MM0010B1");
 
        }
        #endregion

        #region 材料类型选择 mAT_KINDEFDevLookUpEdit_EditValueChanged
        private void mAT_KINDEFDevLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (this.mAT_KINDEFDevLookUpEdit.EditValue == null)
            {
                this.EFMsgInfo = "材料类型不能为空，必须选择！";
                return;
            }
            cs_mat_kind         = this.mAT_KINDEFDevLookUpEdit.EditValue.ToString();    //材料类型
            cs_function_id_inq  = "MM0010A1_INQ" + "_" + cs_mat_kind;                   //MM0010A1_INQ_HR，MM0010A1_INQ_CR，MM0010A1_INQ_SM
            
            //设置Grid的列名
            EF.Utility.SetGridColumn(new EF.EFDevGrid[] { efDevGrid_Mat }, new string[] { cs_function_id_inq }, cs_formPartition);
        }
        #endregion

        #region  翻页事件
        //关于翻页的首页 尾页 向上 向下翻页功能
        private void efDevGrid_Mat_EF_GridBar_First_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
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

        private void efDevGrid_Mat_EF_GridBar_Last_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
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

        private void efDevGrid_Mat_EF_GridBar_NextPage_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
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

        private void efDevGrid_Mat_EF_GridBar_PageTo_Event(object sender, EF.EFDevGrid.EFNavigatorButtonClickEventArgs e)
        {
            try
            {
                this.efDevGrid_Mat.PageSize = e.PageSize;
                iPageIndex = e.PageTo;
                QueryMat(iPageIndex);
            }
            catch (Exception ex)
            {
                EF.EFMessageBox.Show(ex.Message);
            }

        }

        private void efDevGrid_Mat_EF_GridBar_PrePage_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
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

        private void efDevGrid_Mat_EF_GridBar_Refresh_Event(object sender, DevExpress.XtraEditors.NavigatorButtonClickEventArgs e)
        {
            QueryMat(iPageIndex);
        }
        #endregion

    }
}


