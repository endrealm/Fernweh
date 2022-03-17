using System.Collections.Generic;

namespace Core.Utils;

public class PropsArray
{
    private readonly Dictionary<string, object> _data;

    public PropsArray(Dictionary<string, object> data)
    {
        _data = data;
    }

    public object SafeGet(string key)
    {
        return _data.TryGetValue(key, out var data) ? data : null;
    }
}