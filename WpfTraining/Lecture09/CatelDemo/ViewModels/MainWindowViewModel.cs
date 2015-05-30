using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Catel.Data;
using Catel.IoC;
using Catel.Services;

namespace CatelDemo.ViewModels
{
    using Catel.MVVM;

    /// <summary>
    /// MainWindow view model.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ITypeFactory typeFactory;
        private readonly IOpenFileService openFileService;
        private readonly IMessageService messageService;
        private readonly INavigationService navigationService;
        private readonly IUIVisualizerService uiVisualizerService;
        private readonly ObservableCollection<DocumentViewModel> documents = new ObservableCollection<DocumentViewModel>();
        
        private bool forceClose;
        private bool isClosing;

        public MainWindowViewModel(ITypeFactory typeFactory, 
            IOpenFileService openFileService,
            IMessageService messageService,
            INavigationService navigationService,
            IUIVisualizerService uiVisualizerService)
        {
            this.typeFactory = typeFactory;
            this.openFileService = openFileService;
            this.messageService = messageService;
            this.navigationService = navigationService;
            this.uiVisualizerService = uiVisualizerService;
            NewDocumentCommand = new Command(NewDocumentCommandExecute);
            OpenDocumentCommand = new Command(OpenDocumentCommandEvecute);
            CloseDocumentCommand = new TaskCommand<DocumentViewModel>(CloseDocumentCommandExecute, CanCloseDocument);
            AboutCommand = new Command(AboutCommandExecute);
            navigationService.ApplicationClosing += OnApplicationClosing;
        }

        public IList<DocumentViewModel> Documents
        {
            get { return documents; }
        }

        #region SelectedDocument property

        /// <summary>
        /// Gets or sets the SelectedDocument value.
        /// </summary>
        public DocumentViewModel SelectedDocument
        {
            get { return GetValue<DocumentViewModel>(SelectedDocumentProperty); }
            set { SetValue(SelectedDocumentProperty, value); }
        }

        /// <summary>
        /// SelectedDocument property data.
        /// </summary>
        public static readonly PropertyData SelectedDocumentProperty = RegisterProperty("SelectedDocument", typeof (DocumentViewModel));

        #endregion

        #region Commands

        #region NewDocumentCommand

        public ICommand NewDocumentCommand { get; private set; }

        private void NewDocumentCommandExecute()
        {
            var viewModel = CreateDocumentViewModel(string.Empty);
            documents.Add(viewModel);
            SelectedDocument = viewModel;
        }

        #endregion

        #region OpenDocumentCommand

        public ICommand OpenDocumentCommand { get; private set; }

        private void OpenDocumentCommandEvecute()
        {
            try
            {
                openFileService.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileService.FileName = "";
                openFileService.Filter = "Text|*.txt|All files|*.*";
                if (!openFileService.DetermineFile())
                    return;

                string path = openFileService.FileName;
                if (string.IsNullOrWhiteSpace(path))
                    return;

                var document = CreateDocumentViewModel(path);
                documents.Add(document);
                SelectedDocument = document;
            }
            catch (Exception ex)
            {
                messageService.ShowError(ex.Message);
            }
        }

        #endregion

        #region CloseDocumentCommand

        public ICommand CloseDocumentCommand { get; private set; }

        private async Task CloseDocumentCommandExecute(DocumentViewModel viewModel)
        {
            await CloseDocument(viewModel);
        }

        private bool CanCloseDocument(DocumentViewModel viewModel)
        {
            return viewModel != null && !viewModel.IsClosed;
        }

        #endregion

        #region AboutCommand

        public ICommand AboutCommand { get; private set; }

        private void AboutCommandExecute()
        {
            var viewModel = typeFactory.CreateInstanceWithParametersAndAutoCompletion<AboutViewModel>();
            uiVisualizerService.ShowDialog(viewModel);
        }

        #endregion

        #endregion

        private DocumentViewModel CreateDocumentViewModel(string path)
        {
            return typeFactory.CreateInstanceWithParametersAndAutoCompletion<DocumentViewModel>(path);
        }

        private async void OnApplicationClosing(object sender, ApplicationClosingEventArgs e)
        {
            if (!forceClose)
            {
                e.Cancel = true;
                if (isClosing)
                    return;

                isClosing = true;
                if (await CloseDocuments())
                {
                    forceClose = true;
                    await Task.Yield();
                    navigationService.CloseApplication();
                    forceClose = false;
                }
                isClosing = false;
            }
        }

        private async Task<bool> CloseDocuments()
        {
            if (documents.All(x => !x.HasChanges))
                return true;

            var documentsToClose = documents.ToArray();
            bool[] closeResults = await Task.WhenAll(documentsToClose.Select(CloseDocument));
            return closeResults.All(x => x);
        }

        private async Task<bool> CloseDocument(DocumentViewModel viewModel)
        {
            if (viewModel == null)
                return false;

            bool close;
            if (viewModel.HasChanges)
            {
                var configmationResult = await messageService.Show(
                    string.Format("Do you want to save changes to the file \"{0}\"?", viewModel.Name),
                    "Confirmation",
                    MessageButton.YesNoCancel,
                    MessageImage.Question);

                switch (configmationResult)
                {
                    case MessageResult.Yes:
                        close = await viewModel.SaveAndCloseViewModel();
                        break;
                    case MessageResult.No:
                        close = await viewModel.CancelAndCloseViewModel();
                        break;
                    default:
                        close = false;
                        break;
                }
            }
            else
            {
                close = await viewModel.CancelAndCloseViewModel();
            }

            if (close)
            {
                Documents.Remove(viewModel);
            }
            return close;
        }
    }
}
