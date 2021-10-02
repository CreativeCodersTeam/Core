using System;
using System.Windows;
using System.Windows.Input;
using CreativeCoders.Mvvm.Commands;
using JetBrains.Annotations;
using Microsoft.Xaml.Behaviors;

namespace CreativeCoders.Mvvm.Wpf.Commands
{
    [PublicAPI]
    public class EventToCommand : TriggerAction<DependencyObject>
    {
        public const string EventArgsConverterParameterPropertyName = "EventArgsConverterParameter";

        public const string AlwaysInvokeCommandPropertyName = "AlwaysInvokeCommand";

        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(EventToCommand),
                new PropertyMetadata(null, (s, _) =>
                {
                    var sender = s as EventToCommand;

                    if (sender?.AssociatedObject == null)
                    {
                        return;
                    }

                    sender.EnableDisableElement();
                }));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command",
            typeof(ICommand), typeof(EventToCommand),
            new PropertyMetadata(null, (s, e) => OnCommandChanged(s as EventToCommand, e)));

        public static readonly DependencyProperty MustToggleIsEnabledProperty =
            DependencyProperty.Register("MustToggleIsEnabled", typeof(bool), typeof(EventToCommand),
                new PropertyMetadata(false, (s, _) =>
                {
                    var sender = s as EventToCommand;

                    if (sender?.AssociatedObject == null)
                    {
                        return;
                    }

                    sender.EnableDisableElement();
                }));

        public static readonly DependencyProperty EventArgsConverterParameterProperty =
            DependencyProperty.Register(EventArgsConverterParameterPropertyName, typeof(object), typeof(EventToCommand),
                new PropertyMetadata(null));

        public static readonly DependencyProperty AlwaysInvokeCommandProperty =
            DependencyProperty.Register(AlwaysInvokeCommandPropertyName, typeof(bool), typeof(EventToCommand),
                new PropertyMetadata(false));

        private object _commandParameterValue;

        private bool? _mustToggleValue;

        public ICommand Command
        {
            get => (ICommand) GetValue(CommandProperty);

            set => SetValue(CommandProperty, value);
        }

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);

            set => SetValue(CommandParameterProperty, value);
        }

        public object CommandParameterValue
        {
            get => _commandParameterValue ?? CommandParameter;

            set
            {
                _commandParameterValue = value;
                EnableDisableElement();
            }
        }

        public bool MustToggleIsEnabled
        {
            get => (bool) GetValue(MustToggleIsEnabledProperty);

            set => SetValue(MustToggleIsEnabledProperty, value);
        }

        public bool MustToggleIsEnabledValue
        {
            get => _mustToggleValue ?? MustToggleIsEnabled;

            set
            {
                _mustToggleValue = value;
                EnableDisableElement();
            }
        }

        public bool PassEventArgsToCommand { get; set; }

        public IEventArgsConverter EventArgsConverter { get; set; }

        public object EventArgsConverterParameter
        {
            get => GetValue(EventArgsConverterParameterProperty);
            set => SetValue(EventArgsConverterParameterProperty, value);
        }

        public bool AlwaysInvokeCommand
        {
            get => (bool) GetValue(AlwaysInvokeCommandProperty);
            set => SetValue(AlwaysInvokeCommandProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            EnableDisableElement();
        }

        private FrameworkElement GetAssociatedObject()
        {
            return AssociatedObject as FrameworkElement;
        }

        public void Invoke()
        {
            Invoke(null);
        }

        protected override void Invoke(object parameter)
        {
            if (AssociatedElementIsDisabled() && !AlwaysInvokeCommand)
            {
                return;
            }

            var command = GetCommand();
            var commandParameter = CommandParameterValue;

            if (commandParameter == null && PassEventArgsToCommand)
            {
                commandParameter = EventArgsConverter == null
                    ? parameter
                    : EventArgsConverter.Convert(parameter, EventArgsConverterParameter);
            }

            if (command != null && command.CanExecute(commandParameter))
            {
                command.Execute(commandParameter);
            }
        }

        private static void OnCommandChanged(EventToCommand element, DependencyPropertyChangedEventArgs e)
        {
            if (element == null)
            {
                return;
            }

            if (e.OldValue != null)
            {
                ((ICommand) e.OldValue).CanExecuteChanged -= element.OnCommandCanExecuteChanged;
            }

            var command = (ICommand) e.NewValue;

            if (command != null)
            {
                command.CanExecuteChanged += element.OnCommandCanExecuteChanged;
            }

            element.EnableDisableElement();
        }

        private bool AssociatedElementIsDisabled()
        {
            var element = GetAssociatedObject();

            return AssociatedObject == null || element is { IsEnabled: false };
        }

        private ICommand GetCommand()
        {
            return Command;
        }

        private void EnableDisableElement()
        {
            var element = GetAssociatedObject();

            if (element == null)
            {
                return;
            }

            var command = GetCommand();

            if (MustToggleIsEnabledValue && command != null)
            {
                element.IsEnabled = command.CanExecute(CommandParameterValue);
            }
        }

        private void OnCommandCanExecuteChanged(object sender, EventArgs e)
        {
            EnableDisableElement();
        }
    }
}
