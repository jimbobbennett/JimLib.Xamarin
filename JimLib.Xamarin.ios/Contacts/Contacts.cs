using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JimBobBennett.JimLib.Xamarin.Contacts;
using JimBobBennett.JimLib.Xamarin.ios.Images;
using JimBobBennett.JimLib.Xamarin.ios.Navigation;
using MonoTouch.AddressBook;
using MonoTouch.AddressBookUI;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Newtonsoft.Json;
using Xamarin.Contacts;
using Address = Xamarin.Contacts.Address;
using Email = Xamarin.Contacts.Email;
using InstantMessagingAccount = Xamarin.Contacts.InstantMessagingAccount;
using InstantMessagingService = JimBobBennett.JimLib.Xamarin.Contacts.InstantMessagingService;
using Phone = Xamarin.Contacts.Phone;
using Website = Xamarin.Contacts.Website;

namespace JimBobBennett.JimLib.Xamarin.ios.Contacts
{
    public class Contacts : IContacts
    {
        private readonly INavigation _navigation;
        private AuthorizationStatus _authorizationStatus;

        public Contacts(INavigation navigation)
        {
            _navigation = navigation;
            AuthorizationStatus = GetStatus();
        }

        public async Task<bool> AuthoriseContactsAsync()
        {
            return await GetAddressBookAsync() != null;
        }

        public AuthorizationStatus AuthorizationStatus
        {
            get
            {
                AuthorizationStatus = GetStatus();
                return _authorizationStatus;
            }
            private set
            {
                if (_authorizationStatus == value) return;

                _authorizationStatus = value;

                OnAuthorizationStatusChanged();
            }
        }

        private static AuthorizationStatus GetStatus()
        {
            var status = ABAddressBook.GetAuthorizationStatus();

            switch (status)
            {
                case ABAuthorizationStatus.Authorized:
                    return AuthorizationStatus.Authorized;
                case ABAuthorizationStatus.Restricted:
                    return AuthorizationStatus.Restricted;
                case ABAuthorizationStatus.NotDetermined:
                    return AuthorizationStatus.NotDetermined;
                default:
                    return AuthorizationStatus.Denied;
            }
        }

        public void AddContact(ContactOverview contactOverview)
        {
            var person = new ABPerson
            {
                FirstName = contactOverview.FirstName,
                MiddleName = contactOverview.MiddleName,
                LastName = contactOverview.LastName,
                Image = new NSData(contactOverview.ThumbBase64, NSDataBase64DecodingOptions.None),
                Nickname = contactOverview.NickName,
                Prefix = contactOverview.Prefix,
                Suffix = contactOverview.Suffix,
                Organization = contactOverview.Organization
            };

            AddPhones(contactOverview, person);
            AddEmails(contactOverview, person);
            AddAddresses(contactOverview, person);

            var vc = new ABUnknownPersonViewController {DisplayedPerson = person};
            var rootController = _navigation.NavigationController.VisibleViewController;
            var nc = new UINavigationController(vc);

            vc.NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Done",
                UIBarButtonItemStyle.Plain,
                (s, e) => nc.DismissViewController(true, null));
            
            rootController.PresentViewController(nc, true, null);
        }

        public event EventHandler AuthorizationStatusChanged;

