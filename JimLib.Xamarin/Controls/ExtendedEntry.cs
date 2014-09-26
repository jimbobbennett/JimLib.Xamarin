using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class EntryAccessoryButton : BindableObject
    {
        public static readonly BindableProperty TextProperty =
           BindableProperty.Create("Text", typeof(string), typeof(EntryAccessoryButton), string.Empty);

        public static readonly BindableProperty EntryAccessoryButtonItemProperty =
           BindableProperty.Create("EntryAccessoryButtonItem", typeof(EntryAccessoryButtonItem), typeof(EntryAccessoryButton), 
           EntryAccessoryButtonItem.None);

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(EntryAccessoryButton), null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(EntryAccessoryButton), null);

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

    public class ExtendedEntry : Entry
    {
        public ExtendedEntry()
        {
            SetValue(AccessoryButtonsProperty, new List<EntryAccessoryButton>());

            Completed += (s, e) =>
                {
                    if (Command != null && Command.CanExecute(CommandParameter))
                        Command.Execute(CommandParameter);
                };
        }

        public static readonly BindableProperty FontProperty =
            BindableProperty.Create("Font", typeof(Font), typeof(ExtendedEntry), new Font());

        public static readonly BindableProperty XAlignProperty =
            BindableProperty.Create("XAlign", typeof(TextAlignment), typeof(ExtendedEntry),
            TextAlignment.Start);

        public static readonly BindableProperty HasBorderProperty =
            BindableProperty.Create("HasBorder", typeof(bool), typeof(ExtendedEntry), true);

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(ExtendedEntry), null);

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(ExtendedEntry), null);

        public static readonly BindableProperty AccessoryButtonsProperty =
            BindableProperty.Create("AccessoryButtons", typeof(List<EntryAccessoryButton>), typeof(ExtendedEntry), null,
            propertyChanging: AccessoryButtonsPropertyChanging);

        private static void AccessoryButtonsPropertyChanging(BindableObject bindable, object oldvalue, 
            object newvalue)
        {
            var oldList = oldvalue as List<EntryAccessoryButton>;

            if (oldList != null)
            {
                foreach (var button in oldList)
                    button.BindingContext = null;
            }

            var newList = newvalue as List<EntryAccessoryButton>;

            if (newList != null)
            {
                var entry = (ExtendedEntry) bindable;
                foreach (var button in newList)
                    button.BindingContext = entry.BindingContext;
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (AccessoryButtons == null) return;

            foreach (var button in AccessoryButtons)
                button.BindingContext = BindingContext;
        }

        public Font Font
        {
            get { return (Font)GetValue(FontProperty); }
            set { SetValue(FontProperty, value); }
        }

        public TextAlignment XAlign
        {
            get { return (TextAlignment)GetValue(XAlignProperty); }
            set { SetValue(XAlignProperty, value); }
        }

        public bool HasBorder
        {
            get { return (bool)GetValue(HasBorderProperty); }
            set { SetValue(HasBorderProperty, value); }
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

        public List<EntryAccessoryButton> AccessoryButtons
        {
            get { return (List<EntryAccessoryButton>)GetValue(AccessoryButtonsProperty); }
            set { SetValue(AccessoryButtonsProperty, value); }
        }
    }
}
