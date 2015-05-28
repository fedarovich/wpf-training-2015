using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Caliburn.Micro.Extras;

namespace CaliburnMicroDemo.ViewModels
{
    public class ShellViewModel : Conductor<DocumentViewModel>.Collection.OneActive
    {
        private readonly Func<string, DocumentViewModel> documentViewModelFactory;
        private readonly Func<CalculatorViewModel> calculatorViewModelFactory;
        private readonly Func<AboutViewModel> aboutViewModelFactory;
        private readonly IOpenFileService openFileService;
        private readonly IMessageService messageService;
        private readonly IWindowManager windowManager;

        public ShellViewModel(
            Func<string, DocumentViewModel> documentViewModelFactory,
            Func<CalculatorViewModel> calculatorViewModelFactory,
            Func<AboutViewModel> aboutViewModelFactory,
            IOpenFileService openFileService,
            IMessageService messageService,
            IWindowManager windowManager)
        {
            this.documentViewModelFactory = documentViewModelFactory;
            this.calculatorViewModelFactory = calculatorViewModelFactory;
            this.aboutViewModelFactory = aboutViewModelFactory;
            this.openFileService = openFileService;
            this.messageService = messageService;
            this.windowManager = windowManager;
            DisplayName = "Caliburn Micro Demo";
        }


        public void NewDocument()
        {
            var viewModel = documentViewModelFactory(null);
            Items.Add(viewModel);
            ActivateItem(viewModel);
        }

        public void OpenDocument()
        {
            try
            {
                openFileService.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileService.Filter = "Text|*.txt|All files|*.*";
                if (!openFileService.DetermineFile())
                    return;

                var path = openFileService.File.FullName;
                var document = documentViewModelFactory(path);
                Items.Add(document);
                ActivateItem(document);
            }
            catch (Exception ex)
            {
                messageService.Show(ex.Message, "Error", icon: MessageImage.Error);
            }
        }

        public void ShowCalculator()
        {
            var viewModel = calculatorViewModelFactory();
            windowManager.ShowDialog(viewModel);
        }

        public void About()
        {
            var viewModel = aboutViewModelFactory();
            windowManager.ShowDialog(viewModel);
        }
    }
}
