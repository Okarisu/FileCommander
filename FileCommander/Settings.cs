namespace FileCommander;

using System.Configuration;
using System.Collections.Specialized;

public class Settings
{
    public static bool GetBoolValueSetting(string key)
    {
        var value = ConfigurationManager.AppSettings.Get(key)!;

        return value switch
        {
            "true" => true,
            "false" => false,
            _ => false
        };
    }

    public static void SetBoolValueSetting(string key, bool value)
    {
        var strValue = value ? "true" : "false";
        ConfigurationManager.AppSettings.Set(key, strValue);
    }
}