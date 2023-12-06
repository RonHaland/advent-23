namespace Advent23.Solutions;

public class Day5 : BaseDay
{
    public Day5() : base(5)
    {

    }

    public override void Run()
    {
        Console.WriteLine("--- PART 1 ---");

        var inputSections = Input.Split(Environment.NewLine + Environment.NewLine);
        var seeds = inputSections[0].Split(":")[1].Trim().Split(" ").Select(s => long.Parse(s)).ToList();
        var maps = inputSections.Skip(1).Select(m => new Map
        {
            MapName = m.Split(":")[0],
            MapRanges = m.Split(":")[1].Split(Environment.NewLine).Where(s => !string.IsNullOrWhiteSpace(s)).Select(s =>
            {
                var split = s.Split(" ").Select(i => long.Parse(i)).ToList();
                return new MapRange { DestStart = split[0], SourceStart = split[1], Length = split[2] };
            }).ToList(),
        }).ToList();

        var exploredMaps = new List<Dictionary<string, long>>();

        foreach (var seed in seeds)
        {
            var exploredMap = new Dictionary<string, long>() { { "Seed", seed } };
            var result = maps.First().ProcessValue(seed);

            var currentVal = seed;
            maps.ForEach(m =>
            {
                currentVal = m.ProcessValue(currentVal);
                exploredMap.Add(m.MapName, currentVal);
            });
            exploredMaps.Add(exploredMap);
        }

        var lowest = exploredMaps.OrderBy(e => e.Last().Value).First();

        Console.WriteLine(lowest.Last().Value);



        Console.WriteLine("--- PART 2 ---");
        var ranges = seeds?.Select((s, i) => i % 2 == 0 ? new SeedRange { SeedStart = seeds[i], Length = seeds[i + 1] } : null).Where(s => s != null).ToList();

        var exploredMapsRanges = new List<Dictionary<string, List<SeedRange>>>();
        foreach (var seedRange in ranges!)
        {
            var exploredMapRange = new Dictionary<string, List<SeedRange>>{ { "Start", new List<SeedRange> { seedRange! } } };
            var currentRanges = new List<SeedRange>() { seedRange! };
            maps.ForEach(m =>
            {
                currentRanges = currentRanges.SelectMany(r => m.ProcessSeedRange(r)).ToList();
                exploredMapRange.Add(m.MapName, currentRanges);
            });
            exploredMapsRanges.Add(exploredMapRange);
        }

        var lowestLocation = exploredMapsRanges.SelectMany(emr => emr.Values.Last().Select(s => s.SeedStart)).Min();

        Console.WriteLine(lowestLocation);
    }

    public class Map
    {
        public required string MapName { get; set; }
        public required List<MapRange> MapRanges { get; set; }

        public long ProcessValue(long value)
        {
            var range = MapRanges.FirstOrDefault(r => r.SourceStart <= value && r.SourceEnd >= value);
            return value + (range?.Diff ?? 0);
        }

        public List<SeedRange> ProcessSeedRange(SeedRange range)
        {
            // find all mapRanges overlapping with seedRange
            var overlappingRanges = MapRanges
                .Where(m => (m.SourceStart <= range.SeedStart && m.SourceEnd >= range.SeedStart) // start of seeds is within map range
                         || (m.SourceStart <= range.SeedEnd && m.SourceEnd >= range.SeedEnd) // end of seeds is within map range
                         || (range.SeedStart <= m.SourceStart && range.SeedEnd >= m.SourceEnd)) // entire map range is within seeds
                .OrderBy(m => m.SourceStart)
                .ToList();
            var rest = new Queue<SeedRange>(new[]{ range });
            var newRanges = new List<SeedRange>();

            while (rest.TryDequeue(out var r))
            {
                var newRange = new SeedRange { SeedStart = r.SeedStart, Length = r.Length };
                var currentOverlap = overlappingRanges?.FirstOrDefault();
                var isOverlapping = false;
                //Overlapping Start of Range
                if (r.SeedStart < currentOverlap?.SourceStart && r.SeedEnd >= currentOverlap?.SourceStart)
                {
                    newRange.Length = currentOverlap != null ? currentOverlap.SourceStart - r.SeedStart : r.Length;
                    // now new range is no longer overlapping with the full range
                }
                //Overlapping End of Range
                else if (r.SeedStart <= currentOverlap?.SourceEnd && r.SeedEnd >= currentOverlap?.SourceEnd)
                {
                    newRange.Length = currentOverlap != null ? currentOverlap.SourceEnd - r.SeedStart : r.Length;
                    if (r.SeedEnd == currentOverlap?.SourceEnd) newRange.Length++;
                    if (currentOverlap != null)
                        overlappingRanges?.Remove(currentOverlap);
                    isOverlapping = true;
                }
                //Overlapping entire Range
                else if (r.SeedStart >= currentOverlap?.SourceStart && r.SeedEnd <= currentOverlap?.SourceEnd)
                {
                    isOverlapping = true;
                }

                if (newRange.SeedEnd < r.SeedEnd)
                {
                    var remaining = r.SeedEnd - newRange.SeedEnd;
                    r.SeedStart = newRange.SeedEnd+1;
                    r.Length = remaining;
                    rest.Enqueue(r);
                }
                if (isOverlapping)
                { 
                    newRange.SeedStart += currentOverlap?.Diff ?? 0; 
                }
                newRanges.Add(newRange);
            }

            return newRanges;
        }
    }

    public class MapRange
    {
        public long DestStart;
        public long SourceStart;
        public long Length;
        public long Diff => DestStart - SourceStart;
        public long SourceEnd => SourceStart + Length - 1;
    }

    public class SeedRange
    {
        public long SeedStart;
        public long Length;
        public long SeedEnd => SeedStart + Length - 1;
    }
}
