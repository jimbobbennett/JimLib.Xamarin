using System.Threading.Tasks;
using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Xamarin.ios.Extensions;
using JimBobBennett.JimLib.Xamarin.ios.Views;
using JimBobBennett.JimLib.Xamarin.Views;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BaseContentPage), typeof(BaseContentPageRenderer))]

namespace JimBobBennett.JimLib.Xamarin.ios.Views
{
    public class BaseContentPageRenderer : PageRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            ((BaseContentPage) Element).Appearing += (s, e1) => SetOrientation();
            ((BaseContentPage)Element).DisplayAlertFunc = ShowActionSheet;

            SetOrientation();
        }

        public override void WillRotate(UIInterfaceOrientation toInterfaceOrientation, double duration)
        {
            base.WillRotate(toInterfaceOrientation, duration);

            ((BaseContentPage)Element).OrientationChanging();
        }

        public override void DidRotate(UIInterfaceOrientation fromInterfaceOrientation)
        {
            base.DidRotate(fromInterfaceOrientation);

            SetOrientation();
        }

        private void SetOrientation()
        {
            ((BaseContentPage) Element).OrientationChanged(InterfaceOrientation.GetOrientation());
        }

        private async Task<string> ShowActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            string retVal = null;

            var alertContoller = UIAlertController.Create(title, string.Empty, UIAlertControllerStyle.ActionSheet);
            
            if (buttons != null)
            {
                foreach (var button in buttons)
                {
                    var button1 = button;
                    alertContoller.AddAction(UIAlertAction.Create(button, UIAlertActionStyle.Default, 
                        a => retVal = button1));
                }
            }

            if (UIDevice.CurrentDevice.UserInterfaceIdiom != UIUserInterfaceIdiom.Pad)
                alertContoller.AddAction(UIAlertAction.Create(cancel, UIAlertActionStyle.Cancel,
                    a => retVal = cancel));

            if (UIDevice.CurrentDevice.UserInterfaceIdiom != UIUserInterfaceIdiom.Pad &&
                !destruction.IsNullOrEmpty())
                alertContoller.AddAction(UIAlertAction.Create(destruction, UIAlertActionStyle.Destructive, 
                    a => retVal = destruction));

            if (alertContoller.PopoverPresentationController != null)
            {
                alertContoller.PopoverPresentationController.PermittedArrowDirections = 0;

                var rect = ViewController.View.Bounds;
                alertContoller.PopoverPresentationController.SourceRect = rect;
                alertContoller.PopoverPresentationController.SourceView = ViewController.View;
            }

            ViewController.PresentViewController(alertContoller, true, null);

            await this.WaitForAsync(() => !retVal.IsNullOrEmpty(), int.MaxValue);

            return retVal;
        }
    }
}
