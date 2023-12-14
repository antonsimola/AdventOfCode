using System.Text;

namespace AdventOfCode2023;

public class Day12 : BaseDay
{
    private Dictionary<string, ICollection<IList<char>>> Cache = new ();
    
    //TODO brute force, takes years on full input. Memoization? Split into unknown parts and known parts
    public override void Run()
    {
        // var input = Input;
        var input = AllTestInputs[1];
        var grandSum = 0;
        var progress = 0;
        foreach (var line in input)
        {
            var split = line.Split(" ");
            // var records = split[0];
            var records = string.Join("?", Enumerable.Range(0, 5).Select(i => split[0]));
            var checks = split[1].ParseIntList(",");
            checks = Enumerable.Range(0, 5).Select(i => checks).Aggregate((a, b) => a.Concat(b).ToList());


            var grouped = GroupIntoKnownsAndUnknowns(records);

            var checksCount = checks.Sum();

            var allRecords = Generate(records).ToList();


            Console.WriteLine((double)progress / input.Length);

            var sum = 0;
            // Console.WriteLine("records : " + records);
            foreach (var r in allRecords)
            {
                var checksQ = new Queue<int>(checks);
                var ok = true;
                for (var i = 0; i < r.Length; i++)
                {
                    var c = r[i];
                    if (c == '.')
                    {
                        continue;
                    }

                    if (c == '#')
                    {
                        if (!checksQ.TryDequeue(out var checking))
                        {
                            ok = false;
                            break;
                        }

                        var brokenCount = 0;
                        while (i < r.Length && r[i] == '#')
                        {
                            brokenCount++;
                            i++;
                        }

                        if (brokenCount != checking)
                        {
                            ok = false;
                        }
                    }
                }

                if (ok && checksQ.Count == 0)
                {
                    // Console.WriteLine("record OK : " + r);
                    sum += 1;
                }
            }

            grandSum += sum;
            Console.WriteLine("Done this round");

            progress++;
        }

        Console.WriteLine("--------");
        Console.WriteLine(grandSum);
    }

    private List<string> GroupIntoKnownsAndUnknowns(string records)
    {
        var parts = new List<string>();
        var currentPart = "";
        char prev = records[0];
        foreach (var c in records.Skip(1))
        {
            if (prev == '?' && c != '?')
            {
                parts.Add(currentPart);
                currentPart = "";
            }
            else if (prev != '?' && c == '?')
            {
                parts.Add(currentPart);
                currentPart = "";
            }

            prev = c;
            currentPart += c;
        }


        return parts;
    }


    public IEnumerable<string> Generate(string s)
    {
        var jokerLocations = new List<int>();
        var i = 0;

        foreach (var card in s)
        {
            if (card == '?')
            {
                jokerLocations.Add(i);
            }

            i++;
        }

        char[] chars = ['#', '.'];
        var res = new List<string>();
        var combines = chars.CombineWithRepetitions(jokerLocations.Count, Cache);

        foreach (var set in combines)
        {
            var h = new StringBuilder(s);
            var j = 0;
            foreach (var loc in jokerLocations)
            {
                h[loc] = set[j++];
            }

            res.Add(h.ToString());
        }

        return res;
    }
}