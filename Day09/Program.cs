
var input = System.IO.File.ReadAllText("input.txt");

var partOne = PartOne(input);
Console.WriteLine($"Part one: {partOne}");
long PartOne(string inputString)
{
    var blocks = ParseBlocks(inputString);
    var end = blocks.Last().ExpandedIndex;
    foreach (var c in blocks)
    {
        if (AllEmptyBlocksInEnd(blocks.SelectMany(x => x.Values).ToArray())) break;
        
        if (!c.IsEmpty) continue;
        
        var v = blocks.Last(x => !x.IsEmpty);
        for (int j = 0; j < c.Values.Length; j++)
        {
            if (v.Values.All(x => x is null))
            {
                v = blocks.Last(x => !x.IsEmpty);
            }
            c.Values[j] = v.Values.Last(x => x is not null);
            ReplaceLastWhere(v.Values, x => x is not null, null);
        }
        
    }
    
    return CalculateChecksum(blocks);
}

long CalculateChecksum(Block[] blocks)
{
    var result = 0L;
    var i = 0L;
    foreach (var block in blocks)
    {
        if (block.IsEmpty) break;
        foreach (var value in block.Values)
        {
            if (value is null) break;
            result += i * (long)value;
            i++;
        }
    }
    return result;
}
void ReplaceLastWhere<T>(T[] array, Func<T, bool> predicate, T newValue)
{
    for (int i = array.Length - 1; i >= 0; i--)
    {
        if (predicate(array[i]))
        {
            array[i] = newValue;
            break;
        }
    }
}

bool AllEmptyBlocksInEnd(long?[] array)
{
    int totalCount = array.Count(x => x is null);
    if (totalCount == 0)
        return true;
    
    for (int i = array.Length - 1; i >= array.Length - totalCount; i--)
    {
        if (array[i] is not null)
            return false;
    }

    return true;
}

// bool AllEmptyBlocksInEnd(Block[] array)
// {
//     int totalCount = array.Count(x => x.IsEmpty);
//     if (totalCount == 0)
//         return true;
//     
//     for (int i = array.Length - 1; i >= array.Length - totalCount; i--)
//     {
//         if (!array[i].IsEmpty)
//             return false;
//     }
//
//     return true;
// }

Block[] ParseBlocks(string inputString)
{
    var blocks = new Block[inputString.Length];
    var expandedIndex = 0;
    for (int i = 0; i < inputString.Length; i++)
    {
        var length = int.Parse(inputString[i].ToString());
        
        var value = i / 2;
        if (i % 2 == 0)
        {
            blocks[i] = new Block(length, i, expandedIndex, value);
        }
        else
        {
            blocks[i] = new Block(length, i, expandedIndex,null);
        }
        expandedIndex += length;
    }
    return blocks;
}

class Block
{
    public int Length { get; set; }
    public int Index { get; set; }
    public int ExpandedIndex { get; set; }
    public long?[] Values { get; set; }
    public bool IsEmpty => Values.All(x => x == null);

    public Block(int length, int index, int expandedIndex, long? value)
    {
        Length = length;
        Index = index;
        ExpandedIndex = expandedIndex;
        Values = new long?[length];
        for (int i = 0; i < Values.Length; i++)
        {
            Values[i] = value;
        }
    }
}