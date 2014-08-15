using System.Collections.Generic;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Contacts
{
    public class ContactOverview    
    {
        private ImageSource _thumbImageSource;

        public ContactOverview(string displayName, ImageSource thumbImageSource)
        {
            DisplayName = displayName;
            _thumbImageSource = thumbImageSource;

            Emails = new List<Email>();
            Phones = new List<Phone>();
        }

        public string DisplayName { get; set; }

        public ImageSource GetThumbImageSource()
        {
            return _thumbImageSource;
        }

        public void SetThumbImageSource(ImageSource imageSource)
        {
           _thumbImageSource = imageSource;
        }

        public string ThumbBase64 { get; set; }

        public List<Email> Emails { get; set; }

        public List<Phone> Phones { get; set; }

    }
}