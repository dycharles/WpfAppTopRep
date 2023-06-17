using System;
using System.Collections.Generic;
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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using Microsoft.Reporting.Map.WebForms.BingMaps;

namespace WpfAppTopRep
{
    /// <summary>
    /// Interaction logic for Username.xaml
    /// </summary>
    public partial class Username : Window
    {
        //Holds connetion to DB
        SqlConnection sqlConnection;

        public Username()
        {
            InitializeComponent();

            //String connetion from database
            string connectionString = ConfigurationManager.ConnectionStrings["WpfAppTopRep.Properties.Settings.topRepDBConnectionString"].ConnectionString;

            // Initialize sql connection
            sqlConnection = new SqlConnection(connectionString);

            // Call this method to display the username in the username listbox
            DisplayUsername();

            // Call this method to dipslay the store name in the store name listbox
            DisplayStore();
        }

        private void DisplayUsername()
        {
            try
            {
                // Run query, connect to database and fill the username list
                string query = "SELECT * FROM Username";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable usernameTable = new DataTable();
                    sqlDataAdapter.Fill(usernameTable);

                    // Define the column we are displaying in listbox
                    usernameList.DisplayMemberPath = "Name";

                    // Define what is unique about each item in the list
                    usernameList.SelectedValuePath = "Id";

                    // The content we will use to populate the list
                    usernameList.ItemsSource = usernameTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                // Did not catch all the exception because it is a waste of time, be back here someday
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                // When all are passed we clear the textbox
                passwordBox.Password = "";
                verifyPasswordBox.Password = "";
            }
        }

        private void DisplayStore()
        {
            try
            {
                // Run query, connect to database and fill the username list
                string query = "SELECT * FROM Store";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable usernameTable = new DataTable();
                    sqlDataAdapter.Fill(usernameTable);

                    // Define the column we are displaying in listbox
                    storeComboBox.DisplayMemberPath = "Name";

                    // Define what is unique about each item in the list
                    storeComboBox.SelectedValuePath = "Id";

                    // The content we will use to populate the list
                    storeComboBox.ItemsSource = usernameTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                // Did not catch all the exception because it is a waste of time, be back here someday
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                // When all are passed we clear the textbox
                passwordBox.Password = "";
                verifyPasswordBox.Password = "";
            }
        }

