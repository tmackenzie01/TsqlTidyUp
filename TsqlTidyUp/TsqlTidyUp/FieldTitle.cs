using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsqlTidyUp
{
    public class FieldTitle : FieldBase
    {
        public FieldTitle(String title)
        {
            if (title.Contains("_"))
            {
                m_headingRows = new List<String>();
                String[] headings = title.Split('_');

                // Annoying bit of code to add the underscores back in
                for (int i = 0; i < (headings.Length - 1); i++)
                {
                    m_headingRows.Add($"{headings[i]}_");
                }
                m_headingRows.Add($"{headings[headings.Length - 1]}");

                CalculateRowWidth();
            }
            else
            {
                m_headingRows = new List<String>() { title };
                CalculateRowWidth();
            }
        }

        public String GetRow(int i)
        {
            if (i == (m_headingRows.Count - 1))
            {
                return m_headingRows[i].PadRight(m_rowWidth);
            }
            else if (i < (m_headingRows.Count -1))
            {
                return $"{m_headingRows[i]}".PadRight(m_rowWidth);
            }

            return "".PadRight(m_rowWidth);
        }
    }
}
