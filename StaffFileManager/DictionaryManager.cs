using System;
using System.Collections.Generic;

namespace StaffFileManager
{
    public class DictionaryManager
    {
        private Dictionary<int, string> staffList = new Dictionary<int, string>();

        private SortedDictionary<int, string> sortedStaffList = new SortedDictionary<int, string>();


        // Staff Dictionary Methods
        public void PopulateStaffList()
        {
            FileManager fileManager = new FileManager();
            staffList = fileManager.LoadFromCsv();
        }

        public Dictionary<int, string> ReturnStaffList()
        {
            return staffList;
        }

        public void InsertIntoStaffList(int key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && !staffList.ContainsKey(key))
                staffList.Add(key, value);
        }

        public void UpdateKvpStaffList(int key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && staffList.ContainsKey(key))
                staffList[key] = value;
        }

        public void RemoveKvpStaffList(int key)
        {
            staffList.Remove(key);
        }



        // Sorted Staff Dictionary Methods
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
            if (!string.IsNullOrWhiteSpace(value) && !sortedStaffList.ContainsKey(key))
                sortedStaffList.Add(key, value);
        }

        public void UpdateSortedKvp(int key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && sortedStaffList.ContainsKey(key))
                sortedStaffList[key] = value;
        }
        public void RemoveKvpFilteredStaffList(int key)
        {
            sortedStaffList.Remove(key);
        }

    }
}
