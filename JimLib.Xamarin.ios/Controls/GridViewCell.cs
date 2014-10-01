using System.ComponentModel;
using System.Drawing;
using MonoTouch.CoreGraphics;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class GridViewCell : UICollectionViewCell
    {
        public const string Key = "GridViewCell";

        private ViewCell _viewCell;
        private UIView _view;

        public ViewCell ViewCell
        {
            get { return _viewCell; }
            set
            {
                if (_viewCell == value)
                    return;

                UpdateCell(value);
            }
        }

        [Export ("initWithFrame:")]
        public GridViewCell (RectangleF frame) : base (frame)
        {
            // SelectedBackgroundView = new GridItemSelectedViewOverlay (frame);
            // BringSubviewToFront (SelectedBackgroundView);

        }

        private void UpdateCell (ViewCell cell)
        {
            if (_viewCell != null) 
            {
                //viewCell.SendDisappearing ();
                _viewCell.PropertyChanged -= HandlePropertyChanged;
            }
            _viewCell = cell;
            _viewCell.PropertyChanged += HandlePropertyChanged;
            //viewCell.SendAppearing ();
            UpdateView ();
        }

        private void HandlePropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            UpdateView ();
        }

        private void UpdateView ()
        {

            if (_view != null) 
                _view.RemoveFromSuperview ();
            
            _view = RendererFactory.GetRenderer(_viewCell.View).NativeView;
            _view.AutoresizingMask = UIViewAutoresizing.All;
            _view.ContentMode = UIViewContentMode.ScaleToFill;

            AddSubview (_view);
        }

        public override void LayoutSubviews ()
        {
            base.LayoutSubviews ();
            var frame = ContentView.Frame;
            frame.X = (Bounds.Width - frame.Width) / 2;
            frame.Y = (Bounds.Height - frame.Height) / 2;
            ViewCell.View.Layout (frame.ToRectangle ());
            _view.Frame = frame;
        }
    }

    //SelectedView Overlay Windows8 style
    public class GridItemSelectedViewOverlay : UIView
    {

        public GridItemSelectedViewOverlay(RectangleF frame) : base(frame)
        {
            BackgroundColor = UIColor.Clear;
        }

        public override void Draw(RectangleF rect)
        {
            using (var g = UIGraphics.GetCurrentContext())
            {
                g.SetLineWidth(10);
                UIColor.FromRGB(64, 30, 168).SetStroke();
                UIColor.Clear.SetFill();

                //create geometry
                var path = new CGPath();
                path.AddRect(rect);
                path.CloseSubpath();

                //add geometry to graphics context and draw it
                g.AddPath(path);
                g.DrawPath(CGPathDrawingMode.Stroke);
            }
        }
    }
}

