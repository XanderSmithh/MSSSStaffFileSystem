using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffFileManager.Dictionary
{
    public interface IDictionaryManager
    {
        IDictionary<int, string> ReturnDictionary();

        void ClearDictionary();

        bool CreateItem(int key, string value);

        bool DeleteItem(int key);

        bool UpdateItem(int key, string value);

        void ConvertListToDictionary(List<string> csvLines);

        List<string> ConvertDictionaryToList();
    }
}
