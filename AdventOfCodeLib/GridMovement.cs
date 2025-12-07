using System.Numerics;

namespace AdventOfCodeLib;

using static Direction;

[Flags]
public enum Direction
{
    North = 1 << 0,
    East = 1 << 1,
    South = 1 << 2,
    West = 1 << 3
}

public record PB(BigInteger Row, BigInteger Col)
{
    public static implicit operator PB(ValueTuple<BigInteger, BigInteger> tuple)
    {
        return new PB(tuple.Item1, tuple.Item2);
    }

    public static PB operator -(PB p1, PB p2)
    {
        return new PB(p1.Row - p2.Row, p1.Col - p2.Col);
    }

    public static PB operator +(PB p1, PB p2)
    {
        return new PB(p1.Row + p2.Row, p1.Col + p2.Col);
    }
    
    public static PB operator +(PB p1, P p2)
    {
        return new PB(p1.Row + p2.Row, p1.Col + p2.Col);
    }
    
    public static PB operator *(PB p1, int mul)
    {
        return new PB(p1.Row *mul , p1.Col * mul);
    }
}; //bleh...

public record PB3(BigInteger Row, BigInteger Col, BigInteger Z)
{
    public static implicit operator PB3(ValueTuple<BigInteger, BigInteger, BigInteger> tuple)
    {
        return new PB3(tuple.Item1, tuple.Item2, tuple.Item3);
    }

    public static PB3 operator -(PB3 p1, PB3 p2)
    {
        return new PB3(p1.Row - p2.Row, p1.Col - p2.Col, p1.Z - p2.Z);
    }

    public static PB3 operator +(PB3 p1, PB3 p2)
    {
        return new PB3(p1.Row + p2.Row, p1.Col + p2.Col, p1.Z + p2.Z);
    }
    
}; //bleh...

public record PDe(decimal Row, decimal Col)
{
    public static implicit operator PDe(ValueTuple<decimal, decimal> tuple)
    {
        return new PDe(tuple.Item1, tuple.Item2);
    }

    public static PDe operator -(PDe p1, PDe p2)
    {
        return new PDe(p1.Row - p2.Row, p1.Col - p2.Col);
    }

    public static PDe operator +(PDe p1, PDe p2)
    {
        return new PDe(p1.Row + p2.Row, p1.Col + p2.Col);
    }
    
    public static PDe operator +(PDe p1, P p2)
    {
        return new PDe(p1.Row + p2.Row, p1.Col + p2.Col);
    }
    
    public static PDe operator *(PDe p1, int mul)
    {
        return new PDe(p1.Row *mul , p1.Col * mul);
    }
}; //bleh...


public record PL(long Row, long Col)
{
    public static implicit operator PL(ValueTuple<long, long> tuple)
    {
        return new PL(tuple.Item1, tuple.Item2);
    }

    public static PL operator -(PL p1, PL p2)
    {
        return new PL(p1.Row - p2.Row, p1.Col - p2.Col);
    }

    public static PL operator +(PL p1, PL p2)
    {
        return new PL(p1.Row + p2.Row, p1.Col + p2.Col);
    }
    
    public static PL operator +(PL p1, P p2)
    {
        return new PL(p1.Row + p2.Row, p1.Col + p2.Col);
    }
    
    public static PL operator *(PL p1, int mul)
    {
        return new PL(p1.Row *mul , p1.Col * mul);
    }
}; //bleh...

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
    
    public static P operator *(P p1, int mul)
    {
        return new P(p1.Row *mul , p1.Col * mul);
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