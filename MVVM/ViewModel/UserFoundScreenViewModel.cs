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
    internal class UserFoundScreenViewModel: ViewModelBase
    {
        private int _opacity;
        public int Opacity
        {
            get
            {
                return _opacity;
            }
            set
            {
                _opacity = value;
                OnPropertyChanged(nameof(Opacity));
            }
        }
        public ICommand ShowProfileCommand { get; }

        public UserFoundScreenViewModel(NavigationService GoToProfileNavigation)
        {
            _opacity = 0;
            ShowProfileCommand = new NavigateCommand(GoToProfileNavigation);
            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            // Wait for 2 seconds
            await Task.Delay(2000);
            ShowProfileCommand.Execute(null);
        }
    }
}
