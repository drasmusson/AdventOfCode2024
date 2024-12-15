using System.Drawing;

var input = System.IO.File.ReadAllLines("Input.txt");

var map = ParseMap(input);
var guardInput = map.First(x => x.Value != "." && x.Value != "#");
var guard = new Guard(guardInput.Key, guardInput.Value);

var partOne = PartOne(map, guard);
Console.WriteLine($"Part One: {partOne}");

var guard2 = new Guard(guardInput.Key, guardInput.Value);
var partTwo = PartTwo(map, guard2);
Console.WriteLine($"Part Two: {partTwo}");

int PartOne(Dictionary<Coord, string> map, Guard guard)
{
    var visited = new HashSet<Coord> { guard.Position };
    while (true)
    {
        var nextPosition = guard.Next();
        if (!map.ContainsKey(nextPosition)) break;
        if (map[nextPosition] == "#") guard.Turn();
        if (map[nextPosition] != "#")
        {
            guard.Position = nextPosition;
            visited.Add(nextPosition);
        }
    }
    
    return visited.Count;
}

int PartTwo(Dictionary<Coord, string> map, Guard guard)
{
    var result = 0;
    foreach (var coord in map.Keys)
    {
        var currentMap = new Dictionary<Coord, string>(map);

        if (map[coord] != ".") continue;
        
        currentMap[coord] = "#";
        var currentGueard = new Guard(guard.Position, guard.Dir);
        
        var visitedStates = new HashSet<(Coord, Dir)> { (currentGueard.Position, currentGueard.Dir) };
        while (true)
        {
            var nextPosition = currentGueard.Next();
            if (!currentMap.ContainsKey(nextPosition)) break;
            if (visitedStates.Contains((nextPosition, currentGueard.Dir)))
            {
                result++;
                break;
            }

            if (currentMap[nextPosition] == "#")
            {
                currentGueard.Turn();
                continue;
            }
            
            currentGueard.Position = nextPosition;
            visitedStates.Add((nextPosition, currentGueard.Dir));
        }
    }
    return result;
}

Dictionary<Coord, string> ParseMap(string[] strings)
{
    var map = new Dictionary<Coord, string>();

    for (int y = 0; y < strings.Count(); y++)
    for (int x = 0; x < strings[y].Length; x++)
    {
        map[new Coord(x, y)] = strings[y][x].ToString();
    }
    return map;
}

record Coord(int X, int Y);

class Guard
{
    public Coord Position { get; set; }
    public Dir Dir { get; set; }

    public Guard(Coord position, string dir)
    {
        Position = position;

        switch (dir)
        {
            case "^":
                Dir = Dir.Up;
                break;
            case "v":
                Dir = Dir.Down;
                break;
            case "<":
                Dir = Dir.Left;
                break;
            case ">":
                Dir = Dir.Right;
                break;
        }
    }

    public Guard(Coord position, Dir dir)
    {
        Position = position;
        Dir = dir;
    }

    internal Coord Next()
    {
        switch (Dir)
        {
            case Dir.Up:
                return new Coord(Position.X, Position.Y - 1);
            break;
            case Dir.Down:
                return new Coord(Position.X, Position.Y + 1);
            break;
            case Dir.Left:
                return new Coord(Position.X - 1, Position.Y);
            break;
            case Dir.Right:
                return new Coord(Position.X + 1, Position.Y);
            break;
            
        }
        return new Coord(Position.X, Position.Y);
    }

    internal void Turn()
    {
        var i = ((int)Dir + 1) % 4;
        Dir = (Dir)i;
    }
}

enum Dir
{
    Up = 0,
    Right = 1,
    Down = 2,
    Left = 3
}