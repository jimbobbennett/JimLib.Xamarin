using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Xamarin.Contacts;
using JimBobBennett.JimLib.Xamarin.ios.Images;
using MonoTouch.Foundation;
using Newtonsoft.Json;
using Xamarin.Contacts;
using Email = Xamarin.Contacts.Email;
using Phone = Xamarin.Contacts.Phone;

namespace JimBobBennett.JimLib.Xamarin.ios.Contacts
{
    public class Contacts : IContacts
    {
        public async Task<bool> AuthoriseContacts()
        {
            return await GetAddressBookAsync() != null;
        }

        public async Task<IEnumerable<ContactOverview>> GetContactOverviewsAsync()
        {
            var addressBook = await GetAddressBookAsync();
            return await Task<List<ContactOverview>>.Factory.StartNew(() => GetContactsListAsync(addressBook));
        }

        public string Serialize(ContactOverview contact)
        {
            return JsonConvert.SerializeObject(contact, Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
        }
        
        public ContactOverview Deserialize(string contact)
        {
            return JsonConvert.DeserializeObject<ContactOverview>(contact);
        }

        private static List<ContactOverview> GetContactsListAsync(IEnumerable<Contact> addressBook)
        {
            return addressBook.Select(CreateContact).ToList();
        }

        private static ContactOverview CreateContact(Contact c)
        {
            var thumb = c.GetThumbnail();
            var scaled = ImageHelper.MaxResizeImage(thumb, 128, 128);

            var contactOverview = new ContactOverview(c.DisplayName,
                ImageHelper.GetImageSourceFromUIImage(scaled));

            if (scaled != null)
            {
                var data = scaled.AsPNG();
                contactOverview.ThumbBase64 = data.GetBase64EncodedString(NSDataBase64EncodingOptions.None);
            }

            contactOverview.Emails.AddRange(c.Emails.Select(CreateEmail));
            contactOverview.Phones.AddRange(c.Phones.Select(CreatePhone));

            return contactOverview;
        }

        private static Xamarin.Contacts.Phone CreatePhone(Phone arg)
        {
            return new Xamarin.Contacts.Phone
            {
                Number = arg.Number,
                Type = (Xamarin.Contacts.PhoneType)arg.Type,
                Label = arg.Label
            };
        }

        private static Xamarin.Contacts.Email CreateEmail(Email arg)
        {
            return new Xamarin.Contacts.Email
            {
                Address = arg.Address,
                AddressType = (Xamarin.Contacts.AddressType)arg.Type,
                Label = arg.Label
            };
        }
        
        private static async Task<AddressBook> GetAddressBookAsync()
        {
            var addressBook = new AddressBook();

            var t = addressBook.RequestPermission();

            await t;

            return !t.Result ? null : addressBook;
        }
    }
}