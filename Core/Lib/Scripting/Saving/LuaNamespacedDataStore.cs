﻿using System;
using System.Collections;
using System.Collections.Generic;
using Core.Saving;
using NLua;

namespace Core.Scripting.Saving;

public class LuaNamespacedDataStore : NamespacedDataStore, IDataEncoder
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
        
        // Save persistent variables
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
        
        // Save function data
        if(_saveFunction != null) {
            var saveData = _saveFunction.Save(this);
        
            var key = BuildKey("BASE", "FUNC");
            if (_gameSave.Data.ContainsKey(key))
            {
                _gameSave.Data[key] = saveData;
                return;
            }
            _gameSave.Data.Add(key, saveData);
        }
    }

    public override void Load()
    {
        _gameSave.Data.TryGetValue(BuildKey("BASE", "FUNC"), out var data);
        _loadFunction?.Load(this, data);
    }

    private string BuildKey(string key, string type = "VAR")
    {
        return $"{Key.Pretty()}::{type}::{key}";
    }

    private LuaTable ProduceEmptyTable()
    {
        _lua.NewTable("tmp");
        return  _lua.GetTable("tmp");
    }
    
    public object DecodeData(object rawData)
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
    public object EncodeData(object rawData)
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

        if (rawData == null || IsPrimitive(rawData.GetType()))
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