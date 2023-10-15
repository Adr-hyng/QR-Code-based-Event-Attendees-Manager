using QEAMApp.Core;
using System;

namespace QEAMApp.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {

        public RelayCommand PersonalViewCommand { get; set; }
        public RelayCommand StatusViewCommand { get; set; }

        public PersonalViewModel PersonalVM { get; set; }
        public StatusViewModel StatusVM { get; set; }

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
            PersonalVM = new PersonalViewModel();
            StatusVM = new StatusViewModel();

            CurrentView = PersonalVM;

            PersonalViewCommand = new RelayCommand(o =>
            {
                CurrentView = PersonalVM;
            });

            StatusViewCommand = new RelayCommand(o =>
            {
                CurrentView = StatusVM;
            });
        }
    }
}

