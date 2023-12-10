using System.Text;
using Combinatorics.Collections;

namespace AdventOfCode2023;

public class Day7 : BaseDay
{
    record Eval(Func<int[], bool> eval, string hand, string originalHand, int points);

    public override void Run()
    {
        var input = Input;
        //var input = TestInput;
        var hands = new List<Func<int[], bool>>()
        {
            counts => counts.Any(c => c == 5),
            counts => counts.Any(c => c == 4),
            counts => counts.Select(c => c switch
            {
                2 => 2,
                3 => 3,
                _ => 0
            }).Sum() == 5,
            counts => counts.Any(c => c == 3),
            counts => counts.Count(c => c == 2) == 2,
            counts => counts.Count(c => c == 2) == 1,
            _ => true
        };
        var evals = new List<Eval>();

        foreach (var line in input)
        {
            var parts = line.Split(" ");
            var originalHand = parts[0];
            var points = int.Parse(parts[1]);
            var localEvals = new List<Eval>();
            foreach (var hand in MakePossibleHands(originalHand))
            {
                foreach (var h in hands)
                {
                    var counts = hand.GroupBy(c => c).Select(c => c.Count()).ToArray();
                    if (h(counts))
                    {
                        localEvals.Add(new Eval(h, hand, originalHand, points));
                        break;
                    }
                }
            }

            var best = localEvals
                .OrderByDescending(e => hands.IndexOf(e.eval))
                .ThenBy(Tiebreaker).LastOrDefault();
            evals.Add(best);
        }


        WriteLine(
            evals
                .OrderByDescending(e => hands.IndexOf(e.eval))
                .ThenBy(Tiebreaker)
                .Select((eval, i) => eval.points * (i + 1))
                .Sum()
        );
    }

    public static List<string> MakePossibleHands(string hand)
    {
        var ranks = "23456789TQKA";
        var combinations = new List<string>();
        var jokerLocations = new List<int>();
        var i = 0;

        foreach (var card in hand)
        {
            if (card == 'J')
            {
                jokerLocations.Add(i);
            }

            i++;
        }

        var c = new Combinations<char>(ranks.Select(c => c), jokerLocations.Count(), GenerateOption.WithRepetition);

        foreach (var set in c)
        {
            var h = new StringBuilder(hand);
            var j = 0;
            foreach (var loc in jokerLocations)
            {
                h[loc] = set[j++];
            }

            combinations.Add(h.ToString());
        }

        return combinations;
    }


    private static string Tiebreaker(Eval h1)
    {
        var ranks = "J23456789TQKA";
        var sorts = "ABCDEFGHIJKLM";


        string res = "";
        foreach (var card in h1.originalHand)
        {
            var i = ranks.IndexOf(card);
            res += sorts[i];
        }

        return res;
    }
}