using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using DemoMvvm;
using Nito.AsyncEx;
using OpenWeatherMap;

namespace Weather.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private string city = string.Empty;
        private readonly OpenWeatherMapClient client;
        private INotifyTaskCompletion<CurrentWeatherResponse> currentWeather; 

        public ShellViewModel()
        {
            client = new OpenWeatherMapClient();
            // Variant 1: with delegate command.
            GetCurrentWeatherCommand = new DelegateCommand(GetCurrentWeatherCommandExecute);
            // Variant 2: with cancellable async command.
            //GetCurrentWeatherCommand = new AsyncCancellableCommand(GetCurrentWeatherCommandExecuteAsync);
        }

        public string City
        {
            get { return city; }
            set
            {
                if (value == city) return;
                city = value;
                RaisePropertyChanged();
            }
        }

        public INotifyTaskCompletion<CurrentWeatherResponse> CurrentWeather
        {
            get { return currentWeather; }
            set
            {
                if (Equals(value, currentWeather)) return;
                currentWeather = value;
                RaisePropertyChanged();
            }
        }

        public ICommand GetCurrentWeatherCommand { get; private set; }

        // Variant 1: with delegate command.
        private void GetCurrentWeatherCommandExecute()
        {
            CurrentWeather = NotifyTaskCompletion.Create(
                () => GetCurrentWeatherAsync(CancellationToken.None));
        }

        // Variant 2: with cancellable async command.
        private async Task GetCurrentWeatherCommandExecuteAsync(CancellationToken token)
        {
            CurrentWeather = NotifyTaskCompletion.Create(
                () => GetCurrentWeatherAsync(token));

            try
            {
                await CurrentWeather.Task;
            }
            catch
            {
                // Do nothing. Errors are handled in other place.
            }
        }

        private async Task<CurrentWeatherResponse> GetCurrentWeatherAsync(CancellationToken token)
        {
            await Task.Delay(3000, token);
            return await client.CurrentWeather.GetByName(City, MetricSystem.Metric);
        }
    }
}
