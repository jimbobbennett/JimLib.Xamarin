using System.Collections.ObjectModel;
using JimBobBennett.JimLib.Mvvm;
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

        public ContactOverview()
        {
            Emails = new ObservableCollection<Email>();
            Phones = new ObservableCollection<Phone>();
            Websites = new ObservableCollection<Website>();
            Organizations = new ObservableCollection<Organization>();
            Addresses = new ObservableCollection<Address>();
            InstantMessagingAccounts = new ObservableCollection<InstantMessagingAccount>();
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

        public string ThumbBase64 { get; set; }

        public ObservableCollection<Email> Emails { get; set; }
        public ObservableCollection<Phone> Phones { get; set; }
        public ObservableCollection<Website> Websites { get; set; }
        public ObservableCollection<Organization> Organizations { get; set; }
        public ObservableCollection<Address> Addresses { get; set; }
        public ObservableCollection<InstantMessagingAccount> InstantMessagingAccounts { get; set; }

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