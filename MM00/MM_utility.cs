using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Repository;

namespace MM
{

    public class MM_utility
    {

        #region 根据Grid中的生产时刻计算班次班组日期 DEV_SetGridShiftNoGroupDateByTime
        /// <summary>
        /// 根据Grid中的生产时刻,赋值Grid的班次（PROD_SHIFT_NO）、班组（PROD_SHIFT_GROUP）、日期（PROD_DATE）。
        /// 前提：GRID中需要有PROD_SHIFT_NO，PROD_SHIFT_GROUP，PROD_DATE 列
        /// </summary>
        /// <param name="v_formPartition">分区名</param>
        /// <param name="efGrid_now">Grid名</param>
        /// <param name="v_mat_line_type">产线名</param>
        /// <param name="v_prod_time">Grid配置的生产时刻列名</param>
        public static void DEV_SetGridShiftNoGroupDateByTime(string v_formPartition,EF.EFDevGrid efGrid_now, string v_mat_line_type, string v_prod_time_item_ename)
        {
            //若该字段不存在，直接离开
            if (!EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.ContainsKey(v_prod_time_item_ename))
            {
                return;  
            }

            string s_prod_time = EFX.EFCGrid.GetEFCGridBase(efGrid_now).GetGridCellValue(0, v_prod_time_item_ename).ToString();
            if (s_prod_time.Trim().Length != 14)
            {
                return;  
            }
            var d_prod_time = DateTime.ParseExact(s_prod_time, "yyyyMMddHHmmss", null);

            string s_prod_shift_no = "";
            string s_prod_shift_group = "";
            string s_prod_shift_date = ""; //this.EFEName.Substring(2, 2)
            EF.Utility.GetPartitionShiftNoAndShiftGroupAndDate(v_formPartition,d_prod_time, v_mat_line_type, ref s_prod_shift_no, ref s_prod_shift_group, ref s_prod_shift_date);

            if (EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.ContainsKey("PROD_SHIFT_NO")
             && s_prod_shift_no.Trim() != "")
            {
                EFX.EFCGrid.GetEFCGridBase(efGrid_now).SetGridCellValue(0, "PROD_SHIFT_NO", s_prod_shift_no);
            }

            if (EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.ContainsKey("PROD_SHIFT_GROUP")
             && s_prod_shift_group.Trim() != "")
            {
                EFX.EFCGrid.GetEFCGridBase(efGrid_now).SetGridCellValue(0, "PROD_SHIFT_GROUP", s_prod_shift_group);
            }

            if (EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.ContainsKey("PROD_DATE")
             && s_prod_shift_date.Trim() != "")
            {
                s_prod_shift_date = s_prod_shift_date + "000000";
                var d_prod_shift_date = DateTime.ParseExact(s_prod_shift_date, "yyyyMMddHHmmss", null);
                EFX.EFCGrid.GetEFCGridBase(efGrid_now).SetGridCellValue(0, "PROD_DATE", d_prod_shift_date);
            }

            efGrid_now.FocusedView.RefreshData();
  
        }
        #endregion

        #region 根据Grid中的生产时刻计算班次班组日期 DEV_SetGridShiftNoGroupDateByTime1
        /// <summary>
        /// 根据Grid中的生产时刻,赋值Grid的班次（PROD_SHIFT_NO）、班组（PROD_SHIFT_GROUP）、日期（PROD_DATE）。
        /// 前提：GRID中需要有PROD_SHIFT_NO，PROD_SHIFT_GROUP，PROD_DATE 列
        /// </summary>
        /// <param name="v_formPartition">分区名</param>
        /// <param name="efGrid_now">Grid名</param>
        /// <param name="v_mat_line_type">产线名</param>
        /// <param name="v_prod_time">Grid配置的生产时刻列名</param>
        /// <param name="v_prod_shift_no">Grid配置的入炉班次名</param>
        /// <param name="v_prod_shift_group">Grid配置的入炉班次名</param>
        public static void DEV_SetGridShiftNoGroupDateByTime(string v_formPartition,EF.EFDevGrid efGrid_now, string v_mat_line_type, string v_prod_time_item_ename,string v_prod_shift_no,string v_prod_shift_group)
        {
            //若该字段不存在，直接离开
            if (!EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.ContainsKey(v_prod_time_item_ename))
            {
                return;
            }

            string s_prod_time = EFX.EFCGrid.GetEFCGridBase(efGrid_now).GetGridCellValue(0, v_prod_time_item_ename).ToString();
            if (s_prod_time.Trim().Length != 14)
            {
                return;
            }
            var d_prod_time = DateTime.ParseExact(s_prod_time, "yyyyMMddHHmmss", null);

            string s_prod_shift_no = "";
            string s_prod_shift_group = "";
            string s_prod_shift_date = ""; //this.EFEName.Substring(2, 2)
            EF.Utility.GetPartitionShiftNoAndShiftGroupAndDate(v_formPartition,d_prod_time, v_mat_line_type, ref s_prod_shift_no, ref s_prod_shift_group, ref s_prod_shift_date);

            if (EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.ContainsKey(v_prod_shift_no)
             && s_prod_shift_no.Trim() != "")
            {
                EFX.EFCGrid.GetEFCGridBase(efGrid_now).SetGridCellValue(0, v_prod_shift_no, s_prod_shift_no);
            }

            if (EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.ContainsKey(v_prod_shift_group)
             && s_prod_shift_group.Trim() != "")
            {
                EFX.EFCGrid.GetEFCGridBase(efGrid_now).SetGridCellValue(0, v_prod_shift_group, s_prod_shift_group);
            }

            if (EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.ContainsKey("PROD_DATE")
             && s_prod_shift_date.Trim() != "")
            {
                s_prod_shift_date = s_prod_shift_date + "000000";
                var d_prod_shift_date = DateTime.ParseExact(s_prod_shift_date, "yyyyMMddHHmmss", null);
                EFX.EFCGrid.GetEFCGridBase(efGrid_now).SetGridCellValue(0, "PROD_DATE", d_prod_shift_date);
            }

            efGrid_now.FocusedView.RefreshData();

        }
        #endregion

        #region 根据Grid中的生产时刻计算班次班组日期有报错信息 DEV_SetGridShiftNoGroupDateByTime
        /// <summary>
        /// 根据Grid中的生产时刻,赋值Grid的班次（PROD_SHIFT_NO）、班组（PROD_SHIFT_GROUP）、日期（PROD_DATE）。
        /// 前提：GRID中需要有PROD_SHIFT_NO，PROD_SHIFT_GROUP，PROD_DATE 列
        /// </summary>
        /// <param name="v_formPartition">分区名</param>
        /// <param name="efGrid_now">Grid名</param>
        /// <param name="v_mat_line_type">产线名</param>
        /// <param name="v_prod_time">Grid配置的生产时刻列名</param>
        /// <param name="r_error_message">返回出错原因</param>
        public static void DEV_SetGridShiftNoGroupDateByTime(string v_formPartition,EF.EFDevGrid efGrid_now, string v_mat_line_type, string v_prod_time_item_ename, ref string r_error_message)
        {
            //若该字段不存在，直接离开
            if (!EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.ContainsKey(v_prod_time_item_ename))
            {
                if (r_error_message != null)
                {
                    r_error_message = "传入的Grid中无该列[" + v_prod_time_item_ename + "]";
                }
                return;
            }

            string s_prod_time = EFX.EFCGrid.GetEFCGridBase(efGrid_now).GetGridCellValue(0, v_prod_time_item_ename).ToString();
            if (s_prod_time.Trim().Length != 14)
            {
                if (r_error_message != null)
                {
                    r_error_message = "传入的Grid中该列[" + v_prod_time_item_ename + "]的值[" + s_prod_time + "]应该为14位字符型!";
                }
                return;
            }
            var d_prod_time = DateTime.ParseExact(s_prod_time, "yyyyMMddHHmmss", null);

            string s_prod_shift_no = "";
            string s_prod_shift_group = "";
            string s_prod_shift_date = ""; //this.EFEName.Substring(2, 2)
            EF.Utility.GetPartitionShiftNoAndShiftGroupAndDate(v_formPartition,d_prod_time, v_mat_line_type, ref s_prod_shift_no, ref s_prod_shift_group, ref s_prod_shift_date);

            if (EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.ContainsKey("PROD_SHIFT_NO"))
            {

                if (s_prod_shift_no.Trim() != "")
                {
                    EFX.EFCGrid.GetEFCGridBase(efGrid_now).SetGridCellValue(0, "PROD_SHIFT_NO", s_prod_shift_no);
                }
                else
                {
                    r_error_message = "未计算出班次的值!";
                }
                
            }
            else
            {
                r_error_message = "传入的Grid中无该列[PROD_SHIFT_NO]，因此该列无值!";
            }

            if (EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.ContainsKey("PROD_SHIFT_GROUP"))
            {
                if (s_prod_shift_group.Trim() != "")
                {
                    EFX.EFCGrid.GetEFCGridBase(efGrid_now).SetGridCellValue(0, "PROD_SHIFT_GROUP", s_prod_shift_group);
                }
                else
                {
                    r_error_message = "未计算出班组的值!";
                }
            }
            else
            {
                r_error_message = "传入的Grid中无该列[PROD_SHIFT_GROUP]，因此该列无值!";
            }

            if (EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.ContainsKey("PROD_DATE"))
            {
                if (s_prod_shift_date.Trim() != "")
                {
                    s_prod_shift_date = s_prod_shift_date + "000000";
                    var d_prod_shift_date = DateTime.ParseExact(s_prod_shift_date, "yyyyMMddHHmmss", null);
                    EFX.EFCGrid.GetEFCGridBase(efGrid_now).SetGridCellValue(0, "PROD_DATE", d_prod_shift_date);
                }
                else
                {
                    r_error_message = "未计算出班次日期的值!";
                }
            }
            else
            {
                r_error_message = "传入的Grid中无该列[PROD_DATE]，因此该列无值!";
            }

            efGrid_now.FocusedView.RefreshData();

        }
        
        #endregion

        
        //public static string v_para = "";

        //public static string common_parameter_1 = "";
        //public static string common_parameter_2 = "";
        //public static string common_parameter_3 = "";
        //public static string common_parameter_4 = "";

        //public static string common_parameter_5 = "";
        //public static string common_parameter_6 = "";
        //public static string common_parameter_7 = "";
        //public static string common_parameter_8 = "";
        //public static string common_parameter_9 = "";










        //========================CPP 程序重建，利用最新的DEV 控件。===================BY ZY ON 2011-11-29 14:48:40

        //下拉列表可输入性设置。
        //======================
        /*
         //设置出钢记号控件可画面输入
            this.st_no.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

            //设置出钢记号控件画面输入时不会自动弹出下拉列表
            this.st_no.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup;

            //将出钢记号编辑输入事件都委托给事件st_no_ProcessNewValue
            this.st_no.ProcessNewValue += new DevExpress.XtraEditors.Controls.ProcessNewValueEventHandler(st_no_ProcessNewValue);
         * 
           private void lookStNo_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            DataTable dt = this.lookStNo.Properties.DataSource as DataTable;
            DataRow dr = dt.NewRow();
            dr["ST_NO"] = e.DisplayValue.ToString();
            dt.Rows.Add(dr);
            e.Handled = true;
        }

         */

        // /// <summary>
        // /// 下拉列表控件的可输入性设置。
        // /// </summary>
        // /// <param name="lookupedit_now"></param>
        // public static void DEV_Textedit_LookUpEdit(EF.EFDevLookUpEdit lookupedit_now)
        // {
        //     lookupedit_now.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
        //     lookupedit_now.Properties.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup;
        //     lookupedit_now.ProcessNewValue += new DevExpress.XtraEditors.Controls.ProcessNewValueEventHandler(lookupedit_now_ProcessNewValue);
        // }


        //// #region LOOKUPDEIT出钢记号手工输入跳空解决
        // private void lookupedit_now_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e, EF.EFDevLookUpEdit lookupedit_now)
        // {
        //     DataTable dt = lookupedit_now.Properties.DataSource as DataTable;
        //     DataRow dr = dt.NewRow();
        //     dr["ST_NO"] = e.DisplayValue.ToString();
        //     dt.Rows.Add(dr);
        //     e.Handled = true;
        // }
        // //#endregion 



        ///// <summary>
        ///// 设定GRID中，某个[日期]类型的列字段的[初始值]
        ///// </summary>
        ///// <param name="efgridNow">指定的GRID</param>
        ///// <param name="col_name">指定列信息</param>
        ///// <param name="v_dateTime">时间变量</param>
        //public static void DEV_initCol_date(EF.EFDevGrid efgridNow, string col_name, System.DateTime v_dateTime)
        //{

        //    try
        //    {


        //        //先判断，该列是否存在。
        //        //==============EFX.EFCGrid.GetEFCGridBase(efDevGrid1).Columns.ContainsKey("XXX")
        //        string item_ename = "";
        //        item_ename = col_name.ToUpper();
        //        if (!EFX.EFCGrid.GetEFCGridBase(efgridNow).Columns.ContainsKey(item_ename)) return;  //若该字段不存在，直接离开


        //        //日期类型的列信息初始化处理。
        //        //========== 
        //        //grid中的日期字段= 。 
        //        string item_value = ""; 
        //        item_value = v_dateTime.ToString("yyyy-MM-dd");//日期类型转换成字符型。
        //        if (EFX.EFCGrid.GetEFCGridBase(efgridNow).Columns.ContainsKey(item_ename))
        //        {//若该字段存在，则复制。
        //            EFX.EFCGrid.GetEFCGridBase(efgridNow).SetGridCellValue(0, item_ename, item_value);
        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        GC.PM_utility2.Dev_messageBoxError(ex.Message);
        //    }
        //}


        //        /// <summary>
        //        /// 实现GRID 中指定列的多行显示模式。
        //        /// </summary>
        //        /// <param name="efGrid_now"></param>
        //        /// <param name="v_function_id"></param>
        //        public static void DEV_SetCol_MenoEdit(EF.EFDevGrid efGrid_now, string v_function_id)
        //        {

        //            //实现GRID 中列的多行显示模式。
        //            DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;



        //            EI.EIInfo inBlock = new EI.EIInfo();
        //            EI.EIInfo outBlock;
        //            int i = 0;
        //            string v_item_ename = "";


        //            //采用框架提供的动态SQL 的service 。
        //            //==================
        //            string v_sql = "";//  
        //            v_sql = " SELECT item_ename FROM ted54 "
        //                  + " WHERE  UPPER(func_id) = UPPER('" + v_function_id + "') " //根据指定的功能号。
        //                  + " AND    condition_flag = '1'              "               //借用此标记，作为多行显示模式。
        //                  + " ORDER  BY class_code ,seq_no             ";

        //            i = 1;
        //            inBlock.SetColName(1, i++, "v_sql");
        //            inBlock.SetColVal(1, 1, "v_sql", v_sql);// 
        //            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"eped_dyn_sql", inBlock);

        //            //实现GRID 中指定列的多行显示模式。
        //            //=====================以下
        //            //==以下列信息被设置成多行编辑框模式。
        //            //======================================
        //            //列高度自动。
        //            gridView_now.OptionsView.RowAutoHeight = true;

        //            //1#BLK, 返回的是指定功能号下可编辑的列信息。
        //            for (i = 0; i < outBlock.blk_info[0].row; i++)
        //            {//1#BLK, 返回的是指定功能号下可编辑的列信息。
        //                v_item_ename = outBlock.Tables[0].Rows[i]["item_ename"].ToString().Trim().ToUpper();

        //                if (!EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.ContainsKey(v_item_ename))
        //                {//若指定列不存在，则直接下一个。
        //                    continue;
        //                }

        //                //columnEdit==,设置成MemoEdit// Rep**MenoEdit 多行编辑框
        //                gridView_now.Columns[v_item_ename].ColumnEdit = new RepositoryItemMemoEdit();
        //               // gridView_now.Columns[v_item_ename].OptionsColumn.AllowSort = flase ,则不允许排序。

        //            }

        //            //实现GRID 中列的多行显示模式。
        //            //=====================以上
        //        }




        //        /// <summary>
        //        ///  GRID中,指定列信息显示进度条
        //        /// </summary>
        //        /// <param name="efGrid_now">指定的GRID</param>
        //        /// <param name="v_col_name">分子列名称</param>
        //        /// <param name="v_col_name2">分母列名称</param>
        //        /// <param name="v_col_percent">进度条显示的列名称</param>
        //        public static void DEV_SetCol_Percent(EF.EFDevGrid efGrid_now, string v_col_name, string v_col_name2, string v_col_percent)
        //        {
        //            DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
        //            if (gridView_now.RowCount == 0)
        //            {
        //                return;
        //            }



        //            double v_col_value = 0; //分子值
        //            double v_col_value2 = 0;//分母值
        //            int   v_percent = 0;    //百分比。



        //            /*
        //             //返回===TC_INIT/TC/TC_REMAIN
        //                //==========
        //                v_tc_init = 0;
        //                v_tc = 0;
        //                v_tc_remain = 0; 
        //             */

        //            for (int i = 0; i < gridView_now.RowCount; i++)
        //            {
        //                v_col_value = Convert.ToDouble(gridView_now.GetRowCellValue(i, v_col_name).ToString());//分子值
        //                v_col_value2 = Convert.ToDouble(gridView_now.GetRowCellValue(i, v_col_name2).ToString());//分母值。

        //                if (v_col_value2 <= 0)
        //                {//若分母值小于等于0 ，则百分比= 100
        //                    v_percent = 100;
        //                }
        //                else
        //                {
        //                    v_percent = Convert.ToInt32(Math.Round(v_col_value / v_col_value2, 2) * 100);
        //                    if (v_percent >= 100)
        //                    {//若百分比大于等于100 ，则百分比= 100
        //                        v_percent = 100;
        //                    }
        //                }

        //                ////定义一个ProgressBar字段类型。//进度条变量。。
        //                DevExpress.XtraEditors.Repository.RepositoryItemProgressBar progress_bar = new DevExpress.XtraEditors.Repository.RepositoryItemProgressBar();

        //                progress_bar.Minimum = 0;
        //                progress_bar.Maximum = 100;
        //                progress_bar.Step = progress_bar.Maximum;
        //                progress_bar.Appearance.ForeColor = Color.Orange;
        //                progress_bar.ShowTitle = true;
        //                progress_bar.ProgressViewStyle = DevExpress.XtraEditors.Controls.ProgressViewStyle.Solid;

        //                //efGrid_now.RepositoryItems.Add(progress_bar);                   
        //                gridView_now.Columns[v_col_percent].ColumnEdit = progress_bar;
        //                gridView_now.SetRowCellValue(i, v_col_percent, v_percent);

        //            }

        //        }






        //        /// <summary>
        //        /// 根据功能号，获取对应的要求合并的列信息。
        //        /// </summary>
        //        /// <param name="v_func_id"></param>
        //        /// <returns></returns>
        //        public static string[] DEV_GetMergeColName(string v_func_id)
        //        {


        //            EI.EIInfo inBlock = new EI.EIInfo();
        //            EI.EIInfo outBlock;
        //            int i = 0;
        //            string v_item_ename = "";


        //            //定义一个list变量。
        //            List<string> strlist = new List<string>();


        //            //采用框架提供的动态SQL 的service 。
        //            //==================
        //            //t.condition_flag = 借用此字段，若是1=说明是MERGE的基准列。 
        //            //t.item_key_flag  = 借用此字段，若是1=说明需要MERE。
        //            string v_sql = "";//  
        //            v_sql = " SELECT item_ename FROM ted54 "
        //                  + " WHERE  UPPER(func_id) = UPPER('" + v_func_id + "') " //根据指定的功能号。
        //                  + " AND    item_key_flag = '1'              "           //是否为主键= 1,说明需要主键合并单元格。
        //                  + " ORDER  BY condition_flag DESC ,class_code ,seq_no            ";

        //            i = 1;
        //            inBlock.SetColName(1, i++, "v_sql");
        //            inBlock.SetColVal(1, 1, "v_sql", v_sql);// 
        //            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"eped_dyn_sql", inBlock);


        //            //1#BLK, 返回的是指定功能号下可编辑的列信息。
        //            for (i = 0; i < outBlock.blk_info[0].row; i++)
        //            {//1#BLK, 返回的是指定功能号下可编辑的列信息。
        //                v_item_ename = outBlock.Tables[0].Rows[i]["item_ename"].ToString().Trim().ToUpper();
        //                //添加数组内容。
        //                strlist.Add(v_item_ename); 
        //            }


