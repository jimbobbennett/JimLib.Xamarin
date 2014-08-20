using FluentAssertions;
using JimBobBennett.JimLib.Xamarin.Contacts;
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
                ThumbBase64 = "ThumbBase64"
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

            var newCo = JsonConvert.DeserializeObject<ContactOverview>(JsonConvert.SerializeObject(co));

            newCo.Should().NotBe(co);

            newCo.DisplayName.Should().Be("DisplayName");
            newCo.FirstName.Should().Be("FirstName");
            newCo.MiddleName.Should().Be("MiddleName");
            newCo.LastName.Should().Be("LastName");
            newCo.NickName.Should().Be("NickName");
            newCo.Prefix.Should().Be("Prefix");
            newCo.Suffix.Should().Be("Suffix");
            newCo.ThumbBase64.Should().Be("ThumbBase64");

            newCo.Addresses.Should().OnlyContain(a =>
                a.City == "City" &&
                a.Country == "Country" &&
                a.Label == "Label" &&
                a.PostalCode == "PostalCode" &&
                a.Region == "Region" &&
                a.StreetAddress == "StreetAddress" &&
                a.Type == AddressType.Other);
        }
    }
}
