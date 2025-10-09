using StaffFileManager.FileIO;

namespace StaffFileManager.Dictionary
{
    public class StaffDictionary : IDictionaryManager
    {
        private Dictionary<int, string> staffList;

        public StaffDictionary()
        {
            staffList = new Dictionary<int, string>();
            PopulateDictionary();
        }

        public void PopulateDictionary()
        {
            ConvertListToDictionary(FileManager.LoadCsvLines());
        }

        public IDictionary<int, string> ReturnDictionary()
        {
            return staffList;
        }

        public void ClearDictionary()
        {
            if (staffList.Count > 0)
            {
                staffList.Clear();
            }
        }

        public bool CreateItem(int key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && !staffList.ContainsKey(key))
            {
                staffList.Add(key, value);
                return true;
            }
            return false;
        }

        public bool DeleteItem(int key)
        {
            if (staffList.ContainsKey(key))
            {
                staffList.Remove(key);
                return true;
            }
            return false;
        }

        public bool UpdateItem(int key, string value)
        {
            if (!string.IsNullOrWhiteSpace(value) && staffList.ContainsKey(key))
            {
                staffList[key] = value;
                return true;
            }
            return false;
        }

        public void ConvertListToDictionary(List<string> csvLines )
        {
            {
                foreach (var line in csvLines)
                {
                    var parts = line.Split(',');
                    if (parts.Length >= 2 && int.TryParse(parts[0].Trim(), out int staffId))
                    {
                        string staffName = parts[1].Trim();
                        if (!staffList.ContainsKey(staffId))
                            staffList.Add(staffId, staffName);
                    }
                }
            }
        }

        public List<string> ConvertDictionaryToList()
        {
            return staffList.Select(kvp => $"{kvp.Key},{kvp.Value}").ToList();
        }
    }
}
