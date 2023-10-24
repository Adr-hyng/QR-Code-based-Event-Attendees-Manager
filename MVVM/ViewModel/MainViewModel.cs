using QEAMApp.Core;
using System;

namespace QEAMApp.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {

        public RelayCommand BaseViewCommand { get; set; }
        public RelayCommand CardViewCommand { get; set; }

        public HomeViewModel HomeVM { get; set; }
        public BaseViewModel BaseVM { get; set; }
        public CardViewModel CardVM { get; set; }

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
            CardVM = new CardViewModel();

            CurrentView = BaseVM;

            BaseViewCommand = new RelayCommand(o =>
            {
                CurrentView = BaseVM;
            });
            CardViewCommand = new RelayCommand(o =>
            {
                CurrentView = CardVM;
            });
        }
    }
}

