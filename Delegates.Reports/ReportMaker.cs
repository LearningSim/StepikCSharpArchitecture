using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Reports {
	public abstract class ReportMaker {
		protected abstract string MakeCaption(string caption);
		protected abstract string BeginList();
		protected abstract string MakeItem(string valueType, string entry);
		protected abstract string EndList();

		public string MakeReport(string caption, IEnumerable<Measurement> measurements,
			Func<IEnumerable<double>, object> makeStatistics) {
			var data = measurements.ToList();
			var result = new StringBuilder();
			result.Append(MakeCaption(caption));
			result.Append(BeginList());
			result.Append(MakeItem("Temperature", makeStatistics(data.Select(z => z.Temperature)).ToString()));
			result.Append(MakeItem("Humidity", makeStatistics(data.Select(z => z.Humidity)).ToString()));
			result.Append(EndList());
			return result.ToString();
		}
	}

	public class HtmlReportMaker : ReportMaker {
		protected override string MakeCaption(string caption) => $"<h1>{caption}</h1>";
		protected override string BeginList() => "<ul>";
		protected override string EndList() => "</ul>";

		protected override string MakeItem(string valueType, string entry) {
			return $"<li><b>{valueType}</b>: {entry}";
		}
	}

	public class MarkdownReportMaker : ReportMaker {
		protected override string BeginList() => "";
		protected override string EndList() => "";
		protected override string MakeCaption(string caption) => $"## {caption}\n\n";

		protected override string MakeItem(string valueType, string entry) {
			return $" * **{valueType}**: {entry}\n\n";
		}
	}

	public static class ReportMakerHelper {
		private static object MakeMeanAndStd(IEnumerable<double> _data) {
			var data = _data.ToList();
			var mean = data.Average();
			var std = Math.Sqrt(data.Select(z => Math.Pow(z - mean, 2)).Sum() / (data.Count - 1));

			return new MeanAndStd {
				Mean = mean,
				Std = std
			};
		}

		private static object MakeMedian(IEnumerable<double> _data) {
			var list = _data.OrderBy(z => z).ToList();
			if (list.Count % 2 == 0)
				return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;
			else
				return list[list.Count / 2];
		}

		public static string MeanAndStdHtmlReport(IEnumerable<Measurement> data) {
			return new HtmlReportMaker().MakeReport("Mean and Std", data, MakeMeanAndStd);
		}

		public static string MedianMarkdownReport(IEnumerable<Measurement> data) {
			return new MarkdownReportMaker().MakeReport("Median", data, MakeMedian);
		}

		public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> data) {
			return new MarkdownReportMaker().MakeReport("Mean and Std", data, MakeMeanAndStd);
		}

		public static string MedianHtmlReport(IEnumerable<Measurement> data) {
			return new HtmlReportMaker().MakeReport("Median", data, MakeMedian);
		}
	}
}