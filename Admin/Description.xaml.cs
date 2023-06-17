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

namespace WpfAppTopRep
{
    /// <summary>
    /// Interaction logic for Description.xaml
    /// </summary>
    public partial class Description : Window
    {
        //Holds connetion to DB
        SqlConnection sqlConnection;

        public Description()
        {
            InitializeComponent();

            //String connetion from database
            string connectionString = ConfigurationManager.ConnectionStrings["WpfAppTopRep.Properties.Settings.topRepDBConnectionString"].ConnectionString;

            // Initialize sql connection
            sqlConnection = new SqlConnection(connectionString);

            // Call this method to display the description name in the description listbox
            DisplayDescription();
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
                    descriptionList.DisplayMemberPath = "Name";
                    
                    // Define what is unique about each item in the list
                    descriptionList.SelectedValuePath = "Id";
                    
                    // The content we will use to populate the list
                    descriptionList.ItemsSource = usernameTable.DefaultView;

                }
            }
            catch (Exception ex)
            {
                // Did not catch all the exception because it is a waste of time, be back here someday
                MessageBox.Show(ex.ToString());
            }
        }

        private void InsertClick(object sender, RoutedEventArgs e)
        {

            // If description textbox is empty
            if (descriptionTextBox.Text == "")
            {
                MessageBox.Show("Name should not be emplty");
            }
            else
            {
                try
                {
                    // Add list of parameters using textbox and also have to define data type that is matched in the database
                    List<SqlParameter> parameters = new List<SqlParameter>() {
                    new SqlParameter("@Name", SqlDbType.NVarChar){Value=descriptionTextBox.Text}
                    };

                    // Run query to insert data to databse, connect to database
                    string query = "INSERT INTO Description VALUES (@Name)";
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                    sqlCommand.Parameters.AddRange(parameters.ToArray());
                    DataTable descriptionTable = new DataTable();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand)) adapter.Fill(descriptionTable);
                }
                catch (Exception ex)
                {
                    // Did not catch all the exception because it is a waste of time, be back here someday
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    // When all are passed we display the description name
                    DisplayDescription();
                }
            }
        }

        private void DisplayDescriptionToTextbox()
        {
            if (descriptionTextBox.Text.Length == 0)
            {
                //Set the list automatic to selected index = 0
                descriptionList.SelectedIndex = 0;
            }
            try
            {

                // Run query, connect to database, open database then close database connection
                string query = "SELECT Name FROM Description d WHERE d.Id = @DescriptionID";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@DescriptionID", descriptionList.SelectedValue);

                    DataTable descriptionTable = new DataTable();
                    sqlDataAdapter.Fill(descriptionTable);

                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        // Get the description name from the database
                        descriptionTextBox.Text = reader.GetValue(0).ToString();

                    }

                    sqlConnection.Close();
                }

            }
            catch (Exception ex)
            {
                // Did not catch all the exception because it is a waste of time, be back here someday
                MessageBox.Show(ex.ToString());
            }
        }

        private void description_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If desciption list is selected it will display to the textbox
            DisplayDescriptionToTextbox();
        }

        private void DisplayUpdate()
        {
            try
            {
                // Run query, connect to database, open database then close database connection
                string query = "UPDATE Description Set Name='" + descriptionTextBox.Text + "' WHERE Id=@DescriptionID";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@DescriptionID", descriptionList.SelectedValue);

                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
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
                // If desciption list is selected it will display to the textbox
                DisplayDescription();
            }
        }
        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            // Check if the description list is selected
            if (descriptionList.SelectedIndex == -1)
            {
                MessageBox.Show("No item selected in descripton list");
            }
            else
            {
                // Call this method to update the display
                DisplayUpdate();
            }            
        }

        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            // Check the username list is selected
            if (descriptionList.SelectedIndex == -1)
            {
                MessageBox.Show("No item selected in descripton list");
            }
            else
            {
                try
                {
                    // Run query, connect to database, open database then close database connection
                    string query = "DELETE FROM Description WHERE Id = @DescriptionID";

                    // Simple way to execute a query without adapter
                    // Open and close on our own
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@DescriptionID", descriptionList.SelectedValue);
                    // Execute query
                    sqlCommand.ExecuteScalar();
                    sqlConnection.Close();

                }
                catch (Exception ex)
                {
                    // This throws an error because the store inventory
                    // list expects a default store selection
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    // Clear the description text box
                    descriptionTextBox.Text = "";

                    // Call the method to display the description name
                    DisplayDescription();
                }
            }
        }

        private void descriptionTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            // If window loaded the description textbox will received the focus
            descriptionTextBox.Focus();
        }
    }
}
