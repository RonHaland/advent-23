namespace Advent23.Solutions;

public class Day10 : BaseDay
{
    public Day10() : base(10)
    {
    }

    public override void Run()
    {
        Console.WriteLine("--- PART 1 ---");
        var mapItems = InputLines
            .SelectMany((l, y) => l.Select<char, IMapItem>((c, x) => c == '.' ? new Space(c, new Pos { X = x, Y = y }) : new Pipe(c, new Pos { X = x, Y = y })))
            .ToList();

        var pipes = mapItems.Where(m => m.GetType() == typeof(Pipe)).Select(p => (Pipe)p).ToList();

        InitializeStartPipe(pipes);

        var startPipe = pipes.Single(p => p.Symbol == 'S');
        var currentPipe = startPipe.GetNextPipe(startPipe.Connections.First(), pipes);
        var previousPos = startPipe.Position;
        var steps = 1;
        while (currentPipe != startPipe)
        {
            currentPipe.MainLoop = true;
            steps++;
            var nextPreviousPos = currentPipe.Position;
            currentPipe = currentPipe.GetNextPipe(previousPos, pipes);
            previousPos = nextPreviousPos;
            currentPipe.MainLoop = true;
        }
        Console.WriteLine(steps/2);

        Console.WriteLine("--- PART 2 ---");

        var spaces = mapItems.Where(m => m.GetType() == typeof(Space)).ToList();
        spaces.AddRange(pipes.Where(p => !p.MainLoop));
        var mainLoopParts = pipes.Where(p => p.MainLoop).ToList();
        var insideCount = 0;
        var insides = new List<Pos>();
        spaces.ForEach(s =>
        {
            var crossingLoopSegments = mainLoopParts
                                        .Where(p => p.Connections.Any(c => c.Y == p.Position.Y-1))
                                        .Where(p => p.Position.X < s.Position.X && p.Position.Y == s.Position.Y)
                                        .ToList();
            
            // Find number of segments crossed, if count is odd we know we are inside the loop
            insideCount += crossingLoopSegments.Count() % 2 == 1 ? 1 : 0;
            if (crossingLoopSegments.Count() % 2 == 1) { insides.Add(s.Position); }
        });
        Console.WriteLine(insideCount);
    }

    private static void InitializeStartPipe(List<Pipe> pipes)
    {
        var startPipe = pipes.Single(p => p.Symbol == 'S');
        var startPos = startPipe.Position;
        var adjecentPipes = pipes.Where(p => p.Position.X >= startPos.X - 1
                                          && p.Position.X <= startPos.X + 1
                                          && p.Position.Y >= startPos.Y - 1
                                          && p.Position.Y <= startPos.Y + 1)
                                .Where(p => p.Connections.Any(c => c.X == startPos.X && c.Y == startPos.Y))
                                .ToList();
        startPipe.Connections = adjecentPipes.Select(p => p.Position).ToList();
    }

    private interface IMapItem
    {
        public char Symbol { get; }
        public Pos Position { get; set; }
    }

    private sealed class Space : IMapItem
    {
        public Space(char input, Pos position)
        {
            Symbol = input;
            Position = position;
        }
        public char Symbol { get; private set; }
        public Pos Position { get; set; }
    }

    private sealed class Pipe : IMapItem
    {
        public Pipe(char input, Pos position)
        {
            Symbol = input;
            Position = position;
            Connections = input switch
            {
                '-' => [
                    new Pos { X = position.X - 1, Y = position.Y },
                    new Pos { X = position.X + 1, Y = position.Y },
                ],
                '|' => [
                    new Pos { X = position.X, Y = position.Y - 1 },
                    new Pos { X = position.X, Y = position.Y + 1 },
                ],
                '7' => [
                    new Pos { X = position.X - 1, Y = position.Y },
                    new Pos { X = position.X, Y = position.Y + 1 },
                ],
                'L' => [
                    new Pos { X = position.X + 1, Y = position.Y },
                    new Pos { X = position.X, Y = position.Y - 1 },
                ],
                'F' => [
                    new Pos { X = position.X + 1, Y = position.Y },
                    new Pos { X = position.X, Y = position.Y + 1 },
                ],
                'J' => [
                    new Pos { X = position.X - 1, Y = position.Y },
                    new Pos { X = position.X, Y = position.Y - 1 },
                ],
                _ => [],
            };
        }

        public char Symbol { get; private set;  }

        public Pos Position { get; set; }
        public List<Pos> Connections { get; set; }
        public bool MainLoop {  get; set; }

        public Pipe GetNextPipe(Pos previousPipe, IEnumerable<Pipe> pipes)
        {
            var nextPos = Connections.First(c => !previousPipe.Equals(c));
            var nextPipe = pipes.First(p => nextPos.Equals(p.Position));

            return nextPipe;
        }
    }

    private sealed class Pos
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
