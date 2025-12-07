using AdventOfCodeLib;
using MathNet.Spatial.Euclidean;
using static AdventOfCodeLib.Helpers;

namespace AdventOfCode2023;

using static Direction;
using static GridMovement;

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

    public override void Run()
    {
        var input = Input;

        char[][] arr = input.Select(i => i.ToArray()).ToArray();


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
        var polygon = new List<P>() { cur0.Point };
        var other = new List<P>();
        while (true)
        {
            minimalDistance++;
            cur0 = MoveInPipe(arr, cur0.Point, cur0.Dir);
            cur1 = MoveInPipe(arr, cur1.Point!, cur1.Dir);

            if (cur0.Point == cur1.Point)
            {
                polygon.Add(cur0.Point);
                break;
            }

            polygon.Add(cur0.Point);
            other.Add(cur1.Point);
        }

        //Part 2
        other.Reverse();
        polygon.AddRange(other);
        var mathNetPoly = new Polygon2D(polygon.Select(p => new Point2D(p.Col, p.Row)));
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
        return polygon.EnclosesPoint(new Point2D(p.Col - 0.5, p.Row - 0.5));
    }


    private List<Direction> GetStartingDirections(char[][] arr, P start)
    {
        var dirs = new List<Direction>();
        foreach (var dir in Enum.GetValues<Direction>())
        {
            var shift = Shifts[dir];
            var next = new P(start.Row + shift.Row, start.Col + shift.Col);
            var nextChar = SafeGet(arr, next);
            if (nextChar == '.') continue;
            if (CanBeConnectedFrom(dir, nextChar))
            {
                dirs.Add(dir);
            }
        }

        return dirs;
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
        var current = arr[point.Row][point.Col];
        var curMoveDirections = PipeConnectionTypes[current];
        foreach (var dir in Enum.GetValues<Direction>())
        {
            if (curMoveDirections.HasFlag(dir))
            {
                var shift = Shifts[dir];
                var next = new P(point.Row + shift.Row, point.Col + shift.Col);
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