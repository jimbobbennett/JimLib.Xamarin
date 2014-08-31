using System;
using System.Windows.Input;
using JimBobBennett.JimLib.Commands;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class ExtendedImage : Image
    {
        private readonly TapGestureRecognizer _tapGestureRecognizer;

        public ExtendedImage()
        {
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

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(ExtendedImage), null,
            propertyChanged: CommandPropertyChanged);

        private static void CommandPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var command = oldvalue as ICommand;
            if (command != null)
                command.CanExecuteChanged -= ((ExtendedImage)bindable).CommandOnCanExecuteChanged;

            command = newvalue as ICommand;
            if (command != null)
                command.CanExecuteChanged += ((ExtendedImage)bindable).CommandOnCanExecuteChanged;
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

        public event EventHandler Clicked;

        private void OnClicked()
        {
            var handler = Clicked;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
