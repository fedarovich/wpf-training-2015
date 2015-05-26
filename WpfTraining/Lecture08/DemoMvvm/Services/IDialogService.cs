using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMvvm.Services
{
    public interface IDialogService
    {
        bool? ConfirmYesNoCancel(string message);

        void ShowError(string error);

        string ShowOpenDialog(string initialDirectory, string filter);

        string ShowSaveDialog(string initialDirectory, string filter, string fileName);
    }
}
