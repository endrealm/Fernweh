namespace Core.Scripting;

public class DataStoreReader
{
    private readonly NamespacedDataStore _dataStore;

    public IDataType Get(string key)
    {
        return _dataStore.GetData(key);
    }
    
    public DataStoreReader(NamespacedDataStore dataStore)
    {
        _dataStore = dataStore;
    }
}