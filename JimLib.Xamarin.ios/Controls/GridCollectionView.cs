using System;
using System.Diagnostics;
using CoreGraphics;
using Foundation;
using UIKit;

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public sealed class GridCollectionView : UICollectionView
    {
        public bool SelectionEnable { get; set; }

        public GridCollectionView() : this(default(CGRect))
        {
        }

        public GridCollectionView(CGRect frm) : base(default(CGRect), new UICollectionViewFlowLayout())
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

        public override void Draw(CGRect rect)
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

        public CGSize ItemSize
        {
            get { return ((UICollectionViewFlowLayout) CollectionViewLayout).ItemSize; }
            set { ((UICollectionViewFlowLayout) CollectionViewLayout).ItemSize = value; }
        }
    }
}

