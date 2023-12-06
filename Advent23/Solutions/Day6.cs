namespace Advent23.Solutions;

public class Day6 : BaseDay
{
    public Day6() : base(6)
    {
    }

    public override void Run()
    {
        Console.WriteLine("--- PART 1 ---");
        var durations = ShrinkSpaces(InputLines[0]).Split(":")[1].Trim().Split(" ").Select(int.Parse).ToList();
        var targets = ShrinkSpaces(InputLines[1]).Split(":")[1].Trim().Split(" ").Select(int.Parse).ToList();
        var races = targets.Select((t, i) => new Race { Duration = durations[i], Target = t }).ToList();

        var winPossibilities = races.Select(RunRace).ToList();
        var result = winPossibilities.Aggregate((long)1, (a, b) => a * b);
        Console.WriteLine(result);

        Console.WriteLine("--- PART 2 ---");
        var duration = long.Parse(InputLines[0].Replace(" ", "").Split(":")[1].Trim());
        var target = long.Parse(InputLines[1].Replace(" ", "").Split(":")[1].Trim());
        var race = new Race { Duration = duration, Target = target };

        var fullResult = RunRace(race);
        Console.WriteLine(fullResult);
    }

    private long RunRace(Race r)
    {
        long speed = 0;
        long distance = 0;
        while (distance <= r.Target)
        {
            speed++;
            distance = Dist(speed, r.Duration - speed);
        }
        long lowerBound = speed;
        speed = r.Duration;

        distance = 0;
        while (distance <= r.Target)
        {
            speed--;
            distance = Dist(speed, r.Duration - speed);
        }
        long upperBound = speed;
        return upperBound + 1 - lowerBound;
    }

    private long Dist(long speed, long duration) => speed * duration;

    public string ShrinkSpaces(string input)
    {
        var result = input.Trim();
        while(result.Contains("  "))
        {
            result = result.Replace("  ", " ");
        }
        return result;
    }

    public class Race
    {
        public long Duration { get; set; }
        public long Target { get; set; }
    }
}
