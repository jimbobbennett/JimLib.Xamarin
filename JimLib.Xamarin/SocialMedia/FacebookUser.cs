namespace JimBobBennett.JimLib.Xamarin.SocialMedia
{
    public class FacebookUser : BaseUser
    {
        private string _userId;

        public string UserId
        {
            get { return _userId; }
            set
            {
                if (_userId == value) return;
                _userId = value;
                RaisePropertyChanged();
            }
        }

        public override string Type
        {
            get { return "Facebook"; }
        }

        public override string DisplayName
        {
            get { return Name; }
        }
    }
}
