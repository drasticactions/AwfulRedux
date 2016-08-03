using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://github.com/JustinXinLiu/PullToRefreshUWP_WindowsComposition

namespace AwfulRedux.Controls
{
    public sealed partial class PullToRefreshControl : UserControl
    {
        ScrollViewer _scrollViewer;
        Compositor _compositor;

        CompositionPropertySet _scrollerViewerManipulation;
        ExpressionAnimation _rotationAnimation, _opacityAnimation, _offsetAnimation;
        ScalarKeyFrameAnimation _resetAnimation, _loadingAnimation;

        Visual _borderVisual;
        Visual _refreshIconVisual;
        float _refreshIconOffsetY;
        const float REFRESH_ICON_MAX_OFFSET_Y = 36.0f;

        bool _refresh;
        DateTime _pulledDownTime, _restoredTime;

        public PullToRefreshControl()
        {
            this.InitializeComponent();

            this.Loaded += (s, e) =>
            {
                _scrollViewer = ListView.GetScrollViewer();
                _scrollViewer.DirectManipulationStarted += OnDirectManipulationStarted;
                _scrollViewer.DirectManipulationCompleted += OnDirectManipulationCompleted;

                // Retrieve the ScrollViewer manipulation and the Compositor.
                _scrollerViewerManipulation = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(_scrollViewer);
                _compositor = _scrollerViewerManipulation.Compositor;

                // At the moment there are three things happening when pulling down the list -
                // 1. The Refresh Icon fades in.
                // 2. The Refresh Icon rotates (400°).
                // 3. The Refresh Icon gets pulled down a bit (REFRESH_ICON_MAX_OFFSET_Y)
                // QUESTION 5
                // Can we also have Geometric Path animation so we can also draw the Refresh Icon along the way?
                //

                // Create a rotation expression animation based on the overpan distance of the ScrollViewer.
                _rotationAnimation = _compositor.CreateExpressionAnimation("min(max(0, ScrollManipulation.Translation.Y) * Multiplier, MaxDegree)");
                _rotationAnimation.SetScalarParameter("Multiplier", 10.0f);
                _rotationAnimation.SetScalarParameter("MaxDegree", 400.0f);
                _rotationAnimation.SetReferenceParameter("ScrollManipulation", _scrollerViewerManipulation);

                // Create an opacity expression animation based on the overpan distance of the ScrollViewer.
                _opacityAnimation = _compositor.CreateExpressionAnimation("min(max(0, ScrollManipulation.Translation.Y) / Divider, 1)");
                _opacityAnimation.SetScalarParameter("Divider", 30.0f);
                _opacityAnimation.SetReferenceParameter("ScrollManipulation", _scrollerViewerManipulation);

                // Create an offset expression animation based on the overpan distance of the ScrollViewer.
                _offsetAnimation = _compositor.CreateExpressionAnimation("(min(max(0, ScrollManipulation.Translation.Y) / Divider, 1)) * MaxOffsetY");
                _offsetAnimation.SetScalarParameter("Divider", 30.0f);
                _offsetAnimation.SetScalarParameter("MaxOffsetY", REFRESH_ICON_MAX_OFFSET_Y);
                _offsetAnimation.SetReferenceParameter("ScrollManipulation", _scrollerViewerManipulation);

                // Create a keyframe animation to reset properties like Offset.Y, Opacity, etc.
                _resetAnimation = _compositor.CreateScalarKeyFrameAnimation();
                _resetAnimation.InsertKeyFrame(1.0f, 0.0f);

                // Create a loading keyframe animation (in this case, a rotation animation). 
                _loadingAnimation = _compositor.CreateScalarKeyFrameAnimation();
                _loadingAnimation.InsertKeyFrame(1.0f, 360);
                _loadingAnimation.Duration = TimeSpan.FromMilliseconds(1200);
                _loadingAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

                // Get the RefreshIcon's Visual.
                _refreshIconVisual = ElementCompositionPreview.GetElementVisual(RefreshIcon);
                // Set the center point for the rotation animation.
                _refreshIconVisual.CenterPoint = new Vector3(Convert.ToSingle(RefreshIcon.ActualWidth / 2), Convert.ToSingle(RefreshIcon.ActualHeight / 2), 0);

                // Get the ListView's inner Border's Visual.
                var border = (Border)VisualTreeHelper.GetChild(ListView, 0);
                _borderVisual = ElementCompositionPreview.GetElementVisual(border);

                PrepareExpressionAnimationsOnScroll();
            };

            this.Unloaded += (s, e) =>
            {
                _scrollViewer.DirectManipulationStarted -= OnDirectManipulationStarted;
                _scrollViewer.DirectManipulationCompleted -= OnDirectManipulationCompleted;
            };
        }

        void OnDirectManipulationStarted(object sender, object e)
        {
            // QUESTION 1
            // I cannot think of a better way to monitor overpan changes, maybe there should be an Animating event?
            //
            Windows.UI.Xaml.Media.CompositionTarget.Rendering += OnCompositionTargetRendering;

            // Initialise the values.
            _refresh = false;
        }

