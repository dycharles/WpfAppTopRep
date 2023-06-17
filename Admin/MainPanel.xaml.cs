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

namespace WpfAppTopRep
{
    /// <summary>
    /// Interaction logic for MainPanel.xaml
    /// </summary>
    public partial class MainPanel : Window
    {
        public MainPanel()
        {
            InitializeComponent();
        }

        private void UserClick(object sender, RoutedEventArgs e)
        {
            // Call the username window panel and show the window panel
            Username username = new Username();
            username.Show();
            username.Topmost = true;
        }

        private void StoreClick(object sender, RoutedEventArgs e)
        {
            // Call the username window panel and show the window panel
            Store store = new Store();
            store.Show();
            store.Topmost = true;
        }

        private void DescriptionClick(object sender, RoutedEventArgs e)
        {
            // Call the username window panel and show the window panel
            Description description = new Description();
            description.Show();
            description.Topmost = true;
        }

        private void IncomeClick(object sender, RoutedEventArgs e)
        {
            DailyIncome dailyIncome = new DailyIncome();
            dailyIncome.Show();
            dailyIncome.Topmost = true;
        }

        private void ExpensesClick(object sender, RoutedEventArgs e)
        {
            // Call the DailyExpenses window panel and show the window panel
            DailyExpenses dailyExpenses = new DailyExpenses();
            dailyExpenses.Show();
            dailyExpenses.Topmost = true;
        }

        private void dailyIncomeReport_Click(object sender, RoutedEventArgs e)
        {
            // Call the DailyIncomeReport window panel and show the window panel
            DailyIncomeReport dailyIncomeVsExpenses = new DailyIncomeReport();
            dailyIncomeVsExpenses.Show();
        }

        private void dailyExpensesReport_Click(object sender, RoutedEventArgs e)
        {
            // Call the DailyExpensesReport window panel and show the window panel
            DailyExpensesReport dailyIncomeVsExpenses = new DailyExpensesReport();
            dailyIncomeVsExpenses.Show();
        }
    }
}
