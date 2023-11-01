using QEAMApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QEAMApp
{
    /// <summary>
    /// Interaction logic for ControlCenter.xaml
    /// </summary>
    public partial class ControlCenter : Window
    {
        private ApiService API;
        public ControlCenter()
        {
            InitializeComponent();
            API ??= new ApiService();
            FetchStatus();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async Task FetchStatus()
        {
            (bool IsConnected, string? hostAddress, int? port) = await API.GetServerInfo();
            if (IsConnected) StatusIndicator.Fill = Brushes.LimeGreen;
            else StatusIndicator.Fill = Brushes.Red;

            if (hostAddress is null && port is null) return;
            IPAddress.Text = (String.IsNullOrEmpty(hostAddress)) ? "Server Connection Failed" : $"{hostAddress}:{port}";
        }
    }
}
