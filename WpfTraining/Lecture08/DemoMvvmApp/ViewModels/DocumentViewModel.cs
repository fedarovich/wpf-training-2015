using System;
using System.IO;
using System.Text;
using System.Windows.Input;
using DemoMvvm;
using DemoMvvm.Services;
using DemoMvvmApp.Events;

namespace DemoMvvmApp.ViewModels
{
    public class DocumentViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly IEventAggregator eventAggregator;
        private static int documentCounter = 0;
        private string name;
        private string fullFileName;
        private bool hasChanges;
        private string text;

        public DocumentViewModel(string path, IDialogService dialogService, IEventAggregator eventAggregator)
        {
            this.dialogService = dialogService;
            this.eventAggregator = eventAggregator;

            CloseCommand = new DelegateCommand(() => Close());
            SaveCommand = new DelegateCommand(() => SaveFile(), () => HasChanges);
            SaveAsCommand = new DelegateCommand(() => SaveFileAs());

            if (string.IsNullOrWhiteSpace(path))
            {
                name = "Document " + (++documentCounter) + ".txt";
            }
            else
            {
                fullFileName = path;
                name = Path.GetFileName(path);
                text = File.ReadAllText(path);
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (value == name) return;
                name = value;
                RaisePropertyChanged();
            }
        }

        public bool HasChanges
        {
            get { return hasChanges || string.IsNullOrWhiteSpace(fullFileName); }
            set
            {
                if (value.Equals(hasChanges)) return;
                hasChanges = value;
                RaisePropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                if (value == text) return;
                text = value;
                RaisePropertyChanged();
                HasChanges = true;
            }
        }

        #region Commands

        public ICommand CloseCommand { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public ICommand SaveAsCommand { get; private set; }

        #endregion

        public bool Close()
        {
            if (HasChanges)
            {
                bool? confirmation = dialogService.ConfirmYesNoCancel(
                    string.Format("Save changes to file \"{0}\"?", name));

                if (confirmation == null || (confirmation == true && !SaveFile()))
                    return false;
            }
            eventAggregator.PublishOnCurrentThread(new DocumentClosedEvent(this));
            return true;
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
                RaisePropertyChanged(() => HasChanges);
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
    }
}
