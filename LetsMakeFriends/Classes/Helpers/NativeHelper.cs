using System;
using System.Runtime.InteropServices;

namespace LetsMakeFriends.Classes.Helpers
{
    /// <summary>
    /// Purpose of the class is to contain pinvoked functions.
    /// </summary>
    public static class NativeHelper
    {

        [DllImport("User32.dll")]
        public static extern bool RegisterHotKey(
            [In] IntPtr hWnd,
            [In] int id,
            [In] uint fsModifiers,
            [In] uint vk);

        [DllImport("User32.dll")]
        public static extern bool UnregisterHotKey(
            [In] IntPtr hWnd,
            [In] int id);
    }
}
