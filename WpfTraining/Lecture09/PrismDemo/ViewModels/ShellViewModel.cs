using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using PrismDemo.Events;
using PrismDemo.Services;

namespace PrismDemo.ViewModels
{
    public class ShellViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;
        private readonly IDialogService dialogService;
        private readonly IEventAggregator eventAggregator;
        private DocumentViewModel selectedDocument;

        public ShellViewModel(IRegionManager regionManager, 
            IDialogService dialogService, 
            IEventAggregator eventAggregator)
        {
            this.regionManager = regionManager;
            this.dialogService = dialogService;
            this.eventAggregator = eventAggregator;
            NewDocumentCommand = new DelegateCommand(NewDocumentCommandExecute);
            OpenDocumentCommand = new DelegateCommand(OpenDocumentCommandExecute);
            eventAggregator.GetEvent<DocumentClosedEvent>().Subscribe(CloseDocument, true);
        }

        public DocumentViewModel SelectedDocument
        {
            get { return selectedDocument; }
            set { SetProperty(ref selectedDocument, value); }
        }

        public ICommand NewDocumentCommand { get; private set; }

        public ICommand OpenDocumentCommand { get; private set; }

        private void NewDocumentCommandExecute()
        {
            OpenDocument(null);
        }
        
        private void OpenDocumentCommandExecute()
        {
            try
            {
                string initialDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string path = dialogService.ShowOpenDialog(initialDir, "Text|*.txt|All files|*.*");
                if (string.IsNullOrWhiteSpace(path))
                    return;

                OpenDocument(path);
            }
            catch (Exception ex)
            {
                dialogService.ShowError(ex.Message);
            }
        }

        private void OpenDocument(string path)
        {
            var parameters = new NavigationParameters
            {
                {"path", path ?? string.Empty}
            };

            regionManager.RequestNavigate(
                Regions.DocumentRegion,
                new Uri("Document" + parameters, UriKind.Relative));
        }

        private void CloseDocument(DocumentViewModel viewModel)
        {
            IRegion region = regionManager.Regions[Regions.DocumentRegion];
            var view = region.Views
                .Cast<IView>()
                .FirstOrDefault(x => Equals(x.DataContext, viewModel));
            if (view != null)
            {
                region.Remove(view);
            }
        }
    }
}
