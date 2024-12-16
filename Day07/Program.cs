using System.Diagnostics;

var input  = System.IO.File.ReadAllLines("Input.txt");

var sw = new Stopwatch();
sw.Start();
var partOne = PartOne(input);
sw.Stop();
Console.WriteLine($"Part One: {partOne}");
Console.WriteLine($"Time taken: {sw.ElapsedMilliseconds} ms");
sw.Restart();
var partTwo = PartTwo(input);
sw.Stop();
Console.WriteLine($"Part Two: {partTwo}");
Console.WriteLine($"Time taken: {sw.ElapsedMilliseconds} ms");
long PartOne(string[] strings)
{
    var equations = ParseEquations(strings);
    var result = 0L;
    foreach (var equation in equations)
    {
        var masks = GetMasks(equation.Item2.Count - 1);
        var match = false;
        var i = 0;
        while (!match && i <= masks.Count - 1)
        {
            var mask = masks[i];
            if (equation.Item1 == ApplyMask(equation.Item2, mask))
            {
                result += equation.Item1;
                match = true;
            }
            
            i++;
        }
    }
    return result;
}

long PartTwo(string[] strings)
{
    var equations = ParseEquations(strings);
    var result = 0L;
    foreach (var equation in equations)
    {
        var masks = GetMasks2(equation.Item2.Count - 1);
        var match = false;
        var i = 0;
        while (!match && i <= masks.Count - 1)
        {
            var mask = masks[i];
            if (equation.Item1 == ApplyMask(equation.Item2, mask))
            {
                result += equation.Item1;
                match = true;
            }
            
            i++;
        }
    }

    return result;
}

long ApplyMask(List<int> equationValue, int[] ints)
{
    var total = (long)equationValue[0];
    for (int i = 0; i < ints.Length; i++)
    {
        var num = ints[i];

        switch (num)
        {
            case 0:
                total += equationValue[i + 1];
                break;
            case 1:
                total *= equationValue[i + 1];
                break;
            case 2:
                total = Concat(total, equationValue[i + 1]);
                break;
        }
    }
    return total;
}

long Concat(long a, int b) => long.Parse($"{a}{b}");

List<(long, List<int>)> ParseEquations(string[] strings)
{
    var equations = new List<(long, List<int>)>();

    foreach (var s in strings)
    {
        var split = s.Split(": ");
        var testValue = long.Parse(split[0]);
        var operators = split[1].Split(" ").Select(int.Parse).ToList();
        equations.Add((testValue, operators));
    }
    return equations;
}

List<int[]> GetMasks(int length)
{
    var result = new List<int[]>();
    int totalCombinations = 1 << length;
    
    for (int i = 0; i < totalCombinations; i++)
    {
        var combination = new int[length];
        
        for (int j = 0; j < length; j++)
            combination[j] = (i & (1 << j)) != 0 ? 1 : 0;
            
        result.Add(combination);
    }

    return result;
}

List<int[]> GetMasks2(int length)
{
    var result = new List<int[]>();
    var current = new int[length];
        
    void Generate(int position)
    {
        if (position == length)
        {
            result.Add((int[])current.Clone());
            return;
        }
            
        for (int num = 0; num <= 2; num++)
        {
            current[position] = num;
            Generate(position + 1);
        }
    }
        
    Generate(0);
    return result;
}
