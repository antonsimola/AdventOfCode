using System.Reflection;

namespace AdventOfCodeLib;

public class CodeGen
{
    static void Generate(int year)
    {
        for (var i = 2; i <= 25; i++)
        {
            var code = $$"""
                         using AdventOfCodeLib;

                         namespace AdventOfCode{{year}};

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