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
        "Kickstart your adventure! \nScan your ID’s QR code here!",
        "Ready, Set, Connect! Begin \nwith a quick QR scan!",
        "Your journey starts with a simple scan! \nQR code at the ready!"
        };

        private const double speed = 1;
        private readonly Int16 duration = 2;
        private readonly List<TextBlock> textBlocks;
        private Int16 index = 0;

        public MainWindow()
        {
            InitializeComponent();

            this.textBlocks = new List<TextBlock>();

            StartAnimation();
        }

        private void StartAnimation()
        {
            var textBlock = CreateAnimatedTextBlock(this.words[this.index % this.words.Length]);
            this.textBlocks.Add(textBlock);
            MainGrid.Children.Add(textBlock);
            AnimateTextBlock(textBlock,
                             900,
                             0,
                             MainWindow.speed,
                             this.duration);
        }

        private void AnimateTextBlock(TextBlock textBlock, Int16 srcY, Int16 dstY, double speed, double HoldDuration, double StartDuration = 1.0, bool IsPrimary = true)
        {
            var storyboard1 = new Storyboard();
            var storyboard2 = new Storyboard();

            var moveAnimation1 = new DoubleAnimationUsingKeyFrames();
            moveAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(srcY, KeyTime.FromPercent(0)));
            moveAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(srcY - (srcY / 2.0), KeyTime.FromPercent(1)));
            Storyboard.SetTarget(moveAnimation1, textBlock);
            Storyboard.SetTargetProperty(moveAnimation1, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            var moveAnimation2 = new DoubleAnimationUsingKeyFrames();
            moveAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(srcY - (srcY / 2.0), KeyTime.FromPercent(0)));
            moveAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(dstY, KeyTime.FromPercent(1)));
            Storyboard.SetTarget(moveAnimation2, textBlock);
            Storyboard.SetTargetProperty(moveAnimation2, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            var opacityAnimation1 = new DoubleAnimationUsingKeyFrames();
            opacityAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0)));
            opacityAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.85)));
            opacityAnimation1.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(1))); // only 1
            Storyboard.SetTarget(opacityAnimation1, textBlock);
            Storyboard.SetTargetProperty(opacityAnimation1, new PropertyPath("Opacity"));

            var opacityAnimation2 = new DoubleAnimationUsingKeyFrames();
            opacityAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(1, KeyTime.FromPercent(0))); // only 1
            opacityAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(0.15)));
            opacityAnimation2.KeyFrames.Add(new LinearDoubleKeyFrame(0, KeyTime.FromPercent(1)));
            Storyboard.SetTarget(opacityAnimation2, textBlock);
            Storyboard.SetTargetProperty(opacityAnimation2, new PropertyPath("Opacity"));

            storyboard1.Children.Add(moveAnimation1);
            storyboard1.Children.Add(opacityAnimation1);
            storyboard2.Children.Add(opacityAnimation2);
            storyboard2.Children.Add(moveAnimation2);

            storyboard1.Duration = TimeSpan.FromSeconds(speed);
            storyboard1.BeginTime = TimeSpan.FromSeconds(StartDuration);
            storyboard2.BeginTime = TimeSpan.FromSeconds(HoldDuration);
            storyboard2.Duration = TimeSpan.FromSeconds(speed);

            storyboard1.Completed += (s, e) =>
            {
                this.index = (short)((this.index + 1) % this.words.Length);
                var textBlock2 = CreateAnimatedTextBlock(this.words[this.index]);
                //var textBlock2 = CreateAnimatedTextBlock("aNex");
                MainGrid.Children.Add(textBlock2);
                if(IsPrimary) AnimateTextBlock(
                    textBlock: textBlock2,
                    srcY: srcY,
                    dstY: dstY,
                    speed: MainWindow.speed,
                    HoldDuration: 2.5, // 2.25 // 2.5
                    StartDuration: HoldDuration - 0.75, // Always 0.5 Delay from // 0.75
                    IsPrimary: false
                    );
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
                FontSize = 40,
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
