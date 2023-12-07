namespace Advent23.Solutions;

public class Day7 : BaseDay
{
    public Day7() : base(7)
    {
    }

    public override void Run()
    {
        var hands = InputLines.Select(l => new Hand(l.Split(" ")[0], long.Parse(l.Split(" ")[1])));

        Console.WriteLine("--- PART 1 ---");
        var orderedHands = hands.OrderBy(h => h.Strength).ThenBy(h => h.RelativeStrength).ToList();
        var values = orderedHands.Select((h, i) => h.Bid * (i + 1)).ToList();
        var part1Result = values.Sum();

        Console.WriteLine(part1Result);

        Console.WriteLine("--- PART 2 ---");

        var orderedJokerHands = hands.OrderBy(h => h.StrengthWJoker).ThenBy(h => h.RelativeStrengthWJoker).ToList();
        var jokerValues = orderedJokerHands.Select((h, i) => h.Bid * (i + 1)).ToList();
        var part2Result = jokerValues.Sum();

        Console.WriteLine(part2Result);
    }

    public class Hand
    {
        public Hand(string hand, long bid)
        {
            Cards = hand.Select(c => c).ToArray();
            Bid = bid;
            CardsWOJoker = Cards.Where(c => c != 'J').ToArray();
        }

        public long Bid { get; set; }

        public char[] Cards { get; set; }
        public char[] CardsWOJoker { get; set; }
        public int JokerCount => Cards.Count(c => c == 'J');
        public long StrengthWJoker { get
            {
                if (JokerCount == 0)
                    return Strength;

                var groups = CardsWOJoker.GroupBy(c => c).Select(d => d.Count()).ToList();
                switch (groups.Count)
                {
                    case 0:
                    case 1:
                        return 10;
                    case 2:
                        if (groups.Any(g => g + JokerCount == 4))
                            return 9;
                        if (groups.All(g => g + JokerCount == 3 || g == 2) || groups.All(g => g + JokerCount == 2 || g == 3))
                            return 8;
                        break;
                    case 3:
                        if (groups.Any(g => g + JokerCount == 3))
                            return 7;
                        if (groups.Count(g => g + JokerCount == 2) == 1 && groups.Count(g => g == 2) == 1)
                            return 6;
                        break;
                    case 4:
                        if (groups.Any(g => g + JokerCount == 2))
                            return 5;
                        break;

                            default:
                        break;
                }
                return 0;
            } }
        public long Strength { get {
                var groups = Cards.GroupBy(c => c).Select(d => d.Count()).ToList();
                switch (groups.Count)
                {
                    case 1:
                        return 10; // 5 of a kind
                    case 2:
                        if (groups.Any(g => g == 4))
                            return 9; // 4 of a kind
                        if (groups.All(g => g == 3 || g == 2))
                            return 8; // full house
                        break;
                    case 3:
                        if (groups.Any(g => g == 3))
                            return 7; // 3 of a kind
                        if (groups.Count(g => g == 2) == 2)
                            return 6; // 2 pairs
                        break;
                    case 4:
                        if (groups.Count(g => g == 2) == 1)
                            return 5; // 1 pair
                        break;
                    case 5:
                        return 0; // High Card
                    default:
                        break;
                }

                Console.WriteLine($"Strange collection of cards: {string.Join("," , Cards)}");
                return 0;
            } }

        public long RelativeStrength => long.Parse(string.Join("", Cards.Select(c => CardValue(c).ToString().PadLeft(2, '0'))));
        public long RelativeStrengthWJoker => long.Parse(string.Join("", Cards.Select(c => CardValue2(c).ToString().PadLeft(2, '0'))));
    }

    private static List<char> _cardlist = new() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
    private static List<char> _cardlist2 = new() { 'J', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };

    public static int CardValue(char card) => _cardlist.IndexOf(card);
    public static int CardValue2(char card) => _cardlist2.IndexOf(card);
}
