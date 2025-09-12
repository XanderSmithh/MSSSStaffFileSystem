using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffFileManager
{
    public class DictionaryManager
    {
        private Dictionary<int, string> staffList = new Dictionary<int, string>();
        
        private SortedDictionary<int, string> sortedStaffList= new SortedDictionary<int, string>();

        public void PopulateStaffList()
        {
            FileManager fileManager = new FileManager();
            staffList = fileManager.LoadFromCsv();
        }

        public Dictionary<int, string> ReturnStaffList()
        {
            return staffList;
        }

        public SortedDictionary<int, string> ReturnSortedStaffList()
        {
            return sortedStaffList;
        }
        public void ClearSortedStaffList()
        {
            sortedStaffList.Clear();
        }

        public void InsertIntoSortedStaffList(int key, string value)
        {
            sortedStaffList.Add(key, value);
        }
    }
}
