using System;
using System.Collections.Generic;
using System.Linq;
using JimBobBennett.JimLib.Events;
using JimBobBennett.JimLib.Xamarin.Purchases;
using MonoTouch.Foundation;
using MonoTouch.StoreKit;

namespace JimBobBennett.JimLib.Xamarin.ios.Purchases
{
    public class InAppPurchase : IInAppPurchase
    {
        private Dictionary<string, IPurchasableProduct> _purchasableProducts;
        private readonly TransactionObserver _transactionObserver = new TransactionObserver();

        public InAppPurchase()
        {
            _transactionObserver.TransactionFailed += TransactionObserverOnTransactionFailed;
            _transactionObserver.TransactionPurchased += TransactionObserverOnTransactionPurchased;

            if (CanMakePayments)
                SKPaymentQueue.DefaultQueue.AddTransactionObserver(_transactionObserver);
        }

        private void TransactionObserverOnTransactionPurchased(object sender, EventArgs<SKPaymentTransaction> eventArgs)
        {
            SKPaymentQueue.DefaultQueue.FinishTransaction(eventArgs.Value);
            IsPurchasing = false;
            OnProductPurchased(_purchasableProducts[eventArgs.Value.Payment.ProductIdentifier]);
        }

        private void TransactionObserverOnTransactionFailed(object sender, EventArgs<SKPaymentTransaction> eventArgs)
        {
            SKPaymentQueue.DefaultQueue.FinishTransaction(eventArgs.Value);
            IsPurchasing = false;
            OnProductPurchaseFailed(_purchasableProducts[eventArgs.Value.Payment.ProductIdentifier]);
        }

        public event EventHandler<EventArgs<IEnumerable<IPurchasableProduct>>> LoadedAvailablePurchasableProducts
        {
            add { WeakEventManager.GetWeakEventManager(this).AddEventHandler("LoadedAvailablePurchasableProducts", value); }
            remove { WeakEventManager.GetWeakEventManager(this).RemoveEventHandler("LoadedAvailablePurchasableProducts", value); }
        }

        public event EventHandler<EventArgs<IPurchasableProduct>> ProductPurchased
        {
            add { WeakEventManager.GetWeakEventManager(this).AddEventHandler("ProductPurchased", value); }
            remove { WeakEventManager.GetWeakEventManager(this).RemoveEventHandler("ProductPurchased", value); }
        }

        public event EventHandler<EventArgs<IPurchasableProduct>> ProductPurchaseFailed
        {
            add { WeakEventManager.GetWeakEventManager(this).AddEventHandler("ProductPurchaseFailed", value); }
            remove { WeakEventManager.GetWeakEventManager(this).RemoveEventHandler("ProductPurchaseFailed", value); }
        }

        private void OnLoadedAvailablePurchasableProducts(IEnumerable<IPurchasableProduct> e)
        {
            WeakEventManager.GetWeakEventManager(this).RaiseEvent(this,
                new EventArgs<IEnumerable<IPurchasableProduct>>(e), "LoadedAvailablePurchasableProducts");
        }
        
        private void OnProductPurchased(IPurchasableProduct e)
        {
            WeakEventManager.GetWeakEventManager(this).RaiseEvent(this,
                new EventArgs<IPurchasableProduct>(e), "ProductPurchased");
        }
        
        private void OnProductPurchaseFailed(IPurchasableProduct e)
        {
            WeakEventManager.GetWeakEventManager(this).RaiseEvent(this,
                new EventArgs<IPurchasableProduct>(e), "ProductPurchaseFailed");
        }
       
        public bool IsPurchasing { get; private set; }

        public bool PurchasableProductsAvailable { get { return _purchasableProducts != null; } }

        public bool CanMakePayments { get { return SKPaymentQueue.CanMakePayments; } }

        private bool _loadCalled;
        private SKProductsRequest _request; // keep as a field so it doesn't get disposed

        public void LoadAvailableProducts(params string[] productIdentifiers)
        {
            if (!CanMakePayments)
                throw new InAppPurchaseException("Payments are not allowed");

            if (_loadCalled) return;

            _loadCalled = true;

            var productKeys = new NSSet(productIdentifiers);

            _request = new SKProductsRequest(productKeys);

            _request.RequestFailed += (s, e) => OnLoadedAvailablePurchasableProducts(new List<IPurchasableProduct>());

            _request.ReceivedResponse += (s, e) =>
                {
                    _purchasableProducts = e.Response.Products.ToDictionary(p => p.ProductIdentifier,
                        p => (IPurchasableProduct)new PurchasableProduct(p));

                    OnLoadedAvailablePurchasableProducts(_purchasableProducts.Values.ToList());
                };

            _request.Start();
        }

        public void PurchaseProduct(IPurchasableProduct purchasableProduct)
        {
            if (!CanMakePayments)
                throw new InAppPurchaseException("Payments are not allowed");

            if (_purchasableProducts == null)
                throw new InAppPurchaseException("Products not loaded");

            if (purchasableProduct == null)
                throw new ArgumentNullException("purchasableProduct");

            IsPurchasing = true;

            var payment = SKPayment.PaymentWithProduct(((PurchasableProduct)purchasableProduct).Product);
            SKPaymentQueue.DefaultQueue.AddPayment(payment);
        }
    }
}