namespace Advent23.Solutions;

public abstract class BaseDay
{
    public int Number { get; set; }
    public string Input { get; set; }
    public string[] InputLines { get; set; }
    public BaseDay(int number)
    {
        Input = File.ReadAllText($"./Inputs/{number}.txt");
        InputLines = Input.Split(Environment.NewLine);
        Number = number;
    }

    public abstract void Run();
}
