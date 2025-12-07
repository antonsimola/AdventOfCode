using System.Text.RegularExpressions;
using AdventOfCodeLib;

namespace AdventOfCode2023;

public class Day3 : BaseDay
{
    record FoundNumber(int Row, int Index, int Length, int Number);

    record SpecialCharacter(int Row, int Index, int Length, string Value);

    public override void Run()
    {
        var input = Input;
        //var input = TestInput;
        var d = 0;
        var numbers = new List<FoundNumber>();
        var specials = new List<SpecialCharacter>();
        var gears = new List<SpecialCharacter>();

        var digitMatcher = new Regex(@"\d+");

        var rowNum = 0;
        foreach (var line in input)
        {
            foreach (Match match in digitMatcher.Matches(line))
            {
                var index = match.Index;
                var length = match.Length;
                var value = int.Parse(match.Value);
                numbers.Add(new FoundNumber(rowNum, index, length, value));
            }

            var j = 0;
            foreach (var c in line)
            {
                if (!Char.IsDigit(c) && c != '.')
                {
                    specials.Add(new SpecialCharacter(rowNum, j, 1, c + ""));
                }

                if (c == '*')
                {
                    gears.Add(new SpecialCharacter(rowNum, j, 1, c + ""));
                }

                j++;
            }

            rowNum++;
        }

        var sum = 0;
        foreach (var number in numbers)
        {
            var start = number.Index;
            var end = start + number.Length - 1;
            var bbXStart = start - 1;
            var bbYStart = number.Row - 1;
            var bbXEnd = end + 1;
            var bbYEnd = number.Row + 1;
            foreach (var special in specials)
            {
                var s = special.Index;
                var r = special.Row;
                if (bbXStart <= s && s <= bbXEnd
                                  && bbYStart <= r && r <= bbYEnd
                   )
                {
                    sum += number.Number;
                    break;
                }
            }
        }

        //Part 2
        var gearRatio = 0;
        foreach (var gear in gears)
        {
            var s = gear.Index;
            var r = gear.Row;
            var touchedNumbers = new List<int>();
            foreach (var number in numbers)
            {
                var start = number.Index;
                var end = start + number.Length - 1;
                var bbXStart = start - 1;
                var bbYStart = number.Row - 1;
                var bbXEnd = end + 1;
                var bbYEnd = number.Row + 1;
                if (bbXStart <= s && s <= bbXEnd
                                  && bbYStart <= r && r <= bbYEnd
                   )
                {
                    touchedNumbers.Add(number.Number);
                }
            }

            if (touchedNumbers.Count == 2)
            {
                gearRatio += touchedNumbers[0] * touchedNumbers[1];
            }
        }

        WriteLine(sum);
        WriteLine(gearRatio);
    }
}