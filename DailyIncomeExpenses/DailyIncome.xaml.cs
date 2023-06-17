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
using System.Reflection;
using System.Windows.Controls.Primitives;
using System.Text.RegularExpressions;

namespace WpfAppTopRep
{
    /// <summary>
    /// Interaction logic for DailyIncome.xaml
    /// </summary>
    public partial class DailyIncome : Window
    {
        //Holds connetion to DB
        SqlConnection sqlConnection;

        public DailyIncome()
        {
            InitializeComponent();

            //String connetion from database
            string connectionString = ConfigurationManager.ConnectionStrings["WpfAppTopRep.Properties.Settings.topRepDBConnectionString"].ConnectionString;

            // Initialize sql connection
            sqlConnection = new SqlConnection(connectionString);

            // Call this method to display the description name in the description listbox
            DisplayDescription();

            // Call this method to display the income amount in the expenses listbox
            DisplayDailyIncome();

            // Set the totalAmountLabel
            totalAmountLabel.Content = TotalAmount();

            // Set the dailyUniqueID
            dailyUniqueIDLabel.Content = GetDailyIncomeUniqueID();
        }

        private void DisplayDescription()
        {
            try
            {
                // Run query, connect to database and fill the username list
                string query = "SELECT * FROM Description";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable usernameTable = new DataTable();
                    sqlDataAdapter.Fill(usernameTable);

                    // Define the column we are displaying in listbox
                    descriptionComboBox.DisplayMemberPath = "Name";

                    // Define what is unique about each item in the list
                    descriptionComboBox.SelectedValuePath = "Id";

                    // The content we will use to populate the list
                    descriptionComboBox.ItemsSource = usernameTable.DefaultView;

                }
            }
            catch (Exception ex)
            {
                // Did not catch all the exception because it is a waste of time, be back here someday
                MessageBox.Show(ex.ToString());
            }
        }

