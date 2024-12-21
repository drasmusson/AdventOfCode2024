using System.Security.AccessControl;

var input = File.ReadAllLines("input.txt");

var partOne = PartOne(input);
Console.WriteLine($"Part one: {partOne}");
var partTwo = PartTwo(input);
Console.WriteLine($"Part two: {partTwo}");
int PartOne(string[] input)
{
    var map = ParseMap(input);

    var regions = GetRegions(map);
    Console.WriteLine($"Regions: {regions.Count}");
    return regions.Sum(CalculatePrice);
}

int PartTwo(string[] input)
{
    var map = ParseMap(input);
    var regions = GetRegions(map);
    var total = 0;
    foreach (var region in regions)
    {
        var sides = CountSides(region);
        total += region.Count * sides;
    }
    
    return total;
}

int CountSides(HashSet<Coord> region)
{
    var edges = 0;
    foreach (var coord in region)
    {
        var n = coord with { Y = coord.Y - 1 };
        var s = coord with { Y = coord.Y + 1 };
        var e = coord with { X = coord.X + 1 };
        var w = coord with { X = coord.X - 1 };
        
        var nW = new Coord(X: coord.X - 1, Y: coord.Y - 1);
        var nE = new Coord(X: coord.X + 1, Y: coord.Y - 1);
        var sW = new Coord(X: coord.X - 1, Y: coord.Y + 1);
        
        if (!region.Contains(n))
        {
            var sameEdge = region.Contains(w) && !region.Contains(nW);
            if (!sameEdge) edges++;
        }
            
        if (!region.Contains(s))
        {
            var sameEdge = region.Contains(w) && !region.Contains(sW);
            if (!sameEdge) edges++;
        }
            
        if (!region.Contains(w))
        {
            var sameEdge = region.Contains(n) && !region.Contains(nW);
            if (!sameEdge) edges++;
        }
            
        if (!region.Contains(e))
        {
            var sameEdge = region.Contains(n) && !region.Contains(nE);
            if (!sameEdge) edges++;
        }
    }
        
    return edges;
}



List<HashSet<Coord>> GetRegions(Dictionary<Coord, string> map)
{
    var regions = new List<HashSet<Coord>>();
    var visited = new HashSet<Coord>();
    foreach (var coord in map.Keys)
    {
        if (!visited.Contains(coord))
        {
            var region = new HashSet<Coord>();
            SetRegion(map, coord, region, visited);
            regions.Add(region);
        }
    }
    return regions;
}

int CalculatePrice(HashSet<Coord> region)
{
    var total = 0;
    total = region.Count * CalculatePerimiter(region);
    return total;
}

int CalculatePerimiter(HashSet<Coord> region)
{
    var result = 0;
    foreach (var coord in region)
    {
        result += 4 - coord.Neighbors().Count(x => region.Contains(x));
    }
    return result;
}

void SetRegion(Dictionary<Coord, string> map, Coord coord, HashSet<Coord> region, HashSet<Coord> visited)
{
    if (visited.Contains(coord)) return;
        
    visited.Add(coord);
    region.Add(coord);
    
    foreach (var c in GetNeighbors(coord, map))
        SetRegion(map, c, region, visited);
}

IEnumerable<Coord> GetNeighbors(Coord coord, Dictionary<Coord, string> map)
{
    foreach (var n in coord.Neighbors())
        if (map.ContainsKey(n) && map[coord] == map[n]) yield return n;
}

Dictionary<Coord, string> ParseMap(string[] input)
{
    var map = new Dictionary<Coord, string>();
    for (int y = 0; y < input.Length; y++)
        for (int x = 0; x < input[0].Length; x++)
        {
            map[new Coord(x, y)] = input[y][x].ToString();
        }
    
    return map;
}

record Coord(int X, int Y)
{
    public IEnumerable<Coord> Neighbors()
    {
        // up
        yield return this with { X = X, Y = Y - 1 };
        // right
        yield return this with { X = X + 1, Y = Y };
        // down
        yield return this with { X = X, Y = Y + 1 };
        //left
        yield return this with { X = X - 1, Y = Y };
    }
}