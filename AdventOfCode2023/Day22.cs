using System.IO.Compression;
using System.Runtime.Intrinsics.X86;
using System.Threading.Channels;
using AdventOfCodeLib;

namespace AdventOfCode2023;

public class Day22 : BaseDay
{
    record Point3D(int X, int Y, int Z)
    {
        public int X =  X;
        public int Y =  Y;
        public int Z =  Z;


        public static Point3D operator -(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }

        public static Point3D operator +(Point3D p1, Point3D p2)
        {
            return new Point3D(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public Point3D Unit()
        {
            return new Point3D(X == 0 ? 0 : X / X, Y == 0 ? 0 : Y / Y, Z == 0 ? 0 : Z / Z);
        }
    }

    record Brick(Point3D Start, Point3D End, string Name)
    {
        private HashSet<Point3D> _body;
        public int MinZ => Start.Z;
        public Point3D End = End;
        public Point3D Start = Start;


        // public bool Blocks(Brick another)
        // {
        //     var body = GetBody();
        //     var otherBody = another.GetBody();
        //
        //     return body.Overlaps(otherBody);
        // }
        

        public bool IsBlockedBy(Brick another)
        {
            return YOverlap(another) && ZOverlap(another) &&
                   XOverlap(another);
        }

        public bool XOverlap(Brick brick)
        {
            return Start.X <= brick.End.X && brick.Start.X <= End.X;
        }

        public bool ZOverlap(Brick brick)
        {
            return Start.Z <= brick.End.Z && brick.Start.Z <= End.Z;
        }

        public bool YOverlap(Brick brick)
        {
            return Start.Y <= brick.End.Y && brick.Start.Y <= End.Y;
        }
    };

    public override void Run()
    {
        var input = Input;
        // var input = TestInput;

        var bricks = new List<Brick>();
        char name = 'A';
        foreach (var line in input)
        {
            var s = line.Split("~");
            var start = s[0].Split(",").Select(int.Parse).ToList();
            var end = s[1].Split(",").Select(int.Parse).ToList();

            bricks.Add(new Brick(new Point3D(
                    Math.Min(start[0], end[0]),
                    Math.Min(start[1], end[1]),
                    Math.Min(start[2], end[2])),
                new Point3D(
                    Math.Max(start[0], end[0]),
                    Math.Max(start[1], end[1]),
                    Math.Max(start[2], end[2])),
                (name++) + ""));
        }


        var (dropped, _) = SimulateFalling(bricks);

        int safeToRemove = 0;
        var bestBrickCollapse = 0;
        var progress = 0.0;
        foreach (var brick in dropped)
        {
            Console.WriteLine("   " + progress / dropped.Count);
            progress++;
            var (simulateRemoved, collapse) = SimulateFalling(dropped.Where(b => b != brick).ToList());
            bestBrickCollapse += collapse;

            if (simulateRemoved.All(r => dropped.Contains(r)))
            {
                safeToRemove++;
            }
        }


        //part 1
        Console.WriteLine("Part 1: " + safeToRemove);
        //part 2
        Console.WriteLine("Part 2: " + bestBrickCollapse);
    }

    private (IList<Brick>, int) SimulateFalling(List<Brick> bricks)
    {
        var down = new Point3D(0, 0, -1);
        var currentLoop = bricks.OrderBy(b => b.MinZ).ToList();
        var q = new PriorityQueue<Brick, int>();
        currentLoop.ForEach(b => q.Enqueue(b, b.MinZ));

        var supported = new List<Brick>(bricks.Count);
        var waiting = new List<Brick>(bricks.Count);
        var falls = new HashSet<string>();

        var i = 0;

        while (true)
        {
            while (q.TryDequeue(out var b, out var _))
            {
                var brickMovedDown = new Brick(Start: b.Start + down, End: b.End + down, b.Name);
                if (brickMovedDown.Start.Z == 0 || brickMovedDown.End.Z == 0)
                {
                    supported.Add(b);
                    continue;
                }

                var foundBlock = false;
                foreach (var (otherBrick, _) in q.UnorderedItems)
                {
                    if (brickMovedDown.IsBlockedBy(otherBrick))
                    {
                        foundBlock = true;
                        waiting.Add(b);
                        break;
                    }
                }

                if (foundBlock) continue;

                foreach (var otherBrick in supported)
                {
                    if (brickMovedDown.IsBlockedBy(otherBrick))
                    {
                        supported.Add(b);
                        foundBlock = true;
                        break;
                    }
                }

                if (!foundBlock)
                {
                    falls.Add(brickMovedDown.Name);
                    q.Enqueue(brickMovedDown, brickMovedDown.MinZ);
                }
            }

            if (supported.Count < bricks.Count)
            {
                waiting.ForEach(w => q.Enqueue(w, w.MinZ));
                waiting.Clear();
            }
            else
            {
                break;
            }
        }
        
        return (supported, falls.Count);
    }
}