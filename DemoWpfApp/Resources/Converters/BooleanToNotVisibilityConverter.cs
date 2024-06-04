using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;

namespace DemoWpfApp.Resources.Converters
{
    public class BooleanToNotVisibilityConverter : MarkupExtension, IValueConverter
    {
        private static BooleanToNotVisibilityConverter s_converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return s_converter ?? (s_converter = new BooleanToNotVisibilityConverter());
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (bool)value ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
