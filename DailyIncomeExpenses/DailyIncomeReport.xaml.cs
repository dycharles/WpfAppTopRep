using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfAppTopRep.Report;

namespace WpfAppTopRep
{
    /// <summary>
    /// Interaction logic for DailyIncomeReport.xaml
    /// </summary>
    public partial class DailyIncomeReport : Window
    {
        //Holds connetion to DB
        SqlConnection sqlConnection;

        // Get store id to display in the reportviewer
        public static int reportIncomeStoreID;

        // Get unique id to display in the reportviewer
        public static int reportIncomeUniqueID;

        // Get store name to display in the reportviewer
        public static string storeName;

        // Get date submitted to display in the reportviewer
        public static string dateSubmitted;

        // Get total amount to display in the reportviewer
        public static string totalAmountFromLabel;

        public DailyIncomeReport()
        {
            InitializeComponent();

            //String connetion from database
            string connectionString = ConfigurationManager.ConnectionStrings["WpfAppTopRep.Properties.Settings.topRepDBConnectionString"].ConnectionString;

            // Initialize sql connection
            sqlConnection = new SqlConnection(connectionString);

            // Call this method to display the store name in the store listbox
            DisplayStoreList();
        }
        private void DisplayStoreList()
        {
            try
            {
                // Run query, connect to database and fill the store list
                string query = "SELECT * FROM Store";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable usernameTable = new DataTable();
                    sqlDataAdapter.Fill(usernameTable);
                    
                    // Define the column we are displaying in listbox
                    storeList.DisplayMemberPath = "Name";

                    // Define what is unique about each item in the list
                    storeList.SelectedValuePath = "Id";

                    // The content we will use to populate the list
                    storeList.ItemsSource = usernameTable.DefaultView;
                }

            }
            catch (Exception ex)
            {
                // Did not catch all the exception because it is a waste of time, be back here someday
                MessageBox.Show(ex.ToString());
            }
        }

        private void DisplayToDateList()
        {
            try
            {
                // Run query, connect to database and fill the date time list
                string query = "SELECT d.DateTimeSubmitted, d.DailyIncomeUniqueID FROM DailyIncome d WHERE d.StoreID = " + storeList.SelectedValue + " GROUP BY d.DailyIncomeUniqueID, d.DateTimeSubmitted";

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable usernameTable = new DataTable();
                    sqlDataAdapter.Fill(usernameTable);

                    // Define the column we are displaying in listbox
                    dateList.DisplayMemberPath = "DateTimeSubmitted";

                    // Define what is unique about each item in the list
                    dateList.SelectedValuePath = "DailyIncomeUniqueID";

                    // The content we will use to populate the list
                    dateList.ItemsSource = usernameTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                // Did not catch all the exception because it is a waste of time, be back here someday
                MessageBox.Show(ex.ToString());
            }
        }

