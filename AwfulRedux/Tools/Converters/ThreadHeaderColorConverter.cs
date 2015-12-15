using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using AwfulRedux.UI.Models.Threads;

namespace AwfulRedux.Tools.Converters
{
    public class ThreadHeaderColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var item = (bool)value;
            if (item) return new SolidColorBrush(Colors.Yellow);
            return new SolidColorBrush(Color.FromArgb(255, 65, 91, 100));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
