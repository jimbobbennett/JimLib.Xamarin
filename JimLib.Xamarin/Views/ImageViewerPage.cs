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
            grid.AddStarColumnDefinition();

            var doneButton = new Button
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.End,
                TextColor = Color.White,
                Text = "Done"
            };

            doneButton.SetBinding(Button.CommandProperty, new Binding("CloseCommand"));
            
            var image = new ExtendedImage
            {
                Aspect = Aspect.AspectFit,
                IsSharable = true
            };

            image.SetBinding(Image.SourceProperty, new Binding("ImageSource"));

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

            Grid.SetRow(doneButton, 0);
            Grid.SetRow(image, 1);
            Grid.SetRow(titleLabel, 2);
            Grid.SetRow(textLabel, 3);

            grid.Children.Add(doneButton);
            grid.Children.Add(image);
            grid.Children.Add(titleLabel);
            grid.Children.Add(textLabel);
            
            Content = grid;
        }

        public void SetImage(ImageSource imageSource, string imageTextTitle = null, string imageText = null)
        {
            _viewModel.SetImage(imageSource, imageTextTitle, imageText);
        }
    }
}
