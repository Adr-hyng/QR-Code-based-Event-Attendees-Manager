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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QEAMApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {

        private Rectangle ScannerRect;

        public HomeView()
        {
            InitializeComponent();

            MainViewModel mainViewModel = new MainViewModel();
            DataContext = mainViewModel;
        }

        private void ImportButton_MouseEnter(object sender, MouseEventArgs e)
        {
            double buttonWidth = ImportButton.Width;
            double rectangleWidth = buttonWidth - 6;

            DoubleAnimation animation = new(rectangleWidth, TimeSpan.FromSeconds(0.1));
            ImportBoxScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            // Trigger when Import button is clicked
        }

        private void ImportButton_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation animation = new(1, TimeSpan.FromSeconds(0.1));
            ImportBoxScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
        }

        private void ExportButton_MouseEnter(object sender, MouseEventArgs e)
        {
            double buttonWidth = ExportButton.Width;
            double rectangleWidth = buttonWidth - 6;

            DoubleAnimation animation = new(rectangleWidth, TimeSpan.FromSeconds(0.1));
            ExportBoxScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            // Trigger when Export button clicks
        }

        private void ExportButton_MouseLeave(object sender, MouseEventArgs e)
        {
            DoubleAnimation animation = new(1, TimeSpan.FromSeconds(0.1));
            ExportBoxScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, animation);
        }

        private async void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            // When Enter is hit in Textbox
            if (e.Key == Key.Return)
            {
                string userInput = QRCodeTextBox.Text;

                this.ScannerRect ??= Utility.FindVisualChild<Rectangle>(BaseView, "ScannerRect");
                if (userInput == "Cat")
                {
                    this.ScannerRect.Visibility = Visibility.Visible;
                    Utility.AnimateRectangle(this.ScannerRect, 20, 300);
                    QRCodeTextBox.Visibility = Visibility.Hidden;
                    await Task.Delay(2 * 1000);

                    // Trigger the CardViewCommand down below
                    MainViewModel mainViewModel = (MainViewModel)DataContext;
                    mainViewModel.CardViewCommand.Execute(null);

                }
                else if (userInput == "DevOn" || userInput == "b8381b3e-8ae5-5431-ab37-d25e68769ef1")
                {
                    // Execute your desired action here
                    // Replace the comment with your code
                    QRCodeTextBox.Opacity = 1;
                    ImportButton.Visibility = Visibility.Visible;
                    ExportButton.Visibility = Visibility.Visible;
                }

                else if (userInput == "DevOff" || userInput == "b8381b3e-8ae5-5431-ab37-d25e68769ef1")
                {
                    // Execute your desired action here
                    // Replace the comment with your code
                    QRCodeTextBox.Opacity = 0;
                    ImportButton.Visibility = Visibility.Hidden;
                    ExportButton.Visibility = Visibility.Hidden;
                }

                QRCodeTextBox.Text = "";
            }
        }
    }
}
