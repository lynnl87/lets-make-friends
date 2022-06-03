using LetsMakeFriends.Classes.Helpers;
using System;
using System.Windows;
using System.Windows.Threading;

namespace LetsMakeFriends
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Contains an instance of our hotkey helper.
        /// </summary>
        private HotkeyHelper _hotkeyHelper;

        /// <summary>
        /// The key value for F12.
        /// </summary>
        private const uint VK_F12 = 0x7B;

        /// <summary>
        /// The key value for the control key.
        /// </summary>
        private const uint MOD_CTRL = 0x0002;

        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when the window is loaded.
        /// </summary>
        /// <param name="sender">Originator of the event.</param>
        /// <param name="e">{RoutedEventArgs} event arguments.</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _hotkeyHelper = new HotkeyHelper(this);
            _hotkeyHelper.RegisterHotKey(VK_F12, MOD_CTRL, OnCtrlF12KeyPress);
        }

        /// <summary>
        /// Called when our hotkey is pressed.
        /// </summary>
        /// <param name="keyId">KeyID that was pressed.</param>
        private async void OnCtrlF12KeyPress(int keyId)
        {
            AdviceResult result = await AdviceHelper.GetAdviceAsync();
            System.Windows.Clipboard.SetText(result.Slip.AdviceMessage);
            await Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.ContextIdle, new Action(() =>
            {
                RTB_Output.AppendText(string.Format("Advice #{0} - {1}\n", result.Slip.Id, result.Slip.AdviceMessage));
            }));
        }

        /// <summary>
        /// Called when the window is closing.
        /// </summary>
        /// <param name="sender">Originator of the event.</param>
        /// <param name="e">{CancelEventArgs} event arguments.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _hotkeyHelper.Dispose();
        }
    }
}
