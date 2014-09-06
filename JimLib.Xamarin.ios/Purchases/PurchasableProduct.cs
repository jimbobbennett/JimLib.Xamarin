using JimBobBennett.JimLib.Xamarin.Purchases;
using MonoTouch.StoreKit;

namespace JimBobBennett.JimLib.Xamarin.ios.Purchases
{
	internal class PurchasableProduct : IPurchasableProduct
	{
		public PurchasableProduct(SKProduct product)
		{
			Product = product;
			Price = product.LocalizedPrice();
			Description = product.LocalizedDescription;
			Title = product.LocalizedTitle;
			ProductIdentifier = product.ProductIdentifier;
		}

		public string Title { get; private set;}
		public string Description { get; private set;}
		public string Price { get; private set;}
		public string ProductIdentifier { get; private set;}

		public SKProduct Product { get; set;}
	}
    
}