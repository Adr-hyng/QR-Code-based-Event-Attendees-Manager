﻿using QEAMApp.Core;
using QEAMApp.MVVM.Model;
using QEAMApp.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public override async void Execute(object? parameter)
        {
            if (parameter is string uniqueIdentifier)
            {
                string id = uniqueIdentifier;
                _idleScreen.UniqueIdentifier = "";
                _idleScreen.IsReadOnly = true;
                (bool IsValidUser, Attendee attendee) = await _apiService.Authenticate(id);
                if (!string.IsNullOrEmpty(id) && IsValidUser)
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
                    MessageBox.Show("User not found.", "5th ICPEP Regional Convention");
                    _idleScreen.IsReadOnly = false;
                }
            }
        }
    }
}
