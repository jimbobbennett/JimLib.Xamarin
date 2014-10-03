using System.ComponentModel;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PullToRefreshGridView), typeof(PullToRefreshGridViewRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class PullToRefreshGridViewRenderer : GridViewRenderer
    {
        private FormsUIRefreshControl _refreshControl;

        protected override void OnElementChanged(ElementChangedEventArgs<GridView> e)
        {
            base.OnElementChanged(e);

            var pullToRefreshGridView = (PullToRefreshGridView) Element;

            if (_refreshControl == null)
            {
                _refreshControl = new FormsUIRefreshControl
                {
                    RefreshCommand = pullToRefreshGridView.RefreshCommand,
                    Message = pullToRefreshGridView.Message
                };

                Control.AlwaysBounceVertical = true;
                Control.AddSubview(_refreshControl);
            }
        }

        protected override void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var pullToRefreshGridView = (PullToRefreshGridView)Element;

            if (e.PropertyNameMatches(() => pullToRefreshGridView.IsRefreshing))
                _refreshControl.IsRefreshing = pullToRefreshGridView.IsRefreshing;
            if (e.PropertyNameMatches(() => pullToRefreshGridView.Message))
                _refreshControl.Message = pullToRefreshGridView.Message;
            if (e.PropertyNameMatches(() => pullToRefreshGridView.RefreshCommand))
                _refreshControl.RefreshCommand = pullToRefreshGridView.RefreshCommand;
        }
    }
}
