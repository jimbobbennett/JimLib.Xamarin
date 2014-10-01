using System.ComponentModel;
using System.Drawing;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtendedListView), typeof(ExtendedListViewRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class ExtendedListViewRenderer : ListViewRenderer
	{
	    private readonly UIView _footer = new UIView(RectangleF.Empty);
	
		protected override void OnElementChanged (ElementChangedEventArgs<ListView> e)
		{
			base.OnElementChanged (e);

            var extendedListView = (ExtendedListView)Element; 

		    SetShowEmptyCells(extendedListView);

		    extendedListView.ItemSelected += (s, e1) =>
		        {
                    if (Control.IndexPathForSelectedRow != null)
		                Control.DeselectRow(Control.IndexPathForSelectedRow, false);
		        };
		}
       
	    private void SetShowEmptyCells(ExtendedListView pullToRefreshListView)
	    {
	        Control.TableFooterView = !pullToRefreshListView.ShowEmptyCells ? _footer : null;
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
	    }
	}
}

