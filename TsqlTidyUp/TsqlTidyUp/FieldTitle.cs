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
                m_headingRows = new List<String>();
                int previousChar = 0;
                // See if we can split the heading on camelCase, only try it if the string is above a certain length
                if (title.Length > 6)
                {
                    for (int i = 1; i < title.Length; i++)
                    {
                        if (Char.IsUpper(title, i))
                        {
                            m_headingRows.Add(title.Substring(previousChar, i - previousChar));
                            previousChar = i;
                        }
                    }

                    m_headingRows.Add(title.Substring(previousChar, title.Length - previousChar));
                }

                if (m_headingRows.Count == 0)
                {
                    // No camelCase
                    m_headingRows.Add(title);
                }

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
