using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppTaxCalculationSAPDIAPI
{
    internal class MessageHelper
    {
        public static void PrintRectangle(string word)
        {
            int width = word.Length + 4; // Adjusting width based on word length
            int height = 6; // You can adjust the height as needed

            // Top border
            Console.WriteLine("╔" + new string('═', width) + "╗");

            // Sides with word in the middle
            for (int i = 0; i < height; i++)
            {
                if (i == height / 2)
                {
                    // Word line
                    int spacesBefore = (width - word.Length) / 2;
                    int spacesAfter = width - word.Length - spacesBefore;
                    Console.WriteLine("║" + new string(' ', spacesBefore) + word + new string(' ', spacesAfter) + "║");
                }
                else
                {
                    Console.WriteLine("║" + new string(' ', width) + "║");
                }
            }

            // Bottom border
            Console.WriteLine("╚" + new string('═', width) + "╝");
        }
    }
}
