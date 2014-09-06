using MonoTouch.Foundation;
using MonoTouch.StoreKit;

namespace JimBobBennett.JimLib.Xamarin.ios.Purchases
{
// ReSharper disable once InconsistentNaming
	internal static class SKProductExtension 
	{
		/// <remarks>
		/// Use Apple's sample code for formatting a SKProduct price
		/// https://developer.apple.com/library/ios/#DOCUMENTATION/StoreKit/Reference/SKProduct_Reference/Reference/Reference.html#//apple_ref/occ/instp/SKProduct/priceLocale
		/// </remarks>
		public static string LocalizedPrice (this SKProduct product)
		{
			var formatter = new NSNumberFormatter
			{
			    FormatterBehavior = NSNumberFormatterBehavior.Version_10_4,
                NumberStyle = NSNumberFormatterStyle.Currency, 
                Locale = product.PriceLocale
			};

		    return formatter.StringFromNumber(product.Price);
		}
	}
}

