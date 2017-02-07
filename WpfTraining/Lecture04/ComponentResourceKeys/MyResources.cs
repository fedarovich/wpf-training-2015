using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ComponentResourceKeys
{
    public static class MyResources
    {
        public static ResourceKey Brush1Key => new ComponentResourceKey(typeof(MyResources), "Brush1");

        public static ResourceKey Brush2Key => new ComponentResourceKey(typeof(MyResources), "Brush2");

        #region CreateKey function (C#5 or later)

        public static ResourceKey Brush3Key => CreateKey();

        public static ResourceKey Brush4Key => CreateKey();

        private static ResourceKey CreateKey([CallerMemberName] string name = null)
        {
            return new ComponentResourceKey(typeof(MyResources), name.Substring(0, name.Length - 3));
        }

        #endregion
    }
}
