using System;
using Windows.UI.Xaml.Data;

namespace DrawboardProjects.Converters
{
    /// <summary>
    /// Value converter that applies NOT operator to a Boolean value.
    /// </summary>
    public sealed class BoolNegationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool b && b);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !(value is bool b && b);
        }
    }
}
