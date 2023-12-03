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

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            QRCodeTextBox.Focus();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            QRCodeTextBox.Focus();
            AnimationManager.AnimateRectangle(ScannerRect, 130, 350);
            await Task.Delay(200);
            AnimationManager.FadeInWelcomeRibbon(welcomeRibbon, 684);
        }

        private async void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
