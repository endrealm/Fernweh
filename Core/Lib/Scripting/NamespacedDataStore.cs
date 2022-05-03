using System.Collections.Generic;
using Core.Scripting.Saving;

namespace Core.Scripting;

public abstract class NamespacedDataStore
{
    public NamespacedKey Key { get; }
    private Dictionary<string, IDataType> _data = new();
    protected IDataSaveFunction _saveFunction;
    protected IDataLoadFunction _loadFunction;

    public NamespacedDataStore(NamespacedKey key)
    {
        Key = key;
    }

    public VariableWrapper CreateVar(string key, object defaultValue = null)
    {
        var variable = new VariableWrapper(key, defaultValue);
        _data.Add(key, variable);
        variable.Set(defaultValue);
        return variable;
    }

    public abstract VariableWrapper CreateStoredVar(string key, object defaultValue = null);
    
    public IDataType GetData(string key)
    {
        return _data.TryGetValue(key, out var value) ? value : null;
    }

    public abstract void Save();
    public abstract void Load();

    public void SetDataSaveFunction(IDataSaveFunction saveFunction)
    {
        _saveFunction = saveFunction;
    }

    public void SetDataLoadFunction(IDataLoadFunction loadFunction)
    {
        _loadFunction = loadFunction;
    }

}

public interface IDataType
{
    
}