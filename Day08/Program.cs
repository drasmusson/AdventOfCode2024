using System.Runtime.CompilerServices;

var input = File.ReadAllLines("input.txt");

var partOne = PartOne(input);
Console.WriteLine($"Part one: {partOne}");
var partTwo = PartTwo(input);
Console.WriteLine($"Part two: {partTwo}");
int PartOne(string[] strings)
{
    var map = ParseMap(strings);
    
    var antinodes = new HashSet<Coord>();

    foreach (var c in map.Keys)
    {
        var antenna = map[c];
        if (antenna == ".") continue;
        
        var matchingAntennas = map
            .Where(x => x.Key != c && x.Value == antenna)
            .Select(y => y.Key).ToList();
        
        foreach (var matchingAntenna in matchingAntennas)
        {
            var newAntinodes = FindAntinodes1(c, matchingAntenna).Where(x => map.ContainsKey(x));
            foreach (var newAntinode in newAntinodes)
                antinodes.Add(newAntinode);
        }
    }

    return antinodes.Count();;
}
int PartTwo(string[] strings)
{
    var map = ParseMap(strings);
    
    var antinodes = new HashSet<Coord>();

    foreach (var c in map.Keys)
    {
        var antenna = map[c];
        if (antenna == ".") continue;
        
        var matchingAntennas = map
            .Where(x => x.Key != c && x.Value == antenna)
            .Select(y => y.Key).ToList();
        
        if (matchingAntennas.Count > 0) antinodes.Add(c);

        foreach (var matchingAntenna in matchingAntennas)
        {
            var newAntinodes = FindAntinodes2(c, matchingAntenna, map).Where(x => map.ContainsKey(x));
            foreach (var newAntinode in newAntinodes)
                antinodes.Add(newAntinode);
        }
    }

    return antinodes.Count();;
}

List<Coord> FindAntinodes1(Coord c1, Coord c2)
{
    var distance1 = c2 - c1;
    var nC1 = new Coord(c2.X + distance1.X, c2.Y + distance1.Y);
    
    var distance2 = c1 - c2;
    var nC2 = new Coord(c1.X + distance2.X, c1.Y + distance2.Y);
    return [nC1, nC2];
}

List<Coord> FindAntinodes2(Coord c1, Coord c2, Dictionary<Coord, string> map)
{
    var antinodes = new List<Coord>();
    FindAntinodesRe(c1, c2, map, antinodes);
    
    FindAntinodesRe(c2, c1, map, antinodes);
    return antinodes;
}

List<Coord> FindAntinodesRe(Coord c1, Coord c2, Dictionary<Coord, string> map, List<Coord> antinodes)
{
    var distance1 = c2 - c1;
    var nC1 = new Coord(c2.X + distance1.X, c2.Y + distance1.Y);
    if (!map.ContainsKey(nC1)) return antinodes;
    
    antinodes.Add(nC1);
    return FindAntinodesRe(c2, nC1, map, antinodes);
}

Dictionary<Coord, string> ParseMap(string[] strings1)
{
    var map = new Dictionary<Coord, string>();
    for (int y = 0; y < strings1.Length; y++)
        for (int x = 0; x < strings1[0].Length; x++)
            map.Add(new(x, y), strings1[y][x].ToString());
    return map;
}

record Coord(int X, int Y)
{
    public static Coord operator -(Coord a, Coord b) => new Coord(a.X - b.X, a.Y - b.Y);
}
