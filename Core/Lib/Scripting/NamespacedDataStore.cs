using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public override void Save()
    {
        _persistentVariables.ForEach(wrapper =>
        {
            var key = BuildKey(wrapper.Key);
            var data = wrapper.Get();
            data = EncodeData(data);
            if (_gameSave.Data.ContainsKey(key))
            {
                _gameSave.Data[key] = data;
                return;
            }
            _gameSave.Data.Add(key, data);
        });
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
    private object EncodeData(object rawData)
    {
        if (rawData is LuaTable table)
        {
            var isArray = table.Keys.Count == 0;
            foreach (var tableKey in table.Keys)
            {
                if (tableKey is not int)
                {
                    isArray = false;
                }
            }

            if (isArray)
            {
                var list = new List<object>();
                var lastIndex = 0;
                foreach (DictionaryEntry entry in table)
                {
                    var index = (int) entry.Key;

                    // Fill nulls when lua is missing spaces
                    for (var i = lastIndex + 1; i < index; i++)
                    {
                        list.Add(null);
                    }
                    list.Add(EncodeData(entry.Value));
                }

                return list;
            }

            var dict = new Dictionary<string, object>();

            foreach (DictionaryEntry entry in table)
            {
                dict.Add(entry.Key.ToString(), EncodeData(entry.Value));
            }

            return dict;
        }

        if (IsPrimitive(rawData.GetType()))
        {
            return rawData;
        }


        throw new Exception("Unsupported data type");
    }

    private static bool IsPrimitive(Type t)
    {
        return t.IsPrimitive || t == typeof(Decimal) || t == typeof(String);
    }
}

public interface IDataType
{
    
}

public class VariableWrapper: IDataType
{
    private readonly string _key;
    private object _data;

    public VariableWrapper(string key, object data)
    {
        _key = key;
        _data = data;
    }

    public string Key => _key;

    public void Set(object data)
    {
        _data = data;
    }
    
    public object Get()
    {
        return _data;
    }
}