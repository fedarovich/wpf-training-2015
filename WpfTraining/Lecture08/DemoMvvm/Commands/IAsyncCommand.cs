using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Nito.AsyncEx;

namespace DemoMvvm
{
    public interface IAsyncCommand : ICommand, INotifyPropertyChanged
    {
        Task ExecuteAsync(object parameter);

        INotifyTaskCompletion Execution { get; }
    }
}
