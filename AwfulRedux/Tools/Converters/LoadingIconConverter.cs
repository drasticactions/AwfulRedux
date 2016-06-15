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
            var darkLight = val == ApplicationTheme.Dark ? "_dark.gif" : ".gif";
            var throbberval = GetRandomInt(1,3);
            var stringResult = $"ms-appx:///Assets/Throbbers/throbber_{throbberval}{darkLight}";
            return stringResult;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        protected int GetRandomInt(int min, int max)
        {
            return App.Random.Next(min, max);
        }
    }
}
