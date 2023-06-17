using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfAppTopRep.Report
{
    public partial class ReportDailyIncomeForms : Form
    {
        //Holds connetion to DB
        SqlConnection sqlConnection;

        public ReportDailyIncomeForms()
        {
            InitializeComponent();

            //String connetion from database
            string connectionString = ConfigurationManager.ConnectionStrings["WpfAppTopRep.Properties.Settings.topRepDBConnectionString"].ConnectionString;

            //String connetion from database
            sqlConnection = new SqlConnection(connectionString);

            // Call this method to display the report
            ShowReport();
        }

        private void ReportDailyIncomeForms_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'topRepDBDataSet.DailyIncome' table. You can move, or remove it, as needed.
            this.dailyIncomeTableAdapter.Fill(this.topRepDBDataSet.DailyIncome);

            this.rptViewerDailyIncome.RefreshReport();
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void ShowReport()
        {
            DataTable dataTable = new DataTable();

            using (sqlConnection)
            {
                // Call this method to display the report
                SqlCommand sqlCommand = new SqlCommand("GetStoreIDDailyIncomeUniqueIDUpd", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Fill the store id from where clause from statement
                sqlCommand.Parameters.Add("@StoreID", SqlDbType.Int).Value = DailyIncomeReport.reportIncomeStoreID;

                // Fill the unique id from where clause from statement
                sqlCommand.Parameters.Add("@DailyIncomeUniqueID", SqlDbType.Int).Value = DailyIncomeReport.reportIncomeUniqueID;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                sqlDataAdapter.Fill(dataTable);

                ReportDataSource rds = new ReportDataSource("DataSetDI", dataTable);

                rptViewerDailyIncome.LocalReport.DataSources.Add(rds);
                rptViewerDailyIncome.LocalReport.ReportPath = "../../Report/ReportDailyIncomeRdlc.rdlc";

                // Set the data parameter from report data
                ReportParameter rp1 = new ReportParameter("pStoreName", DailyIncomeReport.storeName);
                rptViewerDailyIncome.LocalReport.SetParameters(rp1);

                // Set the data parameter from report data
                ReportParameter rp2 = new ReportParameter("pDateSubmitted", DailyIncomeReport.dateSubmitted);
                rptViewerDailyIncome.LocalReport.SetParameters(rp2);

                // Set the data parameter from report data
                ReportParameter rp3 = new ReportParameter("pTotalAmount", DailyIncomeReport.totalAmountFromLabel);
                rptViewerDailyIncome.LocalReport.SetParameters(rp3);

                rptViewerDailyIncome.LocalReport.Refresh();
            }
        }
    }
}
