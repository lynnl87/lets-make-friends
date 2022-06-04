using LetsMakeFriends.Classes;
using System.Windows;

namespace LetsMakeFriends
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            StateManager.Instance.ReloadConfig();
            StateManager.Instance.RegisterKey(KeyInformation.VK_F12, KeyInformation.MOD_CTRL);
        }
    }
}
