var input = File.ReadAllLines("input.txt");

var partOne = PartOne(input);
Console.WriteLine($"Part one: {partOne}");
var partTwo = PartTwo(input);
Console.WriteLine($"Part two: {partTwo}");

int PartOne(string[] strings)
{
    var result = 0;
    var map = ParseMap(strings);
    var trailheads = map.Where(x => x.Value == 0)
        .Select(x => x.Key).ToList();
    
    foreach (var trailhead in trailheads)
        result += CountReachableGoals(trailhead, 9, map, new HashSet<Coord>(), new HashSet<Coord>());

    return result;
}

int PartTwo(string[] strings)
{
    var result = 0;
    var map = ParseMap(strings);
    var trailheads = map.Where(x => x.Value == 0)
        .Select(x => x.Key).ToList();
    
    foreach (var trailhead in trailheads)
        result += CountPathsToGoal(trailhead, 9, map, new HashSet<Coord>());
    
    return result;
}

int CountReachableGoals(Coord current, int goal, Dictionary<Coord, int> map, HashSet<Coord> visited, HashSet<Coord> goalCoords)
{
    visited.Add(current);
    
    if (map[current] == goal) goalCoords.Add(current);

    foreach (var neighbor in current.Neighbors())
    {
        if (!map.TryGetValue(neighbor, out var n)) continue;
        if (n - map[current] != 1) continue;
        if (!visited.Contains(neighbor))
            CountReachableGoals(neighbor, goal, map, visited, goalCoords);
    }

    return goalCoords.Count;
}

int CountPathsToGoal(Coord current, int goal, Dictionary<Coord, int> map, HashSet<Coord> visited)
{
    var score = 0;

    visited.Add(current);
    
    if (map[current] == goal) score++;

    foreach (var neighbor in current.Neighbors())
    {
        if (!map.TryGetValue(neighbor, out var n)) continue;
        if (n - map[current] != 1) continue;
        if (!visited.Contains(neighbor))
            score += CountPathsToGoal(neighbor, goal, map, visited);
    }
        
    visited.Remove(current);

    return score;
}

Dictionary<Coord, int> ParseMap(string[] lines)
{
    var m = new Dictionary<Coord, int>();
    for (int y = 0; y < lines.Length; y++)
        for (int x = 0; x < lines[y].Length; x++)
            m[new Coord(x, y)] = int.Parse(lines[y][x].ToString());
    
    return m;
}

record Coord(int X, int Y)
{
    public IEnumerable<Coord> Neighbors()
    {
        yield return this with { X = X, Y = Y - 1 };
        yield return this with { X = X + 1, Y = Y };
        yield return this with { X = X, Y = Y + 1 };
        yield return this with { X = X - 1, Y = Y };
    }
}