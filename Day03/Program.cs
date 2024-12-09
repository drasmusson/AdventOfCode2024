using System.Text.RegularExpressions;

var input = System.IO.File.ReadAllText("Input.txt");
var matches = GetMul(input);

var partOne = PartOne(matches);
Console.WriteLine($"Part one: {partOne}");

var partTwo = PartTwo(input);
Console.WriteLine($"Part two: {partTwo}");

int PartTwo(string s)
{
    var pattern = @"don't\(\)";

    var enabled = true;
    var result = 0;
    while (true)
    {
        var match = Regex.Match(s, pattern);
        if (enabled)
        {
            var current = match.Success ? s.Substring(0, match.Index) : s;
            var muls = GetMul(current);
            var nums = muls.Select(x => GetMulValues(x)).ToList();
            
            foreach (var num in nums)
                result += num.Item1 * num.Item2;
            
            pattern = @"do\(\)";
        }

        if (!match.Success) break;
        if (!enabled) pattern = @"don't\(\)";
        
        s = s.Substring(match.Index, s.Length - match.Index);
        enabled = !enabled;
    }
    return result;
}

int PartOne(List<string> list)
{
    var nums = list.Select(x => GetMulValues(x)).ToList();

    var result = 0;

    foreach (var num in nums)
        result += num.Item1 * num.Item2;
    
    return result;
}

(int, int) GetMulValues(string i)
{
    return (int.Parse(i.Split("(")[1].Split(",")[0]), int.Parse(i.Split(",")[1].Split(")")[0]));
}

List<string> GetMul(string input) => Regex.Matches(input, @"mul\([0-9]*,[0-9]*\)")
    .Select(m => m.Value).ToList();