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

            var duration = TimeSpan.FromSeconds(1.2);
            var repeatBehavior = RepeatBehavior.Forever;

            storyboard.Children.Add(moveAnimation);

            storyboard.Duration = duration;
            storyboard.RepeatBehavior = repeatBehavior;

            storyboard.Begin();
        }
    }
}
