using QEAMApp.Core;
using QEAMApp.MVVM.Command;
using QEAMApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QEAMApp.MVVM.ViewModel
{
    internal class ProfileScreenViewModel: ViewModelBase
    {
        public Dictionary<string, RadioButtonViewModel>? RadioButtons { get; set; }

        private const bool AUTO_CLOSE = false;
        private string? _firstName;
        public string? FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string _membership;
        public string Membership
        {
            get
            {
                return _membership;
            }
            set
            {
                _membership = value;
                OnPropertyChanged(nameof(Membership));
            }
        }
        private string _position;
        public string Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }
        private string _institution;
        public string Institution
        {
            get
            {
                return _institution;
            }
            set
            {
                _institution = value;
                OnPropertyChanged(nameof(Institution));
            }
        }

        private Attendee _profile;
        private readonly ApiService _apiService;

        public ICommand StayCommand { get; private set; }
        public ICommand GoBackCommand { get;}
        public ICommand ToggleCommand { get;}

        public ProfileScreenViewModel(NavigationService GoToIdleScreen)
        {
            GoBackCommand = new NavigateCommand(GoToIdleScreen);
            ToggleCommand = new ToggleButtonCommand(RadioButtons!);
        }

        public ProfileScreenViewModel(NavigationService GoToIdleScreen, Attendee Profile, ApiService APIInstance)
        {
            FirstName = Profile.fn;
            Name = $"{Profile.fn} {(!String.IsNullOrEmpty(Profile.mi) ? Profile.mi + "." : "")} {Profile.ln}";
            Membership = Profile.membership;
            Position = Profile.position;
            Institution = Profile.institution;
            GoBackCommand = new NavigateCommand(GoToIdleScreen);
            ToggleCommand = new ToggleButtonCommand(RadioButtons!); // Soon for Developer Mode To Manually Toggle
            _profile = Profile;
            _apiService = APIInstance;
            MarkCheckIn();
            RadioButtonsHandler();

            if (AUTO_CLOSE) CloseTimerOption(Seconds: 3);
        }

        private async void MarkCheckIn()
        {
            // Check if it's first time to check-in.
            // Check first if what day is today, and get if it has already have value.
            if (!_profile!.day1.checkIn.HasValue)
            {
                (bool success, _profile) = await _apiService.UpdateAttendee("5kdIBZbdnNdF7amvWX1sPk", "checkind1", DateTime.Now.ToString());
                if (!success) return;
                await Task.Delay(1000);
                // Insert Animations for this to be resizing
                RadioButtons!["CheckInD1"].IsChecked = true;
                RadioButtons!["CheckInD1"].Opacity = 1;
            }
        }

        private void RadioButtonsHandler()
        {
            Dictionary<string, bool> dayContents = new Dictionary<string, bool>()
            {
                ["AMD1"] = _profile.day1.amSnack.HasValue,
                ["LD1"] = _profile.day1.lunchSnack.HasValue,
                ["PMD1"] = _profile.day1.pmSnack.HasValue,
                ["CheckInD1"] = _profile.day1.checkIn.HasValue,
                ["CheckOutD1"] = _profile.day1.checkOut.HasValue,

                ["AMD2"] = _profile.day2.amSnack.HasValue,
                ["LD2"] = _profile.day2.lunchSnack.HasValue,
                ["PMD2"] = _profile.day2.pmSnack.HasValue,
                ["CheckInD2"] = _profile.day2.checkIn.HasValue,
                ["CheckOutD2"] = _profile.day2.checkOut.HasValue,

                ["AMD3"] = _profile.day3.amSnack.HasValue,
                ["LD3"] = _profile.day3.lunchSnack.HasValue,
                ["PMD3"] = _profile.day3.pmSnack.HasValue,
                ["CheckInD3"] = _profile.day3.checkIn.HasValue,
                ["CheckOutD3"] = _profile.day3.checkOut.HasValue,
            };

            RadioButtons = new Dictionary<string, RadioButtonViewModel>();

            foreach (KeyValuePair<string, bool> ToggleButtonContent in dayContents)
            {
                RadioButtons.Add(ToggleButtonContent.Key, new RadioButtonViewModel { IsChecked = true, Opacity = ToggleButtonContent.Value ? 1 : 0 });
            }
        }

        public async void CloseTimerOption(int Seconds)
        {
            await Task.Delay(1000 * Seconds);
            GoBackCommand.Execute(null);
        }
    }
}
