namespace DemoMvvm.Services
{
    public interface IWindowManager
    {
        bool? ShowDialog(object viewModel);

        void ShowWindow(object viewModel);
    }
}
