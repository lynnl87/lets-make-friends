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
        private readonly Random _random;

        /// <summary>
        /// Our handle for  accessing message pump.
        /// </summary>
        private readonly IntPtr _windowHandle;
        /// <summary>
        /// Contains a dictionary of assigned hotkey id's and their callbacks.
        /// </summary>
        private readonly Dictionary<int, HotkeyPressedCallBack> _hotkeyDict;

        /// <summary>
        /// Initializes an instance of a HotkeyHelper class.
        /// </summary>
        public HotkeyHelper()
        {
            _random = new Random();
            _hotkeyDict = new Dictionary<int, HotkeyPressedCallBack>();
            _windowHandle = new WindowInteropHelper(App.Current.MainWindow).EnsureHandle();
            _source = HwndSource.FromHwnd(_windowHandle);
            _source.AddHook(HwndHook);
        }

        /// <summary>
        /// Finalizes an instance of a HotkeyHelper class.
        /// </summary>
        ~HotkeyHelper()
        {
            Dispose(false);
        }
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
            while (retryCount > -1)
            {
                // Add a check for the final retry.
                if (retryCount == 0)
                {
                    // Should not have happend this fast.
                    throw new Exception("Key already exists");
                }

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
            }

            if (!NativeHelper.RegisterHotKey(_windowHandle, keyId, modifiers, keyValue))
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
            NativeHelper.UnregisterHotKey(_windowHandle, hotkeyId);
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
