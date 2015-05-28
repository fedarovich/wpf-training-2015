using System.Reflection;
using Caliburn.Micro;

namespace CaliburnMicroDemo.ViewModels
{
    public class AboutViewModel : Screen
    {
        public AboutViewModel()
        {
            Copyright = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
            DisplayName = "About";
        }


        public string Copyright { get; private set; }
    }
}
