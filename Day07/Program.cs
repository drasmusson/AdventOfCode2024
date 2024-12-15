var input  = System.IO.File.ReadAllLines("Input.txt");

var partOne = PartOne(input);
Console.WriteLine($"Part One: {partOne}");

long PartOne(string[] strings)
{
    var equations = ParseEquations(strings);
    var result = 0L;
    foreach (var equation in equations)
    {
        var masks = GetMasks(equation.Value.Count - 1);
        var match = false;
        var i = 0;
        while (!match && i <= masks.Count - 1)
        {
            var mask = masks[i];
            if (equation.Key == ApplyMask(equation.Value, mask))
            {
                result += equation.Key;
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

        if (num == 0)
            total += equationValue[i + 1];

        if (num == 1)
            total *= equationValue[i + 1];

    }
    return total;
}

Dictionary<long, List<int>> ParseEquations(string[] strings)
{
    var equations = new Dictionary<long, List<int>>();

    foreach (var s in strings)
    {
        var split = s.Split(": ");
        var testValue = long.Parse(split[0]);
        var operators = split[1].Split(" ").Select(int.Parse).ToList();
        equations.Add(testValue, operators);
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