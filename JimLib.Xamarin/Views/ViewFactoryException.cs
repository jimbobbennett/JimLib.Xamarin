using System;

namespace JimBobBennett.JimLib.Xamarin.Views
{
    public class ViewFactoryException : Exception
    {
        public Type ViewType { get; private set; }
        public Type ViewModelType { get; private set; }

        public ViewFactoryException(string message, Type viewType, Type viewModelType)
            : base(message)
        {
            ViewType = viewType;
            ViewModelType = viewModelType;
        }

        public ViewFactoryException(string message, Type viewModelType)
            : base(message)
        {
            ViewModelType = viewModelType;
        }

        public ViewFactoryException(string message, Type viewType, Type viewModelType, Exception innerException)
            : base(message, innerException)
        {
            ViewType = viewType;
            ViewModelType = viewModelType;
        }
    }
}