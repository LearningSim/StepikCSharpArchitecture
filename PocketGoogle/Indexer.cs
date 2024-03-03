using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PocketGoogle;

public class Indexer : IIndexer
{
    private readonly Dictionary<string, HashSet<int>> index = new();

    /// <summary>
    /// it has structure like {docId: {word: pos}}
    /// </summary>
    private readonly Dictionary<int, Dictionary<string, List<int>>> positions = new();

    public void Add(int id, string documentText)
    {
        const string separators = " .,!?:-\r\n";
        var sb = new StringBuilder();
        for (var i = 0; i < documentText.Length; i++)
        {
            var symbol = documentText[i];
            if (separators.Contains(symbol)) continue;

            sb.Append(symbol);
            if (i + 1 == documentText.Length || separators.Contains(documentText[i + 1]))
            {
                var word = sb.ToString();
                AddWord(word, i + 1 - word.Length, id);
                sb.Clear();
            }
        }
    }

    private void AddWord(string word, int pos, int docId)
    {
        index.GetOrSetDefault(word, new()).Add(docId);
        positions.GetOrSetDefault(docId, new())
            .GetOrSetDefault(word, new())
            .Add(pos);
    }

    public List<int> GetIds(string word) => index.GetOrSetDefault(word, new()).ToList();

    public List<int> GetPositions(int id, string word) => positions
        .GetOrSetDefault(id, new())
        .GetOrSetDefault(word, new());

    public void Remove(int id)
    {
        foreach (var (word, ids) in index)
        {
            ids.Remove(id);
        }

        positions.Remove(id);
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