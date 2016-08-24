using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsqlTidyUp
{
    public class FieldBase
    {
        public FieldBase()
        {
        }

        public void Split()
        {
            // Combine the string back together, then increment the row count and split it evenly
            String originalField = this.ToString();
            int rowCount = Rows + 1;

            List<String> newHeadingRows = new List<String>();
            int splitLength = originalField.Length / rowCount;

            if (splitLength > 1)
            {
                for (int i = 0; i < (rowCount - 1); i++)
                {
                    newHeadingRows.Add(originalField.Substring(0, splitLength));
                    originalField = originalField.Substring(splitLength, originalField.Length - splitLength);
                }
            }

            newHeadingRows.Add(originalField);
            int newRowWidth = newHeadingRows.OrderByDescending(s => s.Length).First().Length;

            if (newRowWidth < m_rowWidth)
            {
                m_headingRows = newHeadingRows;
                CalculateRowWidth();
            }
            else
            {
                m_splitLimitReached = true;
            }
        }

        protected void CalculateRowWidth()
        {
            m_rowWidth = m_headingRows.OrderByDescending(s => s.Length).First().Length;
        }

        public void IncreaseRowWidth(int widthIncrease)
        {
            // Just need to increase each row by the same
            for (int i =0; i < m_headingRows.Count; i++)
            {
                m_headingRows[i] = m_headingRows[i] + new String(' ', widthIncrease);
            }
            CalculateRowWidth();
        }

        public bool CanSeparate
        {
            get
            {
                return m_containsSeparator;
            }
        }

        public bool CanSplit
        {
            get
            {
                return ((m_rowWidth > 3) && !m_splitLimitReached);
            }
        }

        public int RowWidth
        {
            get
            {
                return m_rowWidth;
            }
        }

        public int Rows
        {
            get
            {
                return m_headingRows.Count;
            }
        }

        public int DisplayIndex
        {
            get
            {
                return m_displayIndex;
            }
            set
            {
                m_displayIndex = value;
            }
        }

        public String GetRow(int i)
        {
            if (i <= (m_headingRows.Count - 1))
            {
                return m_headingRows[i].PadRight(m_rowWidth);
            }

            return "".PadRight(m_rowWidth);
        }

        public override string ToString()
        {
            return m_headingRows.Aggregate((a, b) => (a + b));
        }

        protected List<String> m_headingRows;
        protected int m_rowWidth;
        protected bool m_containsSeparator;
        bool m_splitLimitReached;
        int m_displayIndex;
    }
}
