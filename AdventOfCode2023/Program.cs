using AdventOfCodeLib;

namespace AdventOfCode2023;

public class Program
{
    public static void Main(string[] args)
    {
        BaseDay.Year = 2023;
        var dayType = Runner.FindDayType(25);
        Runner.RunDay(dayType);
    }
}