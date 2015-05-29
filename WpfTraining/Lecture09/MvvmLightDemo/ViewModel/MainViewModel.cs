using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MvvmLightDemo.Messages;
using MvvmLightDemo.Services;

namespace MvvmLightDemo.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Func<string, DocumentViewModel> documentViewModelFactory;
        private readonly IDialogService dialogService;

        private readonly ObservableCollection<DocumentViewModel> documents = new ObservableCollection<DocumentViewModel>();
        private DocumentViewModel selectedDocument;

        public MainViewModel(
            Func<string, DocumentViewModel> documentViewModelFactory, 
            IDialogService dialogService)
        {
            this.documentViewModelFactory = documentViewModelFactory;
            this.dialogService = dialogService;
            MessengerInstance.Register<DocumentClosedMessage>(this, OnDocumentClosed);

            NewDocumentCommand = new RelayCommand(NewDocumentCommandExecute);
            OpenDocumentCommand = new RelayCommand(OpenDocumentCommandEvecute);
            ClosingCommand = new RelayCommand<CancelEventArgs>(OnClosing);
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

        #region ClosingCommand

        public ICommand ClosingCommand { get; private set; }

        private void OnClosing(CancelEventArgs args)
        {
            var documents = Documents.ToList();
            args.Cancel = !documents.All(x => x.Close());
        }

        #endregion

        #endregion

        #region Message Handlers

        private void OnDocumentClosed(DocumentClosedMessage message)
        {
            documents.Remove(message.Document);
        }

        #endregion
    }
}