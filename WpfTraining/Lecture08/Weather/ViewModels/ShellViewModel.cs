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
        private string city;
        private readonly OpenWeatherMapClient client;
        private INotifyTaskCompletion<CurrentWeatherResponse> currentWeather; 

        public ShellViewModel()
        {
            client = new OpenWeatherMapClient();
            GetCurrentWeatherCommand = new AsyncCommand(GetCurrentWeatherCommandExecute);
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

        private async Task GetCurrentWeatherCommandExecute()
        {
            var weatherTask = client.CurrentWeather.GetByName(City);
            CurrentWeather = NotifyTaskCompletion.Create(weatherTask);
            await CurrentWeather.Task;
        }
    }
}
