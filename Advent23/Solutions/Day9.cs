namespace Advent23.Solutions
{
    internal class Day9 : BaseDay
    {
        public Day9() : base(9)
        {
        }

        public override void Run()
        {
            Console.WriteLine("--- PART 1 ---");
            var histories = InputLines.Select(l => l.Split(" ").Select(n => long.Parse(n)).ToList()).ToList();

            var historyMaps = new List<List<List<long>>>();

            histories.ForEach(h => 
            {
                var historyMap = new List<List<long>>() { h };
                var currentMap = h;

                while (!currentMap.All(n => n == 0)) 
                {
                    currentMap = FindDiffs(currentMap);
                    historyMap.Add(currentMap);
                }

                historyMaps.Add(historyMap);
            });

            var finalPredictions = historyMaps.Select(hm =>
            {
                hm.Reverse();
                for (var i = 1; i < hm.Count; i++)
                {
                    var historyRow = hm[i];
                    var prediction = historyRow.Last() + hm[i - 1].Last();
                    hm[i].Add(prediction);
                }

                return hm.Last().Last();
            }).ToList();

            Console.WriteLine(finalPredictions.Sum());


            Console.WriteLine("--- PART 2 ---");

            var previousPredictions = historyMaps.Select(hm =>
            {
                //Each histoy Map is already reversed from part 1.
                for (var i = 1; i < hm.Count; i++)
                {
                    var historyRow = hm[i];
                    historyRow.Reverse();
                    var prediction = historyRow.Last() - hm[i - 1].Last();
                    hm[i].Add(prediction);
                }

                return hm.Last().Last();
            }).ToList();

            Console.WriteLine(previousPredictions.Sum());
        }

        private static List<long> FindDiffs(List<long> list)
        {
            return list.Skip(1).Select((s, i) => s - list[i]).ToList();
        }
    }
}
