using Advent23.Solutions;
using System.Reflection;


while (true)
{
    Console.WriteLine("Enter day to run or press enter to run latest");

    var days = Assembly.GetExecutingAssembly().DefinedTypes.Where(t => t.IsAssignableTo(typeof(BaseDay)) && t.Name != "BaseDay").Select(s => (BaseDay)s!.GetConstructor(Array.Empty<Type>())!.Invoke(new object[0]));

    days = days.OrderBy(n => n.Number);

    Console.WriteLine($"Valid days: {string.Join(", ", days.Select(d => d.Number))}");

    var input = Console.ReadLine();
    BaseDay dayToRun;

    if (int.TryParse(input, out var number) && days.Any(d => d.Number == number))
    {
        dayToRun = days.First(d => d.Number == number);
    }
    else
    {
        dayToRun = days.Last();
    }

    Console.WriteLine($"--- Running Day {dayToRun.Number} ---");
    dayToRun.Run();

    Console.WriteLine("");
    Console.WriteLine("Run another? (y/n)");
    var again = Console.ReadKey();
    if (again.KeyChar != 'y' && again.KeyChar != 'Y')
    {
        break;
    }
    Console.Clear();
}