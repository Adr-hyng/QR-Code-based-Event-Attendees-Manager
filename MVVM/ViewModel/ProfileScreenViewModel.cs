using QEAMApp.Core;
using QEAMApp.MVVM.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QEAMApp.MVVM.ViewModel
{
    internal class ProfileScreenViewModel: ViewModelBase
    {
        public string Name { get; }
        public string Membership { get; }
        public string Position { get; }
        public string Institution { get; }

        public ICommand StayCommand { get; private set; }
        public ICommand GoBackCommand { get;}

        public ProfileScreenViewModel(NavigationService GoToIdleScreen)
        {
            GoBackCommand = new NavigateCommand(GoToIdleScreen);
        }
    }
}
