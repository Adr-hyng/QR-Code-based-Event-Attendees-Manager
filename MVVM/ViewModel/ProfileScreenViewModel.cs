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
            AttendanceMarking();
            RadioButtonsHandler();

            if (AUTO_CLOSE) CloseTimerOption(Seconds: 3);
        }

        private async void AttendanceMarking()
        {
            Dictionary<string, DayContent> DayController = new()
            {
                ["11/04"] = _profile.day1,
                ["11/05"] = _profile.day2,
                ["11/06"] = _profile.day3,
            };

            DateTime currentDateTime = DateTime.Now;
            string currentDate = currentDateTime.ToString("MM/dd");

            if (!DayController.ContainsKey(currentDate)) return;
            DayContent subDayController = DayController[currentDate];

            // Check In (Time In)
            if (!subDayController.CheckIn.HasValue)
            {
                (bool IsUpdated, _profile) = await _apiService.UpdateAttendee(_profile.uid, $"checkin{subDayController.id}", currentDateTime.ToString());
                if (!IsUpdated) return;
                await Task.Delay(1000);
                RadioButtons![$"CheckIn{subDayController.id.ToUpper()}"].Opacity = 1;
            }

            // Morning Snack (Between 8 AM - 11:59 AM)
            else if (!subDayController.AmSnack.HasValue && subDayController.InTimeBound(
                currentDateTime,
                new TimeSpan(8, 0, 0),
                new TimeSpan(11, 59, 0)
                ))
            {
                (bool IsUpdated, _profile) = await _apiService.UpdateAttendee(_profile.uid, $"am{subDayController.id}", currentDateTime.ToString());
                if (!IsUpdated) return;
                await Task.Delay(1000);
                RadioButtons![$"AM{subDayController.id.ToUpper()}"].Opacity = 1;
            }

            // Lunch Snack (Between 12 PM - 3:00 PM)
            else if (!subDayController.LunchSnack.HasValue && subDayController.InTimeBound(
                currentDateTime,
                new TimeSpan(12, 0, 0),
                new TimeSpan(15, 0, 0)
                ))
            {
                (bool IsUpdated, _profile) = await _apiService.UpdateAttendee(_profile.uid, $"lunch{subDayController.id}", currentDateTime.ToString());
                if (!IsUpdated) return;
                await Task.Delay(1000);
                RadioButtons![$"L{subDayController.id.ToUpper()}"].Opacity = 1;
            }

            // Evening Snack (Between 4 PM - 5:59 PM)
            else if (!subDayController.PmSnack.HasValue && subDayController.InTimeBound(
                currentDateTime,
                new TimeSpan(16, 0, 0),
                new TimeSpan(17, 59, 59)
                ))
            {
                (bool IsUpdated, _profile) = await _apiService.UpdateAttendee(_profile.uid, $"pm{subDayController.id}", currentDateTime.ToString());
                if (!IsUpdated) return;
                await Task.Delay(1000);
                RadioButtons![$"PM{subDayController.id.ToUpper()}"].Opacity = 1;
            }

            // Check Out (Time Out) (Between 6 PM - 7:59 PM)
            else if (!subDayController.CheckOut.HasValue && subDayController.InTimeBound(
                currentDateTime, 
                new TimeSpan(18, 0, 0), 
                new TimeSpan(20, 0, 0)
                ))
            {
                (bool IsUpdated, _profile) = await _apiService.UpdateAttendee(_profile.uid, $"checkout{subDayController.id}", currentDateTime.ToString());
                if (!IsUpdated) return;
                await Task.Delay(1000);
                RadioButtons![$"CheckOut{subDayController.id.ToUpper()}"].Opacity = 1;
            }
        }

        private void RadioButtonsHandler()
        {
            Dictionary<string, bool> dayContents = new()
            {
                ["AMD1"] = _profile.day1.AmSnack.HasValue,
                ["LD1"] = _profile.day1.LunchSnack.HasValue,
                ["PMD1"] = _profile.day1.PmSnack.HasValue,
                ["CheckInD1"] = _profile.day1.CheckIn.HasValue,
                ["CheckOutD1"] = _profile.day1.CheckOut.HasValue,

                ["AMD2"] = _profile.day2.AmSnack.HasValue,
                ["LD2"] = _profile.day2.LunchSnack.HasValue,
                ["PMD2"] = _profile.day2.PmSnack.HasValue,
                ["CheckInD2"] = _profile.day2.CheckIn.HasValue,
                ["CheckOutD2"] = _profile.day2.CheckOut.HasValue,

                ["AMD3"] = _profile.day3.AmSnack.HasValue,
                ["LD3"] = _profile.day3.LunchSnack.HasValue,
                ["PMD3"] = _profile.day3.PmSnack.HasValue,
                ["CheckInD3"] = _profile.day3.CheckIn.HasValue,
                ["CheckOutD3"] = _profile.day3.CheckOut.HasValue,
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
