using System;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AwfulRedux.Controls
{
    public sealed partial class PreviewView : UserControl
    {
        public PreviewView()
        {
            this.InitializeComponent();
        }

        // strongly-typed view models enable x:bind
        public PreviewViewModel ViewModel => this.DataContext as PreviewViewModel;
    }
}
