using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsqlTidyUp
{
    public class DisplayIndexComparer : IComparer<FieldBase>
    {
        public int Compare(FieldBase x, FieldBase y)
        {
            return x.DisplayIndex.CompareTo(y.DisplayIndex);
        }
    }
}
