using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTaxCalculationSAPDIAPI
{
    internal static class LogService
    {
        public static void Log(string message)
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}";

            try
            {
                if(!File.Exists("Log.txt"))
                {
                    File.Create("Log.txt").Close();
                }
                using (StreamWriter writer = File.AppendText("Log.txt"))
                {
                    writer.WriteLine(logMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}
