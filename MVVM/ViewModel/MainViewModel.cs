using QEAMApp.Core;
using System;

namespace QEAMApp.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand BaseViewCommand { get; set; }
        public HomeViewModel HomeVM { get; set; }
        public BaseViewModel BaseVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            BaseVM = new BaseViewModel();

            CurrentView = BaseVM;

            BaseViewCommand = new RelayCommand(o =>
            {
                CurrentView = BaseVM;
            });
        }
    }
}

