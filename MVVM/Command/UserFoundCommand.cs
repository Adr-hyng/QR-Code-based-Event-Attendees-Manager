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

        public UserFoundCommand(NavigationService navigationService, IdleScreenViewModel idleScreen, ApiService API)
        {
            _idleScreen = idleScreen;
            _navigationService = navigationService;
            _apiService = API;
        }

        public override async void Execute(object? parameter)
        {
            if (parameter is string uniqueIdentifier)
            {
                string id = uniqueIdentifier;
                if (string.IsNullOrEmpty(id)) return;
                _idleScreen.UniqueIdentifier = "";
                _idleScreen.IsReadOnly = true;
                (bool? IsValidUser, Attendee? attendee) = await _apiService.Authenticate(id);
                if (IsValidUser == true)
                {
                    NavigationStore navigationStore = _navigationService._navigationStore;

                    // To change the next page or navigation
                    navigationStore.CurrentViewModel = new UserFoundScreenViewModel(
                        GoToProfileNavigation: new NavigationService(navigationStore, 
                            () => new ProfileScreenViewModel(
                                GoToIdleScreen: new NavigationService(navigationStore,
                                    () => _idleScreen),
                                profile: attendee!
                            )
                        )
                    );

                    // Check if it's first time to check-in.
                    // Check first if what day is today, and get if it has already have value.
                    if(!attendee!.day1.checkIn.HasValue)
                    {
                        bool success = await _apiService.UpdateAttendee("NzmW19gA3ER4GNxwx8JOKh", "pmd1", DateTime.Now.ToString());
                        if (success)
                        {
                            Console.WriteLine("Attendee updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to update attendee.");
                        }
                    }

                    _navigationService._navigationStore = navigationStore;
                    _idleScreen.IsReadOnly = false;
                }
                else if(IsValidUser == false)
                {
                    SystemSounds.Exclamation.Play();
                    AutoClosingMessageBox.Show("User not found.", "ERROR 404", 2000);
                    _idleScreen.IsReadOnly = false;
                }
                else
                {
                    _idleScreen.IsReadOnly = false;
                }
            }
        }
    }
}
