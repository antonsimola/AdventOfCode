namespace AOC;

public class Day6 : BaseDay
{
    public override void Run()
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
}