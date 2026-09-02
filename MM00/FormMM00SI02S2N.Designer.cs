namespace MM
{
    partial class FormMM00SI02S2N
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.efDevGrid_QUERY = new EF.EFDevGrid();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.efDevGrid_INQ = new EF.EFDevGrid();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.gridView_QUERY = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView_INQ = new DevExpress.XtraGrid.Views.Grid.GridView();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.efDevGrid_QUERY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.efDevGrid_INQ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_QUERY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_INQ)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.layoutControl1.Controls.Add(this.efDevGrid_INQ);
            this.layoutControl1.Controls.Add(this.efDevGrid_QUERY);
            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.Root = this.layoutControlGroup1;
            this.layoutControl1.Size = new System.Drawing.Size(984, 516);
            this.layoutControl1.TabIndex = 4;
            this.layoutControl1.Text = "layoutControl1";
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "layoutControlGroup1";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.layoutControlGroup3});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup1.Size = new System.Drawing.Size(984, 516);
            this.layoutControlGroup1.Text = "layoutControlGroup1";
            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.CustomizationFormText = "查询条件";
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup2.Size = new System.Drawing.Size(984, 184);
            this.layoutControlGroup2.Text = "查询条件";
            // 
            // layoutControlGroup3
            // 
            this.layoutControlGroup3.CustomizationFormText = "详细信息";
            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
            this.layoutControlGroup3.Location = new System.Drawing.Point(0, 184);
            this.layoutControlGroup3.Name = "layoutControlGroup3";
            this.layoutControlGroup3.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
            this.layoutControlGroup3.Size = new System.Drawing.Size(984, 332);
            this.layoutControlGroup3.Text = "详细信息";
            // 
            // efDevGrid_QUERY
            // 
            this.efDevGrid_QUERY.Location = new System.Drawing.Point(5, 25);
            this.efDevGrid_QUERY.MainView = this.gridView_QUERY;
            this.efDevGrid_QUERY.Name = "efDevGrid_QUERY";
            this.efDevGrid_QUERY.Size = new System.Drawing.Size(974, 154);
            this.efDevGrid_QUERY.TabIndex = 4;
            this.efDevGrid_QUERY.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView_QUERY});
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.efDevGrid_QUERY;
            this.layoutControlItem1.CustomizationFormText = "layoutControlItem1";
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(978, 158);
            this.layoutControlItem1.Text = "layoutControlItem1";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextToControlDistance = 0;
            this.layoutControlItem1.TextVisible = false;
            // 
            // efDevGrid_INQ
            // 
            this.efDevGrid_INQ.IsUseCustomPageBar = true;
            this.efDevGrid_INQ.Location = new System.Drawing.Point(5, 209);
            this.efDevGrid_INQ.MainView = this.gridView_INQ;
            this.efDevGrid_INQ.Name = "efDevGrid_INQ";
            this.efDevGrid_INQ.Size = new System.Drawing.Size(974, 302);
            this.efDevGrid_INQ.TabIndex = 5;
            this.efDevGrid_INQ.UseEmbeddedNavigator = true;
            this.efDevGrid_INQ.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView_INQ});
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.efDevGrid_INQ;
            this.layoutControlItem2.CustomizationFormText = "layoutControlItem2";
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(978, 306);
            this.layoutControlItem2.Text = "layoutControlItem2";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextToControlDistance = 0;
            this.layoutControlItem2.TextVisible = false;
            // 
            // gridView_QUERY
            // 
            this.gridView_QUERY.FixedLineWidth = 1;
            this.gridView_QUERY.GridControl = this.efDevGrid_QUERY;
            this.gridView_QUERY.IndicatorWidth = 35;
            this.gridView_QUERY.Name = "gridView_QUERY";
            this.gridView_QUERY.OptionsView.ColumnAutoWidth = false;
            this.gridView_QUERY.OptionsView.EnableAppearanceEvenRow = true;
            this.gridView_QUERY.OptionsView.EnableAppearanceOddRow = true;
            this.gridView_QUERY.OptionsView.ShowGroupPanel = false;
            // 
            // gridView_INQ
            // 
            this.gridView_INQ.FixedLineWidth = 1;
            this.gridView_INQ.GridControl = this.efDevGrid_INQ;
            this.gridView_INQ.IndicatorWidth = 35;
            this.gridView_INQ.Name = "gridView_INQ";
            this.gridView_INQ.OptionsView.ColumnAutoWidth = false;
            this.gridView_INQ.OptionsView.EnableAppearanceEvenRow = true;
            this.gridView_INQ.OptionsView.EnableAppearanceOddRow = true;
            this.gridView_INQ.OptionsView.ShowGroupPanel = false;
            // 
            // FormMM00SI02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 562);
            this.Controls.Add(this.layoutControl1);
            this.Name = "FormMM00SI02";
            this.Text = "物料通用跨系统对账信息管理";
            this.EF_PRE_DO_F6 += new EF.EFButtonBar.EFDoFnEventHandler(this.FormMM00SI02_EF_PRE_DO_F6);
            this.EF_DO_F2 += new EF.EFButtonBar.EFDoFnEventHandler(this.FormMM00SI02_EF_DO_F2);
            this.EF_DO_F6 += new EF.EFButtonBar.EFDoFnEventHandler(this.FormMM00SI02_EF_DO_F6);
            this.EF_CANCEL_DO_F6 += new EF.EFButtonBar.EFDoFnEventHandler(this.FormMM00SI02_EF_CANCEL_DO_F6);
            this.Load += new System.EventHandler(this.FormMM00SI02_Load);
            this.Controls.SetChildIndex(this.layoutControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.efDevGrid_QUERY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.efDevGrid_INQ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_QUERY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView_INQ)).EndInit();
            this.ResumeLayout(false);

        }
        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private EF.EFDevGrid efDevGrid_INQ;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_INQ;
        private EF.EFDevGrid efDevGrid_QUERY;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView_QUERY;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
    }
}