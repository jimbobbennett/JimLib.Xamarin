using System;

namespace JimBobBennett.JimLib.Xamarin.Purchases
{
    public class InAppPurchaseException : Exception
    {
        public InAppPurchaseException(string message) : base(message)
        {
        }
    }
}