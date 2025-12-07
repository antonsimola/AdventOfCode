using System.Text;
using AdventOfCodeLib;

namespace AdventOfCode2023;

public class Day25 : BaseDay
{
    record Edge(Node From, Node To);

    record Node(string Name);

    public override void Run()
    {
        // var input = Input;
        var input = AllTestInputs[1];
        var nodes = new Dictionary<string, Node>();
        var edges = new List<Edge>();
        foreach (var line in input)
        {
            var s = line.Split(": ");
            var from = s[0];
            var tos = s[1].Split(" ");
            nodes.GetOrAdd(from, (key) => new Node(key));
            foreach (var to in tos)
            {
                nodes.GetOrAdd(to, (key) => new Node(key));
            }
        }
        foreach (var line in input)
        {
            var s = line.Split(": ");
            var from = s[0];
            var tos = s[1].Split(" ");
            foreach (var to in tos)
            {
                edges.Add(new Edge(nodes[from], nodes[to]));
            }
        }

        Console.WriteLine(Print(edges));
        
    }

    string Print(IList<Edge> edges)
    {
        var sb = new StringBuilder();
        sb.AppendLine("graph {");
        foreach (var edge  in edges)
        {
            sb.AppendLine(edge.From.Name + " -- " + edge.To.Name);
        }
        sb.AppendLine("}");
        return sb.ToString();
    }
}