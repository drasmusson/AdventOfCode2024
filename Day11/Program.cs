using System.Diagnostics;

var input = File.ReadAllText("input.txt");
var sw = Stopwatch.StartNew();
var partOne = PartOne(input);
sw.Stop();
Console.WriteLine($"Part one: {partOne}");
Console.WriteLine($"Part one: {sw.ElapsedMilliseconds}ms");
sw.Restart();
var partTwo = PartTwo(input);
sw.Stop();
Console.WriteLine($"Part two: {partTwo}");
Console.WriteLine($"Part two: {sw.ElapsedMilliseconds}ms");
int PartOne(string input)
{
    var line = ParseInput(input);
    for (int i = 0; i < 25; i++)
    {
        var newLine = new List<long>();
        for (int j = 0; j < line.Count; j++)
        {
            newLine.AddRange(Blink(line[j]));
        }
        line = newLine;
    }

    return line.Count;
}

long PartTwo(string input)
{
    var line = ParseInput(input);
    var map = new Dictionary<long, long>();
    
    foreach (var i in line)
        map[i] = 1;
    
    for (int i = 0; i < 75; i++)
    {
        var newMap = new Dictionary<long, long>();
        foreach (var kp in map)
            foreach (var newNumber in Blink(kp.Key))
                if (!newMap.TryAdd(newNumber, kp.Value)) newMap[newNumber]+= kp.Value;
        
        map = newMap;
    }

    return map.Sum(x => x.Value);
}
    

List<long> Blink(long stone)
{
    var result = new List<long>();

    if (stone == 0)
    {
        result.Add(1);
        return result;
    }

    if (stone.ToString().Length % 2 == 0)
    {
        result.AddRange(CutInHalf(stone));
        return result;
    }
    
    result.Add(stone * 2024);
    return result;
}

List<long> CutInHalf(long stone)
{
    var result = new List<long>();
    var str = stone.ToString();
    var p = str.Substring(0, str.Length / 2);
    var q = str.Substring(str.Length / 2, str.Length / 2);
    result.AddRange([long.Parse(p), long.Parse(q)]);
    return result;
}
List<long> ParseInput(string input) => input.Split(" ").Select(long.Parse).ToList();