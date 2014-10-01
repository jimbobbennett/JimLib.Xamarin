using System.Windows.Input;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class PullToRefreshListView : ExtendedListView
    {
        public static readonly BindableProperty IsRefreshingProperty =
            BindableProperty.Create<PullToRefreshListView, bool>(p => p.IsRefreshing, false);

        public static readonly BindableProperty RefreshCommandProperty =
            BindableProperty.Create<PullToRefreshListView, ICommand>(p => p.RefreshCommand, null);
        
        public static readonly BindableProperty MessageProperty =
            BindableProperty.Create<PullToRefreshListView, string>(p => p.Message, string.Empty);

        public bool IsRefreshing
        {
            get { return (bool)GetValue(IsRefreshingProperty); }
            set { SetValue(IsRefreshingProperty, value); }
        }

        public ICommand RefreshCommand
        {
            get { return (ICommand)GetValue(RefreshCommandProperty); }
            set { SetValue(RefreshCommandProperty, value); }
        }

        public string Message
        {
            get { return (string) GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }
    }
}

