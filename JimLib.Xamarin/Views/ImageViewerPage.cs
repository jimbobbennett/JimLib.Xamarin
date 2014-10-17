using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.Extensions;
using JimBobBennett.JimLib.Xamarin.Navigation;
using JimBobBennett.JimLib.Xamarin.ViewModels;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Views
{
    public class ImageViewerPage : BaseContentPage
    {
        private readonly ImageViewerViewModel _viewModel;

        public ImageViewerPage()
        {
            BuildControls();
        }

        public ImageViewerPage(ImageViewerViewModel viewModel, INavigationStackManager navigationStackManager)
            : base(viewModel, navigationStackManager)
        {
            _viewModel = viewModel;
            BuildControls();
        }

        private void BuildControls()
        {
            BackgroundColor = Color.Black;

            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
            };

            grid.AddAutoRowDefinition();
            grid.AddStarRowDefinition();
            grid.AddAutoRowDefinition();
            grid.AddAutoRowDefinition();
            grid.AddAutoRowDefinition();
            grid.AddStarColumnDefinition();

            var doneButton = new Button
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.End,
                TextColor = Color.White,
                Text = "Done"
            };

            doneButton.SetBinding(Button.CommandProperty, new Binding("CloseCommand"));

            var doneButtonContentView = new ContentView
            {
                Padding = new Thickness(0, 0, 20, 0),
                Content = doneButton
            };

            var image = new ExtendedImage
            {
                Aspect = Aspect.AspectFit
            };

            image.SetBinding(Image.SourceProperty, new Binding("ImageSource"));
            var imageContentView = new ContentView
            {
                Padding = new Thickness(10, 0, 10, 0),
                Content = image
            };

            var titleLabel = new ExtendedLabel
            {
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.White,
                XAlign = TextAlignment.Center,
                LineBreakMode = LineBreakMode.NoWrap,
                AdjustFontSizeToFitWidth = true
            };

            titleLabel.SetBinding(Label.TextProperty, new Binding("ImageTitleText"));

            var textLabel = new ExtendedLabel
            {
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.White,
                XAlign = TextAlignment.Center,
                LineBreakMode = LineBreakMode.WordWrap,
                Font = Font.SystemFontOfSize(NamedSize.Small)
            };

            textLabel.SetBinding(Label.TextProperty, new Binding("ImageText"));

            var toolBar = new ToolBar
            {
                Translucent = true,
                BarStyle = BarStyle.Dark
            };

            toolBar.ToolBarButtons.Add(new ToolBarButton { SystemButtonItem = SystemButtonItem.FlexibleSpace });

            var shareButton = new ToolBarButton
            {
                ImageSource = ImageSource.FromFile("Images/imageviewerpageupload.png")
            };
            shareButton.SetBinding(ToolBarButton.CommandProperty, "ShareImageCommand");

            toolBar.ToolBarButtons.Add(shareButton);

            Grid.SetRow(doneButtonContentView, 0);
            Grid.SetRow(imageContentView, 1);
            Grid.SetRow(titleLabel, 2);
            Grid.SetRow(textLabel, 3);
            Grid.SetRow(toolBar, 4);

            grid.Children.Add(doneButtonContentView);
            grid.Children.Add(imageContentView);
            grid.Children.Add(titleLabel);
            grid.Children.Add(textLabel);
            grid.Children.Add(toolBar);
            
            Content = grid;
        }

        public void SetImage(ImageSource imageSource, string imageTextTitle = null, string imageText = null)
        {
            _viewModel.SetImage(imageSource, imageTextTitle, imageText);
        }
    }
}
