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
