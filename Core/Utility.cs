using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace QEAMApp.Core
{
    public class Utility
    {
        public static void AnimateRectangle(Rectangle ScannerRect, Int16 srcY, Int16 dstY)
        {
            var storyboard = new Storyboard();

            var moveAnimation = new DoubleAnimationUsingKeyFrames();

            var keyFrame1 = new LinearDoubleKeyFrame(srcY, KeyTime.FromPercent(0));
            var keyFrame2 = new LinearDoubleKeyFrame(dstY, KeyTime.FromPercent(0.5));
            moveAnimation.KeyFrames.Add(keyFrame1);
            moveAnimation.KeyFrames.Add(keyFrame2);

            var keyFrame3 = new LinearDoubleKeyFrame(dstY, KeyTime.FromPercent(0.5));
            var keyFrame4 = new LinearDoubleKeyFrame(srcY, KeyTime.FromPercent(1));
            moveAnimation.KeyFrames.Add(keyFrame3);
            moveAnimation.KeyFrames.Add(keyFrame4);

            Storyboard.SetTarget(moveAnimation, ScannerRect);
            Storyboard.SetTargetProperty(moveAnimation, new PropertyPath("(Canvas.Top)"));

            var duration = TimeSpan.FromSeconds(1.5);
            var repeatBehavior = RepeatBehavior.Forever;

            storyboard.Children.Add(moveAnimation);

            storyboard.Duration = duration;
            storyboard.RepeatBehavior = repeatBehavior;

            storyboard.Begin();
        }
        public static T? FindVisualChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null)
                return null;

            T? foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is not T)
                {
                    foundChild = FindVisualChild<T>(child, childName);
                    if (foundChild != null)
                        break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
    }
}
