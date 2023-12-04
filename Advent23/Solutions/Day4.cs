﻿using System.Text.RegularExpressions;

namespace Advent23.Solutions;

public class Day4 : BaseDay
{
    public Day4() : base(4)
    {
        
    }

    public override void Run()
    {
        var tickets = InputLines
            .Select(l => new Ticket(int.Parse(Regex.Match(l.Split(":")[0], "[0-9]+").Value),
                            l.Split(":")[1]
                             .Split("|")[0]
                             .Trim()
                             .Split(" ")
                             .Select(n => int.TryParse(n, out var number) ? number : -1)
                             .Where(n => n > 0),
                            l.Split(":")[1]
                             .Split("|")[1]
                             .Trim()
                             .Split(" ")
                             .Select(n => int.TryParse(n, out var number)? number : -1)
                             .Where(n => n > 0)))
            .ToList();
        var winningNumbers = tickets.Select(t => t.WinningNumbers.Where(w => t.MyNumbers.Contains(w)).Count());

        var points = winningNumbers.Select(w => w > 0 ? Math.Pow(2, w-1) : 0);
        var pointsTotal = points.Sum();
        Console.WriteLine("--- PART 1 ---");
        Console.WriteLine(pointsTotal);


        Console.WriteLine("--- PART 2 ---");
        Console.WriteLine("This takes a while... 🐌");
        var allTickets = new Queue<Ticket>(tickets);
        var count = allTickets.Count;
        while (allTickets.TryDequeue(out var ticket))
        {
            var winningsCount = ticket.WinningNumbers.Count(w => ticket.MyNumbers.Contains(w));
            var index = tickets.IndexOf(ticket);

            var copies = tickets.GetRange(index + 1, winningsCount);
            count+= copies.Count;
            copies.ForEach(c =>
            {
                allTickets.Enqueue(c);
            });
        }

        Console.WriteLine(count);
    }

    internal class Ticket(int Id, IEnumerable<int> WinningNumbers, IEnumerable<int> MyNumbers)
    {
        public int Id { get; set; } = Id;
        public IEnumerable<int> WinningNumbers { get; set; } = WinningNumbers;
        public IEnumerable<int> MyNumbers { get; set; } = MyNumbers;

    }
}
