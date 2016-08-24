using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsqlTidyUp
{
    public class FieldData : FieldBase
    {
        public FieldData(String data)
        {
            m_headingRows = new List<String>();
            m_headingRows.Add(data);
            CalculateRowWidth();
        }

        public void Truncate(int widthLimit)
        {
            // Assumption only one row
            String textData = m_headingRows[0];
            
            if (textData.Length > widthLimit)
            {
                textData = textData.Substring(0, widthLimit);
            }
            else
            {
                textData = textData.PadRight(widthLimit);
            }

            m_headingRows.Clear();
            m_headingRows.Add(textData);
            CalculateRowWidth();
        }
    }
}