        //            //添加数组内容。
        //            strlist.Add(" "); 








        //            //返回数组。
        //            return strlist.ToArray();





        //        }


        //        /// <summary>
        //        /// 根据数组变量col_name[]中定义的列信息，进行单元格合并。
        //        /// </summary>
        //        /// <param name="efGrid_now"></param>
        //        /// <param name="col_name"></param>
        //        public static void DEV_AllowCellMerge(EF.EFDevGrid efGrid_now, string[] col_name)
        //        {//设置指定列的行信息相同，就合并单元格。
        //            try
        //            {
        //                int i = 0;
        //                string v_item_ename = "";//列名称。
        //                v_item_ename = EF.EFDevGrid.SelectionColumnFieldName; //复选框名称。

        //                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

        //                if (gridView_now == null) return;

        //                //若数组变量中没有信息，也直接返回。
        //                if (col_name.Length <= 0) return;

        //                //若数组变量中的第一个内容为空，则直接返回。
        //                if (col_name[0].ToString().Trim() == "") return;

        //                //使得当前VIEW 是允许单元格合并的。
        //                gridView_now.OptionsView.AllowCellMerge = true;



        //                //将VIEW中的所有列都不可合并。
        //                int cols = 0;
        //                for (cols = 0; cols < gridView_now.Columns.Count; cols++)
        //                {//列信息
        //                    gridView_now.Columns[cols].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;

        //                } 


        //                //根据数组中的信息，将指定列的MERGE属性打开。
        //                for (i = 0; i < col_name.Length; i++)
        //                {//使得多列为可写状态
        //                    v_item_ename = col_name[i].ToString().Trim().ToUpper();

        //                    //若是GRID 中不存在的字段，则继续下一个字段循环
        //                    if (!gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(v_item_ename))) continue;
        //                    gridView_now.Columns[v_item_ename].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True; //指定列可MERGE
        //                }





        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, "DEV_SetColEdit_multi");
        //            }




        //        }




        //        /// <summary>
        //        /// （根据主导列【数组中的第一列】，来决定其他允许合并的单元格是否进行合并处理。）
        //        /// </summary>
        //        /// <param name="efGrid_now"></param>
        //        /// <param name="merge_col_name"></param>
        //        /// <param name="e"></param>
        //        public static void DEV_ChkCellMerge(EF.EFDevGrid efGrid_now, string[] merge_col_name, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        //        {

        //            //最终显示效果（第二列根据第一列合并情况决定是否合并）
        //            //===DEMO===
        //            //if (e.Column == gridColumn2)
        //            //{
        //            //    var c1 = gridView1.GetRowCellValue(e.RowHandle1, gridColumn1);
        //            //    var c2 = gridView1.GetRowCellValue(e.RowHandle2, gridColumn1);

        //            //    e.Merge = c1.Equals(c2);
        //            //    e.Handled = true;

        //            //}

        //            try
        //            {

        //                int i = 0;
        //                string v_item_ename_chk = ""; //主导列。
        //                string v_item_ename = "";

        //                //若数组变量中没有信息，也直接返回。
        //                if (merge_col_name.Length <= 0) return; //直接离开。

        //                //获取 efGrid_nowd对应的View .
        //                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

        //                //string[] merge_col_name = new string[] { "DUMMY_MAT_NO", "TD_TYPE", "OU_STATUS" };// 
        //                //根据数组中的信息，将指定列的MERGE属性打开。
        //                for (i = 0; i < merge_col_name.Length; i++)
        //                {//使得多列为可写状态

        //                    //若是第一个字段，记录下。
        //                    if (i == 0)
        //                    {
        //                        //v_item_ename_chk = v_item_ename = merge_col_name[i].ToString().Trim().ToUpper();
        //                        v_item_ename_chk = merge_col_name[i].ToString().Trim().ToUpper();

        //                        //若是GRID 中不存在的字段，则直接离开本循环。
        //                        if (!gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(v_item_ename_chk))) break;

        //                    }
        //                    else
        //                    {//非第一个，进行判断是否MERGE.
        //                        v_item_ename = merge_col_name[i].ToString().Trim().ToUpper();

        //                        //若是GRID 中不存在的字段，则继续下一个字段循环
        //                        if (!gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(v_item_ename))) continue;

        //                        if (e.Column.FieldName == v_item_ename)
        //                        {
        //                            var c1 = gridView_now.GetRowCellValue(e.RowHandle1, v_item_ename_chk);
        //                            var c2 = gridView_now.GetRowCellValue(e.RowHandle2, v_item_ename_chk);

        //                            e.Merge = c1.Equals(c2);
        //                            e.Handled = true;
        //                        }
        //                    }


        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                GC.PM_utility2.Dev_messageBoxError(ex.Message);

        //            }

        //        }



        //        /// <summary>
        //        /// 对于MERGE的列信息进行SUM的特殊处理，防止MERGE后的数字信息累计量翻倍。
        //        /// </summary>
        //        /// <param name="col_name">合并的基准列</param>
        //        /// <param name="col_name2">合并的SUM列</param>
        //        /// <param name="dataTable1">数据表</param>
        //        /// <param name="e"></param>
        //        public static void DEV_SumCell_merge(string col_name,string col_name_sum,DataTable dataTable1 ,DevExpress.XtraGrid.Views.Grid.FooterCellCustomDrawEventArgs e)
        //        {

        //            //==DEMO===
        //            //if (e.Column == gridColumn2)
        //            //{
        //            //    var groupData = from row in dataTable1.AsEnumerable()
        //            //                    group row by new { c2 = row.Field<string>("C1") }
        //            //                        into g
        //            //                        select new { C2 = g.First().Field<int>("C2") };

        //            //    var count = groupData.Sum(x => x.C2);
        //            //    e.Info.DisplayText = count.ToString();
        //            //}

        //            //DataTable dataTable1 = new DataTable();

        //            //DataSet dataset1 = new DataSet();

        //            //dataset1 = this.efDevGrid1.GetGridValue();
        //            //dataTable1 = dataset1.Tables[0];
        //            try
        //            {


        //                //若对订货量，进行汇总计算，需要考虑 此列是否已经进行了MERGE处理。
        //                if (e.Column.FieldName == col_name_sum)
        //                {
        //                    var groupData = from row in dataTable1.AsEnumerable()
        //                                    group row by new { c2 = row.Field<string>(col_name) }
        //                                        into g
        //                                        select new { C2 = g.First().Field<double>(col_name_sum) };

        //                    var sum = groupData.Sum(x => x.C2);
        //                    e.Info.DisplayText = sum.ToString();

        //                }

        //            }
        //            catch (Exception ex)
        //            {

        //                GC.PM_utility2.Dev_messageBoxError(ex.Message);
        //            }


        //        }


        //        /// <summary>
        //        /// （根据主导列【数组中的第一列】，处理组别的颜色标志信息）
        //        /// </summary>
        //        /// <param name="merge_col_name"></param>
        //        /// <param name="outBlock"></param>
        //        public static void DEV_SetCellMergeColor(string[] merge_col_name,EI.EIInfo outBlock)
        //        {


        //            //此画面有列MERG的要求，此处，需要对分组信息进行颜色标志的设置。
        //            //COLOR_FLAG = 0/1/=不变色/变色
        //            //==============================
        //            int i = 0;
        //            string v_item_ename_chk = "";
        //            string v_item_value_chk = "";
        //            int v_group_num = 0; //组别。
        //            int v_color_num = 0;
        //            string v_color_flag = "COLOR_FLAG"; //颜色控制列名称。

        //            //DataTable dt_t1_temp = new DataTable();
        //            //dt_t1_temp = outBlock.Tables[0]; //service返回的0#BLK信息。

        //            DataTable dt_t1_temp = outBlock.Tables[0]; //service返回的0#BLK信息。

        //            //根据数组中的信息，将指定列的MERGE属性打开。

        //            v_item_ename_chk = merge_col_name[0].ToString().Trim().ToUpper(); //数组变量中的第一个字段是，分组关键字段。


        //            //若分组列[v_item_ename_chk]在DT中不存在，则直接离开。
        //            if (!dt_t1_temp.Columns.Contains(v_item_ename_chk))
        //            {//若不存在，直接离开。
        //                return;
        //            }

        //            //若颜色列[v_color_flag]在DT中不存在，则直接离开。 
        //            if (!dt_t1_temp.Columns.Contains(v_color_flag))
        //            {//若不存在，直接离开。
        //                return;
        //            }


        //            i = 0; //行号信息。
        //            foreach (DataRow dr in dt_t1_temp.Rows)
        //            {

        //                if (i == 0)
        //                {//若是返回的第一行，则记录下当前行的[分组字段]的内容。

        //                    v_item_value_chk = dr[v_item_ename_chk].ToString();
        //                    v_group_num = 0; // 第一组

        //                }
        //                else
        //                {//若不是第一行，则进行比较。 

        //                    if (v_item_value_chk != dr[v_item_ename_chk].ToString())
        //                    {
        //                        v_group_num++; //累计组别。
        //                        v_item_value_chk = dr[v_item_ename_chk].ToString();
        //                    }
        //                    else
        //                    {//若相同，就。。。

        //                    }

        //                }
        //                i++; //行信息的累计。

        //                //对[流号]的判断设定
        //                //===================
        //                /*流筛选器*/
        //                /* 
        //                2流的情况：流号 就在 0 和 1 之间切换。
        //                0=0%2;
        //                1=1%2;
        //                0=2%2;
        //                1=3%2;
        //                */
        //                v_color_num = v_group_num % 2;
        //                dr[v_color_flag] = v_color_num.ToString();

        //                //给颜色列赋值。
        //                dr[v_color_flag] = v_color_num.ToString();


        //            }// end for




        //        }





        //        /// <summary>
        //        /// GRID的设置分页显示信息
        //        /// 提示信息风格统一
        //        /// 小按钮变灰处理统一
        //        /// </summary>
        //        /// <param name="efGrid_now"></param>
        //        /// <param name="i_CurrentPage"></param>
        //        /// <param name="outBlock"></param>
        //        public static int DEV_grid_RecordCountMessage(EF.EFDevGrid efGrid_now, int i_CurrentPage, EI.EIInfo outBlock)
        //        {

        //            efGrid_now.InitRowOrdinal = (i_CurrentPage - 1) * efGrid_now.PageSize + 1;
        //            efGrid_now.TotalRecordCount = Convert.ToInt32(outBlock.Tables["PageInfo"].Rows[0]["TotalRecordCount"]);


        //            int i_TotalPageCount = 0;
        //            i_TotalPageCount = ((efGrid_now.TotalRecordCount - 1) / efGrid_now.PageSize) + 1;
        //            //efGrid_now.RecordCountMessage = string.Format("{0}/{1}({2})", i_CurrentPage, i_TotalPageCount, efGrid_now.TotalRecordCount);
        //            //Client为  GC.GCRS.GCRSC0000056/*第 {0}/{1} 页，共 {2} 条记录*/

        //            //若当前记录个数小于等于0 ，则，显示 [第0/0页，共0条记录]
        //            if (efGrid_now.TotalRecordCount <= 0)
        //            {
        //                i_CurrentPage = 0;  //当前页数
        //                i_TotalPageCount = 0;  //总页数
        //            }
        //            efGrid_now.RecordCountMessage = string.Format(GC.GCRS.GCRSC0000056, i_CurrentPage, i_TotalPageCount, efGrid_now.TotalRecordCount);


        //            //控制分页小按钮，
        //            /*
        //             处于  第一页的时候，第一页和上一页的按键【变灰】，
        //             处于最后一页的时候，下一页和最后一页按键【变灰】
        //             */

        //            //默认全显示。
        //            efGrid_now.FirstPageButtonEnable = true; //第一页
        //            efGrid_now.PrePageButtonEnable = true;   //上一页
        //            efGrid_now.LastPageButtonEnable = true;  //最后一页
        //            efGrid_now.NextPageButtonEnable = true;  //下一页


        //            if (i_CurrentPage<= 1 )
        //            {//处于  第一页的时候，第一页和上一页的按键【变灰】，

        //                efGrid_now.FirstPageButtonEnable = false; //第一页
        //                efGrid_now.PrePageButtonEnable = false;   //上一页 
        //            }

        //            if (i_CurrentPage == i_TotalPageCount)
        //            {// 处于最后一页的时候，下一页和最后一页按键【变灰】

        //                efGrid_now.LastPageButtonEnable = false;  //最后一页
        //                efGrid_now.NextPageButtonEnable = false;  //下一页
        //            }




        //            return i_TotalPageCount;

        //        }


        //        //DEV_Init_LayoutGroup
        //        /// <summary>
        //        /// 设置整个FORM，总LAYOUT的格式信息,设置打开的小画面。
        //        /// </summary>
        //        /// <param name="form_now"></param>
        //        /// <param name="layoutControl_now"></param>
        //        public static void DEV_Init_Form_Pop(EF.EFForm form_now, DevExpress.XtraLayout.LayoutControl layoutControl_now)
        //        {


        //            //设置FORM 的格式
        //            form_now.Location = new System.Drawing.Point(0, 0);
        //            form_now.Size = new System.Drawing.Size(700, 339);
        //            form_now.Padding = new Padding(0, 0, 0, 0);

        //            //最底层的layoutControl 设定。
        //            layoutControl_now.Location = new System.Drawing.Point(-7, -7);
        //            layoutControl_now.Size = new System.Drawing.Size(700, 264);
        //            layoutControl_now.Margin = new Padding(0, 0, 0, 0);



        //        }



        //        /// <summary>
        //        /// 设置整个FORM，总LAYOUT的格式信息。
        //        /// </summary>
        //        /// <param name="form_now"></param>
        //        /// <param name="layoutControl_now"></param>
        //        public static void DEV_Init_Form(EF.EFForm form_now, DevExpress.XtraLayout.LayoutControl layoutControl_now)
        //        {

        //            ////设置FORM 的格式。
        //            //form_now.Location = new Point(0, 0);
        //            //form_now.Size = new Size(1000, 639);

        //            ////总LAYOUT 的设定。
        //            //layoutControl_now.Location = new Point(-5, -6); 
        //            //layoutControl_now.Size = new Size(1000, 570); //1005, 567 

        //            // this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
        //            //this.layoutControlGroup1.Size = new System.Drawing.Size(1005, 564);

        //            //设置FORM 的格式
        //            form_now.Location = new System.Drawing.Point(0, 0);
        //            form_now.Size = new System.Drawing.Size(1000, 639);
        //            form_now.Padding = new Padding(0, 0, 0, 0);

        //            //最底层的layoutControl 设定。
        //            layoutControl_now.Location = new System.Drawing.Point(-7, -8);
        //            layoutControl_now.Size = new System.Drawing.Size(1005, 576);
        //            layoutControl_now.Margin = new Padding(0, 0, 0, 0);



        //            // //显示【确认】+【取消】按钮。
        //            ////this.SetOkCancelVisible(true);
        //            form_now.SetOkCancelVisible(true);



        //        }





        //        /// <summary>
        //        /// 统一初始化FORM 的风格=2.0版
        //        /// </summary>
        //        /// <param name="form_now"></param>
        //        /// <param name="layoutControl_now"></param>
        //        /// <param name="layoutControlGroup_now"></param>
        //        public static void DEV_Init_Form2(EF.EFForm form_now, DevExpress.XtraLayout.LayoutControl layoutControl_now, DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup_now)
        //        {

        //            /*
        //             //若当前画面的MdiParent 是XXX ,则设定XXX的尺寸，即可。
        //                if (this.MdiParent.Name == "FormSDIHost")
        //                {
        //                    this.MdiParent.Size = new Size(1008, 608);
        //                }

        //             */

        //            //设置FORM 的格式
        //            form_now.Location = new System.Drawing.Point(0, 0);
        //            form_now.Padding = new Padding(0, 0, 0, 0);
        //            form_now.Size = new System.Drawing.Size(1008, 608);


        //            //if (form_now.MdiParent.Name == "FormSDIHost")
        //            //{//若当前是弹出画面，那么，设定弹出画面的位置，画面尺寸。 
        //            //    form_now.Location = new System.Drawing.Point(0, 0);
        //            //    form_now.MdiParent.Size = new Size(1008, 608);
        //            //}


        //            //最底层的layoutControlGroup 的设定。 
        //            layoutControlGroup_now.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
        //            layoutControlGroup_now.Location = new System.Drawing.Point(0, 0);
        //            layoutControlGroup_now.Size = new System.Drawing.Size(992, 524);
        //            layoutControlGroup_now.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);



        //            /*
        //             this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0); 
        //             this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
        //             this.layoutControlGroup1.Size = new System.Drawing.Size(992, 524);
        //             this.layoutControlGroup1.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);

        //             */


        //            //最底层的layoutControl 设定。
        //            layoutControl_now.Location = new System.Drawing.Point(0, 0);
        //            layoutControl_now.Size = new System.Drawing.Size(992, 524);
        //            layoutControl_now.Dock = DockStyle.None;
        //            //layoutControl_now.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
        //            layoutControl_now.Margin = new Padding(0, 0, 0, 0);



        //            //锚定
        //            layoutControl_now.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        //                       | System.Windows.Forms.AnchorStyles.Left)
        //                       | System.Windows.Forms.AnchorStyles.Right)));


        //        }




        //        // a）	警告类
        //        //EF.EFMessageBox.Show("确认要删除订单 XXXXXX 吗？", EF.EF_Args.epEname,MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
        //        //b）	问题类
        //        //EF.EFMessageBox.Show("是否要保存更改？", EF.EF_Args.epEname, MessageBoxButtons.YesNo,MessageBoxIcon.Question);
        //        //c）	提示类
        //        //EF.EFMessageBox.Show("新增成功。", EF.EF_Args.epEname, MessageBoxButtons.OK,MessageBoxIcon.Information);
        //        //d）	错误类
        //        //EF.EFMessageBox.Show("具体的出错信息", EF.EF_Args.epEname, MessageBoxButtons.OK, MessageBoxIcon.Error);




        //        /// <summary>
        //        /// 警告类[提示框],提供【是】【否】按钮。
        //        /// </summary>
        //        /// <param name="v_msg">提示的内容</param>
        //        /// 
        //        public static Boolean Dev_messageBoxWarning(string v_msg)
        //        {
        //            //警告类，惊叹号
        //            // DialogResult result = EF.EFMessageBox.Show(v_msg, EF.EF_Args.epEname, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);            

        //            //if( result == DialogResult.No)
        //            //{ 
        //            //    return false;
        //            //}
        //            //else
        //            //{
        //            //    return true;
        //            //}


        //            //问题类，问号
        //            DialogResult result = EF.EFMessageBox.Show(v_msg, EF.EF_Args.epEname, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        //            if (result == DialogResult.No)
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                return true;
        //            }


        //        }

        //        /// <summary>
        //        /// 问题类[提示框],提供【是】【否】按钮。
        //        /// </summary>
        //        /// <param name="v_msg">提示的内容</param>
        //        public static Boolean Dev_messageBoxQuestion(string v_msg)
        //        {
        //            DialogResult result = EF.EFMessageBox.Show(v_msg, EF.EF_Args.epEname, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

        //            if (result == DialogResult.No)
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                return true;
        //            }
        //        }

        //        /// <summary>
        //        /// 提示类[提示框],提供【确认】按钮
        //        /// </summary>
        //        /// <param name="v_msg">提示的内容</param>
        //        public static void Dev_messageBoxInfo(string v_msg)
        //        {
        //            EF.EFMessageBox.Show(v_msg, EF.EF_Args.epEname, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }

        //        /// <summary>
        //        /// 错误类[提示框],提供【确认】按钮
        //        /// </summary>
        //        /// <param name="v_msg">提示的内容</param>
        //        public static void Dev_messageBoxError(string v_msg)
        //        {
        //            //EF.EFMessageBox.Show(v_msg, EF.EF_Args.epEname, MessageBoxButtons.OK, MessageBoxIcon.Error);

        //            EF.EFMessageBox.Show(v_msg, EF.EF_Args.epEname, MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }





        //        /// <summary>
        //        /// 加载GRID 中的某个字段下拉列表信息。
        //        /// </summary>
        //        /// <param name="efgridNow">指定的GRID </param>
        //        /// <param name="col_name">指定的列</param>
        //        /// <param name="outBlock">外部传入的数据集</param> 
        //        /// <param name="out_para">显示列</param>
        //        /// <param name="out_para2">显示列数组</param>
        //        /// <param name="out_para_title2">显示列标题数组</param>
        //        public static void DEV_initCol_LookUpEdit3(EF.EFDevGrid efgridNow, string col_name, EI.EIInfo outBlock, string out_para, string[] out_para2, string[] out_para_title2)
        //        {

