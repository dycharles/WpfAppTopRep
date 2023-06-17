using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAppTopRep
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Holds connection to DB
        SqlConnection sqlConnection;

        // To know the username of the login user
        public static int userNameId;

        // To know the storename of the login user
        public static int storeId;

        // To know the userid for the login user
        public int userId = 0;

        public MainWindow()
        {
            InitializeComponent();

            //String connetion from database
            string connectionString = ConfigurationManager.ConnectionStrings["WpfAppTopRep.Properties.Settings.topRepDBConnectionString"].ConnectionString;

            // Initialize sql connection
            sqlConnection = new SqlConnection(connectionString);
        }

        // This method is used to search for username and password
        private bool IsSearchForUsernamePassword()
        {
            bool searchUsername = false;
            bool searchPassword = false;

            try
            {
                // Run query, connect to database, open database then close database connection
                string queryUsername = "SELECT CASE WHEN (EXISTS(SELECT Username FROM Username WHERE Username = '" + usernameTextBox.Text + "' COLLATE Latin1_General_CS_AS)) then 1 else 0 end";

                SqlCommand sqlCommandUsername = new SqlCommand(queryUsername, sqlConnection);
                SqlDataAdapter sqlDataAdapterUsername = new SqlDataAdapter(sqlCommandUsername);

                using (sqlDataAdapterUsername)
                {
                    DataTable descriptionTable = new DataTable();
                    sqlDataAdapterUsername.Fill(descriptionTable);

                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommandUsername.ExecuteReader();

                    while (reader.Read())
                    {
                        //get username value from database
                        if (reader.GetValue(0).ToString() == "1") searchUsername = true;

                    }

                    sqlConnection.Close();
                }

                // Run query, connect to database, open database then close database connection
                string queryPassword = "SELECT CASE WHEN (EXISTS(SELECT Username FROM Username WHERE Username = '" + passwordBox.Password + "' COLLATE Latin1_General_CS_AS)) then 1 else 0 end";
                SqlCommand sqlCommandPassword = new SqlCommand(queryPassword, sqlConnection);

                SqlDataAdapter sqlDataAdapterPasword = new SqlDataAdapter(sqlCommandPassword);

                using (sqlDataAdapterPasword)
                {
                    DataTable descriptionTable = new DataTable();
                    sqlDataAdapterPasword.Fill(descriptionTable);

                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommandUsername.ExecuteReader();

                    while (reader.Read())
                    {
                        // Check the  password value (1=true, 0=false)
                        if (reader.GetValue(0).ToString() == "1") searchPassword = true;
                    }

                    sqlConnection.Close();
                }

            }
            catch (Exception ex)
            {
               // Did not catch all the exception because it is a waste of time, be back here someday
               MessageBox.Show(ex.ToString());
            }

            // Return true if unsername and password match
            if (searchUsername == true && searchPassword == true) return true;

            // Return false if unsername and password did not match
            return false;

        }

        private void SearchForUserID()
        {
            try
            {
                // Run query, connect to database, open database then close database connection
                string queryUsername = "SELECT Id, StoreId FROM Username WHERE Username = '" + usernameTextBox.Text + "' COLLATE Latin1_General_CS_AS";

                SqlCommand sqlCommandUsername = new SqlCommand(queryUsername, sqlConnection);
                SqlDataAdapter sqlDataAdapterUsername = new SqlDataAdapter(sqlCommandUsername);

                using (sqlDataAdapterUsername)
                {
                    DataTable descriptionTable = new DataTable();
                    sqlDataAdapterUsername.Fill(descriptionTable);

                    sqlConnection.Open();
                    SqlDataReader reader = sqlCommandUsername.ExecuteReader();

                    while (reader.Read())
                    {
                        // Get the value of username from database
                        userNameId = Convert.ToInt32(reader.GetValue(0).ToString());

                        // Get the value of store id from database
                        storeId = Convert.ToInt32(reader.GetValue(1).ToString());
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

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            if (IsSearchForUsernamePassword() == true)
            {
                // Call the method to perform if username and password matched in the database
                SearchForUserID();

                // Clear the username and password textbox
                usernameTextBox.Text = "";
                passwordBox.Password = "";

                // Call the main panel
                MainPanel mainPanel = new MainPanel();
                mainPanel.Show();
            }
            else
            {
                // Username and password does not match
                MessageBox.Show("Please check Username and Password.","ERROR");
            }
        }

        private void CancelClick(object sender, RoutedEventArgs e)
        {
            // Exit the application
            App.Current.Shutdown();
        }

        private void usernameTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            // Username textbox got focus when mainwindow loaded
            usernameTextBox.Focus();
        }
    }
}
