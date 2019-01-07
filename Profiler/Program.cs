using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPhotoshop;

namespace Profiler {
	class Program {
		static void Main(string[] args) {
			var simpleHandler = new SimpleParameterHandler<LighteningParameters>();
			var staticHandler = new StaticParameterHandler<LighteningParameters>();
			var expressionHandler = new ExpressionParameterHandler<LighteningParameters>();

			int n = 100000;
			Test(values => simpleHandler.CreateParameters(values), n);
			Test(values => staticHandler.CreateParameters(values), n);
			Test(values => expressionHandler.CreateParameters(values), n);
			Test(values => new LighteningParameters {Coefficient = values[0]}, n);
			Console.Read();
		}

		static void Test(Func<double[], LighteningParameters> method, int n) {
			var values = new double[] {0};
			method(values);
			var watch = new Stopwatch();
			watch.Start();
			for (int i = 0; i < n; i++) {
				method(values);
			}

			Console.WriteLine(1000 * (double) watch.ElapsedMilliseconds / n);
		}
	}
}