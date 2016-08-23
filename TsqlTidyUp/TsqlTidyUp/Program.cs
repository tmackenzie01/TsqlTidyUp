﻿using System;
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
                    result.Append(headingRow.FitToWidth(powershellWidth));

                    List<int> originalWidths = tsqlText[1].Split(' ').Select(s => s.Length).ToList<int>();
                    for (int i = 2; i < tsqlText.Length; i++)
                    {
                        // Don't parse empty rows or the bottom (X rows affected) line
                        if (!String.IsNullOrEmpty(tsqlText[i]))
                        {
                            if (!tsqlText[i].Contains("rows affected"))
                            {
                                // Get the first row of data and get the minimum width of each data element (we pass in the heading widths as a guide)
                                RowData data = new RowData(tsqlText[i], originalWidths, headingRow.RowWidths());
                                rows.Add(data);
                            }
                        }
                    }

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
