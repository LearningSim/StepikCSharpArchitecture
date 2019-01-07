using System;
using System.Collections.Generic;
using System.Linq;

namespace Delegates.PairsAnalysis {
	public static class Analysis {
		public static int FindMaxPeriodIndex(params DateTime[] data) {
			return data.Pairs().Select(p => (p.Item2 - p.Item1).TotalSeconds).MaxIndex();
		}

		public static double FindAverageRelativeDifference(params double[] data) {
			return data.Pairs().Select(p => (p.Item2 - p.Item1) / p.Item1).Average();
		}

		public static IEnumerable<Tuple<T, T>> Pairs<T>(this IEnumerable<T> data) {
			T last = default(T);
			bool ignore = true;
			foreach (var el in data) {
				if (!ignore) {
					yield return Tuple.Create(last, el);
				}

				ignore = false;
				last = el;
			}
		}

		public static int MaxIndex<T>(this IEnumerable<T> data) where T : IComparable<T> {
			var elements = data.ToList();
			if (!elements.Any()) {
				throw new ArgumentException();
			}

			return elements.Select((v, i) => Tuple.Create(v, i)).OrderByDescending(t => t.Item1).First().Item2;
		}
	}
}