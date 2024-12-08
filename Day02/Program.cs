
var input  = System.IO.File.ReadAllLines("Input.txt");

var partOne = PartOne(input);
var partTwo = PartTwo(input);

Console.WriteLine(partOne);
Console.WriteLine(partTwo);

int PartOne(string[] input)
{
    var result = 0;
    foreach (var line in input)
    {
        var nums = line.Split(" ").Select(int.Parse).ToList();
        var asc = new List<int>(nums).Order();
        var desc = new List<int>(nums).OrderDescending();

        var valid = new List<bool>();
        
        if (nums.SequenceEqual(asc) || nums.SequenceEqual(desc))
        {
            for (int i = 0; i < nums.Count() - 1; i ++)
                valid.Add(IsClose(nums[i], nums[i + 1]));
            
            if (valid.All(x => x == true)) result++;
        }
    }
    return result;
}

int PartTwo(string[] input)
{
    var result = 0;
    foreach (var line in input)
    {
        var nums = line.Split(" ").Select(int.Parse).ToList();
        if (IsRowValid(nums))
        {
            result++;
            continue;
        }
        
    }
    return result;
}

bool IsRowValid(List<int> nums)
{
    if (IsSafe(nums))
    {
        return true;
    }

    for (int i = 0; i < nums.Count(); i++)
    {
        var curr = nums.Where((_, index) => i != index).ToList();
        if (IsSafe(curr))
        {
            return true;
        }
    }
    return false;
}

bool IsSafe(List<int> nums)
{
    var asc = new List<int>(nums).Order();
    var desc = new List<int>(nums).OrderDescending();

    var valid = new List<bool>();

    if (nums.SequenceEqual(asc) || nums.SequenceEqual(desc))
    {
        for (int i = 0; i < nums.Count() - 1; i++)
            valid.Add(IsClose(nums[i], nums[i + 1]));
        
        if (valid.All(x => x == true)) return true;
    }
    
    return false;
}

int OutOfOrderCount(List<int> original, List<int> compare)
{
    var matches = 0;
    for (int i = 0; i < original.Count; i++)
        if (original[i] == compare[i]) matches++;

    return matches;
}

bool IsClose(int x, int y) => Math.Abs(x - y) <= 3 && x != y;
bool IsAsc(int x, int y) => x < y;