using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComponentResourceKeys
{
    public static class MyResources
    {
        public static ResourceKey Brush1Key
        {
            get
            {
                return new ComponentResourceKey(typeof(MyResources), "Brush1");
            }
        }

        public static ResourceKey Brush2Key
        {
            get
            {
                return new ComponentResourceKey(typeof(MyResources), "Brush2");
            }
        }
    }
}
