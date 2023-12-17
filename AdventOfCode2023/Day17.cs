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

//         var input = """
// 111111111111
// 999999999991
// 999999999991
// 999999999991
// 999999999991
// """.Trim().Split("\r\n");
        
        var grid = input.Select(c => c.Select(c => c - '0').ToArray()).ToArray();

        var vertices = GetVertices(grid);
        var nodeVertices = vertices.GroupBy(v => v.From).ToDictionary(v => v.Key, v => v.ToList());

        var res = ShortestPath(grid, nodeVertices, new P(0, 0), new P(grid.Length - 1, grid[0].Length - 1));
        Console.WriteLine(res.Item1);
    }

    private (int, List<P> path) ShortestPath(int[][] grid, Dictionary<P, List<Vertex>> vertices, P from, P goal)
    {
        var startPdc = new PDC(from, Direction.South, 0);
        var startPdc2 = new PDC(from, Direction.East , 0);
        var pQueue = new PriorityQueue<PDC, int>();
        var minCostToStart = new Dictionary<PDC, int>() { { startPdc, 0 } };
        var nearestToStart = new Dictionary<PDC, PDC>();
        var visited = new HashSet<PDC>();
        pQueue.Enqueue(startPdc, 0);
        pQueue.Enqueue(startPdc2 , 0);
        while (pQueue.TryDequeue(out var cur, out var heatLoss))
        {
            var connections = vertices[cur.Point];
            foreach (var con in connections.OrderBy(c => c.Weight))
            {
                var shift = con.To - con.From;
                var dir = ShiftToEnum[shift];
                if (cur.Direction != null && dir == Opposites[cur.Direction.Value]) continue;

                var sameDir = cur.Direction != null && cur.Direction == dir;
                
                if (cur.Count > 9 && sameDir) continue;
                if (cur.Count < 4 && cur.Direction != null && !sameDir) continue;
                
                var pdc = new PDC(con.To, dir, sameDir ? cur.Count + 1 : 1);
                // if (visited.Contains(pdc)) continue;

                var minCostToStartCur = minCostToStart.GetValueOrDefault(cur, 0);
                var minCostToStartNext = minCostToStart.GetValueOrDefault(pdc, -1);

                var weight = con.Weight;
                if (minCostToStartNext == -1 || minCostToStartCur + weight < minCostToStartNext)
                {
                    minCostToStart[pdc] = minCostToStartCur + weight;
                    nearestToStart[pdc] = cur;

                    pQueue.Enqueue(pdc, minCostToStartCur + weight);
                }
            }

            visited.Add(cur);
        }

        //MakePath(nearestToStart, goal);

        foreach (var VARIABLE in minCostToStart
                     .Where(k => k.Key.Point == goal)
                     .OrderBy(k => k.Key.Point.Row)
                     .ThenBy(k => k.Key.Point.Col))
        {
            Console.WriteLine(VARIABLE);
        }
        

        return (minCostToStart.Where(b => b.Key.Point == goal).Where(g => g.Key.Count > 3 ).Min(b => b.Value), new List<P>());
    }
    
    private (int, P) JumpWeight(int[][] grid, P curPos, P shift, int times)
    {
        var sum = 0;
        var cur = curPos;
        for (var i = 0; i < times; i++)
        {
            cur += shift;

            if (cur.Row < 0 || cur.Row >= grid.Length || cur.Col < 0 ||
                cur.Col >= grid[0].Length)
            {
                continue;
            }

            sum += grid[cur.Row][cur.Col];
        }

        return (sum, curPos);
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