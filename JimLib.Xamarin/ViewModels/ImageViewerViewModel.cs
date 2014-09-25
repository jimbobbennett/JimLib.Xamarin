using JimBobBennett.JimLib.Extensions;
using JimBobBennett.JimLib.Mvvm;
using JimBobBennett.JimLib.Xamarin.Mvvm;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.ViewModels
{
    public class ImageViewerViewModel : ContentPageViewModelBase
    {
        private ImageSource _imageSource;
        private string _imageText;
        private string _imageTitleText;

        public ImageSource ImageSource
        {
            get { return _imageSource; }
            private set
            {
                if (Equals(ImageSource, value)) return;

                _imageSource = value;

                RaisePropertyChanged();
            }
        }

        [NotifyPropertyChangeDependency("ShowImageText")]
        public string ImageText
        {
            get { return _imageText; }
            private set
            {
                if (_imageText == value) return;

                _imageText = value;

                RaisePropertyChanged();
            }
        }

        [NotifyPropertyChangeDependency("ShowImageTitleText")]
        public string ImageTitleText
        {
            get { return _imageTitleText; }
            private set
            {
                if (_imageTitleText == value) return;

                _imageTitleText = value;

                RaisePropertyChanged();
            }
        }

        public bool ShowImageText { get { return !ImageText.IsNullOrEmpty(); } }

        public void SetImage(ImageSource imageSource, string imageTitleText = null, string imageText = null)
        {
            ImageSource = imageSource;
            ImageText = imageText;
            ImageTitleText = imageTitleText;
        }
    }
}
