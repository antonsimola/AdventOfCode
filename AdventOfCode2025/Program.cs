using System.Diagnostics;
using System.Reflection;
using AdventOfCodeLib;

public class Program
{
    public static void Main(string[] args)
    {
        BaseDay.Year = 2025;
        var dayType = Runner.FindDayType(6);
        Runner.RunDay(dayType);
    }
}