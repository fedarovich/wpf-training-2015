using System.Reflection;
using System.Windows.Input;
using Catel.MVVM;

namespace CatelDemo.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel()
        {
            Copyright = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
            Title = "About";
            CloseCommand = new TaskCommand(CancelAndCloseViewModel);
        }

        public string Copyright { get; private set; }

        public ICommand CloseCommand { get; private set; }
    }
}
