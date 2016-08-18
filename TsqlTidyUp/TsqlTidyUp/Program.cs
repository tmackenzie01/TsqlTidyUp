using System;
using System.Collections.Generic;
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
                Console.WriteLine($"HEllo {args[0]}");
                String filename = args[0];
                String[] tsqlText = File.ReadAllLines(filename);
                List<int> titleLengths = new List<int>();
                List<int> fullLengths = new List<int>();
                StringBuilder result = new StringBuilder();

                // Get the title lengths
                if (tsqlText.Length >= 2)
                {
                    // Split 1st row (titles), second row (underlines)
                    String[] titleHeadings = tsqlText[0].Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
                    String[] underlines = tsqlText[1].Split(" ".ToCharArray());

                    foreach (String titleHeading in titleHeadings)
                    {
                        titleLengths.Add(titleHeading.Length);
                    }

                    foreach (String underline in underlines)
                    {
                        fullLengths.Add(underline.Length);
                    }

                    result.Append($" {titleHeadings[0]} |");
                    for (int i = 1; i < titleHeadings.Length; i++)
                    {
                        result.Append($" {titleHeadings[i]} |");
                    }

                    result.AppendLine();
                    result.Append($"-{underlines[0].Substring(0, titleLengths[0])}-|");
                    for (int j = 1; j < titleHeadings.Length; j++)
                    {
                        result.Append($" {underlines[j].Substring(0, titleLengths[j])} |");
                    }

                    result.AppendLine();
                    foreach (int fullLength in fullLengths)
                    {
                        result.AppendLine($"{fullLength} chars");
                    }

                    // Use fullLengths on the remaining lines 
                    // get substring for each field
                    // trimright and get their length
                }

                Console.WriteLine(result.ToString());
            }
        }
    }
}
