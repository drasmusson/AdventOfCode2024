using System.Diagnostics;

var input = System.IO.File.ReadAllLines("input.txt");
var map = ParseMap(input);

var sw = Stopwatch.StartNew();
var partOne = PartOne(map);
var t = sw.Elapsed;
sw.Stop();
Console.WriteLine($"Part one: {partOne}");
Console.WriteLine($"Time: {t}");

var partTwo = PartTwo(map);
Console.WriteLine($"Part two: {partTwo}");
int PartTwo(Dictionary<Coord, string> map)
{
    var result = 0;
    foreach (var coord in map.Keys)
    {
        var dirWord1 = GetWordInDirection(map, coord, "BR", 3);
        if (dirWord1 == "MAS" || dirWord1 == "SAM")
        {
            var dirWord2 = GetWordInDirection(map, new Coord(coord.X, coord.Y + 2), "TR", 3);
            if (dirWord2 == "MAS" || dirWord2 == "SAM") result++;
        }
    }

    return result;
}

int PartOne(Dictionary<Coord, string> map)
{
    var words = new List<string>();

    foreach (var coord in map.Where(x => x.Value == "X").Select(x => x.Key))
    {
        foreach (var direction in Directions())
        {
            var dirWord = GetWordInDirection(map, coord, direction, 4);
            if (dirWord != null)
                words.Add(dirWord);
        }

    }

    return words.Count(x => x == "XMAS");
}

Dictionary<Coord, string> ParseMap(string[] input)
{
    var map = new Dictionary<Coord, string>();
    
    for (int y = 0; y < input.Length; y++)
        for (int x = 0; x < input[y].Length; x++)
            map[new Coord(x, y)] = input[y][x].ToString();
    
    return map;
}

string? GetWordInDirection(Dictionary<Coord, string> map, Coord c, string direction, int length)
{
    var s = "";
    for (var i = 0; i < length; i++)
    {
        if (map.TryGetValue(c, out var cs)) s += cs;
        else break;
        switch (direction)
        {
            case "TL":
                c = c.TL();
                break;
            case "T":
                c = c.T();
                break;
            case "TR":
                c = c.TR();
                break;
            case "L":
                c = c.L();
                break;
            case "R":
                c = c.R();
                break;
            case "BL":
                c = c.BL();
                break;
            case "B":
                c = c.B();
                break;
            case "BR":
                c = c.BR();
                break; 
            default:
                break;
        }
    }

    if (s.Length == length) return s;
    return null;
}

List<string> Directions() => ["TL", "T", "TR", "L", "R", "BL", "B", "BR"];
record Coord(int X, int Y)
{
    internal Coord TL() => this with { Y = Y - 1, X = X - 1 };
    internal Coord T() => this with { Y = Y - 1 };
    internal Coord TR() => this with { Y = Y - 1, X = X + 1 };
    internal Coord L() => this with { X = X - 1 };
    internal Coord R() => this with { X = X + 1 };
    internal Coord BL() => this with { Y = Y + 1, X = X - 1 };
    internal Coord B() => this with { Y = Y + 1 };
    internal Coord BR() => this with { Y = Y + 1, X = X + 1 };

}