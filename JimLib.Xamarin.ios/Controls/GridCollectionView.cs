using System;
using System.Diagnostics;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public sealed class GridCollectionView : UICollectionView
    {
        public bool SelectionEnable { get; set; }

        public GridCollectionView() : this(default(RectangleF))
        {
        }

        public GridCollectionView(RectangleF frm) : base(default(RectangleF), new UICollectionViewFlowLayout())
        {
            AutoresizingMask = UIViewAutoresizing.All;
            ContentMode = UIViewContentMode.ScaleToFill;
            RegisterClassForCell(typeof (GridViewCell), new NSString(GridViewCell.Key));

        }

        public override UICollectionViewCell CellForItem(NSIndexPath indexPath)
        {
            if (indexPath == null) return null;

            try
            {
                return base.CellForItem(indexPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get cell: " + ex.Message);
                return null;
            }
        }

        public override void Draw(RectangleF rect)
        {
            CollectionViewLayout.InvalidateLayout();

            base.Draw(rect);
        }

        public double RowSpacing
        {
            get { return ((UICollectionViewFlowLayout) CollectionViewLayout).MinimumLineSpacing; }
            set { ((UICollectionViewFlowLayout) CollectionViewLayout).MinimumLineSpacing = (float) value; }
        }

        public double ColumnSpacing
        {
            get { return ((UICollectionViewFlowLayout) CollectionViewLayout).MinimumInteritemSpacing; }
            set { ((UICollectionViewFlowLayout) CollectionViewLayout).MinimumInteritemSpacing = (float) value; }
        }

        public SizeF ItemSize
        {
            get { return ((UICollectionViewFlowLayout) CollectionViewLayout).ItemSize; }
            set { ((UICollectionViewFlowLayout) CollectionViewLayout).ItemSize = value; }
        }
    }
}

