using System.Diagnostics;
using System.Reflection.Metadata;

namespace AdventOfCode2023;

using static GridMovement;

public class Day17 : BaseDay
{
    record Vertex(P From, P To, int Weight);

    record PDC(P Point, Direction? Direction, int Count);
    

    public override void Run()
    {
        var input = Input;
        // var input = TestInput;
        var grid = input.Select(c => c.Select(c => c - '0').ToArray()).ToArray();

        var vertices = GetVertices(grid);
        var nodeVertices = vertices.GroupBy(v => v.From).ToDictionary(v => v.Key, v => v.ToList());

        var res = ShortestPath(grid, nodeVertices, new P(0, 0), new P(grid.Length - 1, grid[0].Length - 1));
        Visualize(grid, res.path);

        Console.WriteLine(res.Item1);
    }

    private void Visualize(int[][] grid, List<P> res)
    {
        Console.Clear();
        for (int r = 0; r < grid.Length; r++)
        {
            for (int c = 0; c < grid[0].Length; c++)
            {
                if (res.Contains(new P(r, c)))
                {
                    var def = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(grid[r][c]);
                    Console.ForegroundColor = def;
                }
                else
                {
                    Console.Write(grid[r][c]);
                }
            }

            Console.WriteLine();
        }
    }

    // private void GreedyShortestPath(int[][] grid, Dictionary<P, List<Vertex>> vertices, P from, P goal, State state)
    // {
    //
    //     var cur = from;
    //     while (true)
    //     {
    //         var connections = vertices[cur];
    //         foreach (var con in connections.OrderBy(c => c.Weight))
    //         {
    //             
    //             GreedyShortestPath(grid, vertices, con.To, goal, state with {SumWeight = state.SumWeight + con.Weight});
    //         }
    //     }
    // }


    private (int, List<P> path) ShortestPath(int[][] grid, Dictionary<P, List<Vertex>> vertices, P from, P goal)
    {
        var startPdc = new PDC(from, null, 0);
        var pQueue = new PriorityQueue<PDC, int>();
        var minCostToStart = new Dictionary<PDC, int>() { { startPdc, 0 } };
        var nearestToStart = new Dictionary<PDC, PDC>();
        var visited = new HashSet<PDC>();
        pQueue.Enqueue(startPdc, 0);
        while (true)
        {
            var cur = pQueue.Dequeue();
            var connections = vertices[cur.Point];
            foreach (var con in connections.OrderBy(c => c.Weight))
            {
             
                var dir = ShiftToEnum[con.To - con.From];
                if (cur.Direction != null && dir == Opposites[cur.Direction.Value]) continue;
                
                var pdc = new PDC(con.To, dir, cur.Direction == dir ? cur.Count +1 : 0) ;
                if (visited.Contains(pdc)) continue;
                
                if (pdc.Count > 2) continue;
                
                var minCostToStartCur = minCostToStart.GetValueOrDefault(cur, 0);
                var minCostToStartNext = minCostToStart.GetValueOrDefault(pdc, -1);
                if (minCostToStartNext == -1 || minCostToStartCur + con.Weight < minCostToStartNext)
                {
                    
                    minCostToStart[pdc] = minCostToStartCur + con.Weight;
                    nearestToStart[pdc] = cur;
                    
                    pQueue.Enqueue(pdc, minCostToStartCur + con.Weight);
                }
            }
    
            visited.Add(cur);
            if (cur.Point == goal)
            {
                break;
            }
    
            if (pQueue.Count == 0)
            {
                break;
            }
        }
    
        // var thepath = new List<P>() { goal };
        // var n = goal;
        // while (true)
        // {
        //     if (n == from) break;
        //     n = nearestToStart[n];
        //     thepath.Add(n);
        // }
        //
        // thepath.Reverse();
    
    
        // return (minCostToStart[goal], thepath);

        
        return (minCostToStart.Where(b => b.Key.Point == goal).Min(b => b.Value), new List<P>());
        
        return (0, new List<P>());
    }

    private bool CheckBackwards(int[][]grid, Dictionary<P, P> nearestToStart, P from, P conTo)
    {
        var thepath = new List<P>() { conTo };
        var n = conTo;
        while (true)
        {
            if (n == from) break;
            n = nearestToStart[n];
            thepath.Add(n);
        }

        thepath.Reverse();
        
        // Visualize(grid, thepath);
        
        var sameCount = 0;
        Direction? prevDir = null;
        
        
        
        foreach (var p in thepath.Zip(thepath.Skip(1)))
        {
            var prev = p.First;
            var cur = p.Second;
            var shift =   ShiftToEnum[cur - prev];
            if (prevDir != null && shift  == Opposites[prevDir.Value])
            {
                return false;
            }

            if (prevDir == null || shift == prevDir)
            {
                sameCount++;
                if (sameCount > 3)
                {
                    return false;
                }
            }

            prevDir = shift;
        }

        return true;
    }


    

    private List<Vertex> GetVertices(int[][] grid)

    {
        var vertices = new List<Vertex>((grid.Length + grid[0].Length) * 4);
        for (var r = 0; r < grid.Length; r++)
        {
            for (int c = 0; c < grid[0].Length; c++)
            {
                var p = new P(r, c);
                foreach (var dir in Shifts.Values.Select(s => p + s))
                {
                    if (dir.Row < 0 || dir.Row >= grid.Length || dir.Col < 0 || dir.Col >= grid[0].Length) continue;

                    vertices.Add(new Vertex((r, c), dir, grid[dir.Row][dir.Col]));
                }
            }
        }

        return vertices;
    }
}