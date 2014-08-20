using System;
using JimBobBennett.JimLib.Mvvm;

namespace JimBobBennett.JimLib.Xamarin.Contacts
{
    public class Phone : NotificationObject, IEquatable<Phone>
    {
        private string _number;
        private string _label;
        private PhoneType _type;

        public bool Equals(Phone other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Type == other.Type && 
                string.Equals(Label, other.Label) && 
                string.Equals(Number, other.Number);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Phone) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Type;
                hashCode = (hashCode*397) ^ (Label != null ? Label.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Number != null ? Number.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Phone left, Phone right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Phone left, Phone right)
        {
            return !Equals(left, right);
        }

        public PhoneType Type
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

        public string Number
        {
            get { return _number; }
            set
            {
                if (value == _number) return;
                _number = value;
                RaisePropertyChanged();
            }
        }
    }
}
