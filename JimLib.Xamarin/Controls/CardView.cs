using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using JimBobBennett.JimLib.Commands;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class CardView : PullToRefreshScrollView
    {
        public static readonly BindableProperty PortraitColumnsCountProperty =
            BindableProperty.Create<CardView, int>(p => p.PortraitColumnsCount, 2,
            propertyChanged: (s, o, n) => ((CardView)s).BuildGrid());

        public static readonly BindableProperty LandscapeColumnsCountProperty =
            BindableProperty.Create<CardView, int>(p => p.LandscapeColumnsCount, 4,
            propertyChanged: (s, o, n) => ((CardView)s).BuildGrid());

        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create<CardView, DataTemplate>(p => p.ItemTemplate, null,
            propertyChanged: (s, o, n) => ((CardView)s).BuildGrid());

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<CardView, object>(p => p.ItemsSource, null,
            propertyChanged: ItemsSourceChanged);

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create<CardView, ICommand>(p => p.Command, null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create<CardView, object>(p => p.CommandParameter, null);

        public int PortraitColumnsCount
        {
            get { return (int)GetValue(PortraitColumnsCountProperty); }
            set { SetValue(PortraitColumnsCountProperty, value); }
        }

        public int LandscapeColumnsCount
        {
            get { return (int)GetValue(LandscapeColumnsCountProperty); }
            set { SetValue(LandscapeColumnsCountProperty, value); }
        }

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
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

        private readonly Grid _grid = new Grid();

        public CardView()
        {
            SizeChanged += (s, e) => BuildGrid();

            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.Fill;
            
            _grid.HorizontalOptions = LayoutOptions.Fill;
            _grid.VerticalOptions = LayoutOptions.Start;

            Content = _grid;
        }

        private static void ItemsSourceChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var oldColChanged = oldvalue as INotifyCollectionChanged;
            if (oldColChanged != null)
                oldColChanged.CollectionChanged -= ((CardView)bindable).ItemsSourceCollectionChanged;

            var newColChanged = newvalue as INotifyCollectionChanged;
            if (newColChanged != null)
                newColChanged.CollectionChanged += ((CardView)bindable).ItemsSourceCollectionChanged;

            ((CardView)bindable).BuildGrid();
        }

        private void ItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
           BuildGrid();
        }

        private void BuildGrid()
        {
            Device.BeginInvokeOnMainThread(BuildGridImpl);
        }

        private void BuildGridImpl()
        {
            _grid.Children.Clear();
            _grid.ColumnDefinitions.Clear();
            _grid.RowDefinitions.Clear();

            var columnCount = Height > Width ? PortraitColumnsCount : LandscapeColumnsCount;

            if (columnCount == 0)
                return;

            var items = ItemsSource as IEnumerable;
            if (items == null) return;

            var itemsList = items.OfType<object>().ToList();

            var rowsCount = itemsList.Count/columnCount;

            if (itemsList.Count%columnCount != 0)
                rowsCount++;

            for (var i = 0; i < columnCount; ++i)
                _grid.ColumnDefinitions.Add(new ColumnDefinition {Width = new GridLength(1, GridUnitType.Star)});

            for (var i = 0; i < rowsCount; ++i)
                _grid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});

            var row = 0;
            var column = 0;

            foreach (var o in itemsList)
                CreateViewCard(o, columnCount, ref row, ref column);
        }

        private void CreateViewCard(object o, int columnCount, ref int row, ref int column)
        {
            var view = new ContentView
            {
                Content = (View) ItemTemplate.CreateContent(),
                BindingContext = o,
                Padding = new Thickness(2),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start,
                WidthRequest = (Width / columnCount) - 20
            };

            view.Content.VerticalOptions = LayoutOptions.Start;

            var grid = new TappableGrid();
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            grid.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});
            grid.Command = new RelayCommand(p =>
                {
                    if (Command != null && Command.CanExecute(p ?? CommandParameter ?? o))
                        Command.Execute(p ?? CommandParameter ?? o);
                },
                p => Command != null && Command.CanExecute(p ?? CommandParameter ?? o));

            grid.Children.Add(view);

            _grid.Children.Add(grid);
            Grid.SetColumn(grid, column);
            Grid.SetRow(grid, row);

            ++column;
            if (column >= columnCount)
            {
                column = 0;
                ++row;
            }
        }
    }
}
