using System.Text;
using System.Windows;
using StaffFileManager;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MSSSStaffFileSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DictionaryManager dictionaryManager = new DictionaryManager();
        FileManager fileManager = new FileManager();
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            dictionaryManager.PopulateStaffList();
            DisplayStaffList();
        }



        private void tbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = tbSearch.Text;

            DisplayFilteredStaffList(filter);
        }




        // Staff Listbox Display
        public void DisplayStaffList()
        {
            lbStaffList.ItemsSource = null;
            lbStaffList.ItemsSource = dictionaryManager.ReturnStaffList();
        }

        // Filtered Staff Listbox Display
        public void DisplayFilteredStaffList(string filter)
        {
            lbFilteredStaffList.ItemsSource = null;
            dictionaryManager.ClearSortedStaffList();

            if (string.IsNullOrWhiteSpace(filter)) return;

            foreach(var kvp in dictionaryManager.ReturnStaffList())
            {
                if (kvp.Value.Contains(filter, StringComparison.OrdinalIgnoreCase))
                {
                    dictionaryManager.InsertIntoSortedStaffList(kvp.Key, kvp.Value);
                }
            }

            lbFilteredStaffList.ItemsSource = dictionaryManager.ReturnSortedStaffList();
        }





    }
}