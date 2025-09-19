using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffFileManager
{
    public class SortedStaffDictionary : IDictionaryManager
    {
        private SortedDictionary<int, string> sortedStaffList;

        public SortedStaffDictionary()
        {
            sortedStaffList = new SortedDictionary<int, string>();
        }

        public IDictionary<int, string> ReturnDictionary()
        {
            return sortedStaffList;
        }

        public void ClearDictionary()
        {
            if (sortedStaffList.Count > 0)
            {
                sortedStaffList.Clear();
            }
        }

        public bool CreateItem(int key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && !sortedStaffList.ContainsKey(key))
            {
                sortedStaffList.Add(key, value);
                return true;
            }
            return false;
        }

        public bool DeleteItem(int key)
        {
            if (sortedStaffList.ContainsKey(key))
            {
                sortedStaffList.Remove(key);
                return true;
            }
            return false;
        }

        public bool UpdateItem(int key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && sortedStaffList.ContainsKey(key))
            {
                sortedStaffList[key] = value;
                return true;
            }
            return false;
        }

        public void ConvertListToDictionary(List<string> csvLines)
        {
            {
                foreach (var line in csvLines)
                {
                    var parts = line.Split(',');
                    if (parts.Length >= 2 && int.TryParse(parts[0].Trim(), out int staffId))
                    {
                        string staffName = parts[1].Trim();
                        if (!sortedStaffList.ContainsKey(staffId))
                            sortedStaffList.Add(staffId, staffName);
                    }
                }
            }
        }

        public List<string> ConvertDictionaryToList()
        {
            return sortedStaffList.Select(kvp => $"{kvp.Key},{kvp.Value}").ToList();
        }

    }
}
