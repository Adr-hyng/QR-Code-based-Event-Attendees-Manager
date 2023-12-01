using QEAMApp.Core;
using QEAMApp.MVVM.Command;
using QEAMApp.MVVM.Model;
using System;
using System.Collections.Generic;
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
		private string _ipAddress = "192.168.1.10:5000";

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

		public ControlScreenViewModel(NavigationService _, ApiService InstanceAPI)
        {
            BrushConverter brushConverter = new BrushConverter();
            IndicatorColor = (Brush) brushConverter.ConvertFrom("#FFE9E9E9")!;
            SubmitAddressCommand = new DistributeAddressCommand(InstanceAPI, this);
        }
    }
}
