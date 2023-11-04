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
        private readonly ApiService _apiService;
        private readonly ControlScreenViewModel _controlViewModel;
        public DistributeAddressCommand(ApiService API, ControlScreenViewModel controlViewModel)
        {
            _apiService = API;
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

        private void NotConnected(string Response = "Connection Not Found", string CodeResponse = "DB 404")
        {
            SystemSounds.Exclamation.Play();
            AutoClosingMessageBox.Show(Response, CodeResponse, 5000);
            _controlViewModel.IndicatorColor = Brushes.Red;
            return;
        }

        private void Connected()
        {
            AutoClosingMessageBox.Show("Connection Found", "DB 200", 5000);
            _controlViewModel.IndicatorColor = Brushes.LimeGreen;
            return;
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
                    AutoClosingMessageBox.Show("Invalid IP Address", "Server 404", 5000);
                    _controlViewModel.IndicatorColor = Brushes.Red;
                    return;
                }
                _apiService.ChangeDefaultGateWay($"{ipAddress}:{port}");
                bool IsConnected = await _apiService.GetServerInfo(ipAddress, port);
                if (!IsConnected)
                {
                    NotConnected();
                    return;
                }
                Connected();
            }
        }
    }
}
