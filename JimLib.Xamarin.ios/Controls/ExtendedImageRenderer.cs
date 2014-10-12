using System;
using System.ComponentModel;
using System.Drawing;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Extensions;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedImage), typeof(ExtendedImageRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class ExtendedImageRenderer : ImageRenderer
    {
        private UIImageView _imageView;
        private UILabel _label;

        protected override void OnElementChanged(ElementChangedEventArgs<Image> e)
        {
            base.OnElementChanged(e);

            var imageElement = (ExtendedImage)Element;
            SetTintColor(imageElement.TintColor);

            imageElement.Clicked += (s, e1) => ShowActivitySheet();

            if (_imageView == null)
            {
                _imageView = new UIImageView(Control.Frame);
                Control.AddSubview(_imageView);
            }

            BuildFallbackImage();
            SetCircular(imageElement);
        }

        private void ShowActivitySheet()
        {
            if (Element is ExtendedImage && ((ExtendedImage)Element).IsSharable)
            {
                var items = new NSObject[] { new NSString(((ExtendedImage)Element).ShareText), Control.Image };

                var controller = new UIActivityViewController(items, null);

                UIApplication.SharedApplication.KeyWindow.RootViewController.GetTopViewController()
                    .PresentViewController(controller, true, null);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var imageElement = (ExtendedImage)Element;

            if (e.PropertyNameMatches(() => imageElement.TintColor) ||
                e.PropertyNameMatches(() => Element.Source))
                SetTintColor(imageElement.TintColor);

            if (e.PropertyNameMatches(() => imageElement.ImageLabelText) ||
                e.PropertyNameMatches(() => imageElement.LabelColor))
            {
                SetLabelDetails(imageElement);
                BuildFallbackImage();
            }
            if (e.PropertyNameMatches(() => imageElement.Circular))
                SetCircular(imageElement);
        }

        private void SetTintColor(Color tintColor)
        {
            if (tintColor == Color.Default)
            {
                Control.TintColor = null;

                if (Control.Image != null)
                    Control.Image = Control.Image.ImageWithRenderingMode(UIImageRenderingMode.Automatic);
            }
            else
            {
                if (Control.Image != null)
                    Control.Image = Control.Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);

                Control.TintColor = tintColor.ToUIColor();
            }
        }

        private void SetCircular(ExtendedImage element)
        {
            if (element.Circular)
            {
                Control.Layer.CornerRadius = Control.Frame.Size.Width / 2;
                Control.Layer.MasksToBounds = true;
                Control.Layer.BorderWidth = element.CircleBorderWidth;
                Control.Layer.BorderColor = element.CircleBorderColor.ToCGColor();
            }
            else
            {
                Control.Layer.CornerRadius = 0;
                Control.Layer.MasksToBounds = false;
                Control.Layer.BorderWidth = 0;
                Control.Layer.BorderColor = UIColor.Clear.CGColor;
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            _imageView.Frame = Control.Frame;

            BuildFallbackImage();

            var imageElement = ((ExtendedImage)Element);
            SetCircular(imageElement);
        }

        private void BuildFallbackImage()
        {
            var imageElement = ((ExtendedImage)Element);
            
            if (_label == null)
            {
                _label = new UILabel
                {
                    AdjustsFontSizeToFitWidth = true,
                    TextAlignment = UITextAlignment.Center,
                    LineBreakMode = UILineBreakMode.WordWrap,
                    Lines = 0
                };

                SetLabelDetails(imageElement);
                Control.AddSubview(_label);
            }

            if (imageElement.ImageLabelText.IsNullOrEmpty())
            {
                _label.Text = string.Empty;
                _imageView.Image = new UIImage();
                return;
            }

            SetLabelSizeAndPosition();

            if (Control.Frame.IsEmpty) return;

            var image = new UIImage();

            UIGraphics.BeginImageContextWithOptions(Control.Frame.Size, false, UIScreen.MainScreen.Scale);

            imageElement.LabelBackgroundColor.ToUIColor().SetFill();

            var pathFrameWidth = Math.Min(Control.Frame.Width, _label.Frame.Width * 1.2f);
            var pathFrameHeight = Math.Min(Control.Frame.Height, _label.Frame.Height * 1.2f);
            var pathFrame = new RectangleF((Control.Frame.Width - pathFrameWidth) / 2f,
                (Control.Frame.Height - pathFrameHeight) / 2f,
                pathFrameWidth,
                pathFrameHeight);

            var path = UIBezierPath.FromRoundedRect(pathFrame, 5f);
            path.Fill();

            image.Draw(Control.Frame);

            _imageView.Image = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();
        }

        private void SetLabelSizeAndPosition()
        {
            _label.Frame = new RectangleF(Control.Frame.Width / 10,
                Control.Frame.Height / 10,
                Control.Frame.Width * 0.8f,
                _label.Frame.Height * 0.8f);

            _label.SizeToFit();

            var newFrame = new RectangleF((Control.Frame.Width - _label.Frame.Width) / 2,
                (Control.Frame.Height - _label.Frame.Height) / 2,
                _label.Frame.Width,
                _label.Frame.Height);

            _label.Frame = newFrame;
        }

        private void SetLabelDetails(ExtendedImage imageElement)
        {
            _label.Text = imageElement.ImageLabelText;
            _label.TextColor = imageElement.LabelColor.ToUIColor();
        }
    }
}