using Core.Scripting.Saving;
using NLua;

namespace Core.Scripting;

public class DataStoreWriter
{
    private readonly NamespacedDataStore _dataStore;

    public DataStoreWriter(NamespacedDataStore dataStore)
    {
        _dataStore = dataStore;
    }

    public VariableWrapper CreateVar(string key, object defaultValue = null)
    {
        return _dataStore.CreateVar(key, defaultValue);
    }

    public VariableWrapper CreateStoredVar(string key, object defaultValue = null)
    {
        return _dataStore.CreateStoredVar(key, defaultValue);
    }

    public VariableWrapper CreateFunc(string key, LuaFunction defaultValue)
    {
        return _dataStore.CreateVar(key, defaultValue);
    }
}