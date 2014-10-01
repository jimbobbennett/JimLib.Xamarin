using System;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Mvvm;
using JimBobBennett.JimLib.Xamarin.Extensions;
using JimBobBennett.JimLib.Xamarin.Mvvm;
using JimBobBennett.JimLib.Xamarin.Navigation;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Views
{
    public class BaseContentPage : ContentPage, IView
    {
        public static readonly BindableProperty OpacityBackgroundColorProperty =
            BindableProperty.Create<BaseContentPage, Color>(p => p.OpacityBackgroundColor, Color.White,
            propertyChanged: (s, o, n) => ((BaseContentPage)s)._opacityGrid.BackgroundColor = n);

        public static readonly BindableProperty BusyIndicatorBackgroundColorProperty =
            BindableProperty.Create<BaseContentPage, Color>(p => p.BusyIndicatorBackgroundColor, Color.FromRgba(0, 0, 0, 0.5),
                propertyChanged: (s, o, n) => ((BaseContentPage) s)._activityFrame.BackgroundColor = n);

        public static readonly BindableProperty ShowSeperatorProperty =
            BindableProperty.Create<BaseContentPage, bool>(p => p.ShowSeperator, false,
            propertyChanged: (s, o, n) => ((BaseContentPage)s)._seperator.IsVisible = n);

        public Color OpacityBackgroundColor
        {
            get { return (Color)GetValue(OpacityBackgroundColorProperty); }
            set { SetValue(OpacityBackgroundColorProperty, value); }
        }

        public Color BusyIndicatorBackgroundColor
        {
            get { return (Color)GetValue(BusyIndicatorBackgroundColorProperty); }
            set { SetValue(BusyIndicatorBackgroundColorProperty, value); }
        }

        public bool ShowSeperator
        {
            get { return (bool)GetValue(ShowSeperatorProperty); }
            set { SetValue(ShowSeperatorProperty, value); }
        }

        public INavigationStackManager NavigationStackManager { get; protected set; }

        private readonly Grid _contentGrid;
        private readonly Grid _opacityGrid;
        private readonly Grid _mainGrid;
        private readonly ActivityIndicator _activityIndicator;
        private readonly Label _activityLabel;
        private readonly Frame _activityFrame;
        private readonly ProgressBar _seperator;

        protected BaseContentPage()
        {
            _contentGrid = CreateFillGrid();

            _opacityGrid = CreateFillGrid();
            _opacityGrid.IsVisible = false;
            _opacityGrid.Opacity = 0.75;
            _opacityGrid.BackgroundColor = OpacityBackgroundColor;

            var activityGrid = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
            };

            activityGrid.AddAutoRowDefinition();
            activityGrid.AddAutoRowDefinition();
            activityGrid.AddStarColumnDefinition();
            
            _activityIndicator = new ActivityIndicator
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Color = Color.White
            };

            _activityLabel = new Label
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.White
            };

            Grid.SetRow(_activityIndicator, 0);
            Grid.SetRow(_activityLabel, 1);

            activityGrid.Children.Add(_activityIndicator);
            activityGrid.Children.Add(_activityLabel);

            _activityFrame = new Frame
            {
                BackgroundColor = BusyIndicatorBackgroundColor,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = activityGrid,
                IsVisible = false
            };

            _seperator = new ProgressBar
            {
                VerticalOptions = LayoutOptions.Fill,
                IsVisible = ShowSeperator
            };

            _mainGrid = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            _mainGrid.AddStarColumnDefinition();
            _mainGrid.AddAutoRowDefinition();
            _mainGrid.AddStarRowDefinition();

            _mainGrid.Children.Add(_contentGrid);
            _mainGrid.Children.Add(_opacityGrid);
            _mainGrid.Children.Add(_activityFrame);
            _mainGrid.Children.Add(_seperator);
            
            Grid.SetRow(_contentGrid, 1);
            Grid.SetRow(_opacityGrid, 1);
            Grid.SetRow(_activityFrame, 1);
            Grid.SetRow(_seperator, 0);
            
            Content = _mainGrid;
            Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);
        }

        protected BaseContentPage(object viewModel, INavigationStackManager navigationStackManager)
            : this()
        {
            NavigationStackManager = navigationStackManager;
            SetViewModel(viewModel);
        }

        private static Grid CreateFillGrid()
        {
            var grid = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
            };

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            return grid;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext is IBusy)
            {
                _opacityGrid.SetBinding(IsVisibleProperty, new Binding("IsBusy"));
                _activityFrame.SetBinding(IsVisibleProperty, new Binding("IsBusy"));
                _activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, new Binding("IsBusy"));
                _activityLabel.SetBinding(Label.TextProperty, new Binding("BusyMessage"));

                SetBinding(IsBusyProperty, new Binding("IsBusy"));
            }
        }

        protected override void OnChildAdded(Element child)
        {
            base.OnChildAdded(child);

            if (child != _mainGrid)
            {
                Content = _mainGrid;
                _contentGrid.Children.Add(child as View);
            }
        }

        public async Task<string> GetOptionFromUserAsync(string title, string cancel, string destruction, params string[] buttons)
        {
            return await DisplayActionSheet(title, cancel, destruction, buttons);
        }

        public async Task<bool> ShowAlertAsync(string title, string message, string cancel, string accept = null)
        {
            if (title == null)
                throw new ArgumentNullException("title");

            if (cancel == null)
                throw new ArgumentNullException("title");

            return await DisplayAlert(title, message, accept, cancel);
        }

        protected void SetViewModel(object viewModel)
        {
            BindingContext = viewModel;

            var vm = viewModel as IContentPageViewModelBase;
            if (vm != null)
            {
                vm.View = this;

                if (NavigationStackManager != null)
                    vm.NeedClose += (s, e) =>
                        {
                            if (NavigationStackManager != null)
                                Device.BeginInvokeOnMainThread(async () => await NavigationStackManager.PopAsync());
                        };
            }
        }
    }
}
