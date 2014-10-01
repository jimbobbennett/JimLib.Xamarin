using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class GridViewDelegate: UICollectionViewDelegate
    {
        public delegate void OnItemSelected (UICollectionView tableView, NSIndexPath indexPath);

        private readonly OnItemSelected _onItemSelected;

        public GridViewDelegate (OnItemSelected onItemSelected)
        {
            _onItemSelected = onItemSelected;
        }

        public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
        {
            _onItemSelected (collectionView, indexPath);
        }

        public override void ItemHighlighted(UICollectionView collectionView, NSIndexPath indexPath)
        {
            _onItemSelected.Invoke(collectionView, indexPath);
        }
    }
}

