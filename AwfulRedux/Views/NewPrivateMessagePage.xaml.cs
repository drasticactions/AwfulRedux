﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AwfulRedux.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewPrivateMessagePage : Page
    {
        public NewPrivateMessagePage()
        {
            this.InitializeComponent();
            SmiliesView.ViewModel.ReplyBox = ReplyText;
            ViewModel.Subject = Subject;
            ViewModel.PostIconViewModel = PostIconView.ViewModel;
            ViewModel.SmiliesViewModel = SmiliesView.ViewModel;
            ViewModel.ReplyBox = ReplyText;
            ViewModel.Recipient = Recipient;
        }

        // strongly-typed view models enable x:bind
        public NewPrivateMessageViewModel ViewModel => this.DataContext as NewPrivateMessageViewModel;
    }
}
