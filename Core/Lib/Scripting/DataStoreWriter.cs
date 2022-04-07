namespace Core.Scripting;

public class DataStoreWriter
{
    private readonly NamespacedDataStore _dataStore;

    public VariableWrapper CreateVariable(string key, object defaultValue = null)
    {
        return _dataStore.CreateVar(key, defaultValue);
    }

    public DataStoreWriter(NamespacedDataStore dataStore)
    {
        _dataStore = dataStore;
    }
}