using System.Collections.Generic;
using System.Linq;

namespace yield
{
	public static class ExpSmoothingTask
	{
		public static IEnumerable<DataPoint> SmoothExponentialy(this IEnumerable<DataPoint> data, double alpha)
		{
			using (var e = data.GetEnumerator())
			{
				var hasAny = e.MoveNext();
				if (!hasAny)
				{
					yield break;
				}

				var prev = e.Current;
				prev.ExpSmoothedY = prev.OriginalY;
				yield return prev;
				while (e.MoveNext())
				{
					e.Current.ExpSmoothedY = prev.ExpSmoothedY + alpha * (e.Current.OriginalY - prev.ExpSmoothedY);
					prev = e.Current;
					yield return prev;
				}
			}
		}
	}
}