        public void OnAuthorizationStatusChanged()
        {
            var handler = AuthorizationStatusChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        private static void AddAddresses(ContactOverview contactOverview, ABPerson person)
        {
            var addresses = new ABMutableDictionaryMultiValue();

            foreach (var address in contactOverview.Addresses)
            {
                var a = new NSMutableDictionary
                {
                    {new NSString(ABPersonAddressKey.City), new NSString(address.City)}, 
                    {new NSString(ABPersonAddressKey.Country), new NSString(address.Country)},
                    {new NSString(ABPersonAddressKey.Zip), new NSString(address.PostalCode)}, 
                    {new NSString(ABPersonAddressKey.State), new NSString(address.Region)},
                    {new NSString(ABPersonAddressKey.Street), new NSString(address.StreetAddress)}
                };

                addresses.Add(a, new NSString(address.Label));
            }

            person.SetAddresses(addresses);
        }

        private static void AddPhones(ContactOverview contactOverview, ABPerson person)
        {
            ABMutableMultiValue<string> phones = new ABMutableStringMultiValue();
            foreach (var phone in contactOverview.Phones)
                phones.Add(phone.Number, new NSString(phone.Label));

            person.SetPhones(phones);
        }

        private static void AddEmails(ContactOverview contactOverview, ABPerson person)
        {
            ABMutableMultiValue<string> emails = new ABMutableStringMultiValue();
            foreach (var email in contactOverview.Emails)
                emails.Add(email.Address, new NSString(email.Label));

            person.SetEmails(emails);
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
            var scaled = ImageHelper.CropToCircle(ImageHelper.MaxResizeImage(thumb, 128, 128));

            var contactOverview = new ContactOverview
            {
                DisplayName = c.DisplayName, 
                FirstName = c.FirstName, 
                MiddleName = c.MiddleName, 
                LastName = c.LastName, 
                NickName = c.Nickname, 
                Prefix = c.Prefix, 
                Suffix = c.Suffix
            };

            var organization = c.Organizations.FirstOrDefault();
            if (organization != null)
                contactOverview.Organization = organization.Name;

            contactOverview.SetThumbImageSource(ImageHelper.GetImageSourceFromUIImage(scaled));

            if (scaled != null)
            {
                var data = scaled.AsPNG();
                contactOverview.ThumbBase64 = data.GetBase64EncodedString(NSDataBase64EncodingOptions.None);
            }

            contactOverview.Emails.AddRange(c.Emails.Select(CreateEmail));
            contactOverview.Phones.AddRange(c.Phones.Select(CreatePhone));
            contactOverview.Addresses.AddRange(c.Addresses.Select(CreateAddress));
            contactOverview.InstantMessagingAccounts.AddRange(c.InstantMessagingAccounts.Select(CreateInstantMessagingAccount));
            contactOverview.Websites.AddRange(c.Websites.Select(CreateWebsites));
            
            return contactOverview;
        }

        private static Xamarin.Contacts.InstantMessagingAccount CreateInstantMessagingAccount(InstantMessagingAccount arg)
        {
            return new Xamarin.Contacts.InstantMessagingAccount
            {
                Account = arg.Account,
                Service = (InstantMessagingService) arg.Service,
                ServiceLabel = arg.ServiceLabel
            };
        }

        private static Xamarin.Contacts.Address CreateAddress(Address arg)
        {
            return new Xamarin.Contacts.Address
            {
                City = arg.City,
                Label = arg.Label,
                Type = (Xamarin.Contacts.AddressType)arg.Type,
                Country = arg.Country,
                PostalCode = arg.PostalCode,
                Region = arg.Region,
                StreetAddress = arg.StreetAddress
            };
        }

        private static Xamarin.Contacts.Website CreateWebsites(Website arg)
        {
            return new Xamarin.Contacts.Website
            {
                Address = arg.Address
            };
        }

        private static Xamarin.Contacts.Phone CreatePhone(Phone arg)
        {
            return new Xamarin.Contacts.Phone
            {
                Number = arg.Number,
                Type = (Xamarin.Contacts.PhoneType) arg.Type,
                Label = arg.Label
            };
        }

        private static Xamarin.Contacts.Email CreateEmail(Email arg)
        {
            return new Xamarin.Contacts.Email
            {
                Address = arg.Address,
                AddressType = (Xamarin.Contacts.AddressType) arg.Type,
                Label = arg.Label
            };
        }

        private async Task<AddressBook> GetAddressBookAsync()
        {
            var addressBook = new AddressBook();

            var t = addressBook.RequestPermission();

            await t;

            AuthorizationStatus = GetStatus();

            return !t.Result ? null : addressBook;
        }
    }
}