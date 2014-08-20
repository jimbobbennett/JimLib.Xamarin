using System;
using JimBobBennett.JimLib.Mvvm;

namespace JimBobBennett.JimLib.Xamarin.Contacts
{
    public class Address : NotificationObject, IEquatable<Address>
    {
        private AddressType _type;
        private string _postalCode;
        private string _country;
        private string _region;
        private string _city;
        private string _streetAddress;
        private string _label;

        public bool Equals(Address other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Type == other.Type && 
                string.Equals(Label, other.Label) && 
                string.Equals(StreetAddress, other.StreetAddress) && 
                string.Equals(City, other.City) && 
                string.Equals(Region, other.Region) && 
                string.Equals(Country, other.Country) && 
                string.Equals(PostalCode, other.PostalCode);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Address) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Type;
                hashCode = (hashCode*397) ^ (Label != null ? Label.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (StreetAddress != null ? StreetAddress.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (City != null ? City.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Region != null ? Region.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Country != null ? Country.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (PostalCode != null ? PostalCode.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Address left, Address right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Address left, Address right)
        {
            return !Equals(left, right);
        }

        public AddressType Type
        {
            get { return _type; }
            set
            {
                if (value == _type) return;
                _type = value;
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

        public string StreetAddress
        {
            get { return _streetAddress; }
            set
            {
                if (value == _streetAddress) return;
                _streetAddress = value;
                RaisePropertyChanged();
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                if (value == _city) return;
                _city = value;
                RaisePropertyChanged();
            }
        }

        public string Region
        {
            get { return _region; }
            set
            {
                if (value == _region) return;
                _region = value;
                RaisePropertyChanged();
            }
        }

        public string Country
        {
            get { return _country; }
            set
            {
                if (value == _country) return;
                _country = value;
                RaisePropertyChanged();
            }
        }

        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                if (value == _postalCode) return;
                _postalCode = value;
                RaisePropertyChanged();
            }
        }
    }
}