        //            try
        //            {

        //                string item_ename = "";
        //                item_ename = col_name.ToUpper();
        //                if (!EFX.EFCGrid.GetEFCGridBase(efgridNow).Columns.ContainsKey(item_ename)) return;  //若该字段不存在，直接离开





        //                if ( out_para.Trim() == "")
        //                {
        //                    GC.PM_utility2.Dev_messageBoxError("入口参数错误。");
        //                    return;
        //                } 
        //                out_para = out_para.ToUpper();





        //                //新增一行空值。
        //                // outBlock.Tables[0].Rows.Add(" ", " ");


        //                //(EFX.EFCGrid.GetEFCGridBase(efDevGridMaster).Columns["DEP"]).SetPopupGridDataSource(dt, "CODE", new string[] { "text", "value" });
        //                DataTable dt_temp = new DataTable();
        //                dt_temp = (DataTable)outBlock[0];

        //                //人工修正，各列标题。//demo //dataTable.Columns[0].Caption = "名称"
        //                //==================
        //                int i = 0;
        //                for (i = 0; i < out_para2.Length; i++)
        //                {//修正多列的信息。 

        //                    dt_temp.Columns[i].Caption = out_para_title2[i].ToString().Trim();

        //                }


        //                if ((EFX.EFCGrid.GetEFCGridBase(efgridNow).Columns[col_name]).SetPopupGridDataSource(dt_temp, out_para, out_para2))
        //                {//若初始化成功。
        //                    EFX.EFCGrid.GetEFCGridBase(efgridNow).Columns[col_name].SetPopupGridColumn(out_para, false);  //删除多于的一个列。                   
        //                }
        //                else
        //                {
        //                    //GC.PM_utility2.Dev_messageBoxError("SetPopupGridDataSource error。col_name[" + col_name + "]"); 
        //                    //若失败，就不翻译成[代码+中文]模式。

        //                }





        //            }
        //            catch (Exception ex)
        //            {
        //                GC.PM_utility2.Dev_messageBoxError(ex.Message);
        //            }




        //        }





        // /// <summary>
        // /// 加载GRID 中的某个字段下拉列表信息，下拉情况下显示指定任意多列，当前字段只记录第一列CODE.
        // /// 使用场景： OM00B2C画面中，订货用户的录入。
        // /// </summary>
        // /// <param name="efgridNow">指定的GRID </param>
        // /// <param name="col_name">指定的列</param>
        // /// <param name="service_name"></param>
        // /// <param name="para_name">入口参数名称</param>
        // /// <param name="para_value">入口参数内容</param>
        // /// <param name="out_para">显示列</param>
        // /// <param name="out_para2">显示列数组</param>
        // /// <param name="out_para_title2">显示列标题数组</param>
        //        public static void DEV_initCol_LookUpEdit2(EF.EFDevGrid efgridNow, string col_name, string service_name, string para_name, string para_value, string out_para, string[] out_para2, string[] out_para_title2)
        //        {

        //            try
        //            {

        //                //string item_ename = "";
        //                //DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efgridNow.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
        //                //item_ename = col_name.ToUpper();
        //                //if (!gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(item_ename))) return; //若该字段不存在，直接离开。 

        //                //先判断，该列是否存在。
        //                //==============EFX.EFCGrid.GetEFCGridBase(efDevGrid1).Columns.ContainsKey("XXX")
        //                string item_ename = "";
        //                item_ename = col_name.ToUpper();
        //                if (!EFX.EFCGrid.GetEFCGridBase(efgridNow).Columns.ContainsKey(item_ename)) return;  //若该字段不存在，直接离开


        //                EI.EIInfo inBlock = new EI.EIInfo();
        //                EI.EIInfo outBlock = new EI.EIInfo();
        //                EI.EIInfo.eiinfo_sys sys = outBlock.GetSys();






        //                string v_sql = "";
        //                string v_culture = "";//语种。
        //                v_culture = sys.culture;


        //                if (service_name.Trim() == "" || out_para.Trim() == ""  )
        //                {
        //                    GC.PM_utility2.Dev_messageBoxError("入口参数错误。");
        //                    return;
        //                }



        //                //为了使用的通用性，不再使用PM的service ,而采用框架提供的动态sql程序==eped_dyn_sql
        //                if (service_name.Trim() == "pmdmep02_inq")
        //                {
        //                    //service_name = "epep01_inq2";//框架提供的代码查询方法。
        //                    //para_name = "code_class"; //入口参数的名字
        //                    ////para_value = "";
        //                    //out_para = "CODE";//返回参数的名字
        //                    //out_para2 = "CODE_DESC_1_CONTENT";//返回参数2的名字


        //                    //==根据语种，获取对应的代码信息。 
        //                    //采用框架提供的动态SQL 的service 。===eped_dyn_sql
        //                    //==================
        //                    //SELECT t.code,t.code_desc_1_content,t.code || '_' || t.code_desc_1_content as AABB FROM tep0002_res t
        //                    //WHERE   t.culture = @culture and t.code_class = @code_class
        //                    //ORDER BY t.code


        //                    v_sql = " SELECT  t.code,t.code_desc_1_content,t.code || '_' || t.code_desc_1_content as AABB FROM tep0002_res t "
        //                          + " WHERE   t.culture     = '" + v_culture + "' "    //指定的语种
        //                          + " and     t.code_class  = '" + para_value + "' "   //指定代码编号。 
        //                          + " ORDER  BY t.code             ";

        //                    service_name = "eped_dyn_sql";  //采用框架提供的动态SQL 的service 。
        //                    para_name = "v_sql"; //入口参数的名字
        //                    para_value = v_sql;  //查询的SQL语句。
        //                    //out_para = "CODE";   //返回参数的名字
        //                    //out_para2 = "AABB";  //返回参数2的名字= 代码+描述。

        //                }


        //                out_para = out_para.ToUpper();



        //                inBlock.SetColName(1, para_name);
        //                inBlock.SetColVal(1, para_name, para_value);//入口参数信息= 
        //                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,service_name, inBlock);

        //                //新增一行空值。
        //               // outBlock.Tables[0].Rows.Add(" ", " ");


        //                //(EFX.EFCGrid.GetEFCGridBase(efDevGridMaster).Columns["DEP"]).SetPopupGridDataSource(dt, "CODE", new string[] { "text", "value" });
        //                DataTable dt_temp = new DataTable();
        //                dt_temp = (DataTable)outBlock[0];

        //                //人工修正，各列标题。//demo //dataTable.Columns[0].Caption = "名称"
        //                //==================
        //                int i = 0;
        //                for (i = 0; i < out_para2.Length; i++)
        //                {//修正多列的信息。 

        //                    dt_temp.Columns[i].Caption = out_para_title2[i].ToString().Trim();

        //                }


        //                if ((EFX.EFCGrid.GetEFCGridBase(efgridNow).Columns[col_name]).SetPopupGridDataSource(dt_temp, out_para, out_para2))
        //                {//若初始化成功。
        //                    EFX.EFCGrid.GetEFCGridBase(efgridNow).Columns[col_name].SetPopupGridColumn(out_para, false);  //删除多于的一个列。                   
        //                }
        //                else
        //                {
        //                 //GC.PM_utility2.Dev_messageBoxError("SetPopupGridDataSource error。col_name[" + col_name + "]"); 
        //                    //若失败，就不翻译成[代码+中文]模式。

        //                }



        //                //一个主字段，多个副字段的绑定模式。
        //                //=============
        //                /*
        //                   EI.EIInfo u_code = new EI.EIInfo();
        // u_code = Common.Utility.QueryCodeEIInfoDynamic(" select USER_CODE as CODE ,USER_ENNAME as CODE_DESC_1_CONTENT,USER_CNNAME as CODE_DESC_2_CONTENT  from tomomsu01 WHERE RAW_VENDOR_FLAG = '1' and DISABLE_FLAG = '0'", true);
        //              EFX.EFCGrid.GetEFCGridBase(efDevGrid1).Columns["ORDER_CUST_CODE"].SetPopupGridDataSource(u_code.Tables[0], "CODE", new string[] { "CODE", "CODE_DESC_1_CONTENT", "CODE_DESC_2_CONTENT" });
        //                EFX.EFCGrid.GetEFCGridBase(efDevGrid1).Columns["ORDER_CUST_CODE"].SetPopupGridColumn("CODE", false); 
        //                 */


        //            }
        //            catch (Exception ex)
        //            {
        //                GC.PM_utility2.Dev_messageBoxError(ex.Message);
        //            }




        //        }



        //        /// <summary>
        //        /// GRID中的[Column]设置成下拉列表模式==EFX模式[SetPopupGridDataSource]
        //        /// </summary>
        //        /// <param name="efgridNow">GRID控件</param>
        //        /// <param name="col_name">GRID 中需要转换的列名称</param>
        //        /// <param name="service_name">需要显示信息的业务service</param>
        //        /// <param name="para_name">入口参数名称</param>
        //        /// <param name="para_value">入口参数值</param>
        //        /// <param name="out_para">出口参数值1</param>
        //        /// <param name="out_para2">出口参数值2</param>
        //        public static void DEV_initCol_LookUpEdit(EF.EFDevGrid efgridNow, string col_name, string service_name, string para_name, string para_value, string out_para, string out_para2)
        //        {

        //            try
        //            {

        //                //若GRID 的数据源是空，则直接离开。 
        //                if (efgridNow.DataSource == null) return; 

        //                //先判断，该列是否存在。
        //                //==============EFX.EFCGrid.GetEFCGridBase(efDevGrid1).Columns.ContainsKey("XXX")
        //                string item_ename = "";
        //                item_ename = col_name.ToUpper(); 
        //                if (!EFX.EFCGrid.GetEFCGridBase(efgridNow).Columns.ContainsKey(item_ename)) return;  //若该字段不存在，直接离开


        //                EI.EIInfo inBlock = new EI.EIInfo();
        //                EI.EIInfo outBlock = new EI.EIInfo();
        //                EI.EIInfo.eiinfo_sys sys = outBlock.GetSys();






        //                string v_sql = "";
        //                string v_culture = "";//语种。
        //                v_culture = sys.culture;


        //                if (service_name.Trim() == "" || out_para.Trim() == "" || out_para2.Trim() == "")
        //                {
        //                    GC.PM_utility2.Dev_messageBoxError("入口参数错误。");
        //                    return;
        //                }



        //                //为了使用的通用性，不再使用PM的service ,而采用框架提供的动态sql程序==eped_dyn_sql
        //                if(service_name.Trim() == "pmdmep02_inq")
        //                {
        //                    //service_name = "epep01_inq2";//框架提供的代码查询方法。
        //                    //para_name = "code_class"; //入口参数的名字
        //                    ////para_value = "";
        //                    //out_para = "CODE";//返回参数的名字
        //                    //out_para2 = "CODE_DESC_1_CONTENT";//返回参数2的名字


        //                    //==根据语种，获取对应的代码信息。 
        //                    //采用框架提供的动态SQL 的service 。===eped_dyn_sql
        //                    //==================
        //                    //SELECT t.code,t.code_desc_1_content,t.code || '_' || t.code_desc_1_content as AABB FROM tep0002_res t
        //                    //WHERE   t.culture = @culture and t.code_class = @code_class
        //                    //ORDER BY t.code


        //                    v_sql = " SELECT  t.code,t.code_desc_1_content,t.code || '_' || t.code_desc_1_content as AABB FROM tep0002_res t "
        //                          + " WHERE   t.culture     = '" + v_culture + "' "    //指定的语种
        //                          + " and     t.code_class  = '" + para_value + "' "   //指定代码编号。 
        //                          + " ORDER  BY t.code             ";

        //                    service_name = "eped_dyn_sql";  //采用框架提供的动态SQL 的service 。
        //                    para_name = "v_sql"; //入口参数的名字
        //                    para_value = v_sql;  //查询的SQL语句。
        //                    //out_para = "CODE";   //返回参数的名字
        //                    //out_para2 = "AABB";  //返回参数2的名字= 代码+描述。

        //                }


        //                out_para = out_para.ToUpper();
        //                out_para2 = out_para2.ToUpper();


        //                inBlock.SetColName(1, para_name);
        //                inBlock.SetColVal(1, para_name, para_value);//入口参数信息= 
        //                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,service_name, inBlock);

        //                //新增一行空值。
        //                outBlock.Tables[0].Rows.Add(" ", " ");


        //                //(EFX.EFCGrid.GetEFCGridBase(efDevGridMaster).Columns["DEP"]).SetPopupGridDataSource(dt, "CODE", new string[] { "text", "value" });
        //                DataTable dt_temp = new DataTable();
        //                dt_temp = (DataTable)outBlock[0];

        //                if (!(EFX.EFCGrid.GetEFCGridBase(efgridNow).Columns[col_name]).SetPopupGridDataSource(dt_temp, out_para, new string[] { out_para2 }))
        //                {//若初始化失败。 
        //                    //GC.PM_utility2.Dev_messageBoxError("SetPopupGridDataSource error。col_name[" + col_name + "]");

        //                    //若失败，就不翻译成[代码+中文]模式。
        //                }



        //                //一个主字段，多个副字段的绑定模式。
        //                //=============
        //                /*
        //                   EI.EIInfo u_code = new EI.EIInfo();
        // u_code = Common.Utility.QueryCodeEIInfoDynamic(" select USER_CODE as CODE ,USER_ENNAME as CODE_DESC_1_CONTENT,USER_CNNAME as CODE_DESC_2_CONTENT  from tomomsu01 WHERE RAW_VENDOR_FLAG = '1' and DISABLE_FLAG = '0'", true);
        //              EFX.EFCGrid.GetEFCGridBase(efDevGrid1).Columns["ORDER_CUST_CODE"].SetPopupGridDataSource(u_code.Tables[0], "CODE", new string[] { "CODE", "CODE_DESC_1_CONTENT", "CODE_DESC_2_CONTENT" });
        //                EFX.EFCGrid.GetEFCGridBase(efDevGrid1).Columns["ORDER_CUST_CODE"].SetPopupGridColumn("CODE", false); 
        //                 */


        //            }
        //            catch (Exception ex)
        //            {
        //                GC.PM_utility2.Dev_messageBoxError(ex.Message);
        //            }






        //        }




        //        /// <summary>
        //        /// GRID中的[Column]设置成下拉列表模式==非EFX模式[SetPopupGridDataSource]
        //        /// 非EPED54配置的GIRD 信息的控制。
        //        /// </summary>
        //        /// <param name="efgridNow">GRID控件</param>
        //        /// <param name="col_name">GRID 中需要转换的列名称</param>
        //        /// <param name="service_name">需要显示信息的业务service</param>
        //        /// <param name="para_name">入口参数名称</param>
        //        /// <param name="para_value">入口参数值</param>
        //        /// <param name="out_para">出口参数值1</param>
        //        /// <param name="out_para2">出口参数值2</param>
        //        public static void DEV_initCol_LookUpEdit_notEped54(EF.EFDevGrid efgridNow, string col_name, string service_name, string para_name, string para_value, string out_para, string out_para2)
        //        {

        //            try
        //            {
        //                string item_ename = "";
        //                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efgridNow.FocusedView as DevExpress.XtraGrid.Views.Grid.GridView;
        //                //若是GRID 中不存在的字段，则继续下一个字段循环
        //                item_ename = col_name.ToUpper();
        //                if (!gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(item_ename))) return; //若该字段不存在，直接离开。 





        //                EI.EIInfo inBlock = new EI.EIInfo();
        //                EI.EIInfo outBlock = new EI.EIInfo();
        //                EI.EIInfo.eiinfo_sys sys = outBlock.GetSys();




        //                //if (service_name.Trim() == "" || out_para.Trim() == "" || out_para2.Trim() == "")
        //                //{
        //                //    service_name = "epep01_inq2";
        //                //    para_name = "code_class"; //入口参数的名字
        //                //    //para_value = "";
        //                //    out_para = "CODE";//返回参数的名字
        //                //    out_para2 = "CODE_DESC_1_CONTENT";//返回参数2的名字
        //                //}



        //                string v_sql = "";
        //                string v_culture = "";//语种。
        //                v_culture = sys.culture;


        //                if (service_name.Trim() == "" || out_para.Trim() == "" || out_para2.Trim() == "")
        //                {
        //                    GC.PM_utility2.Dev_messageBoxError("入口参数错误。");
        //                    return;
        //                }

        //                //为了使用的通用性，不再使用PM的service ,而采用框架提供的动态sql程序==eped_dyn_sql
        //                if(service_name.Trim() == "pmdmep02_inq")
        //                {
        //                    //service_name = "epep01_inq2";//框架提供的代码查询方法。
        //                    //para_name = "code_class"; //入口参数的名字
        //                    ////para_value = "";
        //                    //out_para = "CODE";//返回参数的名字
        //                    //out_para2 = "CODE_DESC_1_CONTENT";//返回参数2的名字


        //                    //==根据语种，获取对应的代码信息。 
        //                    //采用框架提供的动态SQL 的service 。===eped_dyn_sql
        //                    //==================
        //                    //SELECT t.code,t.code_desc_1_content,t.code || '_' || t.code_desc_1_content as AABB FROM tep0002_res t
        //                    //WHERE   t.culture = @culture and t.code_class = @code_class
        //                    //ORDER BY t.code


        //                    v_sql = " SELECT  t.code,t.code_desc_1_content,t.code || '_' || t.code_desc_1_content as AABB FROM tep0002_res t "
        //                          + " WHERE   t.culture     = '" + v_culture + "' "    //指定的语种
        //                          + " and     t.code_class  = '" + para_value + "' "   //指定代码编号。 
        //                          + " ORDER  BY t.code             ";

        //                    service_name = "eped_dyn_sql";  //采用框架提供的动态SQL 的service 。
        //                    para_name = "v_sql"; //入口参数的名字
        //                    para_value = v_sql;  //查询的SQL语句。
        //                    //out_para = "CODE";   //返回参数的名字
        //                    //out_para2 = "AABB";  //返回参数2的名字= 代码+描述。

        //                }


        //                out_para = out_para.ToUpper();
        //                out_para2 = out_para2.ToUpper();


        //                inBlock.SetColName(1, para_name);
        //                inBlock.SetColVal(1, para_name, para_value);//入口参数信息= 
        //                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,service_name, inBlock);

        //                //新增一行空值。
        //                outBlock.Tables[0].Rows.Add(" ", " ");  

        //                //定义一个lookup字段类型。
        //                DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lookupedit = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
        //                gridView_now.Columns[item_ename].ColumnEdit = lookupedit; //将指定的列设置成Lookupedit控件模式。



        //                //下拉列表的列宽度自动调节。
        //                lookupedit.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;


        //                lookupedit.Columns.Clear();/* 清空 */
        //                lookupedit.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(out_para, "代码"));
        //                if (out_para != out_para2) lookupedit.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(out_para2, "描述"));

        //                lookupedit.DataSource = outBlock.Tables[0];/* 值集表 信息 = tep0002*/

        //                //lookupedit.Name = "LookUpEdit_" + service_name;
        //                lookupedit.ValueMember = out_para;
        //                if (out_para != out_para2) lookupedit.DisplayMember = out_para2;


        //                if (outBlock.Tables[0].Rows.Count <= 6)
        //                {
        //                    lookupedit.Properties.DropDownRows = outBlock.Tables[0].Rows.Count; //小于等于6记录的，下拉列表的长度=记录数
        //                }
        //                else
        //                {
        //                    lookupedit.Properties.DropDownRows = 7; //默认是七
        //                }

        //                //使得当前列，不可编辑。
        //                //===============  
        //                //efgridNow 
        //                //EFX.EFCGrid.GetEFCGridBase(efgridNow).Columns[item_ename].EnableEdit = false;


        //            }
        //            catch (Exception ex)
        //            {
        //                GC.PM_utility2.Dev_messageBoxError(ex.Message);
        //            }


        //        }


















        //        /// <summary>
        //        /// 可设定列标题的GRID 中指定列的下拉列表模式处理。
        //        /// </summary>
        //        /// <param name="efgridNow">GRID</param>
        //        /// <param name="col_name">列名称</param>
        //        /// <param name="service_name">调用的SERVICE</param>
        //        /// <param name="para_name">入口参数名称</param>
        //        /// <param name="para_value">入口参数值</param>
        //        /// <param name="out_para">返回列1</param>
        //        /// <param name="out_para2">返回列2</param>
        //        /// <param name="out_title">返回列标题1</param>
        //        /// <param name="out_title2">返回列标题2</param>
        //        public static void DEV_initCol_LookUpEdit_title(EF.EFDevGrid efgridNow, string col_name, string service_name, string para_name, string para_value, string out_para, string out_para2, string out_title, string out_title2)
        //        {

