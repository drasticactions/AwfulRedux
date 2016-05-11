using System;
using System.Globalization;
using Xamarin.Forms;

namespace AwfulRedux.Mobile.Tools.Converters
{
    public class ThreadAnnounceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // If value is IsAnnounce, return true;
            if (!(value is bool)) return Color.Green;
            if ((bool)value)
            {
                return Color.Yellow;
            }
            return Color.Green;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
