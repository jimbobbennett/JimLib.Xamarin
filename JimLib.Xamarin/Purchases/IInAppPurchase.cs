using System;
using System.Collections.Generic;
using JimBobBennett.JimLib.Events;

namespace JimBobBennett.JimLib.Xamarin.Purchases
{
    public interface IInAppPurchase
    {
        event EventHandler<EventArgs<IEnumerable<IPurchasableProduct>>> LoadedAvailablePurchasableProducts;
        event EventHandler<EventArgs<IPurchasableProduct>> ProductPurchased;
        event EventHandler<EventArgs<IPurchasableProduct>> ProductPurchaseFailed;

        bool IsPurchasing { get; }
        bool CanMakePayments { get; }
        bool PurchasableProductsAvailable { get; }

        void LoadAvailableProducts(params string[] productIdentifiers);
        void PurchaseProduct(IPurchasableProduct purchasableProduct);
    }
}
