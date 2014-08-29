using JimBobBennett.JimLib.Collections;
using JimBobBennett.JimLib.Mvvm;
using JimBobBennett.JimLib.Xamarin.SocialMedia;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Contacts
{
    public class ContactOverview : NotificationObject
    {
        private ImageSource _thumbImageSource;
        private string _suffix;
        private string _prefix;
        private string _nickName;
        private string _lastName;
        private string _middleName;
        private string _firstName;
        private string _thumbBase64;

        public ContactOverview()
        {
            Emails = new ObservableCollectionEx<Email>();
            Phones = new ObservableCollectionEx<Phone>();
            Websites = new ObservableCollectionEx<Website>();
            Organizations = new ObservableCollectionEx<Organization>();
            Addresses = new ObservableCollectionEx<Address>();
            InstantMessagingAccounts = new ObservableCollectionEx<InstantMessagingAccount>();
            SocialMediaUsers = new ObservableCollectionEx<BaseUser>();
        }
        
        public string DisplayName { get; set; }

        public ImageSource GetThumbImageSource()
        {
            return _thumbImageSource;
        }

        public void SetThumbImageSource(ImageSource imageSource)
        {
           _thumbImageSource = imageSource;
        }

        public string ThumbBase64
        {
            get { return _thumbBase64; }
            set
            {
                if (value == _thumbBase64) return;
                _thumbBase64 = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollectionEx<Email> Emails { get; set; }
        public ObservableCollectionEx<Phone> Phones { get; set; }
        public ObservableCollectionEx<Website> Websites { get; set; }
        public ObservableCollectionEx<Organization> Organizations { get; set; }
        public ObservableCollectionEx<Address> Addresses { get; set; }
        public ObservableCollectionEx<InstantMessagingAccount> InstantMessagingAccounts { get; set; }
        public ObservableCollectionEx<BaseUser> SocialMediaUsers { get; set; }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (value == _firstName) return;
                _firstName = value;
                RaisePropertyChanged();
            }
        }

        public string MiddleName
        {
            get { return _middleName; }
            set
            {
                if (value == _middleName) return;
                _middleName = value;
                RaisePropertyChanged();
            }
        }

        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (value == _lastName) return;
                _lastName = value;
                RaisePropertyChanged();
            }
        }

        public string NickName
        {
            get { return _nickName; }
            set
            {
                if (value == _nickName) return;
                _nickName = value;
                RaisePropertyChanged();
            }
        }

        public string Prefix
        {
            get { return _prefix; }
            set
            {
                if (value == _prefix) return;
                _prefix = value;
                RaisePropertyChanged();
            }
        }

        public string Suffix
        {
            get { return _suffix; }
            set
            {
                if (value == _suffix) return;
                _suffix = value;
                RaisePropertyChanged();
            }
        }
    }
}