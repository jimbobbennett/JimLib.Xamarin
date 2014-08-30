using System;
using JimBobBennett.JimLib.Mvvm;

namespace JimBobBennett.JimLib.Xamarin.SocialMedia
{
    public class Account : NotificationObject, IEquatable<Account>
    {
        private string _name;
        private string _type;
        private string _displayName;
        private string _userId;
        private bool _canEditUserId;

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

        public string Type
        {
            get { return _type; }
            set
            {
                if (_type == value) return;

                _type = value;
                RaisePropertyChanged();
            }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                if (_displayName == value) return;

                _displayName = value;
                RaisePropertyChanged();
            }
        }

        public string UserId
        {
            get { return _userId; }
            set
            {
                if (_userId == value) return;
                _userId = value;
                RaisePropertyChanged();
            }
        }

        public bool CanEditUserId
        {
            get { return _canEditUserId; }
            set
            {
                if (_canEditUserId == value) return;

                _canEditUserId = value;
                RaisePropertyChanged();
            }
        }

        public bool Equals(Account other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_name, other._name) && 
                string.Equals(_type, other._type) && 
                string.Equals(_userId, other._userId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Account) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Type != null ? Type.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (UserId != null ? UserId.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Account left, Account right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Account left, Account right)
        {
            return !Equals(left, right);
        }
    }
}