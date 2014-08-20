using System;
using JimBobBennett.JimLib.Mvvm;

namespace JimBobBennett.JimLib.Xamarin.Contacts
{
    public class Organization : NotificationObject, IEquatable<Organization>
    {
        private string _contactTitle;
        private string _name;
        private string _label;
        private OrganizationType _type;

        public bool Equals(Organization other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Type == other.Type && 
                string.Equals(Label, other.Label) && 
                string.Equals(Name, other.Name) && 
                string.Equals(ContactTitle, other.ContactTitle);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Organization) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Type;
                hashCode = (hashCode*397) ^ (Label != null ? Label.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ContactTitle != null ? ContactTitle.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(Organization left, Organization right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Organization left, Organization right)
        {
            return !Equals(left, right);
        }

        public OrganizationType Type
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

        public string Name
        {
            get { return _name; }
            set
            {
                if (value == _name) return;
                _name = value;
                RaisePropertyChanged();
            }
        }

        public string ContactTitle
        {
            get { return _contactTitle; }
            set
            {
                if (value == _contactTitle) return;
                _contactTitle = value;
                RaisePropertyChanged();
            }
        }
    }
}
