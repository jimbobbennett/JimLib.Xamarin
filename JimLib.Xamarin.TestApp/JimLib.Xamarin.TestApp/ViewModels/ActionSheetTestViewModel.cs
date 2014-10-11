using System.Windows.Input;
using JimBobBennett.JimLib.Commands;
using JimBobBennett.JimLib.Xamarin.Mvvm;

namespace TestApp.ViewModels
{
    public class ActionSheetTestViewModel : ContentPageViewModelBase
    {
        public ActionSheetTestViewModel()
        {
            ShowActionSheetCommand = new AsyncCommand(async () =>
                {
                    var result = await View.GetOptionFromUserAsync("Options", "Cancel", "Destroy", "Foo", "Bar");
                    await View.ShowAlertAsync("Selection", result, "OK");
                });
        }

        public ICommand ShowActionSheetCommand { get; private set; }
    }
}
