using QEAMApp.Core;
using QEAMApp.MVVM.Model;
using QEAMApp.MVVM.View;
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
        private readonly NavigationStore _mainNavigationStore;
        private readonly NavigationStore _controlNavigationStore;
        private ControlCenter? _secondWindow;
        private readonly ApiService _apiService;

        public App()
        {
            _apiService = new ApiService();
            _mainNavigationStore = new NavigationStore();
            _controlNavigationStore = new NavigationStore();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Default Page
            _mainNavigationStore.CurrentViewModel = GetIdleScreenViewModel();
            _controlNavigationStore.CurrentViewModel = GetControlCenterViewModel();

            MainWindow = new MainWindow()
            {
                DataContext = new MainViewModel(_mainNavigationStore)
            };
            MainWindow.Closing += MainWindow_Closing!; // Attach the Closing event handler
            MainWindow.Show();



            _secondWindow = new ControlCenter()
            {
                DataContext = new MainViewModel(_controlNavigationStore)
            };
            _secondWindow.Show();

            base.OnStartup(e);
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _secondWindow.Close(); // Close the secondWindow when the MainWindow is closing
        }




        // Screens
        private ControlScreenViewModel GetControlCenterViewModel()
        {
            return new ControlScreenViewModel(new NavigationService(_controlNavigationStore, null), _apiService);
        }

        private IdleScreenViewModel GetIdleScreenViewModel()
        {
            return new IdleScreenViewModel( new NavigationService(_mainNavigationStore, GetFoundScreenViewModel), _apiService);
        }

        private UserFoundScreenViewModel GetFoundScreenViewModel()
        {
            return new UserFoundScreenViewModel(new NavigationService(_mainNavigationStore, GetProfileScreenViewModel));
        }

        private ProfileScreenViewModel GetProfileScreenViewModel()
        {
            return new ProfileScreenViewModel(new NavigationService(_mainNavigationStore, GetIdleScreenViewModel));
        }
    }
}
