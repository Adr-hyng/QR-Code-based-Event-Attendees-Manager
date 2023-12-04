using QEAMApp.Core;
using QEAMApp.MVVM.CEventHandler;
using System;
using System.Windows;

namespace QEAMApp.MVVM.ViewModel
{
    internal class MainViewModel: ViewModelBase
    {
        public readonly NavigationStore _navigationStore;
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        private string _snackBarImageSource;
        public string SnackBarImageSource
        {
            get { return _snackBarImageSource; }
            set
            {
                _snackBarImageSource = value;
                OnPropertyChanged(nameof(SnackBarImageSource));
            }
        }

        private String _snackBarContent;
        public String SnackBarContent
        {
            get
            {
                return _snackBarContent;
            }
            set
            {
                _snackBarContent = value;
                OnPropertyChanged(nameof(SnackBarContent));
            }
        }

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            SnackBarImageSource = "/QEAMApp;component/Images/success_snackbar.png";
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public event EventHandler<ShowSnackBarEventArgs> ShowSnackBarEvent;

        public void ShowSnackBar(String content, double duration, bool successIcon = true)
        {
            SnackBarContent = content;
            SnackBarImageSource = successIcon ? "/QEAMApp;component/Images/success_snackbar.png" : "/QEAMApp;component/Images/failed_snackbar.png";
            ShowSnackBarEvent?.Invoke(this, new ShowSnackBarEventArgs(duration));
        }
    }
}

