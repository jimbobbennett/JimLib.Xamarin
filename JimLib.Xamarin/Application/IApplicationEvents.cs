using System;

namespace JimBobBennett.JimLib.Xamarin.Application
{
    public interface IApplicationEvents
    {
        event EventHandler Start;
        event EventHandler Appear;
        event EventHandler Disappear;
        event EventHandler Closing;
        void OnStart();
        void OnAppear();
        void OnDisappear();
        void OnClosing();
    }
}
