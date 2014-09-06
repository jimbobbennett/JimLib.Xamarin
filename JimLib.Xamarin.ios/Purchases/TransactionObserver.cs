using System;
using JimBobBennett.JimLib.Events;
using MonoTouch.StoreKit;

namespace JimBobBennett.JimLib.Xamarin.ios.Purchases
{
    internal class TransactionObserver : SKPaymentTransactionObserver
    {
        public override void UpdatedTransactions(SKPaymentQueue queue, SKPaymentTransaction[] transactions)
        {
            foreach (var transaction in transactions)
            {
                switch (transaction.TransactionState)
                {
                    case SKPaymentTransactionState.Purchased:
                    case SKPaymentTransactionState.Restored:
                        OnTransactionPurchased(transaction);
                        break;
                    case SKPaymentTransactionState.Failed:
                        OnTransactionFailed(transaction);
                        break;
                }
            }
        }

        public event EventHandler<EventArgs<SKPaymentTransaction>> TransactionPurchased;
        public event EventHandler<EventArgs<SKPaymentTransaction>> TransactionFailed;

        private void OnTransactionPurchased(SKPaymentTransaction e)
        {
            var handler = TransactionPurchased;
            if (handler != null)
                handler(this, new EventArgs<SKPaymentTransaction>(e));
        }

        private void OnTransactionFailed(SKPaymentTransaction e)
        {
            var handler = TransactionFailed;
            if (handler != null)
                handler(this, new EventArgs<SKPaymentTransaction>(e));
        }
    }
}