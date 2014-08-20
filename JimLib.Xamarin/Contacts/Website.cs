using System;
using JimBobBennett.JimLib.Mvvm;

namespace JimBobBennett.JimLib.Xamarin.Contacts
{
    public class Website : NotificationObject, IEquatable<Website>
    {
        private string _address;

        public bool Equals(Website other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Address, other.Address);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Website) obj);
        }

        public override int GetHashCode()
        {
            return (Address != null ? Address.GetHashCode() : 0);
        }

        public static bool operator ==(Website left, Website right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Website left, Website right)
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
    }
}
