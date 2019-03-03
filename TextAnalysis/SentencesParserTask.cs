using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TextAnalysis
{
	static class SentencesParserTask
	{
		public static List<List<string>> ParseSentences(string text)
		{
			var rawSentences = text.ToLower().Split(".!?;:()".ToCharArray());
			var sentences = rawSentences.Select(ParseSentence);

			return sentences.Where(s => s.Count > 0).ToList();
		}

		private static List<string> ParseSentence(string text)
		{
			var sentence = new List<string>();
			var word = new StringBuilder();
			foreach (var l in text)
			{
				if (char.IsLetter(l) || l == '\'')
				{
					word.Append(l);
				}
				else if (word.Length > 0)
				{
					sentence.Add(word.ToString());
					word.Length = 0;
				}
			}

			if (word.Length > 0)
			{
				sentence.Add(word.ToString());
			}

			return sentence;
		}
	}
}