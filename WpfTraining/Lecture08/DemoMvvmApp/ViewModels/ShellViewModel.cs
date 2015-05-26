using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using DemoMvvm;
using DemoMvvm.Services;
using DemoMvvmApp.Events;

namespace DemoMvvmApp.ViewModels
{
    public class ShellViewModel : ViewModelBase, IEventSubscriber<DocumentClosedEvent>
    {
        private readonly Func<string, DocumentViewModel> documentViewModelFactory;
        private readonly Func<AboutViewModel> aboutViewModelFactory;
        private readonly IEventAggregator eventAggregator;
        private readonly IDialogService dialogService;
        private readonly IWindowManager windowManager;

        private readonly ObservableCollection<DocumentViewModel> documents = new ObservableCollection<DocumentViewModel>();
        private DocumentViewModel selectedDocument;

        public ShellViewModel(
            Func<string, DocumentViewModel> documentViewModelFactory, 
            Func<AboutViewModel> aboutViewModelFactory,
            IEventAggregator eventAggregator,
            IDialogService dialogService,
            IWindowManager windowManager)
        {
            this.documentViewModelFactory = documentViewModelFactory;
            this.aboutViewModelFactory = aboutViewModelFactory;
            this.eventAggregator = eventAggregator;
            this.dialogService = dialogService;
            this.windowManager = windowManager;
            eventAggregator.Subscribe(this);

            NewDocumentCommand = new DelegateCommand(NewDocumentCommandExecute);
            OpenDocumentCommand = new DelegateCommand(OpenDocumentCommandEvecute);
            AboutCommand = new DelegateCommand(AboutCommandExecute);
            ClosingCommand = new DelegateCommand<CancelEventArgs>(OnClosing);
        }

        public IList<DocumentViewModel> Documents
        {
            get { return documents; }
        }

        public DocumentViewModel SelectedDocument
        {
            get { return selectedDocument; }
            set
            {
                if (Equals(value, selectedDocument)) return;
                selectedDocument = value;
                RaisePropertyChanged();
            }
        }

        #region Commands

        #region NewDocumentCommand

        public ICommand NewDocumentCommand { get; private set; }

        private void NewDocumentCommandExecute()
        {
            var viewModel = documentViewModelFactory(null);
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
                string initialDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string path = dialogService.ShowOpenDialog(initialDir, "Text|*.txt|All files|*.*");
                if (string.IsNullOrWhiteSpace(path))
                    return;

                var document = documentViewModelFactory(path);
                documents.Add(document);
                SelectedDocument = document;
            }
            catch (Exception ex)
            {
                dialogService.ShowError(ex.Message);
            }
        }

        #endregion

        #region AboutCommand

        public ICommand AboutCommand { get; private set; }

        private void AboutCommandExecute()
        {
            var aboutView = aboutViewModelFactory();
            windowManager.ShowDialog(aboutView);
        }

        #endregion

        #region ClosingCommand

        public ICommand ClosingCommand { get; private set; }

        private void OnClosing(CancelEventArgs args)
        {
            var documents = Documents.ToList();
            args.Cancel = !documents.All(x => x.Close());
        }

        #endregion

        #endregion

        #region IEventSubscriber<DocumentClosedEvent> members

        public void ReceiveEvent(DocumentClosedEvent @event)
        {
            documents.Remove(@event.Document);
        }

        #endregion
    }
}
