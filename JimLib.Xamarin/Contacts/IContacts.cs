using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JimBobBennett.JimLib.Xamarin.Contacts
{
    public interface IContacts
    {
        Task<bool> AuthoriseContactsAsync();
        Task<IEnumerable<ContactOverview>> GetContactOverviewsAsync();

        string Serialize(ContactOverview contact);
        ContactOverview Deserialize(string contact);
        AuthorizationStatus AuthorizationStatus { get; }

        void AddContact(ContactOverview contactOverview);

        event EventHandler AuthorizationStatusChanged;

        void MakePhoneCall(Phone phone);
        void SendEmail(Email email);
    }
}