        //            try
        //            {
        //                //以下判断的方法作废。
        //                //string item_ename = "";
        //                //DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efgridNow.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
        //                //item_ename = col_name.ToUpper();
        //                //if (!gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(item_ename))) return; //若该字段不存在，直接离开。 


        //                //先判断，该列是否存在。
        //                //==============EFX.EFCGrid.GetEFCGridBase(efDevGrid1).Columns.ContainsKey("XXX")
        //                string item_ename = "";
        //                item_ename = col_name.ToUpper();
        //                if (!EFX.EFCGrid.GetEFCGridBase(efgridNow).Columns.ContainsKey(item_ename)) return;  //若该字段不存在，直接离开


        //                EI.EIInfo inBlock = new EI.EIInfo();
        //                EI.EIInfo outBlock = new EI.EIInfo();
        //                EI.EIInfo.eiinfo_sys sys = outBlock.GetSys();



        //                string v_sql = "";
        //                string v_culture = "";//语种。
        //                v_culture = sys.culture;


        //                if (service_name.Trim() == "" || out_para.Trim() == "" || out_para2.Trim() == "")
        //                {
        //                    GC.PM_utility2.Dev_messageBoxError("入口参数错误。");
        //                    return;
        //                }



        //                //为了使用的通用性，不再使用PM的service ,而采用框架提供的动态sql程序==eped_dyn_sql
        //                if (service_name.Trim() == "pmdmep02_inq")
        //                {

        //                    //==根据语种，获取对应的代码信息。 
        //                    //采用框架提供的动态SQL 的service 。===eped_dyn_sql
        //                    //==================
        //                    //SELECT t.code,t.code_desc_1_content,t.code || '_' || t.code_desc_1_content as AABB FROM tep0002_res t
        //                    //WHERE   t.culture = @culture and t.code_class = @code_class
        //                    //ORDER BY t.code


        //                    v_sql = " SELECT  t.code,t.code_desc_1_content,t.code || '_' || t.code_desc_1_content as AABB FROM tep0002_res t "
        //                          + " WHERE   t.culture     = '" + v_culture + "' "    //指定的语种
        //                          + " and     t.code_class  = '" + para_value + "' "   //指定代码编号。 
        //                          + " ORDER  BY t.code             ";

        //                    service_name = "eped_dyn_sql";  //采用框架提供的动态SQL 的service 。
        //                    para_name = "v_sql"; //入口参数的名字
        //                    para_value = v_sql;  //查询的SQL语句。
        //                    //out_para = "CODE";   //返回参数的名字
        //                    //out_para2 = "AABB";  //返回参数2的名字= 代码+描述。

        //                }


        //                out_para = out_para.ToUpper();
        //                out_para2 = out_para2.ToUpper();


        //                inBlock.SetColName(1, para_name);
        //                inBlock.SetColVal(1, para_name, para_value);//入口参数信息= 
        //                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,service_name, inBlock);

        //                //新增一行空值。
        //                outBlock.Tables[0].Rows.Add(" ", " "); 

        //                DataTable dt_temp = new DataTable();
        //                dt_temp = (DataTable)outBlock[0];

        //                //设定列标题=  dt_temp.Columns[i].Caption = out_para_title2[i].ToString().Trim();
        //                //本方法是固定的2列，SO ，写死即可。
        //                dt_temp.Columns[0].Caption = out_title;
        //                dt_temp.Columns[1].Caption = out_title2;


        //                if (!(EFX.EFCGrid.GetEFCGridBase(efgridNow).Columns[col_name]).SetPopupGridDataSource(dt_temp, out_para, new string[] { out_para2 }))
        //                {//若初始化失败。 
        //                    //GC.PM_utility2.Dev_messageBoxError("SetPopupGridDataSource error。col_name[" + col_name + "]"); 
        //                    //若失败，就不翻译成[代码+中文]模式。
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                GC.PM_utility2.Dev_messageBoxError(ex.Message);
        //            } 
        //        }







        //        /// <summary>
        //        /// 设置LayoutGroup内部的所有控件的只读属性。
        //        /// </summary>
        //        /// <param name="layoutGroup_now"></param>
        //        public static void DEV_Init_LayoutGroup_ReadOnly(DevExpress.XtraLayout.LayoutControlGroup layoutGroup_now)
        //        {

        //            int i = 0;
        //            for (i = 0; i < layoutGroup_now.Items.Count; i++)
        //            {
        //                Control control = (layoutGroup_now.Items[i] as DevExpress.XtraLayout.LayoutControlItem).Control;

        //                if (control != null)
        //                {

        //                    if (control is EF.EFDevTextEdit)
        //                    {//文本控件
        //                        (control as EF.EFDevTextEdit).Properties.ReadOnly = true;
        //                    }

        //                    if (control is EF.EFDevSpinEdit)
        //                    {//数字控件
        //                        (control as EF.EFDevSpinEdit).Properties.ReadOnly = true;
        //                    }

        //                    if (control is EF.EFDevLookUpEdit)
        //                    {//下拉列表控件。
        //                        (control as EF.EFDevLookUpEdit).Properties.ReadOnly = true;
        //                    }

        //                    if (control is EF.EFDevButtonEdit)
        //                    {//按钮文本框控件。
        //                        (control as EF.EFDevButtonEdit).Properties.ReadOnly = true;
        //                    }


        //                }
        //            }
        //        }

        //        /// <summary>
        //        /// 设置Panel控件内部的所有控件的只读属性= false。
        //        /// </summary>
        //        /// <param name="panel_now"></param>
        //        public static void DEV_Init_Panel_ReadOnly_False(EF.EFPanel panel_now)
        //        {
        //            int i = 0;
        //            int v_col_exist = -1; //字段名称存在的位置。
        //            string v_col_name = "order_no"; //字段的名称。

        //            for (i = 0; i < panel_now.Controls.Count; i++)
        //            {
        //                //Control control = (layoutGroup_now.Items[i] as DevExpress.XtraLayout.LayoutControlItem).Control;

        //                Control control = panel_now.Controls[i];



        //                if (control != null)
        //                {

        //                    if (control is EF.EFDevTextEdit)
        //                    {//文本控件
        //                        (control as EF.EFDevTextEdit).Properties.ReadOnly = false;
        //                    }

        //                    if (control is EF.EFDevSpinEdit)
        //                    {//数字控件
        //                        (control as EF.EFDevSpinEdit).Properties.ReadOnly = false;
        //                    }

        //                    if (control is EF.EFDevLookUpEdit)
        //                    {//下拉列表控件。
        //                        (control as EF.EFDevLookUpEdit).Properties.ReadOnly = false;
        //                    }

        //                    if (control is EF.EFDevButtonEdit)
        //                    {//按钮文本框控件。
        //                        //(control as EF.EFDevButtonEdit).Properties.ReadOnly = false;
        //                        (control as EF.EFDevButtonEdit).Enabled = true;
        //                    }

        //                    if (control is EF.EFDevDateEdit)
        //                    {//日期控件。
        //                        //(control as EF.EFDevDateEdit).Properties.ReadOnly = false;
        //                        (control as EF.EFDevDateEdit).Enabled = true;
        //                    }




        //                }


        //            }

        //        }


        //        /// <summary>
        //        /// 根据【功能号】，设置Panel控件内部的控件的只读属性= false。
        //        /// </summary>
        //        /// <param name="panel_now"></param>
        //        public static void DEV_Init_Panel_ReadOnly(EF.EFPanel panel_now, string v_function_id)
        //        {

        //            int i = 0;
        //            int j = 0;
        //            int rows = 0;
        //            int cols = 0;
        //            string item_ename = "";

        //            //初始化ComboBox中的数据
        //            EI.EIInfo inBlock = new EI.EIInfo();
        //            EI.EIInfo outBlock;








        //            //先初始化当前PANEL 中所有控件的只读属性= TRUE.
        //            for (i = 0; i < panel_now.Controls.Count; i++)
        //            {
        //                //Control control = (layoutGroup_now.Items[i] as DevExpress.XtraLayout.LayoutControlItem).Control;

        //                Control control = panel_now.Controls[i];

        //                if (control != null)
        //                {

        //                    if (control is EF.EFDevTextEdit)
        //                    {//文本控件
        //                        (control as EF.EFDevTextEdit).Properties.ReadOnly = true;
        //                    }

        //                    if (control is EF.EFDevSpinEdit)
        //                    {//数字控件
        //                        (control as EF.EFDevSpinEdit).Properties.ReadOnly = true;
        //                    }

        //                    if (control is EF.EFDevLookUpEdit)
        //                    {//下拉列表控件。
        //                        (control as EF.EFDevLookUpEdit).Properties.ReadOnly = true;
        //                    }

        //                    if (control is EF.EFDevButtonEdit)
        //                    {//按钮文本框控件。
        //                        //(control as EF.EFDevButtonEdit).Properties.ReadOnly = true;
        //                        (control as EF.EFDevButtonEdit).Enabled = false;
        //                    }

        //                    if (control is EF.EFDevDateEdit)
        //                    {//日期控件。
        //                        //(control as EF.EFDevDateEdit).Properties.ReadOnly = true;
        //                        (control as EF.EFDevDateEdit).Enabled = false;
        //                    }




        //                }


        //            }

        //            string v_item_ename = "";
        //            string v_control_name = "";
        //            int v_col_exist = -1;




        //            i = 1;
        //            //inBlock.SetColName(1, i++, "func_id");
        //            //inBlock.SetColVal(1, 1, "func_id", v_function_id); //功能号。
        //            //outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"pmog_edit", inBlock);


        //            //采用框架提供的动态SQL 的service 。
        //            //==================
        //            string v_sql = "";// 工序代码表 = tsi0001, 
        //            v_sql = " SELECT item_ename FROM ted54 "
        //                  + " WHERE  UPPER(func_id) = UPPER('" + v_function_id + "') " //根据指定的功能号。
        //                  + " AND    form_edit_flag = '1'              "             //前台可编辑标记= 1,说明允许编辑
        //                  + " ORDER  BY class_code ,seq_no             ";

        //            inBlock.SetColName(1, i++, "v_sql");
        //            inBlock.SetColVal(1, 1, "v_sql", v_sql);// 
        //            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"eped_dyn_sql", inBlock); 

        //            //根据功能号中对应的可编辑列，设置对应的只读属性。
        //            for (j = 0; j < outBlock.blk_info[0].row; j++)
        //            {//使得多列为可写状态
        //                v_item_ename = outBlock.Tables[0].Rows[j]["item_ename"].ToString().Trim().ToUpper();//可编辑列的名称。


        //                for (i = 0; i < panel_now.Controls.Count; i++)
        //                {
        //                    //Control control = (layoutGroup_now.Items[i] as DevExpress.XtraLayout.LayoutControlItem).Control;

        //                    Control control = panel_now.Controls[i];





        //                    //使得对应控件的只读属性= FALSE.
        //                    if (control != null)
        //                    {

        //                        if (control is EF.EFLabel)
        //                        {//若是LABEL 控件，直接离开。
        //                            continue;
        //                        }

        //                        if (control is System.Windows.Forms.Label)
        //                        {//若是LABEL 控件，直接离开。
        //                            continue;

        //                        }

        //                        v_control_name = control.Name.ToUpper(); //控件的名称。
        //                        v_col_exist = -1;
        //                        v_col_exist = v_control_name.IndexOf(v_item_ename);
        //                        if (v_col_exist < 0)
        //                        {//该字段若不存在，则检查下一个控件。
        //                            continue;
        //                        }

        //                        if (control is EF.EFDevTextEdit)
        //                        {//文本控件
        //                            (control as EF.EFDevTextEdit).Properties.ReadOnly = false;
        //                        }

        //                        if (control is EF.EFDevSpinEdit)
        //                        {//数字控件
        //                            (control as EF.EFDevSpinEdit).Properties.ReadOnly = false;
        //                        }

        //                        if (control is EF.EFDevLookUpEdit)
        //                        {//下拉列表控件。
        //                            (control as EF.EFDevLookUpEdit).Properties.ReadOnly = false;
        //                        }

        //                        if (control is EF.EFDevButtonEdit)
        //                        {//按钮文本框控件。
        //                            //(control as EF.EFDevButtonEdit).Properties.ReadOnly = false;
        //                            (control as EF.EFDevButtonEdit).Enabled = true;
        //                        }

        //                        if (control is EF.EFDevDateEdit)
        //                        {//日期控件。
        //                            //(control as EF.EFDevDateEdit).Properties.ReadOnly = false;
        //                            (control as EF.EFDevDateEdit).Enabled = true;
        //                        }

        //                        //若找到字段，属性修改完成后，直接break;
        //                        break;


        //                    }


        //                }






        //            }




        //        }


        //        /// <summary>
        //        /// 设置Panel控件内部的所有控件的只读属性= true。
        //        /// </summary>
        //        /// <param name="panel_now"></param>
        //        public static void DEV_Init_Panel_ReadOnly(EF.EFPanel panel_now)
        //        {
        //            int i = 0;
        //            for (i = 0; i < panel_now.Controls.Count; i++)
        //            {
        //                //Control control = (layoutGroup_now.Items[i] as DevExpress.XtraLayout.LayoutControlItem).Control;

        //                Control control = panel_now.Controls[i];

        //                if (control != null)
        //                {

        //                    if (control is EF.EFDevTextEdit)
        //                    {//文本控件
        //                        (control as EF.EFDevTextEdit).Properties.ReadOnly = true;
        //                    }

        //                    if (control is EF.EFDevSpinEdit)
        //                    {//数字控件
        //                        (control as EF.EFDevSpinEdit).Properties.ReadOnly = true;
        //                    }

        //                    if (control is EF.EFDevLookUpEdit)
        //                    {//下拉列表控件。
        //                        (control as EF.EFDevLookUpEdit).Properties.ReadOnly = true;
        //                    }

        //                    if (control is EF.EFDevButtonEdit)
        //                    {//按钮文本框控件。
        //                        //(control as EF.EFDevButtonEdit).Properties.ReadOnly = true;
        //                        (control as EF.EFDevButtonEdit).Enabled = false;
        //                    }

        //                    if (control is EF.EFDevDateEdit)
        //                    {//日期控件。
        //                        //(control as EF.EFDevDateEdit).Properties.ReadOnly = true;
        //                        (control as EF.EFDevDateEdit).Enabled = false;
        //                    }




        //                }


        //            }

        //        }

        //        //设置LayoutGroup的伸缩按钮。
        //        /// <summary>
        //        /// 设置LayoutGroup的伸缩按钮。
        //        /// 设置LayoutGroup内部的控件边距等。
        //        /// </summary>
        //        /// <param name="layoutGroup_now">需要设置的layoutGroup</param>
        //        public static void DEV_Init_LayoutGroup(DevExpress.XtraLayout.LayoutControlGroup layoutGroup_now)
        //        {


        //            int i = 0;


        //            layoutGroup_now.ExpandButtonLocation = DevExpress.Utils.GroupElementLocation.AfterText;
        //            layoutGroup_now.ExpandButtonVisible = true;

        //            // this.layoutControlGroup1.Spacing = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);

        //            layoutGroup_now.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);

        //            layoutGroup_now.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far; //LABLE 列信息右对齐。


        //            //group中的控件与GROUP的边距设定为0.
        //            //layoutGroup_now.Padding.All = 3;
        //            //layoutGroup_now.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);


        //            //若当前GROUP 中有GRID OR 有 TAB ，那么，PADDING = 0
        //            //若当前GROUP 中没GRID ，那么，PADDING = 9
        //            layoutGroup_now.Padding = new DevExpress.XtraLayout.Utils.Padding(9, 9, 9, 9);

        //            for (i = 0; i < layoutGroup_now.Items.Count; i++)
        //            {
        //                Control control = (layoutGroup_now.Items[i] as DevExpress.XtraLayout.LayoutControlItem).Control;
        //                if (control != null)
        //                {
        //                    //若是GRID 控件，则 PADDING = 0
        //                    //===========
        //                    if (control is EF.EFDevGrid || control is EF.EFSkinTabControl)
        //                    {//PADDING = 0
        //                        layoutGroup_now.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
        //                        break; //离开本循环。
        //                    }

        //                }


        //            }





        //            for (i = 0; i < layoutGroup_now.Items.Count; i++)
        //            {
        //                Control control = (layoutGroup_now.Items[i] as DevExpress.XtraLayout.LayoutControlItem).Control;

        //                DevExpress.XtraLayout.LayoutControlItem control_item = layoutGroup_now.Items[i] as DevExpress.XtraLayout.LayoutControlItem  ;


        //                if(control_item != null)
        //                {

        //                    /*
        //                     ////layoutControlItem3==查询条件中,合同信息列固定长度.


        ////1设置LayoutControlItem的宽度: 
        ////a.    把FillcontroltoClient属性设为False.  
        ////b.    把SizeConstraintsType属性设为Custom 
        ////c.     设置ControlMinsize和ControlMaxsize的值. 
        ////d.   设置Minsize的值, 
        ////e.    设置Size的值

        //                this.layoutControlItem3.FillControlToClientArea = false;
        //                this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
        //                this.layoutControlItem3.ControlMinSize = this.layoutControlItem3.Size;
        //                this.layoutControlItem3.ControlMaxSize = this.layoutControlItem3.Size;
        //                this.layoutControlItem3.MinSize        = this.layoutControlItem3.Size;

        //                     */

        //                    //control_item.Size = new System.Drawing.Size(175, 75);

        //                    //control_item.FillControlToClientArea = false;
        //                    //control_item.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
        //                    //control_item.ControlMinSize = control_item.Size;
        //                    //control_item.ControlMaxSize = control_item.Size;
        //                    //control_item.MinSize = control_item.Size;




        //                    // 控制控件的和text的边距信息。
        //                    /*
        //                       this.layoutControlItem24.AppearanceItemCaption.Options.UseTextOptions = true;
        //            this.layoutControlItem24.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
        //            this.layoutControlItem24.Control = this.pROD_DIFEFDevLookUpEdit;
        //            resources.ApplyResources(this.layoutControlItem24, "layoutControlItem24");
        //            this.layoutControlItem24.Location = new System.Drawing.Point(0, 0);
        //            this.layoutControlItem24.Name = "layoutControlItem24";
        //            this.layoutControlItem24.Size = new System.Drawing.Size(205, 25);
        //            this.layoutControlItem24.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
        //            this.layoutControlItem24.TextSize = new System.Drawing.Size(72, 14);
        //            this.layoutControlItem24.TextToControlDistance = 5;
        //                     */


        //                    control_item.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;//保证右对齐。
        //                    control_item.AppearanceItemCaption.Options.UseTextOptions = true;
        //                    //control_item.Size = new System.Drawing.Size(170, 25); //LayoutControlItem的总长度+宽度
        //                    //control_item.Size.Width =  170; //设置宽度。

        //                    //系统定义的格式。
        //                    control_item.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.UseParentOptions;

        //                    if (control_item.Text.Trim() == "--")
        //                    {//若是个范围值的控件，则格式是AutoSize
        //                        control_item.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;

        //                    }
        //                    else
        //                    {//===非--的时候。 
        //                        if (control_item.Text.Length <= 5)
        //                        {//若字段内容《=4位的，则长度设定。

        //                            //根据用户的自定义格式。
        //                            control_item.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
        //                            control_item.TextSize = new System.Drawing.Size(65, 14); //标题信息的长度。 
        //                            control_item.TextToControlDistance = 5; //标题和控件的距离。

        //                        }
        //                        else
        //                        {//其他较长的。
        //                            //根据用户的自定义格式。
        //                            control_item.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
        //                            control_item.TextSize = new System.Drawing.Size(80, 14); //标题信息的长度。 
        //                            control_item.TextToControlDistance = 5; //标题和控件的距离。

        //                        }
        //                    }










        //                }


        //                if (control != null)
        //                {
        //                    //若是日期控件，则修正其日期格式。
        //                    //===========
        //                    if (control is EF.EFDevDateEdit)
        //                    {//日期控件,格式控制。

        //                        (control as EF.EFDevDateEdit).Properties.Mask.EditMask = "yyyy-MM-dd ";
        //                        (control as EF.EFDevDateEdit).Properties.Mask.UseMaskAsDisplayFormat = true;
        //                    }









