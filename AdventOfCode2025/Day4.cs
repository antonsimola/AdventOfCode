using AdventOfCodeLib;
using static AdventOfCodeLib.GridMovement;

namespace AdventOfCode2025;

public class Day4 : BaseDay
{
    public override void Run()
    {
        var input = Input;
        // var input = TestInput;

        char[][] arr = input.Select(i => i.ToArray()).ToArray();

        var totalCount = 0;
        while (true)
        {
            var listRemoval = new List<P>();
            for (int row = 0; row < arr.Length; row++)
            {
                for (int col = 0; col < arr[0].Length; col++)
                {
                    var sumPaper = 0;
                    if (arr[row][col] != '@') continue;
                    sumPaper += SafeGet(arr, new P(row - 1, col - 1), '.') == '@' ? 1 : 0;
                    sumPaper += SafeGet(arr, new P(row - 1, col), '.') == '@' ? 1 : 0;
                    sumPaper += SafeGet(arr, new P(row - 1, col + 1), '.') == '@' ? 1 : 0;
                    sumPaper += SafeGet(arr, new P(row, col - 1), '.') == '@' ? 1 : 0;
                    sumPaper += SafeGet(arr, new P(row, col + 1), '.') == '@' ? 1 : 0;
                    sumPaper += SafeGet(arr, new P(row + 1, col - 1), '.') == '@' ? 1 : 0;
                    sumPaper += SafeGet(arr, new P(row + 1, col), '.') == '@' ? 1 : 0;
                    sumPaper += SafeGet(arr, new P(row + 1, col + 1), '.') == '@' ? 1 : 0;
                    if (sumPaper < 4)
                    {
                        listRemoval.Add(new P(row, col));
                    }
                }
            }

            if (listRemoval.Count == 0)
            {
                break;
            }

            totalCount += listRemoval.Count;
            foreach (var remove in listRemoval)
            {
                arr[remove.Row][remove.Col] = '.';
            }
        }


        Console.WriteLine(totalCount);
    }
}