using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Client.ViewModel;

namespace Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private NavigationStore _navigationStore;
        protected override void OnStartup(StartupEventArgs e)
        {
            _navigationStore = new NavigationStore();

            _navigationStore.CurrentViewModel = new LoginWindowViewModel(_navigationStore);

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            _navigationStore.Client.Stop();
        }
    }
}