        //                    //if (control is EF.EFDevTextEdit)
        //                    //{//文本控件
        //                    //    (control as EF.EFDevTextEdit).Properties.ReadOnly = true;
        //                    //}

        //                    //if (control is EF.EFDevSpinEdit)
        //                    //{//数字控件
        //                    //    (control as EF.EFDevSpinEdit).Properties.ReadOnly = true;
        //                    //}

        //                    //if (control is EF.EFDevLookUpEdit)
        //                    //{//下拉列表控件。
        //                    //    (control as EF.EFDevLookUpEdit).Properties.ReadOnly = true;
        //                    //}

        //                    //if (control is EF.EFDevButtonEdit)
        //                    //{//按钮文本框控件。
        //                    //    (control as EF.EFDevButtonEdit).Properties.ReadOnly = true;
        //                    //}


        //                }
        //            }
        //        }

        //        //设置LayoutGroup的内容是【纯ED54】配置的【单记录模式】的信息。
        //        /// <summary>
        //        /// 设置LayoutGroup的伸缩按钮。
        //        /// 设置LayoutGroup内部的控件边距等。
        //        /// </summary>
        //        /// <param name="layoutGroup_now">需要设置的layoutGroup</param>
        //        public static void DEV_Init_LayoutGroup_where(DevExpress.XtraLayout.LayoutControlGroup layoutGroup_now)
        //        {


        //            layoutGroup_now.ExpandButtonLocation = DevExpress.Utils.GroupElementLocation.AfterText;
        //            layoutGroup_now.ExpandButtonVisible = true; 

        //            layoutGroup_now.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);

        //            layoutGroup_now.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far; //LABLE 列信息右对齐。


        //            //group中的控件与GROUP的边距设定为0.
        //            //layoutGroup_now.Padding.All = 3;
        //            //layoutGroup_now.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);



        //            //若当前GROUP 中是GRID ，那么，PADDING = 9
        //            //因为当前方法就是专门给ED54配置的【单记录】模式使用的。
        //            //此处只控制PADDING，即可，对齐的功能已经在ED54中控制。
        //            layoutGroup_now.Padding = new DevExpress.XtraLayout.Utils.Padding(9, 9, 9, 9); 



        //        }

        //        //设置LayoutGroup的内容是【纯ED54】配置的【单记录模式】的信息,旁边就其他LayoutGroup
        //        /// <summary>
        //        /// 设置LayoutGroup的伸缩按钮。
        //        /// 设置LayoutGroup内部的控件边距等。
        //        /// </summary>
        //        /// <param name="layoutGroup_now">需要设置的layoutGroup</param>
        //        public static void DEV_Init_LayoutGroup_where_with_others(DevExpress.XtraLayout.LayoutControlGroup layoutGroup_now)
        //        {


        //            layoutGroup_now.ExpandButtonLocation = DevExpress.Utils.GroupElementLocation.AfterText;
        //            layoutGroup_now.ExpandButtonVisible = true;

        //            layoutGroup_now.Spacing = new DevExpress.XtraLayout.Utils.Padding(2, 2, 2, 2);

        //            layoutGroup_now.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far; //LABLE 列信息右对齐。


        //            //group中的控件与GROUP的边距设定为0.
        //            //layoutGroup_now.Padding.All = 3;
        //            //layoutGroup_now.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);



        //            //若当前GROUP 中是GRID ，那么，PADDING = 9
        //            //因为当前方法就是专门给ED54配置的【单记录】模式使用的。
        //            //此处只控制PADDING，即可，对齐的功能已经在ED54中控制。
        //            layoutGroup_now.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 6, 6);



        //        }






        //        //GRID 初始化设置，提供翻页，跳转，等小按钮功能。
        //        /// <summary>
        //        /// GRID 初始化设置：显示复选框列标题，列AutoSize,右键设置，行拖拉设置，提供翻页(first,last,next,prev,跳转页)，导出等GRID自带小按钮功能。
        //        /// </summary>
        //        /// <param name="efGrid_now">需要设置的GRID</param>
        //        public static void DEV_Init_grid(EF.EFDevGrid efGrid_now)
        //        {
        //            DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;




        //            //使得整个GRID 可编辑
        //            //gridView_now.OptionsBehavior.Editable = true;

        //            ////
        //            ////初始化GRID,中，设置GRID 中选择列可操作。
        //            //string[] col_name2 = new string[] { "xx" };//
        //            ////设置某列N列信息可写.
        //            //source_dm.DEV_SetColEdit_multi(this,efGrid_now, col_name2, "1");



        //            efGrid_now.ShowContextMenu = true; //允许右键
        //            efGrid_now.AllowDragRow = false;    //不允许行拖拉
        //            efGrid_now.ShowSelectionColumn = true; //采用新方式。//显示行选择列。
        //            //efGrid_now.ShowSelectedColumn = true; //显示选择列





        //            efGrid_now.IsUseCustomPageBar = true; //显示GRID自带的工具栏
        //            //翻页功能的小按钮显示。
        //            efGrid_now.PageSize = 500; //初始化每页的记录数[500]行。
        //            efGrid_now.FirstPageButtonEnable = true;
        //            efGrid_now.LastPageButtonEnable = true;
        //            efGrid_now.NextPageButtonEnable = true;
        //            efGrid_now.PrePageButtonEnable = true;
        //            efGrid_now.ShowRecordCountMessage = true;
        //            efGrid_now.ShowPageToButton = true; //跳转页 



        //            //隐藏不需要的小按钮。
        //            efGrid_now.ShowFilterButton = false; //过滤行按钮
        //            efGrid_now.ShowGroupButton = false;  //列分组按钮
        //            efGrid_now.ShowRefreshButton = false;//刷新按钮
        //            efGrid_now.ShowSaveLayoutButton = false;//格式保存按钮。

        //            //efGrid_now.IsUseCustomPageBar = true;
        //            //efGrid_now.ShowAddRowButton = true;


        //            //列宽自动调整。
        //            gridView_now.BestFitColumns();

        //            //设置GRID中的所有列都不能被用户隐藏
        //            //=======================
        //            int cols = 0;
        //            for (cols = 0; cols < gridView_now.Columns.Count; cols++)
        //            {//列信息
        //                gridView_now.Columns[cols].OptionsColumn.AllowShowHide = false;

        //            }

        //            //设置 GRID 中的底色，字体等。
        //            //this.gridView1.Appearance.Row.Font = new System.Drawing.Font("Arial", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        //            //this.gridView1.RowHeight = 20;
        //            gridView_now.Appearance.Row.Font = new System.Drawing.Font("Arial", 11F);
        //            gridView_now.RowHeight = 20;



        //        }


        //        //GRID 初始化设置，提供翻页，跳转，等小按钮功能。
        //        /// <summary>
        //        /// GRID 初始化设置：显示复选框列标题，列AutoSize,右键设置，行拖拉设置，提供翻页(first,last,next,prev,跳转页)，
        //        /// 导出等GRID自带小按钮功能。
        //        /// 记录创建者，根据EPED54配置成‘代码描述’类型后，可以显示用户代码+用户名称。
        //        /// </summary>
        //        /// <param name="efGrid_now">需要设置的GRID</param>
        //        ///<param name="v_multi_page">是否分页显示的标志[0/1=不分页/分页]</param>
        //        /// 
        //        public static void DEV_Init_grid2(EF.EFDevGrid efGrid_now, string v_multi_page)
        //        {
        //            DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;




        //            //使得整个GRID 可编辑
        //            //gridView_now.OptionsBehavior.Editable = true;

        //            ////
        //            ////初始化GRID,中，设置GRID 中选择列可操作。
        //            //string[] col_name2 = new string[] { "xx" };//
        //            ////设置某列N列信息可写.
        //            //source_dm.DEV_SetColEdit_multi(this,efGrid_now, col_name2, "1");



        //            efGrid_now.ShowContextMenu = true; //允许右键
        //            efGrid_now.AllowDragRow = false;    //不允许行拖拉
        //            efGrid_now.ShowSelectionColumn = true; //采用新方式。
        //            //efGrid_now.ShowSelectedColumn = true; //显示选择列





        //            efGrid_now.IsUseCustomPageBar = true; //显示GRID自带的工具栏


        //            if (v_multi_page.Trim() == "1")
        //            {//若有分页要求，显示分页小按钮。
        //                //翻页功能的小按钮显示。
        //                efGrid_now.PageSize = 500; //初始化每页的记录数[500]行。
        //                efGrid_now.ShowPageButton = true;

        //                //grid初始化的时候，翻页小按钮，默认都是灰的。
        //                efGrid_now.FirstPageButtonEnable = false;
        //                efGrid_now.LastPageButtonEnable = false;
        //                efGrid_now.NextPageButtonEnable = false;
        //                efGrid_now.PrePageButtonEnable = false;

        //                efGrid_now.ShowRecordCountMessage = true;
        //                efGrid_now.ShowPageToButton = true; //跳转页 
        //                efGrid_now.RecordCountMessage = string.Format(GC.GCRS.GCRSC0000056, 0, 0, 0);//初始化页数信息。


        //            }
        //            else
        //            {//不显示分页小按钮。
        //                efGrid_now.ShowPageButton = false;  
        //            }



        //            //隐藏不需要的小按钮。
        //            efGrid_now.ShowFilterButton = false; //过滤行按钮
        //            efGrid_now.ShowGroupButton = false;  //列分组按钮
        //            efGrid_now.ShowRefreshButton = false;//刷新按钮
        //            efGrid_now.ShowSaveLayoutButton = false;

        //            efGrid_now.ShowExportButton   = true; //信息导出按钮。 
        //            //efGrid_now.IsUseCustomPageBar = true;
        //            //efGrid_now.ShowAddRowButton   = true; //新增小按钮。

        //            /*//右键控制。
        //              efGrid_now.ShowContextMenu = true;
        //            efGrid_now.ShowContextMenuAddCopyNew = false;
        //            efGrid_now.ShowContextMenuAddNew = false;
        //             */


        //            //列宽自动调整。
        //            gridView_now.BestFitColumns();

        //            //若有分组，则，分组全部展开。
        //            gridView_now.ExpandAllGroups(); 




        //            ////设置GRID 中的字体大小。
        //            ////基于产品化的规范性，以下逻辑暂时屏蔽。BY ZY ON 2014-1-8 9:51:59
        //            ////==============================
        //            //gridView_now.Appearance.Row.Options.UseFont = true;
        //            //gridView_now.Appearance.Row.Font = new System.Drawing.Font("Arial", 11F);
        //            //gridView_now.RowHeight = 25;


        //            //将可编辑列设置成[蓝色+粗体]标题
        //            //================================
        //            //若有列标题信息，则设置可编辑列的颜色属性。====xxx
        //            if (EFX.EFCGrid.GetEFCGridBase(efGrid_now) != null)
        //            {
        //                EFX.EFCGrid.GetEFCGridBase(efGrid_now).TitleEditEnableColor = Color.DodgerBlue;  //设置成蓝色。 
        //                //EFX.EFCGrid.GetEFCGridBase(efGrid_now).TitleEditEnableFont = new System.Drawing.Font(form_now.Font.FontFamily, form_now.Font.Size, System.Drawing.FontStyle.Bold);//字体变化。

        //            }

        //            //grid中所有列不能被删除。 
        //            int cols = 0;
        //            for (cols = 0; cols < gridView_now.Columns.Count; cols++)
        //            {//列信息
        //                gridView_now.Columns[cols].OptionsColumn.AllowShowHide = false;

        //            }


        //            //所有列不可编辑。
        //            DEV_SetColEdit_allNot2(efGrid_now);


        //            //REC_CREATOR=提示用户名称。 //记录创建者//计划责任者//投料责任者//申请责任者
        //            //select t.ename,t.cname, t.*  from  tesuserinfo t
        //            //======================
        //            string v_sql2 = "";
        //            v_sql2 = "SELECT t.ename as CODE ,t.ename || '_' || t.cname as AABB from tesuserinfo t "
        //                + "   order by t.ename ";

        //            //调用初始化GRID 中的列信息。
        //            //记录创建者
        //            DEV_initCol_LookUpEdit(efGrid_now, "REC_CREATOR", "eped_dyn_sql", "XX", v_sql2, "CODE", "AABB");//代码+描述。 


        //            //记录修改者
        //            DEV_initCol_LookUpEdit(efGrid_now, "REC_REVISOR", "eped_dyn_sql", "XX", v_sql2, "CODE", "AABB");//代码+描述。 


        //            //PLAN_MAKER==计划责任者。 
        //            DEV_initCol_LookUpEdit(efGrid_now, "PLAN_MAKER", "eped_dyn_sql", "XX", v_sql2, "CODE", "AABB");//代码+描述。 
        //            //DEVO_MAKER==投料责任者。
        //            DEV_initCol_LookUpEdit(efGrid_now, "DEVO_MAKER", "eped_dyn_sql", "XX", v_sql2, "CODE", "AABB");//代码+描述。 

        //            //APP_MAKER;   //申请责任者
        //            DEV_initCol_LookUpEdit(efGrid_now, "APP_MAKER", "eped_dyn_sql", "XX", v_sql2, "CODE", "AABB");//代码+描述。

        //        }


        //        // //再使得当前行唯一被选中。 
        //        /// <summary>
        //        ///  //再使得当前行唯一被选中。 
        //        /// </summary>
        //        /// <param name="efGrid_now">需要操作的GRID</param>
        //        public static void DEV_Choice_one_grid(EF.EFDevGrid efGrid_now)
        //        {

        //            int rownum = 0;  //行数目
        //            string item_ename = EF.EFDevGrid.SelectionColumnFieldName;  //获取 GRID 中复选框的名称。[check_option]

        //            DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
        //            if (!gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(item_ename)))
        //            {//若没有复选框，直接离开。
        //                return;
        //            }

        //            //将所有行都不选中。
        //            for (rownum = 0; rownum <= gridView_now.RowCount; rownum++)
        //            {
        //                gridView_now.SetRowCellValue(rownum, item_ename, false); //使得指定行不被选中。
        //            }


        //            //使得当前行被选中。
        //            gridView_now.SetRowCellValue(gridView_now.FocusedRowHandle, item_ename, true); //使得当前行被选中。

        //        }





        //        //设置GRID中的所有行被选中（全不选）
        //        /// <summary>
        //        /// 设置GRID中的所有行被选中（全不选）
        //        /// </summary>
        //        /// <param name="efGrid_now">需要操作的GRID</param>
        //        public static void DEV_Choice_no_grid(EF.EFDevGrid efGrid_now)
        //        {

        //            int rownum = 0;  //行数目
        //            string item_ename = EF.EFDevGrid.SelectionColumnFieldName;  //获取 GRID 中复选框的名称。[check_option]

        //            DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
        //            if (!gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(item_ename)))
        //            {//若没有复选框，直接离开。
        //                return;
        //            }

        //            for (rownum = 0; rownum <= gridView_now.RowCount; rownum++)
        //            {
        //                gridView_now.SetRowCellValue(rownum, item_ename, false); //使得指定行不被选中。
        //            }

        //        }


        //        //设置GRID中的所有行被选中（全选）
        //        /// <summary>
        //        /// 设置GRID中的所有行被选中（全选）
        //        /// </summary>
        //        /// <param name="efGrid_now">需要操作的GRID</param>
        //        public static void DEV_Choice_all_grid(EF.EFDevGrid efGrid_now)
        //        {
        //            int rownum = 0;  //行数目 
        //            string item_ename = EF.EFDevGrid.SelectionColumnFieldName;

        //            DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
        //            if (!gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(item_ename)))
        //            {//若没有复选框，直接离开。
        //                return;
        //            }

        //            for (rownum = 0; rownum <= gridView_now.RowCount; rownum++)
        //            {
        //                gridView_now.SetRowCellValue(rownum, item_ename, true); //使得指定行不被选中。
        //            }

        //        }



        //        /// <summary>
        //        /// TREE信息的初始化。
        //        /// </summary>
        //        /// <param name="efTreeView_now"></param>
        //        /// <param name="service_name"></param>
        //        /// <param name="para_name"></param>
        //        /// <param name="para_value"></param>
        //        /// <param name="out_para"></param>
        //        /// <param name="out_para2"></param>
        //        public static void DEV_init_TreeView(EF.EFTreeView efTreeView_now, string service_name, string para_name, string para_value, string out_para, string out_para2)
        //        {

        //            EI.EIInfo inBlock = new EI.EIInfo();
        //            EI.EIInfo outBlock = new EI.EIInfo();
        //            int i = 0;



        //            if (service_name.Trim() == "" || out_para.Trim() == "" || out_para2.Trim() == "")
        //            {
        //                service_name = "eped_dyn_sql"; //框架提供的动态SQL 的service .
        //                para_name = "xx "; //入口参数的名字,此处随便给一个。
        //                para_value = "select t.CODE,t.CODE_DESC_1_CONTENT from tep0002 t where t.code_class = 'xx' ";
        //                out_para = "CODE";//返回参数的名字
        //                out_para2 = "CODE_DESC_1_CONTENT";//返回参数2的名字
        //            }



        //            out_para = out_para.ToUpper();
        //            out_para2 = out_para2.ToUpper();

        //            inBlock.SetColName(1, para_name);
        //            inBlock.SetColVal(1, para_name, para_value);//入口参数信息 
        //            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,service_name, inBlock);

        //            if (outBlock.sys_info.flag == 0)
        //            {



        //                // EF.EFTreeView efTreeView_now = this.efTreeView1;
        //                TreeNode v_tree_node; //树的NODE.


        //                string v_tree_value = ""; //树上显示的信息。

        //                efTreeView_now.Nodes.Clear();
        //                for (i = 1; i <= outBlock.Tables[0].Rows.Count; i++)
        //                {
        //                    //v_plan_backlog_code = outBlock.Tables[0].Rows[i - 1][out_para].ToString();
        //                    //v_plan_backlog_name = dt_unit.Rows[i].Field<string>("UNIT_CNAME").ToString();

        //                    v_tree_value = outBlock.Tables[0].Rows[i - 1][out_para].ToString() + "_" + outBlock.Tables[0].Rows[i - 1][out_para2].ToString();
        //                    if (v_tree_value.Trim() != "")
        //                    {
        //                        //静态数据表代码+表名称。
        //                        v_tree_node = new TreeNode(v_tree_value);
        //                        efTreeView_now.Nodes.Add(v_tree_node);
        //                    }
        //                }




        //            }
        //            else
        //            {
        //                MessageBox.Show(outBlock.sys_info.msg.ToString());
        //            }

        //        }



        //        //根据code_class初始化对应的下拉列表信息。
        //        /// <summary>
        //        /// 根据code_class初始化对应的下拉列表信息,读取表TEP0002
        //        /// </summary>
        //        /// <param name="lookupedit_now">DEV的LookUp控件</param>
        //        /// <param name="v_code_class">代码编号</param>
        //        public static void DEV_init_LookUpEdit(EF.EFDevLookUpEdit lookupedit_now, string v_code_class)
        //        {
        //            EI.EIInfo inBlock = new EI.EIInfo();
        //            EI.EIInfo outBlock = new EI.EIInfo();
        //            int i = 0;


        //            //下拉列表的列宽度自动调节。
        //            lookupedit_now.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;


        //            //框架提供的获取代码描述1 的方法。
        //            outBlock = EF.Utility.GetPartitionCodeClassValue(cs_formPartition,v_code_class);




        //            if (outBlock.Tables.Contains(v_code_class))
        //            {
        //                //新增一行空值。
        //                outBlock.Tables[v_code_class].Rows.Add();
        //                i = outBlock.Tables[v_code_class].Rows.Count;
        //                outBlock.Tables[v_code_class].Rows[i - 1]["CODE"] = " ";
        //                outBlock.Tables[v_code_class].Rows[i - 1]["CODE_DESC_1_CONTENT"] = " ";



        //                lookupedit_now.Properties.DataSource = outBlock.Tables[v_code_class];/* 代码名称 */
        //                lookupedit_now.Properties.DisplayMember = "CODE_DESC_1_CONTENT";/* 显示内容 */
        //                lookupedit_now.Properties.ValueMember = "CODE";/* 取值内容 */
        //                lookupedit_now.Properties.Columns.Clear();/* 清空 */
        //                lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "代码"));/* 定义显示列标题 */
        //                lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE_DESC_1_CONTENT", "描述"));/* 定义显示列标题 */

