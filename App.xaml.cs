using QEAMApp.Core;
using QEAMApp.MVVM.Model;
using QEAMApp.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace QEAMApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore;

        public App()
        {
            _navigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Default Page
            _navigationStore.CurrentViewModel = GetIdleScreenViewModel();

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_navigationStore)
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

        // Navigations
        private IdleScreenViewModel GetIdleScreenViewModel()
        {
            return new IdleScreenViewModel( new NavigationService(_navigationStore, GetFoundScreenViewModel));
        }

        private UserFoundScreenViewModel GetFoundScreenViewModel()
        {
            return new UserFoundScreenViewModel(new NavigationService(_navigationStore, GetProfileScreenViewModel));
        }

        private ProfileScreenViewModel GetProfileScreenViewModel()
        {
            return new ProfileScreenViewModel(new NavigationService(_navigationStore, GetIdleScreenViewModel));
        }
    }
}
