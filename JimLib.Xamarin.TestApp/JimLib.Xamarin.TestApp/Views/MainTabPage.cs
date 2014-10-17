using JimBobBennett.JimLib.Xamarin.Controls;

namespace TestApp.Views
{
    public class MainTabPage : ExtendedTabbedPage
    {
        public MainTabPage(MainPage mainPage)
        {
            Children.Add(mainPage);
        }
    }
}
