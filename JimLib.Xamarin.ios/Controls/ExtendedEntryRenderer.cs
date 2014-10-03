using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Images;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        private static IImageSourceHandler GetHandler(ImageSource source)
        {
            IImageSourceHandler returnValue = null;
            if (source is UriImageSource)
                returnValue = new ImageLoaderSourceHandler();
            else if (source is FileImageSource)
                returnValue = new FileImageSourceHandler();
            else if (source is StreamImageSource)
                returnValue = new StreamImagesourceHandler();
            return returnValue;
        }

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

            var toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, Control.Frame.Size.Width, 44.0f))
            {
                Translucent = true, 
                Items = view.AccessoryButtons.Select(CreateButton).ToArray()
            };

            Control.InputAccessoryView = toolbar;
        }

        private static UIBarButtonItem CreateButton(EntryAccessoryButton button)
        {
            UIBarButtonItem uiButton;

            if (button.EntryAccessoryButtonItem == EntryAccessoryButtonItem.None)
            {
                uiButton = new UIBarButtonItem(button.Text, UIBarButtonItemStyle.Plain,
                    (s, e) => HandleButtonClick(button));

                if (button.ImageSource != null)
                    SetImage(uiButton, button.ImageSource);
            }
            else
            {
                uiButton = new UIBarButtonItem(ConvertToSystemItem(button.EntryAccessoryButtonItem),
                    (s, e) => HandleButtonClick(button));

                if (button.EntryAccessoryButtonItem == EntryAccessoryButtonItem.Done)
                    uiButton.Style = UIBarButtonItemStyle.Done;
            }

            WireUpEnabledToCanExecute(button, uiButton);

            return uiButton;
        }

        private static async void SetImage(UIBarItem uiButton, ImageSource source)
        {
            var handler = GetHandler(source);
            using (var image = await handler.LoadImageAsync(source))
            {
                UIGraphics.BeginImageContext(image.Size);
                image.Draw(new RectangleF(0, 0, image.Size.Width, image.Size.Height));
                using (var resultImage = UIGraphics.GetImageFromCurrentImageContext())
                {
                    UIGraphics.EndImageContext();
                    using (var resizableImage = resultImage.CreateResizableImage(new UIEdgeInsets(0, 0, image.Size.Width, image.Size.Height)))
                        uiButton.Image = resizableImage.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
                }
            }
        }

        private static void HandleButtonClick(EntryAccessoryButton button)
        {
            if (button.Command != null && button.Command.CanExecute(button.CommandParameter))
                button.Command.Execute(button.CommandParameter);
        }

        private static void WireUpEnabledToCanExecute(EntryAccessoryButton button, UIBarItem uiButton)
        {
            if (button.Command != null)
                button.Command.CanExecuteChanged += (s, e) =>
                    {
                        var enabled = button.Command.CanExecute(button.CommandParameter);

                        if (uiButton.Enabled == enabled) return;

                        uiButton.Enabled = enabled;

                        if (uiButton.Image != null)
                            uiButton.Image = ImageHelper.AdjustOpacity(uiButton.Image, enabled ? 1 : 0.5f);
                    };

            uiButton.Enabled = button.Command == null || button.Command.CanExecute(button.CommandParameter);

            if (!uiButton.Enabled && uiButton.Image != null)
                uiButton.Image = ImageHelper.AdjustOpacity(uiButton.Image, 0.5f);
        }

        private static UIBarButtonSystemItem ConvertToSystemItem(EntryAccessoryButtonItem item)
        {
            switch (item)
            {
                case EntryAccessoryButtonItem.Cancel:
                    return UIBarButtonSystemItem.Cancel;
                case EntryAccessoryButtonItem.Edit:
                    return UIBarButtonSystemItem.Edit;
                case EntryAccessoryButtonItem.Save:
                    return UIBarButtonSystemItem.Save;
                case EntryAccessoryButtonItem.Add:
                    return UIBarButtonSystemItem.Add;
                case EntryAccessoryButtonItem.FlexibleSpace:
                    return UIBarButtonSystemItem.FlexibleSpace;
                case EntryAccessoryButtonItem.FixedSpace:
                    return UIBarButtonSystemItem.FixedSpace;
                case EntryAccessoryButtonItem.Compose:
                    return UIBarButtonSystemItem.Compose;
                case EntryAccessoryButtonItem.Reply:
                    return UIBarButtonSystemItem.Reply;
                case EntryAccessoryButtonItem.Action:
                    return UIBarButtonSystemItem.Action;
                case EntryAccessoryButtonItem.Organize:
                    return UIBarButtonSystemItem.Organize;
                case EntryAccessoryButtonItem.Bookmarks:
                    return UIBarButtonSystemItem.Bookmarks;
                case EntryAccessoryButtonItem.Search:
                    return UIBarButtonSystemItem.Search;
                case EntryAccessoryButtonItem.Refresh:
                    return UIBarButtonSystemItem.Refresh;
                case EntryAccessoryButtonItem.Stop:
                    return UIBarButtonSystemItem.Stop;
                case EntryAccessoryButtonItem.Camera:
                    return UIBarButtonSystemItem.Camera;
                case EntryAccessoryButtonItem.Trash:
                    return UIBarButtonSystemItem.Trash;
                case EntryAccessoryButtonItem.Play:
                    return UIBarButtonSystemItem.Play;
                case EntryAccessoryButtonItem.Pause:
                    return UIBarButtonSystemItem.Pause;
                case EntryAccessoryButtonItem.Rewind:
                    return UIBarButtonSystemItem.Rewind;
                case EntryAccessoryButtonItem.FastForward:
                    return UIBarButtonSystemItem.FastForward;
                case EntryAccessoryButtonItem.Undo:
                    return UIBarButtonSystemItem.Undo;
                case EntryAccessoryButtonItem.Redo:
                    return UIBarButtonSystemItem.Redo;
                case EntryAccessoryButtonItem.PageCurl:
                    return UIBarButtonSystemItem.PageCurl;
                default:
                    return UIBarButtonSystemItem.Done;
            }
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

            Control.Frame = new RectangleF(0.0f, 0.0f, (float) Element.Width, height);

            Element.HeightRequest = height;
        }
    }
}