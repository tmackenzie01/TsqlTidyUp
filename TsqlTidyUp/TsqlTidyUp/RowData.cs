using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsqlTidyUp
{
    public class RowData
    {
        public RowData(String rowData, List<int> headingWidths, List<int> newWidths)
        {
            m_fields = new List<FieldData>();
            int total = 0;
            foreach (int width in headingWidths)
            {
                total = total + width + 1;
            }
            total--;

            if (total != rowData.Length)
            {
                throw new Exception("Widths don't match");
            }

            int start = 0;
            String textData = "";
            for (int i = 0; i < headingWidths.Count; i++)
            {
                textData = rowData.Substring(start, headingWidths[i]);

                // Trim starting and trailing spaces (text fields seem to be left adjusted, number values to the right)
                textData = textData.TrimStart(' ').TrimEnd(' ');
                if (textData.Length > newWidths[i])
                {
                    textData = textData.Substring(0, newWidths[i]);
                }
                else
                {
                    textData = textData.PadRight(newWidths[i]);
                }
                m_fields.Add(new FieldData(textData));
                start = start + headingWidths[i] + 1;
            }
        }

        public void ConstructRow(StringBuilder result)
        {
            int maxRows = 1;
            for (int row = 0; row < maxRows; row++)
            {
                result.Append($" {m_fields[0].GetRow(row)} |");
                for (int i = 1; i < m_fields.Count; i++)
                {
                    result.Append($" {m_fields[i].GetRow(row)} |");
                }
                result.AppendLine();
            }
        }

        List<FieldData> m_fields;
    }
}
