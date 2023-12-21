namespace AdventOfCode2023;

using static GridMovement;

public class Day21 : BaseDay
{
    public override void Run()
    {
        var input = Input;
        // var input = TestInput;

        var grid = input.Select(s => s.ToArray()).ToArray();


        P start = (0, 0);

        foreach (var (row, i) in grid.WithIndex())
        {
            foreach (var (col, j) in row.WithIndex())
            {
                if (col == 'S')
                {
                    start = (i, j);
                    grid[i][j] = '.';
                }
            }
        }

        var startingLocationsForStep = new List<P>() { start };


        // keep track of
        
        
        
        for (var i = 0; i < 64; i++)
        {
            var nextSteps = new HashSet<P>();
            foreach (var s in startingLocationsForStep)
            {
                foreach (var shift in Shifts.Values)
                {
                    var step = s + shift;
                    
                    var land = SafeGet(grid, step, '#');
                    if (land == '.')
                    {
                        nextSteps.Add(step);
                    }
                }
            }

            startingLocationsForStep = [..nextSteps];

            // Draw(grid, startingLocationsForStep);
        }


        Console.WriteLine(startingLocationsForStep.Count);
    }

    private void Draw(char[][] grid, List<P> startingLocationsForStep)
    {
        foreach (var (r, i) in grid.WithIndex())
        {
            foreach (var (c, j) in r.WithIndex())
            {
                if (startingLocationsForStep.Contains((i, j)))
                {
                    Console.Write("O");
                }
                else
                {
                    Console.Write(c);
                }
            }

            Console.WriteLine();
        }
    }
}