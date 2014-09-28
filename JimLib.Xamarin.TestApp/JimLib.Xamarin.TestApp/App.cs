using Autofac;
using JimBobBennett.JimLib.Xamarin;
using Xamarin.Forms;

namespace JimLib.Xamarin.TestApp
{
    public class App : AppBase
    {
        protected override void OnInitialize(ContainerBuilder builder)
        {
            
        }

        public override Page GetMainPage()
        {
            return new ContentPage
            {
                Content = new Label
                {
                    Text = "Hello, Forms !",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                },
            };
        }
    }
}
