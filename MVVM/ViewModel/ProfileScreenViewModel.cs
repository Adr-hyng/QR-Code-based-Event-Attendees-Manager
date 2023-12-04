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
using System.Xml.Linq;

namespace QEAMApp.MVVM.ViewModel
{
    internal class ProfileScreenViewModel: ViewModelBase
    {
        public Dictionary<string, RadioButtonViewModel>? RadioButtons { get; set; }
        public Dictionary<string, RadioButtonViewModel>? OriginalRadioButtons { get; set; }
        public ApiService _apiService;
        public Attendee _profile;
        public NavigationService? _navigationService;
        public ICommand? StayCommand { get; private set; }
        public ICommand? GoBackCommand { get; }
        public ICommand? ToggleCommand { get; }

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
        private string? _name;
        public string Name
        {
            get
            {
                return _name!;
            }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        private string? _membership;
        public string Membership
        {
            get
            {
                return _membership!;
            }
            set
            {
                _membership = value;
                OnPropertyChanged(nameof(Membership));
            }
        }
        private string? _position;
        public string Position
        {
            get
            {
                return _position!;
            }
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
            }
        }
        private string? _institution;
        public string Institution
        {
            get
            {
                return _institution!;
            }
            set
            {
                _institution = value;
                OnPropertyChanged(nameof(Institution));
            }
        }

        public ProfileScreenViewModel(NavigationService GoToIdleScreen)
        {
            // This is not used anymore. Used only for fake initializing won't be fixing this. but can be better.
            GoBackCommand = new NavigateCommand(GoToIdleScreen);
        }

        public ProfileScreenViewModel(NavigationService GoToIdleScreen, Attendee Profile, ApiService APIInstance)
        {
            FirstName = Profile.FN;
            Name = $"{Profile.FN} {(!String.IsNullOrEmpty(Profile.MI) ? Profile.MI + "." : "")} {Profile.LN}";
            Membership = Profile.Membership;
            Position = Profile.Position;
            Institution = Profile.Institution;
            _profile = Profile;
            _apiService = APIInstance;
            _navigationService = GoToIdleScreen;
            RadioButtonsHandler();
            AttendanceMarking();
            GoBackCommand = new GoBackUpdateCommand(this);
            ToggleCommand = new ToggleButtonCommand(this); // Soon for Developer Mode To Manually Toggle
            if (!_apiService.DebugMode) CloseTimerOption(Seconds: 3);
        }

        private async void AttendanceMarking()
        {
            Dictionary<string, DayContent> DayController = ScheduleManager.GetDayController(_profile);

            DateTime currentDateTime = DateTime.Now;
            string currentDate = currentDateTime.ToString("MM/dd");

            if (!DayController.ContainsKey(currentDate)) return;
            DayContent subDayController = DayController[currentDate];

            PropertyInfo[] properties = subDayController.GetType().GetProperties();

            // Check In (Time In)
            if (!subDayController.CheckIn.HasValue)
            {
                (bool IsUpdated, _profile) = await _apiService.UpdateAttendee(_profile.UID, $"checkin{subDayController.id}", currentDateTime.ToString());
                if (!IsUpdated) return;
                String name = _profile!.FN + $" {(_profile.MI.Length > 0 ? $"{_profile.MI}. " : "")}" + _profile.LN;
                //_apiService.LogEntries.Add(name, new LogEntry(currentDateTime, _apiService.LogEntries.Count + 1, name, "Checked-In"));
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
                    if (ScheduleManager.GetTimeController().TryGetValue(propertyName, out var TimeSchedule))
                    {
                        if (!Utility.IsNotNullOrEmpty(propertyValue) && DayContent.InTimeBound(currentDateTime, TimeSchedule.from, TimeSchedule.to))
                        {
                            Action<string, string> updateMarkIcon = async (columnPrefix, radioButtonPrefix) =>
                            {
                                (bool IsUpdated, _profile) = await _apiService.UpdateAttendee(_profile.UID, $"{columnPrefix}{subDayController.id}", currentDateTime.ToString());
                                if (!IsUpdated) return;
                                String name = _profile!.FN + $" {(_profile.MI.Length > 0 ? $"{_profile.MI}. " : "")}" + _profile.LN;
                                //_apiService.LogEntries.Add(name, new LogEntry(currentDateTime, _apiService.LogEntries.Count + 1, name, "Checked-Out"));
                                await Task.Delay(1000);
                                RadioButtons![$"{radioButtonPrefix}{subDayController.id.ToUpper()}"].Opacity = 1;
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
                ["AMD1"] = _profile.Day1.AmSnack.HasValue,
                ["LD1"] = _profile.Day1.LunchSnack.HasValue,
                ["PMD1"] = _profile.Day1.PmSnack.HasValue,
                ["CheckInD1"] = _profile.Day1.CheckIn.HasValue,
                ["CheckOutD1"] = _profile.Day1.CheckOut.HasValue,

                ["AMD2"] = _profile.Day2.AmSnack.HasValue,
                ["LD2"] = _profile.Day2.LunchSnack.HasValue,
                ["PMD2"] = _profile.Day2.PmSnack.HasValue,
                ["CheckInD2"] = _profile.Day2.CheckIn.HasValue,
                ["CheckOutD2"] = _profile.Day2.CheckOut.HasValue,

                ["AMD3"] = _profile.Day3.AmSnack.HasValue,
                ["LD3"] = _profile.Day3.LunchSnack.HasValue,
                ["PMD3"] = _profile.Day3.PmSnack.HasValue,
                ["CheckInD3"] = _profile.Day3.CheckIn.HasValue,
                ["CheckOutD3"] = _profile.Day3.CheckOut.HasValue,
            };

            RadioButtons = new();

            foreach (KeyValuePair<string, bool> ToggleButtonContent in dayContents)
            {
                RadioButtons.Add(ToggleButtonContent.Key, new RadioButtonViewModel { IsChecked = true, Opacity = ToggleButtonContent.Value ? 1 : 0 });
            }
            OriginalRadioButtons = CloneDictionaryCloningValues(RadioButtons);
        }
        public async void CloseTimerOption(int Seconds)
        {
            await Task.Delay(1000 * Seconds);
            GoBackCommand!.Execute(null);
        }

        private static Dictionary<TKey, TValue> CloneDictionaryCloningValues<TKey, TValue>
 (Dictionary<TKey, TValue> original) where TValue : ICloneable
        {
            Dictionary<TKey, TValue> ret = new (original.Count,
                                                              original.Comparer);
            foreach (KeyValuePair<TKey, TValue> entry in original)
            {
                ret.Add(entry.Key, (TValue)entry.Value.Clone());
            }
            return ret;
        }
    }
}
