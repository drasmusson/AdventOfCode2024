
var input = File.ReadAllLines("Input.txt");

var partOne = PartOne(input);
Console.WriteLine(partOne);
var partTwo = PartTwo(input);
Console.WriteLine(partTwo);
int PartOne(string[] input)
{
    var row1 = new List<int>();
    var row2 = new List<int>();

    foreach (var line in input)
    {
        var pair = line.Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
        row1.Add(int.Parse(pair[0]));
        row2.Add(int.Parse(pair[1]));
    }

    row1.Sort();
    row2.Sort();

    var partOne = 0;
    for (var i = 0; i < row1.Count; i++)
    {
        partOne += Math.Abs(row1[i] - row2[i]);
    }

    return partOne;
}

int PartTwo(string[] input)
{
    var row1 = new List<int>();
    var row2 = new List<int>();

    foreach (var line in input)
    {
        var pair = line.Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
        row1.Add(int.Parse(pair[0]));
        row2.Add(int.Parse(pair[1]));
    }

    var groups = row2.GroupBy(x => x).ToList();
    var partTwo = 0;
    for (var i = 0; i < row1.Count; i++)
    {
        var value = row1[i];
        if (groups.Any(x => x.Key == value))
            partTwo += value * groups.First(x => x.Key == value).Count();
    }

    return partTwo;
}