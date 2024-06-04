using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DemoWpfApp.Resources.Converters
{
    public class ObjectNullToVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static ObjectNullToVisibilityConverter s_converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return s_converter ?? (s_converter = new ObjectNullToVisibilityConverter());
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value != null ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ObjectNotNullToVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static ObjectNotNullToVisibilityConverter _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new ObjectNotNullToVisibilityConverter());
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
