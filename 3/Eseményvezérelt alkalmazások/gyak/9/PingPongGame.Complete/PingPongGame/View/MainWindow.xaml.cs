using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace ELTE.PingPongGame.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Event to notify that the ball has moved towards the new target position
        /// </summary>
        public event EventHandler<Thickness>? BallLayoutUpdated;
        /// <summary>
        /// Event to notify that the pad has moved towards the new target position
        /// </summary>
        public event EventHandler<Thickness>? PadLayoutUpdated;

        /// <summary>
        /// Initialize
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Start moving the ball towards the new target position
        /// </summary>
        public void StartBallAnimation(Thickness nextPosition)
        {
            ThicknessAnimation animation = new ThicknessAnimation
            {
                From = Ball.Margin,
                To = nextPosition,
                Duration = new Duration(TimeSpan.FromMilliseconds(5)),
                SpeedRatio = 1 / TravelDistance(Ball.Margin, nextPosition) //Speed depends on the distance to travel
            };

            Ball.BeginAnimation(Ellipse.MarginProperty, animation, HandoffBehavior.SnapshotAndReplace);
        }

        /// <summary>
        /// Start moving the pad towards the new target position
        /// </summary>
        public void StartPadAnimation(Thickness nextPosition)
        {
            ThicknessAnimation animation = new ThicknessAnimation
            {
                From = Pad.Margin,
                To = nextPosition,
                Duration = new Duration(TimeSpan.FromMilliseconds(100)),
            };

            Pad.BeginAnimation(Rectangle.MarginProperty, animation, HandoffBehavior.SnapshotAndReplace);
        }

        /// <summary>
        /// Notify that the ball has moved towards the new target position
        /// </summary>
        private void OnBallLayoutUpdated(object? sender, EventArgs e)
        {
            BallLayoutUpdated?.Invoke(this, Ball.Margin);
        }

        /// <summary>
        /// Notify that the pad has moved towards the new target position
        /// </summary>
        private void OnPadLayoutUpdated(object? sender, EventArgs e)
        {
            PadLayoutUpdated?.Invoke(this, Pad.Margin);
        }

        /// <summary>
        /// Calculate the distance between the current and new target position
        /// </summary>
        private static double TravelDistance(Thickness currentPosition, Thickness nextPosition)
        {
            return Math.Sqrt(Math.Pow(nextPosition.Left - currentPosition.Left, 2) +
                             Math.Pow(nextPosition.Top - currentPosition.Top, 2));
        }
    }
}
