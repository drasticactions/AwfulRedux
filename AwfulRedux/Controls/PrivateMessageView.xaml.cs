﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.UI.Models.Messages;
using AwfulRedux.UI.Models.Threads;
using AwfulRedux.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AwfulRedux.Controls
{
    public sealed partial class PrivateMessageView : UserControl
    {
        public PrivateMessageView()
        {
            this.InitializeComponent();
        }

        public PrivateMessageViewModel ViewModel => this.DataContext as PrivateMessageViewModel;

        public async Task LoadPrivateMessage(PrivateMessage thread)
        {
            ViewModel.Selected = thread;
            await ViewModel.LoadPrivateMessage();
        }
    }
}
