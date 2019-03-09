using System;
using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
    static class TextGeneratorTask
    {
        public static string ContinuePhrase(Dictionary<string, string> nextWords, string phraseBeginning, int wordsCount)
        {
	        var words = phraseBeginning.Split(' ').ToList();
			for (int i = 0; i < wordsCount; i++)
			{
				var next = words.TakeLast(2).ToList().GetNext(nextWords);
				if (next == null)
				{
					break;
				}
				words.Add(next);
			}
            return string.Join(" ", words);
        }

	    private static string GetNext(this List<string> phraseBeginning, Dictionary<string, string> nextWords)
	    {
		    if (phraseBeginning.Count == 2)
		    {
			    var beginnig = string.Join(" ", phraseBeginning);
			    if (nextWords.ContainsKey(beginnig))
			    {
				    return nextWords[beginnig];
			    }

			    return nextWords.Get(phraseBeginning[1]);
		    }
		    if (phraseBeginning.Count == 1)
		    {
			    return nextWords.Get(phraseBeginning[0]);
		    }

		    return null;
	    }

	    private static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count) {
		    return source.Skip(Math.Max(0, source.Count() - count));
	    }

	    private static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key) {
		    if (source.ContainsKey(key))
		    {
			    return source[key];
		    }

		    return default(TValue);
	    }
	}
}