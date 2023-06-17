using System.Runtime.CompilerServices;

namespace WpfAppTopRep.Report
{
    partial class ReportDailyExpensesForms
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
            this.dailyExpensesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.topRepDBDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.topRepDBDataSet = new WpfAppTopRep.topRepDBDataSet();
            this.dailyExpensesTableAdapter = new WpfAppTopRep.topRepDBDataSetTableAdapters.DailyExpensesTableAdapter();
            this.rptViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.dailyExpensesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.topRepDBDataSetBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.topRepDBDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // dailyExpensesBindingSource
            // 
            this.dailyExpensesBindingSource.DataMember = "DailyExpenses";
            this.dailyExpensesBindingSource.DataSource = this.topRepDBDataSetBindingSource;
            // 
            // topRepDBDataSetBindingSource
            // 
            this.topRepDBDataSetBindingSource.DataSource = this.topRepDBDataSet;
            this.topRepDBDataSetBindingSource.Position = 0;
            // 
            // topRepDBDataSet
            // 
            this.topRepDBDataSet.DataSetName = "topRepDBDataSet";
            this.topRepDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dailyExpensesTableAdapter
            // 
            this.dailyExpensesTableAdapter.ClearBeforeFill = true;
            // 
            // rptViewer1
            // 
            this.rptViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rptViewer1.LocalReport.ReportEmbeddedResource = "WpfAppTopRep.Report.ReportDailyExpensesRdlc.rdlc";
            this.rptViewer1.Location = new System.Drawing.Point(0, 0);
            this.rptViewer1.Name = "rptViewer1";
            this.rptViewer1.ServerReport.BearerToken = null;
            this.rptViewer1.Size = new System.Drawing.Size(651, 406);
            this.rptViewer1.TabIndex = 0;
            this.rptViewer1.Load += new System.EventHandler(this.rptViewer1_Load);
            // 
            // ReportDailyExpensesForms
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 406);
            this.Controls.Add(this.rptViewer1);
            this.Name = "ReportDailyExpensesForms";
            this.Text = "ReportDailyExpenses";
            this.Load += new System.EventHandler(this.ReportDailyExpenses_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dailyExpensesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.topRepDBDataSetBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.topRepDBDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource topRepDBDataSetBindingSource;
        private topRepDBDataSet topRepDBDataSet;
        private System.Windows.Forms.BindingSource dailyExpensesBindingSource;
        private topRepDBDataSetTableAdapters.DailyExpensesTableAdapter dailyExpensesTableAdapter;
        private Microsoft.Reporting.WinForms.ReportViewer rptViewer1;
    }
}