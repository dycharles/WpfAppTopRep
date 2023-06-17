namespace WpfAppTopRep.Report
{
    partial class ReportDailyIncomeForms
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
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.dailyIncomeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.topRepDBDataSet = new WpfAppTopRep.topRepDBDataSet();
            this.rptViewerDailyIncome = new Microsoft.Reporting.WinForms.ReportViewer();
            this.dailyIncomeTableAdapter = new WpfAppTopRep.topRepDBDataSetTableAdapters.DailyIncomeTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.dailyIncomeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.topRepDBDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dailyIncomeBindingSource
            // 
            this.dailyIncomeBindingSource.DataMember = "DailyIncome";
            this.dailyIncomeBindingSource.DataSource = this.topRepDBDataSet;
            // 
            // topRepDBDataSet
            // 
            this.topRepDBDataSet.DataSetName = "topRepDBDataSet";
            this.topRepDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // rptViewerDailyIncome
            // 
            this.rptViewerDailyIncome.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSetDailyExpenses";
            reportDataSource1.Value = this.dailyIncomeBindingSource;
            this.rptViewerDailyIncome.LocalReport.DataSources.Add(reportDataSource1);
            this.rptViewerDailyIncome.LocalReport.ReportEmbeddedResource = "WpfAppTopRep.Report.ReportDailyIncomeRdlc.rdlc";
            this.rptViewerDailyIncome.Location = new System.Drawing.Point(0, 0);
            this.rptViewerDailyIncome.Name = "rptViewerDailyIncome";
            this.rptViewerDailyIncome.ServerReport.BearerToken = null;
            this.rptViewerDailyIncome.Size = new System.Drawing.Size(666, 418);
            this.rptViewerDailyIncome.TabIndex = 0;
            this.rptViewerDailyIncome.Load += new System.EventHandler(this.reportViewer1_Load);
            // 
            // dailyIncomeTableAdapter
            // 
            this.dailyIncomeTableAdapter.ClearBeforeFill = true;
            // 
            // ReportDailyIncomeForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 418);
            this.Controls.Add(this.rptViewerDailyIncome);
            this.Name = "ReportDailyIncomeForms";
            this.Text = "ReportDailyIncome";
            this.Load += new System.EventHandler(this.ReportDailyIncomeForms_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dailyIncomeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.topRepDBDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer rptViewerDailyIncome;
        private topRepDBDataSet topRepDBDataSet;
        private System.Windows.Forms.BindingSource dailyIncomeBindingSource;
        private topRepDBDataSetTableAdapters.DailyIncomeTableAdapter dailyIncomeTableAdapter;
    }
}