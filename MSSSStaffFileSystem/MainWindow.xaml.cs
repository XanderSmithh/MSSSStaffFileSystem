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


        // Displays matching values with entered characters, Filtered Staff Listbox Display 
        public void DisplayFilteredStaffList(string filter)
        {
            dictionaryManager.ClearSortedStaffList();
            lbFilteredStaffList.Items.Clear();

            if (string.IsNullOrWhiteSpace(filter)) return;
            foreach (var kvp in dictionaryManager.ReturnStaffList())
            {
                if (kvp.Value.Contains(filter, StringComparison.OrdinalIgnoreCase))
                {
                    dictionaryManager.InsertIntoSortedStaffList(kvp.Key, kvp.Value);
                    lbFilteredStaffList.Items.Add($"{kvp.Key}, {kvp.Value}");
                }
            }
        }


        private void lbFilteredStaffList_ItemClicked(object sender, MouseButtonEventArgs e)
        {
            if (lbFilteredStaffList.SelectedItem != null)
            {
                string selectedItem = lbFilteredStaffList.SelectedItem.ToString();
                PopulateStaffTextBox(selectedItem);
            }
        }

        private void lbStaffList_ItemClicked(object sender, MouseButtonEventArgs e)
        {
            if (lbStaffList.SelectedItem != null)
            {
                string selectedItem = lbStaffList.SelectedItem.ToString();
                PopulateStaffTextBox(selectedItem);
            }
        }

        private void PopulateStaffTextBox(string selectedItem)
        {
            string key = selectedItem.Split(',')[2];
            string value = selectedItem.Split(',')[1];

            txtNameBox.Text = key.Trim(']', ' ');
            txtNumberBox.Text = value.Trim();

        }



        // KEYBIND METHODS

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) && e.Key == Key.F || e.SystemKey == Key.F)
            {
                FocusStaffNameField();
                e.Handled = true; 
            }

            else if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) && e.Key == Key.D || e.SystemKey == Key.D)
            {
                FocusNumberNameField();
                e.Handled = true; 
            }

            else if (e.Key == Key.Tab)
            {
                if (lbStaffList.SelectedItem != null)
                {
                    string selectedItem = lbStaffList.SelectedItem.ToString();
                    PopulateStaffTextBox(selectedItem);
                    lbStaffList.UnselectAll();
                }

                else if (lbFilteredStaffList.SelectedItem != null)
                {
                    string selectedItem = lbFilteredStaffList.SelectedItem.ToString();
                    PopulateStaffTextBox(selectedItem);
                    lbFilteredStaffList.UnselectAll();
                }
                e.Handled = true; 
            }

            else if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) && e.Key == Key.A || e.SystemKey == Key.A)
            {
                adminPopup.IsOpen = true;
                AdminGuiValueAssign();
                e.Handled = true;
            }



        }


        // Textbox Focus ( ID Textbox & Name TextBox).
        public void FocusStaffNameField()
        {
            txtNameBox.Text = string.Empty;
            txtNameBox.Focus();
        }

        public void FocusNumberNameField()
        {
            txtNumberBox.Text = string.Empty;
            txtNumberBox.Focus();
        }



        // Assigns Values in txtNameBox & txtNumberBox to related admin Popup textboxes.
        private void AdminGuiValueAssign()
        {
            txtAdminStaffID.Text = txtNumberBox.Text;
            txtAdminStaffName.Text = txtNameBox.Text;
        }



        // Sets popup to fasle when the close button inside of the popup is clicked by User
        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            adminPopup.IsOpen = false;
        }

        // Does stuff 
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("I do nothing lol...");
        }
    }
}