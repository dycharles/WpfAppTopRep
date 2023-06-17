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
using System.Text.RegularExpressions;

namespace WpfAppTopRep
{
    /// <summary>
    /// Interaction logic for DailyExpenses.xaml
    /// </summary>
    public partial class DailyExpenses : Window
    {//Holds connetion to DB
        SqlConnection sqlConnection;

        public DailyExpenses()
        {
            InitializeComponent();
            
            //String connetion from database
            string connectionString = ConfigurationManager.ConnectionStrings["WpfAppTopRep.Properties.Settings.topRepDBConnectionString"].ConnectionString;

            // Initialize sql connection
            sqlConnection = new SqlConnection(connectionString);

            // Call this method to display the description name in the description listbox
            DisplayDescription();

            // Call this method to display the expenses amount in the expenses listbox
            DisplayDailyExpenses();
            
            // Set the totalAmountLabel
            totalAmountLabel.Content = TotalAmount();
            
            // Set the dailyUniqueID
            dailyUniqueIDLabel.Content = GetDailyExpensesUniqueID();

        }

        private void DisplayDescription()
        {
            try
            {
                // Run query, connect to database and fill the description list
                string query = "SELECT * FROM Description";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable dt1 = new DataTable();
                    sqlDataAdapter.Fill(dt1);

                    // Define the column we are displaying in listbox
                    descriptionComboBox.DisplayMemberPath = "Name";

                    // Define what is unique about each item in the list
                    descriptionComboBox.SelectedValuePath = "Id";

                    // The content we will use to populate the list
                    descriptionComboBox.ItemsSource = dt1.DefaultView;
                }
            }
            catch (Exception ex)
            {
                // Did not catch all the exception because it is a waste of time, be back here someday
                MessageBox.Show(ex.ToString());
            }
        }

