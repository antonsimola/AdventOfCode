using System.Reflection;

namespace AOC;

public class Program
{

    public static void Main(string[] args)
    {
        var day = DateTime.Now.Day;
        //var day = 10;
        var allDays = Assembly.GetExecutingAssembly().GetExportedTypes()
            .Where(t => t.IsAssignableTo(typeof(BaseDay)))
            .ToList();

        // foreach (var d in allDays.OrderBy(d => d.Name))
        // {
        //     RunDay(d);
        // }

        var currentDay = allDays.FirstOrDefault(d => d.Name.Replace("Day", "") == (day + ""));
        RunDay(currentDay!);

    }

    public static void RunDay(Type day)
    {
        var d = (BaseDay) Activator.CreateInstance(day);
        d.Setup();
        d.Run();
    }
    
    public static void CodeGen()
    {
        for (var i = 1; i <= 25; i++)
        {

            var code = $$"""
                         namespace AOC;
                         public class Day{{i}} : BaseDay
                         {
                          
                                public override void Run()
                                {
                                   var input = Input;
                                    //var input = TestInput;
                                
                                }
                         }
                          
                         """;

            var path = Assembly.GetExecutingAssembly().Location;
            var projFolder =
                Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(path))));
            File.WriteAllText(Path.Combine(projFolder, $"Day{i}.cs"), code);
        }

        
    }
}