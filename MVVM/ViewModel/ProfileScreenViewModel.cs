using QEAMApp.Core;
using QEAMApp.MVVM.Command;
using QEAMApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
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
            _profile = Profile;
            _apiService = APIInstance;
            RadioButtonsHandler();
            AttendanceMarking();

            ToggleCommand = new ToggleButtonCommand(RadioButtons!); // Soon for Developer Mode To Manually Toggle
            if (AUTO_CLOSE) CloseTimerOption(Seconds: 3);
        }

        private async void AttendanceMarking()
        {
            Dictionary<string, DayContent> DayController = new()
            {
                ["12/02"] = _profile.day1,
                ["12/03"] = _profile.day2,
                ["12/04"] = _profile.day3,
            };

            Dictionary<string, (TimeSpan from, TimeSpan to, String columnPrefix, String radioButtonPrefix)> timeSpans = new()
            {
                { "AmSnack", (new TimeSpan(8, 0, 0), new TimeSpan(11, 59, 0), "am", "AM") }, // Morning Snack (Between 8 AM - 11:59 AM)
                { "LunchSnack", (new TimeSpan(12, 0, 0), new TimeSpan(15, 59, 59), "lunch" ,"L") }, // Lunch Snack (Between 12 PM - 3:59 PM)
                { "PmSnack", (new TimeSpan(16, 0, 0), new TimeSpan(17, 59, 59), "pm" , "PM") }, // Evening Snack (Between 4 PM - 5:59 PM)
                { "CheckOut", (new TimeSpan(18, 0, 0), new TimeSpan(20, 0, 0), "checkout" , "CheckOut") }, // Check Out (Time Out) (Between 6 PM - 8:00 PM)
            };

            DateTime currentDateTime = DateTime.Now;
            string currentDate = currentDateTime.ToString("MM/dd");

            if (!DayController.ContainsKey(currentDate)) return;
            DayContent subDayController = DayController[currentDate];

            PropertyInfo[] properties = subDayController.GetType().GetProperties();

            // Check In (Time In)
            if (!subDayController.CheckIn.HasValue)
            {
                (bool IsUpdated, _profile) = await _apiService.UpdateAttendee(_profile.uid, $"checkin{subDayController.id}", currentDateTime.ToString());
                if (!IsUpdated) return;
                await Task.Delay(1000);
                RadioButtons![$"CheckIn{subDayController.id.ToUpper()}"].Opacity = 1;
            }

            else
            {
                foreach (PropertyInfo property in properties)
                {
                    string propertyName = property.Name;
                    object propertyValue = property.GetValue(subDayController)!;
                    if (propertyName.Contains("CheckIn")) continue;
                    if (timeSpans.TryGetValue(propertyName, out var TimeSchedule))
                    {
                        if (!Utility.IsNotNullOrEmpty(propertyValue) && subDayController.InTimeBound(currentDateTime, TimeSchedule.from, TimeSchedule.to))
                        {
                            Action<string, string> updateMarkIcon = async (columnPrefix, radioButtonPrefix) =>
                            {
                                (bool IsUpdated, _profile) = await _apiService.UpdateAttendee(_profile.uid, $"{columnPrefix}{subDayController.id}", currentDateTime.ToString());
                                if (IsUpdated)
                                {
                                    await Task.Delay(1000);
                                    RadioButtons![$"{radioButtonPrefix}{subDayController.id.ToUpper()}"].Opacity = 1;
                                }
                            };

                            updateMarkIcon.Invoke(TimeSchedule.columnPrefix, TimeSchedule.radioButtonPrefix);
                        }
                    }
                }
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
