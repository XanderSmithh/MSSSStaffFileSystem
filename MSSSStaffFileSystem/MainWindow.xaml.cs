using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Diagnostics;
using StaffFileManager.FileIO;

namespace MSSSStaffFileSystem
{
    public partial class MainWindow : Window
    {
        private StaffManager staffManager;
        private bool isDictionarySorted = false;

        // Constructor & Loaded
        public MainWindow()
        {
            InitializeComponent();
            staffManager = new StaffManager();
            DataContext = this;
            Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DisplayStaffList();
        }

        // Status Strip
        private void SetStatusMsg(string message)
        {
            if (StatusText != null)
                StatusText.Text = message;
        }




        // GUI Display Methods
        private void DisplayStaffList()
        {
            StatusText.Text = "";
            try
            {
                lbStaffList.Items.Clear();
                foreach (var kvp in staffManager.GetDictionary())
                    lbStaffList.Items.Add($"[{kvp.Key}, {kvp.Value}]");
            }
            catch
            {
                SetStatusMsg("Error displaying staff list.");
            }
        }

        private void DisplayFilteredStaffList(string filter)
        {
            StatusText.Text = "";
            try
            {
                lbFilteredStaffList.Items.Clear();
                if (string.IsNullOrWhiteSpace(filter)) return;

                var filtered = staffManager.GetDictionary()
                    .Where(kvp => kvp.Value.Contains(filter, StringComparison.OrdinalIgnoreCase)
                               || kvp.Key.ToString().Contains(filter));

                if (isDictionarySorted)
                    filtered = filtered.OrderBy(kvp => kvp.Key);

                foreach (var kvp in filtered)
                    lbFilteredStaffList.Items.Add($"[{kvp.Key}, {kvp.Value}]");
            }
            catch
            {
                SetStatusMsg("Error filtering staff list.");
            }
        }




        // Button Methods
        private void BtnToggleDirectory_Click(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "";
            staffManager.SwitchDictionary();
            isDictionarySorted = staffManager.IsSorted;
            DisplayStaffList();

            SetStatusMsg("Dictionary has been changed!");
        }

        private void BtnAdminAdd_Click(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "";
            var newStaff = CreateStaff();
            if (newStaff.HasValue)
            {
                staffManager.AddStaff(newStaff.Value.staffId, newStaff.Value.staffName);
                DisplayStaffList();
                SetStatusMsg("Staff User has been Created!");
            }
        }

        private void BtnAdminUpdate_Click(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "";
            if (int.TryParse(txtAdminStaffID.Text, out int id))
            {
                staffManager.UpdateStaff(id, txtAdminStaffName.Text);
                DisplayStaffList();
                SetStatusMsg("Staff User has been Updated!");

                txtNameBox.Text = string.Empty;
                txtNumberBox.Text = string.Empty;
            }
            else
            {
                SetStatusMsg("Invalid staff ID for update.");
            }
        }

        private void BtnAdminDelete_Click(object sender, RoutedEventArgs e)
        {
            StatusText.Text = "";
            if (int.TryParse(txtAdminStaffID.Text, out int id))
            {
                staffManager.DeleteStaff(id);
                DisplayStaffList();
                SetStatusMsg("Staff user has been Deleted Successfully!");

                txtNameBox.Text = string.Empty;
                txtNumberBox.Text = string.Empty;
            }
            else
            {
                SetStatusMsg("Failed to delete staff: invalid ID.");
            }
        }

        private void BtnAdminClear_Click(object sender, RoutedEventArgs e)
        {
            txtAdminStaffID.Text = "";
            txtAdminStaffName.Text = "";
            SetStatusMsg("Values have been cleared from admin fields!");
        }

        private void BtnClosePopup_Click(object sender, RoutedEventArgs e)
        {

            StatusText.Text = "";
            try
            {
                adminPopup.IsOpen = false;
                FileManager.SaveCsvLines(staffManager.ConvertDictionary());                
                SetStatusMsg("Data has been saved as a Csv!");
            }
            catch
            {
                SetStatusMsg("Error saving CSV.");
            }
        }



        // ListBox Item Clicks
        private void LbStaffList_ItemClicked(object sender, MouseButtonEventArgs e)
        {
            if (lbStaffList.SelectedItem != null)
                PopulateStaffTextBox(lbStaffList.SelectedItem.ToString());
        }

        private void LbFilteredStaffList_ItemClicked(object sender, MouseButtonEventArgs e)
        {
            if (lbFilteredStaffList.SelectedItem != null)
                PopulateStaffTextBox(lbFilteredStaffList.SelectedItem.ToString());
        }

        private void PopulateStaffTextBox(string selectedItem)
        {
            StatusText.Text = "";
            try
            {
                var parts = selectedItem.Trim('[', ']').Split(',');
                txtNumberBox.Text = parts[0].Trim();
                txtNameBox.Text = parts[1].Trim();
            }
            catch
            {
                SetStatusMsg("Error populating textboxes.");
            }
        }

        private void TbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            DisplayFilteredStaffList(tbSearch.Text);
        }




        // Keybind Methods
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
                FileManager.SaveCsvLines(staffManager.ConvertDictionary());
                e.Handled = true;
            }
        }

        private void FocusStaffNameField()
        {
            txtNameBox.Text = "";
            txtNameBox.Focus();
        }

        private void FocusNumberNameField()
        {
            txtNumberBox.Text = "";
            txtNumberBox.Focus();
        }



        // Admin GUI Methods
        private void AdminGuiValueAssign()
        {
            txtAdminStaffID.Text = txtNumberBox.Text;
            txtAdminStaffName.Text = txtNameBox.Text;
        }

        private (int staffId, string staffName)? CreateStaff()
        {
            StatusText.Text = "";
            try
            {
                if (!string.IsNullOrWhiteSpace(txtAdminStaffName.Text))
                {
                    string staffName = txtAdminStaffName.Text;
                    int staffId;
                    var rnd = new Random();

                    do
                    {
                        staffId = 770000000 + rnd.Next(0, 10000000);
                    } while (staffManager.GetDictionary().ContainsKey(staffId));

                    return (staffId, staffName);
                }
                return null;
            }
            catch
            {
                SetStatusMsg("Error creating staff.");
                return null;
            }
        }
    }
}
