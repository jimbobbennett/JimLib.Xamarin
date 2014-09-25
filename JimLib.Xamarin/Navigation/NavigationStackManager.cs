using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using JimBobBennett.JimLib.Events;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Navigation
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class NavigationStackManager : INavigationStackManager
    {
        enum PageState
        {
            Modal,
            Normal,
            Root
        }

        private Stack<Tuple<Page, PageState>> _pages;
        private readonly IComponentContext _componentContext;

        public NavigationStackManager(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public void SetPages(Page rootPage, NavigationPage navigationPage)
        {
            // handle the back navigation
            navigationPage.Popped += (s, e) =>
            {
                if (_pages.Peek().Item1 == e.Page)
                    OnPagePopped(_pages.Pop().Item1);
            };

            _pages = new Stack<Tuple<Page, PageState>>();
            _pages.Push(Tuple.Create(rootPage, PageState.Root));
        }

        public event EventHandler<EventArgs<Page>> PagePushed
        {
            add { WeakEventManager.GetWeakEventManager(this).AddEventHandler("PagePushed", value); }
            remove { WeakEventManager.GetWeakEventManager(this).RemoveEventHandler("PagePushed", value); }
        }

        public event EventHandler<EventArgs<Page>> PagePopped
        {
            add { WeakEventManager.GetWeakEventManager(this).AddEventHandler("PagePopped", value); }
            remove { WeakEventManager.GetWeakEventManager(this).RemoveEventHandler("PagePopped", value); }
        }

        public Page TopPage { get { return _pages.Peek().Item1; } }

        private void OnPagePushed(Page page)
        {
            WeakEventManager.GetWeakEventManager(this).RaiseEvent(this, new EventArgs<Page>(page), "PagePushed");
        }

        private void OnPagePopped(Page page)
        {
            WeakEventManager.GetWeakEventManager(this).RaiseEvent(this, new EventArgs<Page>(page), "PagePopped");
        }

        public async Task PushModalAsync(Page page)
        {
            if (_pages == null) throw new NavigationException("Not set up");

            var top = _pages.Peek();
            _pages.Push(Tuple.Create(page, PageState.Modal));

            await top.Item1.Navigation.PushModalAsync(page);

            OnPagePushed(page);
        }

        public async Task PushAsync(Page page)
        {
            if (_pages == null) throw new NavigationException("Not set up");

            var top = _pages.Peek();
            _pages.Push(Tuple.Create(page, PageState.Normal));

            await top.Item1.Navigation.PushAsync(page);

            OnPagePushed(page);
        }

        public async Task PushModalAsync<TPage>(Action<TPage> startupAction = null) where TPage : Page
        {
            var page = _componentContext.Resolve<TPage>();

            if (startupAction != null)
                startupAction(page);

            await PushModalAsync(page);
        }

        public async Task PushAsync<TPage>(Action<TPage> startupAction = null) where TPage : Page
        {
            var page = _componentContext.Resolve<TPage>();

            if (startupAction != null)
                startupAction(page);

            await PushAsync(page);
        }

        public async Task PopAsync()
        {
            if (_pages == null) throw new NavigationException("Not set up");

            var top = _pages.Peek();

            switch (top.Item2)
            {
                case PageState.Normal:
                    _pages.Pop();
                    await top.Item1.Navigation.PopAsync();
                    OnPagePopped(top.Item1);
                    break;
                case PageState.Modal:
                    _pages.Pop();
                    await top.Item1.Navigation.PopModalAsync();
                    OnPagePopped(top.Item1);
                    break;
            }
        }

        public async Task RollbackToRootAsync()
        {
            if (_pages == null) throw new NavigationException("Not set up");

            Tuple<Page, PageState> top;

            while ((top = _pages.Peek()).Item2 != PageState.Root)
            {
                _pages.Pop();

                switch (top.Item2)
                {
                    case PageState.Modal:
                        await top.Item1.Navigation.PopModalAsync();
                        break;
                    case PageState.Normal:
                        await top.Item1.Navigation.PopAsync();
                        break;
                }
            }
        }
    }
}
