using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace DemoWpfApp.Resources.Converters
{
    public class TaskResultConverter : MarkupExtension, IValueConverter
    {
        private static TaskResultConverter s_converter;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Task<string> task)
            {
                return task.Status == TaskStatus.RanToCompletion ? task.Result : default;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return s_converter ?? (s_converter = new TaskResultConverter());
        }
    }
}
