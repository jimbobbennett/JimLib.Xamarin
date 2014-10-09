using System.Drawing;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.Controls;
using JimBobBennett.JimLib.Xamarin.ios.Images;
using MonoTouch.UIKit;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.ios.Extensions
{
    public static class ToolBarButtonExtensions
    {
        public static UIBarButtonItem CreateButton(this ToolBarButton button)
        {
            UIBarButtonItem uiButton;

            if (button.SystemButtonItem == SystemButtonItem.None)
            {
                uiButton = new UIBarButtonItem(button.Text, UIBarButtonItemStyle.Plain,
                    (s, e) => HandleButtonClick(button));

                if (button.ImageSource != null)
                    SetImage(uiButton, button.ImageSource);
            }
            else
            {
                uiButton = new UIBarButtonItem(button.SystemButtonItem.ConvertToSystemItem(),
                    (s, e) => HandleButtonClick(button));
                
                if (button.SystemButtonItem == SystemButtonItem.Done)
                    uiButton.Style = UIBarButtonItemStyle.Done;
            }

            WireUpEnabledToCanExecute(button, uiButton);

            button.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyNameMatches(() => button.Command))
                        WireUpEnabledToCanExecute(button, uiButton);
                };

            return uiButton;
        }

        public static UIBarButtonItem CreateButtonForToolbar(this ToolBarButton toolBarButton)
        {
            if (toolBarButton.ImageSource == null)
                return toolBarButton.CreateButton();

            var uiButton = new UIButton(UIButtonType.Custom);
            SetImage(uiButton, toolBarButton.ImageSource);
            uiButton.Frame = new RectangleF (0, 0, 24, 24);

            var barButtonItem = new UIBarButtonItem (uiButton);

            uiButton.TouchUpInside += (sender, e) => HandleButtonClick(toolBarButton);

            WireUpEnabledToCanExecute(toolBarButton, barButtonItem);

            toolBarButton.PropertyChanged += (s, e) =>
            {
                if (e.PropertyNameMatches(() => toolBarButton.Command))
                    barButtonItem.InvokeOnMainThread(() => WireUpEnabledToCanExecute(toolBarButton, barButtonItem));
            };

            return barButtonItem;
        }

        private static void HandleButtonClick(ToolBarButton button)
        {
            if (button.Command != null && button.Command.CanExecute(button.CommandParameter))
                button.Command.Execute(button.CommandParameter);
        }

        private static void WireUpEnabledToCanExecute(ToolBarButton button, UIBarItem uiButton)
        {
            if (button.Command != null)
                button.Command.CanExecuteChanged += (s, e) =>
                        uiButton.InvokeOnMainThread(() =>
                            {
                                var enabled = button.Command.CanExecute(button.CommandParameter);

                                if (uiButton.Enabled == enabled) return;

                                uiButton.Enabled = enabled;


                                if (uiButton.Image != null)
                                    uiButton.Image = ImageHelper.AdjustOpacity(uiButton.Image, enabled ? 1 : 0.5f);
                            });

            uiButton.Enabled = button.Command == null || button.Command.CanExecute(button.CommandParameter);

            if (!uiButton.Enabled && uiButton.Image != null)
                uiButton.Image = ImageHelper.AdjustOpacity(uiButton.Image, 0.5f);
        }

        private static async void SetImage(UIBarItem uiButton, ImageSource source)
        {
            uiButton.Image = await source.GetImageAsync();
        }

        private static async void SetImage(UIButton uiButton, ImageSource source)
        {
            uiButton.SetImage(await source.GetImageAsync(), UIControlState.Normal);
        }
    }
}
