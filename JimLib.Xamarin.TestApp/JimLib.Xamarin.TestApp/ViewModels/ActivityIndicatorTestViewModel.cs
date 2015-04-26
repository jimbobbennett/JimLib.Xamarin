using System.Threading.Tasks;
using System.Windows.Input;
using JimBobBennett.JimLib.Commands;
using JimBobBennett.JimLib.Xamarin.Mvvm;

namespace TestApp.ViewModels
{
    public class ActivityIndicatorTestViewModel : ContentPageViewModelBase
    {
        public ActivityIndicatorTestViewModel()
        {
            DoBusyActionCommand = new AsyncCommand(async () =>
            {
                await RunWithBusyIndicatorAsync(async () =>
                {
                    await Task.Delay(10000);
                }, "Loading data...");
            });
        }

        public ICommand DoBusyActionCommand { get; private set; }
    }
}
