using QEAMApp.Core;
using QEAMApp.MVVM.Model;
using QEAMApp.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace QEAMApp.MVVM.Command
{
    internal class GoBackUpdateCommand: CommandBase
    {

        readonly ProfileScreenViewModel _profileScreen;
        public GoBackUpdateCommand(ProfileScreenViewModel profileScreen)
        {
            _profileScreen = profileScreen;
        }

        public override void Execute(object? parameter)
        {
            // Update all the RadioButtons
            DateTime currentDateTime = DateTime.Now;

            Action<string, string> updateMarkIcon = async (columnName, radioButtonKey) =>
            {
                if (_profileScreen.RadioButtons![radioButtonKey].Opacity != 1) return;
                if(_profileScreen.OriginalRadioButtons![radioButtonKey].IsChecked == _profileScreen.RadioButtons![radioButtonKey].IsChecked)
                {
                    (bool IsUpdated, _profileScreen._profile) = await _profileScreen._apiService.UpdateAttendee(_profileScreen._profile.UID, columnName, currentDateTime.ToString());
                } else
                {
                    (bool IsUpdated, _profileScreen._profile) = await _profileScreen._apiService.UpdateAttendee(_profileScreen._profile.UID, columnName, "NULL");
                }
            };

            string[] dbColumns = ScheduleManager.GetDayDBColumnName(combined: true);

            foreach (var (RadioButton, index) in _profileScreen.RadioButtons!.Select((value, index) => (value, index)))
            {
                updateMarkIcon.Invoke(dbColumns[index], RadioButton.Key);
            }
            _profileScreen._navigationService.Navigate();
        }
    }
}
