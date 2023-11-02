﻿using QEAMApp.Core;
using QEAMApp.MVVM.Command;
using QEAMApp.MVVM.Model;
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
        private bool _isReadOnly;
        public bool IsReadOnly
        {
            get
            {
                return _isReadOnly;
            }
            set
            {
                _isReadOnly = value;
                OnPropertyChanged(nameof(IsReadOnly));
            }
        }
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

        public IdleScreenViewModel(NavigationService GoToFoundScreenNavigation, ApiService InstanceAPI)
        {
            ScanningCommand = new UserFoundCommand(GoToFoundScreenNavigation, this, InstanceAPI);
        }
    }
}
