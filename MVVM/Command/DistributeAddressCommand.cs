using QEAMApp.Core;
using QEAMApp.MVVM.Model;
using QEAMApp.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Navigation;

namespace QEAMApp.MVVM.Command
{
    internal class DistributeAddressCommand : CommandBase
    {
        private readonly ControlScreenViewModel _controlViewModel;
        public DistributeAddressCommand(ControlScreenViewModel controlViewModel)
        {
            _controlViewModel = controlViewModel;
        }
        static bool ValidateIPAddress(string input, out string? ipAddress, out int port)
        {
            string ipAddressPattern = @"^(?:\d{1,3}\.){3}\d{1,3}:\d+$";
            if (Regex.IsMatch(input, ipAddressPattern))
            {
                string[] parts = input.Split(':');
                ipAddress = parts[0];
                port = int.Parse(parts[1]);
                if (port >= 1 && port <= 65535)
                {
                    return true;
                }
            }
            ipAddress = null;
            port = 0;
            return false;
        }

        public override async void Execute(object? parameter)
        {
            if (parameter is string IpAddress)
            {
                if (String.IsNullOrEmpty(IpAddress) || String.IsNullOrWhiteSpace(IpAddress)) return;
                bool IsValid = ValidateIPAddress(IpAddress, out string? ipAddress, out int port);
                if (String.IsNullOrEmpty(ipAddress) || String.IsNullOrEmpty(port.ToString()) || !IsValid)
                {
                    SystemSounds.Exclamation.Play();
                    (_controlViewModel._navigationService._navigationStore.RootViewModel as MainViewModel)!.ShowSnackBar("Invalid IP Address", 3, false);
                    _controlViewModel.IndicatorColor = Brushes.Red;
                    return;
                }
                _controlViewModel._apiService.ChangeDefaultGateWay($"{ipAddress}:{port}");
                bool IsConnected = await _controlViewModel._apiService.GetServerInfo(ipAddress, port);
                if (!IsConnected)
                {
                    SystemSounds.Exclamation.Play();
                    (_controlViewModel._navigationService._navigationStore.RootViewModel as MainViewModel)!.ShowSnackBar("No Connection Found", 3, false);
                    _controlViewModel.IndicatorColor = Brushes.Red;
                    return;
                }
                (_controlViewModel._navigationService._navigationStore.RootViewModel as MainViewModel)!.ShowSnackBar("Connection Found", 3);
                _controlViewModel.IndicatorColor = Brushes.LimeGreen;
            }
        }
    }
}
