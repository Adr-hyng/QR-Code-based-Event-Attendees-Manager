using QEAMApp.Core;
using QEAMApp.MVVM.ViewModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QEAMApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for IdleScreenView.xaml
    /// </summary>
    public partial class IdleScreenView : UserControl
    {
        public IdleScreenView()
        {
            InitializeComponent();
        }

        private async void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            // When Enter is hit in Textbox
            if (e.Key == Key.Return)
            {
                QRCodeTextBox.IsReadOnly = true;
                await Task.Delay(1000);
                QRCodeTextBox.Text = "";
                QRCodeTextBox.IsReadOnly = false;
            }
        }

                private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            QRCodeTextBox.Focus();
        }

    }
}
