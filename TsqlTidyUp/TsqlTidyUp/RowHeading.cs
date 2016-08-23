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

        public void FitToWidth(int fitWidth)
        {
            // Get the current width of all the headings
            int currentWidth = 0;
            int displayIndex = 0;
            foreach (FieldTitle field in m_fields)
            {
                currentWidth = currentWidth + field.RowWidth + 3;

                // Set the display index here as my creation for m_fields was far too clever for me to fit in a second parameter for display index
                field.DisplayIndex = displayIndex++;
            }

            // Stage 1 - Separate
            FieldTitle selectedHeading = null;
            m_fields.Sort(new RowWidthComparer());
            for (int i = 0; i < m_fields.Count; i++)
            {
                if (currentWidth > fitWidth)
                {
                    selectedHeading = m_fields[i];
                    if (selectedHeading.CanSeparate)
                    {
                        currentWidth = currentWidth - selectedHeading.RowWidth;
                        selectedHeading.Separate();
                        currentWidth = currentWidth + selectedHeading.RowWidth;
                    }
                }
            }

            //DebugRowDisplay("STAGE 1\r\n");

            // Stage 2 - Split headings not separated
            m_fields.Sort(new RowWidthComparer());
            for (int j = 0; j < m_fields.Count; j++)
            {
                if (currentWidth > fitWidth)
                {
                    selectedHeading = m_fields[j];
                    if ((selectedHeading.CanSplit) && (!selectedHeading.CanSeparate))
                    {
                        currentWidth = currentWidth - selectedHeading.RowWidth;
                        selectedHeading.Split();
                        currentWidth = currentWidth + selectedHeading.RowWidth;
                    }
                }
            }

            //DebugRowDisplay("STAGE 2\r\n");

            // Stage 3 - Split any
            m_fields.Sort(new RowWidthComparer());
            bool fullySplit = false;

            while ((currentWidth > fitWidth) && (!fullySplit))
            {
                fullySplit = true;
                for (int k = 0; k < m_fields.Count; k++)
                {
                    if (currentWidth > fitWidth)
                    {
                        selectedHeading = m_fields[k];
                        if (selectedHeading.CanSplit)
                        {
                            currentWidth = currentWidth - selectedHeading.RowWidth;
                            selectedHeading.Split();
                            currentWidth = currentWidth + selectedHeading.RowWidth;
                        }
                    }

                    fullySplit = fullySplit && !selectedHeading.CanSplit;
                }
            }

            //DebugRowDisplay("STAGE 3\r\n");

            // Important make sure everything is sorted back to display order
            m_fields.Sort(new DisplayIndexComparer());
        }

        private void DebugRowDisplay(String debugHeading)
        {
            m_fields.Sort(new RowWidthComparer());
            StringBuilder debugText = new StringBuilder(debugHeading);
            ConstructRow(debugText);
            Debug.WriteLine(debugText.ToString());
        }

        public void ConstructRow(StringBuilder result)
        {
            int maxRows = 0;
            foreach (FieldTitle title in m_fields)
            {
                if (title.Rows > maxRows)
                {
                    maxRows = title.Rows;
                }
            }

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

        public void ConstructUnderlineRow(StringBuilder result)
        {
            result.Append($"-{ new String('-', m_fields[0].RowWidth)}-|");
            for (int i = 1; i < m_fields.Count; i++)
            {
                result.Append($"-{ new String('-', m_fields[i].RowWidth)}-|");
            }
            result.AppendLine();
        }

        public List<int> RowWidths()
        {
            return m_fields.Select(s => s.RowWidth).ToList<int>();
        }

        List<FieldTitle> m_fields;
    }
}
