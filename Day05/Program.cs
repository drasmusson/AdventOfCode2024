var input = File.ReadAllLines("input.txt");

var orderingRules = GetOrderingRules(input);
var pagesList = GetPagesList(input);

var partOne = PartOne(orderingRules, pagesList);
Console.WriteLine($"Part one: {partOne}");
int PartOne(List<(int, int)> valueTuples, List<List<int>> list)
{
    return 0;
}

List<List<int>> GetPagesList(string[] strings)
{
    var pagesList = new List<List<int>>();
    var i = strings.ToList().FindIndex(s => s == string.Empty);
    for (int j = i + 1; j < strings.Length; j++)
    {
        var pages = new List<int>();
        strings[j].Split(',').ToList().ForEach(p => pages.Add(int.Parse(p)));
        pagesList.Add(pages);
    }
    return pagesList;
}

List<(int, int)> GetOrderingRules(string[] strings)
{
    var orderingRules = new List<(int, int)>();

    foreach (var line in strings)
    {
        if (string.IsNullOrWhiteSpace(line)) break;
        var parts = line.Split("|");
        orderingRules.Add((int.Parse(parts[0]), int.Parse(parts[1])));
    }
    return orderingRules;
}