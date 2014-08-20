using System;
using JimBobBennett.JimLib.Mvvm;

namespace JimBobBennett.JimLib.Xamarin.Contacts
{
    public class Email : NotificationObject, IEquatable<Email>
    {
        private string _address;
        private string _label;
        private AddressType _addressType;

        public bool Equals(Email other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Address, other.Address) && 
                string.Equals(Label, other.Label) && 
                AddressType == other.AddressType;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Email) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Address != null ? Address.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Label != null ? Label.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) AddressType;
                return hashCode;
            }
        }

        public static bool operator ==(Email left, Email right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Email left, Email right)
        {
            return !Equals(left, right);
        }

        public string Address
        {
            get { return _address; }
            set
            {
                if (value == _address) return;
                _address = value;
                RaisePropertyChanged();
            }
        }

        public string Label
        {
            get { return _label; }
            set
            {
                if (value == _label) return;
                _label = value;
                RaisePropertyChanged();
            }
        }

        public AddressType AddressType
        {
            get { return _addressType; }
            set
            {
                if (value == _addressType) return;
                _addressType = value;
                RaisePropertyChanged();
            }
        }
    }
}
