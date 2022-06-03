using System;
using System.Collections.Generic;
using System.Windows.Interop;
using static LetsMakeFriends.Classes.Helpers.Delegates;

namespace LetsMakeFriends.Classes.Helpers
{
    /// <summary>
    /// Point of this class is to make interfacing with windows hotkeys easier.
    /// </summary>
    public class HotkeyHelper : IDisposable
    {
        // To detect redundant calls
        private bool _disposedValue;

        /// <summary>
        /// handle to our window.
        /// </summary>
        private HwndSource _source;

        /// <summary>
        /// Our random generator for key ids.
        /// </summary>
        private Random _random;

        /// <summary>
        /// Our interop helper for accessing message pump.
        /// </summary>
        private WindowInteropHelper _windowInteropHelper;

        /// <summary>
        /// Contains a dictionary of assigned hotkey id's and their callbacks.
        /// </summary>
        private Dictionary<int, HotkeyPressedCallBack> _hotkeyDict;

        /// <summary>
        /// Initializes an instance of a HotkeyHelper class.
        /// </summary>
        /// <param name="ourWindow">The {Window} window of our running application</param>
        public HotkeyHelper(System.Windows.Window ourWindow)
        {
            _random = new Random();
            _hotkeyDict = new Dictionary<int, HotkeyPressedCallBack>();
            _windowInteropHelper = new WindowInteropHelper(ourWindow);
            _source = HwndSource.FromHwnd(_windowInteropHelper.Handle);
            _source.AddHook(HwndHook);
        }

        /// <summary>
        /// Finalizes an instance of a HotkeyHelper class.
        /// </summary>
        ~HotkeyHelper() => Dispose(false);

        /// <summary>
        /// Handles clearing up some internal resources.
        /// </summary>
        public void ClearInteralDictionary()
        {
            foreach (var kvp in _hotkeyDict)
            {
                UnregisterHotKey(kvp.Key);
            }

            _hotkeyDict.Clear();
        }

        /// <summary>
        /// Public implementation of Dispose pattern callable by consumers.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Protected implenetation of Dispose Pattern.
        /// </summary>
        /// <param name="disposing">True if we need to clear up unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                }

                _source.RemoveHook(HwndHook);
                _source = null;
                ClearInteralDictionary();
                _disposedValue = true;
            }
        }

        /// <summary>
        /// Handles registering a hotkey to be monitored.
        /// </summary>
        /// <param name="keyValue">Key Value</param>
        /// <param name="modifiers">Ctrl/Alt/Shift modifiers.</param>
        /// <param name="callback">Function to be called on hotkey pressed.</param>
        /// <returns>An assigned int for the hotkey.</returns>
        public int RegisterHotKey(uint keyValue, uint modifiers, HotkeyPressedCallBack callback)
        {
            int keyId = -1;
            int retryCount = 5;
            while (retryCount > 0)
            {
                keyId = _random.Next();
                if (_hotkeyDict.ContainsKey(keyId))
                {
                    retryCount--;
                    continue;
                }
                else
                {
                    break;
                }

                // Add a check for the final retry.
                if (retryCount == 1)
                {
                    // Should not have happend this fast.
                    throw new Exception("Key already exists");
                }
            }

            if (!NativeHelper.RegisterHotKey(_windowInteropHelper.Handle, keyId, modifiers, keyValue))
            {
                // handle error
                throw new Exception("Error registering for hotkey.");
            }

            _hotkeyDict[keyId] = callback;
            return keyId;
        }

        /// <summary>
        /// Unregisters our known hotkeys.
        /// </summary>
        /// <param name="hotkeyId">Hotkey id to remove.</param>
        public void UnregisterHotKey(int hotkeyId)
        {
            NativeHelper.UnregisterHotKey(_windowInteropHelper.Handle, hotkeyId);
        }

        /// <summary>
        /// Standard windows message pump.
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <param name="handled"></param>
        /// <returns>IntPtr.Zero per documentation.</returns>
        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            switch (msg)
            {
                case WM_HOTKEY:
                    if (_hotkeyDict.ContainsKey(wParam.ToInt32()))
                    {
                        _hotkeyDict[wParam.ToInt32()].Invoke(wParam.ToInt32());
                        handled = true;
                    }
                    break;
            }

            return IntPtr.Zero;
        }
    }
}
