using System;
using System.Globalization;
using System.Windows;

namespace ResxResources
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            if (e.Args.Length == 0)
                return;

            try
            {
                var cultureName = e.Args[0];
                var culture = CultureInfo.GetCultureInfo(cultureName);
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }
            catch (Exception)
            {
            }
        }
    }
}
