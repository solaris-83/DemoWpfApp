using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace DemoWpfApp.Resources.Converters
{
    public class BooleansOrConverter : MarkupExtension, IMultiValueConverter
    {
        private static BooleansOrConverter s_converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return s_converter ?? (s_converter = new BooleansOrConverter());
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = false;

            foreach (var v in values)
            {
                result |= (bool)v;
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleansAndConverter : MarkupExtension, IMultiValueConverter
    {
        private static BooleansAndConverter _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new BooleansAndConverter());
        }

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var result = true;

            foreach (var v in values)
            {
                result &= ((bool?)v) ?? false;
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanNotConverter : MarkupExtension, IValueConverter
    {
        private static BooleanNotConverter _converter;

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _converter ?? (_converter = new BooleanNotConverter());
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
