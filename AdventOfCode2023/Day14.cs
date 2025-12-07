using AdventOfCodeLib;

namespace AdventOfCode2023;

public class Day14 : BaseDay
{
    public override void Run()
    {
        // var input = Input;
        IList<string> input = TestInput.ToList();
        
        for (var i = 0; i < 1; i++)
        {
            input = Cycle(input);
        }

        foreach (var line in input)
        {
            Console.WriteLine(line);    
        }
        
        
        
        Console.WriteLine(Count(input));
    }

    private IList<string> Cycle(IList<string> input)
    {
        //TODO does not work
        // north 
        // var c = input.ColumnWise();
        // c=  Tilt(c);
        //
        // //west
        // c = c.ColumnWise();
        // c=  Tilt(c);
        //
        // //south
        // c = c.ColumnWise();
        // c=  Tilt(c);
        //
        // //east
        // c = c.ColumnWise();
        // c=  Tilt(c);

        // return c;
        return null;
    }

    private static IList<string> Tilt(IList<string> columnWise)
    {
        var tiltedLines = new List<string>(columnWise.Count);
        foreach (var line in columnWise)
        {
            var rolling = new Queue<char>(line.Length);
            var empty = new Queue<char>(line.Length);
            foreach (var c in line)
            {
                if (c == '.')
                {
                    empty.Enqueue(c);
                }

                ;
                if (c == '#')
                {
                    while (empty.TryDequeue(out var c1))
                    {
                        rolling.Enqueue(c1);
                    }

                    rolling.Enqueue('#');
                }

                if (c == 'O')
                {
                    rolling.Enqueue(c);
                }
            }
            while (empty.TryDequeue(out var c1))
            {
                rolling.Enqueue(c1);
            }
            tiltedLines.Add(string.Join("", rolling));
        }
        
        return tiltedLines;
    }

    private int Count(IList<string> tiltedLines)
    {
        var sum = 0;
        foreach (var line in tiltedLines)
        {
            var i = line.Length;
            foreach (var c in line)
            {
                if (c == 'O')
                {
                    sum += i;
                }

                i--;
            }
        }

        return sum;
    }
}