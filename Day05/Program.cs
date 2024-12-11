using System.Runtime.CompilerServices;

var input = File.ReadAllLines("input.txt");

var orderingRules = GetOrderingRules(input);
var pagesList = GetPagesList(input);

var partOne = PartOne(orderingRules, pagesList);
Console.WriteLine($"Part one: {partOne}");

var partTwo = PartTwo(orderingRules, pagesList);
Console.WriteLine($"Part two: {partTwo}");

int PartTwo(List<(int, int)> orderingRules, List<List<int>> pagesList)
{
    var result = 0;
    foreach (var pages in pagesList)
    {
        var matchingOrderingRules = GetMatchingOrderingRules(orderingRules, pages);
        if (!PagesAreInOrder(pages, matchingOrderingRules))
        {
            var sortedList = SortPages(pages, matchingOrderingRules);
            result += sortedList[sortedList.Count / 2];
        }
    }
    return result;
}

List<int> SortPages(List<int> pages, List<(int, int)> orderingRules)
{
    var sortedList = new int[pages.Count];

    foreach (var page in pages)
    {
        var i = pages.Count() - orderingRules.Count(x => x.Item1 == page) - 1;
        sortedList[i] = page;
    }
    return sortedList.ToList();
}

int PartOne(List<(int, int)> orderingRules, List<List<int>> pagesList)
{
    var result = 0;
    foreach (var pages in pagesList)
    {
        var matchingOrderingRules = GetMatchingOrderingRules(orderingRules, pages);
        if (PagesAreInOrder(pages, matchingOrderingRules)) result += pages[pages.Count / 2];
    }
    return result;
}
bool PagesAreInOrder(List<int> pages, List<(int, int)> orderingRules)
{
    foreach (var orderingRule in orderingRules)
    {
        if (!(pages.IndexOf(orderingRule.Item1) < pages.IndexOf(orderingRule.Item2))) return false;
    }

    return true;
}

List<(int, int)> GetMatchingOrderingRules(List<(int, int)> orderingRules, List<int> pages)
{
    var matchingOrderingRules = new List<(int, int)>();

    foreach (var orderingRule in orderingRules)
    {
        if (pages.Contains(orderingRule.Item1) && pages.Contains(orderingRule.Item2))
            matchingOrderingRules.Add(orderingRule);
    }
    return matchingOrderingRules;
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