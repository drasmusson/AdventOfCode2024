var input = File.ReadAllLines("Input.txt");

var partOne = PartOne(input);
Console.WriteLine($"Part one: {partOne}");
int PartOne(string[] input)
{
    int splitIndex = input.ToList().FindIndex(str => string.IsNullOrEmpty(str));
    
    var mapInput = input.Take(splitIndex).ToArray();
    var instructions = string.Join("", input.Skip(splitIndex + 1).ToList());
    var map = ParseMap(mapInput);

    var robot = map.First(x => x.Type == '@');
    
    for (int i = 0; i < instructions.Length; i++)
    {
        // PrintToConsole(map, i);
        var instruction = instructions[i];
        MoveInDir(map, robot, instruction);
        // PrintToConsole(map, i);
        Console.WriteLine(i);
    }
    
    return map.Where(p => p.Type == 'O').Sum(o => CalcGpsCoord(o.Coord));
}

int CalcGpsCoord(Coord c) => (100 * c.Y) + c.X;

bool MoveInDir(List<Point> map, Point current, char direction)
{
    var coordInDir = current.Coord.CoordInDir(direction);
    var pointInDir = map.FirstOrDefault(x => x.Coord == coordInDir);

    if (pointInDir == null)
    {
        current.Coord = coordInDir;
        return true;
    }
    
    switch (pointInDir.Type)
    {
        case '#':
            return false;
        case '.':
            current.Coord = coordInDir;
            return true;
        case 'O':
            if (MoveInDir(map, pointInDir, direction))
            {
                current.Coord = coordInDir;
                return true;
            }
            return false;
        default:
            break;
    }

    return false;
}

void PrintToConsole(List<Point> points, int iteration)
{
    var coordSet = points.Select(r => r.Coord).ToList();
    var maxX = coordSet.Max(x => x.X);
    var maxY = coordSet.Max(x => x.Y);
    
    
    for (int y = 0; y <= maxY; y++)
    {
        for (int x = 0; x <= maxX; x++)
        {
            if (!points.Any(p => p.Coord == new Coord(x, y)))
                Console.Write(".");
            else
            {
                var c = points.First(p => p.Coord == new Coord(x, y)).Type;
                if (c == '.') c = points.Any(p => p.Coord == new Coord(x, y) && p.Type == 'O') ? 'O' : c;
                Console.Write(c);
            }
        }
        Console.WriteLine();
    }

    Console.WriteLine($"Iteration: {iteration}");
}

List<Point> ParseMap(string[] lines)
{
    var map = new List<Point>();

    for (int y = 0; y < lines.Length; y++)
    for (int x = 0; x < lines[y].Length; x++)
    {
        var c = lines[y][x];
        if (c == '.') continue;
        map.Add(new Point(new Coord(x, y), c));
    }
    
    return map;
}

class Point
{
    public Coord Coord { get; set; }
    public char Type { get; init; }

    public Point(Coord coord, char c)
    {
        Coord = coord;
        Type = c;
    }
}

record Coord(int X, int Y)
{
    Coord Right => new Coord(X: X + 1, Y);
    Coord Down => new Coord(X, Y + 1);
    Coord Left => new Coord(X - 1, Y);
    Coord Up => new Coord(X, Y - 1);

    public Coord CoordInDir(char dir) => dir switch
    {
        '>' => Right,
        'v' => Down,
        '<' => Left,
        '^' => Up,
        _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
    };
}