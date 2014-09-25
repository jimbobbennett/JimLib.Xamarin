using System;
using System.Windows.Input;
using JimBobBennett.JimLib.Commands;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class ExtendedImage : Image
    {
        private TapGestureRecognizer _tapGestureRecognizer;

        private void CreateOrRemoveGestureRecognizer()
        {
            if (!IsSharable && Command == null)
            {
                if (_tapGestureRecognizer == null) return;

                GestureRecognizers.Remove(_tapGestureRecognizer);
                _tapGestureRecognizer = null;
            }
            else
            {
                if (_tapGestureRecognizer != null) return;

                _tapGestureRecognizer = new TapGestureRecognizer
                {
                    Command = new RelayCommand(p =>
                    {
                        if (Command != null)
                            Command.Execute(CommandParameter ?? p);

                        OnClicked();
                    }, p => Command == null || Command.CanExecute(CommandParameter ?? p))
                };

                GestureRecognizers.Add(_tapGestureRecognizer);
            }
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(ExtendedImage), null,
            propertyChanged: CommandPropertyChanged);

        private static void CommandPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var command = oldvalue as ICommand;
            if (command != null)
                command.CanExecuteChanged -= ((ExtendedImage) bindable).CommandOnCanExecuteChanged;

            command = newvalue as ICommand;
            if (command != null)
                command.CanExecuteChanged += ((ExtendedImage) bindable).CommandOnCanExecuteChanged;

            ((ExtendedImage) bindable).CreateOrRemoveGestureRecognizer();
        }

        private void CommandOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            ((RelayCommand)_tapGestureRecognizer.Command).RaiseCanExecuteChanged();
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(ExtendedImage), null,
             propertyChanged: CommandParameterPropertyChanged);
        
        private static void CommandParameterPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ((RelayCommand)((ExtendedImage)bindable)._tapGestureRecognizer.Command).RaiseCanExecuteChanged();
        }

        public static readonly BindableProperty TintColorProperty =
            BindableProperty.Create("TintColor", typeof(Color), typeof(ExtendedImage), Color.Default);

        public static readonly BindableProperty IsSharableProperty =
            BindableProperty.Create("IsSharable", typeof(bool), typeof(ExtendedImage), false,
            propertyChanged: IsSharablePropertyChanged);

        private static void IsSharablePropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            ((ExtendedImage)bindable).CreateOrRemoveGestureRecognizer();
        }

        public static readonly BindableProperty ShareTextProperty =
            BindableProperty.Create("ShareText", typeof(string), typeof(ExtendedImage), string.Empty);

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

        public Color TintColor
        {
            get { return (Color)GetValue(TintColorProperty); }
            set { SetValue(TintColorProperty, value); }
        }

        public bool IsSharable
        {
            get { return (bool)GetValue(IsSharableProperty); }
            set { SetValue(IsSharableProperty, value); }
        }

        public string ShareText
        {
            get { return (string)GetValue(ShareTextProperty); }
            set { SetValue(ShareTextProperty, value); }
        }

        public event EventHandler Clicked;

        private void OnClicked()
        {
            var handler = Clicked;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
