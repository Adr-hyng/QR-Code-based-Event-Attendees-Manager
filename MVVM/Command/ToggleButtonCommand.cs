using QEAMApp.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace QEAMApp.MVVM.Command
{
    internal class ToggleButtonCommand: CommandBase
    {
        private Dictionary<string, RadioButtonViewModel> RadioButtons;
        public ToggleButtonCommand(Dictionary<string, RadioButtonViewModel> _RadioButtons)
        {
            RadioButtons = _RadioButtons;
        }
        public override void Execute(object? parameter)
        {
            if (parameter is object[] parameters)
            {
                bool isChecked = (bool)parameters[0];
                string name = (string)parameters[1];

                if (RadioButtons.TryGetValue(name, out var SelectedRadioButton))
                {
                    SelectedRadioButton.Opacity = 1;
                    SelectedRadioButton.IsChecked = isChecked;
                }
            }
        }

    }
}