        //                if (outBlock.Tables[v_code_class].Rows.Count <= 6)
        //                {
        //                    lookupedit_now.Properties.DropDownRows = outBlock.Tables[v_code_class].Rows.Count; //小于等于6记录的，下拉列表的长度=记录数
        //                }
        //                else
        //                {
        //                    lookupedit_now.Properties.DropDownRows = 7; //默认是七
        //                }

        //            }

        //            //初始值，DELETE值。
        //            lookupedit_now.Properties.NullText = "";
        //            lookupedit_now.EditValue = ""; //信息若没查询成功，列表中的值是空格。



        //        }




        //        //根据code_class初始化对应的下拉列表信息。
        //        /// <summary>
        //        /// 根据code_class初始化对应的下拉列表信息,读取表TEP0002,代码模式。
        //        /// </summary>
        //        /// <param name="lookupedit_now">DEV的LookUp控件</param>
        //        /// <param name="v_code_class">代码编号</param>
        //        public static void DEV_init_LookUpEdit_code(EF.EFDevLookUpEdit lookupedit_now, string v_code_class)
        //        {
        //            EI.EIInfo inBlock = new EI.EIInfo();
        //            EI.EIInfo outBlock = new EI.EIInfo();
        //            int i = 0;





        //            outBlock = EF.Utility.GetPartitionCodeClassValue(cs_formPartition,v_code_class);




        //            if (outBlock.Tables.Contains(v_code_class))
        //            {
        //                //新增一行空值。
        //                outBlock.Tables[v_code_class].Rows.Add();
        //                i = outBlock.Tables[v_code_class].Rows.Count;
        //                outBlock.Tables[v_code_class].Rows[i - 1]["CODE"] = " ";
        //                outBlock.Tables[v_code_class].Rows[i - 1]["CODE_DESC_1_CONTENT"] = " ";



        //                lookupedit_now.Properties.DataSource = outBlock.Tables[v_code_class];/* 代码名称 */
        //                lookupedit_now.Properties.DisplayMember = "CODE";/* 显示内容 */
        //                lookupedit_now.Properties.ValueMember = "CODE";/* 取值内容 */
        //                lookupedit_now.Properties.Columns.Clear();/* 清空 */
        //                lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "代码"));/* 定义显示列标题 */
        //                lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE_DESC_1_CONTENT", "描述"));/* 定义显示列标题 */

        //                if (outBlock.Tables[v_code_class].Rows.Count <= 6)
        //                {
        //                    lookupedit_now.Properties.DropDownRows = outBlock.Tables[v_code_class].Rows.Count; //小于等于6记录的，下拉列表的长度=记录数
        //                }
        //                else
        //                {
        //                    lookupedit_now.Properties.DropDownRows = 7; //默认是七
        //                }

        //            }

        //            //初始值，DELETE值。
        //            lookupedit_now.Properties.NullText = "";
        //            lookupedit_now.EditValue = ""; //信息若没查询成功，列表中的值是空格。



        //        }



        //        //DEV_init_LookUpEdit_desc
        //        //根据[code_class]初始化对应的下拉列表信息,并将代码描述显示在指定列中。
        //        /// <summary>
        //        /// 根据[code_class]初始化对应的下拉列表信息,并将代码描述显示在指定列中。
        //        /// </summary>
        //        /// <param name="lookupedit_now">DEV的LookUp控件</param>
        //        /// <param name="v_code_class">代码编号</param>
        //        public static void DEV_init_LookUpEdit_desc(EF.EFDevLookUpEdit lookupedit_now, string v_code_class, string[] col_name)
        //        {
        //            EI.EIInfo inBlock = new EI.EIInfo();
        //            EI.EIInfo outBlock = new EI.EIInfo();
        //            int i = 0;
        //            string v_code_desc_name = "";
        //            string v_column_name = "";
        //            string v_column_desc = "";

        //            outBlock = EF.Utility.GetPartitionCodeClassValue(cs_formPartition,v_code_class);

        //            if (outBlock.Tables.Contains(v_code_class))
        //            {
        //                //新增一行空值。
        //                outBlock.Tables[v_code_class].Rows.Add();
        //                i = outBlock.Tables[v_code_class].Rows.Count;
        //                outBlock.Tables[v_code_class].Rows[i - 1]["CODE"] = " ";
        //                outBlock.Tables[v_code_class].Rows[i - 1]["CODE_DESC_1_CONTENT"] = " ";



        //                lookupedit_now.Properties.DataSource = outBlock.Tables[v_code_class];/* 代码名称 */
        //                lookupedit_now.Properties.DisplayMember = "CODE";/* 显示内容=代码值 */
        //                lookupedit_now.Properties.ValueMember = "CODE";/* 取值内容 */
        //                lookupedit_now.Properties.Columns.Clear();/* 清空 */

        //                lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "代码"));/* 定义显示列标题 */
        //                //lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE_DESC_1_CONTENT", "描述"));/* 定义显示列标题 */
        //                for (i = 1; i <= col_name.Length - 1; i++)
        //                {//获取绑定字段的个数-1
        //                    v_column_name = string.Format("CODE_DESC_{0}_CONTENT", i.ToString());
        //                    v_column_desc = string.Format("描述{0}", i.ToString());
        //                    lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(v_column_name, v_column_desc));/* 定义显示列标题 */
        //                }

        //                if (outBlock.Tables[v_code_class].Rows.Count <= 6)
        //                {
        //                    lookupedit_now.Properties.DropDownRows = outBlock.Tables[v_code_class].Rows.Count; //小于等于6记录的，下拉列表的长度=记录数
        //                }
        //                else
        //                {
        //                    lookupedit_now.Properties.DropDownRows = 7; //默认是七
        //                }

        //            }

        //            //初始值，DELETE值。
        //            lookupedit_now.Properties.NullText = "";
        //            lookupedit_now.EditValue = ""; //信息若没查询成功，列表中的值是空格。 

        //        }



        //        //DEV_init_LookUpEdit_desc
        //        //根据[code_class]初始化对应的下拉列表信息,并将代码描述显示在指定列中。

        //        /// <summary>
        //        /// 根据[code_class]初始化对应的[N列]的下拉信息, 
        //        /// </summary>
        //        /// <param name="lookupedit_now"></param>
        //        /// <param name="v_code_class"></param>
        //        /// <param name="col_num"></param>
        //        public static void DEV_init_LookUpEdit_code(EF.EFDevLookUpEdit lookupedit_now, string v_code_class, int col_num)
        //        {
        //            EI.EIInfo inBlock = new EI.EIInfo();
        //            EI.EIInfo outBlock = new EI.EIInfo();
        //            int i = 0;
        //            string v_code_desc_name = "";
        //            string v_column_name = "";
        //            string v_column_desc = "";

        //            outBlock = EF.Utility.GetPartitionCodeClassValue(cs_formPartition,v_code_class);

        //            if (outBlock.Tables.Contains(v_code_class))
        //            {
        //                //新增一行空值。
        //                outBlock.Tables[v_code_class].Rows.Add();
        //                i = outBlock.Tables[v_code_class].Rows.Count;
        //                outBlock.Tables[v_code_class].Rows[i - 1]["CODE"] = " ";
        //                outBlock.Tables[v_code_class].Rows[i - 1]["CODE_DESC_1_CONTENT"] = " ";



        //                lookupedit_now.Properties.DataSource = outBlock.Tables[v_code_class];/* 代码名称 */
        //                lookupedit_now.Properties.DisplayMember = "CODE";/* 显示内容=代码值 */
        //                lookupedit_now.Properties.ValueMember = "CODE";/* 取值内容 */
        //                lookupedit_now.Properties.Columns.Clear();/* 清空 */

        //                lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE", "代码"));/* 定义显示列标题 */
        //                //lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo("CODE_DESC_1_CONTENT", "描述"));/* 定义显示列标题 */
        //                for (i = 1; i <= col_num - 1; i++)
        //                {//获取绑定字段的个数-1
        //                    v_column_name = string.Format("CODE_DESC_{0}_CONTENT", i.ToString());
        //                    v_column_desc = string.Format("描述{0}", i.ToString());
        //                    lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(v_column_name, v_column_desc));/* 定义显示列标题 */
        //                }

        //                if (outBlock.Tables[v_code_class].Rows.Count <= 6)
        //                {
        //                    lookupedit_now.Properties.DropDownRows = outBlock.Tables[v_code_class].Rows.Count; //小于等于6记录的，下拉列表的长度=记录数
        //                }
        //                else
        //                {
        //                    lookupedit_now.Properties.DropDownRows = 7; //默认是七
        //                }

        //            }

        //            //初始值，DELETE值。
        //            lookupedit_now.Properties.NullText = "";
        //            lookupedit_now.EditValue = ""; //信息若没查询成功，列表中的值是空格。 

        //        }



        //        //根据[service_name]初始化对应的下拉列表信息。
        //        /// <summary>
        //        /// 根据[service_name]初始化对应的下拉列表信息。
        //        /// </summary>
        //        /// <param name="lookupedit_now">DEV的LookUp控件</param>
        //        /// <param name="in_para">入口参数值</param>
        //        /// <param name="service_name">业务service名称</param>
        //        public static void DEV_init_LookUpEdit(EF.EFDevLookUpEdit lookupedit_now, string in_para, string service_name)
        //        {
        //            EI.EIInfo inBlock = new EI.EIInfo();
        //            EI.EIInfo outBlock = new EI.EIInfo();
        //            int i = 0;

        //            //outBlock = EF.Utility.GetPartitionCodeClassValue(cs_formPartition,v_code_class);
        //            //下拉列表的列宽度自动调节。
        //            lookupedit_now.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;



        //            //初始化代码查询的功能。
        //            string para_name = "code_class"; //入口参数的名字
        //            string out_para = "CODE";       //返回参数的名字
        //            string out_para2 = "CODE_DESC_1_CONTENT";//返回参数2的名字


        //            if(service_name.Trim() == "")
        //            {//PM代码查询
        //                service_name = "epep01_inq2"; //框架提供的代码查询功能。
        //                para_name = "code_class"; //入口参数的名字
        //                out_para = "CODE";//返回参数的名字
        //                out_para2 = "CODE_DESC_1_CONTENT";//返回参数2的名字
        //            }


        //            out_para = out_para.ToUpper();
        //            out_para2 = out_para2.ToUpper();


        //            inBlock.SetColName(1, para_name);
        //            inBlock.SetColVal(1, para_name, in_para);//入口参数信息=检验标准
        //            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,service_name, inBlock);
        //            if (outBlock.sys_info.flag == 0)
        //            {


        //                //新增一行空值。
        //                outBlock.Tables[0].Rows.Add();
        //                i = outBlock.Tables[0].Rows.Count;
        //                outBlock.Tables[0].Rows[i - 1][out_para] = " ";
        //                outBlock.Tables[0].Rows[i - 1][out_para2] = " ";

        //                if (outBlock.sys_info.flag == 0)
        //                {

        //                    lookupedit_now.Properties.DataSource = outBlock.Tables[0];/* 代码名称 */
        //                    lookupedit_now.Properties.DisplayMember = out_para2;/* 显示内容 */
        //                    lookupedit_now.Properties.ValueMember = out_para;/* 取值内容 */
        //                    lookupedit_now.Properties.Columns.Clear();/* 清空 */
        //                    lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(out_para, "代码"));/* 定义显示列标题 */
        //                    if (out_para != out_para2)
        //                    {//若内容列！= 显示列，则显示描述信息。
        //                        lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(out_para2, "描述"));/* 定义显示列标题 */
        //                    }


        //                    if (outBlock.Tables[0].Rows.Count <= 6)
        //                    {
        //                        lookupedit_now.Properties.DropDownRows = outBlock.Tables[0].Rows.Count; //小于等于6记录的，下拉列表的长度=记录数
        //                    }
        //                    else
        //                    {
        //                        lookupedit_now.Properties.DropDownRows = 7; //默认是七
        //                    }

        //                }
        //                else
        //                {
        //                    MessageBox.Show(outBlock.sys_info.msg.ToString());
        //                }
        //            }

        //            lookupedit_now.Properties.NullText = "";
        //            lookupedit_now.EditValue = " "; //信息若没查询成功，列表中的值是空格。







        //        }



        //        /// <summary>
        //        /// /根据[service_name]初始化对应的下拉列表信息。
        //        /// </summary>
        //        /// <param name="lookupedit_now">DEV的LookUp控件</param>
        //        /// <param name="service_name">业务service名称</param>
        //        /// <param name="para_name">入口参数名称</param>
        //        /// <param name="para_value">入口参数值</param>
        //        /// <param name="out_para">出口参数值1</param>
        //        /// <param name="out_para2">出口参数值2</param>
        //        public static void DEV_init_LookUpEdit(EF.EFDevLookUpEdit lookupedit_now, string service_name, string para_name, string para_value, string out_para, string out_para2)
        //        {
        //            EI.EIInfo inBlock = new EI.EIInfo();
        //            EI.EIInfo outBlock = new EI.EIInfo();
        //            int i = 0;


        //            //.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
        //            //下拉列表的列宽度自动调节。
        //            lookupedit_now.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;



        //            //outBlock = EF.Utility.GetPartitionCodeClassValue(cs_formPartition,v_code_class);

        //            if (service_name.Trim() == "" || out_para.Trim() == "" || out_para2.Trim() == "")
        //            {
        //                service_name = "epep01_inq2";
        //                para_name = "code_class"; //入口参数的名字
        //                //para_value = "";
        //                out_para = "CODE";//返回参数的名字
        //                out_para2 = "CODE_DESC_1_CONTENT";//返回参数2的名字
        //            }



        //            out_para = out_para.ToUpper();
        //            out_para2 = out_para2.ToUpper();

        //            inBlock.SetColName(1, para_name);
        //            inBlock.SetColVal(1, para_name, para_value);//入口参数信息 
        //            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,service_name, inBlock);

        //            if (outBlock.sys_info.flag == 0)
        //            {

        //                //新增一行空值。
        //                outBlock.Tables[0].Rows.Add();
        //                i = outBlock.Tables[0].Rows.Count;
        //                outBlock.Tables[0].Rows[i - 1][out_para] = " ";
        //                outBlock.Tables[0].Rows[i - 1][out_para2] = " ";

        //                if (outBlock.sys_info.flag == 0)
        //                {

        //                    lookupedit_now.Properties.DataSource = outBlock.Tables[0];/* 代码名称 */
        //                    lookupedit_now.Properties.DisplayMember = out_para2;/* 显示内容= */
        //                    lookupedit_now.Properties.ValueMember = out_para;/* 取值内容 */
        //                    lookupedit_now.Properties.Columns.Clear();/* 清空 */

        //                    lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(out_para, "代码"));/* 定义显示列标题 */
        //                    if (out_para != out_para2)
        //                    {//若内容列！= 显示列，则显示描述信息。
        //                        lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(out_para2, "描述"));/* 定义显示列标题 */
        //                    }






        //                    if (outBlock.Tables[0].Rows.Count <= 6)
        //                    {
        //                        lookupedit_now.Properties.DropDownRows = outBlock.Tables[0].Rows.Count; //小于等于6记录的，下拉列表的长度=记录数
        //                    }
        //                    else
        //                    {
        //                        lookupedit_now.Properties.DropDownRows = 7; //默认是七
        //                    }
        //                }
        //                else
        //                {
        //                    MessageBox.Show(outBlock.sys_info.msg.ToString());
        //                }
        //            }

        //            lookupedit_now.Properties.NullText = "";
        //            lookupedit_now.EditValue = ""; //信息若没查询成功，列表中的值是空格。







        //        }





        //        /// <summary>
        //        /// /根据[service_name]初始化对应的下拉列表信息,根据[null_flag=0/1=不空/增空]标志，来是否新增空行。
        //        /// </summary>
        //        /// <param name="lookupedit_now">DEV的LookUp控件</param>
        //        /// <param name="service_name">业务service名称</param>
        //        /// <param name="para_name">入口参数名称</param>
        //        /// <param name="para_value">入口参数值</param>
        //        /// <param name="out_para">出口参数值1</param>
        //        /// <param name="out_para2">出口参数值2</param>
        //        /// <param name="null_flag">新增空行标志= 0/1=不增空行/新增空行</param>
        //        public static void DEV_init_LookUpEdit2(EF.EFDevLookUpEdit lookupedit_now, string service_name, string para_name, string para_value, string out_para, string out_para2, string null_flag)
        //        {
        //            EI.EIInfo inBlock = new EI.EIInfo();
        //            EI.EIInfo outBlock = new EI.EIInfo();
        //            int i = 0;


        //            //.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
        //            //下拉列表的列宽度自动调节。
        //            lookupedit_now.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;



        //            //outBlock = EF.Utility.GetPartitionCodeClassValue(cs_formPartition,v_code_class);

        //            if (service_name.Trim() == "" || out_para.Trim() == "" || out_para2.Trim() == "")
        //            {
        //                service_name = "epep01_inq2";
        //                para_name = "code_class"; //入口参数的名字
        //                //para_value = "";
        //                out_para = "CODE";//返回参数的名字
        //                out_para2 = "CODE_DESC_1_CONTENT";//返回参数2的名字
        //            }



        //            out_para = out_para.ToUpper();
        //            out_para2 = out_para2.ToUpper();

        //            inBlock.SetColName(1, para_name);
        //            inBlock.SetColVal(1, para_name, para_value);//入口参数信息 
        //            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,service_name, inBlock);

        //            if (outBlock.sys_info.flag == 0)
        //            {
        //                if (null_flag.Trim() == "1")
        //                {//若要求新增空行，才新增。
        //                    //新增一行空值。
        //                    outBlock.Tables[0].Rows.Add();
        //                    i = outBlock.Tables[0].Rows.Count;
        //                    outBlock.Tables[0].Rows[i - 1][out_para] = " ";
        //                    outBlock.Tables[0].Rows[i - 1][out_para2] = " ";
        //                }

        //                if (outBlock.sys_info.flag == 0)
        //                {

        //                    lookupedit_now.Properties.DataSource = outBlock.Tables[0];/* 代码名称 */
        //                    lookupedit_now.Properties.DisplayMember = out_para2;/* 显示内容= */
        //                    lookupedit_now.Properties.ValueMember = out_para;/* 取值内容 */
        //                    lookupedit_now.Properties.Columns.Clear();/* 清空 */

        //                    lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(out_para, "代码"));/* 定义显示列标题 */
        //                    if (out_para != out_para2)
        //                    {//若内容列！= 显示列，则显示描述信息。
        //                        lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(out_para2, "描述"));/* 定义显示列标题 */
        //                    }






        //                    if (outBlock.Tables[0].Rows.Count <= 6)
        //                    {
        //                        lookupedit_now.Properties.DropDownRows = outBlock.Tables[0].Rows.Count; //小于等于6记录的，下拉列表的长度=记录数
        //                    }
        //                    else
        //                    {
        //                        lookupedit_now.Properties.DropDownRows = 7; //默认是七
        //                    }
        //                }
        //                else
        //                {
        //                    MessageBox.Show(outBlock.sys_info.msg.ToString());
        //                }
        //            }

        //            lookupedit_now.Properties.NullText = "";
        //            lookupedit_now.EditValue = ""; //信息若没查询成功，列表中的值是空格。







        //        }







        //        //DEV_init_LookUpEdit_one()[单列模式]
        //        /// <summary>
        //        /// 下拉控件的单列显示模式。
        //        /// </summary>
        //        /// <param name="lookupedit_now">下拉控件</param>
        //        /// <param name="service_name">service名称</param>
        //        /// <param name="para_name">入口参数名称</param>
        //        /// <param name="para_value">入口参数值</param>
        //        /// <param name="out_para">返回信息</param>
        //        /// <param name="null_flag">新增空行标志= 0/1=不增空行/新增空行</param>
        //        public static void DEV_init_LookUpEdit_one3(EF.EFDevLookUpEdit lookupedit_now, string service_name, string para_name, string para_value, string out_para, string null_flag)
        //        {
        //            EI.EIInfo inBlock = new EI.EIInfo();
        //            EI.EIInfo outBlock = new EI.EIInfo();
        //            int i = 0;


        //            //.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
        //            //下拉列表的列宽度自动调节。
        //            lookupedit_now.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;



        //            //outBlock = EF.Utility.GetPartitionCodeClassValue(cs_formPartition,v_code_class);

        //            if (service_name.Trim() == "" || out_para.Trim() == "")
        //            {
        //                service_name = "epep01_inq2";
        //                para_name = "code_class"; //入口参数的名字
        //                //para_value = "";
        //                out_para = "CODE";//返回参数的名字

        //            }



        //            out_para = out_para.ToUpper();


