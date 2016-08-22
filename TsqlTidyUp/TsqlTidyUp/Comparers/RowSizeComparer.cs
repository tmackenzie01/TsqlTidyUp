using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsqlTidyUp
{
    public class RowWidthComparer : IComparer<FieldBase>
    {
        public int Compare(FieldBase x, FieldBase y)
        {
            return y.RowWidth.CompareTo(x.RowWidth);
        }
    }
}
