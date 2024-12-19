using System.Reflection.Metadata.Ecma335;

var input = File.ReadAllText("input.txt");

var partOne = PartOne(input);
Console.WriteLine($"Part one: {partOne}");
int PartOne(string input)
{
    var line = ParseInput(input);
    for (int i = 0; i < 6; i++)
    {
        for (int j = 0; j < line.Count; j++)
            line.AddRange(Blink(line[j]));
    }

    return line.Count;
}

List<int> Blink(int stone)
{
    var result = new List<int>();

    if (stone == 0)
    {
        result.Add(1);
        return result;
    }

    if (stone.ToString().Length % 2 == 0)
    {
        var str = stone.ToString();
        var p = str.Substring(0, str.Length / 2);
        var q = str.Substring(str.Length / 2, str.Length / 2);
        result.AddRange([int.Parse(p), int.Parse(q)]);
        return result;
    }
    
    result.Add(stone * 2024);
    return result;
}
List<int> ParseInput(string input) => input.Split(" ").Select(int.Parse).ToList();