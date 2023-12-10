using MathNet.Spatial.Euclidean;
using static AdventOfCode2023.Helpers;

namespace AdventOfCode2023;

using static Direction;

[Flags]
enum Direction
{
    North = 1 << 0,
    East = 1 << 1,
    South = 1 << 2,
    West = 1 << 3
}

record P(int Y, int X); //bleh...

record PD(P point, Direction direction);

public class Day10 : BaseDay
{
    Dictionary<char, Direction> PipeConnectionTypes = new()
    {
        { '|', North | South },
        { '-', West | East },
        { 'L', North | East },
        { 'J', North | West },
        { '7', South | West },
        { 'F', South | East },
        { 'S', South | East | North | West },
    };

    Dictionary<Direction, Direction> Opposites = new()
    {
        { North, South },
        { West, East },
        { East, West },
        { South, North },
    };

    Dictionary<Direction, P> Shifts = new()
    {
        { North, new (-1, 0) },
        { West, new (0, -1) },
        { East, new (0, 1) },
        { South, new (1, 0) },
    };


    public override void Run()
    {
        var input = Input;

        char[][] arr = input.Select(i => i.AsEnumerable().ToArray()).ToArray();


        var y = 0;
        P start = null;
        foreach (var row in arr)
        {
            var x = 0;
            foreach (var col in row)
            {
                if (col == 'S')
                {
                    start = new (y, x);
                }

                x++;
            }

            y++;
        }

        var startDirs = GetStartingDirections(arr, start);

        var minimalDistance = 0;

        var cur0 = new PD(start, startDirs[0]);
        var cur1 = new PD(start, startDirs[1]);
        var polygon = new List<P>() { cur0.point };
        var other = new List<P>();
        while (true)
        {
            minimalDistance++;
            cur0 = MoveInPipe(arr, cur0.point, cur0.direction);
            cur1 = MoveInPipe(arr, cur1.point!, cur1.direction);

            if (cur0.point == cur1.point)
            {
                polygon.Add(cur0.point);
                break;
            }

            polygon.Add(cur0.point);
            other.Add(cur1.point);
        }

        //Part 2
        other.Reverse();
        polygon.AddRange(other);
        var mathNetPoly = new Polygon2D(polygon.Select(p => new Point2D(p.X, p.Y)));
        var locations = new HashSet<P>();
        y = 0;
        foreach (var row in arr)
        {
            var x = 0;
            foreach (var c in row)
            {
                var testY = new P(y, x);
                if (!polygon.Contains(testY))
                {
                    if (WithinPolygon(mathNetPoly, testY))
                    {
                        locations.Add(testY);
                    }
                }

                x++;
            }

            y++;
        }

        Console.WriteLine("Minimal distance " + minimalDistance);
        Console.WriteLine("Animal Locations " + locations.Count);
    }

    private bool WithinPolygon(Polygon2D polygon, P p)
    {
        return polygon.EnclosesPoint(new Point2D(p.X - 0.5, p.Y - 0.5));
    }


    private List<Direction> GetStartingDirections(char[][] arr, P start)
    {
        var dirs = new List<Direction>();
        foreach (var dir in Enum.GetValues<Direction>())
        {
            var shift = Shifts[dir];
            var next = new P(start.Y + shift.Y, start.X + shift.X);
            var nextChar = SafeGet(arr, next);
            if (nextChar == '.') continue;
            if (CanBeConnectedFrom(dir, nextChar))
            {
                dirs.Add(dir);
            }
        }

        return dirs;
    }


    char SafeGet(char[][] arr, P point)
    {
        if (point.X < 0 || point.X >= arr[0].Length)
        {
            return '.';
        }

        if (point.Y < 0 || point.Y >= arr.Length)
        {
            return '.';
        }

        return arr[point.Y][point.X];
    }



    bool CanBeConnectedFrom(Direction direction, char to)
    {
        var dirs = PipeConnectionTypes[to];

        foreach (var iterFlag in IterFlags(dirs))
        {
            if (direction == Opposites[iterFlag])
            {
                return true;
            }
        }

        return false;
    }

    PD MoveInPipe(char[][] arr, P point, Direction arriveFrom)
    {
        var current = arr[point.Y][point.X];
        var curMoveDirections = PipeConnectionTypes[current];
        foreach (var dir in Enum.GetValues<Direction>())
        {
            if (curMoveDirections.HasFlag(dir))
            {
                var shift = Shifts[dir];
                var next = new P(point.Y + shift.Y, point.X + shift.X);
                var nextChar = SafeGet(arr, next);
                if (nextChar == '.') continue;
                if (CanBeConnectedFrom(dir, nextChar) && dir != arriveFrom)
                {
                    return new PD(next, Opposites[dir]);
                }
            }
        }

        throw new Exception("Something wrong");
    }
}