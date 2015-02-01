using System.ComponentModel;
using CoreGraphics;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedListView), typeof(ExtendedListViewRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class ExtendedListViewRenderer : ListViewRenderer
	{
	    private readonly UIView _footer = new UIView(CGRect.Empty);
	
		protected override void OnElementChanged (ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged (e);

			var oldExtendedListView = e.OldElement as ExtendedListView;
			if (oldExtendedListView != null)
				oldExtendedListView.ItemSelected -= OnElementItemSelected;

			var extendedListView = (ExtendedListView)e.NewElement; 
			if (extendedListView != null)
			{
				extendedListView.ItemSelected += OnElementItemSelected;

			    SetShowEmptyCells(extendedListView);
	            SetAlwaysBounceVertical(extendedListView);
			}
		}
			
		private void OnElementItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (Control != null && Control.IndexPathForSelectedRow != null)
				Control.DeselectRow(Control.IndexPathForSelectedRow, false);
		}

        private void SetAlwaysBounceVertical(ExtendedListView extendedListView)
        {
            Control.AlwaysBounceVertical = extendedListView.AlwaysBounceVertical;
        }

		private void SetShowEmptyCells(ExtendedListView extendedListView)
	    {
			Control.TableFooterView = !extendedListView.ShowEmptyCells ? _footer : null;
	    }

	    /// <summary>
	    /// Raises the element property changed event.
	    /// </summary>
	    /// <param name="sender">Sender.</param>
	    /// <param name="e">E.</param>
	    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
	    {
	        base.OnElementPropertyChanged(sender, e);
            var extendedListView = Element as ExtendedListView;
	        if (extendedListView == null)
	            return;

            if (e.PropertyNameMatches(() => extendedListView.ShowEmptyCells))
                SetShowEmptyCells(extendedListView);

            if (e.PropertyNameMatches(() => extendedListView.AlwaysBounceVertical))
                SetAlwaysBounceVertical(extendedListView);
	    }
	}
}

