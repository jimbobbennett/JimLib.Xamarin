using JimBobBennett.JimLib.Mvvm;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.SocialMedia
{
    public abstract class BaseUser : NotificationObject
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                RaisePropertyChanged();
            }
        }

        public abstract string Type { get; }
        public abstract string DisplayName { get; }
    }
}