using System;
using System.ComponentModel;
using CoreGraphics;
using System.Linq;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Extensions;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            var view = (ExtendedEntry)Element;

            SetFont(view);
            SetTextAlignment(view);
            SetBorder(view);
            SetButtons(view);
            SetPlaceholderTextColor(view);
            SetKeyboardStyle(view);

            ResizeHeight();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var view = (ExtendedEntry)Element;

            if (e.PropertyNameMatches(() => view.Font))
                SetFont(view);

            if (e.PropertyNameMatches(() => view.XAlign))
                SetTextAlignment(view);

            if (e.PropertyNameMatches(() => view.HasBorder) ||
                e.PropertyNameMatches(() => view.BorderColor) ||
                e.PropertyNameMatches(() => view.BorderWidth))
                SetBorder(view);

            if (e.PropertyNameMatches(() => view.AccessoryButtons))
                SetButtons(view);

            if (e.PropertyNameMatches(() => view.PlaceholderColor))
                SetPlaceholderTextColor(view);

            if (e.PropertyNameMatches(() => view.KeyboardStyle))
                SetKeyboardStyle(view);

            ResizeHeight();
        }

        private void SetKeyboardStyle(ExtendedEntry view)
        {
            Control.KeyboardAppearance = view.KeyboardStyle == KeyboardStyle.Dark ? UIKeyboardAppearance.Dark : UIKeyboardAppearance.Light;
        }

        private void SetPlaceholderTextColor(ExtendedEntry view)
        {
            if (string.IsNullOrEmpty(view.Placeholder) == false && view.PlaceholderColor != Color.Default)
            {
                var placeholderString = new NSAttributedString(view.Placeholder, 
                    new UIStringAttributes { ForegroundColor = view.PlaceholderColor.ToUIColor() });
                Control.AttributedPlaceholder = placeholderString;
            }
        }

        private void SetButtons(ExtendedEntry view)
        {
            if (view.AccessoryButtons == null || !view.AccessoryButtons.Any())
            {
                Control.InputAccessoryView = null;
                return;
            }

            var toolbar = new UIToolbar(new CGRect(0.0f, 0.0f, Control.Frame.Size.Width, 44.0f))
            {
                Translucent = true, 
                Items = view.AccessoryButtons.Select(b => b.CreateButton()).ToArray()
            };

            Control.InputAccessoryView = toolbar;
        }

        private void SetBorder(ExtendedEntry view)
        {
            Control.BorderStyle = view.HasBorder ? UITextBorderStyle.RoundedRect : UITextBorderStyle.None;
            Control.Layer.CornerRadius = Convert.ToSingle(view.BorderCornerRadius);
            Control.Layer.BorderWidth = Convert.ToSingle(view.BorderWidth);
            Control.Layer.BorderColor = view.BorderColor.ToCGColor();
        }

        private void SetTextAlignment(ExtendedEntry view)
        {
            switch (view.XAlign)
            {
                case TextAlignment.Center:
                    Control.TextAlignment = UITextAlignment.Center;
                    break;
                case TextAlignment.End:
                    Control.TextAlignment = UITextAlignment.Right;
                    break;
                case TextAlignment.Start:
                    Control.TextAlignment = UITextAlignment.Left;
                    break;
            }
        }

        private void SetFont(ExtendedEntry view)
        {
            UIFont uiFont;
            if (view.Font != Font.Default && (uiFont = view.Font.ToUIFont()) != null)
                Control.Font = uiFont;
            else if (view.Font == Font.Default)
                Control.Font = UIFont.SystemFontOfSize(17f);
        }

        private void ResizeHeight()
        {
            if (Element.HeightRequest >= 0) return;

            var height = Math.Max(Bounds.Height,
                new UITextField {Font = Control.Font}.IntrinsicContentSize.Height);

			Control.Frame = new CGRect((nfloat)0.0, (nfloat)0.0, (nfloat) Element.Width, (nfloat)height);

            Element.HeightRequest = height;
        }
    }
}