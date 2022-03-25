using System;
using System.Collections.Generic;

namespace Core.Scenes.Ingame;

public readonly struct ModeParameters
{
    private readonly Dictionary<string, object> _data = new();


    public ModeParameters AppendData(string key, object value)
    {
        _data.Add(key, value);
        return this;
    }
    
    public bool HasKey(string key)
    {
        return _data.ContainsKey(key);
    }

    public T GetValue<T>(string key)
    {
        return (T) _data[key];
    }
    
    public T GetValueOrDefault<T>(string key, T defaultValue)
    {
        return GetValueOrDefault(key, () => defaultValue);
    }
    public T GetValueOrDefault<T>(string key, Func<T> defaultValue)
    {
        if(_data.ContainsKey(key)) return (T) _data[key];
        return defaultValue.Invoke();
    }
    
}