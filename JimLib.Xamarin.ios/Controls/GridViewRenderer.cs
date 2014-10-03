using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.Extensions;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using PropertyChangingEventArgs = Xamarin.Forms.PropertyChangingEventArgs;

[assembly: ExportRenderer (typeof(GridView), typeof(GridViewRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Controls
{
    public class GridViewRenderer : ViewRenderer<GridView, GridCollectionView>
    {
        private UILabel _label;

        protected override void OnElementChanged(ElementChangedEventArgs<GridView> e)
        {
            base.OnElementChanged(e);

            var collectionView = new GridCollectionView
            {
                AllowsMultipleSelection = false,
                SelectionEnable = e.NewElement.SelectionEnabled,
                ContentInset = new UIEdgeInsets((float) Element.Padding.Top, (float) Element.Padding.Left, (float) Element.Padding.Bottom, (float) Element.Padding.Right),
                BackgroundColor = Element.BackgroundColor.ToUIColor(),
                ItemSize = new SizeF((float) Element.ItemWidth, (float) Element.ItemHeight),
                RowSpacing = Element.RowSpacing,
                ColumnSpacing = Element.ColumnSpacing
            };

            Unbind(e.OldElement);
            Bind(e.NewElement);

            collectionView.Source = DataSource;

            SetNativeControl(collectionView);

            if (_label == null)
            {
                _label = new UILabel
                {
                    AdjustsFontSizeToFitWidth = true,
                    TextAlignment = UITextAlignment.Center,
                    LineBreakMode = UILineBreakMode.WordWrap,
                    Lines = 0
                };

                SetLabelDetails();
                SetLabelSizeAndPosition();
                Control.AddSubview(_label);
            }

            ShowOrHideLabel(Element);
        }

        private void Unbind(GridView oldElement)
        {
            if (oldElement != null)
            {
                oldElement.PropertyChanging += ElementPropertyChanging;
                oldElement.PropertyChanged -= ElementPropertyChanged;
                if (oldElement.ItemsSource is INotifyCollectionChanged)
                    (oldElement.ItemsSource as INotifyCollectionChanged).CollectionChanged -= DataCollectionChanged;
            }
        }

        private void Bind(GridView newElement)
        {
            if (newElement != null)
            {
                newElement.PropertyChanging += ElementPropertyChanging;
                newElement.PropertyChanged += ElementPropertyChanged;
                if (newElement.ItemsSource is INotifyCollectionChanged)
                    (newElement.ItemsSource as INotifyCollectionChanged).CollectionChanged += DataCollectionChanged;
            }
        }

        protected virtual void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyNameMatches(() => Element.ItemsSource))
            {
                if (Element.ItemsSource is INotifyCollectionChanged)
                    (Element.ItemsSource as INotifyCollectionChanged).CollectionChanged -= DataCollectionChanged;
            }
        }

        private void ElementPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyNameMatches(() => Element.ItemsSource))
            {
                if (Element.ItemsSource is INotifyCollectionChanged)
                    (Element.ItemsSource as INotifyCollectionChanged).CollectionChanged += DataCollectionChanged;
            }
        }

        protected virtual void DataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            try
            {
                if (Control != null)
                    Control.ReloadData();
            }
            catch (Exception ex)
            {

            }
            
            ShowOrHideLabel(Element);
        }

        private GridDataSource _dataSource;

        private GridDataSource DataSource
        {
            get { return _dataSource ?? (_dataSource = new GridDataSource(GetCell, RowsInSection, ItemSelected)); }
        }

        private GridViewDelegate _gridViewDelegate;

        private GridViewDelegate GridViewDelegate
        {
            get { return _gridViewDelegate ?? (_gridViewDelegate = new GridViewDelegate(ItemSelected)); }
        }

        public int RowsInSection(UICollectionView collectionView, int section)
        {
            var collection = Element.ItemsSource as ICollection;
            return collection != null ? collection.Count : 0;
        }

        public void ItemSelected(UICollectionView tableView, NSIndexPath indexPath)
        {
            var item = Element.ItemsSource.Cast<object>().ElementAt(indexPath.Row);
            Element.InvokeItemSelectedEvent(this, item);
        }

        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = Element.ItemsSource.Cast<object>().ElementAt(indexPath.Row);
            var viewCellBinded = (ViewCell)Element.ItemTemplate.CreateContent();
            viewCellBinded.BindingContext = item;

            return GetCell(collectionView, viewCellBinded, indexPath);
        }

        protected virtual UICollectionViewCell GetCell(UICollectionView collectionView, ViewCell item, NSIndexPath indexPath)
        {
            var collectionCell = (GridViewCell)collectionView.DequeueReusableCell(new NSString(GridViewCell.Key), indexPath);

            collectionCell.ViewCell = item;

            return collectionCell;
        }
        
        private void ShowOrHideLabel(GridView view)
        {
            _label.Hidden = view.ItemsSource != null && view.ItemsSource.OfType<object>().Any();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            
            if (e.PropertyNameMatches(() => Element.NoItemsText))
            {
                SetLabelDetails();
                SetLabelSizeAndPosition();
            }

            if (e.PropertyNameMatches(() => Element.NoItemsTextColor))
                SetLabelDetails();

            if (e.PropertyNameMatches(() => Element.ItemsSource))
                ShowOrHideLabel(Element);
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

        private void SetLabelDetails()
        {
            _label.Text = Element.NoItemsText;
            _label.TextColor = Element.NoItemsTextColor.ToUIColor();
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            SetLabelSizeAndPosition();
        }
    }
}
