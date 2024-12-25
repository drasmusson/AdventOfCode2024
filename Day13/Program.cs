var input = File.ReadAllLines("Input.txt");
var partOne = PartOne(input);
Console.WriteLine($"Part One: {partOne}");

var partTwo = PartTwo(input);
Console.WriteLine($"Part Two: {partTwo}");
int PartOne(string[] input)
{
    var result = 0;

    var configs = ParseInput(input);

    foreach (var config in configs)
    {
        var cost = SearchForCheapest(config);

        if (cost is not null) result += cost.Value;
    }

    return result;
}

long PartTwo(string[] input)
{
    var result = 0L;

    var configs = ParseInput(input);

    foreach (var config in configs)
    {
        var cost = SearchForCheapest2(config);

        result += cost;
    }

    return result;
}

long SearchForCheapest2(ClawMachineConfig config)
{
        long aX = config.ButtonA.X;
        long aY = config.ButtonA.Y;
        long bX = config.ButtonB.X;
        long bY = config.ButtonB.Y;
        
        var prizeX = config.Prize.X + 10000000000000;
        var prizeY = config.Prize.Y + 10000000000000;

        var leftSide = (aX * bY) - (aY * bX);
        var rightSide = (prizeX * bY) - (prizeY * bX);
        
        if (rightSide % leftSide != 0)
            return 0;
            
        var buttonAPresses = rightSide / leftSide;
        
        var remainingDistance = prizeX - (aX * buttonAPresses);
        
        if (remainingDistance % bX != 0)
            return 0;
            
        var buttonBPresses = remainingDistance / bX;
        
        if (buttonAPresses < 0 || buttonBPresses < 0)
            return 0;
        
        return (buttonAPresses * 3) + buttonBPresses;
}

int? SearchForCheapest(ClawMachineConfig config)
{
    var costs = new List<int>();
    
    var maxA = 100;
    var maxB = 100;
    
    for (int a = 0; a <= maxA; a++)
    {
        for (int b = 0; b <= maxB; b++)
        {
            int x = a * config.ButtonA.X + b * config.ButtonB.X;
            int y = a * config.ButtonA.Y + b * config.ButtonB.Y;

            if (x == config.Prize.X && y == config.Prize.Y)
            {
                int cost = (3 * a) + b;
                costs.Add(cost);
            }
        }
    }

    if (costs.Count == 0) return null;
    
    return costs.Min();
}

List<ClawMachineConfig> ParseInput(string[] strings)
{
    var result = new List<ClawMachineConfig>();
    var config = new ClawMachineConfig();
    for (int i = 0; i < strings.Length; i++)
    {
        var d = i % 4;

        if (d == 0)
        {
            var x = int.Parse(strings[i].Split("X+")[1].Split(",")[0]);
            var y = int.Parse(strings[i].Split("Y+")[1]);

            config.ButtonA = new Coord(x, y);
        }
        if (d == 1)
        {
            var x = int.Parse(strings[i].Split("X+")[1].Split(",")[0]);
            var y = int.Parse(strings[i].Split("Y+")[1]);

            config.ButtonB = new Coord(x, y);
        }
        if (d == 2)
        {
            var x = int.Parse(strings[i].Split("X=")[1].Split(",")[0]);
            var y = int.Parse(strings[i].Split("Y=")[1]);

            config.Prize = new Coord(x, y);
        }

        if (d == 3)
        {
            result.Add(config);
            config = new ClawMachineConfig();
        }
    }
    result.Add(config);
    return result;
}
class ClawMachineConfig
{
    public Coord ButtonA { get; set; }
    public Coord ButtonB { get; set; }
    public Coord Prize { get; set; }

}

record Coord(int X, int Y)
{
    public static Coord operator +(Coord a, Coord b) => new(a.X + b.X, a.Y + b.Y);
}