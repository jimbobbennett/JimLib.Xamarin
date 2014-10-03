using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class ExtendedEntry : Entry
    {
        public ExtendedEntry()
        {
            SetValue(AccessoryButtonsProperty, new List<EntryAccessoryButton>());

            Completed += (s, e) =>
                {
                    if (Command != null && Command.CanExecute(CommandParameter))
                        Command.Execute(CommandParameter);
                };
        }

        public static readonly BindableProperty FontProperty =
            BindableProperty.Create("Font", typeof(Font), typeof(ExtendedEntry), new Font());

        public static readonly BindableProperty XAlignProperty =
            BindableProperty.Create<ExtendedEntry, TextAlignment>(p => p.XAlign, TextAlignment.Start);

        public static readonly BindableProperty KeyboardStyleProperty =
            BindableProperty.Create<ExtendedEntry, KeyboardStyle>(p => p.KeyboardStyle, KeyboardStyle.Light);

        public static readonly BindableProperty HasBorderProperty =
            BindableProperty.Create<ExtendedEntry, bool>(p => p.HasBorder, true);

        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create<ExtendedEntry, double>(p => p.BorderWidth, 0);

        public static readonly BindableProperty BorderCornerRadiusProperty =
            BindableProperty.Create<ExtendedEntry, double>(p => p.BorderCornerRadius, 0);

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create<ExtendedEntry, Color>(p => p.BorderColor, Color.Default);

        public static readonly BindableProperty PlaceholderColorProperty =
            BindableProperty.Create<ExtendedEntry, Color>(p => p.PlaceholderColor, Color.Default);

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create<ExtendedEntry, ICommand>(p => p.Command, null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create<ExtendedEntry, object>(p => p.CommandParameter, null);

        public static readonly BindableProperty AccessoryButtonsProperty =
            BindableProperty.Create<ExtendedEntry, List<EntryAccessoryButton>>(p => p.AccessoryButtons, null,
            propertyChanging: AccessoryButtonsPropertyChanging);

        private static void AccessoryButtonsPropertyChanging(BindableObject bindable, object oldvalue, 
            object newvalue)
        {
            var oldList = oldvalue as List<EntryAccessoryButton>;

            if (oldList != null)
            {
                foreach (var button in oldList)
                    button.BindingContext = null;
            }

            var newList = newvalue as List<EntryAccessoryButton>;

            if (newList != null)
            {
                var entry = (ExtendedEntry) bindable;
                foreach (var button in newList)
                    button.BindingContext = entry.BindingContext;
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (AccessoryButtons == null) return;

            foreach (var button in AccessoryButtons)
                button.BindingContext = BindingContext;
        }

        public Font Font
        {
            get { return (Font)GetValue(FontProperty); }
            set { SetValue(FontProperty, value); }
        }

        public TextAlignment XAlign
        {
            get { return (TextAlignment)GetValue(XAlignProperty); }
            set { SetValue(XAlignProperty, value); }
        }

        public KeyboardStyle KeyboardStyle
        {
            get { return (KeyboardStyle)GetValue(KeyboardStyleProperty); }
            set { SetValue(KeyboardStyleProperty, value); }
        }

        public bool HasBorder
        {
            get { return (bool)GetValue(HasBorderProperty); }
            set { SetValue(HasBorderProperty, value); }
        }

        public double BorderWidth
        {
            get { return (double)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }

        public double BorderCornerRadius
        {
            get { return (double)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public Color BorderColor
        {
            get { return (Color)GetValue(BorderColorProperty); }
            set { SetValue(BorderColorProperty, value); }
        }

        public Color PlaceholderColor
        {
            get { return (Color)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }

        public List<EntryAccessoryButton> AccessoryButtons
        {
            get { return (List<EntryAccessoryButton>)GetValue(AccessoryButtonsProperty); }
            set { SetValue(AccessoryButtonsProperty, value); }
        }
    }
}
