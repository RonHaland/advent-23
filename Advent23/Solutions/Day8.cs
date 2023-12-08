using System.Security;
using System.Security.AccessControl;

namespace Advent23.Solutions;

public class Day8 : BaseDay
{
    public Day8() : base(8)
    {
    }

    public override void Run()
    {
        Console.WriteLine("--- PART 1 ---");
        var instructions = InputLines.First().Select(c => c == 'L' ? Instruction.Left : Instruction.Right).ToList();
        var instructionQueue = new Queue<Instruction>(instructions);
        var nodes = InputLines
            .Skip(2)
            .Select(l => new Node(
                l.Split("=")[0].Trim(), 
                l.Split("=")[1].Split(",")[0].Trim().Trim('('), 
                l.Split("=")[1].Split(",")[1].Trim().Trim(')')))
            .ToDictionary(k => k.Name, v => v);
        int steps = 0;
        var currentNode = nodes["AAA"];
        while (currentNode.Name != "ZZZ")
        {
            var instruction = instructionQueue.Dequeue();
            instructionQueue.Enqueue(instruction);

            currentNode = nodes[currentNode.GetNextNode(instruction)];
            steps++;
        }
        Console.WriteLine(steps);


        Console.WriteLine("--- PART 2 ---");
        steps = 0;
        var currentNodes = nodes.Where(kv => kv.Key.EndsWith('A')).Select(k => k.Value).ToList();
        var map = new List<List<int>>();
        currentNodes.ForEach(startNode =>
        {
            var localInstructionQueue = new Queue<Instruction>(instructionQueue);
            var completions = new List<int>();
            int steps = 0;
            var currentNode = startNode;
            while (!currentNode.Name.EndsWith('Z'))
            {
                var instruction = localInstructionQueue.Dequeue();
                localInstructionQueue.Enqueue(instruction);

                currentNode = nodes[currentNode.GetNextNode(instruction)];
                steps++;

                if (currentNode.Name.EndsWith('Z'))
                    completions.Add(steps);
            }
            map.Add(completions);
        });
        var allValues = map.SelectMany(s => s).ToList();
        // Got a hint that lcm was the way to go.
        var lcm = allValues.Aggregate((ulong)allValues.First(), (a, b) => LCM(a, (ulong)b));

        Console.WriteLine(lcm);
    }

    //From LCM page on Wikipedia
    private static ulong LCM(ulong a, ulong b)
    {
        return a * (b / GCD(a, b));
    }

    //https://stackoverflow.com/a/41766138
    private static ulong GCD(ulong a, ulong b)
    {
        while (a != 0 && b != 0)
        {
            if (a > b)
                a %= b;
            else
                b %= a;
        }

        return a | b;
    }

    private enum Instruction
    {
        Left = 0,
        Right = 1,
    }
    private class Node
    {
        public Node(string name, string left, string right)
        {
            Name = name;
            Left = left;
            Right = right;
        }
        public string Name { get; set; }
        public string Left { get; set; }
        public string Right { get; set; }

        public string GetNextNode(Instruction instruction)
        {
            switch (instruction)
            {
                case Instruction.Left:
                    return Left;
                case Instruction.Right:
                    return Right;
                default:
                    throw new ArgumentException("Invalid Instruction", nameof(instruction));
            }

        }
    }
}
