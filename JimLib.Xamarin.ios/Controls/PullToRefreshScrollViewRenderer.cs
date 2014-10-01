using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PullToRefreshScrollView), typeof(PullToRefreshScrollViewRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class PullToRefreshScrollViewRenderer : ScrollViewRenderer
    {
        private FormsUIRefreshControl _refreshControl;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (_refreshControl != null)
                return;

            var pullToRefreshScrollView = (PullToRefreshScrollView)Element;
            pullToRefreshScrollView.PropertyChanged += OnElementPropertyChanged;

            _refreshControl = new FormsUIRefreshControl
            {
                RefreshCommand = pullToRefreshScrollView.RefreshCommand,
                Message = pullToRefreshScrollView.Message
            };

            AlwaysBounceVertical = true;

            AddSubview(_refreshControl);
        }

        private void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var pullToRefreshScrollView = Element as PullToRefreshScrollView;
            if (pullToRefreshScrollView == null)
                return;

            if (e.PropertyName == PullToRefreshScrollView.IsRefreshingProperty.PropertyName)
                _refreshControl.IsRefreshing = pullToRefreshScrollView.IsRefreshing;

            if (e.PropertyName == PullToRefreshScrollView.MessageProperty.PropertyName)
                _refreshControl.Message = pullToRefreshScrollView.Message;

            if (e.PropertyName == PullToRefreshScrollView.RefreshCommandProperty.PropertyName)
                _refreshControl.RefreshCommand = pullToRefreshScrollView.RefreshCommand;
        }
    }
}