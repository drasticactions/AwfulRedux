using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace AwfulRedux.Tools.Converters
{
    public class HasSeenThreadConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? Application.Current.Resources["HasSeenThreadColor"] as SolidColorBrush : Application.Current.Resources["ThreadColor"] as SolidColorBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
