using System.Numerics;
using System.Text;
using AdventOfCodeLib;

namespace AdventOfCode2023;

public class Day20 : BaseDay
{
    public record Message(Module From, Module To, bool Flag)
    {
        public string Extra;
    };

    public abstract class Module(string Name)
    {
        public string Name { get; } = Name;
        public List<Module> InConnections { get; set; } = new List<Module>();
        public List<Module> OutConnections { get; set; } = new List<Module>();
        public List<string> OutConnectionsNames { get; set; } = new List<string>();
        public Queue<Message> SentMessages { get; set; } = new Queue<Message>();

        public abstract void Process(IList<Message> messages);

        public void Send(Module toModule, bool message, string extra = null)
        {
            SentMessages.Enqueue(new Message(this, toModule, message){Extra = extra});
        }

        public virtual void Init()
        {
        }

        public override string ToString()
        {
            return Name;
        }
    }


    public class Button(string Name) : Broadcaster(Name)
    {
    }

    public class Broadcaster(string Name) : Module(Name)
    {
        public override void Process(IList<Message> messages)
        {
            foreach (var o in OutConnections)
            {
                Send(o, false);
            }
        }
    }

    public class FlipFlop(string Name) : Module(Name)
    {
        private bool _state;


        public override void Process(IList<Message> messages)
        {
            if (!messages[0].Flag)
            {
                _state = !_state;
                foreach (var o in OutConnections)
                {
                    Send(o, _state);
                }
            }
        }
    }

    public class Conjunction(string Name) : Module(Name)
    {
        private IList<bool> Memory { get; set; } = new List<bool>();
        private IList<string> Search { get; set; } = new List<string>() {"bg","qq", "ls", "sj"};

        
        
        public override void Process(IList<Message> messages)
        {
            foreach (var m in messages)
            {
                var i = InConnections.FindIndex(e => e == m.From);
                Memory[i] = m.Flag;
            }


            var allSet = Memory.All(b => b);


            foreach (var o in OutConnections)
            {
                if (!allSet && Search.Contains(Name))
                {
                    Send(null, false, "found " + Name);
                }
                
                Send(o, !allSet);
            }
        }

        public override void Init()
        {
            base.Init();
            Memory = Enumerable.Range(0, InConnections.Count).Select(s => false).ToList();
        }
    }

    public class DevNull(string Name) : Module(Name)
    {
        public override void Process(IList<Message> messages)
        {
            if (!messages[0].Flag)
            {
                throw new Exception("FOUND! goto...");
            }
        }
    }


    public List<Module> MakeModules(string[] input)
    {
        var modules = new List<Module>();

        foreach (var line in input)
        {
            var sp = line.Split("->");

            var conName = sp[0].Trim();
            var connections = sp[1].Split(", ").Select(s => s.Trim()).ToList();

            if (conName == "broadcaster")
            {
                modules.Add(new Broadcaster(conName)
                {
                    OutConnectionsNames = connections
                });
            }
            else if (conName.StartsWith("%"))
            {
                modules.Add(new FlipFlop(conName.Substring(1))
                {
                    OutConnectionsNames = connections
                });
            }
            else if (conName.StartsWith("&"))
            {
                modules.Add(new Conjunction(conName.Substring(1))
                {
                    OutConnectionsNames = connections
                });
            }
        }

        modules.Add(new Button("button") { OutConnectionsNames = ["broadcaster"] });

        return modules;
    }

    public override void Run()
    {
        var input = Input;
        // var input = TestInput;

        var modules = MakeModules(input);
        var button = modules.LastOrDefault();
        PairModules(modules);

        Print(modules);
        
        
        // analyze the graph vis, four conjunctions that need to be on to make rx on...
        // take least common multiple from these
        // found bg 3739
        // found ls 3797
        // found sj 3919
        // found qq 4003
        
        
        // not a generic answer .... 
        
        
        Console.WriteLine(Helpers.LeastCommonMultiple([3739, 3797, 3919, 4003]));
        return;// uncomment to analyze the graph
        
        
        
        
        var devNulls = modules.SelectMany(m => m.OutConnections).Where(m => !modules.Contains(m)).ToList();

        modules.AddRange(devNulls);

        var totalLow = 0;
        var totalHigh = 0;
        long  i = 0;

        var moduleTable = modules.ToDictionary(m => m, m => m);
        try
        {
            var queue = new Queue<Message>();
            while (true)
            {
             
                button.Process([new Message(null, null, false)]);
                i++;
                AddToGlobalQueue(button, queue, i);


                while (queue.TryDequeue(out var m))
                {
                    if (m.Flag)
                    {
                        totalLow++;
                    }
                    else
                    {
                        totalHigh++;
                    }

                    var receiver = moduleTable[m.To];
                    receiver.Process([m]);
                    AddToGlobalQueue(receiver, queue, i);
                }
            }
        }
        catch (Exception)
        {
            Console.WriteLine(i);
        }


        Console.WriteLine(totalHigh * totalLow);
    }

    private void Print(List<Module> modules)
    {
        var sb = new StringBuilder();
        foreach (var module in modules)
        {

            sb.Append(module.Name).Append("->").Append(string.Join(",", module.OutConnectionsNames));
            sb.AppendLine();
            sb.AppendLine(module.Name + " [shape="+  (module is FlipFlop ? "Mdiamond" : module is Conjunction ? "Msquare" :    "circle") + "]" );
            
        }

        Console.WriteLine(sb.ToString());
    }

    private void AddToGlobalQueue(Module receiver, Queue<Message> queue, long i)
    {
        while (receiver.SentMessages.TryDequeue(out var m))
        {
            if (m.Extra != null)
            {
                Console.WriteLine(m.Extra + " " + i);
                continue;
            }
            queue.Enqueue(m);
        }
            
    }

    private static void PairModules(IList<Module> modules)
    {
        foreach (var module in modules)
        {
            foreach (var outName in module.OutConnectionsNames)
            {
                var mTo = modules.FirstOrDefault(m => m.Name == outName);

                if (mTo == null)
                {
                    mTo = new DevNull(outName);
                }

                mTo.InConnections.Add(module);
                module.OutConnections.Add(mTo);
            }
        }

        foreach (var m in modules)
        {
            m.Init();
        }
    }
}