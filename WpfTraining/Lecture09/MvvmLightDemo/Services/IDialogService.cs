namespace MvvmLightDemo.Services
{
    public interface IDialogService
    {
        bool? ConfirmYesNoCancel(string message);

        void ShowError(string error);

        string ShowOpenDialog(string initialDirectory, string filter);

        string ShowSaveDialog(string initialDirectory, string filter, string fileName);
    }
}
