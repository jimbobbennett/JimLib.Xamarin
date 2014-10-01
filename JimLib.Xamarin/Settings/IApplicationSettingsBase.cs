using System;
using JimBobBennett.JimLib.Events;

namespace JimBobBennett.JimLib.Xamarin.Settings
{
    public interface IApplicationSettingsBase
    {
        event EventHandler<EventArgs<string>> SettingChanged;
    }
}