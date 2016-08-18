using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsqlTidyUp
{
    public class RowHeading
    {
        public RowHeading(String row)
        {
            m_fields = row.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList<String>().Select(s => new FieldTitle(s)).ToList<FieldTitle>();
        }

        public String FitToWidth(int fitWidth)
        {
            List<int> titleLengths = new List<int>();
            bool fitSuccess = false;

            // Loop until we fit the length
            bool fitsLine = false;
            bool firstRun = true;
            bool fullySplit = false;
            int rows = 0;

            while ((!fitsLine) || (fullySplit))
            {
                int titleLengthTotal = 0;
                FieldTitle fieldToSplit = new FieldTitle("");
                fullySplit = true;

                foreach (FieldTitle field in m_fields)
                {
                    titleLengths.Add(field.RowWidth);
                    titleLengthTotal = titleLengthTotal + field.RowWidth + 3;

                    if (field.Rows > rows)
                    {
                        rows = field.Rows;
                    }

                    if (field.RowWidth > fieldToSplit.RowWidth)
                    {
                        if (field.CanSplit)
                        {
                            fieldToSplit = field;
                        }
                    }

                    // If we've split everything to the max then we'll exit the loop
                    fullySplit = fullySplit && !field.CanSplit;
                }
                
                int diff = titleLengthTotal - fitWidth;
                fitsLine = (diff < 0);

                // After the first run we have shortened all the titles with underscores "title_and_this"
                // so we have to split the largest title until it all fits
                if (!firstRun)
                {                    
                    fieldToSplit.Split();
                }

                firstRun = false;
            }

            // Now format then together
            StringBuilder result = new StringBuilder();

            // The headings
            for (int row = 0; row < rows; row++)
            {
                result.Append($" {m_fields[0].GetRow(row)} |");
                for (int i = 1; i < m_fields.Count; i++)
                {
                    result.Append($" {m_fields[i].GetRow(row)} |");
                }
                result.AppendLine();

                fitSuccess = (result.Length != fitWidth);
            }

            // Underlines
            result.Append($"-{ new String('-', m_fields[0].RowWidth)}-|");
            for (int i = 1; i < m_fields.Count; i++)
            {
                result.Append($"-{ new String('-', m_fields[i].RowWidth)}-|");
            }
            result.AppendLine();

            if (fitSuccess)
            {
                Debug.WriteLine($"*** Could meet specified width of {fitWidth}");
            }

            return result.ToString();

        }

        List<FieldTitle> m_fields;
    }
}
