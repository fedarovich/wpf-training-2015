using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using Caliburn.Micro.Extras;

namespace CaliburnMicroDemo.ViewModels
{
    public class DocumentViewModel : Screen
    {
        private readonly IMessageService messageService;
        private readonly ISaveFileService saveFileService;
        private readonly IEventAggregator eventAggregator;
        private static int documentCounter = 0;
        private string text;
        private string fullFileName;
        private bool hasChanges;

        public DocumentViewModel(string path, 
            IMessageService messageService, 
            ISaveFileService saveFileService,
            IEventAggregator eventAggregator)
        {
            this.messageService = messageService;
            this.saveFileService = saveFileService;
            this.eventAggregator = eventAggregator;

            if (string.IsNullOrWhiteSpace(path))
            {
                DisplayName = "Document " + (++documentCounter) + ".txt";
            }
            else
            {
                fullFileName = path;
                DisplayName = Path.GetFileName(path);
                text = File.ReadAllText(path);
            }
        }

        public bool HasChanges
        {
            get { return hasChanges || string.IsNullOrWhiteSpace(fullFileName); }
            set
            {
                if (value.Equals(hasChanges)) return;
                hasChanges = value;
                NotifyOfPropertyChange(string.Empty);
            }
        }

        public string Text
        {
            get { return text; }
            set
            {
                if (value == text) return;
                text = value;
                NotifyOfPropertyChange();
                HasChanges = true;
            }
        }

        public override void CanClose(Action<bool> callback)
        {
            if (HasChanges)
            {
                var confirmation = messageService.Show(
                    string.Format("Save changes to file \"{0}\"?", DisplayName),
                    "Confirmation",
                    MessageButton.YesNoCancel,
                    MessageImage.Question);

                if (confirmation == MessageResult.Cancel ||
                    (confirmation == MessageResult.Yes && !SaveFile()))
                {
                    callback(false);
                    return;
                }
            }
            callback(true);
        }

        public bool SaveFile()
        {
            string path = fullFileName;
            if (string.IsNullOrWhiteSpace(path))
            {
                path = GetSavePath(null, DisplayName);
                if (string.IsNullOrWhiteSpace(path))
                    return false;
            }

            return SaveFileInternal(path);
        }

        public bool CanSaveFile
        {
            get { return HasChanges; }
        }

        public bool SaveFileAs()
        {
            var path = GetSavePath(fullFileName, DisplayName);
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
                DisplayName = Path.GetFileName(path);
                HasChanges = false;
                NotifyOfPropertyChange(() => HasChanges);
                return true;
            }
            catch (Exception ex)
            {
                messageService.Show(ex.Message, "Error", icon: MessageImage.Error);
                return false;
            }
        }

        private string GetSavePath(string initialDirectory, string fileName)
        {
            if (string.IsNullOrWhiteSpace(initialDirectory))
            {
                initialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            }

            saveFileService.InitialDirectory = initialDirectory;
            saveFileService.Filter = "Text|*.txt|All Files|*.*";
            return saveFileService.DetermineFile() ? saveFileService.FileName : null;
        }
    }
}
