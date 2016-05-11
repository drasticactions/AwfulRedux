using System;
using System.Globalization;
using Xamarin.Forms;

namespace AwfulRedux.Mobile.Tools.Converters
{
    public class HasSeenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var thread = (bool)value;
            return App.IsLoggedIn && thread;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
