using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplicitDataTemplates
{
    public class RightItem
    {
        private static int counter;

        public RightItem()
        {
            Index = ++counter;
        }

        public int Index { get; private set; }
    }
}
