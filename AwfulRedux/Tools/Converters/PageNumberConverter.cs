﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using AwfulRedux.UI.Models.Threads;

namespace AwfulRedux.Tools.Converters
{
    public class PageNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var currentPage = (int)value;
            return $"{currentPage} / {Controls.ThreadView.Instance.ViewModel.Selected.TotalPages}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
