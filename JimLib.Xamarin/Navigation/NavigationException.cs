using System;

namespace JimBobBennett.JimLib.Xamarin.Navigation
{
    public class NavigationException : Exception
    {
        public NavigationException(string message)
            : base(message)
        {
        }
    }
}