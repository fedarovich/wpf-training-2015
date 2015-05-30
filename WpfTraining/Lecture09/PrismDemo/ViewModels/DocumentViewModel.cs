using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using Microsoft.Practices.Prism;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using PrismDemo.Events;
using PrismDemo.Services;

namespace PrismDemo.ViewModels
{
    public class DocumentViewModel : BindableBase, INavigationAware, IActiveAware
    {
        private readonly IDialogService dialogService;
        private readonly IEventAggregator eventAggregator;
        private static int documentCounter = 0;
        private string name;
        private string fullFileName;
        private bool hasChanges;
        private string text;
        private bool loaded;
        private bool isActive;

        public DocumentViewModel(IDialogService dialogService, IEventAggregator eventAggregator)
        {
            this.dialogService = dialogService;
            this.eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(() => SaveFile(), CanSaveFile);
            SaveAsCommand = new DelegateCommand(() => SaveFileAs());
            CloseCommand = new DelegateCommand<CancelEventArgs>(CloseCommandExecute);

            Commands.CloseCommand.RegisterCommand(CloseCommand);
            Commands.SaveCommand.RegisterCommand(SaveCommand);
            Commands.SaveAsCommand.RegisterCommand(SaveAsCommand);
        }

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public string Text
        {
            get { return text; }
            set
            {
                SetProperty(ref text, value);
                HasChanges = true;
            }
        }

        public bool HasChanges
        {
            get { return hasChanges; }
            set
            {
                SetProperty(ref hasChanges, value);
                SaveCommand.RaiseCanExecuteChanged();
            }
        }

        #region Commands

        public DelegateCommand<CancelEventArgs> CloseCommand { get; private set; }

        public DelegateCommand SaveCommand { get; private set; }

        public DelegateCommand SaveAsCommand { get; private set; }

        #endregion    

        #region Methods

        public bool Close()
        {
            if (HasChanges)
            {
                bool? confirmation = dialogService.ConfirmYesNoCancel(
                    string.Format("Save changes to file \"{0}\"?", name));

                if (confirmation == null || (confirmation == true && !SaveFile()))
                    return false;
            }
            UnregisterCommands();
            eventAggregator.GetEvent<DocumentClosedEvent>().Publish(this);
            return true;
        }

        private void UnregisterCommands()
        {
            Commands.CloseCommand.UnregisterCommand(CloseCommand);
            Commands.SaveCommand.UnregisterCommand(SaveCommand);
            Commands.SaveAsCommand.UnregisterCommand(SaveAsCommand);
        }

        private void CloseCommandExecute(CancelEventArgs args)
        {
            bool closed = Close();
            if (args != null)
            {
                args.Cancel = !closed;
            }
        }

        private bool SaveFile()
        {
            string path = fullFileName;
            if (string.IsNullOrWhiteSpace(path))
            {
                path = GetSavePath(null, name);
                if (string.IsNullOrWhiteSpace(path))
                    return false;
            }

            return SaveFileInternal(path);
        }

        private bool CanSaveFile()
        {
            return HasChanges;
        }

        private bool SaveFileAs()
        {
            var path = GetSavePath(fullFileName, name);
            if (string.IsNullOrWhiteSpace(path))
                return false;

            return SaveFileInternal(path);
        }

        private bool SaveFileInternal(string path)
        {
            try
            {
                File.WriteAllText(path, Text, Encoding.UTF8);
                fullFileName = path;
                Name = Path.GetFileName(path);
                HasChanges = false;
                OnPropertyChanged(() => HasChanges);
                return true;
            }
            catch (Exception ex)
            {
                dialogService.ShowError(ex.Message);
                return false;
            }
        }

        private string GetSavePath(string initialDirectory, string fileName)
        {
            if (string.IsNullOrWhiteSpace(initialDirectory))
            {
                initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }

            return dialogService.ShowSaveDialog(initialDirectory, "Text|*.txt|All Files|*.*", fileName);
        }

        #endregion

        #region INavigationAware

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (loaded)
                return;

            loaded = true;
            var path = navigationContext.Parameters["path"] as string;

            if (string.IsNullOrWhiteSpace(path))
            {
                Name = "Document " + (++documentCounter) + ".txt";
                HasChanges = true;
            }
            else
            {
                fullFileName = path;
                Name = Path.GetFileName(path);
                Text = File.ReadAllText(path);
                HasChanges = false;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            if (string.IsNullOrWhiteSpace(fullFileName))
                return false;

            var path = navigationContext.Parameters["path"] as string;
            return StringComparer.InvariantCultureIgnoreCase.Equals(fullFileName, path);
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        #endregion

        #region IActiveAware

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    OnPropertyChanged(() => IsActive);
                    OnIsActiveChanged();
                }
            }
        }

        private void OnIsActiveChanged()
        {
            SaveCommand.IsActive = IsActive;
            SaveAsCommand.IsActive = IsActive;

            var handler = IsActiveChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler IsActiveChanged;

        #endregion
    }
}
