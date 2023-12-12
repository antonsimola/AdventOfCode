using System.Diagnostics;

namespace AdventOfCode2023;

public class Day11 : BaseDay
{
    record Cell(int Row, int Col);

    public override void Run()
    {
        var input = Input;
        // var input = TestInput;

        var isDot = (char e) => e == '.';

        List<List<char>> grid = input.Select(l => l.ToList()).ToList();
        
        List<int> dupesRow = grid.Select((r, i) => r.All(isDot) ? i : -1).Where(i => i >= 0).ToList();

        List<int> dupesCol = grid[0]
            .Select((c, i) => grid.Select(row => row[i]).All(isDot) ? i : -1)
            .Where(i => i >= 0)
            .ToList();
        
        List<Cell> galaxies = grid
            .SelectMany((r, i) =>
                r.Select((c, j) => c == '#' ? new Cell(i, j) : null)
                .WhereNotNull()
            ).ToList();

        long grandSum = 0;
        var pairs = new HashSet<(Cell, Cell)>();
        foreach (var g1 in galaxies)
        {
            foreach (var g2 in galaxies)
            {
                if (g1 == g2)
                {
                    continue;
                }

                var pair = (g1, g2);
                if (pairs.Contains((g2, g1)))
                {
                    continue;
                }

                pairs.Add(pair);

                long dc = Math.Abs(g2.Col - g1.Col);
                long dr = Math.Abs(g2.Row - g1.Row);

                long lc = Math.Max(g1.Col, g2.Col);
                long sc = Math.Min(g1.Col, g2.Col);
                long lr = Math.Max(g1.Row, g2.Row);
                long sr = Math.Min(g1.Row, g2.Row);

                var expand = 1000000 - 1;
                long sum = dc + dr
                              + dupesRow.Count(i => sr < i && i < lr) * expand +
                              dupesCol.Count(i => sc < i && i < lc) * expand;
                grandSum += sum;
            }
        }

        Console.WriteLine(grandSum); //504715068438
    }
}