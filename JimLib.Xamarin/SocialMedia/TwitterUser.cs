using JimBobBennett.JimLib.Mvvm;

namespace JimBobBennett.JimLib.Xamarin.SocialMedia
{
    public class TwitterUser : BaseUser
    {
        private string _handle;
        private bool _canEditHandle;

        [NotifyPropertyChangeDependency("DisplayName")]
        public string Handle
        {
            get { return _handle; }
            set
            {
                if (_handle == value) return;
                _handle = value;
                RaisePropertyChanged();
            }
        }

        public override string Type
        {
            get { return "Twitter"; }
        }

        public override string DisplayName
        {
            get { return Handle; }
        }

        public bool CanEditHandle
        {
            get { return _canEditHandle; }
            set
            {
                if (_canEditHandle == value) return;

                _canEditHandle = value;
                RaisePropertyChanged();
            }
        }
    }
}
