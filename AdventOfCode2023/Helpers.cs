using System.Collections.ObjectModel;

namespace AdventOfCode2023;

public static class Helpers
{
    public static int[] ParseIntArray(this string line, string separator = " ")
    {
        return line.Split(separator).Select(int.Parse).ToArray();
    }

    public static List<int> ParseIntList(this string line, string separator = " ")
    {
        return line.Split(separator).Select(int.Parse).ToList();
    }
    
    public static long[] ParseLongArray(this string line, string separator = " ")
    {
        return line.Split(separator).Select(long.Parse).ToArray();
    }

    public static List<long> ParseLongList(this string line, string separator = " ")
    {
        return line.Split(separator).Select(long.Parse).ToList();
    }
    
    
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> enumerable)
    {
        return enumerable.Where(t  => t != null).Cast<T>();
    }
    
    public static IEnumerable<(T v, int i)> WithIndex<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.Select((v, i) => (v: v, i: i));
    }

    
   public static IEnumerable<TEnum> IterFlags<TEnum>(TEnum d) where TEnum : struct, Enum
    {
        foreach (var f in Enum.GetValues<TEnum>())
        {
            if (d.HasFlag(f))
            {
                yield return f;
            }
        }
    }

    public static TValue GetOrAdd<TKey,TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> adderFunction)
    {
        if (dictionary.TryGetValue(key, out var v))
        {
            return v;
        }
        var newV = adderFunction(key);
        dictionary[key] = newV;
        return newV;
    }
    
    public static IEnumerable<IList<T>> CombineWithRepetitions<T>(this IEnumerable<T> input, int take)
    {
        ICollection<IList<T>> output = new Collection<IList<T>>();
        IList<T> item = new T[take];

        CombineWithRepetitions(output, input, item, 0);

        return output;
    }

    private static void CombineWithRepetitions<T>(ICollection<IList<T>> output, IEnumerable<T> input, IList<T> item, int count)
    {
        if (count < item.Count)
        {
            var enumerable = input as IList<T> ?? input.ToList();
            foreach (var symbol in enumerable)
            {
                item[count] = symbol;
                CombineWithRepetitions(output, enumerable, item, count + 1);
            }
        }
        else
        {
            output.Add(new List<T>(item));
        }
    }
}