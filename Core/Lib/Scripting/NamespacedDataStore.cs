using System.Collections.Generic;
using Core.Saving;
using NLua;

namespace Core.Scripting;

public abstract class NamespacedDataStore
{
    public NamespacedKey Key { get; }
    private Dictionary<string, IDataType> _data = new();

    public NamespacedDataStore(NamespacedKey key)
    {
        Key = key;
    }

    public VariableWrapper CreateVar(string key, object defaultValue = null)
    {
        var variable = new VariableWrapper(defaultValue);
        _data.Add(key, variable);
        variable.Set(defaultValue);
        return variable;
    }

    public abstract VariableWrapper CreateStoredVar(string key, object defaultValue = null);
    
    public IDataType GetData(string key)
    {
        return _data.TryGetValue(key, out var value) ? value : null;
    }
}

public class LuaNamespacedDataStore : NamespacedDataStore
{
    private readonly Lua _lua;
    private readonly IGameSave _gameSave;
    private List<VariableWrapper> _persistentVariables = new();

    public LuaNamespacedDataStore(NamespacedKey key, Lua lua, IGameSave gameSave): base(key)
    {
        _lua = lua;
        _gameSave = gameSave;
    }

    public override VariableWrapper CreateStoredVar(string key, object defaultValue = null)
    {
        if (_gameSave.Data.ContainsKey(BuildKey(key)))
        {
            var rawData = _gameSave.Data[BuildKey(key)];
            defaultValue = DecodeData(rawData);
        }

        var variable = CreateVar(key, defaultValue);
        _persistentVariables.Add(variable);
        return variable;
    }

    private string BuildKey(string key)
    {
        return Key.Pretty()+"::"+key;
    }

    private LuaTable ProduceEmptyTable()
    {
        _lua.NewTable("tmp");
        return  _lua.GetTable("tmp");
    }
    
    private object DecodeData(object rawData)
    {
        if (rawData is List<object> list)
        {
            var listTable = ProduceEmptyTable();

            for (var i = 0; i < list.Count; i++)
            {
                listTable[i + 1] = DecodeData(list[i]);
            }
            
            return rawData;
        }
        
        if (rawData is not Dictionary<string, object> dict)
        {
            return rawData;
        }

        var root = ProduceEmptyTable();

        foreach (var keyValuePair in dict)
        {
            root[keyValuePair.Key] = DecodeData(keyValuePair.Value);
        }
        
        return root;
    }
}

public interface IDataType
{
    
}

public class VariableWrapper: IDataType
{
    private object _data;

    public VariableWrapper(object data)
    {
        _data = data;
    }

    public void Set(object data)
    {
        _data = data;
    }
    
    public object Get()
    {
        return _data;
    }
}