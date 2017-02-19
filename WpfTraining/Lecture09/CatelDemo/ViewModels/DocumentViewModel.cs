using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Catel.Data;
using Catel.Messaging;
using Catel.Services;

namespace CatelDemo.ViewModels
{
    using Catel.MVVM;

    /// <summary>
    /// UserControl view model.
    /// </summary>
    public class DocumentViewModel : ViewModelBase
    {
        private readonly IMessageService messageService;
        private readonly ISaveFileService saveFileService;
        private readonly IMessageMediator messageMediator;
        private static int documentCounter = 0;
        private string fullFileName;

        public DocumentViewModel(string path, 
            IMessageService messageService,
            ISaveFileService saveFileService,
            IMessageMediator messageMediator)
        {
            this.messageService = messageService;
            this.saveFileService = saveFileService;
            this.messageMediator = messageMediator;

            SaveCommand = new TaskCommand(SaveAsync, () => HasChanges);
            SaveAsCommand = new TaskCommand(SaveFileAs);

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


        /// <summary>
        /// Gets the title of the view model.
        /// </summary>
        /// <value>The title.</value>
        public override string Title => Name;

        #region Name property

        /// <summary>
        /// Gets or sets the Name value.
        /// </summary>
        public string Name
        {
            get { return GetValue<string>(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        /// <summary>
        /// Name property data.
        /// </summary>
        public static readonly PropertyData NameProperty = RegisterProperty("Name", typeof (string), null,
            (sender, e) => ((DocumentViewModel)sender).OnNameChanged());

        private void OnNameChanged()
        {
            RaisePropertyChanged(() => Title);
        }

        #endregion

        #region HasChanges property

        /// <summary>
        /// Gets or sets the HasChanges value.
        /// </summary>
        public bool HasChanges
        {
            get { return GetValue<bool>(HasChangesProperty); }
            set { SetValue(HasChangesProperty, value); }
        }

        /// <summary>
        /// HasChanges property data.
        /// </summary>
        public static readonly PropertyData HasChangesProperty = RegisterProperty("HasChanges", typeof (bool), null,
            (sender, e) => ((DocumentViewModel) sender).OnHasChangesChanged());

        /// <summary>
        /// Called when the HasChanges property has changed.
        /// </summary>
        private void OnHasChangesChanged()
        {
            System.Windows.Input.CommandManager.InvalidateRequerySuggested();
            SaveCommand.RaiseCanExecuteChanged();
        }

        #endregion

        #region Text property

        /// <summary>
        /// Gets or sets the Text value.
        /// </summary>
        public string Text
        {
            get { return GetValue<string>(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        /// <summary>
        /// Text property data.
        /// </summary>
        public static readonly PropertyData TextProperty = RegisterProperty("Text", typeof (string), null,
            (sender, e) => ((DocumentViewModel) sender).OnTextChanged());

        /// <summary>
        /// Called when the Text property has changed.
        /// </summary>
        private void OnTextChanged()
        {
            HasChanges = true;
        }

        #endregion

        #region Commands

        public TaskCommand SaveCommand { get; private set; }

        public TaskCommand SaveAsCommand { get; private set; }

        #endregion

        protected override async Task<bool> SaveAsync()
        {
            string path = fullFileName;
            if (string.IsNullOrWhiteSpace(path))
            {
                path = GetSavePath(null, Name);
                if (string.IsNullOrWhiteSpace(path))
                    return false;
            }

            return await SaveFileInternal(path);
        }

        private async Task SaveFileAs()
        {
            var path = GetSavePath(fullFileName, Name);
            if (string.IsNullOrWhiteSpace(path))
                return;

            await SaveFileInternal(path);
        }

        private async Task<bool> SaveFileInternal(string path)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true))
                using (var writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    await writer.WriteAsync(Text);
                }
                fullFileName = path;
                Name = Path.GetFileName(path);
                HasChanges = false;
                RaisePropertyChanged(() => HasChanges);
                return true;
            }
            catch (Exception ex)
            {
                await messageService.ShowErrorAsync(ex.Message);
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
            saveFileService.FileName = fileName;
            return saveFileService.DetermineFile() ? saveFileService.FileName : null;
        }
    }
}
