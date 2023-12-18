using System.Runtime.InteropServices.ComTypes;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics.Optimization;
using MathNet.Spatial.Euclidean;
using static AdventOfCode2023.Direction;

namespace AdventOfCode2023;

using static GridMovement;

public class Day18 : BaseDay
{
    public override void Run()
    {
        checked
        {
            var input = Input;
        // var input = TestInput;


        var curPoint = new PL(0, 0);
        var curDir = South;
        var polygon = new List<Point2D>() { };
        var points = new List<PL>();
        long perimeter = 0;
        foreach (var line in input)
        {
            var s = line.Split(" ");
            // var dir = s[0] switch { "R" => East, "D" => South, "L" => West, "U" => North };
            // var amount = int.Parse(s[1]);
            var color = s[2].Replace("(#", "").Replace(")", "");

            var dist = color.Substring(0, 5);
            var dirC = color.LastOrDefault();
            var dir = dirC switch { '0' => East, '1' => South, '2' => West, '3' => North };
            var amount = Convert.ToInt32("0x"+dist, 16);


            var shift = Shifts[dir];
            curPoint += shift * amount;
            polygon.Add(new Point2D(curPoint.Row, curPoint.Col));
            points.Add(curPoint);

            perimeter += amount;
        }


        //brute force
        // var p = new Polygon2D(polygon);

        // var rowMin = polygon.Min(p => p.X);
        // var rowMax = polygon.Max(p => p.X);
        // var colMin = polygon.Min(p => p.Y);
        // var colMax = polygon.Max(p => p.Y);
        //
        // var area = 0;
        // var outside = 0;
        // for (var r = rowMin; r <= rowMax; r++)
        // {
        //     for (var c = colMin; c <= colMax; c++)
        //     {
        //         //Console.SetCursorPosition((int)c, (int)r);
        //         if (p.EnclosesPoint(new Point2D(r-0.5, c - 0.5)) &&
        //             p.EnclosesPoint(new Point2D(r+0.5, c + 0.5)) &&
        //             p.EnclosesPoint(new Point2D(r+0.5, c - 0.5)) &&
        //             p.EnclosesPoint(new Point2D(r-0.5, c + 0.5)))
        //         {
        //             area += 1;
        //         }
        //         else
        //         {
        //             outside += 1;
        //         }
        //     }
        // }

        var shoelace = Helpers.PolygonAreaShoelace(points);
        var a = Helpers.PolygonPickTheoremInnerPointCount(perimeter, shoelace);
        Console.WriteLine("shoelace " + shoelace);
        Console.WriteLine("Polygon grid area a " + a);
        Console.WriteLine("Perimeter " + perimeter);
        Console.WriteLine("turns " + input.Length);
        Console.WriteLine();
        Console.WriteLine(perimeter + a);

        }
        
        
    }
}