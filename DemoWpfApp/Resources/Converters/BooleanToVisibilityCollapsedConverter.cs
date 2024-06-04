using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace DemoWpfApp.Resources.Converters
{
    public class BooleanToVisibilityCollapsedConverter : MarkupExtension, IValueConverter
    {
        private static BooleanToVisibilityCollapsedConverter s_converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return s_converter ?? (s_converter = new BooleanToVisibilityCollapsedConverter());
        }
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}