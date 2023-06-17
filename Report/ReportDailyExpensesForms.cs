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
    public partial class ReportDailyExpensesForms : Form
    {
        //Holds connetion to DB
        SqlConnection sqlConnection;

        public ReportDailyExpensesForms()
        {
            InitializeComponent();

            //String connetion from database
            string connectionString = ConfigurationManager.ConnectionStrings["WpfAppTopRep.Properties.Settings.topRepDBConnectionString"].ConnectionString;

            //String connetion from database
            sqlConnection = new SqlConnection(connectionString);

            // Call this method to display the report
            ShowReport();
        }

        private void ReportDailyExpenses_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'topRepDBDataSet.DailyExpenses' table. You can move, or remove it, as needed.
            this.dailyExpensesTableAdapter.Fill(this.topRepDBDataSet.DailyExpenses);

            this.rptViewer1.RefreshReport();
            this.rptViewer1.RefreshReport();
        }

        private void rptViewer1_Load(object sender, EventArgs e)
        {
            
        }

        private void ShowReport()
        {
            DataTable dataTable = new DataTable();

            using (sqlConnection)
            {
                // Call this method to display the report
                SqlCommand sqlCommand = new SqlCommand("GetStoreIDDailyExpensesUniqueIDUpd", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                // Fill the store id from where clause from statement
                sqlCommand.Parameters.Add("@StoreID", SqlDbType.Int).Value = DailyExpensesReport.reportExpensesStoreID;

                // Fill the unique id from where clause from statement
                sqlCommand.Parameters.Add("@DailyExpensesUniqueID", SqlDbType.Int).Value = DailyExpensesReport.reportExpensesUniqueID;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                sqlDataAdapter.Fill(dataTable);
                
                ReportDataSource rds = new ReportDataSource("DataSetDE", dataTable);

                rptViewer1.LocalReport.DataSources.Add(rds);
                rptViewer1.LocalReport.ReportPath = "../../Report/ReportDailyExpensesRdlc.rdlc";
                
                // Set the data parameter from report data
                ReportParameter rp1 = new ReportParameter("pStoreName", DailyExpensesReport.storeName);
                rptViewer1.LocalReport.SetParameters(rp1);

                // Set the data parameter from report data
                ReportParameter rp2 = new ReportParameter("pDateSubmitted", DailyExpensesReport.dateSubmitted);
                rptViewer1.LocalReport.SetParameters(rp2);

                // Set the data parameter from report data
                ReportParameter rp3 = new ReportParameter("pTotalAmount", DailyExpensesReport.totalAmountFromLabel);
                rptViewer1.LocalReport.SetParameters(rp3);

                rptViewer1.LocalReport.Refresh();
            }
        }
    }
}
