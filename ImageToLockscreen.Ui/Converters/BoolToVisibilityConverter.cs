using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace ImageToLockscreen.Ui.Converters
{
    public sealed class BoolToVisibilityConverter : IValueConverter
    {
        private static readonly IReadOnlyDictionary<object, string> Visibility = new Dictionary<object, string>()
        {
            {string.Empty, "Collapsed" },
            {true, "Visible" },
            {false, "Hidden" }
        };
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BoolToVisibilityConverter.Visibility[value ?? string.Empty];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("BoolToVisibilityConverter ConvertBack Not Implemented");
        }
    }
}
