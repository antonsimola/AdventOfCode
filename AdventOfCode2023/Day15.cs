using AdventOfCodeLib;
using MathNet.Numerics.LinearAlgebra;

namespace AdventOfCode2023;

public class Day15 : BaseDay
{
    record KV(string Label, int Value);

    class Bucket : List<KV>
    {
    }

    public override void Run()
    {
        var input = Input[0];
        // var input = TestInput[0];
        var checks = input.Split(",");
        // var input = "HASH";


        var hashMap = new Bucket[256];

        var x = 0;
        foreach (var b in hashMap)
        {
            hashMap[x++] = new Bucket();
        }

        foreach (var check in checks)
        {
            var q = new Queue<char>(check);
            var label = "";
            char operation = '1';
            while (q.TryDequeue(out var p))
            {
                if (p != '=' && p != '-')
                {
                    label += p;
                }
                else
                {
                    operation = p;
                    break;
                }
            }

            var iCheck = 0;
            var ca = "";
            if (operation == '=')
            {
                while (q.TryDequeue(out var p))
                {
                    ca += p;
                }

                iCheck = int.Parse(ca);
            }

            var hash = 0;
            foreach (var c in label)
            {
                var i = (int)c;
                hash += i;
                hash = hash * 17;
                hash = hash % 256;
            }

            var bucket = hashMap[hash];

            if (bucket == null)
            {
                bucket = new Bucket();
                hashMap[hash] = bucket;
            }

            if (operation == '-')
            {
                var existingInd = bucket.FindIndex(b => b.Label == label);
                if (existingInd > -1)
                {
                    bucket.RemoveAt(existingInd);
                }
            }
            else
            {
                var existingInd = bucket.FindIndex(b => b.Label == label);
                if (existingInd > -1)
                {
                    bucket[existingInd] = new KV(label, iCheck);
                }
                else
                {
                    bucket.Add(new KV(label, iCheck));
                }
            }
        }

        var boxn = 0;
        foreach (var b in hashMap)
        {
            if (b.Count > 0)
            {
                Console.WriteLine($"{boxn} {string.Join(",", b)}");
            }

            boxn++;
        }

        var id = 1;
        var sum = 0;

        var result = new Dictionary<int, int>();
        foreach (var b in hashMap)
        {
            var j = 1;
            foreach (var kv in b)
            {
                sum += kv.Value * id * j;
                j++;
            }

            id++;
        }

        Console.WriteLine(sum);
    }
}