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

namespace QEAMApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for UserFoundScreenView.xaml
    /// </summary>
    public partial class UserFoundScreenView : UserControl
    {
        private DispatcherTimer timer;
        private int periodCount;
        public UserFoundScreenView()
        {
            InitializeComponent();

            DoubleAnimation animation = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.8),
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever
            };

            // Set the target property to animate
            Storyboard.SetTarget(animation, FoundIcon);
            Storyboard.SetTargetProperty(animation, new PropertyPath(Border.OpacityProperty));

            // Create a Storyboard and add the animation to it
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(animation);

            // Start the storyboard animation
            storyboard.Begin();

            periodCount = 1;

            // Create a DispatcherTimer with an interval of 500 milliseconds
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Update the text with the appropriate number of periods
            loadingText.Text = "User Found, now loading" + new string('.', periodCount);

            // Increment the period count and reset it if it reaches 4
            periodCount++;
            if (periodCount > 3)
            {
                periodCount = 1;
            }
        }
    }
}
