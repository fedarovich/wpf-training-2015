﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace CustomMarkupExtensions.ViewModels
{
    public class ViewModelBindingViewModel : INotifyPropertyChanged
    {
        private string newName;

        public ViewModelBindingViewModel()
        {
            Names = new ObservableCollection<string>
            {
                "Alice",
                "Bob",
                "Charlie"
            };

            AddCommand = new RelayCommand(
                () =>
                {
                    Names.Add(NewName);
                    NewName = string.Empty;
                });

            RemoveCommand = new RelayCommand<string>(n => Names.Remove(n));
        }

        public ObservableCollection<string> Names { get; private set; }

        public string NewName
        {
            get { return newName; }
            set
            {
                if (newName != value)
                {
                    newName = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand AddCommand { get; private set; }

        public ICommand RemoveCommand { get; private set; }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
