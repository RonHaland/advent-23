namespace Advent23.Solutions;

public class Day2 : BaseDay
{
    public Day2() : base(2) { }

    public override void Run()
    {
        var games = InputLines.Select(l => new Game(l));

        Console.WriteLine("--- PART 1 ---");
        var results = games.Where(g => g.ContainsLessThan(12, 13, 14)).Sum(g => g.Id);
        Console.WriteLine(results);

        Console.WriteLine("--- PART 2 ---");
        results = games.Sum(g => g.Power);
        Console.WriteLine(results);
    }


    internal class Game
    {
        public Game(string line)
        {
            var gameSplit = line.Split(':');
            Id = int.Parse(gameSplit[0].Split(' ')[1]);

            var setSplits = gameSplit[1].Split(";");
            Sets = setSplits.Select(s => new Set(s)).ToList();
        }

        public int Id { get; set; }
        internal List<Set> Sets { get; set; } = new List<Set>();
        public int MaxReds => Sets.Max(s => s.Red);
        public int MaxBlues => Sets.Max(s => s.Blue);
        public int MaxGreens => Sets.Max(s => s.Green);

        public int Power => MaxReds * MaxBlues * MaxGreens;

        internal bool ContainsLessThan(int red, int green, int blue)
        {
            return (Sets.Max(s => s.Red) <= red)
                && (Sets.Max(s => s.Green) <= green)
                && (Sets.Max(s => s.Blue) <= blue);
        }
    }

    internal class Set
    {
        public Set(string setString)
        {
            var cubes = setString.Split(",").Select(s => s.Trim());
            var red = cubes.FirstOrDefault(c => c.Contains("red"))?.Split(" ")[0] ?? "0";
            var green = cubes.FirstOrDefault(c => c.Contains("green"))?.Split(" ")[0] ?? "0";
            var blue = cubes.FirstOrDefault(c => c.Contains("blue"))?.Split(" ")[0] ?? "0";

            Red = int.Parse(red);
            Green = int.Parse(green);
            Blue = int.Parse(blue);
        }

        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }
    }
}
