using System.Windows.Input;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class ExtendedListView : ListView
    {
        public ExtendedListView()
        {
            ItemTapped += (s, e) =>
                {
                    if (Command != null)
                    {
                        var param = CommandParameter ?? e.Item;
                        if (Command.CanExecute(param))
                            Command.Execute(param);
                    }
                };
        }
        
        public static readonly BindableProperty ShowEmptyCellsProperty =
            BindableProperty.Create<ExtendedListView, bool>(p => p.ShowEmptyCells, true);

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create<ExtendedListView, ICommand>(p => p.Command, null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create<ExtendedListView, object>(p => p.CommandParameter, null);
        
        public bool ShowEmptyCells
        {
            get { return (bool)GetValue(ShowEmptyCellsProperty); }
            set { SetValue(ShowEmptyCellsProperty, value); }
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
    }
}
