using QEAMApp.Core;
using QEAMApp.MVVM.Command;
using QEAMApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace QEAMApp.MVVM.ViewModel
{

    internal class ControlScreenViewModel: ViewModelBase
    {
        public ICommand SubmitAddressCommand { get; }
        public ICommand ExportCommand { get; }
        public NavigationService _navigationService;
        private readonly ApiService _apiService;
        private string _ipAddress = "127.0.0.1:5000";

		private Brush _indicatorColor;
		public Brush IndicatorColor
		{
			get
			{
				return _indicatorColor;
			}
			set
			{
                _indicatorColor = value;
				OnPropertyChanged(nameof(IndicatorColor));
			}
		}
		public string IpAddress
		{
			get
			{
				return _ipAddress;
			}
			set
			{
				_ipAddress = value;
				OnPropertyChanged(nameof(IpAddress));
			}
		}

        public bool DebugMode
        {
            get
            {
                return _apiService.DebugMode;
            }
            set
            {
                _apiService.DebugMode = value;
                OnPropertyChanged(nameof(_apiService.DebugMode));
            }
        }

        public ControlScreenViewModel(NavigationService navigationService, ApiService InstanceAPI)
        {
            _apiService = InstanceAPI;
            _navigationService = navigationService;
            BrushConverter brushConverter = new();
            IndicatorColor = (Brush) brushConverter.ConvertFrom("#FFE9E9E9")!;
            SubmitAddressCommand = new DistributeAddressCommand(InstanceAPI, this);
            ExportCommand = new ExportLogEntriesCommand(InstanceAPI);
        }
    }
}
