using System;
using JimBobBennett.JimLib.Mvvm;

namespace JimBobBennett.JimLib.Xamarin.Contacts
{
    public class InstantMessagingAccount : NotificationObject, IEquatable<InstantMessagingAccount>
    {
        private InstantMessagingService _service;
        private string _serviceLabel;
        private string _account;

        public bool Equals(InstantMessagingAccount other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Service == other.Service && 
                string.Equals(ServiceLabel, other.ServiceLabel) && 
                string.Equals(Account, other.Account);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((InstantMessagingAccount) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Service;
                hashCode = (hashCode*397) ^ (ServiceLabel != null ? ServiceLabel.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Account != null ? Account.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(InstantMessagingAccount left, InstantMessagingAccount right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(InstantMessagingAccount left, InstantMessagingAccount right)
        {
            return !Equals(left, right);
        }

        public InstantMessagingService Service
        {
            get { return _service; }
            set
            {
                if (value == _service) return;
                _service = value;
                RaisePropertyChanged();
            }
        }

        public string ServiceLabel
        {
            get { return _serviceLabel; }
            set
            {
                if (value == _serviceLabel) return;
                _serviceLabel = value;
                RaisePropertyChanged();
            }
        }

        public string Account
        {
            get { return _account; }
            set
            {
                if (value == _account) return;
                _account = value;
                RaisePropertyChanged();
            }
        }
    }
}
