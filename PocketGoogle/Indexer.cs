using System.Collections.Generic;
using System.Linq;

namespace PocketGoogle;

public class Indexer : IIndexer
{
    /// <summary>
    /// it has a structure like {word: {docId: pos}}
    /// </summary>
    private readonly Dictionary<string, Dictionary<int, List<int>>> positions = new();

    public void Add(int id, string documentText)
    {
        var pos = 0;
        foreach (var word in documentText.Split(" .,!?:-\r\n".ToCharArray()))
        {
            if (word == string.Empty)
            {
                pos++;
                continue;
            }

            positions.GetOrSetDefault(word, new())
                .GetOrSetDefault(id, new())
                .Add(pos);
            pos += word.Length + 1;
        }
    }

    public List<int> GetIds(string word) => positions.GetOrSetDefault(word, new()).Keys.ToList();

    public List<int> GetPositions(int id, string word) => positions
        .GetOrSetDefault(word, new())
        .GetOrSetDefault(id, new());

    public void Remove(int id)
    {
        foreach (var (_, docs) in positions)
        {
            docs.Remove(id);
        }
    }
}

public static class DictionaryExtensions
{
    public static TVal GetOrSetDefault<TKey, TVal>(this Dictionary<TKey, TVal> dict, TKey key, TVal defVal)
        where TKey : notnull
    {
        dict.TryAdd(key, defVal);
        return dict[key];
    }
}