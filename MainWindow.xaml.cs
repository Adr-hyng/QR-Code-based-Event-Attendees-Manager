using QEAMApp.Core;
using QEAMApp.MVVM.CEventHandler;
using QEAMApp.MVVM.Model;
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
        public TextBox _QRCodePrompt;
        private MainViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _QRCodePrompt = Utility.FindVisualChild<TextBox>(CurrentView, "QRCodeTextBox")!;

            _viewModel = (MainViewModel)DataContext;
            _viewModel.ShowSnackBarEvent += ShowSnackBar;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            _QRCodePrompt.Focus();

        }
        private void ShowSnackBar(object sender, ShowSnackBarEventArgs e)
        {
            AnimationManager.ShowSnackBar(SnackBarPopUp, 680, 630, e.Duration); // Adjust the parameters as needed
        }
    }
}
