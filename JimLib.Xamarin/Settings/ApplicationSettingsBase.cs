using System;
using JimBobBennett.JimLib.Events;
using JimBobBennett.JimLib.Extensions;
using Org.BouncyCastle.Security;
using Refractored.Xam.Settings.Abstractions;

namespace JimBobBennett.JimLib.Xamarin.Settings
{
    public class ApplicationSettingsBase : IApplicationSettingsBase
    {
        private readonly ISettings _settings;
        private readonly string _password;

        public event EventHandler<EventArgs<string>> SettingChanged
        {
            add { WeakEventManager.GetWeakEventManager(this).AddEventHandler("SettingChanged", value); }
            remove { WeakEventManager.GetWeakEventManager(this).RemoveEventHandler("SettingChanged", value); }
        }

        protected void OnSettingChanged(string settingKey)
        {
            WeakEventManager.GetWeakEventManager(this).RaiseEvent(this, new EventArgs<string>(settingKey), "SettingChanged");
        }

        protected ApplicationSettingsBase(ISettings settings, string password = null)
        {
            _settings = settings;
            _password = password;
        }

        protected string GetSetting(string key, string defaultValue = default(string))
        {
            return _settings.GetValueOrDefault(key, defaultValue);
        }

        protected string GetEncryptedSetting(string key, string defaultValue = default(string))
        {
            if (_password.IsNullOrEmpty())
                throw new PasswordException("Password cannot be null");

            var setting = _settings.GetValueOrDefault(key, defaultValue);

            if (!Equals(setting, defaultValue))
                setting = setting.Decrypt(_password);

            return setting;
        }

        protected T GetEnumSetting<T>(string key, T defaultValue = default(T)) where T : struct
        {
            var setting = _settings.GetValueOrDefault(key, default(string));
            T value;
            return Enum.TryParse(setting, true, out value) ? value : defaultValue;
        }

        protected bool GetBoolSetting(string key, bool defaultValue = default(bool))
        {
            return _settings.GetValueOrDefault(key, defaultValue);
        }

        protected void SetSetting(string key, string value)
        {
            _settings.AddOrUpdateValue(key, value ?? string.Empty);
            _settings.Save();
            OnSettingChanged(key);
        }

        protected void SetEncryptedSetting(string key, string value)
        {
            if (_password.IsNullOrEmpty())
                throw new PasswordException("Password cannot be null");

            if (!value.IsNullOrEmpty())
                value = value.Encrypt(_password);

            SetSetting(key, value ?? string.Empty);
        }

        protected void SetSetting(string key, bool value)
        {
            _settings.AddOrUpdateValue(key, value);
            _settings.Save();
            OnSettingChanged(key);
        }

        protected void SetEnumSetting<T>(string key, T value) where T : struct
        {
            _settings.AddOrUpdateValue(key, value.ToString());
            _settings.Save();
            OnSettingChanged(key);
        }
    }
}