        private void DisplayDailyIncome()
        {
            try
            {
                string query = "SELECT dc.Name AS 'Description' ,di.Quantity ,FORMAT(di.Amount, 'N', 'en-us') AS Amount,  FORMAT((di.Quantity*di.Amount) , 'N', 'en-us')AS Total, di.Comment AS 'Comment' FROM DailyIncome di INNER JOIN Description dc ON di.IdDescription = dc.Id WHERE di.DailyIncomeUniqueID = " + GetDailyIncomeUniqueID();
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable usernameTable = new DataTable();
                    sqlDataAdapter.Fill(usernameTable);

                    // Define the column we are displaying in listbox
                    descriptionDataGrid.DisplayMemberPath = "Name";

                    // Define what is unique about each item in the list
                    descriptionDataGrid.SelectedValuePath = "Id";

                    // The content we will use to populate the list
                    descriptionDataGrid.ItemsSource = usernameTable.DefaultView;

                }
            }
            catch (Exception ex)
            {
                // Did not catch all the exception because it is a waste of time, be back here someday
                MessageBox.Show(ex.ToString());
            }
        }

        private void DisplayDailyIncomeNew()
        {
            try
            {
                string query = "SELECT IdDescription, Quantity, Amount FROM DailyIncome d WHERE d.DailyIncomeUniqueID = " + (GetDailyIncomeUniqueID() + 1);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable usernameTable = new DataTable();
                    sqlDataAdapter.Fill(usernameTable);

                    // Define the column we are displaying in listbox
                    descriptionDataGrid.DisplayMemberPath = "Name";

                    // Define what is unique about each item in the list
                    descriptionDataGrid.SelectedValuePath = "Id";

                    // The content we will use to populate the list
                    descriptionDataGrid.ItemsSource = usernameTable.DefaultView;
                }

            }
            catch (Exception ex)
            {
                // Did not catch all the exception because it is a waste of time, be back here someday
                MessageBox.Show(ex.ToString());
            }
        }

        private void SubmitClick(object sender, RoutedEventArgs e)
        {
            // Get unique id from the database
            int count = GetDailyIncomeUniqueID();

            // Get unique id from the database and plus 1
            int count2 = GetDailyIncomeUniqueID() + 1;

            try
            {
                // Run query, connect to database, open database then close database connection
                string query = "UPDATE DailyIncome Set " +
                "DateTimeSubmitted='" + DateTime.Now + "'," +
                "SubmittedAlready= 1" +
                " WHERE DailyIncomeUniqueID =" + count;

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }


                // Run query, connect to database, open database then close database connection
                string queryUpdateDailyIncomeUniqueID = "UPDATE DailyUniqueID Set " + "DailyIncomeUniqueID=(" + count2 + ")" + " WHERE DailyIncomeUniqueID=" + count;

                SqlCommand sqlCommandUpdateDailyIncomeUniqueID = new SqlCommand(queryUpdateDailyIncomeUniqueID, sqlConnection);

                SqlDataAdapter sqlDataAdapterUpdateDailyIncomeUniqueID = new SqlDataAdapter(sqlCommandUpdateDailyIncomeUniqueID);

                using (sqlDataAdapterUpdateDailyIncomeUniqueID)
                {
                    sqlConnection.Open();
                    sqlCommandUpdateDailyIncomeUniqueID.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                // Did not catch all the exception because it is a waste of time, be back here someday
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                // Clear the amount text box
                amountTextBox.Text = "";

                // Clear the description combo box
                descriptionComboBox.Text = "";

                // Clear the total amount label
                totalAmountLabel.Content = "";

                // Clear the comment text box
                commentTextBox.Text = "";

                // Call the method to display new income form
                DisplayDailyIncomeNew();

                // Set the daily unir id label
                dailyUniqueIDLabel.Content = GetDailyIncomeUniqueID();

                // Show the user if the report is submitted already
                MessageBox.Show("Report Submitted");
            }
        }

        private int GetDailyIncomeUniqueID()
        {
            // Get the daily expenses unique id to return the value of the method
            string DailyIncomeUniqueID = "";

            try
            {
                // Run query, connect to database, open database then close database connection
                string queryDailyUniqueID = "SELECT DailyIncomeUniqueID FROM DailyUniqueID";
                SqlCommand sqlCommandDailyUniqueID = new SqlCommand(queryDailyUniqueID, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommandDailyUniqueID);

                using (sqlDataAdapter)
                {
                    DataTable descriptionTable = new DataTable();
                    sqlDataAdapter.Fill(descriptionTable);

                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommandDailyUniqueID.ExecuteReader();
                    while (reader.Read())
                    {
                        // Get the daily income unique id from the database
                        DailyIncomeUniqueID = reader.GetValue(0).ToString();

                    }
                    sqlConnection.Close();

                }
            }
            catch (Exception ex)
            {
                // Did not catch all the exception because it is a waste of time, be back here someday
                MessageBox.Show(ex.ToString());
            }

            // Return the converted value from daily income unique id
            return Convert.ToInt32(DailyIncomeUniqueID);
        }

        private string TotalAmount()
        {
            // Get the total amount to return the value of the method
            double totalAmountLabel = 0;

            try
            {
                // Run query, connect to database, open database then close database connection
                string queryTotalAmount = "SELECT SUM(Quantity*Amount) FROM DailyIncome d WHERE d.DailyIncomeUniqueID= " + GetDailyIncomeUniqueID();

                SqlCommand sqlTotalAmount = new SqlCommand(queryTotalAmount, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlTotalAmount);

                using (sqlDataAdapter)
                {
                    DataTable descriptionTable = new DataTable();
                    sqlDataAdapter.Fill(descriptionTable);

                    sqlConnection.Open();
                    SqlDataReader reader = sqlTotalAmount.ExecuteReader();
                    while (reader.Read())
                    {
                        // Get the total amount (quantity*amount) from the database
                        totalAmountLabel = Convert.ToDouble(reader.GetValue(0).ToString());

                    }
                    sqlConnection.Close();

                }

            }
            catch (Exception ex)
            {
                sqlConnection.Close();
            }

            // Return the total amount with formatted string to xxx,xxx.xx
            return String.Format("{0:0,0.00}", totalAmountLabel);
        }

        private void InsertClick(object sender, RoutedEventArgs e)
        {

            // If quantity textbox is empty
            if (quantityTextBox.Text == "")
            {
                MessageBox.Show("Quantity should not be empty");
            }
            else
            {
                // If description combo box is empty
                if (descriptionComboBox.Text == "")
                {
                    MessageBox.Show("Description should not be empty");
                }
                else
                {
                    // If amount textbox is empty
                    if (amountTextBox.Text == "")
                    {
                        MessageBox.Show("Amount should not be empty");
                    }
                    else
                    {
                        // Parsed the value to double to check if it is a valid number    
                        double parsedValue;
                        if (!double.TryParse(amountTextBox.Text, out parsedValue))
                        {
                            MessageBox.Show("Check the value of the amount");
                        }
                        else
                        {
                            // Set the unique id
                            int count = GetDailyIncomeUniqueID();

                            try
                            {
                                // Add list of parameters using textbox and also have to define data type that is matched in the database
                                List<SqlParameter> parameters = new List<SqlParameter>() {
                                new SqlParameter("@Quantity", SqlDbType.Int){Value=quantityTextBox.Text},
                                new SqlParameter("@Amount", SqlDbType.Money){Value=amountTextBox.Text},
                                new SqlParameter("@DateTimeCreated", SqlDbType.DateTime){Value=DateTime.Now},
                                new SqlParameter("@DateIncomeUniqueID", SqlDbType.Int){Value=count},
                                new SqlParameter("@Comment", SqlDbType.NVarChar){Value=commentTextBox.Text},
                                new SqlParameter("@IdUsernameSubmitted", SqlDbType.Int){Value=MainWindow.userNameId},
                                new SqlParameter("@StoreId", SqlDbType.Int){Value=MainWindow.storeId}
                                };

                                // Run query to insert data to databse, connect to database
                                string query = "INSERT INTO DailyIncome VALUES (@Quantity, @Amount, @IDDescription,@IdUsernameSubmitted,@DateTimeCreated,@DateTimeCreated,0,@DateIncomeUniqueID,@Comment,@StoreId)";
                                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                                sqlCommand.Parameters.AddWithValue("@IDDescription", descriptionComboBox.SelectedValue);
                                sqlCommand.Parameters.AddRange(parameters.ToArray());
                                DataTable usernameTable = new DataTable();
                                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand)) adapter.Fill(usernameTable);

                            }
                            catch (Exception ex)
                            {
                                // Run query to insert data to databse, connect to database
                                MessageBox.Show(ex.ToString());
                            }
                            finally
                            {
                                // Display the daily income
                                DisplayDailyIncome();

                                // Clear the description combo box
                                descriptionComboBox.Text = "";

                                // Clear the amount text box
                                amountTextBox.Text = "";

                                // Clear the comment text box
                                commentTextBox.Text = "";

                                // set the total amount label to total amount
                                totalAmountLabel.Content = TotalAmount();
                            }
                        }
                    }
                }
            }
        }

        private void amountTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Will accept number, negative and decimal point only
            e.Handled = new Regex("[^-0-9.]+").IsMatch((string)e.Text);
        }

        private void quantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Will accept number only
            e.Handled = new Regex("[^0-9]+").IsMatch((string)e.Text);
        }

        private void quantityTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            // Quantity text got focus when loaded
            quantityTextBox.Focus();

            // Select all the text in quantity text box
            quantityTextBox.SelectAll();    
        }

        private void amountTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Prevent from entering twice the decimal point
            if (e.Key.GetHashCode() == 88)
            {
                e.Handled = (sender as TextBox).Text.Contains(".");
            }

            // Prevent from entering twice the negative sign
            if (e.Key.GetHashCode() == 87)
            {
                e.Handled = (sender as TextBox).Text.Contains("-");
            }
        }
    }
}
