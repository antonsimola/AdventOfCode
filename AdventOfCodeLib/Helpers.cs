using System.Collections.ObjectModel;
using System.Numerics;
using System.Text;

namespace AdventOfCodeLib;

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
        return enumerable.Where(t => t != null).Cast<T>();
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

    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key,
        Func<TKey, TValue> adderFunction)
    {
        if (dictionary.TryGetValue(key, out var v))
        {
            return v;
        }

        var newV = adderFunction(key);
        dictionary[key] = newV;
        return newV;
    }

    public static IEnumerable<IList<T>> CombineWithRepetitions<T>(this IEnumerable<T> input, int take,
        IDictionary<string, ICollection<IList<T>>> cache)
    {
        var cacheKey = string.Join("", input) + take;
        if (cache.TryGetValue(cacheKey, out var v))
        {
            return v;
        }

        ICollection<IList<T>> output = new Collection<IList<T>>();
        IList<T> item = new T[take];

        CombineWithRepetitions(output, input, item, 0);
        cache[cacheKey] = output;
        return output;
    }

    private static void CombineWithRepetitions<T>(ICollection<IList<T>> output, IEnumerable<T> input, IList<T> item,
        int count)
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

    public static long Factorial(long n)
    {
        if (n == 1)
        {
            return 1;
        }

        return n * Factorial(n - 1);
    }

    public static long CombinationsWithRepetitionCount(long sampleSize, long totalCount)
    {
        return Factorial(sampleSize + totalCount - 1) / (Factorial(sampleSize) * Factorial(totalCount - 1));
    }

    public static string[] ColumnWise(this string[] grid)
    {
        var columnWise = new List<string>();

        for (var i = 0; i < grid[0].Length; i++)
        {
            var sb = new StringBuilder();
            foreach (var line in grid)
            {
                sb.Append(line[i]);
            }

            columnWise.Add(sb.ToString());
        }

        return columnWise.ToArray();
    }

    public static long PolygonAreaShoelace(List<PL> points)
    {
        long sum = 0;
        points = [..points, points[0]];
        for (var i = 0; i < points.Count - 1; i++)
        {
            sum += (points[i].Row * points[i + 1].Col - points[i + 1].Row * points[i].Col);
        }

        return Math.Abs(sum / 2);
    }

    public static long PolygonPickTheoremInnerPointCount(long perimeter, long area)
    {
        return area - perimeter / 2 + 1;
    }

    public static long LeastCommonMultiple(List<long> args)
    {
        return args.Aggregate(LeastCommonMultiple);
    }


    public static long GreatestCommonDivisor(long a, long b)
    {
        if (a == 0)
            return b;
        return GreatestCommonDivisor(b % a, a);
    }

    public static long LeastCommonMultiple(long a, long b)
    {
        return (a / GreatestCommonDivisor(a, b)) * b;
    }

    public static (BigInteger X , BigInteger Y)? LineIntercect(PB l1p1, PB l1p2, PB l2p1, PB l2p2)
    {
        checked
        {
            BigInteger denom = (l1p1.Row - l1p2.Row) * (l2p1.Col - l2p2.Col) - (l1p1.Col - l1p2.Col) * (l2p1.Row - l2p2.Row);
            if (denom == 0) return null;
            var pxNom = (l1p1.Row * l1p2.Col - l1p1.Col * l1p2.Row) * (l2p1.Row - l2p2.Row) -
                        (l1p1.Row - l1p2.Row) * (l2p1.Row * l2p2.Col - l2p1.Col * l2p2.Row);
            var pyNom = (l1p1.Row * l1p2.Col - l1p1.Col * l1p2.Row) * (l2p1.Col - l2p2.Col) -
                        (l1p1.Col - l1p2.Col) * (l2p1.Row * l2p2.Col - l2p1.Col * l2p2.Row);
            
            return (pxNom / denom, pyNom /  denom);    
        }
        
    }

    public static double PointDistance((decimal Row, decimal Col) p1, PDe p2)
    {
        checked
        {
            return Math.Sqrt(Math.Pow((double)p2.Row - (double)p1.Row, 2) + Math.Pow((double)p2.Col - (double)p1.Col, 2));    
        }
    }

    public static decimal DotProduct(PDe p1, PDe p2)
    {
        return p1.Row * p2.Row + p1.Col * p2.Col;
    }
}