        //            inBlock.SetColName(1, para_name);
        //            inBlock.SetColVal(1, para_name, para_value);//入口参数信息 
        //            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,service_name, inBlock);
        //            if (outBlock != null)
        //            {

        //                if (null_flag == "1")
        //                {
        //                    //新增一行空值。
        //                    outBlock.Tables[0].Rows.Add();
        //                    i = outBlock.Tables[0].Rows.Count;
        //                    outBlock.Tables[0].Rows[i - 1][out_para] = " ";
        //                }




        //                if (outBlock.sys_info.flag == 0)
        //                {

        //                    lookupedit_now.Properties.DataSource = outBlock.Tables[0];/* 代码名称 */
        //                    lookupedit_now.Properties.DisplayMember = out_para;/* 显示内容= */
        //                    lookupedit_now.Properties.ValueMember = out_para;/* 取值内容 */
        //                    lookupedit_now.Properties.Columns.Clear();/* 清空 */
        //                    lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(out_para, "代码"));/* 定义显示列标题 */


        //                    if (outBlock.Tables[0].Rows.Count <= 6)
        //                    {
        //                        lookupedit_now.Properties.DropDownRows = outBlock.Tables[0].Rows.Count; //小于等于6记录的，下拉列表的长度=记录数
        //                    }
        //                    else
        //                    {
        //                        lookupedit_now.Properties.DropDownRows = 7; //默认是七
        //                    }
        //                }
        //                else
        //                {
        //                    MessageBox.Show(outBlock.sys_info.msg.ToString());
        //                }
        //            }

        //            lookupedit_now.Properties.NullText = "";
        //            lookupedit_now.EditValue = ""; //信息若没查询成功，列表中的值是空格。 

        //        }



        //        /// <summary>
        //        /// 根据 outBlock中的信息，加载到下拉列表控件中。[单列信息]
        //        /// </summary>
        //        /// <param name="lookupedit_now">DEV的LookUp控件</param>
        //        /// <param name="outBlock"></param>
        //        /// <param name="out_para">返回列名</param>
        //        /// <param name="out_desc">返回列标题</param>
        //        /// <param name="null_flag">新增空行标志= 0/1=不增空行/新增空行</param> 
        //        public static void DEV_init_LookUpEdit_one2(EF.EFDevLookUpEdit lookupedit_now, EI.EIInfo outBlock, string out_para, string out_desc, string null_flag)
        //        {

        //            //下拉列表的列宽度自动调节。
        //            lookupedit_now.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
        //            int i = 0;


        //            if (outBlock != null)
        //            {

        //                if (null_flag == "1")
        //                {
        //                    //新增一行空值。
        //                    outBlock.Tables[0].Rows.Add();
        //                    i = outBlock.Tables[0].Rows.Count;
        //                    outBlock.Tables[0].Rows[i - 1][out_para] = " ";
        //                }


        //                if (outBlock.sys_info.flag == 0)
        //                {

        //                    lookupedit_now.Properties.DataSource = outBlock.Tables[0];/* 代码名称 */
        //                    lookupedit_now.Properties.DisplayMember = out_para;/* 显示内容= */
        //                    lookupedit_now.Properties.ValueMember = out_para;/* 取值内容 */
        //                    lookupedit_now.Properties.Columns.Clear();/* 清空 */
        //                    lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(out_para, out_desc));/* 定义显示列标题 */


        //                    if (outBlock.Tables[0].Rows.Count <= 6)
        //                    {
        //                        lookupedit_now.Properties.DropDownRows = outBlock.Tables[0].Rows.Count; //小于等于6记录的，下拉列表的长度=记录数
        //                    }
        //                    else
        //                    {
        //                        lookupedit_now.Properties.DropDownRows = 7; //默认是七
        //                    }
        //                }
        //                else
        //                {
        //                    MessageBox.Show(outBlock.sys_info.msg.ToString());
        //                }
        //            }

        //            lookupedit_now.Properties.NullText = "";
        //            lookupedit_now.EditValue = ""; //信息若没查询成功，列表中的值是空格。 

        //        }



        //        //DEV_init_LookUpEdit_one()[单列模式]
        //        /// <summary>
        //        /// 下拉控件的单列显示模式。
        //        /// </summary>
        //        /// <param name="lookupedit_now">下拉控件</param>
        //        /// <param name="service_name">service名称</param>
        //        /// <param name="para_name">入口参数名称</param>
        //        /// <param name="para_value">入口参数值</param>
        //        /// <param name="out_para">返回信息</param>
        //        public static void DEV_init_LookUpEdit_one(EF.EFDevLookUpEdit lookupedit_now, string service_name, string para_name, string para_value, string out_para)
        //        {
        //            EI.EIInfo inBlock = new EI.EIInfo();
        //            EI.EIInfo outBlock = new EI.EIInfo();
        //            int i = 0;


        //            //.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
        //            //下拉列表的列宽度自动调节。
        //            lookupedit_now.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;



        //            //outBlock = EF.Utility.GetPartitionCodeClassValue(cs_formPartition,v_code_class);

        //            if (service_name.Trim() == "" || out_para.Trim() == "")
        //            {
        //                service_name = "epep01_inq2";
        //                para_name = "code_class"; //入口参数的名字
        //                //para_value = "";
        //                out_para = "CODE";//返回参数的名字

        //            }



        //            out_para = out_para.ToUpper();


        //            inBlock.SetColName(1, para_name);
        //            inBlock.SetColVal(1, para_name, para_value);//入口参数信息 
        //            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,service_name, inBlock);
        //            if (outBlock != null)
        //            {

        //                //新增一行空值。
        //                outBlock.Tables[0].Rows.Add();
        //                i = outBlock.Tables[0].Rows.Count;
        //                outBlock.Tables[0].Rows[i - 1][out_para] = " ";


        //                if (outBlock.sys_info.flag == 0)
        //                {

        //                    lookupedit_now.Properties.DataSource = outBlock.Tables[0];/* 代码名称 */
        //                    lookupedit_now.Properties.DisplayMember = out_para;/* 显示内容= */
        //                    lookupedit_now.Properties.ValueMember = out_para;/* 取值内容 */
        //                    lookupedit_now.Properties.Columns.Clear();/* 清空 */
        //                    lookupedit_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(out_para, "代码"));/* 定义显示列标题 */


        //                    if (outBlock.Tables[0].Rows.Count <= 6)
        //                    {
        //                        lookupedit_now.Properties.DropDownRows = outBlock.Tables[0].Rows.Count; //小于等于6记录的，下拉列表的长度=记录数
        //                    }
        //                    else
        //                    {
        //                        lookupedit_now.Properties.DropDownRows = 7; //默认是七
        //                    }
        //                }
        //                else
        //                {
        //                    MessageBox.Show(outBlock.sys_info.msg.ToString());
        //                }
        //            }

        //            lookupedit_now.Properties.NullText = "";
        //            lookupedit_now.EditValue = ""; //信息若没查询成功，列表中的值是空格。 

        //        }























        //        //DEV_init_DevCheckedComboBoxEdit_one(),对控件[DevCheckedComboBoxEdit]的信息列初始化。
        //        /// <summary>
        //        /// 对控件[DevCheckedComboBoxEdit]的信息列初始化。[单列模式]
        //        /// </summary>
        //        /// <param name="checkedcombobox_now"></param>
        //        /// <param name="service_name"></param>
        //        /// <param name="para_name"></param>
        //        /// <param name="para_value"></param>
        //        /// <param name="out_para"></param>
        //        public static void DEV_init_DevCheckedComboBoxEdit_one(EF.EFDevCheckedComboBoxEdit checkedcombobox_now, string service_name, string para_name, string para_value, string out_para)
        //        {
        //            EI.EIInfo inBlock = new EI.EIInfo();
        //            EI.EIInfo outBlock = new EI.EIInfo();
        //            int i = 0;


        //            //.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
        //            //下拉列表的列宽度自动调节。
        //            //lookupedit_now.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;


        //            checkedcombobox_now.Properties.SelectAllItemVisible = true;
        //            checkedcombobox_now.Properties.ShowButtons = true;
        //            checkedcombobox_now.Properties.ShowPopupCloseButton = true;
        //            checkedcombobox_now.Properties.ShowPopupShadow = true;
        //            checkedcombobox_now.Properties.PopupSizeable = true;
        //            checkedcombobox_now.Properties.AutoHeight = true;









        //            out_para = out_para.ToUpper();


        //            inBlock.SetColName(1, para_name);
        //            inBlock.SetColVal(1, para_name, para_value);//入口参数信息 
        //            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,service_name, inBlock);
        //            if (outBlock != null)
        //            {

        //                ////新增一行空值。
        //                //outBlock.Tables[0].Rows.Add();
        //                //i = outBlock.Tables[0].Rows.Count;
        //                //outBlock.Tables[0].Rows[i - 1][out_para] = " ";


        //                if (outBlock.sys_info.flag == 0)
        //                {

        //                    checkedcombobox_now.Properties.DataSource = outBlock.Tables[0];/* 代码名称 */
        //                    checkedcombobox_now.Properties.DisplayMember = out_para;/* 显示内容= */
        //                    checkedcombobox_now.Properties.ValueMember = out_para;/* 取值内容 */

        //                    for (i = 0; i < outBlock.Tables[0].Rows.Count; i++)
        //                    {
        //                        checkedcombobox_now.Properties.Items.Add(outBlock.Tables[0].Rows[i][out_para], CheckState.Unchecked, true);
        //                    }


        //                    //checkedcombobox_now.Properties.Columns.Clear();/* 清空 */
        //                    //checkedcombobox_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(out_para, "代码"));/* 定义显示列标题 */


        //                    checkedcombobox_now.Properties.SelectAllItemVisible = true;
        //                    checkedcombobox_now.Properties.SelectAllItemCaption = "(Select All)";
        //                    checkedcombobox_now.Properties.SeparatorChar = ',';//分隔符 是逗号。


        //                    if (outBlock.Tables[0].Rows.Count <= 6)
        //                    {
        //                        checkedcombobox_now.Properties.DropDownRows = outBlock.Tables[0].Rows.Count; //小于等于6记录的，下拉列表的长度=记录数
        //                    }
        //                    else
        //                    {
        //                        checkedcombobox_now.Properties.DropDownRows = 7; //默认是七
        //                    }
        //                }
        //                else
        //                {
        //                    MessageBox.Show(outBlock.sys_info.msg.ToString());
        //                }
        //            }

        //            checkedcombobox_now.Properties.NullText = "";
        //            checkedcombobox_now.EditValue = ""; //信息若没查询成功，列表中的值是空格。 

        //        }





        //        //DEV_init_DevCheckedComboBoxEdit(),对控件[DevCheckedComboBoxEdit]的信息列初始化。
        //        /// <summary>
        //        /// 对控件[DevCheckedComboBoxEdit]的信息列初始化。[双列模式]
        //        /// </summary>
        //        /// <param name="checkedcombobox_now"></param>
        //        /// <param name="service_name"></param>
        //        /// <param name="para_name"></param>
        //        /// <param name="para_value"></param>
        //        /// <param name="out_para"></param>
        //        public static void DEV_init_DevCheckedComboBoxEdit(EF.EFDevCheckedComboBoxEdit checkedcombobox_now, string service_name, string para_name, string para_value, string out_para, string out_para2)
        //        {
        //            EI.EIInfo inBlock = new EI.EIInfo();
        //            EI.EIInfo outBlock = new EI.EIInfo();
        //            int i = 0;
        //            string v_item_value = "";


        //            //.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
        //            //下拉列表的列宽度自动调节。
        //            //lookupedit_now.Properties.BestFitMode = DevExpress.XtraEditors.Controls.BestFitMode.BestFitResizePopup;
        //            checkedcombobox_now.Properties.BestFitWidth = (int)DevExpress.XtraEditors.Controls.BestFitMode.BestFit;


        //            checkedcombobox_now.Properties.SelectAllItemVisible = true;
        //            checkedcombobox_now.Properties.ShowButtons = true;
        //            checkedcombobox_now.Properties.ShowPopupCloseButton = true;
        //            checkedcombobox_now.Properties.ShowPopupShadow = true;
        //            checkedcombobox_now.Properties.PopupSizeable = true;




        //            //outBlock = EF.Utility.GetPartitionCodeClassValue(cs_formPartition,v_code_class);

        //            if (service_name.Trim() == "" || out_para.Trim() == "")
        //            {
        //                service_name = "epep01_inq2";
        //                para_name = "code_class"; //入口参数的名字
        //                //para_value = "";
        //                out_para = "CODE";//返回参数的名字 
        //                out_para2 = "CODE_DESC_1_CONTENT";//返回参数2的名字
        //            }



        //            out_para = out_para.ToUpper();


        //            inBlock.SetColName(1, para_name);
        //            inBlock.SetColVal(1, para_name, para_value);//入口参数信息 
        //            outBlock = EI.EIManager.Instance.CallService(cs_formPartition,service_name, inBlock);
        //            if (outBlock != null)
        //            {

        //                ////新增一行空值。
        //                //outBlock.Tables[0].Rows.Add();
        //                //i = outBlock.Tables[0].Rows.Count;
        //                //outBlock.Tables[0].Rows[i - 1][out_para] = " ";
        //                //outBlock.Tables[0].Rows[i - 1][out_para2] = " ";


        //                if (outBlock.sys_info.flag == 0)
        //                {

        //                    checkedcombobox_now.Properties.DataSource = outBlock.Tables[0];/* 代码名称 */
        //                    checkedcombobox_now.Properties.DisplayMember = out_para2;/* 显示内容=2列信息 */
        //                    checkedcombobox_now.Properties.ValueMember = out_para;/* 取值内容 */

        //                    for (i = 0; i < outBlock.Tables[0].Rows.Count; i++)
        //                    {
        //                        v_item_value = outBlock.Tables[0].Rows[i][out_para].ToString();
        //                        checkedcombobox_now.Properties.Items.Add(v_item_value, CheckState.Unchecked, true);

        //                    }


        //                    //checkedcombobox_now.Properties.Columns.Clear();/* 清空 */
        //                    //checkedcombobox_now.Properties.Columns.Add(new DevExpress.XtraEditors.Controls.LookUpColumnInfo(out_para, "代码"));/* 定义显示列标题 */


        //                    checkedcombobox_now.Properties.SelectAllItemVisible = true;
        //                    checkedcombobox_now.Properties.SelectAllItemCaption = "(Select All)";
        //                    checkedcombobox_now.Properties.SeparatorChar = ',';//分隔符 是逗号。



        //                    ////复选框控件的行个数处理，比数据信息多4行。
        //                    //if (outBlock.Tables[0].Rows.Count <= 6)
        //                    //{
        //                    //    checkedcombobox_now.Properties.DropDownRows = outBlock.Tables[0].Rows.Count + 4; //小于等于6记录的，下拉列表的长度=记录数
        //                    //}
        //                    //else
        //                    //{
        //                    //    checkedcombobox_now.Properties.DropDownRows = 7 + 4; //默认是七
        //                    //}
        //                }
        //                else
        //                {
        //                    MessageBox.Show(outBlock.sys_info.msg.ToString());
        //                }
        //            }

        //            checkedcombobox_now.Properties.NullText = "";
        //            checkedcombobox_now.EditValue = ""; //信息若没查询成功，列表中的值是空格。 

        //        }



        //        //
        //        /// <summary>
        //        /// 将grid的前N列字段信息,左冻结
        //        /// </summary>
        //        /// <param name="efgrid"></param>
        //        /// <param name="fix_len"></param>
        //        public static void DEV_ColFixedLeft(EF.EFDevGrid efgrid, int fix_len)
        //        {
        //            int i = 0;
        //            string item_ename = EF.EFDevGrid.SelectionColumnFieldName; //"selected";
        //            DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efgrid.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
        //            if (gridView_now == null) return;


        //            //复选框永远靠左。
        //            if (gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(item_ename)))
        //            {
        //                gridView_now.Columns.ColumnByFieldName(item_ename).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left; //这个字段的靠左冻结。

        //            }
        //            if (gridView_now.Columns.Count < fix_len)

        //                fix_len = gridView_now.Columns.Count;
        //            for (i = 0; i < fix_len; i++)
        //            {//使得多列为处于冻结状态

        //                gridView_now.Columns[i].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left; //这个字段的靠左冻结。

        //            }

        //        }



        //        //将数组中的列信息，靠左冻结
        //        /// <summary>
        //        /// 将数组中的列信息，靠左冻结
        //        /// </summary>
        //        /// <param name="efgrid">DEVGRID控件</param>
        //        /// <param name="col_name">需要冻结的列数组</param>
        //        public static void DEV_ColFixedLeft(EF.EFDevGrid efgrid, string[] col_name)
        //        {
        //            int i = 0;
        //            string item_ename = EF.EFDevGrid.SelectionColumnFieldName;  //获取 GRID 中复选框的名称。[check_option]
        //            DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efgrid.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
        //            if (gridView_now == null) return;


        //            //复选框永远靠左。
        //            if (gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(item_ename)))
        //            {
        //                gridView_now.Columns.ColumnByFieldName(item_ename).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left; //这个字段的靠左冻结。

        //            }

        //            for (i = 0; i < col_name.Length; i++)
        //            {//使得多列为可写状态
        //                item_ename = col_name[i].ToString().Trim().ToUpper();


        //                //若是GRID 中不存在的字段，则继续下一个字段循环
        //                if (!gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(item_ename))) continue;

        //                gridView_now.Columns.ColumnByFieldName(item_ename).Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left; //这个字段的靠左冻结。

        //            }




        //        }






        //        /// <summary>
        //        /// 根据GRID 中的指定列【选中行+指定列】信息进行汇总计算,返回的是汇总的数字信息。
        //        /// </summary>
        //        /// <param name="efGrid_now">GRID </param>
        //        /// <param name="col_name">列名称</param>
        //        public static double DEV_SetCol_sum(EF.EFDevGrid efGrid_now, string col_name)
        //        {

        //            DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;


        //            double v_value = 0;
        //            int i = 0;
        //            for (i = 0; i < gridView_now.RowCount; i++)
        //            {
        //                if (efGrid_now.GetSelectedColumnChecked(i) == true)
        //                    v_value = v_value + Convert.ToDouble(gridView_now.GetRowCellValue(i, col_name));
        //            }
        //            return v_value;
        //        }


        //        /// <summary>
        //        /// 根据GRID 中的指定列【选中行】的【指定列*指定列2】信息进行汇总计算,返回的是汇总的数字信息。
        //        /// </summary>
        //        /// <param name="efGrid_now">指定的GRID </param>
        //        /// <param name="col_name">列名</param>
        //        /// <param name="col_name2">列名2</param>
        //        /// <returns></returns>
        //        public static double DEV_SetCol_sum2(EF.EFDevGrid efGrid_now, string col_name, string col_name2)
        //        {

        //            DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;


        //            double v_value = 0;
        //            double v_value2 = 0;
        //            int i = 0;
        //            for (i = 0; i < gridView_now.RowCount; i++)
        //            {
        //                if (efGrid_now.GetSelectedColumnChecked(i) == true)
        //                {
        //                    v_value2 = Convert.ToDouble(gridView_now.GetRowCellValue(i, col_name)) * Convert.ToDouble(gridView_now.GetRowCellValue(i, col_name2));

        //                    v_value = v_value + v_value2;

        //                }


        //            }
        //            return v_value;
        //        }






        //        //根据定义的数组信息，进行可编辑列设置。
        //        /// <summary>
        //        /// 根据定义的数组信息，进行可编辑列设置。[EFX]方式
        //        /// 设置grid某几列的可写性,并修改字体颜色(蓝色+粗体),
        //        /// </summary> 
        //        /// <param name="efgrid">当前GRID</param>
        //        /// <param name="col_name">可编辑的列数组信息</param>
        //        /// <param name="color_num">颜色代码（此代码暂不启用）</param>
        //        public static void DEV_SetColEdit_multi(EF.EFDevGrid efGrid_now, string[] col_name, string color_num)
        //        {//设置grid某几列的可写性,并修改字体颜色(绿色+粗体)  


        //            try
        //            {
        //                int i = 0;
        //                string v_item_ename = "";//列名称。
        //                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

        //                if (gridView_now == null) return;


        //                i = 1;

        //                //efGrid_now
        //                //使得整个GRID 不可编辑，除了复选框。
        //                //efGrid_now.SetAllColumnEditableWithoutSelection(false); 


        //                //初始化所有列为不可编辑,统一用 EFX 方法。 

        //                //设置[GRID]中所有列不编辑。
        //                foreach (EFX.EFCGridImp.EFCGridColumn col in EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.Values)
        //                {
        //                    col.EnableEdit = false;
        //                }


