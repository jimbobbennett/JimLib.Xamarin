using System;
using System.ComponentModel;
using JimBobBennett.JimLib.Xamarin.Views;

namespace JimBobBennett.JimLib.Xamarin.Mvvm
{
    public interface IContentPageViewModelBase : INotifyPropertyChanged
    {
        IView View { get; set; }

        event EventHandler NeedClose;

        void OnOrientationChanged(Orientation orientation);
    }
}