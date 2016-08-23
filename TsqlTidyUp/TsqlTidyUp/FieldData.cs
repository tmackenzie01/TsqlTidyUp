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
    }
}
