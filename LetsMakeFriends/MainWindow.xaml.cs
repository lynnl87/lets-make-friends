using System.Windows;

namespace LetsMakeFriends
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Scrolls to the end when text is changed.
        /// </summary>
        /// <param name="sender">Control that started the event.</param>
        /// <param name="e">Text change arguments.</param>
        private void RTB_Output_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            RTB_Output.ScrollToEnd();
        }
    }
}
