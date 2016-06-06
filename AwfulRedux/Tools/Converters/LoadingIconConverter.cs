using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using AwfulRedux.Services.SettingsServices;

namespace AwfulRedux.Tools.Converters
{
    public class LoadingIconConverter : IValueConverter
    {
        Template10.Services.SettingsService.ISettingsHelper _helper;

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            _helper = new Template10.Services.SettingsService.SettingsHelper();
            var theme = ApplicationTheme.Light;
            var themevalue = _helper.Read<string>("AppTheme", theme.ToString());
            var val = Enum.TryParse<ApplicationTheme>(themevalue, out theme) ? theme : ApplicationTheme.Light;
            var stringResult = val ==
                    ApplicationTheme.Dark ? "ms-appx:///Assets/awful-anime-dark.gif" : "ms-appx:///Assets/awful-anime.gif";
            return stringResult;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
