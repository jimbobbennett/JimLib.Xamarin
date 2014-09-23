using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class ExtendedViewCell : ViewCell
    {
        public static readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create<ExtendedViewCell, Color>(p => p.BackgroundColor, Color.Transparent);

        public Color BackgroundColor
        {
            get { return (Color) GetValue(BackgroundColorProperty); }
            set { SetValue(BackgroundColorProperty, value); }
        }

        public static readonly BindableProperty SeparatorColorProperty =
            BindableProperty.Create<ExtendedViewCell, Color>(p => p.SeparatorColor, Color.FromRgba(199, 199, 204, 255));

        public Color SeparatorColor
        {
            get { return (Color) GetValue(SeparatorColorProperty); }
            set { SetValue(SeparatorColorProperty, value); }
        }

        public static readonly BindableProperty SeparatorPaddingProperty =
            BindableProperty.Create<ExtendedViewCell, Thickness>(p => p.SeparatorPadding, default(Thickness));

        public Thickness SeparatorPadding
        {
            get { return (Thickness) GetValue(SeparatorPaddingProperty); }
            set { SetValue(SeparatorPaddingProperty, value); }
        }

        public static readonly BindableProperty ShowSeparatorProperty =
            BindableProperty.Create<ExtendedViewCell, bool>(p => p.ShowSeparator, true);

        public bool ShowSeparator
        {
            get { return (bool) GetValue(ShowSeparatorProperty); }
            set { SetValue(ShowSeparatorProperty, value); }
        }

        public static readonly BindableProperty ShowDisclosureProperty =
            BindableProperty.Create<ExtendedViewCell, bool>(p => p.ShowDisclosure, false);

        public bool ShowDisclosure
        {
            get { return (bool) GetValue(ShowDisclosureProperty); }
            set { SetValue(ShowDisclosureProperty, value); }
        }

        public static readonly BindableProperty DisclosureImageProperty =
            BindableProperty.Create<ExtendedViewCell, string>(
                p => p.DisclosureImage, string.Empty);

        public string DisclosureImage
        {
            get { return (string) GetValue(DisclosureImageProperty); }
            set { SetValue(DisclosureImageProperty, value); }
        }
    }
}
