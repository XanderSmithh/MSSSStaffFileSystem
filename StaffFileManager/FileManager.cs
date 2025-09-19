using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaffFileManager
{
    public static class FileManager
    {

        // ---- LOAD CSV ----

        // Loads csv as List<String>
        public static List<string> LoadCsvLines()
        {
            var linesList = new List<string>();
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MalinStaffNamesV3.csv");
                linesList = File.ReadAllLines(filePath)
                                .Where(line => !string.IsNullOrWhiteSpace(line))
                                .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading CSV lines: {ex.Message}");
            }
            return linesList;
        }



        // ---- SAVE CSV ----

        // Saves the passed List<string> as filename
        public static void SaveCsvLines(List<string> csvLines, string fileName = "MalinStaffNamesV3.1.csv")
        {
            try
            {
                string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                File.WriteAllLines(filePath, csvLines);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving CSV: {ex.Message}");
            }
        }



    }
}
