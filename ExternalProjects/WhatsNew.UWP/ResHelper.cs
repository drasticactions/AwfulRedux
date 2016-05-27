using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsNew.UWP
{
    public class ResHelper
    {
        public static string GetResource(string key)
        {
            return Windows.ApplicationModel.Resources.Core.ResourceManager.Current.MainResourceMap.GetValue($"WhatsNew.UWP/Resources/{key}").ValueAsString;
        }
    }
}
