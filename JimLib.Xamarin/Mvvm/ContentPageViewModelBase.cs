using System;
using JimBobBennett.JimLib.Commands;
using JimBobBennett.JimLib.Events;
using JimBobBennett.JimLib.Mvvm;
using JimBobBennett.JimLib.Xamarin.Views;

namespace JimBobBennett.JimLib.Xamarin.Mvvm
{
    public abstract class ContentPageViewModelBase<T> : ViewModelBase<T>, IContentPageViewModelBase
    {
        protected ContentPageViewModelBase(T model) : base(model)
        {
            SetupCommands();
        }

        protected ContentPageViewModelBase()
        {
            SetupCommands();
        }

        private void SetupCommands()
        {
            CloseCommand = new RelayCommand(o => OnNeedClose());
        }

        public RelayCommand CloseCommand { get; private set; }

        public IView View { get; set; }

        public event EventHandler NeedClose
        {
            add { WeakEventManager.GetWeakEventManager(this).AddEventHandler("NeedClose", value); }
            remove { WeakEventManager.GetWeakEventManager(this).RemoveEventHandler("NeedClose", value); }
        }

        protected virtual void OnNeedClose()
        {
            WeakEventManager.GetWeakEventManager(this).HandleEvent(this, EventArgs.Empty, "NeedClose");
        }
    }

    public abstract class ContentPageViewModelBase : ContentPageViewModelBase<object>
    {
        protected ContentPageViewModelBase(object model) : base(model)
        {
        }

        protected ContentPageViewModelBase()
        {
        }
    }
}
