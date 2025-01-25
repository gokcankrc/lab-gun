#if DEBUG
using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class DebugSetting
{
    [NonSerialized] public string keyRaw;

    [ShowInInspector, ToggleLeft]
    public bool Value { get => PlayerPrefs.GetInt(Key, 0) == 1; set => PlayerPrefs.SetInt(Key, value ? 1 : 0); }

    public string Key => "debug " + keyRaw;

    public string KeyDisplay
    {
        get
        {
            string s = System.Text.RegularExpressions.Regex.Replace(
                keyRaw,
                "(\\B[A-Z])",
                " $1"
            );

            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }

    public DebugSetting(string key)
    {
        keyRaw = key;
    }

    public static implicit operator bool(DebugSetting setting)
    {
        return setting.Value;
    }

    public void Toggle()
    {
        Value = !Value;
    }
}
#endif