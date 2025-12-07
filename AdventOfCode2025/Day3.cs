using System.Numerics;
using AdventOfCodeLib;

namespace AdventOfCode2025;

public class Day3 : BaseDay
{
    public override void Run()
    {
        var input = Input;
        // var input = TestInput;

        List<List<int>> banks = input.Select(i => i.Select(c => int.Parse(c + "")).ToList()).ToList();

        var sum = new BigInteger(0);

        foreach (var bank in banks)
        {
            var numStr = "";
            var currentPos = 0;
            for (var j = 11; j >= 0; j--)
            {
                var allExceptLast = bank[currentPos..^j];
                var firstDigit = allExceptLast.Max();
                var indexOfFirst = bank.IndexOf(firstDigit, currentPos);
                currentPos = indexOfFirst + 1;
                numStr += firstDigit + "";
                //Console.WriteLine(num);
            }

            Console.WriteLine(numStr);
            sum +=  BigInteger.Parse(numStr);
        }

        Console.WriteLine(sum);
    }
    
}