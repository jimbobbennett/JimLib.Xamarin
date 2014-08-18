using System;
using Refractored.Xam.Settings.Abstractions;

namespace JimBobBennett.JimLib.Xamarin.Settings
{
    public class ApplicationSettingsBase
    {
        private readonly ISettings _settings;

        protected ApplicationSettingsBase(ISettings settings)
        {
            _settings = settings;
        }
        
        protected string GetSetting(string key, string defaultValue = default(string))
        {
            return _settings.GetValueOrDefault(key, defaultValue);
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
        }

        protected void SetSetting(string key, bool value)
        {
            _settings.AddOrUpdateValue(key, value);
        }

        protected void SetEnumSetting<T>(string key, T value) where T : struct
        {
            _settings.AddOrUpdateValue(key, value.ToString());
        }
    }
}
