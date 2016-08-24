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
        public RowData(String rowData, List<int> headingWidths)
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
                m_fields.Add(new FieldData(textData));
                start = start + headingWidths[i] + 1;
            }
        }

        public void FitToHeadingWidths(List<int> newWidths)
        {
            for (int i = 0; i < m_fields.Count; i++)
            {
                m_fields[i].Truncate(newWidths[i]);
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

        public List<int> RowWidths()
        {
            return m_fields.Select(s => s.RowWidth).ToList<int>();
        }

        List<FieldData> m_fields;
    }
}
