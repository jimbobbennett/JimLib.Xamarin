using System;
using System.Collections.Generic;
using Autofac;
using JimBobBennett.JimLib.Xamarin.Mvvm;

namespace JimBobBennett.JimLib.Xamarin.Views
{
    public class ViewFactory : IViewFactory
    {
        private readonly IComponentContext _componentContext;
        private readonly object _syncObj = new object();
        private readonly Dictionary<Type, Type> _viewsForViewModels = new Dictionary<Type, Type>();

        public ViewFactory(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }

        public void Register<TView, TViewModel>()
            where TView : BaseContentPage
            where TViewModel : class, IContentPageViewModelBase
        {
            lock (_syncObj)
            {
                if (_viewsForViewModels.ContainsKey(typeof(TViewModel)))
                    throw new ViewFactoryException("ViewModel is already registered", typeof(TView), typeof(TViewModel));

                _viewsForViewModels[typeof (TViewModel)] = typeof (TView);
            }
        }

        public BaseContentPage ResolveView<TViewModel>()
            where TViewModel : class, IContentPageViewModelBase
        {
            lock (_syncObj)
            {
                Type viewType;
                if (_viewsForViewModels.TryGetValue(typeof (TViewModel), out viewType))
                    return _componentContext.Resolve(viewType) as BaseContentPage;

                throw new ViewFactoryException("View not found for ViewModel", typeof(TViewModel));
            }
        }
    }
}
