using System.Collections.Generic;
using System.Windows.Input;
using JimBobBennett.JimLib.Commands;
using JimBobBennett.JimLib.Xamarin.Images;
using JimBobBennett.JimLib.Xamarin.Mvvm;
using Xamarin.Forms;

namespace TestApp.ViewModels
{
    public class CaptureImagePageViewModel : ContentPageViewModelBase
    {
        public CaptureImagePageViewModel(IImageHelper imageHelper)
        {
            GetImageCommand = new AsyncCommand(async () =>
                {
                    if (imageHelper.AvailablePhotoSources == PhotoSource.None) return;

                    var options = new List<string>();

                    if ((imageHelper.AvailablePhotoSources & PhotoSource.Camera) == PhotoSource.Camera)
                        options.Add("Take photo");
                    if ((imageHelper.AvailablePhotoSources & PhotoSource.Existing) == PhotoSource.Existing)
                        options.Add("Choose existing");

                    var result = await View.GetOptionFromUserAsync(null, "Cancel", null, options.ToArray());

                    ImageSource image = null;
                    switch (result)
                    {
                        case "Take photo":
                            image = await imageHelper.GetImageAsync(PhotoSource.Camera);
                            break;
                        case "Choose existing":
                            image = await imageHelper.GetImageAsync(PhotoSource.Existing);
                            break;
                    }

                    if (image != null)
                        ImageSource = image;
                });
        }

        private ImageSource _imageSource;

        public ImageSource ImageSource
        {
            get { return _imageSource; }
            set
            {
                if (_imageSource == value) return;

                _imageSource = value;
                RaisePropertyChanged();
            }
        }

        public ICommand GetImageCommand { get; private set; }
    }
}
