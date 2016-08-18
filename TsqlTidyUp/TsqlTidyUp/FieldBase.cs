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

            m_headingRows = new List<String>();
            int splitLength = originalField.Length / rowCount;

            if (splitLength > 1)
            {
                for (int i = 0; i < (rowCount - 1); i++)
                {
                    m_headingRows.Add(originalField.Substring(0, splitLength));
                    originalField = originalField.Substring(splitLength, originalField.Length - splitLength);
                }
            }

            m_headingRows.Add(originalField);
            CalculateRowWidth();
        }

        protected void CalculateRowWidth()
        {
            m_rowWidth = m_headingRows.OrderByDescending(s => s.Length).First().Length;
        }

        public bool CanSplit
        {
            get
            {
                return (m_rowWidth > 3);
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

        public override string ToString()
        {
            return m_headingRows.Aggregate((a, b) => (a + b));
        }

        protected List<String> m_headingRows;
        protected int m_rowWidth;
    }
}
