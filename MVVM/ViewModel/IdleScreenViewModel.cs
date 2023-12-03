using QEAMApp.Core;
using QEAMApp.MVVM.Command;
using QEAMApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace QEAMApp.MVVM.ViewModel
{
    internal class IdleScreenViewModel: ViewModelBase
    {
        private ApiService _apiService;
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
            _apiService.PropertyChanged += ApiService_PropertyChanged;

            ScanningCommand = new UserFoundCommand(GoToFoundScreenNavigation, this, InstanceAPI);
        }

        private void ApiService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ApiService.DebugMode))
            {
                _opacity = Convert.ToByte(_apiService.DebugMode);
                OnPropertyChanged(nameof(Opacity));
            }
        }
    }
}
