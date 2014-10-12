namespace JimBobBennett.JimLib.Xamarin.Images
{
    public class ImageOptions
    {
        public ImageOptions(float maxWidth = -1, float maxHeight = -1)
        {
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;
        }

        public float MaxWidth { get; set; }
        public float MaxHeight { get; set; }

        internal bool HasSizeSet { get { return MaxHeight > -1 && MaxWidth > -1; } }
        internal bool FixOrientation { get; set; }
    }
}