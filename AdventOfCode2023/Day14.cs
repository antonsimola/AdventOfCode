namespace AdventOfCode2023;

public class Day14 : BaseDay
{
    public override void Run()
    {
        var input = Input;
        // var input = TestInput;
        var columnWise = input.ColumnWise();

        var sum = 0;
        foreach (var line in columnWise)
        {
            var rolling = new Queue<char>();
            var empty = new Queue<char>();
            foreach (var c in line)
            {
                if (c == '.')
                {
                    empty.Enqueue(c);
                };
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

            var i = line.Length;
            foreach (var c in rolling)
            {
                if (c == 'O')
                {
                    sum += i;
                }

                i--;
            }
        }

        Console.WriteLine(sum);
    }
}