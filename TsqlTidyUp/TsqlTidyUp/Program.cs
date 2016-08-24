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

                String filename = args[0];
                String[] tsqlText = File.ReadAllLines(filename);
                StringBuilder result = new StringBuilder();

                // Get the title lengths
                if (tsqlText.Length >= 2)
                {
                    // Split 1st row (titles), second row (underlines)
                    RowHeading headingRow = new RowHeading(tsqlText[0]);
                    List<RowData> rows = new List<RowData>();
                    headingRow.FitToWidth(powershellWidth);

                    List<int> originalWidths = tsqlText[1].Split(' ').Select(s => s.Length).ToList<int>();
                    for (int i = 2; i < tsqlText.Length; i++)
                    {
                        // Don't parse empty rows or the bottom (X rows affected) line
                        if (!String.IsNullOrEmpty(tsqlText[i]))
                        {
                            if (!tsqlText[i].Contains("rows affected"))
                            {
                                // Get the first row of data and get the minimum width of each data element (we pass in the heading widths as a guide)
                                RowData data = new RowData(tsqlText[i], originalWidths);
                                rows.Add(data);
                            }
                        }
                    }

                    // Look at each row and determine which fields require larger widths
                    List<int> currentHeadingWidths = headingRow.RowWidths();
                    int[] widthIncreases = new int[currentHeadingWidths.Count];
                    foreach (RowData eachRow in rows)
                    {
                        List<int> eachRowWidths = eachRow.RowWidths();
                        for (int j = 0; j < eachRowWidths.Count; j++)
                        {
                            int adjustedWidth = currentHeadingWidths[j] + widthIncreases[j];
                            if (eachRowWidths[j] > adjustedWidth)
                            {
                                widthIncreases[j] = eachRowWidths[j] - currentHeadingWidths[j];
                            }
                        }
                    }

                    for (int k = 0; k < widthIncreases.Length; k++)
                    {
                        if (widthIncreases[k] > 0)
                        {
                            Debug.WriteLine($"Field {k + 1} requires {widthIncreases[k]} extra characters");
                        }
                    }

                    int fullRowWidth = headingRow.FullRowWidth;
                    if (fullRowWidth < powershellWidth)
                    {
                        int index = 0;
                        while ((fullRowWidth + widthIncreases[index]) < powershellWidth)
                        {
                            headingRow.IncreaseRowWidth(index, widthIncreases[index]);

                            fullRowWidth = headingRow.FullRowWidth;
                            index++;

                            if (index >= widthIncreases.Length)
                            {
                                break;
                            }
                        }
                    }

                    // Re-calculate the heading row widths
                    currentHeadingWidths = headingRow.RowWidths();

                    // Fit each row to the headings widths
                    foreach (RowData newDataRow in rows)
                    {
                        newDataRow.FitToHeadingWidths(currentHeadingWidths);
                    }

                    // Headings and underlines
                    result.AppendLine(new String('-', powershellWidth));
                    headingRow.ConstructRow(result);
                    headingRow.ConstructUnderlineRow(result);

                    StringBuilder result3 = new StringBuilder("");

                    // Data
                    int count = 0;
                    foreach (RowData eachRow in rows)
                    {
                        eachRow.ConstructRow(result);
                    }
                }

                Console.WriteLine(result.ToString());
                Debug.WriteLine(result.ToString());
            }
        }
    }
}