        async void OnDirectManipulationCompleted(object sender, object e)
        {
            Windows.UI.Xaml.Media.CompositionTarget.Rendering -= OnCompositionTargetRendering;

            //Debug.WriteLine($"ScrollViewer Rollback animation duration: {(_restoredTime - _pulledDownTime).Milliseconds}");

            // The ScrollViewer's rollback animation is appx. 200ms. So if the duration between the two DateTimes we recorded earlier
            // is greater than 250ms, we should cancel the refresh.
            var cancelled = (_restoredTime - _pulledDownTime) > TimeSpan.FromMilliseconds(250);

            if (_refresh)
            {
                if (cancelled)
                {
                    Debug.WriteLine("Refresh cancelled...");

                    StartResetAnimations();
                }
                else
                {
                    Debug.WriteLine("Refresh now!!!");

                    await StartLoadingAnimation(() => StartResetAnimations());
                }
            }
        }

        async Task StartLoadingAnimation(Action completed)
        {
            // Create a short delay to allow the expression rotation animation to more smoothly transition
            // to the new keyframe animation
            await Task.Delay(100);

            _refreshIconVisual.StartAnimation("RotationAngleInDegrees", _loadingAnimation);

            await FakeServiceCall();
            completed();
        }

        void StartResetAnimations()
        {
            var batch = _compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            // Looks like expression aniamtions will be removed after the following keyframe
            // animations have run. So here I have to re-start them once the keyframe animations
            // are completed.
            batch.Completed += (s, e) => PrepareExpressionAnimationsOnScroll();

            _borderVisual.StartAnimation("Offset.Y", _resetAnimation);
            _refreshIconVisual.StartAnimation("Opacity", _resetAnimation);
            batch.End();
        }

        private void PrepareExpressionAnimationsOnScroll()
        {
            _refreshIconVisual.StartAnimation("RotationAngleInDegrees", _rotationAnimation);
            _refreshIconVisual.StartAnimation("Opacity", _opacityAnimation);
            _refreshIconVisual.StartAnimation("Offset.Y", _offsetAnimation);
            _borderVisual.StartAnimation("Offset.Y", _offsetAnimation);
        }

        async Task FakeServiceCall()
        {
            await Task.Delay(2000);

            //_list.Insert(0, Contact.GetContactsGrouped(1)[0]);
        }

        void OnCompositionTargetRendering(object sender, object e)
        {
            // QUESTION 2
            // What I've noticed is that I have to manually stop and
            // start the animation otherwise the Offset.Y is 0. Why?
            //
            _refreshIconVisual.StopAnimation("Offset.Y");

            // QUESTION 3
            // Why is the Translation always (0,0,0)?
            //
            //Vector3 translation;
            //var translationStatus = _scrollerViewerManipulation.TryGetVector3("Translation", out translation);
            //switch (translationStatus)
            //{
            //    case CompositionGetValueStatus.Succeeded:
            //        Debug.WriteLine($"ScrollViewer's Translation Y: {translation.Y}");
            //        break;
            //    case CompositionGetValueStatus.TypeMismatch:
            //    case CompositionGetValueStatus.NotFound:
            //    default:
            //        break;
            //}

            _refreshIconOffsetY = _refreshIconVisual.Offset.Y;
            //Debug.WriteLine($"RefreshIcon's Offset Y: {_refreshIconOffsetY}");

            // Question 4
            // It's not always the case here as the user can pull it all the way down and then push it back up to
            // CANCEL a refresh!! Though I cannot seem to find an easy way to detect right after the finger is lifted.
            // DirectManipulationCompleted is called too late.
            // What might be really helpful is to have a DirectManipulationDelta event with velocity and other values.
            //
            // At the moment I am calculating the time difference between the list gets pulled all the way down and rolled back up.
            // 
            if (!_refresh)
            {
                _refresh = _refreshIconOffsetY == REFRESH_ICON_MAX_OFFSET_Y;
            }

            if (_refreshIconOffsetY == REFRESH_ICON_MAX_OFFSET_Y)
            {
                _pulledDownTime = DateTime.Now;
                //Debug.WriteLine($"When the list is pulled down: {_pulledDownTime}");

                // Stop the Opacity animation on the RefreshIcon and the Offset.Y animation on the Border (ScrollViewer's host)
                _refreshIconVisual.StopAnimation("Opacity");
                _borderVisual.StopAnimation("Offset.Y");
            }

            if (_refresh && _refreshIconOffsetY <= 1)
            {
                _restoredTime = DateTime.Now;
                //Debug.WriteLine($"When the list is back up: {_restoredTime}");
            }

            _refreshIconVisual.StartAnimation("Offset.Y", _offsetAnimation);
        }
    }

    public static class Extensions
    {
        public static ScrollViewer GetScrollViewer(this DependencyObject element)
        {
            if (element is ScrollViewer)
            {
                return (ScrollViewer)element;
            }

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);

                var result = GetScrollViewer(child);
                if (result == null)
                {
                    continue;
                }
                else
                {
                    return result;
                }
            }

            return null;
        }
    }
}
