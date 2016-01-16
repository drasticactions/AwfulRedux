﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using AwfulRedux.Core.Managers;
using AwfulRedux.UI.Models.Threads;
using AwfulRedux.ViewModels;
using Newtonsoft.Json;
using Template10.Common;
using Template10.Controls;
using Template10.Services.NavigationService;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace AwfulRedux.Views
{
    public sealed partial class Shell
    {
        public static Shell Instance { get; set; }
        private static WindowWrapper Window { get; set; }
        public static HamburgerMenu HamburgerMenu => Instance.MyHamburgerMenu;

        // strongly-typed view models enable x:bind
        public ShellViewModel ViewModel => this.DataContext as ShellViewModel;

        public void SetNav(NavigationService navigationService)
        {
            MyHamburgerMenu.NavigationService = navigationService;
        }

        public Shell(NavigationService navigationService)
        {
            Instance = this;
            this.InitializeComponent();

            // setup for static calls
            Window = WindowWrapper.Current();
            MyHamburgerMenu.NavigationService = navigationService;
            ViewModel.WebManager = new WebManager();

            // any nav change, reset to normal
            //navigationService.FrameFacade.Navigated += (s, e) =>
            //    BusyModal.IsModal  = LoginModal.IsModal = false;
        }

        #region Busy

        public static void ShowBusy(bool busy, string text = null)
        {
            Window.Dispatcher.Dispatch(() =>
            {
                Instance.BusyText.Text = text ?? string.Empty;
                Instance.BusyModal.IsModal = busy;
            });
        }

        #endregion
    }
}
