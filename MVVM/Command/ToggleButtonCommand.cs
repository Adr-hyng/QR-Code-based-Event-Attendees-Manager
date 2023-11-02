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
        public override async void Execute(object? parameter)
        {
            if (parameter is string radioButtonName)
            {
            }
        }
    }
}
