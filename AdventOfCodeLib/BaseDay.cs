using System.Reflection;
using HtmlAgilityPack;
using HtmlAgilityPack.CssSelectors.NetCore;

namespace AdventOfCodeLib;

public abstract class BaseDay
{
    public string[] TestInput;
    public List<string[]> AllTestInputs;
    public string[] Input;
    public static int Year { get; set; }

    public abstract void Run();


    public void WriteLine(object o)
    {
        Console.WriteLine(o);
    }

    public void Setup()
    {
        var testName = this.GetType().Name;
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
            .Select(HtmlEntity.DeEntitize)
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
        var year = Year;
        var loc = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var p = Path.Combine(loc, "input", year + "", day + "", fileName);
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
}