using System.Collections.Generic;
using System.Linq;

namespace TextAnalysis
{
	static class FrequencyAnalysisTask
	{
		public static Dictionary<string, string> GetMostFrequentNextWords(List<List<string>> text)
		{
			var pairs = text
				.SelectMany(s => s.GetPairs())
				.Where(pair => pair != null)
				.GetMostFrequentPairs();

			var triples = text
				.SelectMany(s => s.GetTriples())
				.Where(triple => triple != null)
				.Select(triple => new[] {$"{triple[0]} {triple[1]}", triple[2]})
				.GetMostFrequentPairs();

			var result = new Dictionary<string, string>();
			foreach (var pair in pairs)
			{
				result[pair.Key] = pair.Val;
			}

			foreach (var triple in triples)
			{
				result[triple.Key] = triple.Val;
			}

			return result;
		}

		private static IEnumerable<(string Key, string Val)> GetMostFrequentPairs(this IEnumerable<string[]> pairs)
		{
			return pairs
				.GroupBy(pair => pair[0])
				.Select(group =>
					(
						Key: group.Key,
						Val: group.GroupBy(pair => pair[1]).GetMostFrequentValue()
					)
				);
		}

		private static string GetMostFrequentValue(this IEnumerable<IGrouping<string, string[]>> groups)
		{
			var value = (Count: -1, Val: "");
			foreach (var group in groups)
			{
				if (group.Count() > value.Count)
				{
					value.Count = group.Count();
					value.Val = group.Key;
				}
				else if (group.Count() == value.Count && string.CompareOrdinal(group.Key, value.Val) < 0)
				{
					value.Count = group.Count();
					value.Val = group.Key;
				}
			}

			return value.Val;
		}

		private static string[][] GetPairs(this List<string> sentence)
		{
			if (sentence.Count < 2)
			{
				return new string[0][];
			}

			var pairs = new string[sentence.Count - 1][];
			for (int i = 0; i < pairs.Length; i++)
			{
				pairs[i] = new[] {sentence[i], sentence[i + 1]};
			}

			return pairs;
		}

		private static string[][] GetTriples(this List<string> sentence)
		{
			if (sentence.Count < 3)
			{
				return new string[0][];
			}

			var triples = new string[sentence.Count - 2][];
			for (int i = 0; i < triples.Length; i++)
			{
				triples[i] = new[] {sentence[i], sentence[i + 1], sentence[i + 2]};
			}

			return triples;
		}
	}
}