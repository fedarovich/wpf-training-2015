using System.Windows;
using Microsoft.Win32;

namespace MvvmLightDemo.Services.Implementation
{
    public class DialogService : IDialogService
    {
        public bool? ConfirmYesNoCancel(string message)
        {
            var result = MessageBox.Show(message, "Confirmation", MessageBoxButton.YesNoCancel,
                MessageBoxImage.Question);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    return true;
                case MessageBoxResult.No:
                    return false;
                default:
                    return null;
            }
        }

        public void ShowError(string error)
        {
            MessageBox.Show(error, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public string ShowOpenDialog(string initialDirectory, string filter)
        {
            var dialog = new OpenFileDialog
            {
                InitialDirectory = initialDirectory,
                Filter = filter
            };
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }

        public string ShowSaveDialog(string initialDirectory, string filter, string fileName)
        {
            var dialog = new SaveFileDialog
            {
                InitialDirectory = initialDirectory,
                FileName = fileName,
                Filter = filter
            };
            return dialog.ShowDialog() == true ? dialog.FileName : null;
        }
    }
}
