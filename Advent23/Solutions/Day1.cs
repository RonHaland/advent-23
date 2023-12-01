using System.Threading;

namespace Advent23.Solutions;

public class Day1 : BaseDay
{
    public Day1() : base(1)
    {
    }

    public override void Run()
    {
        var result = InputLines.Select(l =>
        {
            var firstDigit = l.First(c => int.TryParse(c.ToString(), out var i));
            var lastDigit = l.Last(c => int.TryParse(c.ToString(), out var i));
            return int.Parse($"{firstDigit}{lastDigit}");
        }).Sum();
        Console.WriteLine("PART 1: ");
        Console.WriteLine(result);

        result = InputLines.Select(l =>
        {
            var line = TranslateLine(l);
            var firstDigit = line.First(c => int.TryParse(c.ToString(), out var i));
            var lastDigit = line.Last(c => int.TryParse(c.ToString(), out var i));
            return int.Parse($"{firstDigit}{lastDigit}");
        }).Sum();
        Console.WriteLine("PART 2: ");
        Console.WriteLine(result);
    }


    private string TranslateLine(string line)
    {
        
        var numbers = new[] { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" }.Select((n,i) => new { Label = n, Value = i+1});
        foreach (var item in numbers)
        {
            line = line.Replace(item.Label, $"{item.Label}{item.Value}{item.Label}");
        }

        return line;
    }
}
