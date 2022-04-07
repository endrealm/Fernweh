using System;
using System.Collections.Generic;
using System.Linq;

namespace Core.Utils;

public static class Extensions
{
    public static Stack<T> ToStack<T>(this IEnumerable<T> list)
    {
        var queue = new Stack<T>();

        foreach (var x1 in list.Reverse()) queue.Push(x1);

        return queue;
    }
    public static ISet<T> ToSet<T>(this IEnumerable<T> list)
    {
        var queue = new HashSet<T>();

        foreach (var x1 in list) queue.Add(x1);

        return queue;
    }

    public static void AddRange<T>(this Stack<T> queue, IEnumerable<T> range)
    {
        foreach (var x1 in range.Reverse()) queue.Push(x1);
    }  
    public static V GetOrCreate<K, V>(this Dictionary<K, V> dictionary, K key, Func<V> create)
    {
        if (dictionary.TryGetValue(key, out var data)) return data;
        var value = create.Invoke();
        dictionary.Add(key, value);
        return value;
    }
}