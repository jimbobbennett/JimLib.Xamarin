using System.Windows.Input;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class EntryAccessoryButton : BindableObject
    {
        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create<EntryAccessoryButton, ImageSource>(p => p.ImageSource, null);

        [TypeConverter(typeof(ImageSourceConverter))]
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create<EntryAccessoryButton, string>(p => p.Text, string.Empty);

        public static readonly BindableProperty EntryAccessoryButtonItemProperty =
            BindableProperty.Create<EntryAccessoryButton, EntryAccessoryButtonItem>(p => p.EntryAccessoryButtonItem, 
                EntryAccessoryButtonItem.None);
        
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create<EntryAccessoryButton, ICommand>(p => p.Command, null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create<EntryAccessoryButton, object>(p => p.CommandParameter, null);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        public EntryAccessoryButtonItem EntryAccessoryButtonItem
        {
            get { return (EntryAccessoryButtonItem)GetValue(EntryAccessoryButtonItemProperty); }
            set { SetValue(EntryAccessoryButtonItemProperty, value); }
        }
    }
}