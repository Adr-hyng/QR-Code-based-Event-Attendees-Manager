using QEAMApp.Core;
using QEAMApp.MVVM.Model;
using QEAMApp.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QEAMApp.MVVM.Command
{
    public class AutoClosingMessageBox
    {
        System.Threading.Timer _timeoutTimer;
        string _caption;
        AutoClosingMessageBox(string text, string caption, int timeout)
        {
            _caption = caption;
            _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                null, timeout, System.Threading.Timeout.Infinite);
            using (_timeoutTimer)
                MessageBox.Show(text, caption);
        }
        public static void Show(string text, string caption, int timeout)
        {
            new AutoClosingMessageBox(text, caption, timeout);
        }
        void OnTimerElapsed(object state)
        {
            IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
            if (mbWnd != IntPtr.Zero)
                SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
            _timeoutTimer.Dispose();
        }
        const int WM_CLOSE = 0x0010;
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }

    internal class UserFoundCommand: CommandBase
    {
        private readonly IdleScreenViewModel _idleScreen;
        private readonly NavigationService _navigationService;
        private readonly ApiService _apiService;

        public UserFoundCommand(NavigationService navigationService, IdleScreenViewModel idleScreen)
        {
            _idleScreen = idleScreen;
            _navigationService = navigationService;
            _apiService = new ApiService();
        }

        public static async Task PlaySound(string filePath)
        {
            MediaPlayer mediaPlayer = new MediaPlayer();
            mediaPlayer.Open(new Uri(filePath));

            TaskCompletionSource<bool> taskCompletionSource = new TaskCompletionSource<bool>();

            mediaPlayer.MediaEnded += (sender, e) =>
            {
                mediaPlayer.Close();
                taskCompletionSource.SetResult(true);
            };

            while (true)
            {
                mediaPlayer.Play();

                await taskCompletionSource.Task;

                // Reset the task completion source for the next play
                taskCompletionSource = new TaskCompletionSource<bool>();
            }
        }


        public override async void Execute(object? parameter)
        {
            if (parameter is string uniqueIdentifier)
            {
                string id = uniqueIdentifier;
                if (string.IsNullOrEmpty(id)) return;
                _idleScreen.UniqueIdentifier = "";
                _idleScreen.IsReadOnly = true;
                (bool IsValidUser, Attendee attendee) = await _apiService.Authenticate(id);
                if (IsValidUser)
                {
                    NavigationStore navigationStore = _navigationService._navigationStore;

                    // To change the next page or navigation
                    navigationStore.CurrentViewModel = new UserFoundScreenViewModel(
                        GoToProfileNavigation: new NavigationService(navigationStore, 
                            () => new ProfileScreenViewModel(
                                GoToIdleScreen: new NavigationService(navigationStore,
                                    () => _idleScreen),
                                profile: attendee
                            )
                        )
                    );

                    _navigationService._navigationStore = navigationStore;
                    _idleScreen.IsReadOnly = false;
                }
                else
                {
                    SystemSounds.Exclamation.Play();
                    AutoClosingMessageBox.Show("User not found.", "ERROR 404", 2000);
                    _idleScreen.IsReadOnly = false;
                }
            }
        }
    }
}
