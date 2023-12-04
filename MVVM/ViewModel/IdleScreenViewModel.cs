using QEAMApp.Core;
using QEAMApp.MVVM.CEventHandler;
using QEAMApp.MVVM.Command;
using QEAMApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QEAMApp.MVVM.ViewModel
{
    internal class IdleScreenViewModel: ViewModelBase
    {
        private readonly ApiService _apiService;
        NavigationService _navigationService;

        private Timer _timer;

        private String _welcomeRibbonSource;
        public String WelcomeRibbonSource
        {
            get
            {
                return _welcomeRibbonSource;
            }
            set
            {
                _welcomeRibbonSource = value;
                OnPropertyChanged(nameof(WelcomeRibbonSource));
            }
        }
        private byte _opacity;
        public byte Opacity
        {
            get
            {
                return _opacity;
            }
        }
        private bool _isReadOnly;
        public bool IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
            set
            {
                _isReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }
        private string _uniqueIdentifier;
        public string UniqueIdentifier
        {
            get
            {
                return _uniqueIdentifier;
            }
            set
            {
                _uniqueIdentifier = value;
                OnPropertyChanged(nameof(UniqueIdentifier));
            }
        }

        public ICommand ScanningCommand { get; }

        public IdleScreenViewModel(NavigationService GoToFoundScreenNavigation, ApiService InstanceAPI)
        {
            _apiService = InstanceAPI;
            _navigationService = GoToFoundScreenNavigation;
            WelcomeRibbonSource = "/QEAMApp;component/Images/noon_ribbon.png";
            _apiService.PropertyChanged += ApiService_PropertyChanged;

            ScanningCommand = new UserFoundCommand(GoToFoundScreenNavigation, this, InstanceAPI);

            _timer = new Timer(1000 * 30); // Every x Seconds
            _timer.AutoReset = true;
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        private void ApiService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ApiService.DebugMode))
            {
                _opacity = Convert.ToByte(_apiService.DebugMode);
                OnPropertyChanged(nameof(Opacity));
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateRibbonScheduler();
        }

        private void StopScheduler()
        {
            _timer.Stop();
        }

        private void UpdateRibbonScheduler()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Your code here
                DateTime currentDateTime = DateTime.Now;
                // 8 AM - 11:59 AM
                if (DayContent.InTimeBound(currentDateTime, new TimeSpan(8, 0, 0), new TimeSpan(11, 59, 59)))
                {
                    WelcomeRibbonSource = "/QEAMApp;component/Images/morning_ribbon.png";
                }
                // 12:00 PM - 5:59 PM
                else if (DayContent.InTimeBound(currentDateTime, new TimeSpan(12, 0, 0), new TimeSpan(17, 59, 59)))
                {
                    WelcomeRibbonSource = "/QEAMApp;component/Images/noon_ribbon.png";
                } 
                // 6:00 PM - 11:59 PM
                else if (DayContent.InTimeBound(currentDateTime, new TimeSpan(18, 0, 0), new TimeSpan(23, 59, 59)))
                {
                    WelcomeRibbonSource = "/QEAMApp;component/Images/evening_ribbon.png";
                }
            });
        } 
    }
}
