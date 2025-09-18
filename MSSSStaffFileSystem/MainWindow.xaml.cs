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



        // BUTTON METHODS

        private void btnAdminAdd_Click(object sender, RoutedEventArgs e)
        {
            var newStaff = CreateStaff(); 
            if (newStaff.HasValue) 
            {
                AddToStaffList(newStaff.Value.staffId, newStaff.Value.staffName);
                DisplayStaffList();
            }
        }

        private void btnAdminUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateStaff();
            DisplayStaffList();
        }

        private void btnAdminDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteStaff();
            DisplayStaffList();
        }

        private void btnAdminClear_Click(object sender, RoutedEventArgs e)
        {
            txtAdminStaffID.Text = string.Empty;
            txtAdminStaffName.Text = string.Empty;
        }

        private void btnClosePopup_Click(object sender, RoutedEventArgs e)
        {
            adminPopup.IsOpen = false;
            fileManager.SaveAsCsv(dictionaryManager.ReturnStaffList());
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("I do nothing lol...");
        }





        // GUI METHODS

        public void DisplayStaffList()
        {
            lbStaffList.Items.Clear();
            foreach (var kvp in dictionaryManager.ReturnStaffList())
            {
                lbStaffList.Items.Add($"[{kvp.Key}, {kvp.Value}]");
            }
        }

        public void DisplayFilteredStaffList(string filter)
        {
            dictionaryManager.ClearSortedStaffList();
            lbFilteredStaffList.Items.Clear();

            if (string.IsNullOrWhiteSpace(filter)) return;
            foreach (var kvp in dictionaryManager.ReturnStaffList())
            {
                if (kvp.Value.Contains(filter, StringComparison.OrdinalIgnoreCase) || // Checks for matching KvP
                    kvp.Key.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase))
                {
                    dictionaryManager.InsertIntoSortedStaffList(kvp.Key, kvp.Value);
                    lbFilteredStaffList.Items.Add($"[{kvp.Key}, {kvp.Value}]");
                }
            }
        }

        private void tbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            string filter = tbSearch.Text;
            DisplayFilteredStaffList(filter);
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
            string key = selectedItem.Split(',')[1];
            string value = selectedItem.Split(',')[0];

            txtNameBox.Text = key.Trim(']', ' ');
            txtNumberBox.Text = value.Trim('[',' ');
        }

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

        private void AdminGuiValueAssign()
        {
            txtAdminStaffID.Text = txtNumberBox.Text;
            txtAdminStaffName.Text = txtNameBox.Text;
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

            else if (Keyboard.Modifiers.HasFlag(ModifierKeys.Alt) && e.Key == Key.L || e.SystemKey == Key.L)
            {
                adminPopup.IsOpen = false;
                fileManager.SaveAsCsv(dictionaryManager.ReturnStaffList());
                e.Handled = true;
            }
        }




        // ADMIN GUI METHODS

        public (int staffId, string staffName)? CreateStaff()
        {
            if (!string.IsNullOrWhiteSpace(txtAdminStaffName.Text))
            {
                string staffName = txtAdminStaffName.Text;
                int staffId;
                var staffList = dictionaryManager.ReturnStaffList();

                var rnd = new Random();

                // Gets a random 77 number whilst ensuring it's not duplicating an existing number
                do
                {
                    staffId = 770000000 + rnd.Next(0, 10000000);
                } while (staffList.ContainsKey(staffId)); // ensure unique

                return (staffId, staffName);
            }

            return null; // nothing to create
        }

        private void UpdateStaff()
        {
            string staffName = txtAdminStaffName.Text;
            if (int.TryParse(txtAdminStaffID.Text, out int staffId) && !string.IsNullOrWhiteSpace(staffName))
            {
                if (dictionaryManager.ReturnStaffList().ContainsKey(staffId))
                {
                    dictionaryManager.UpdateKvpStaffList(staffId, staffName);
                }
            }
        }

        private void DeleteStaff()
        {
            if (int.TryParse(txtAdminStaffID.Text, out int staffId))
            {
                dictionaryManager.RemoveKvpStaffList(staffId);
            }
        }

        private void AddToStaffList(int staffId, string staffName)
        {
            dictionaryManager.InsertIntoStaffList(staffId, staffName);
        }



    }
}