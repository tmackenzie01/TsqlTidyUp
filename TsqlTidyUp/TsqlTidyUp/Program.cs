using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsqlTidyUp
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("No filename provided");
            }
            else
            {
                int powershellWidth = 119;

                // TODO This is a guide - remove if when complete
                Debug.WriteLine(new String('-', powershellWidth));
                Console.WriteLine(new String('-', powershellWidth));
                
                String filename = args[0];
                String[] tsqlText = File.ReadAllLines(filename);
                StringBuilder result = new StringBuilder();

                // Get the title lengths
                if (tsqlText.Length >= 2)
                {
                    // Split 1st row (titles), second row (underlines)
                    RowHeading headingRow = new RowHeading(tsqlText[0]);
                    result.AppendLine(headingRow.FitToWidth(powershellWidth));
                }

                Console.WriteLine(result.ToString());
                Debug.WriteLine(result.ToString());
            }
        }
    }
}
