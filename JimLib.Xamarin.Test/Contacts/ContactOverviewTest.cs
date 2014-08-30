using FluentAssertions;
using JimBobBennett.JimLib.Xamarin.Contacts;
using JimBobBennett.JimLib.Xamarin.SocialMedia;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JimLib.Xamarin.Test.Contacts
{
    [TestFixture]
    public class ContactOverviewTest
    {
        [Test]
        public void SerializeToAndFromJsonWorks()
        {
            var co = new ContactOverview
            {
                DisplayName = "DisplayName",
                FirstName = "FirstName",
                MiddleName = "MiddleName",
                LastName = "LastName",
                NickName = "NickName",
                Prefix = "Prefix",
                Suffix = "Suffix",
                ThumbBase64 = "ThumbBase64",
                Organization = "Organization"
            };

            co.Addresses.Add(new Address
            {
                City = "City",
                Country = "Country",
                Label = "Label",
                PostalCode = "PostalCode",
                Region = "Region",
                StreetAddress = "StreetAddress",
                Type = AddressType.Other
            });

            co.SocialMediaUsers.Add(new Account
            {
                Name = "Name",
                UserId = "UserId"
            });

            co.SocialMediaUsers.Add(new Account
            {
                Name = "Name",
                UserId = "Handle"
            });

            var settings = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All};
            var serializeObject = JsonConvert.SerializeObject(co, settings);
            var newCo = JsonConvert.DeserializeObject<ContactOverview>(serializeObject, settings);

            newCo.Should().NotBe(co);

            newCo.DisplayName.Should().Be("DisplayName");
            newCo.FirstName.Should().Be("FirstName");
            newCo.MiddleName.Should().Be("MiddleName");
            newCo.LastName.Should().Be("LastName");
            newCo.NickName.Should().Be("NickName");
            newCo.Prefix.Should().Be("Prefix");
            newCo.Suffix.Should().Be("Suffix");
            newCo.ThumbBase64.Should().Be("ThumbBase64");
            newCo.Organization.Should().Be("Organization");

            newCo.Addresses.Should().OnlyContain(a =>
                a.City == "City" &&
                a.Country == "Country" &&
                a.Label == "Label" &&
                a.PostalCode == "PostalCode" &&
                a.Region == "Region" &&
                a.StreetAddress == "StreetAddress" &&
                a.Type == AddressType.Other);

            newCo.SocialMediaUsers.Should().Contain(b => b.Name == "Name" &&
                                                         b.UserId == "UserId");

            newCo.SocialMediaUsers.Should().Contain(b => b.Name == "Name" &&
                                                         b.UserId == "Handle");
        }
    }
}
