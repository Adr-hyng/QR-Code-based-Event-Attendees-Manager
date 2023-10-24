using QEAMApp.Core;
using QEAMApp.MVVM.View;
using QEAMApp.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QEAMApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Spaghetti code, please use User Control, and ViewModel for EventHandling Soon :D
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextBox QRCodePrompt;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            this.QRCodePrompt ??= Utility.FindVisualChild<TextBox>(MainView, "QRCodeTextBox");
            this.QRCodePrompt.Focus();
        }

        private void FocusTextBoxOnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                this.QRCodePrompt.Focus();
            }
        }
    }
}
