using QEAMApp.MVVM.Model;
using QEAMApp.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QEAMApp.MVVM.Command
{
    internal class ToggleButtonCommand: CommandBase
    {
        readonly ProfileScreenViewModel _profileScreen;
        public ToggleButtonCommand(ProfileScreenViewModel profileScreen)
        {
            _profileScreen = profileScreen;
        }
        public override void Execute(object? parameter)
        {
            if (parameter is object[] parameters)
            {
                bool isChecked = (bool)parameters[0];
                string name = (string)parameters[1];
                if (_profileScreen.RadioButtons!.TryGetValue(name, out var SelectedRadioButton))
                {
                    // Made only since Food Distribution is not needed anymore. 
                    if (! (new[] { "am", "l", "pm" }.Any(element => element.Contains(name))) )
                    {
                        if (_profileScreen._apiService.DebugMode)
                        {
                            SelectedRadioButton.Opacity = 1;
                            SelectedRadioButton.IsChecked = isChecked;
                        }
                        else
                        {
                            SelectedRadioButton.Opacity = SelectedRadioButton.Opacity;
                            SelectedRadioButton.IsChecked = !SelectedRadioButton.IsChecked;
                        }
                    } else
                    {
                        SelectedRadioButton.Opacity = SelectedRadioButton.Opacity;
                        SelectedRadioButton.IsChecked = !SelectedRadioButton.IsChecked;
                    }
                }
                
            }
        }

    }
}
