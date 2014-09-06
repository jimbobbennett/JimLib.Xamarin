namespace JimBobBennett.JimLib.Xamarin.Purchases
{
	public interface IPurchasableProduct
	{
		string Title { get;}
		string Description { get;}
		string Price { get;}
		string ProductIdentifier { get;}
	}
    
}
