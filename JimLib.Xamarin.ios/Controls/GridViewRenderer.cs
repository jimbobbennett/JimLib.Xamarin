using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using CoreGraphics;
using System.Linq;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.Extensions;
using JimBobBennett.JimLib.Xamarin.ios.Controls;
using Foundation;
using UIKit;
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

            try
            {
                var collectionView = new GridCollectionView
                {
                    AllowsMultipleSelection = false,
                    SelectionEnable = e.NewElement != null && e.NewElement.SelectionEnabled,
                    ContentInset = new UIEdgeInsets((float) Element.Padding.Top, (float) Element.Padding.Left, (float) Element.Padding.Bottom, (float) Element.Padding.Right),
                    BackgroundColor = Element.BackgroundColor.ToUIColor(),
                    ItemSize = new CGSize((float) Element.ItemWidth, (float) Element.ItemHeight),
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

                if (Element != null)
                    ShowOrHideLabel(Element);
            }
            catch
            {
                // do nothing as this should be called again
            }
        }

        private void Unbind(GridView oldElement)
        {
            if (oldElement != null)
            {
                oldElement.PropertyChanging -= ElementPropertyChanging;
                oldElement.PropertyChanged -= ElementPropertyChanged;
                var notifyCollectionChanged = oldElement.ItemsSource as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                    notifyCollectionChanged.CollectionChanged -= DataCollectionChanged;
            }
        }

        private void Bind(GridView newElement)
        {
            if (newElement != null)
            {
                newElement.PropertyChanging += ElementPropertyChanging;
                newElement.PropertyChanged += ElementPropertyChanged;
                var notifyCollectionChanged = newElement.ItemsSource as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                    notifyCollectionChanged.CollectionChanged += DataCollectionChanged;
            }
        }

        protected virtual void ElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyNameMatches(() => Element.ItemsSource))
                {
                    var notifyCollectionChanged = Element.ItemsSource as INotifyCollectionChanged;
                    if (notifyCollectionChanged != null)
                        notifyCollectionChanged.CollectionChanged += DataCollectionChanged;
                }
            }
            catch
            {
                // do nothing
            }
        }

        private void ElementPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (e.PropertyNameMatches(() => Element.ItemsSource))
            {
                var notifyCollectionChanged = Element.ItemsSource as INotifyCollectionChanged;
                if (notifyCollectionChanged != null)
                    notifyCollectionChanged.CollectionChanged -= DataCollectionChanged;
            }
        }

        protected virtual void DataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            Console.WriteLine("Collection updated");
            InvokeOnMainThread(UpdateFromCollectionChange);
        }

        private void UpdateFromCollectionChange()
        {
            try
            {
                if (Control != null)
                    Control.ReloadData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception reloading data: " + ex.Message);
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

        public nint RowsInSection(UICollectionView collectionView, nint section)
        {
            return Element.ItemsSource != null ? Element.ItemsSource.Cast<object>().Count() : 0;
        }

        public void ItemSelected(UICollectionView tableView, NSIndexPath indexPath)
        {
            if (Element.ItemsSource.Cast<object>().Count() > indexPath.Row)
            {
                try
                {
                    var item = Element.ItemsSource.Cast<object>().ElementAt(indexPath.Row);
                    Element.InvokeItemSelectedEvent(this, item);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Failed to select item at path: " + indexPath);
                }
            }
        }

        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var viewCellBinded = (ViewCell)Element.ItemTemplate.CreateContent();
            try
            {
                viewCellBinded.BindingContext = Element.ItemsSource.Cast<object>().ElementAt(indexPath.Row);
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to select item at path: " + indexPath);
            }

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
            _label.Hidden = view != null && view.ItemsSource != null && view.ItemsSource.OfType<object>().Any();
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
            var size = _label.SizeThatFits(new CGSize(Control.Frame.Width, float.MaxValue));
            
            var newFrame = new CGRect((Control.Frame.Width - size.Width) / 2,
                (Control.Frame.Height - size.Height) / 2,
                size.Width,
                size.Height);

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
