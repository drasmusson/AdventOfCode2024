using System.Text.RegularExpressions;

var input = System.IO.File.ReadAllText("Input.txt");
var matches = GetMatches(input);

var partOne = PartOne(matches);
Console.WriteLine($"Part one: {partOne}");

int PartOne(List<string> list)
{
    var nums = list.Select(x => ValueMatch(x)).ToList();

    var result = 0;

    foreach (var num in nums)
        result += num.Item1 * num.Item2;
    
    return result;
}

(int, int) ValueMatch(string i)
{
    return (int.Parse(i.Split("(")[1].Split(",")[0]), int.Parse(i.Split(",")[1].Split(")")[0]));
}

List<string> GetMatches(string input) => Regex.Matches(input, @"mul\([0-9]*,[0-9]*\)")
    .Select(m => m.Value).ToList();