        private void DisplayToDescriptionDataGrid()
        {
            try
            {
                // Run query, connect to database
                string query = "SELECT dc.Name AS 'Description' ,Quantity, FORMAT(di.Amount, 'N', 'en-us') AS Amount,  FORMAT((di.Quantity*di.Amount) , 'N', 'en-us')AS Total, di.Comment AS'Comment' FROM DailyIncome di INNER JOIN Description dc ON di.IdDescription = dc.Id WHERE di.StoreID = " + storeList.SelectedValue + " AND di.DailyIncomeUniqueID = " + dateList.SelectedValue;

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable usernameTable = new DataTable();
                    sqlDataAdapter.Fill(usernameTable);

                    // Define the column we are displaying in listbox
                    descriptionDataGrid.DisplayMemberPath = "Amount";

                    // Define what is unique about each item in the list
                    descriptionDataGrid.SelectedValuePath = "Id";

                    // The content we will use to populate the list
                    descriptionDataGrid.ItemsSource = usernameTable.DefaultView;
                }

                // Run query, connect to database and fill the description list
                string queryTotalAmount = "SELECT SUM(d.Quantity * d.Amount) FROM DailyIncome d WHERE d.StoreID = " + storeList.SelectedValue + " AND DailyIncomeUniqueID = " + dateList.SelectedValue;

                SqlDataAdapter sqlDataAdapter3 = new SqlDataAdapter(queryTotalAmount, sqlConnection);
                SqlCommand sqlCommand2 = new SqlCommand(queryTotalAmount, sqlConnection);

                using (sqlDataAdapter3)
                {
                    DataTable descriptionTable1 = new DataTable();
                    sqlDataAdapter3.Fill(descriptionTable1);

                    sqlConnection.Open();
                    SqlDataReader reader2 = sqlCommand2.ExecuteReader();
                    while (reader2.Read())
                    {
                        // Set the value of total amount label for report viewer
                        totalAmountFromLabel = String.Format("{0:0,0.00}", Convert.ToDouble(reader2.GetValue(0).ToString()));

                        // Set the value of total amount label
                        totalAmountLabel.Content = totalAmountFromLabel;

                    }

                    sqlConnection.Close();
                }

                // Run query, connect to database, open database then close database connection
                string queryStoreName = "SELECT Name FROM Store d WHERE d.Id = " + storeList.SelectedValue;

                SqlDataAdapter sqlDataAdapter34 = new SqlDataAdapter(queryStoreName, sqlConnection);
                SqlCommand sqlCommand23 = new SqlCommand(queryStoreName, sqlConnection);

                using (sqlDataAdapter34)
                {
                    DataTable descriptionTable1 = new DataTable();
                    sqlDataAdapter34.Fill(descriptionTable1);

                    sqlConnection.Open();
                    SqlDataReader reader2 = sqlCommand23.ExecuteReader();
                    while (reader2.Read())
                    {
                        // Get the store name from the database
                        storeName = reader2.GetValue(0).ToString();
                    }

                    sqlConnection.Close();
                }

                // Run query, connect to database, open database then close database connection
                string queryDateSubmitted = "SELECT DISTINCT d.DateTimeSubmitted FROM DailyIncome d WHERE d.DailyIncomeUniqueID = " + dateList.SelectedValue;

                SqlDataAdapter sqlDataAdapter344 = new SqlDataAdapter(queryStoreName, sqlConnection);
                SqlCommand sqlCommand234 = new SqlCommand(queryDateSubmitted, sqlConnection);

                using (sqlDataAdapter344)
                {
                    DataTable descriptionTable1 = new DataTable();
                    sqlDataAdapter344.Fill(descriptionTable1);

                    sqlConnection.Open();
                    SqlDataReader reader2 = sqlCommand234.ExecuteReader();
                    while (reader2.Read())
                    {
                        // Get the date submitted from the database
                        dateSubmitted = reader2.GetValue(0).ToString();
                    }

                    sqlConnection.Close();
                }

                // Set the value of report income store id
                reportIncomeStoreID = Convert.ToInt32(storeList.SelectedValue.ToString());

                // Set the value of report income unique id
                reportIncomeUniqueID = Convert.ToInt32(dateList.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                // Did not catch all the exception because it is a waste of time, be back here someday
                MessageBox.Show(ex.ToString());
            }
        }

        private void storeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If store list is change display to date list
            DisplayToDateList();
        }

        private void dateList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If date list is change display to data grid
            DisplayToDescriptionDataGrid();
        }

        private void reportIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            // if store list is empty or none selected
            if (storeList.SelectedIndex == -1)
            {
                MessageBox.Show("Select a value first in store list");
            }
            else
            {
                // if date list is empty or none selected
                if (dateList.SelectedIndex == -1)
                {
                    MessageBox.Show("Select a value first in date list");
                }
                else
                {
                    // Show the report daily expenses form for report viewer
                    ReportDailyIncomeForms cd = new ReportDailyIncomeForms();
                    cd.Show();
                }
            }
        }
    }
}
