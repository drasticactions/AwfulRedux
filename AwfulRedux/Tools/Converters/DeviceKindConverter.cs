using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.RemoteSystems;
using Windows.UI.Xaml.Data;

namespace AwfulRedux.Tools.Converters
{
    public class DeviceKindConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            // Switch statements won't work with non-constant values. What the hell.
            var type = (string) value;
            if (type == RemoteSystemKinds.Desktop)
            {
                return "\uE977";
            }
            if (type == RemoteSystemKinds.Phone)
            {
                return "\uE8EA";
            }
            if (type == RemoteSystemKinds.Holographic)
            {
                return "\uEBD2";
            }
            if (type == RemoteSystemKinds.Xbox)
            {
                return "\uE990";
            }
            if (type == RemoteSystemKinds.Hub)
            {
                return "\uE8AE";
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
