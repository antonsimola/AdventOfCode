using System.Collections;
using System.Diagnostics;
using System.Net.Quic;

namespace AdventOfCode2023;

using static Direction;
using static GridMovement;

public class Day16 : BaseDay
{
    record Path
    {
        public List<PD> Steps { get; set; } = new List<PD>();
        public bool Completed { get; set; }
    }


    public override void Run()
    {
        var input = Input;
        // var input = TestInput;

        char[][] grid = input.Select(i => i.ToArray()).ToArray();

        var max = 0;

        var startPos = GenerateStartPds(grid).ToList();


        foreach (var startingPd in GenerateStartPds(grid))
        {
            var paths = new List<Path>() { new Path { Steps = [startingPd] } };
            var visited = new HashSet<PD>();

            while (true)
            {
                Traverse(grid, paths, visited, false);
                if (paths.All(p => p.Completed))
                {
                    break;
                }
            }
            
            
            max = Math.Max(paths.SelectMany(p => p.Steps.Select(p => p.Point)).ToHashSet().Count, max);
        }

        Console.WriteLine(max);
    }

    private IEnumerable<PD> GenerateStartPds(char[][] grid)
    {
        var i = 0;
        foreach (var row in grid)
        {
            yield return new PD((i, 0), West);
            yield return new PD((i++, row.Length - 1), East);
        }

        i = 0;
        foreach (var col in grid[0])
        {
            yield return new PD((0, i), North);
            yield return new PD((grid[0].Length - 1, i++), South);
        }
    }

    private void Visualize(char[][] grid, IList<Path> paths)
    {
        Console.Clear();

        var allPathPoints = paths.SelectMany(p => p.Steps.Select(s => s.Point)).ToHashSet();
        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid[0].Length; j++)
            {
                if (allPathPoints.Contains((i, j)))
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(grid[i][j]);
                }
            }

            Console.WriteLine();
        }

        Console.WriteLine("");
        Thread.Sleep(100);
    }

    void  Traverse(char[][] arr, List<Path> paths, HashSet<PD> visited, bool visu = false)
    {
        HashSet<Path> pathsToRemove = null;
        List<Path> newPaths = null;
        foreach (var path in paths)
        {
            var prev = path.Steps.Last();
            var weAreAt = prev.Point; //we are at
            var arriveFrom = prev.Dir; //arriveFrom
            var current = SafeGet(arr, weAreAt, '?');

            if (current == '?')
            {
                path.Steps.RemoveAt(path.Steps.Count - 1);
                path.Completed = true;
            }

            if (path.Completed) continue;


            if (current == '.')
            {
                var next = MoveInGrid(weAreAt, Opposites[prev.Dir]);
                var pd = prev with { Point = next };
                if (!visited.Add(pd))
                {
                    path.Completed = true;
                    continue;
                }

                path.Steps.Add(pd);
            }
            else if (current == '\\')
            {
                var nextDir = arriveFrom switch
                {
                    North => East,
                    East => North,
                    South => West,
                    West => South,
                    _ => throw new ArgumentOutOfRangeException()
                };

                var next = MoveInGrid(weAreAt, nextDir);
                var pd = new PD(next, Opposites[nextDir]);
                if (!visited.Add(pd))
                {
                    path.Completed = true;
                    continue;
                }

                path.Steps.Add(pd);
            }

            else if (current == '/')
            {
                var nextDir = arriveFrom switch
                {
                    North => West,
                    West => North,
                    South => East,
                    East => South,
                    _ => throw new ArgumentOutOfRangeException()
                };

                var next = MoveInGrid(prev.Point, nextDir);
                var pd = new PD(next, Opposites[nextDir]);
                if (!visited.Add(pd))
                {
                    path.Completed = true;
                    continue;
                }

                path.Steps.Add(pd);
            }

            else if (current == '|')
            {
                if ((East | West).HasFlag(arriveFrom))
                {
                    List<PD> newStepsN = [..path.Steps];
                    List<PD> newStepsS = [..path.Steps];
                    var pdN = new PD(MoveInGrid(weAreAt, North), South);
                    newStepsN.Add(pdN);
                    var pdS = new PD(MoveInGrid(weAreAt, South), North);
                    newStepsS.Add(pdS);

                    if (newPaths == null) newPaths = new List<Path>();
                    AddChecking(visited, newStepsN, newPaths, pdN);
                    AddChecking(visited, newStepsS, newPaths, pdS);

                    if (pathsToRemove == null) pathsToRemove = new HashSet<Path>();
                    pathsToRemove.Add(path);
                }
                else
                {
                    var next = MoveInGrid(prev.Point, Opposites[arriveFrom]);
                    var pd = prev with { Point = next };
                    if (!visited.Add(pd))
                    {
                        path.Completed = true;
                        continue;
                    }

                    path.Steps.Add(pd);
                }
            }

            else if (current == '-')
            {
                if ((North | South).HasFlag(arriveFrom))
                {
                    List<PD> newPathW = [..path.Steps];
                    List<PD> newPathE = [..path.Steps];
                    var pdW = new PD(MoveInGrid(weAreAt, West), East);
                    newPathW.Add(pdW);
                    var pdE = new PD(MoveInGrid(weAreAt, East), West);
                    newPathE.Add(pdE);

                    if (newPaths == null) newPaths = new List<Path>();

                    AddChecking(visited, newPathW, newPaths, pdW);
                    AddChecking(visited, newPathE, newPaths, pdE);

                    if (pathsToRemove == null) pathsToRemove = new HashSet<Path>();
                    pathsToRemove.Add(path);
                }
                else
                {
                    var next = MoveInGrid(prev.Point, Opposites[arriveFrom]);
                    var pd = prev with { Point = next };
                    if (!visited.Add(pd))
                    {
                        path.Completed = true;
                        continue;
                    }

                    path.Steps.Add(pd);
                }
            }
        }

        if (newPaths != null)
        {
            paths.AddRange(newPaths);
        }

        paths.RemoveAll(p => pathsToRemove?.Contains(p) ?? false);


        if (visu) Visualize(arr, paths);

        // while loop instead of recursion for part 2 to avoid stack overflow
        // if (!paths.All(p => p.Completed))
        // {
        //     Traverse(arr, paths, visited, visu);
        // }
    }

    private static void AddChecking(HashSet<PD> visited, List<PD> newPath, List<Path> newPaths, PD pd)
    {
        var newPath2 = new Path() { Steps = newPath };
        newPaths.Add(newPath2);
        if (!visited.Add(pd))
        {
            newPath2.Completed = true;
        }
    }

    private bool HasLoop(Path path)
    {
        var hashSet = new HashSet<PD>(path.Steps.Count);
        foreach (var p in path.Steps)
        {
            if (!hashSet.Add(p))
            {
                return true;
            }
        }

        return false;
    }
}