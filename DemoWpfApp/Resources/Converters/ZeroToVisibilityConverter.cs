using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DemoWpfApp.Resources.Converters
{
    public class ZeroToVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static ZeroToVisibilityConverter s_converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return s_converter ?? (s_converter = new ZeroToVisibilityConverter());
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var number = (int)value;

            return number == 0 ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
