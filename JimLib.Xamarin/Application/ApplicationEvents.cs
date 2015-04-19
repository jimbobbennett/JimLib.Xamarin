using System;
using JimBobBennett.JimLib.Events;

namespace JimBobBennett.JimLib.Xamarin.Application
{
    public class ApplicationEvents : IApplicationEvents
    {
        public event EventHandler Start
        {
            add { WeakEventManager.GetWeakEventManager(this).AddEventHandler("Start", value); }
            remove { WeakEventManager.GetWeakEventManager(this).RemoveEventHandler("Start", value); }
        }

        public event EventHandler Appear
        {
            add { WeakEventManager.GetWeakEventManager(this).AddEventHandler("Appear", value); }
            remove { WeakEventManager.GetWeakEventManager(this).RemoveEventHandler("Appear", value); }
        }

        public event EventHandler Disappear
        {
            add { WeakEventManager.GetWeakEventManager(this).AddEventHandler("Disappear", value); }
            remove { WeakEventManager.GetWeakEventManager(this).RemoveEventHandler("Disappear", value); }
        }

        public event EventHandler Closing
        {
            add { WeakEventManager.GetWeakEventManager(this).AddEventHandler("Closing", value); }
            remove { WeakEventManager.GetWeakEventManager(this).RemoveEventHandler("Closing", value); }
        }

        public void OnStart()
        {
            WeakEventManager.GetWeakEventManager(this).RaiseEvent(this, EventArgs.Empty, "Start");
        }

        public void OnAppear()
        {
            WeakEventManager.GetWeakEventManager(this).RaiseEvent(this, EventArgs.Empty, "Appear");
        }

        public void OnDisappear()
        {
            WeakEventManager.GetWeakEventManager(this).RaiseEvent(this, EventArgs.Empty, "Disappear");
        }

        public void OnClosing()
        {
            WeakEventManager.GetWeakEventManager(this).RaiseEvent(this, EventArgs.Empty, "Closing");
        }
    }
}
