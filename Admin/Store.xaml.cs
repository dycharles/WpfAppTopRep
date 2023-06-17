using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfAppTopRep
{
    /// <summary>
    /// Interaction logic for Store.xaml
    /// </summary>
    public partial class Store : Window
    {
        //Holds connetion to DB
        SqlConnection sqlConnection;

        public Store()
        {
            InitializeComponent();

            //String connetion from database
            string connectionString = ConfigurationManager.ConnectionStrings["WpfAppTopRep.Properties.Settings.topRepDBConnectionString"].ConnectionString;

            // Initialize sql connection
            sqlConnection = new SqlConnection(connectionString);

            // Call this method to display the store in the storename listbox
            DisplayStore();
        }

        private void DisplayStore()
        {
            try
            {
                // Run query, connect to database and fill the store name list
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

        
        private void DisplayStoreToTextbox()
        {
            // Check if the name textbox length is not empty
            if (nameTextBox.Text.Length == 0)
            {
                //Set the list automatic to selected index = 0
                storeList.SelectedIndex = 0;
            }
            try
            {
                // Run query, connect to database, open database then close database connection
                string query = "SELECT Name, Street, City, State, Zipcode, Contact FROM Store d WHERE d.Id = @StoreID";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@StoreID", storeList.SelectedValue);

                    DataTable descriptionTable = new DataTable();
                    sqlDataAdapter.Fill(descriptionTable);

                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        // Get the name from the database
                        nameTextBox.Text = reader.GetValue(0).ToString();

                        // Get the street from the database
                        streetTextBox.Text = reader.GetValue(1).ToString();

                        // Get the city from the database
                        cityTextBox.Text = reader.GetValue(2).ToString();

                        // Get the state from the database
                        stateTextBox.Text = reader.GetValue(3).ToString();

                        // Get the zipcode from the database
                        zipCodeTextBox.Text = reader.GetValue(4).ToString();

                        // Get the contact from the database
                        contactTextBox.Text = reader.GetValue(5).ToString();

                    }

                    sqlConnection.Close();
                }
            }
            catch (Exception ex)
            {
                //Bug to fix here from delete button
                // Did not catch all the exception because it is a waste of time, be back here someday
                MessageBox.Show(ex.ToString());
            }
        }

        private void store_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Display to textbox
            DisplayStoreToTextbox();
        }

        private void InsertClick(object sender, RoutedEventArgs e)
        {
            // Check if name textbox is empty
            if (nameTextBox.Text == "")
            {
                MessageBox.Show("Name should not be empty");
            }
            else
            {
                try
                {
                    // Add list of parameters using textbox and also have to define data type that is matched in the database
                    List<SqlParameter> parameters = new List<SqlParameter>() {
                    new SqlParameter("@Name", SqlDbType.NVarChar){Value=nameTextBox.Text},
                    new SqlParameter("@Street", SqlDbType.NVarChar){Value=streetTextBox.Text},
                    new SqlParameter("@City", SqlDbType.NVarChar){Value=cityTextBox.Text},
                    new SqlParameter("@State", SqlDbType.NChar){Value=stateTextBox.Text},
                    new SqlParameter("@Zipcode", SqlDbType.Int){Value=zipCodeTextBox.Text},
                    new SqlParameter("@Contact", SqlDbType.Int){Value=contactTextBox.Text}
                    };

                    // Run query to insert data to databse, connect to database
                    string query = "INSERT INTO Store VALUES (@Name, @Street, @City, @State, @ZipCode, @Contact)";
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
                    // When all are passed we display the store name
                    DisplayStore();
                }
            }
        }

        private void DisplayUpdate()
        {
            try
            {
                // Run query, connect to database, open database then close database connection
                string query = "UPDATE Store Set " +
                "Name='" + nameTextBox.Text + "'," +
                "Street='" + streetTextBox.Text + "'," +
                "City='" + cityTextBox.Text + "'," +
                "State='" + stateTextBox.Text + "'," +
                "Zipcode='" + zipCodeTextBox.Text + "'," +
                "Contact='" + contactTextBox.Text + "'" +
                " WHERE Id=@StoreID";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    //sqlCommand.Parameters.AddWithValue("@DescriptionNAME", descriptionList.SelectedValue);
                    sqlCommand.Parameters.AddWithValue("@StoreID", storeList.SelectedValue);

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
                // When all are passed we display the store name
                DisplayStore();
            }
        }

        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            // Check if the username list is selected
            if (storeList.SelectedIndex == -1)
            {
                MessageBox.Show("No item selected in store list");
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
            if (storeList.SelectedIndex == -1)
            {
                MessageBox.Show("No item selected in store list");
            }
            else
            {
                try
                {
                    // Run query, connect to database, open database then close database connection
                    string query = "DELETE FROM Store WHERE Id = @StoreID";

                    // Simple way to execute a query without adapter
                    // Open and close on our own
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@StoreID", storeList.SelectedValue);
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
                    // Cleare the name text box
                    nameTextBox.Text = "";

                    // Call the method to display the store name
                    DisplayStore();
                }
            }
        }

        private void zipCodeTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //will accept number only, negative and decimal point
            e.Handled = new Regex("[^0-9]+").IsMatch((string)e.Text);
        }

        private void contactTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //will accept number only
            e.Handled = new Regex("[^0-9]+").IsMatch((string)e.Text);
        }

        private void nameTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            // If window loaded the name texbox will received the focus
            nameTextBox.Focus();
        }
    }
}
