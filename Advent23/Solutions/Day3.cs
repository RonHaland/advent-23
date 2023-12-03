using System.Text.RegularExpressions;

namespace Advent23.Solutions;

public class Day3 : BaseDay
{
    public Day3() : base(3)
    {
        
    }

    public override void Run()
    {
        var symbols = InputLines
            .SelectMany((s, y) => 
                s.Select((c, x) => (c != '.' && !(c >= 48 && c <= 57)) ? new Pos(x,y,c) : null))
            .Where(p => p != null)
            .ToList();
        var numbers = InputLines.SelectMany((s, i) => Regex.Matches(s, "[0-9]+").Select((m) => new PartNumber(m.Value, new Pos(m.Index, i))));

        var partNumbers = numbers.Where(n => n.IsAdjecentToSymbol(symbols));
        var valueSum = partNumbers.Sum(p => p.Value);
        Console.WriteLine("--- PART 1 ---");
        Console.WriteLine(valueSum);

        Console.WriteLine("--- PART 2 ---");
        var result = symbols.Where(s => s?.C == '*' && s.Numbers.Count == 2).Select(s => (s!.Numbers.First().Value * s!.Numbers.Last().Value));
        Console.WriteLine(result.Sum());

    }

    internal class PartNumber
    {
        public PartNumber(string input, Pos startPos)
        {
            var lineIndexes = Enumerable.Range(startPos.X - 1, input.Length + 2);
            var adjecentPositions = lineIndexes.SelectMany((x) => GetAdjecentPositionsForNumber(x, startPos, input.Length));
            Value = int.Parse(input);
            StartPos = startPos;
            AdjecentPositions = adjecentPositions;
        }

        public int Value { get; }
        public Pos StartPos { get; }
        public IEnumerable<Pos> AdjecentPositions { get; set; }
        public IEnumerable<char> AdjecentSymbols { get; set; }

        public bool IsAdjecentToSymbol(IEnumerable<Pos?> symbolPositions)
        {
            var adjacentSymbols = symbolPositions.Where(s => s != null && AdjecentPositions.Any(a => a.X == s.X && a.Y == s.Y));
            foreach (var symbol in adjacentSymbols)
            {
                if (!symbol!.Numbers.Select(n => n.StartPos).Contains(StartPos))
                    symbol!.Numbers.Add(this);
            }
            AdjecentSymbols = adjacentSymbols.Select(c => c!.C!.Value);
            return AdjecentSymbols.Any();
        }

        private IEnumerable<Pos> GetAdjecentPositionsForNumber(int currentX, Pos StartPos, int length)
        {
            var adjecentPositions = new List<Pos>();
            if (currentX < 0) return Enumerable.Empty<Pos>();
            if (currentX < StartPos.X || currentX > StartPos.X + length-1)
            {
                adjecentPositions.Add(new(currentX, StartPos.Y));
            }
            adjecentPositions.Add(new(currentX, StartPos.Y-1));
            adjecentPositions.Add(new(currentX, StartPos.Y+1));

            return adjecentPositions;
        }
    }


    internal class Pos(int X, int Y, char? C = null)
    {
        public int X { get; } = X;
        public int Y { get; } = Y;
        public char? C { get; } = C;

        public List<PartNumber> Numbers { get; } = new List<PartNumber>();

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is Pos)
                return ((Pos)obj).X == X && ((Pos)obj).Y == Y;
            return base.Equals(obj);
        }
    }
}
