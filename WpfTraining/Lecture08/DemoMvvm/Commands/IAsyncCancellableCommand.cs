using System.Windows.Input;

namespace DemoMvvm
{
    public interface IAsyncCancellableCommand
    {
        ICommand CancelCommand { get; }
    }
}
