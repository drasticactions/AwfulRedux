﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using AwfulRedux.UI.Models.Threads;

namespace AwfulRedux.Tools.Converters
{
    public class ForwardButtonEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var currentThread = (Thread)value;
            return currentThread.TotalPages != currentThread.CurrentPage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
