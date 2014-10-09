using System.Windows.Input;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class ToolBarButton : BindableObject
    {
        public static readonly BindableProperty ImageSourceProperty =
            BindableProperty.Create<ToolBarButton, ImageSource>(p => p.ImageSource, null);

        [TypeConverter(typeof(ImageSourceConverter))]
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create<ToolBarButton, string>(p => p.Text, string.Empty);

        public static readonly BindableProperty SystemButtonItemProperty =
            BindableProperty.Create<ToolBarButton, SystemButtonItem>(p => p.SystemButtonItem,
                SystemButtonItem.None);
        
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create<ToolBarButton, ICommand>(p => p.Command, null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create<ToolBarButton, object>(p => p.CommandParameter, null);

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

        public SystemButtonItem SystemButtonItem
        {
            get { return (SystemButtonItem)GetValue(SystemButtonItemProperty); }
            set { SetValue(SystemButtonItemProperty, value); }
        }
    }
}