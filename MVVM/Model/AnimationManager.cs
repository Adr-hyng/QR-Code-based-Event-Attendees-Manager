using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace QEAMApp.MVVM.Model
{
    public class AnimationManager
    {
        public static void FadeInWelcomeRibbon(Border border, double dstX)
        {
            var storyboard = new Storyboard();

            var moveAnimation = new DoubleAnimationUsingKeyFrames();

            var keyFrame1 = new SplineDoubleKeyFrame(dstX, KeyTime.FromPercent(1), new KeySpline(0.05, 0, 0.12, 1));
            moveAnimation.KeyFrames.Add(keyFrame1);

            Storyboard.SetTarget(moveAnimation, border);
            Storyboard.SetTargetProperty(moveAnimation, new PropertyPath("(Canvas.Left)"));

            storyboard.Children.Add(moveAnimation);

            storyboard.Begin();
        }

        public static async void ShowSnackBar(Border border, double srcY, double dstY, double Duration = 2)
        {
            var storyboard = new Storyboard();

            // First animation: Move from srcY to dstY
            var moveAnimation1 = new DoubleAnimationUsingKeyFrames();
            var keyFrame1 = new SplineDoubleKeyFrame(dstY, KeyTime.FromPercent(1), new KeySpline(0.5, 0, 0.75, 1));
            moveAnimation1.KeyFrames.Add(keyFrame1);
            Storyboard.SetTarget(moveAnimation1, border);
            Storyboard.SetTargetProperty(moveAnimation1, new PropertyPath("(Canvas.Top)"));
            storyboard.Children.Add(moveAnimation1);

            // Pause after the first animation
            moveAnimation1.Completed += async (s, e) =>
            {
                // Pause for 1 second
                await Task.Delay(TimeSpan.FromSeconds(Duration));

                // Clear the Storyboard
                storyboard.Children.Clear();

                // Second animation: Move back from dstY to srcY
                var moveAnimation2 = new DoubleAnimationUsingKeyFrames
                {
                    FillBehavior = FillBehavior.Stop // Set FillBehavior to Stop
                };
                var keyFrame2 = new SplineDoubleKeyFrame(srcY, KeyTime.FromPercent(1), new KeySpline(0.5, 0, 0.75, 1));
                moveAnimation2.KeyFrames.Add(keyFrame2);
                Storyboard.SetTarget(moveAnimation2, border);
                Storyboard.SetTargetProperty(moveAnimation2, new PropertyPath("(Canvas.Top)"));
                storyboard.Children.Add(moveAnimation2);

                storyboard.Begin();
            };

            storyboard.Begin();
        }

        public static void AnimateRectangle(Rectangle ScannerRect, short srcY, short dstY)
        {
            var storyboard = new Storyboard();

            var moveAnimation = new DoubleAnimationUsingKeyFrames();

            var keyFrame1 = new LinearDoubleKeyFrame(srcY, KeyTime.FromPercent(0));
            var keyFrame2 = new LinearDoubleKeyFrame(dstY, KeyTime.FromPercent(0.5));
            moveAnimation.KeyFrames.Add(keyFrame1);
            moveAnimation.KeyFrames.Add(keyFrame2);

            var keyFrame3 = new LinearDoubleKeyFrame(dstY, KeyTime.FromPercent(0.5));
            var keyFrame4 = new LinearDoubleKeyFrame(srcY, KeyTime.FromPercent(1.0));
            moveAnimation.KeyFrames.Add(keyFrame3);
            moveAnimation.KeyFrames.Add(keyFrame4);

            Storyboard.SetTarget(moveAnimation, ScannerRect);
            Storyboard.SetTargetProperty(moveAnimation, new PropertyPath("(Canvas.Top)"));

            var duration = TimeSpan.FromSeconds(1);
            var repeatBehavior = RepeatBehavior.Forever;

            storyboard.Children.Add(moveAnimation);

            storyboard.Duration = duration;
            storyboard.RepeatBehavior = repeatBehavior;

            storyboard.Begin();
        }
    }
}
