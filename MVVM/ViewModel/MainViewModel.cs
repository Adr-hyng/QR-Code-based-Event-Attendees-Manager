using QEAMApp.Core;
using System;
using System.Windows;

namespace QEAMApp.MVVM.ViewModel
{
    internal class MainViewModel: ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Window _window;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}

