using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Extensions;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ToolBar), typeof(ToolBarRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class ToolBarRenderer : ViewRenderer<ToolBar, UIToolbar>
    {
        private INotifyCollectionChanged _buttonsCollection;

        protected override void OnElementChanged(ElementChangedEventArgs<ToolBar> e)
        {
            base.OnElementChanged(e);

			if (Element == null) return;

            BackgroundColor = Element.BackgroundColor.ToUIColor();

            if (Control == null)
            {
                var toolbar = new UIToolbar(Bounds);
                SetNativeControl(toolbar);
                
                Element.PropertyChanged += ElementOnPropertyChanged;

                Resize();
            }

            BuildButtons();
            SetTranslucent();
            SetTintColor();
            SetBarTintColor();
            SetBarStyle();

            _buttonsCollection = Element.ToolBarButtons as INotifyCollectionChanged;

            if (_buttonsCollection != null)
                _buttonsCollection.CollectionChanged += ButtonsCollectionOnCollectionChanged;
        }

        private void SetTintColor()
        {
            if (Element.TintColor != Color.Default)
                Control.TintColor = Element.TintColor.ToUIColor();
        }

        private void SetBarTintColor()
        {
            if (Element.BarTintColor != Color.Default)
                Control.BarTintColor = Element.BarTintColor.ToUIColor();
        }

        private void SetTranslucent()
        {
            Control.Translucent = Element.Translucent;
        }

        private void SetBarStyle()
        {
            Control.BarStyle = Element.BarStyle == BarStyle.Light ? UIBarStyle.Default :  UIBarStyle.Black;
        }

        private void Resize()
        {
            Control.SizeToFit();
            Element.HeightRequest = Control.Bounds.Height;
        }

        private void ElementOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InvokeOnMainThread(() => HandleElementOnPropertyChanged(e));
        }

        private void HandleElementOnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyNameMatches(() => Element.ToolBarButtons))
            {
                if (_buttonsCollection != null)
                    _buttonsCollection.CollectionChanged -= ButtonsCollectionOnCollectionChanged;

                BuildButtons();

                _buttonsCollection = Element.ToolBarButtons as INotifyCollectionChanged;

                if (_buttonsCollection != null)
                    _buttonsCollection.CollectionChanged += ButtonsCollectionOnCollectionChanged;
            }
            else if (e.PropertyNameMatches(() => Element.Translucent))
                SetTranslucent();
            else if (e.PropertyNameMatches(() => Element.TintColor))
                SetTintColor();
            else if (e.PropertyNameMatches(() => Element.BarTintColor))
                SetBarTintColor();
            else if (e.PropertyNameMatches(() => Element.BarStyle))
                SetBarStyle();
        }

        private void ButtonsCollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            InvokeOnMainThread(BuildButtons);
        }

        private void BuildButtons()
        {
            Control.SetItems(Element.ToolBarButtons == null ? null : Element.ToolBarButtons
                .Select(b => b.CreateButtonForToolbar()).ToArray(), false);

            Resize();
        }
    }
}
