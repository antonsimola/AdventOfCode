namespace AOC;

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
}