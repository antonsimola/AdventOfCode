using System.Diagnostics;
using System.Numerics;
using System.Reflection;

namespace AdventOfCodeLib;

public class Runner
{
    public static Type FindDayType(int? day = null)
    {
        if (day == null) day = DateTime.Now.Day;

        var allDays = Assembly.GetEntryAssembly().GetExportedTypes()
            .Where(t => t.IsAssignableTo(typeof(BaseDay)))
            .Where(t => !t.IsAbstract)
            .ToList();

// foreach (var d in allDays.OrderBy(d => int.Parse(d.Name.Replace("Day", ""))))
// {
//     if (int.Parse(d.Name.Replace("Day", "")) <= day)
//     {
//         RunDay(d);
//     }
// }

        var dayType = allDays.FirstOrDefault(d => d.Name.Replace("Day", "") == (day + ""));
        return dayType;
    }

    public static void RunDay(Type day)
    {
        var d = (BaseDay)Activator.CreateInstance(day);
        Console.WriteLine(day.Name);
        Console.WriteLine();
        d.Setup();
        var sw = Stopwatch.StartNew();
        d.Run();
        Console.WriteLine();
        Console.WriteLine($"---{sw.ElapsedMilliseconds}ms---");
    }
}