        //                for (i = 0; i < col_name.Length; i++)
        //                {//使得多列为可写状态
        //                    v_item_ename = col_name[i].ToString().Trim().ToUpper();

        //                    //若是GRID 中不存在的字段，则继续下一个字段循环
        //                    if (!gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(v_item_ename))) continue;
        //                    EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns[v_item_ename].EnableEdit = true;

        //                }

        //                //将可编辑列设置成[蓝色+粗体]标题
        //                //================================
        //                EFX.EFCGrid.GetEFCGridBase(efGrid_now).TitleEditEnableColor = Color.DodgerBlue;  //设置成蓝色。 
        //                //EFX.EFCGrid.GetEFCGridBase(efGrid_now).TitleEditEnableFont = new System.Drawing.Font(form_now.Font.FontFamily, form_now.Font.Size, System.Drawing.FontStyle.Bold);//字体变化。


        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, "DEV_SetColEdit_multi");
        //            }



        //        }

        //        //根据定义的数组信息，进行可编辑列设置。
        //        /// <summary>
        //        /// 根据定义的数组信息，进行可编辑列设置。[EFX]方式
        //        /// 设置grid某几列的可写性,并修改字体颜色(蓝色+粗体),
        //        /// </summary>
        //        /// <param name="form_now">当前FORM</param>
        //        /// <param name="efgrid">当前GRID</param>
        //        /// <param name="col_name">可编辑的列数组信息</param>
        //        /// <param name="color_num">颜色代码（此代码暂不启用）</param>
        //        public static void DEV_SetColEdit_multi(EF.EFForm form_now, EF.EFDevGrid efGrid_now, string[] col_name, string color_num)
        //        {//设置grid某几列的可写性,并修改字体颜色(绿色+粗体)


        //            /*
        //             * 框架提供的最新[列可编辑]控制语句。
        //             * on 2014-3-3 12:26:52 
        //             */

        //            ////设置制定列，可编辑。
        //            //EFX.EFCGrid.GetEFCGridBase(efDevGrid2).Columns["REC_CREATE_TIME"].EnableEdit = false;

        //            ////设置可编辑列的变色。
        //            //EFX.EFCGrid.GetEFCGridBase(efDevGrid1).TitleEditEnableColor = Color.Blue;


        //            try
        //            { 
        //                int i = 0;
        //                string v_item_ename = "";//列名称。
        //                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

        //                if (gridView_now == null) return;


        //                i = 1;

        //                //efGrid_now
        //                //使得整个GRID 不可编辑，除了复选框。
        //                //efGrid_now.SetAllColumnEditableWithoutSelection(false); 


        //                //初始化所有列为不可编辑,统一用 EFX 方法。 

        //                //设置[GRID]中所有列不编辑。
        //                foreach (EFX.EFCGridImp.EFCGridColumn col in EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.Values)
        //                {
        //                    col.EnableEdit = false;
        //                }


        //                for (i = 0; i < col_name.Length; i++)
        //                {//使得多列为可写状态
        //                    v_item_ename = col_name[i].ToString().Trim().ToUpper();

        //                    //若是GRID 中不存在的字段，则继续下一个字段循环
        //                    if (!gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(v_item_ename))) continue;
        //                    EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns[v_item_ename].EnableEdit = true;

        //                }

        //                //将可编辑列设置成[蓝色+粗体]标题
        //                //================================
        //                EFX.EFCGrid.GetEFCGridBase(efGrid_now).TitleEditEnableColor = Color.DodgerBlue;  //设置成蓝色。 
        //                //EFX.EFCGrid.GetEFCGridBase(efGrid_now).TitleEditEnableFont = new System.Drawing.Font(form_now.Font.FontFamily, form_now.Font.Size, System.Drawing.FontStyle.Bold);//字体变化。


        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, "DEV_SetColEdit_multi");
        //            }


        //            //try
        //            //{

        //            //    int i = 0;
        //            //    int rows = 0;
        //            //    int cols = 0;

        //            //    string item_ename = "";
        //            //    DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

        //            //    if (gridView_now == null) return;
        //            //    i = 1;
        //            //    //使得整个GRID 可编辑
        //            //    gridView_now.OptionsBehavior.Editable = true;



        //            //    //初始化所有列的标题行信息：字体颜色,黑色
        //            //    for (cols = 0; cols < gridView_now.Columns.Count; cols++)
        //            //    {//列信息
        //            //        //修改颜色= 黑色
        //            //        gridView_now.Columns[cols].AppearanceHeader.ForeColor = Color.Black;
        //            //        //修改字体,非粗体
        //            //        gridView_now.Columns[cols].AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular);
        //            //        //各列不可写。
        //            //        //gridView_now.Columns[cols].OptionsColumn.AllowEdit = false;
        //            //        gridView_now.Columns[cols].OptionsColumn.ReadOnly = true;  //将各列设置成只读。 

        //            //    } 

        //            //    for (i = 0; i < col_name.Length; i++)
        //            //    {//使得多列为可写状态
        //            //        item_ename = col_name[i].ToString().Trim().ToUpper();


        //            //        //若是GRID 中不存在的字段，则继续下一个字段循环
        //            //        if (!gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(item_ename))) continue;


        //            //        //efgrid.Cols[item_ename].AllowEditing = true;
        //            //        //gridView_now.Columns.ColumnByFieldName(item_ename).OptionsColumn.AllowEdit = true; //指定列可编辑。
        //            //        gridView_now.Columns.ColumnByFieldName(item_ename).OptionsColumn.ReadOnly = false; //将指定列设置成非只读。
        //            //        gridView_now.Columns.ColumnByFieldName(item_ename).AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
        //            //        gridView_now.Columns.ColumnByFieldName(item_ename).AppearanceHeader.ForeColor = Color.DodgerBlue;

        //            //    }

        //            //    //复选框，永远可写。。。。。
        //            //    //item_ename = "selected";
        //            //    //EF.EFDevGrid.SelectionColumnFieldName
        //            //    item_ename = EF.EFDevGrid.SelectionColumnFieldName;
        //            //    if (gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(item_ename)))
        //            //    {
        //            //        gridView_now.Columns.ColumnByFieldName(item_ename).OptionsColumn.AllowEdit = true; //指定列可编辑。
        //            //        gridView_now.Columns.ColumnByFieldName(item_ename).OptionsColumn.ReadOnly = false;

        //            //        gridView_now.Columns.ColumnByFieldName(item_ename).AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
        //            //        gridView_now.Columns.ColumnByFieldName(item_ename).AppearanceHeader.ForeColor = Color.DodgerBlue; //Color.Green; //DodgerBlue
        //            //    }

        //            //}
        //            //catch (Exception ex)
        //            //{
        //            //    Console.WriteLine("DEV_SetColEdit_multi2" + ex.ToString());
        //            //}


        //        }


        //        /// <summary>
        //        /// 设置所有列不可编辑。除了复选框=2.0版
        //        /// </summary>
        //        /// <param name="form_now"></param>
        //        /// <param name="efGrid_now"></param> 
        //        public static void DEV_SetColEdit_allNot2(EF.EFDevGrid efGrid_now)
        //        {//设置所有列不可编辑。除了复选框。

        //            try
        //            {

        //                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

        //                if (gridView_now == null) return;
        //                if (EFX.EFCGrid.GetEFCGridBase(efGrid_now) == null) return;

        //                //初始化所有列为不可编辑,统一用 EFX 方法。  
        //                //设置[GRID]中所有列不编辑。
        //                foreach (EFX.EFCGridImp.EFCGridColumn col in EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.Values)
        //                {

        //                    if (col.ColumnInfo.ItemHideFlag == EFX.EFCGridImp.EFCGridColumnInfo.EnumItemHideFlag.NONE)
        //                    {
        //                        //若当前列没有被隐藏了，设置EnableEdit 属性。
        //                        col.EnableEdit = false;
        //                    }
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, "DEV_SetColEdit_allNot2");
        //            }

        //        }




        //        /// <summary>
        //        /// 设置所有列不可编辑。除了复选框。
        //        /// </summary>
        //        /// <param name="form_now"></param>
        //        /// <param name="efGrid_now"></param> 
        //        public static void DEV_SetColEdit_allNot(EF.EFForm form_now, EF.EFDevGrid efGrid_now)
        //        {//设置所有列不可编辑。除了复选框。


        //            /*
        //             * 框架提供的最新[列可编辑]控制语句。
        //             * on 2014-3-3 12:26:52 
        //             */

        //            ////设置制定列，可编辑。
        //            //EFX.EFCGrid.GetEFCGridBase(efDevGrid2).Columns["REC_CREATE_TIME"].EnableEdit = false;

        //            ////设置可编辑列的变色。
        //            //EFX.EFCGrid.GetEFCGridBase(efDevGrid1).TitleEditEnableColor = Color.Blue;


        //            try
        //            {

        //                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

        //                if (gridView_now == null) return;
        //                if (EFX.EFCGrid.GetEFCGridBase(efGrid_now) == null) return;

        //                //初始化所有列为不可编辑,统一用 EFX 方法。  
        //                //设置[GRID]中所有列不编辑。
        //                foreach (EFX.EFCGridImp.EFCGridColumn col in EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.Values)
        //                {
        //                   // col.EnableEdit = false;

        //                    if (col.ColumnInfo.ItemHideFlag == EFX.EFCGridImp.EFCGridColumnInfo.EnumItemHideFlag.NONE)
        //                    {
        //                        //若当前列没有被隐藏了，设置EnableEdit 属性。
        //                        col.EnableEdit = false;
        //                    } 
        //                } 

        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, "DEV_SetColEdit_allNot");
        //            }

        //        }


        //        /// <summary>
        //        /// 设置所有列【可编辑】。 
        //        /// </summary>
        //        /// <param name="form_now"></param>
        //        /// <param name="efGrid_now"></param>
        //        public static void DEV_SetColEdit_all(EF.EFForm form_now, EF.EFDevGrid efGrid_now)
        //        {//设置所有列【可编辑】。 

        //            try
        //            {

        //                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView;

        //                if (gridView_now == null) return;
        //                if (EFX.EFCGrid.GetEFCGridBase(efGrid_now) == null) return;

        //                //初始化所有列为【可编辑】,统一用 EFX 方法。  
        //                //设置[GRID]中所有列【可编辑】。
        //                foreach (EFX.EFCGridImp.EFCGridColumn col in EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.Values)
        //                {

        //                    if (col.ColumnInfo.ItemHideFlag == EFX.EFCGridImp.EFCGridColumnInfo.EnumItemHideFlag.NONE)
        //                    {
        //                        //若当前列没有被隐藏了，设置EnableEdit 属性。
        //                        col.EnableEdit = true;
        //                    }
        //                }


        //                //将可编辑列设置成[蓝色+粗体]标题
        //                //================================
        //                EFX.EFCGrid.GetEFCGridBase(efGrid_now).TitleEditEnableColor = Color.DodgerBlue;  //设置成蓝色。 
        //                //EFX.EFCGrid.GetEFCGridBase(efGrid_now).TitleEditEnableFont = new System.Drawing.Font(form_now.Font.FontFamily, form_now.Font.Size, System.Drawing.FontStyle.Bold);//字体变化。






        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, "DEV_SetColEdit_all");
        //            }

        //        }









        //        //根据EPED54画面的配置信息,动态设置可编辑性。
        //        /// <summary>
        //        /// 根据EPED54画面的配置信息,动态设置可编辑性。[EFX]方式
        //        /// 设置grid某几列的可写性,并修改字体颜色(蓝色+粗体),
        //        /// </summary>
        //        /// <param name="form_now">当前FORM</param>
        //        /// <param name="efGrid_now">当前GRID</param>
        //        /// <param name="v_function_id">功能号</param>
        //        /// <param name="color_num">颜色代码（此代码暂不启用）</param>
        //        public static void DEV_SetColEdit_multi2(EF.EFForm form_now, EF.EFDevGrid efGrid_now, string v_function_id, string color_num)
        //        {//设置grid某几列的可写性,并修改字体颜色(绿色+粗体efGrid_now 
        //            try
        //            {
        //                int i = 0;
        //                int rows = 0;
        //                int cols = 0;

        //                string v_item_ename = "";

        //                DevExpress.XtraGrid.Views.Grid.GridView gridView_now = efGrid_now.MainView as DevExpress.XtraGrid.Views.Grid.GridView; 
        //                if (gridView_now == null) return;



        //                EI.EIInfo inBlock = new EI.EIInfo();
        //                EI.EIInfo outBlock; 


        //                //设置[GRID]中所有列不编辑。
        //                foreach (EFX.EFCGridImp.EFCGridColumn col in EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.Values)
        //                {
        //                    col.EnableEdit = false;
        //                } 


        //                //查询功能号
        //                i = 1;

        //                //采用框架提供的动态SQL 的service 。
        //                //==================
        //                string v_sql = "";// 工序代码表 = tsi0001, 
        //                v_sql = " SELECT item_ename FROM ted54 "
        //                      + " WHERE  UPPER(func_id) = UPPER('" + v_function_id + "') " //根据指定的功能号。
        //                      + " AND    form_edit_flag = '1'              "             //前台可编辑标记= 1,说明允许编辑
        //                      + " ORDER  BY class_code ,seq_no             ";

        //                inBlock.SetColName(1, i++, "v_sql");
        //                inBlock.SetColVal(1, 1, "v_sql", v_sql);// 
        //                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"eped_dyn_sql", inBlock); 




        //                for (i = 0; i < outBlock.blk_info[0].row; i++)
        //                {//使得多列为可写状态
        //                    v_item_ename = outBlock.Tables[0].Rows[i]["item_ename"].ToString().Trim().ToUpper();

        //                    //若是GRID 中不存在的字段，则继续下一个字段循环
        //                    if (!gridView_now.Columns.Contains(gridView_now.Columns.ColumnByFieldName(v_item_ename))) continue;
        //                    EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns[v_item_ename].EnableEdit = true; //可编辑。 

        //                }


        //                //将可编辑列设置成[蓝色+粗体]标题
        //                //================================
        //                EFX.EFCGrid.GetEFCGridBase(efGrid_now).TitleEditEnableColor = Color.DodgerBlue;  //设置成蓝色。 
        //                //EFX.EFCGrid.GetEFCGridBase(efGrid_now).TitleEditEnableFont = new System.Drawing.Font(form_now.Font.FontFamily, form_now.Font.Size, System.Drawing.FontStyle.Bold);//字体变化。

        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, "DEV_SetColEdit_multi2");
        //            }




        //        }












        //        //设置[单记录]模式的[GRID]某几item的可写性,并修改字体颜色(绿色+粗体),
        //        //根据EPED54画面的配置信息,动态设置可编辑性。
        //        /// <summary>
        //        /// 根据EPED54画面的配置信息,动态设置可编辑性。
        //        ///设置[单记录]模式的[GRID]某几item的可写性,并修改字体颜色(绿色+粗体),
        //        /// </summary>
        //        /// <param name="efgrid">DEVGRID控件</param>
        //        /// <param name="v_function_id">根据EPED54画面中配置的可编辑列信息</param>
        //        /// <param name="color_num">变色代码（此代码暂不启用）</param>
        //        public static void DEV_SetColEdit_multi3(EF.EFDevGrid efGrid_now, string v_function_id, string color_num)
        //        {//设置[单记录]模式的[GRID]某几item的可写性
        //            int i = 0;
        //            int rows = 0;
        //            int cols = 0;
        //            string v_item_ename = "";

        //            //初始化ComboBox中的数据
        //            EI.EIInfo inBlock = new EI.EIInfo();
        //            EI.EIInfo outBlock;

        //            try
        //            {



        //                //设置[GRID]中所有列不编辑。
        //                foreach (EFX.EFCGridImp.EFCGridColumn col in EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.Values)
        //                {
        //                    //col.EnableEdit = false;

        //                    if (col.ColumnInfo.ItemHideFlag == EFX.EFCGridImp.EFCGridColumnInfo.EnumItemHideFlag.NONE)
        //                    {
        //                        //若当前列没有被隐藏了，设置EnableEdit 属性。
        //                        col.EnableEdit = false;
        //                    } 
        //                }






        //                //采用框架提供的动态SQL 的service 。
        //                //==================
        //                string v_sql = "";// 工序代码表 = tsi0001, 
        //                v_sql = " SELECT item_ename FROM ted54 "
        //                      + " WHERE  UPPER(func_id) = UPPER('" + v_function_id + "') " //根据指定的功能号。
        //                      + " AND    form_edit_flag = '1'              "             //前台可编辑标记= 1,说明允许编辑
        //                      + " ORDER  BY class_code ,seq_no             ";

        //                i = 1;
        //                inBlock.SetColName(1, i++, "v_sql");
        //                inBlock.SetColVal(1, 1, "v_sql", v_sql);// 
        //                outBlock = EI.EIManager.Instance.CallService(cs_formPartition,"eped_dyn_sql", inBlock); 


        //                //1#BLK, 返回的是指定功能号下可编辑的列信息。
        //                for (i = 0; i < outBlock.blk_info[0].row; i++)
        //                {//1#BLK, 返回的是指定功能号下可编辑的列信息。
        //                    v_item_ename = outBlock.Tables[0].Rows[i]["item_ename"].ToString().Trim().ToUpper();


        //                    //EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns[v_item_ename].EnableEdit = true;


        //                    if (!EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.ContainsKey(v_item_ename))
        //                    {//若指定列不存在，则直接下一个。
        //                        continue;
        //                    }


        //                    EFX.EFCGridImp.EFCGridColumn col = EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns[v_item_ename];
        //                    if (col.ColumnInfo.ItemHideFlag == EFX.EFCGridImp.EFCGridColumnInfo.EnumItemHideFlag.NONE)
        //                    {
        //                        //若当前列没有被隐藏了，设置EnableEdit 属性。
        //                        col.EnableEdit = true;
        //                    } 


        //                }


        //                //将可编辑列设置成[蓝色+粗体]标题
        //                //================================
        //                EFX.EFCGrid.GetEFCGridBase(efGrid_now).TitleEditEnableColor = Color.DodgerBlue;  //设置成蓝色。 
        //                //EFX.EFCGrid.GetEFCGridBase(efGrid_now).TitleEditEnableFont = new System.Drawing.Font(form_now.Font.FontFamily, form_now.Font.Size, System.Drawing.FontStyle.Bold);//字体变化。




        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, "error");
        //            }



        //        }


        //        //设置[单记录]模式
        //        /// <summary>
        //        /// 根据数组变量[],设置[单记录]模式的[GRID]某几item的可写性。
        //        /// </summary>
        //        /// <param name="efGrid_now"></param>
        //        /// <param name="col_name"></param>
        //        /// <param name="color_num"></param>
        //        public static void DEV_SetColEdit_multi3(EF.EFDevGrid efGrid_now, string[] col_name, string color_num)
        //        {//设置[单记录]模式的[GRID]某几item的可写性
        //            int i = 0; 
        //            string v_item_ename = "";




        //            try
        //            {

        //                //查询功能号
        //                i = 1;


        //                //设置[GRID]中所有列不编辑。
        //                foreach (EFX.EFCGridImp.EFCGridColumn col in EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.Values)
        //                {
        //                    if (col.ColumnInfo.ItemHideFlag == EFX.EFCGridImp.EFCGridColumnInfo.EnumItemHideFlag.NONE)
        //                    {
        //                     //若当前列没有被隐藏了，设置EnableEdit 属性。
        //                        col.EnableEdit = false; 
        //                    } 
        //                }


        //                for (i = 0; i < col_name.Length; i++)
        //                {//使得多列为可写状态
        //                    v_item_ename = col_name[i].ToString().Trim().ToUpper();



        //                    if (!EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns.ContainsKey(v_item_ename))
        //                    {//若指定列不存在，则直接下一个。
        //                        continue;
        //                    }


        //                    EFX.EFCGridImp.EFCGridColumn col = EFX.EFCGrid.GetEFCGridBase(efGrid_now).Columns[v_item_ename];
        //                    if (col.ColumnInfo.ItemHideFlag == EFX.EFCGridImp.EFCGridColumnInfo.EnumItemHideFlag.NONE)
        //                    {
        //                        //若当前列没有被隐藏了，设置EnableEdit 属性。
        //                        col.EnableEdit = true;
        //                    }  

        //                }








        //            }
        //            catch (Exception ex)
        //            {
        //                MessageBox.Show(ex.Message, "error");
        //            }



        //        }





        //    }
    }
}
