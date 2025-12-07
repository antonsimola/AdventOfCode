using System.Collections.Concurrent;
using AdventOfCodeLib;

namespace AdventOfCode2023;

public class Day4 : BaseDay
{
    record Card(int Id, HashSet<int> Marked, HashSet<int> Winners);


    public override void Run()
    {
        var input = Input;
        //var input = TestInput;
        var grandSum = 0;
        var initialCards = new List<Card>();
        var copyCards = new Dictionary<int, List<Card>>();
        
        
        foreach (var line in input)
        {
            var split = line.Split(":");
            var id = int.Parse(split[0].Replace("   ", " ").Replace("  ", " ").Split(" ")[1]) - 1; // 0 based
            var a = split[1].Split("|");
            var b = a[0].Trim().Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => int.Parse(s))
                .ToHashSet();
            var c = a[1].Trim().Split(" ").Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => int.Parse(s))
                .ToHashSet();

            initialCards.Add(new Card(id, b, c));
        }

        foreach (var card in initialCards)
        {
            var nextId = card.Id + 1;
            foreach (var w in card.Winners)
            {
                if (card.Marked.Contains(w))
                {
                    var cardToClone = initialCards[nextId];
                    var list = copyCards.GetOrAdd(nextId, (i) => new List<Card>());
                    list.Add(cardToClone);
                    nextId++;
                }
            }

            copyCards.TryGetValue(card.Id, out var cloned);

            foreach (var c in cloned ?? new List<Card>())
            {
                nextId = c.Id + 1;
                foreach (var w in c.Winners)
                {
                    if (card.Marked.Contains(w))
                    {
                        var cardToClone = initialCards[nextId];
                        var list = copyCards.GetOrAdd(nextId, (i) => new List<Card>());
                        list.Add(cardToClone);
                        nextId++;
                    }
                }
            }
        }

        WriteLine(copyCards.Select(kv => kv.Value.Count).Sum() + initialCards.Count);
    }
}