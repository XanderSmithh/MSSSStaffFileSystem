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
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MalinStaffNamesV3.csv");


                var lines = File.ReadAllLines(filePath);

                // Dictionary where key = line index + 1, value = staff name
                var result = lines
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select((line, index) => new { Index = index + 1, Name = line.Trim() })
                    .ToDictionary(x => x.Index, x => x.Name);

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading CSV: {ex.Message}");
                return new Dictionary<int, string>();
            }
        }



        public void SaveAsCsv(Dictionary<int, string> staffData)
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MalinStaffNamesV3.csv");

                File.WriteAllLines(filePath, staffData.Values);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving CSV: {ex.Message}");
            }
        }

    }
}
