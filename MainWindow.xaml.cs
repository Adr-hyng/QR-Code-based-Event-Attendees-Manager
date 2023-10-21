using System;
using System.Collections.Generic;
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
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string[] words = new[]
        {
        "Kickstart your adventure! Scan your ID’s QR code here!",
        "Ready, Set, Connect! Begin with a quick QR scan!",
        "Your journey starts with a simple scan! QR code at the ready!"
    };

        private readonly double speed = 1;
        private readonly Int16 duration = 1;
        private List<TextBlock> textBlocks;
        private Int16 index = 0;

        public MainWindow()
        {
            InitializeComponent();

            this.textBlocks = new List<TextBlock>();

            StartAnimation();
        }

        private void StartAnimation()
        {
            var textBlock = CreateAnimatedTextBlock(this.words[this.index++ % this.words.Length]);
            this.textBlocks.Add(textBlock);
            MainGrid.Children.Add(textBlock);
            AnimateTextBlock(textBlock, 400, 0, this.speed, this.duration);
        }

        private void AnimateTextBlock(TextBlock textBlock, Int16 srcY, Int16 dstY, double speed = 1, double duration = 1, bool IsPrimary = true)
        {
            var storyboard1 = new Storyboard();
            var storyboard2 = new Storyboard();

            var moveAnimation1 = new DoubleAnimationUsingKeyFrames();
            moveAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(srcY, KeyTime.FromPercent(0)));
            moveAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(srcY - 200, KeyTime.FromPercent(1)));
            Storyboard.SetTarget(moveAnimation1, textBlock);
            Storyboard.SetTargetProperty(moveAnimation1, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            var moveAnimation2 = new DoubleAnimationUsingKeyFrames();
            moveAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(srcY - 200, KeyTime.FromPercent(0)));
            moveAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(dstY, KeyTime.FromPercent(1)));
            Storyboard.SetTarget(moveAnimation2, textBlock);
            Storyboard.SetTargetProperty(moveAnimation2, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            var opacityAnimation1 = new DoubleAnimationUsingKeyFrames();
            opacityAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)));
            opacityAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.25)));
            opacityAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.9)));
            opacityAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(1))); // only 1
            Storyboard.SetTarget(opacityAnimation1, textBlock);
            Storyboard.SetTargetProperty(opacityAnimation1, new PropertyPath("Opacity"));

            var opacityAnimation2 = new DoubleAnimationUsingKeyFrames();
            opacityAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(0))); // only 1
            opacityAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.1)));
            opacityAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.75)));
            opacityAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(1)));
            Storyboard.SetTarget(opacityAnimation2, textBlock);
            Storyboard.SetTargetProperty(opacityAnimation2, new PropertyPath("Opacity"));

            storyboard1.Children.Add(moveAnimation1);
            storyboard1.Children.Add(opacityAnimation1);
            storyboard2.Children.Add(opacityAnimation2);
            storyboard2.Children.Add(moveAnimation2);

            storyboard1.Duration = TimeSpan.FromSeconds(speed);
            if(!IsPrimary)
            {
                storyboard1.BeginTime = TimeSpan.FromSeconds(speed + duration);
            }
            storyboard2.BeginTime = TimeSpan.FromSeconds(speed + duration); // 2 seconds delay
            storyboard2.Duration = TimeSpan.FromSeconds(speed);

            storyboard1.Completed += (s, e) =>
            {
                this.index = (short)((this.index + 1) % this.words.Length);
                var textBlock2 = CreateAnimatedTextBlock(this.words[this.index % this.words.Length]);
                MainGrid.Children.Add(textBlock2);
                if(IsPrimary) AnimateTextBlock(textBlock2, 400, 0, 1.25, 0, false);
                textBlock.BeginStoryboard(storyboard2);
            };

            storyboard2.Completed += (s, e) =>
            {
                MainGrid.Children.Remove(textBlock);
                this.textBlocks.Remove(textBlock);
                if (this.textBlocks.Count == 0)
                {
                    StartAnimation();
                }
            };

            textBlock.BeginStoryboard(storyboard1);
        }


        private static TextBlock CreateAnimatedTextBlock(string word)
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

            return textBlock;
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
