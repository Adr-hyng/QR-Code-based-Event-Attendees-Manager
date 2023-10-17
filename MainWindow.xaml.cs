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

            var words = new[] { "beautiful", "maintainable", "perfect 👌", "Cat" };
            double speed = 1.5;
            var textBlocks = new List<TextBlock>();

            foreach (var word in words)
            {
                var textBlock = new TextBlock
                {
                    Text = word,
                    FontSize = 25,
                    RenderTransform = new TranslateTransform(),
                    Opacity = 0,
                    Margin = new Thickness(10, 10, 0, 0) // Set the position of the TextBlock
                };

                textBlocks.Add(textBlock);
                MainGrid.Children.Add(textBlock); // Add the TextBlock to the MainGrid
            }

            StartNextAnimation(textBlocks, speed, 0);
        }

        private void StartNextAnimation(List<TextBlock> textBlocks, double speed, int index)
        {
            var textBlock = textBlocks[index];

            var storyboard = new Storyboard();

            var moveAnimation = new DoubleAnimationUsingKeyFrames();
            moveAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(50, KeyTime.FromPercent(0)));
            moveAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.2)));
            moveAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(-25, KeyTime.FromPercent(1)));
            Storyboard.SetTarget(moveAnimation, textBlock);
            Storyboard.SetTargetProperty(moveAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            var opacityAnimation = new DoubleAnimationUsingKeyFrames();
            opacityAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)));
            opacityAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(0.2)));
            opacityAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(0.8)));
            opacityAnimation.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(1)));
            Storyboard.SetTarget(opacityAnimation, textBlock);
            Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath("Opacity"));

            storyboard.Children.Add(moveAnimation);
            storyboard.Children.Add(opacityAnimation);

            storyboard.BeginTime = TimeSpan.FromSeconds(index *speed);
            storyboard.Duration = TimeSpan.FromSeconds(speed);

            storyboard.Completed += (s, e) =>
            {
                index = (index + 1) % textBlocks.Count;
                StartNextAnimation(textBlocks, speed, index);
                MainGrid.Children.Remove(textBlock);
            };

            textBlock.BeginStoryboard(storyboard);
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
