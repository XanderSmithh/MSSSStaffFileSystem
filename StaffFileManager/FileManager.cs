using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffFileManager
{
    public class FileManager
    {

        public Dictionary<int, string> LoadFromCsv()
        {
            var result = new Dictionary<int, string>();
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MalinStaffNamesV3.csv");
                var lines = File.ReadAllLines(filePath)
                                .Where(line => !string.IsNullOrWhiteSpace(line));

                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length >= 2 && int.TryParse(parts[0].Trim(), out int staffId))
                    {
                        string staffName = parts[1].Trim();
                        if (!result.ContainsKey(staffId))
                            result.Add(staffId, staffName);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading CSV: {ex.Message}");
            }
            return result;
        }




        public void SaveAsCsv(Dictionary<int, string> staffData)
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MalinStaffNamesV3.1.csv"); // File Path

                var lines = staffData.Select(kvp => $"{kvp.Key},{kvp.Value}"); // Saves Key and Value to one line
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving CSV: {ex.Message}");
            }
    }

}
}
