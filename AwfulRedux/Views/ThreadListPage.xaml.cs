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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.UI.Models.Threads;
using AwfulRedux.ViewModels;

namespace AwfulRedux.Views
{
    public sealed partial class ThreadListPage : Page, IDisposable
    {
        public ThreadListPage()
        {
            this.InitializeComponent();
            XamlAnimatedGif.AnimationBehavior.Loaded += AnimationBehaviorOnLoaded;
        }

        private void AnimationBehaviorOnLoaded(object sender, EventArgs eventArgs)
        {
            // Is the image actually a gif?
            var newImage = (Image)sender;
            if (((newImage.ActualWidth == 0) || (newImage.ActualHeight == 0)) && newImage.Source is WriteableBitmap)
            {
                var bitmap = (WriteableBitmap)newImage.Source;
                if (bitmap.PixelBuffer.Length < 10000)
                {
                    XamlAnimatedGif.AnimationBehavior.SetAutoStart(newImage, true);
                }
                newImage.Width = bitmap.PixelWidth;
                newImage.Height = bitmap.PixelHeight;
                newImage.MaxHeight = 500;
                newImage.MaxWidth = 500;
            }
        }


        // strongly-typed view models enable x:bind
        public ThreadListPageViewModel ViewModel => this.DataContext as ThreadListPageViewModel;

        private async void ThreadList_OnClick(object sender, ItemClickEventArgs e)
        {
            await ViewModel.LoadThread(e.ClickedItem as Thread);
        }

        public void Dispose()
        {
            XamlAnimatedGif.AnimationBehavior.Loaded -= AnimationBehaviorOnLoaded;
        }
    }
}
