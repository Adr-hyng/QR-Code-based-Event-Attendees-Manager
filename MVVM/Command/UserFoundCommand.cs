using QEAMApp.Core;
using QEAMApp.MVVM.Model;
using QEAMApp.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QEAMApp.MVVM.Command
{
    internal class UserFoundCommand: CommandBase
    {
        private readonly NavigationService _navigationService;
        private readonly ApiService _apiService;

        public UserFoundCommand(NavigationService navigationService)
        {
            _navigationService = navigationService;
            _apiService = new ApiService();
        }

        public override async void Execute(object? parameter)
        {
            if (parameter is string uniqueIdentifier)
            {
                (bool IsValidUser, Attendee attendee) = await _apiService.Authenticate(uniqueIdentifier);
                if (!string.IsNullOrEmpty(uniqueIdentifier) && IsValidUser)
                {
                    MessageBox.Show(attendee.ToString());
                    _navigationService.Navigate();
                }
                else
                {
                    MessageBox.Show("User not found.", "5th ICPEP Regional Convention");
                }
            }
        }
    }
}
