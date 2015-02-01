using Foundation;
using UIKit;
using System;

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class GridDataSource : UICollectionViewSource
    {
        public delegate UICollectionViewCell OnGetCell(UICollectionView collectionView, NSIndexPath indexPath);

        public delegate int OnRowsInSection(UICollectionView collectionView, nint section);

        public delegate void OnItemSelected(UICollectionView collectionView, NSIndexPath indexPath);

        private readonly OnGetCell _onGetCell;
        private readonly OnRowsInSection _onRowsInSection;
        private readonly OnItemSelected _onItemSelected;

        public GridDataSource(OnGetCell onGetCell, OnRowsInSection onRowsInSection, OnItemSelected onItemSelected)
        {
            _onGetCell = onGetCell;
            _onRowsInSection = onRowsInSection;
            _onItemSelected = onItemSelected;
        }

        #region implemented abstract members of UICollectionViewDataSource

		public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return _onRowsInSection(collectionView, section);
        }

        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            _onItemSelected(collectionView, indexPath);
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = _onGetCell(collectionView, indexPath);
            if (((GridCollectionView) collectionView).SelectionEnable)
            {
                cell.AddGestureRecognizer(new UITapGestureRecognizer(v => ItemSelected(collectionView, indexPath)));
            }
            else
                cell.SelectedBackgroundView = new UIView();

            return cell;
        }

        #endregion
    }
}

