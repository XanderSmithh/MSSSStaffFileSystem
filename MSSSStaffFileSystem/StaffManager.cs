using StaffFileManager;
using StaffFileManager.Dictionary;

namespace MSSSStaffFileSystem
{
    public class StaffManager
    {
        private StaffDictionary staffDictionary = new StaffDictionary();
        private SortedStaffDictionary sortedStaffDictionary = new SortedStaffDictionary();

        public IDictionaryManager CurrentDictionary { get; private set; }
        public bool IsSorted { get; private set; } = false;

        public StaffManager()
        {
            CurrentDictionary = staffDictionary;
        }

        public void SwitchDictionary()
        {
            if (IsSorted)
            {
                sortedStaffDictionary.ClearDictionary();
                staffDictionary.PopulateDictionary();
                CurrentDictionary = staffDictionary;
                IsSorted = false;
            }
            else
            {
                foreach (var kvp in staffDictionary.ReturnDictionary())
                {
                    if (!sortedStaffDictionary.ReturnDictionary().ContainsKey(kvp.Key))
                        sortedStaffDictionary.CreateItem(kvp.Key, kvp.Value);
                }
                staffDictionary.ClearDictionary();
                CurrentDictionary = sortedStaffDictionary;
                IsSorted = true;
            }
        }

        public IDictionary<int, string> GetDictionary() => CurrentDictionary.ReturnDictionary();
        public bool AddStaff(int id, string name) => CurrentDictionary.CreateItem(id, name);
        public bool UpdateStaff(int id, string name) => CurrentDictionary.UpdateItem(id, name);
        public bool DeleteStaff(int id) => CurrentDictionary.DeleteItem(id);
        public List<string> ConvertDictionary() => CurrentDictionary.ConvertDictionaryToList();
    }

}
