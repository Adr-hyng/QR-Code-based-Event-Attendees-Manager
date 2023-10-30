using QEAMApp.Core;
using QEAMApp.MVVM.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace QEAMApp.MVVM.ViewModel
{
    internal class IdleScreenViewModel: ViewModelBase
    {
        private string _uniqueIdentifier;
        public string UniqueIdentifier
        {
            get
            {
                return _uniqueIdentifier;
            }
            set
            {
                _uniqueIdentifier = value;
                OnPropertyChanged(nameof(UniqueIdentifier));
            }
        }

        public ICommand ScanningCommand { get; }

        public IdleScreenViewModel(NavigationService GoToFoundScreenNavigation)
        {
            ScanningCommand = new NavigateCommand(GoToFoundScreenNavigation);
        }
    }
}
