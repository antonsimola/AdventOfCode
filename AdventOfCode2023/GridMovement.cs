namespace AdventOfCode2023;

using static Direction;

[Flags]
public enum Direction
{
    North = 1 << 0,
    East = 1 << 1,
    South = 1 << 2,
    West = 1 << 3
}

public record P(int Row, int Col)
{
    public static implicit operator P(ValueTuple<int, int> tuple)
    {
        return new P(tuple.Item1, tuple.Item2);
    }

    public static P operator -(P p1, P p2)
    {
        return new P(p1.Row - p2.Row, p1.Col - p2.Col);
    }

    public static P operator +(P p1, P p2)
    {
        return new P(p1.Row + p2.Row, p1.Col + p2.Col);
    }
}; //bleh...

public record PD(P Point, Direction Dir);

public static class GridMovement
{
    public static Dictionary<Direction, Direction> Opposites = new()
    {
        { North, South },
        { West, East },
        { East, West },
        { South, North },
    };

    public static Dictionary<Direction, P> Shifts = new()
    {
        { North, new(-1, 0) },
        { West, new(0, -1) },
        { East, new(0, 1) },
        { South, new(1, 0) },
    };

    public static Dictionary<P, Direction> ShiftToEnum = new()
    {
        { new(-1, 0), North },
        { new(0, -1), West },
        { new(0, 1), East },
        { new(1, 0), South },
    };

    public static char SafeGet(char[][] arr, P point, char safeChar = '.')
    {
        if (point.Col < 0 || point.Col >= arr[0].Length)
        {
            return safeChar;
        }

        if (point.Row < 0 || point.Row >= arr.Length)
        {
            return safeChar;
        }

        return arr[point.Row][point.Col];
    }

    public static P MoveInGrid(P fromPoint, Direction toDirection)
    {
        var s = Shifts[toDirection];
        return new P(fromPoint.Row + s.Row, fromPoint.Col + s.Col);
    }
}