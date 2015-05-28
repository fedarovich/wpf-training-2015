using Caliburn.Micro;

namespace CaliburnMicroDemo.ViewModels
{
    public class CalculatorViewModel : Screen
    {
        private int? sum;

        public CalculatorViewModel()
        {
            DisplayName = "Calculator";
        }

        public void Add(int? x, int? y)
        {
            Sum = x + y;
        }

        public int? Sum
        {
            get { return sum; }
            set
            {
                if (value == sum) return;
                sum = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
