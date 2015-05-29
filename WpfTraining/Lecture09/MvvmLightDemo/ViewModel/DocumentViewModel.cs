using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using MvvmLightDemo.Messages;
using MvvmLightDemo.Services;

namespace MvvmLightDemo.ViewModel
{
    public class DocumentViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private static int documentCounter = 0;
        private string name;
        private string fullFileName;
        private bool hasChanges;
        private string text;

        public DocumentViewModel(string path, IDialogService dialogService)
        {
            this.dialogService = dialogService;

            CloseCommand = new RelayCommand(() => Close());
            SaveCommand = new RelayCommand(() => SaveFile(), () => HasChanges);
            SaveAsCommand = new RelayCommand(() => SaveFileAs());

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
            set { Set(ref name, value); }
        }

        public bool HasChanges
        {
            get { return hasChanges || string.IsNullOrWhiteSpace(fullFileName); }
            set
            {
                Set(ref hasChanges, value);
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                Set(ref text, value);
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
            MessengerInstance.Send(new DocumentClosedMessage(this));
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
