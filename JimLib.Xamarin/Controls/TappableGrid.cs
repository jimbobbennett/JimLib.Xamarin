using System;
using System.Windows.Input;
using JimBobBennett.JimLib.Commands;
using Xamarin.Forms;

namespace JimBobBennett.JimLib.Xamarin.Controls
{
    public class TappableGrid : Grid
    {
        private TapGestureRecognizer _tapGestureRecognizer;

        private void CreateOrRemoveGestureRecognizer()
        {
            if (Command == null)
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
                    }, p => Command == null || Command.CanExecute(CommandParameter ?? p))
                };

                GestureRecognizers.Add(_tapGestureRecognizer);
            }
        }

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create("Command", typeof(ICommand), typeof(TappableGrid), null,
            propertyChanged: CommandPropertyChanged);

        private static void CommandPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var command = oldvalue as ICommand;
            if (command != null)
                command.CanExecuteChanged -= ((TappableGrid)bindable).CommandOnCanExecuteChanged;

            command = newvalue as ICommand;
            if (command != null)
                command.CanExecuteChanged += ((TappableGrid)bindable).CommandOnCanExecuteChanged;

            ((TappableGrid)bindable).CreateOrRemoveGestureRecognizer();
        }

        private void CommandOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            if (_tapGestureRecognizer != null && _tapGestureRecognizer.Command != null)
                ((RelayCommand)_tapGestureRecognizer.Command).RaiseCanExecuteChanged();
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create("CommandParameter", typeof(object), typeof(TappableGrid), null,
             propertyChanged: CommandParameterPropertyChanged);

        private static void CommandParameterPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var gesture = ((TappableGrid)bindable)._tapGestureRecognizer;

            if (gesture != null && gesture.Command != null)
                ((RelayCommand)gesture.Command).RaiseCanExecuteChanged();
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
    }
}
