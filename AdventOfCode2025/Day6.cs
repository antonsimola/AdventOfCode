using System.Globalization;
using System.Numerics;
using AdventOfCodeLib;

namespace AdventOfCode2025;

public class Day6 : BaseDay
{
    public override void Run()
    {
        var input = Input;
        // var input = TestInput;

        var arr = input.Select(i => i.ToCharArray()).ToArray();

        var numbers = new List<List<BigInteger>>();
        var operations = new List<bool>();

        var currentProblem = new List<BigInteger>();
        for (var c = arr[0].Length - 1; c >= 0; c--)
        {
            var currentNumber = "";
            bool? operation = null;
            for (var r = 0; r < arr.Length; r++)
            {
                var ch = arr[r][c];
                if (ch == '+')
                {
                    operation = false;
                }
                else if (ch == '*')
                {
                    operation = true;
                }
                else if (ch == ' ')
                {
                }
                else
                {
                    currentNumber += ch;
                }
            }

            currentProblem.Add(BigInteger.Parse(currentNumber));
            if (operation != null)
            {
                operations.Add(operation.Value);
                c--;
                numbers.Add(currentProblem);
                currentProblem = new List<BigInteger>();
            }
        }


        var sum = new BigInteger();
        foreach (var pairs in numbers.Zip(operations))
        {
            if (pairs.Second)
            {
                sum += pairs.First.Aggregate((acc, cur) => acc * cur);
            }
            else
            {
                sum += pairs.First.Aggregate((acc, cur) => acc + cur);
            }
        }


        Console.WriteLine(sum);
    }
}