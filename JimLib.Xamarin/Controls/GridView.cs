using System;
using System.Collections;
using System.Windows.Input;
using JimBobBennett.JimLib.Events;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class GridView : ContentView
    {
        public GridView()
        {
            SelectionEnabled = true;
        }

        public static readonly BindableProperty ItemsSourceProperty = 
            BindableProperty.Create<GridView,IEnumerable>(p => p.ItemsSource, null);

        public static readonly BindableProperty ItemTemplateProperty = 
            BindableProperty.Create<GridView, DataTemplate>(p => p.ItemTemplate, null);

        public static readonly BindableProperty RowSpacingProperty =
            BindableProperty.Create<GridView, double>(p => p.RowSpacing, 0);

        public static readonly BindableProperty ColumnSpacingProperty =
            BindableProperty.Create<GridView, double>(p => p.ColumnSpacing, 0);

        public static readonly BindableProperty ItemWidthProperty =
            BindableProperty.Create<GridView, double>(p => p.ItemWidth, 100);

        public static readonly BindableProperty ItemHeightProperty =
            BindableProperty.Create<GridView, double>(p => p.ItemHeight, 100);

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create<GridView, ICommand>(p => p.Command, null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create<GridView, object>(p => p.CommandParameter, null);

        public static readonly BindableProperty NoItemsTextProperty =
            BindableProperty.Create<GridView, string>(p => p.NoItemsText, string.Empty);

        public static readonly BindableProperty NoItemsTextColorProperty =
            BindableProperty.Create<GridView, Color>(p => p.NoItemsTextColor, Color.Black);

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable) GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate) GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public double RowSpacing
        {
            get { return (double) GetValue(RowSpacingProperty); }
            set { SetValue(RowSpacingProperty, value); }
        }

        public double ColumnSpacing
        {
            get { return (double) GetValue(ColumnSpacingProperty); }
            set { SetValue(ColumnSpacingProperty, value); }
        }

        public double ItemWidth
        {
            get { return (double) GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        public double ItemHeight
        {
            get { return (double) GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public string NoItemsText
        {
            get { return (string)GetValue(NoItemsTextProperty); }
            set { SetValue(NoItemsTextProperty, value); }
        }

        public Color NoItemsTextColor
        {
            get { return (Color)GetValue(NoItemsTextColorProperty); }
            set { SetValue(NoItemsTextColorProperty, value); }
        }

        public event EventHandler<EventArgs<object>> ItemSelected;

        public void InvokeItemSelectedEvent(object sender, object item)
        {
            if (ItemSelected != null)
                ItemSelected.Invoke(sender, new EventArgs<object>(item));

            if (Command != null)
            {
                var param = CommandParameter ?? item;
                if (Command.CanExecute(param))
                    Command.Execute(param);
            }
        }

        public bool SelectionEnabled { get; set; }
    }
}
