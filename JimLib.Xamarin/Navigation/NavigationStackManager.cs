using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Navigation
{
    public class NavigationStackManager : INavigationStackManager
    {
        enum PageState
        {
            Modal,
            Normal,
            Root
        }

        private Stack<Tuple<Page, PageState>> _pages;

        public void SetPages(Page rootPage, NavigationPage navigationPage)
        {
            // handle the back navigation
            navigationPage.Popped += (s, e) =>
            {
                if (_pages.Peek().Item1 == e.Page)
                    _pages.Pop();
            };

            _pages = new Stack<Tuple<Page, PageState>>();
            _pages.Push(Tuple.Create(rootPage, PageState.Root));
        }

        public async Task PushModalAsync(Page page)
        {
            if (_pages == null) throw new NavigationException("Not set up");

            var top = _pages.Peek();
            _pages.Push(Tuple.Create(page, PageState.Modal));

            await top.Item1.Navigation.PushModalAsync(page);
        }

        public async Task PopModalAsync()
        {
            if (_pages == null) throw new NavigationException("Not set up");

            var top = _pages.Peek();
            if (top.Item2 == PageState.Modal)
            {
                _pages.Pop();
                await top.Item1.Navigation.PopModalAsync();
            }
            else
                throw new NavigationException("Cannot pop modal - the top page is not modal");
        }

        public async Task PushAsync(Page page)
        {
            if (_pages == null) throw new NavigationException("Not set up");

            var top = _pages.Peek();
            _pages.Push(Tuple.Create(page, PageState.Normal));

            await top.Item1.Navigation.PushAsync(page);
        }

        public async Task PopAsync()
        {
            if (_pages == null) throw new NavigationException("Not set up");

            var top = _pages.Peek();
            if (top.Item2 == PageState.Normal)
            {
                _pages.Pop();
                await top.Item1.Navigation.PopAsync();
            }
            else
                throw new NavigationException("Cannot pop - the top page is shown modal");
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

    public class NavigationException : Exception
    {
        public NavigationException(string message)
            : base(message)
        {
        }
    }
}
