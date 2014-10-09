using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class ToolBar : View
    {
        public ToolBar()
        {
            SetValue(ToolBarButtonsProperty, new ObservableCollection<ToolBarButton>());
        }

        public static readonly BindableProperty ToolBarButtonsProperty =
            BindableProperty.Create<ToolBar, IList<ToolBarButton>>(p => p.ToolBarButtons, null,
            propertyChanging: AccessoryButtonsPropertyChanging);

        public static readonly BindableProperty TranslucentProperty =
            BindableProperty.Create<ToolBar, bool>(p => p.Translucent, false);

        public static readonly BindableProperty BarStyleProperty =
            BindableProperty.Create<ToolBar, BarStyle>(p => p.BarStyle, BarStyle.Light);

        public static readonly BindableProperty BarTintColorProperty =
            BindableProperty.Create<ToolBar, Color>(p => p.BarTintColor, Color.Default);

        public static readonly BindableProperty TintColorProperty =
            BindableProperty.Create<ToolBar, Color>(p => p.TintColor, Color.Default);
        
        private static void AccessoryButtonsPropertyChanging(BindableObject bindable, object oldvalue,
            object newvalue)
        {
            var oldList = oldvalue as List<ToolBarButton>;

            if (oldList != null)
            {
                foreach (var button in oldList)
                    button.BindingContext = null;
            }

            var oldObs = oldvalue as INotifyCollectionChanged;
            if (oldObs != null)
                oldObs.CollectionChanged -= ((ToolBar)bindable).ButtonsOnCollectionChanged;

            var newList = newvalue as List<ToolBarButton>;

            if (newList != null)
            {
                var entry = (ExtendedEntry)bindable;
                foreach (var button in newList)
                    button.BindingContext = entry.BindingContext;
            }

            var newObs = newvalue as INotifyCollectionChanged;
            if (newObs != null)
                newObs.CollectionChanged += ((ToolBar)bindable).ButtonsOnCollectionChanged;
        }

        private void ButtonsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            UpdateBindingContext();
        }

        private void UpdateBindingContext()
        {
            if (ToolBarButtons == null) return;

            foreach (var button in ToolBarButtons)
                button.BindingContext = BindingContext;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            UpdateBindingContext();
        }

        public IList<ToolBarButton> ToolBarButtons
        {
            get { return (IList<ToolBarButton>)GetValue(ToolBarButtonsProperty); }
            set { SetValue(ToolBarButtonsProperty, value); }
        }

        public bool Translucent
        {
            get { return (bool)GetValue(TranslucentProperty); }
            set { SetValue(TranslucentProperty, value); }
        }

        public BarStyle BarStyle
        {
            get { return (BarStyle)GetValue(BarStyleProperty); }
            set { SetValue(BarStyleProperty, value); }
        }

        public Color TintColor
        {
            get { return (Color)GetValue(TintColorProperty); }
            set { SetValue(TintColorProperty, value); }
        }

        public Color BarTintColor
        {
            get { return (Color)GetValue(BarTintColorProperty); }
            set { SetValue(BarTintColorProperty, value); }
        }
    }
}
