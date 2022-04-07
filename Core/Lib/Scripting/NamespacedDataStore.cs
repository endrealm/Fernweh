using Core.States.ScriptApi;

namespace Core.Scripting;

public class NamespacedDataStore
{
    public NamespacedKey Key { get; }
    
    public NamespacedDataStore(NamespacedKey key)
    {
        Key = key;
    }

}