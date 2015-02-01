using JimBobBennett.JimLib.Xamarin.Controls;
using UIKit;

namespace JimBobBennett.JimLib.Xamarin.ios.Extensions
{
    public static class SystemButtonItemExtensions
    {
        public static UIBarButtonSystemItem ConvertToSystemItem(this SystemButtonItem item)
        {
            switch (item)
            {
                case SystemButtonItem.Cancel:
                    return UIBarButtonSystemItem.Cancel;
                case SystemButtonItem.Edit:
                    return UIBarButtonSystemItem.Edit;
                case SystemButtonItem.Save:
                    return UIBarButtonSystemItem.Save;
                case SystemButtonItem.Add:
                    return UIBarButtonSystemItem.Add;
                case SystemButtonItem.FlexibleSpace:
                    return UIBarButtonSystemItem.FlexibleSpace;
                case SystemButtonItem.FixedSpace:
                    return UIBarButtonSystemItem.FixedSpace;
                case SystemButtonItem.Compose:
                    return UIBarButtonSystemItem.Compose;
                case SystemButtonItem.Reply:
                    return UIBarButtonSystemItem.Reply;
                case SystemButtonItem.Action:
                    return UIBarButtonSystemItem.Action;
                case SystemButtonItem.Organize:
                    return UIBarButtonSystemItem.Organize;
                case SystemButtonItem.Bookmarks:
                    return UIBarButtonSystemItem.Bookmarks;
                case SystemButtonItem.Search:
                    return UIBarButtonSystemItem.Search;
                case SystemButtonItem.Refresh:
                    return UIBarButtonSystemItem.Refresh;
                case SystemButtonItem.Stop:
                    return UIBarButtonSystemItem.Stop;
                case SystemButtonItem.Camera:
                    return UIBarButtonSystemItem.Camera;
                case SystemButtonItem.Trash:
                    return UIBarButtonSystemItem.Trash;
                case SystemButtonItem.Play:
                    return UIBarButtonSystemItem.Play;
                case SystemButtonItem.Pause:
                    return UIBarButtonSystemItem.Pause;
                case SystemButtonItem.Rewind:
                    return UIBarButtonSystemItem.Rewind;
                case SystemButtonItem.FastForward:
                    return UIBarButtonSystemItem.FastForward;
                case SystemButtonItem.Undo:
                    return UIBarButtonSystemItem.Undo;
                case SystemButtonItem.Redo:
                    return UIBarButtonSystemItem.Redo;
                case SystemButtonItem.PageCurl:
                    return UIBarButtonSystemItem.PageCurl;
                default:
                    return UIBarButtonSystemItem.Done;
            }
        }
    }
}
