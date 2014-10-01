using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class GridCollectionView : UICollectionView
    {
        public bool SelectionEnable {
            get;
            set;
        }
        public GridCollectionView () : this (default(RectangleF))
        {
        }

        public GridCollectionView (RectangleF frm) : base (default(RectangleF), new UICollectionViewFlowLayout () { })
        {
            this.AutoresizingMask = UIViewAutoresizing.All;
            this.ContentMode = UIViewContentMode.ScaleToFill;
            RegisterClassForCell (typeof(GridViewCell), new NSString (GridViewCell.Key));

        }

        public override UICollectionViewCell CellForItem(NSIndexPath indexPath)
        {
            return base.CellForItem(indexPath);
        }

        public override void Draw (RectangleF rect)
        {
            this.CollectionViewLayout.InvalidateLayout ();

            base.Draw (rect);
        }

        public double RowSpacing {
            get { 
                return (double)(this.CollectionViewLayout as UICollectionViewFlowLayout).MinimumLineSpacing;
            }
            set {
                (this.CollectionViewLayout as UICollectionViewFlowLayout).MinimumLineSpacing = (float)value;
            }
        }

        public double ColumnSpacing {
            get { 
                return (double)(this.CollectionViewLayout as UICollectionViewFlowLayout).MinimumInteritemSpacing;
            }
            set {
                (this.CollectionViewLayout as UICollectionViewFlowLayout).MinimumInteritemSpacing = (float)value;
            }
        }

        public SizeF ItemSize {
            get { 
                return (this.CollectionViewLayout as UICollectionViewFlowLayout).ItemSize;
            }
            set {
                (this.CollectionViewLayout as UICollectionViewFlowLayout).ItemSize = value;
            }
        }
    }
}

