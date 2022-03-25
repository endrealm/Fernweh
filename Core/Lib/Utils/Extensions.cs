using System.Collections.Generic;
using System.Linq;

namespace Core.Utils;

public static class Extensions
{
    public static Queue<T> ToQueue<T>(this IEnumerable<T> list)
    {
        var queue = new Queue<T>();

        foreach (var x1 in list.Reverse()) queue.Enqueue(x1);

        return queue;
    }

    public static void AddRange<T>(this Queue<T> queue, IEnumerable<T> range)
    {
        foreach (var x1 in range) queue.Enqueue(x1);
    }
}