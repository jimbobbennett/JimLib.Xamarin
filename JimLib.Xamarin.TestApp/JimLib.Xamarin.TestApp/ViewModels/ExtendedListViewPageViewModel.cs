using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JimBobBennett.JimLib.Collections;
using JimBobBennett.JimLib.Commands;
using JimBobBennett.JimLib.Xamarin.Mvvm;

namespace TestApp.ViewModels
{
    public class ExtendedListViewPageViewModel : ContentPageViewModelBase
    {
        private readonly ObservableCollectionEx<string> _items = new ObservableCollectionEx<string>(); 
        public IEnumerable<string> Items { get; set; }

        public ExtendedListViewPageViewModel()
        {
            Items = new ReadOnlyObservableCollection<string>(_items);

            _items.AddRange(new List<string>
            {
                "Hello",
                "World"
            });

            ItemTouchedCommand = new AsyncCommand<string>(async s => await View.ShowAlertAsync("Touched", s, "OK"));
        }

        public ICommand ItemTouchedCommand { get; private set; }
    }
}
