using System;
using JimBobBennett.JimLib.Xamarin.Views;

namespace JimBobBennett.JimLib.Xamarin.Mvvm
{
    public interface IContentPageViewModelBase
    {
        IView View { get; set; }

        event EventHandler NeedClose;
    }
}