using System.Reflection;
using DemoMvvm;

namespace DemoMvvmApp.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel()
        {
            Copyright = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
        }


        public string Copyright { get; private set; }
    }
}
