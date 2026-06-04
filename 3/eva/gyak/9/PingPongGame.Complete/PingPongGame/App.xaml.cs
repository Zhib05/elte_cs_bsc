using System;
using System.Windows;
using ELTE.PingPongGame.ViewModel;
using ELTE.PingPongGame.View;

namespace ELTE.PingPongGame
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow? _mainWindow;
        private MainViewModel? _mainViewModel;
        public App()
        {
            Startup += Application_Started;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        public void Application_Started(object? sender, StartupEventArgs e)
        {
            _mainViewModel = new MainViewModel();
            _mainViewModel.BallNextPosition += OnBallNextPosition;
            _mainViewModel.PadNextPosition += OnPadNextPosition;
            _mainViewModel.GameOver += OnGameOver;


            _mainWindow = new MainWindow
            {
                DataContext = _mainViewModel
            };

            _mainWindow.BallLayoutUpdated += OnBallLayoutChanged;
            _mainWindow.PadLayoutUpdated += OnPadLayoutChanged;

            _mainWindow.Show();
        }

        /// <summary>
        /// Handle that the ball moved towards the new target position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBallLayoutChanged(object? sender, Thickness e)
        {
            _mainViewModel?.MoveBall(e.Left, e.Top);
        }

        /// <summary>
        /// Handle that the pad moved towards the new target position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPadLayoutChanged(object? sender, Thickness e)
        {
            _mainViewModel?.MovePad(e.Left, e.Top);
        }

        /// <summary>
        /// Handle that the ball has a new target position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBallNextPosition(object? sender, Thickness e)
        {
            _mainWindow?.StartBallAnimation(e);
        }

        /// <summary>
        /// Handle that the pad has a new target position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPadNextPosition(object? sender, Thickness e)
        {
            _mainWindow?.StartPadAnimation(e);
        }
        
        /// <summary>
        /// Handle game over
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameOver(object? sender, EventArgs e)
        {
           MessageBoxResult close = MessageBox.Show("Vége a játéknak!");

            if(close == MessageBoxResult.OK) 
            {
                Application.Current.Shutdown();
            }
        }
    }
}
