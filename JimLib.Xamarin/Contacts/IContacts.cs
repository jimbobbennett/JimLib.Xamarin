using System.Collections.Generic;
using System.Threading.Tasks;

namespace JimBobBennett.JimLib.Xamarin.Contacts
{
    public interface IContacts
    {
        Task<bool> AuthoriseContacts();
        Task<IEnumerable<ContactOverview>> GetContactOverviewsAsync();

        string Serialize(ContactOverview contact);
        ContactOverview Deserialize(string contact);
    }
}
