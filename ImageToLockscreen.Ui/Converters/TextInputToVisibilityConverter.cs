using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ImageToLockscreen.Ui.Converters
{
    public sealed class TextInputToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int length = 0;

            int.TryParse(value?.ToString(), out length);

            if (length < 1)
                return Visibility.Visible;

            return Visibility.Hidden;
        }


        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
