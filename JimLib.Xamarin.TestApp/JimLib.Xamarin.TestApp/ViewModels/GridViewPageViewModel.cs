using System;

using Xamarin.Forms;
using JimBobBennett.JimLib.Xamarin.Mvvm;
using System.Collections.ObjectModel;
using System.Windows.Input;
using JimBobBennett.JimLib.Commands;

namespace TestApp
{
    public class GridViewPageViewModel : ContentPageViewModelBase
    {
        private ObservableCollection<string> _items = new ObservableCollection<string>();
        public ReadOnlyObservableCollection<string> Items { get; private set; }
        public ICommand AddItemCommand { get; private set; }

        public GridViewPageViewModel()
        {
            Items = new ReadOnlyObservableCollection<string>(_items);
            AddItemCommand = new RelayCommand(o => _items.Add("Hello"));
        }
    }
}


