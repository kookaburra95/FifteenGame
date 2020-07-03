using System.Windows;

namespace Fifteen
{
    public partial class App : Application
    {
        System.Threading.Mutex mutex;
        private void App_Startup(object sender, StartupEventArgs e)
        {
            bool createdNew;
            string mutName = "Application";
            mutex = new System.Threading.Mutex(true, mutName, out createdNew);
            if (!createdNew)
            {
                MessageBox.Show("Application is already running!");
                this.Shutdown();
            }
        }
    }
}
