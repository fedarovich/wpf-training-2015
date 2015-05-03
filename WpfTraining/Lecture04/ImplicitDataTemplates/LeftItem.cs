using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplicitDataTemplates
{
    public class LeftItem
    {
        private static int counter;

        public LeftItem()
        {
            Index = ++counter;
        }

        public int Index { get; private set; }
    }
}
