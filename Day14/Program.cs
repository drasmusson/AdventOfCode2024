var input = File.ReadAllLines("Input.txt");
var robots = ParseRobots(input);
var partOne = PartOne(input);
Console.WriteLine($"Part One: {partOne}");
var partTwo = PartTwo(input);
Console.WriteLine($"Part Two: {partTwo}");
int PartOne(string[] input)
{
    var robots = ParseRobots(input);
    var maxX = 100;
    var maxY = 102;

    for (int i = 0; i < 100; i++)
    {
        foreach (var robot in robots)
            Blink(robot, maxX, maxY);
    }

    var result = 1;
    for (int i = 1; i <= 4; i++)
        result *= CountRobotsInQuadrant(i, robots, maxX, maxY);
    
    return result;
}

int PartTwo(string[] input)
{
    var robots = ParseRobots(input);
    var maxX = 100;
    var maxY = 102;

    var robotCount = robots.Count;
    var positions = robots.Select(r => r.Position).ToHashSet();
    var i = 0;
    
    while (robotCount != positions.Count)
    {
        i++;
        foreach (var robot in robots)
            Blink(robot, maxX, maxY);
        
        positions = robots.Select(r => r.Position).ToHashSet();
        if (positions.Count == robotCount)
        {
            PrintToConsole(robots, maxX, maxY, i);
            return i;
        }
    }

    return i;
}

void PrintToConsole(List<Robot> robots, int maxX, int maxY, int iteration)
{
    var coordSet = robots.Select(r => r.Position).ToList();

    var coordChar = '#';
    var empty = '.';
    
    for (int y = 0; y <= maxY; y++)
    {
        for (int x = 0; x <= maxX; x++)
        {
            if (coordSet.Contains(new Coord(x, y)))
                Console.Write(coordChar);
            else
                Console.Write(empty);
        }
        Console.WriteLine();
    }

    Console.WriteLine($"Iteration: {iteration}");
}

int CountRobotsInQuadrant(int quadrant, List<Robot> robots, int maxX, int maxY)
{
    if (quadrant == 1)
    {
        return robots.Count(r => r.Position.X < maxX / 2 && r.Position.Y < maxY / 2);
    }

    if (quadrant == 2)
    {
        return robots.Count(r => r.Position.X > maxX / 2 && r.Position.Y < maxY / 2);
    }
    
    if (quadrant == 3)
    {
        return robots.Count(r => r.Position.X < maxX / 2 && r.Position.Y > maxY / 2);
    }
    
    if (quadrant == 4)
    {
        return robots.Count(r => r.Position.X > maxX / 2 && r.Position.Y > maxY / 2);
    }

    return 0;
}

void Blink(Robot robot, int maxX, int maxY)
{
    var newX = Wrap(robot.Position.X + robot.Velocity.X, maxX + 1);
    var newY = Wrap(robot.Position.Y + robot.Velocity.Y, maxY + 1);
    
    robot.Position = new Coord(newX, newY);
}

int Wrap(int x, int max) => ((x % max) + max) % max;

List<Robot> ParseRobots(string[] input)
{
    var robots = new List<Robot>();

    foreach (var line in input)
    {
        var pV = line.Split(" ");
        var pC = pV[0].Split("=")[1];
        var pX = int.Parse(pC.Split(",")[0]);
        var pY = int.Parse(pC.Split(",")[1]);
        
        var vC = pV[1].Split("=")[1];
        var vX = int.Parse(vC.Split(",")[0]);
        var vY = int.Parse(vC.Split(",")[1]);
        var robot = new Robot
        {
            Position = new Coord(pX, pY),
            Velocity = new Coord(vX, vY)
        };
        robots.Add(robot);
    }
    
    return robots;
}

class Robot
{
    public Coord Position { get; set; }
    public Coord Velocity { get; init; }
}

record Coord(int X, int Y);