        private void DisplayDailyExpenses()
        {
            try
            {
                // Run query, connect to database and fill the description list
                string query = "SELECT dc.Name AS 'Description' ,de.Quantity, FORMAT(de.Amount, 'N', 'en-us') AS Amount, FORMAT((de.Quantity*de.Amount) , 'N', 'en-us')AS Total, de.Comment AS'Comment' FROM DailyExpenses de INNER JOIN Description dc ON de.IdDescription = dc.Id WHERE de.DailyExpensesUniqueID = " + GetDailyExpensesUniqueID();

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable dt1 = new DataTable();
                    sqlDataAdapter.Fill(dt1);

                    // Define the column we are displaying in listbox
                    descriptionDataGrid.DisplayMemberPath = "Name";

                    // Define what is unique about each item in the list
                    descriptionDataGrid.SelectedValuePath = "Id";

                    // The content we will use to populate the list
                    descriptionDataGrid.ItemsSource = dt1.DefaultView;
                }
            }
            catch (Exception ex)
            {
                // Did not catch all the exception because it is a waste of time, be back here someday
                MessageBox.Show(ex.ToString());
            }
        }

        private void DisplayDailyExpensesNew()
        {
            try
            {
                // Run query, connect to database
                string query = "SELECT IdDescription, Quantity, Amount FROM DailyExpenses d WHERE d.DailyExpensesUniqueID = " + (GetDailyExpensesUniqueID() + 1);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable dt1 = new DataTable();
                    sqlDataAdapter.Fill(dt1);

                    // Define the column we are displaying in listbox
                    descriptionDataGrid.DisplayMemberPath = "Name";

                    // Define what is unique about each item in the list
                    descriptionDataGrid.SelectedValuePath = "Id";

                    // The content we will use to populate the list
                    descriptionDataGrid.ItemsSource = dt1.DefaultView;
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
            int countUniqueID = GetDailyExpensesUniqueID();

            // Get unique id from the database and plus 1
            int countUniqueIDPlus1 = GetDailyExpensesUniqueID() + 1;
           
            try
            {
                // Run query, connect to database, open database then close database connection
                string query = "UPDATE DailyExpenses Set " +
                "DateTimeSubmitted='" + DateTime.Now + "'," +
                "SubmittedAlready= 1" +
                " WHERE DailyExpensesUniqueID =" + countUniqueID;

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }


                // Run query, connect to database, open database then close database connection
                string queryUpdateDailyExpensesUniqueID = "UPDATE DailyUniqueID Set " + "DailyExpensesUniqueID=(" + countUniqueIDPlus1 + ")" + " WHERE DailyExpensesUniqueID=" + countUniqueID;

                SqlCommand sqlCommandUpdateDailyExpensesUniqueID = new SqlCommand(queryUpdateDailyExpensesUniqueID, sqlConnection);

                SqlDataAdapter sqlDataAdapterUpdateDailyExpensesUniqueID = new SqlDataAdapter(sqlCommandUpdateDailyExpensesUniqueID);

                using (sqlDataAdapterUpdateDailyExpensesUniqueID)
                {
                    sqlConnection.Open();
                    sqlCommandUpdateDailyExpensesUniqueID.ExecuteNonQuery();
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
                
                // Call the method to display new expenses form
                DisplayDailyExpensesNew();

                // Set the daily unir id label
                dailyUniqueIDLabel.Content = GetDailyExpensesUniqueID();
                
                // Show the user if the report is submitted already
                MessageBox.Show("Report Submitted");
            }
        }

        private int GetDailyExpensesUniqueID()
        {
            // Get the daily expenses unique id to return the value of the method
            string DailyExpensesUniqueID = "";

            try
            {
                // Run query, connect to database, open database then close database connection
                string queryDailyUniqueID = "SELECT DailyExpensesUniqueID FROM DailyUniqueID";
                SqlCommand sqlCommandDailyUniqueID = new SqlCommand(queryDailyUniqueID, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommandDailyUniqueID);

                using (sqlDataAdapter)
                {
                    DataTable dt1 = new DataTable();
                    sqlDataAdapter.Fill(dt1);

                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommandDailyUniqueID.ExecuteReader();
                    while (reader.Read())
                    {
                        // Get the daily expenses unique id from the database
                        DailyExpensesUniqueID = reader.GetValue(0).ToString();

                    }
                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                // Did not catch all the exception because it is a waste of time, be back here someday
                MessageBox.Show(ex.ToString());
            }

            // Return the converted value from daily expenses unique id
            return Convert.ToInt32(DailyExpensesUniqueID);
        }

        private string TotalAmount()
        {
            // Get the total amount to return the value of the method
            double totalAmountLabel = 0;

            try
            {
                // Run query, connect to database, open database then close database connection
                string queryTotalAmount = "SELECT SUM(Quantity*Amount) FROM DailyExpenses d WHERE d.DailyExpensesUniqueID= " + GetDailyExpensesUniqueID();

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
                MessageBox.Show("Qauntity should not be empty");
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
                        double  parsedValue;
                        if (!double.TryParse(amountTextBox.Text, out parsedValue))
                        {
                            MessageBox.Show("Check the value of the amount");
                        }
                        else
                        {
                            // Set the unique id
                            int countUniqueID = GetDailyExpensesUniqueID();

                            try
                            {
                                // Add list of parameters using textbox and also have to define data type that is matched in the database
                                List<SqlParameter> parameters = new List<SqlParameter>() {
                                new SqlParameter("@Quantity", SqlDbType.Int){Value=quantityTextBox.Text},
                                new SqlParameter("@Amount", SqlDbType.Money){Value=amountTextBox.Text},
                                new SqlParameter("@DateTimeCreated", SqlDbType.DateTime){Value=DateTime.Now},
                                new SqlParameter("@DateExpensesUniqueID", SqlDbType.Int){Value=countUniqueID},
                                new SqlParameter("@Comment", SqlDbType.NVarChar){Value=commentTextBox.Text},
                                new SqlParameter("@IdUsernameSubmitted", SqlDbType.Int){Value=MainWindow.userNameId},
                                new SqlParameter("@StoreId", SqlDbType.Int){Value=MainWindow.storeId}
                                };

                                // Run query to insert data to databse, connect to database
                                string query = "INSERT INTO DailyExpenses VALUES (@Quantity, @Amount, @IDDescription,@IdUsernameSubmitted,@DateTimeCreated,@DateTimeCreated,0,@DateExpensesUniqueID,@Comment,@StoreId)";
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
                                // Display the daily expenses
                                DisplayDailyExpenses();

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
