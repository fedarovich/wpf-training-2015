using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Weather.Converters
{
    public class TaskStatusToDataTemlateConverter : IValueConverter
    {
        public DataTemplate EmptyTemplate { get; set; }

        public DataTemplate ProgressTemplate { get; set; }

        public DataTemplate SuccessfullyCompletedTemplate { get; set; }

        public DataTemplate CancelledTemplate { get; set; }

        public DataTemplate ErrorTemplate { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var taskStatus = value as TaskStatus?;
            switch (taskStatus)
            {
                case TaskStatus.Created:
                case TaskStatus.WaitingForActivation:
                case TaskStatus.WaitingToRun:
                case TaskStatus.Running:
                case TaskStatus.WaitingForChildrenToComplete:
                {
                    return ProgressTemplate ?? EmptyTemplate;
                }
                case TaskStatus.RanToCompletion:
                {
                    return SuccessfullyCompletedTemplate ?? EmptyTemplate;
                }
                case TaskStatus.Canceled:
                {
                    return CancelledTemplate ?? EmptyTemplate;
                }
                case TaskStatus.Faulted:
                {
                    return ErrorTemplate ?? EmptyTemplate;
                }
                default:
                {
                    return EmptyTemplate;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
