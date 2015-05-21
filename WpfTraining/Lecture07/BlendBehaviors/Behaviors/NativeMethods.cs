using System.Runtime.InteropServices;
using System.Text;

namespace BlendBehaviors.Behaviors
{
    /// <summary>
    /// Provides native methods.
    /// </summary>
    internal static class NativeMethods
    {
        // ReSharper disable InconsistentNaming

        internal enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }

        // ReSharper restore InconsistentNaming

        [DllImport("user32.dll")]
        internal static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)] 
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [DllImport("user32.dll")]
        internal static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        internal static extern uint MapVirtualKey(uint uCode, MapType uMapType);
    }
}
