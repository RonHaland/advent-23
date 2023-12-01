using Advent23.Solutions;
using System.Reflection;

Console.WriteLine("Enter day to run or press enter to run latest");

var days = Assembly.GetExecutingAssembly().DefinedTypes.Where(t => t.IsAssignableTo(typeof(BaseDay)) && t.Name != "BaseDay").Select(s => (BaseDay)s.GetConstructor(Array.Empty<Type>()).Invoke(new object[0]));

days = days.OrderBy(n => n.Number);

Console.WriteLine($"Valid days: {string.Join(", ", days.Select(d => d.Number))}");

var res = Console.ReadLine();


if (int.TryParse(res, out var number) && days.Any(d => d.Number == number))
{
    days.First(d => d.Number == number).Run();
}
else
{
    days.Last().Run();
}