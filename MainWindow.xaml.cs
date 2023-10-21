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
using System.Windows.Threading;

namespace QEAMApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var words = new[] { "Kickstart your adventure! Scan your ID’s QR code here!", "Ready, Set, Connect! Begin with a quick QR scan!", "Your journey starts with a simple scan! QR code at the ready!" };
            double speed = 0.5 ;
            var textBlocks = new List<TextBlock>();

            foreach (var word in words)
            {
                var textBlock = new TextBlock
                {
                    Text = word,
                    FontSize = 25,
                    RenderTransform = new TranslateTransform(),
                    Opacity = 0,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center,
                    Foreground = Brushes.White
                };

                textBlocks.Add(textBlock);
                MainGrid.Children.Add(textBlock); // Add the TextBlock to the MainGrid
            }

            var textBlocksCopy = new List<TextBlock>(textBlocks); // Create a copy of the list
            StartNextAnimation(textBlocks, textBlocksCopy, speed, 0);
        }

        private void StartNextAnimation(List<TextBlock> textBlocks, List<TextBlock> textBlocksCopy, double speed, int index)
        {
            if (textBlocksCopy.Count <= 0)
            {
                var words = new[] { "Kickstart your adventure! Scan your ID’s QR code here!", "Ready, Set, Connect! Begin with a quick QR scan!", "Your journey starts with a simple scan! QR code at the ready!"};

                foreach (var word in words)
                {
                    var textBlock1 = new TextBlock
                    {
                        Text = word,
                        FontSize = 25,
                        RenderTransform = new TranslateTransform(),
                        Opacity = 0,
                        TextWrapping = TextWrapping.Wrap,
                        TextAlignment = TextAlignment.Center,
                        Foreground = Brushes.White
                    };

                    textBlocks.Add(textBlock1);
                    textBlocksCopy.Add(textBlock1);
                    MainGrid.Children.Add(textBlock1); // Add the TextBlock to the MainGrid
                }

                index = 0;
            }
            var textBlock = textBlocksCopy[index];

            var storyboard1 = new Storyboard();
            var storyboard2 = new Storyboard();

            var moveAnimation1 = new DoubleAnimationUsingKeyFrames();
            moveAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(400, KeyTime.FromPercent(0)));
            moveAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(200, KeyTime.FromPercent(1)));
            Storyboard.SetTarget(moveAnimation1, textBlock);
            Storyboard.SetTargetProperty(moveAnimation1, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            var moveAnimation2 = new DoubleAnimationUsingKeyFrames();
            moveAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(200, KeyTime.FromPercent(0)));
            moveAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(1)));
            Storyboard.SetTarget(moveAnimation2, textBlock);
            Storyboard.SetTargetProperty(moveAnimation2, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            var opacityAnimation1 = new DoubleAnimationUsingKeyFrames();
            opacityAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)));
            opacityAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.25)));
            opacityAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.9)));
            opacityAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(1)));
            Storyboard.SetTarget(opacityAnimation1, textBlock);
            Storyboard.SetTargetProperty(opacityAnimation1, new PropertyPath("Opacity"));

            var opacityAnimation2 = new DoubleAnimationUsingKeyFrames();
            opacityAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(0)));
            opacityAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.1)));
            opacityAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.75)));
            opacityAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(1)));
            Storyboard.SetTarget(opacityAnimation2, textBlock);
            Storyboard.SetTargetProperty(opacityAnimation2, new PropertyPath("Opacity"));

            storyboard1.Children.Add(moveAnimation1);
            storyboard1.Children.Add(opacityAnimation1); // Add opacityAnimation to storyboard1 only
            storyboard2.Children.Add(opacityAnimation2); // Add opacityAnimation to storyboard1 only
            storyboard2.Children.Add(moveAnimation2);

            storyboard1.Duration = TimeSpan.FromSeconds(1);
            storyboard2.BeginTime = TimeSpan.FromSeconds(1 + 2); // 2 seconds delay
            storyboard2.Duration = TimeSpan.FromSeconds(1);

            storyboard1.Completed += (s, e) =>
            {
                textBlock.BeginStoryboard(storyboard2);
            };

            storyboard2.Completed += (s, e) =>
            {
                MainGrid.Children.Remove(textBlock);
                textBlocks.Remove(textBlock);
                textBlocksCopy.Remove(textBlock);

                if (textBlocksCopy.Count > 0)
                {
                    index = (index + 1) % textBlocksCopy.Count;
                }
                StartNextAnimation(textBlocks, textBlocksCopy, speed, index);
            };

            textBlock.BeginStoryboard(storyboard1);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
