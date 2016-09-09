// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace AwfulRedux_iOS
{
    [Register ("ThreadWebViewController")]
    partial class ThreadWebViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ThreadWe { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIWebView ThreadWebView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ThreadWe != null) {
                ThreadWe.Dispose ();
                ThreadWe = null;
            }

            if (ThreadWebView != null) {
                ThreadWebView.Dispose ();
                ThreadWebView = null;
            }
        }
    }
}