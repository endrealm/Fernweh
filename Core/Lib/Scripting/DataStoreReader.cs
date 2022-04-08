using System;
using NLua;

namespace Core.Scripting;

public class DataStoreReader
{
    private readonly NamespacedDataStore _dataStore;

    public IDataType Get(string key)
    {
        return _dataStore.GetData(key);
    }
    public LuaFunction GetFunc(string key)
    {
        if (_dataStore.GetData(key) is not VariableWrapper wrapper)
        {
            throw new Exception("Not a function");
        }
        
        if (wrapper.Get() is not LuaFunction function)
        {
            throw new Exception("Not a function");
        }
        
        return function;
    }
    
    public DataStoreReader(NamespacedDataStore dataStore)
    {
        _dataStore = dataStore;
    }
}