using JimBobBennett.JimLib.Xamarin.Mvvm;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Views
{
    public interface IViewFactory
    {
        void Register<TView, TViewModel>()
            where TView : BaseContentPage
            where TViewModel : class, IContentPageViewModelBase;

        BaseContentPage ResolveView<TViewModel>()
            where TViewModel : class, IContentPageViewModelBase;
    }
}