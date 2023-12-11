using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Advent23.Solutions;

public class Day11 : BaseDay
{
    public Day11() : base(11)
    {
    }

    public override void Run()
    {
        Console.WriteLine("--- PART 1 ---");
        var universe = InputLines.ToList();
        var expandedUniverse = ExpandUniverse(universe);

        var galaxies = expandedUniverse.SelectMany((l, y) => l.Select((c, x) => c == '#' ? new Galaxy { Position = new Pos { X = x, Y = y } } : null)).Where(c => c != null).Select(c => c!).ToList();

        var distances = galaxies.SelectMany((g, i) =>
        {
            var distancesToNexts = galaxies.Skip(i+1).Select(c => Math.Abs(c.Position.X - g.Position.X) + Math.Abs(c.Position.Y - g.Position.Y));

            return distancesToNexts;
        }).ToList();
        Console.WriteLine(distances.Sum());

        Console.WriteLine("--- PART 2 ---");
        var megaUniverse = MegaExpandUniverse(universe);
        var megaGaps = megaUniverse.SelectMany((l, y) => l.Select((c, x) => c == 'M' ? new Pos { X = x, Y = y } : null)).Where(c => c != null).Select(c => c!).ToList();

        var megaGalaxies = megaUniverse.SelectMany((l, y) => l.Select((c, x) => c == '#' ? 
                new Galaxy 
                { 
                    Position = new Pos 
                    { 
                        X = x - megaGaps.Count(g => g.X < x && g.Y == y) + megaGaps.Count(g => g.X < x && g.Y == y) * 1_000_000, 
                        Y = y - megaGaps.Count(g => g.Y < y && g.X == x) + megaGaps.Count(g => g.Y < y && g.X == x) * 1_000_000
                    } 
                } : null)).Where(c => c != null).Select(c => c!).ToList();


        var megaDistances = megaGalaxies.SelectMany((g, i) =>
        {
            var distancesToNexts = megaGalaxies.Skip(i + 1).Select(c => Math.Abs((long)c.Position.X - g.Position.X) + Math.Abs((long)c.Position.Y - g.Position.Y));

            return distancesToNexts;
        }).ToList();
        Console.WriteLine(megaDistances.Sum());

    }

    private List<string> ExpandUniverse(List<string> universe)
    {
        var expandedRows = universe.SelectMany(l => l.All(c => c == '.') ? new List<string> { l, l } : [l]).ToList();
        var pivoted = expandedRows.Pivot();
        var expandedColumns = pivoted.SelectMany(l => l.All(c => c == '.') ? new List<string> { l, l } : [l]).ToList();

        //Console.WriteLine(string.Join(Environment.NewLine, expandedColumns.Pivot()));

        return expandedColumns.Pivot();
    }

    private List<string> MegaExpandUniverse(List<string> universe)
    {
        var expandedRows = universe.Select(l => l.All(c => c == '.') ? l.Replace('.', 'M') : l).ToList();
        var pivoted = expandedRows.Pivot();
        var expandedColumns = pivoted.Select(l => l.All(c => c == '.' || c == 'M') ? l.Replace('.', 'M') : l).ToList();

        return expandedColumns.Pivot();
    }

    public class Galaxy
    {
        public Pos Position { get; set; }
    }   

    public class Pos
    {
        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object? obj)
        {
            var other = obj as Pos;
            return other?.X == X && other.Y == Y;
        }
    }
}

public static class EnumerableExt
{
    public static List<string> Pivot(this IEnumerable<string> enumerable)
    {
        return enumerable.SelectMany(r => r.Select((c, i) => new { c, i }))
                                .GroupBy(i => i.i, i => i.c)
                                .Select(g => string.Join("", g.ToList()))
                                .ToList();
    }
}
