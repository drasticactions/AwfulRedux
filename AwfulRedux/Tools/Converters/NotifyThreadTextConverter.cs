using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using AwfulRedux.UI.Models.Threads;

namespace AwfulRedux.Tools.Converters
{
    public class NotifyThreadTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var thread = value as Thread;
            if (thread == null)
            {
                return string.Empty;
            }
            return thread.IsNotified ? "Remove from Notification List" : "Add To Notification List";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
