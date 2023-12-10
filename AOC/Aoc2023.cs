using System.Collections.Concurrent;
using System.Text;
using System.Text.RegularExpressions;
using Combinatorics.Collections;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace AOC;

public class Aoc2023
{
    public string[] TestInput;
    public List<string[]> AllTestInputs;
    public string[] Input;

    [SetUp]
    public void Setup()
    {
        var testName = TestContext.CurrentContext.Test.Name;
        if (!testName.Contains("Day"))
        {
            return;
        }

        var day = int.Parse(testName.Replace("Day", ""));
        var question = LoadFile(day, "test.txt");
        var scraped = ScrapeTests(question);
        TestInput = scraped[0];
        AllTestInputs = scraped;
        Input = LoadFile(day, "input.txt");
    }

    private List<string[]> ScrapeTests(string[] testFileContents)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(string.Join("\n", testFileContents));

        var codeNodes = doc.DocumentNode.QuerySelectorAll("pre>code");
        var innerTexts = codeNodes
            .Select(n => n.InnerText)
            .Select(t => t.Split("\n"))
            .Select(splitted => splitted.LastOrDefault() == "" ? splitted[..^1] : splitted)
            .ToList();

        return innerTexts;
    }

    private string DownloadAocUrl(string url)
    {
        using var httpClient = new HttpClient();
        var session = Environment.GetEnvironmentVariable("AOC_SESSION", EnvironmentVariableTarget.User);
        httpClient.DefaultRequestHeaders.Add("Cookie", "session=" + session);
        return httpClient.GetStringAsync(url).Result;
    }

    private string DownloadInput(int year, int day)
    {
        var url = $"https://adventofcode.com/{year}/day/{day}/input";
        return DownloadAocUrl(url);
    }


    private string DownloadQuestion(int year, int day)
    {
        var url = $"https://adventofcode.com/{year}/day/{day}";
        return DownloadAocUrl(url);
    }


    private string[] LoadFile(int day, string fileName)
    {
        var year = 2023;
        var p = Path.Combine(TestContext.CurrentContext.TestDirectory, "input", year + "", day + "", fileName);
        if (!File.Exists(p) && fileName == "input.txt")
        {
            var content = DownloadInput(year, day);
            File.WriteAllText(p, content);
        }

        if (!File.Exists(p) && fileName == "test.txt")
        {
            if (!Directory.Exists(Path.GetDirectoryName(p)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(p));
            }

            var question = DownloadQuestion(year, day);
            File.WriteAllText(p, question);
        }

        return File.ReadAllLines(p);
    }


    private void WriteLine(object o)
    {
        TestContext.Out.WriteLine(o);
    }

    private void Write(object o)
    {
        TestContext.Out.Write(o);
    }


    //[Test]
    public void CodeGen()
    {
        for (var i = 1; i < 25; i++)
        {
            WriteLine($$"""
                        [Test]
                         public void Day{{i}}()
                         {
                                var input = Input;
                                //var input = TestInput;
                         }
                         
                        """);
        }


        Assert.Pass();
    }

    [Test]
    public void Day1()
    {
        var input = Input;
        //var input = TestInput;
        var text = input;

        var stringValues = new Dictionary<Regex, int>()
        {
            { new Regex("one"), 1 },
            { new Regex("two"), 2 },
            { new Regex("three"), 3 },
            { new Regex("four"), 4 },
            { new Regex("five"), 5 },
            { new Regex("six"), 6 },
            { new Regex("seven"), 7 },
            { new Regex("eight"), 8 },
            { new Regex("nine"), 9 },
            { new Regex("1"), 1 },
            { new Regex("2"), 2 },
            { new Regex("3"), 3 },
            { new Regex("4"), 4 },
            { new Regex("5"), 5 },
            { new Regex("6"), 6 },
            { new Regex("7"), 7 },
            { new Regex("8"), 8 },
            { new Regex("9"), 9 },
        };
        var sum = 0;
        foreach (var line in text)
        {
            var min = int.MaxValue;
            var minVal = 0;
            var max = -1;
            var maxVal = 0;
            foreach (var (reg, val) in stringValues)
            {
                foreach (Match match in reg.Matches(line))
                {
                    var i = match.Index;
                    if (i < min)
                    {
                        min = i;
                        minVal = val;
                    }

                    if (i > max)
                    {
                        max = i;
                        maxVal = val;
                    }
                }
            }

            sum += int.Parse(minVal + "" + maxVal);
        }


        WriteLine(sum);
    }

    [Test]
    public void Day2()
    {
        var input = Input;
        //var input = TestInput;
        var text = input;
        var sum = 0;
        foreach (var line in text)
        {
            var a = line.Split(":");
            var id = int.Parse(a[0].Split(" ")[1]);
            var shows = a[1].Trim().Split(";");
            var possible = true;
            var minRed = 0;
            var minBlue = 0;
            var minGreen = 0;
            foreach (var show in shows)
            {
                foreach (var cubeCount in show.Split(","))
                {
                    var colorCount = cubeCount.Trim().Split(" ");
                    var count = int.Parse(colorCount[0].Trim());
                    var color = colorCount[1].Trim();

                    if (color == "blue" && count > minBlue)
                    {
                        minBlue = count;
                    }
                    else if (color == "red" && count > minRed)
                    {
                        minRed = count;
                    }
                    else if (color == "green" && count > minGreen)
                    {
                        minGreen = count;
                    }
                }
            }

            if (possible)
            {
                sum += minBlue * minRed * minGreen;
            }
        }

        WriteLine(sum);
    }

    record FoundNumber(int Row, int Index, int Length, int Number);

    record SpecialCharacter(int Row, int Index, int Length, string Value);

    [Test]
    public void Day3()
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

    record Card(int Id, HashSet<int> Marked, HashSet<int> Winners);

    [Test]
    public void Day4()
    {
        var input = Input;
        //var input = TestInput;
        var grandSum = 0;
        var initialCards = new List<Card>();
        var copyCards = new ConcurrentDictionary<int, List<Card>>();

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

    record Range(long SourceStart, long DestStart, long Length);

    record Map(string From, string To, List<Range> Ranges);


    [Test]
    [Ignore("It is slow, run manually")]
    public void Day5()
    {
        // var input = Input;
        var input = TestInput;

        var seeds = input[0].Split(": ")[1].Split(" ").Select(long.Parse).ToList();


        var ranges = new List<ValueTuple<long, long>>();

        for (int i = 0; i < seeds.Count; i += 2)
        {
            ranges.Add((seeds[i], seeds[i + 1]));
        }

        var max = ranges.Max(r => r.Item1 + r.Item2);
        var min = ranges.Min(r => r.Item1);
        WriteLine(min);
        WriteLine(max);
        WriteLine(max - min);

        Map mapToCollect = null;
        List<Map> maps = new List<Map>();

        foreach (var line in input.Skip(1))
        {
            if (line.Length == 0)
            {
                continue;
            }

            if (char.IsLetter(line[0]))
            {
                var parts = line.Split(" ")[0].Split("-");
                var from = parts[0];
                var to = parts[2];
                mapToCollect = new Map(from, to, new List<Range>());
                maps.Add(mapToCollect);
            }
            else if (mapToCollect != null)
            {
                var range = line.Split(" ").Select(long.Parse).ToList();
                mapToCollect.Ranges.Add(new Range(range[1], range[0], range[2]));
            }
        }

        var globalMin = ranges.AsParallel().Select((seedrange, i) =>
        {
            var min = long.MaxValue;
            var size = seedrange.Item2;
            var counter = 0;
            for (var seed = seedrange.Item1; seed < seedrange.Item1 + seedrange.Item2; seed++)
            {
                var cur = seed;
                foreach (var map in maps)
                {
                    var found = false;
                    foreach (var range in map.Ranges)
                    {
                        if (found)
                        {
                            break;
                        }

                        var rangeStart = range.SourceStart;
                        var rangeEnd = range.SourceStart + range.Length;

                        if (rangeStart <= cur && cur <= rangeEnd)
                        {
                            found = true;
                            cur = range.DestStart + cur - rangeStart;
                        }
                    }
                }

                min = Math.Min(cur, min);
                if (counter % 100_0000 == 0)
                {
                    WriteLine($"Progress {i} {+((double)counter / (double)size) * 100} % ");
                }

                counter++;
            }

            return min;
        }).Min();


        WriteLine(globalMin);
    }

    [Test]
    public void Day6()
    {
        var input = Input;
        //var input = TestInput;
        long[] times =
        [
            long.Parse(input[0].Split(":")[1]
                .Replace(" ", "")
            )
        ];
        long[] records =
        [
            long.Parse(input[1].Split(":")[1]
                .Replace(" ", "")
            )
        ];


        long i = 0;

        var beats = new List<long>();
        foreach (var time in times)
        {
            var recordBeats = 0;
            for (var j = 0; j < time; j++)
            {
                var chargeTime = j;
                var moveTime = time - chargeTime;
                var distance = moveTime * chargeTime;
                if (distance > records[i])
                {
                    recordBeats++;
                }
            }

            beats.Add(recordBeats);
            i++;
        }


        WriteLine(beats.Aggregate((a, b) => a * b));
    }

    record Eval(Func<int[], bool> eval, string hand, string originalHand, int points);


    [Test]
    public void Day7()
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

    record Node(Node Left, Node Right, string l, string r, string Cur)
    {
        public Node Left { get; set; } = Left;
        public Node Right { get; set; } = Right;
        public string Cur { get; set; } = Cur;
    }

    [Test]
    public void Day8()
    {
        var input = Input;
        //var input = TestInput;

        var instructions = input[0];


        var nodes = new List<Node>();

        foreach (var l in input.Skip(2))
        {
            var s = l.Split(" = ");
            var nexts = s[1].Split(", ");
            var left = nexts[0].Substring(1, 3);
            var right = nexts[1].Substring(0, 3);
            var cur = s[0];
            var n = new Node(null, null, left, right, cur);
            nodes.Add(n);
        }

        foreach (var node in nodes)
        {
            node.Left = nodes.FirstOrDefault(n => n.Cur == node.l);
            node.Right = nodes.FirstOrDefault(n => n.Cur == node.r);
        }


        var currentNodes = nodes
            .Select(kv => kv.Cur.EndsWith("A") ? kv : null)
            .Where(c => c != null)
            .ToList();


        var mins = new List<long>();

        foreach (var curNode in currentNodes)
        {
            var count = 0;
            var cNode = curNode;
            while (true)
            {
                foreach (var i in instructions)
                {
                    count++;
                    var everyZ = true;

                    var n = i == 'L' ? cNode.Left : cNode.Right;
                    if (!n.Cur.EndsWith("Z"))
                    {
                        everyZ = false;
                    }

                    if (everyZ)
                    {
                        goto o;
                    }

                    cNode = n;

                    if (count % 10_000_000 == 0)
                    {
                        Console.Write($"Count : {count} ");
                        WriteLine(string.Join(" ", currentNodes.Select(c => c.Cur)));
                    }
                }
            }

            o:
            mins.Add(count);
        }

        WriteLine(LeastCommonMultiple(mins));
    }

    static long LeastCommonMultiple(List<long> args)
    {
        return args.Aggregate(LeastCommonMultiple);
    }


    static long GreatestCommonDivisor(long a, long b)
    {
        if (a == 0)
            return b;
        return GreatestCommonDivisor(b % a, a);
    }

    static long LeastCommonMultiple(long a, long b)
    {
        return (a / GreatestCommonDivisor(a, b)) * b;
    }

    [Test]
    public void Day9()
    {
        var input = Input;
        //var input = TestInput;
        var seqs = new List<List<int>>();
        foreach (var line in input)
        {
            seqs.Add(line.ParseIntList());
        }

        var sum = 0;
        foreach (var seq in seqs)
        {
            var result = new List<List<int>>() { seq };
            GetDiffsRec(seq, result);

            result.Reverse();
            result.First().Insert(0, 0);

            var i = 0;
            foreach (var first in result)
            {
                if (i + 1 < result.Count)
                {
                    var next = result[i + 1];
                    var l = next.First();
                    next.Insert(0, l - first.First());
                }

                i++;
            }

            sum += result.Last().First();
        }

        WriteLine(sum);
    }

    public static List<List<int>> GetDiffsRec(List<int> seq, List<List<int>> result)
    {
        var diffs = seq.Skip(1).Zip(seq).Select(pair => pair.First - pair.Second).ToList();
        result.Add(diffs);
        if (diffs.All(e => e == 0))
        {
            return result;
        }

        return GetDiffsRec(diffs, result);
    }


    [Test]
    public void Day10()
    {
        var input = Input;

        // new Day10().Run(input);

        new Day10().Run(AllTestInputs[9]);
        new Day10().Run(AllTestInputs[12]);
        new Day10().Run(AllTestInputs[14]);
         new Day10().Run(AllTestInputs[AllTestInputs.Count -2]);
        //var input = TestInput;
    }

    [Test]
    public void Day11()
    {
        var input = Input;
        //var input = TestInput;
    }

    [Test]
    public void Day12()
    {
        var input = Input;
        //var input = TestInput;
    }

    [Test]
    public void Day13()
    {
        var input = Input;
        //var input = TestInput;
    }

    [Test]
    public void Day14()
    {
        var input = Input;
        //var input = TestInput;
    }

    [Test]
    public void Day15()
    {
        var input = Input;
        //var input = TestInput;
    }

    [Test]
    public void Day16()
    {
        var input = Input;
        //var input = TestInput;
    }

    [Test]
    public void Day17()
    {
        var input = Input;
        //var input = TestInput;
    }

    [Test]
    public void Day18()
    {
        var input = Input;
        //var input = TestInput;
    }

    [Test]
    public void Day19()
    {
        var input = Input;
        //var input = TestInput;
    }

    [Test]
    public void Day20()
    {
        var input = Input;
        //var input = TestInput;
    }

    [Test]
    public void Day21()
    {
        var input = Input;
        //var input = TestInput;
    }

    [Test]
    public void Day22()
    {
        var input = Input;
        //var input = TestInput;
    }

    [Test]
    public void Day23()
    {
        var input = Input;
        //var input = TestInput;
    }

    [Test]
    public void Day24()
    {
        var input = Input;
        //var input = TestInput;
    }
}