        private void DisplayUsernameToTextbox()
        {
            // If name textbox is empty
            if (nameTextBox.Text.Length == 0)
            {
                //Set the list automatic to selected index = 0
                usernameList.SelectedIndex = 0;
            }
            try
            {
                // Run query, connect to database, open database then close database connection
                string query = "SELECT Username, Name, EmailAddress, Contact, StoreId from Username u WHERE u.Id = @UsernameID";

                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("UsernameID", usernameList.SelectedValue);

                    DataTable descriptionTable = new DataTable();
                    sqlDataAdapter.Fill(descriptionTable);

                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        // Get the username from the database
                        usernameTextBox.Text = reader.GetValue(0).ToString();

                        // Get the name from the database
                        nameTextBox.Text = reader.GetValue(1).ToString();

                        // Get the email address from the database
                        emailAddressTextBox.Text = reader.GetValue(2).ToString();

                        // Get the contact numbers from the database
                        contactTextBox.Text = reader.GetValue(3).ToString();

                        // Get the store name from the database
                        storeComboBox.SelectedValue = reader.GetValue(4).ToString();
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

        private void username_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // If username list is selected it will display to the textbox
            DisplayUsernameToTextbox();
        }

        private void InsertClick(object sender, RoutedEventArgs e)
        {
            // If username textbox is empty
            if (usernameTextBox.Text == "")
            {
                MessageBox.Show("User name should not be empty");
            }
            else
            {
                // If the email is valid or not
                if (isValidEmailAddress(emailAddressTextBox.Text) == false)
                {
                    MessageBox.Show("Incorrect email address");
                }
                else
                {
                    // If the password length is 10
                    if (passwordBox.Password.ToString().Length != 10)
                    {
                        MessageBox.Show("Password should be 10 characters long.", "ERROR");
                    }
                    else
                    {
                        // if password matched from user input to the databse 
                        if (passwordBox.Password != verifyPasswordBox.Password)
                        {
                            MessageBox.Show("Please check username and password.", "ERROR");
                        }
                        else
                        {
                            try
                            {
                                // Add list of parameters using textbox and also have to define data type that is matched in the database
                                List<SqlParameter> parameters = new List<SqlParameter>() {
                                new SqlParameter("@Username", SqlDbType.NVarChar){Value=usernameTextBox.Text},
                                new SqlParameter("@Name", SqlDbType.NVarChar){Value=nameTextBox.Text},
                                new SqlParameter("@Password", SqlDbType.NChar){Value=passwordBox.Password},
                                new SqlParameter("@EmailAddress", SqlDbType.NVarChar){Value=emailAddressTextBox.Text},
                                new SqlParameter("@Contact", SqlDbType.NVarChar){Value=contactTextBox.Text}
                                };

                                // Run query to insert data to databse, connect to database
                                string query = "INSERT INTO Username VALUES (@Username, @Name,HASHBYTES('SHA1','@Password') , @EmailAddress, @Contact, @StoreId)";

                                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                                sqlCommand.Parameters.AddWithValue("@StoreId", storeComboBox.SelectedValue);

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
                                // When all are passed we display the user name
                                DisplayUsername();
                            }
                        }
                    }
                }
            }
        }

        private void DisplayUpdate()
        {
            // Check if store combobox is no empty or selected
            if (storeComboBox.Text == "" )
            {
                MessageBox.Show("Store list should not be empty");
            }
            else
            {
                // Check if the password lenght is 10
                if (passwordBox.Password.ToString().Length != 10)
                {
                    MessageBox.Show("Password should be 10 characters long.", "ERROR");
                }
                else
                {
                    // Check if password is true from the user input to the database
                    if (passwordBox.Password != verifyPasswordBox.Password)
                    {
                        MessageBox.Show("Please check username and password.", "ERROR");
                    }
                    else
                    {
                        try
                        {
                            // Run query, connect to database, open database then close database connection
                            string query = "UPDATE Username Set " +
                            "Username='" + usernameTextBox.Text + "'," +
                            "Name='" + nameTextBox.Text + "'," +
                            "Password=HASHBYTES('SHA1','" + passwordBox.Password + "')," +
                            "EmailAddress='" + emailAddressTextBox.Text + "'," +
                            "Contact='" + contactTextBox.Text + "'," +
                            "StoreId=@StoreId " +
                            "WHERE Id=@UsernameID";

                            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                            using (sqlDataAdapter)
                            {
                                sqlCommand.Parameters.AddWithValue("@UsernameID", usernameList.SelectedValue);
                                sqlCommand.Parameters.AddWithValue("@StoreId", storeComboBox.SelectedValue);

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
                            //DisplayUsername();
                        }
                    }
                }
            }
        }

        private void UpdateClick(object sender, RoutedEventArgs e)
        {
            // Check if the username list is selected
            if (usernameList.SelectedIndex == -1)
            {
                MessageBox.Show("No item selected in User name list");
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
            if (usernameList.SelectedIndex == -1)
            {
                MessageBox.Show("No item selected in User name list");
            }
            else
            {
                try
                {
                    // Run query, connect to database, open database then close database connection
                    string query = "DELETE FROM Username WHERE Id = @UsernameID";

                    // Simple way to execute a query without adapter
                    // Open and close on our own
                    SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                    sqlConnection.Open();

                    sqlCommand.Parameters.AddWithValue("@UsernameID", usernameList.SelectedValue);

                    // Execute query
                    sqlCommand.ExecuteScalar();
                    sqlConnection.Close();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                finally
                {
                    // Clear the name text box
                    nameTextBox.Text = "";

                    // Call the method to display the user name
                    DisplayUsername();
                }
            }
        }


        private void contactTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //will accept number only
            e.Handled = new Regex("[^0-9]+").IsMatch((string)e.Text);
        }

        private void emailAddressTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //call the method isValidEmailAddress to check the value of text box
            if (isValidEmailAddress(emailAddressTextBox.Text)==false) MessageBox.Show("Incorrect email address");
        }

        //method to check if email address is valid
        private bool isValidEmailAddress(string textBoxName)
        {
            string email = textBoxName;
            // Regex pattern for validating email address
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            if (!match.Success)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void usernameTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            // If window loaded the username textbox will received the focus
            usernameTextBox.Focus();
        }
    }
}